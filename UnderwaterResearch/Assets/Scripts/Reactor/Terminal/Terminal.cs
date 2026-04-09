using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Terminal : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text outputText;
    [SerializeField] private ReactorCore reactorCore;

    [Header("Puzzle")]
    [SerializeField] private GameObject[] activateOnWin;
    [SerializeField] private GameObject[] deactivateOnWin;

    private TermFileSystem fs;
    private TermDisplay display;
    private TermCommands commands;

    private StateMachine sm;
    private bool terminalActive = true;
    private bool isFocused;

    private Keyboard kb;

    private FirstPersonController playerController;
    private PlayerInput playerInput;
    private GameObject crosshairObject;
    private QuitToMenu quitToMenu;

    void Awake() {
        kb = Keyboard.current;
        if (kb == null) Debug.LogError("Terminal: No keyboard found!");
    }

    void Start() {
        if (outputText == null) outputText = GetComponentInChildren<TMP_Text>();

        AttachClickProxy();

        fs       = TermFileSystem.CreateDefault();
        display  = new TermDisplay(outputText);
        commands = new TermCommands(fs, display);

        var bootState = new State("Boot",
            onEnter:  Boot,
            onUpdate: (_) => { if (kb != null && kb.anyKey.wasPressedThisFrame) sm.SetState("Input"); },
            onExit:   () => { }
        );

        var inputState = new State("Input",
            onEnter:  () => { },
            onUpdate: (_) => HandleTyping(),
            onExit:   () => { }
        );

        var solvedState = new State("Solved",
            onEnter:  OnPuzzleSolved,
            onUpdate: (_) => { },
            onExit:   () => { }
        );

        inputState.AddTransition(new Transition("Solved", () => commands.PuzzleSolved));

        sm = new StateMachine();
        sm.AddState(bootState);
        sm.AddState(inputState);
        sm.AddState(solvedState);
        sm.SetState("Boot");
    }

    void Update() {
        if (!terminalActive) return;

        if (isFocused && kb != null && kb.qKey.wasPressedThisFrame) {
            ReleaseFocus();
            return;
        }

        if (display != null && commands != null)
            display.Refresh(commands.GetPromptString(), !commands.PuzzleSolved);

        if (!isFocused) return;

        sm.Update(Time.deltaTime);
    }

    private void Boot() {
        display.Clear();
        display.Println("AQUARIUS DEEP-SEA RESEARCH STATION");
        display.Println("Reactor Control Terminal v3.1.7");
        display.Println("========================================");
        display.Println("");
        display.Println("WARNING: Reactor offline. Core temp dropping.");
        display.Println("Backup power remaining: ██░░░░░░░░ 18%");
        display.Println("");
        display.Println("Login: operator (auto)");
        display.Println("Last login: Thu Mar 12 03:14:07 2026");
        display.Println("");
        display.Println("Type 'help' for available commands.");
        display.Println("Press [TAB] to autocomplete.");
        display.Println("");
        display.Refresh(commands.GetPromptString(), true);
    }

    private void HandleTyping() {
        if (kb.backspaceKey.wasPressedThisFrame) {
            string buf = display.InputBuffer;
            if (buf.Length > 0) display.InputBuffer = buf.Substring(0, buf.Length - 1);
        }

        if (kb.tabKey.wasPressedThisFrame)
            display.InputBuffer = commands.TabComplete(display.InputBuffer);

        if (kb.enterKey.wasPressedThisFrame || kb.numpadEnterKey.wasPressedThisFrame) {
            string cmd = display.InputBuffer.Trim();
            display.Println(commands.GetPromptString() + display.InputBuffer);
            display.InputBuffer = "";
            commands.Execute(cmd);
        }

        display.Refresh(commands.GetPromptString(), !commands.PuzzleSolved);
    }

    private void OnEnable() {
        if (kb == null) kb = Keyboard.current;
        if (kb != null) kb.onTextInput += OnTextInput;
    }

    private void OnDisable() { if (kb != null) kb.onTextInput -= OnTextInput; }

    private void OnTextInput(char c) {
        if (!isFocused) return;
        if (c >= ' ' && c != '\t')
            display.InputBuffer += c;
    }

    private void OnPuzzleSolved() {
        display.Refresh(commands.GetPromptString(), false);

        if (activateOnWin != null)
            foreach (var go in activateOnWin)
                if (go != null) go.SetActive(true);

        if (deactivateOnWin != null)
            foreach (var go in deactivateOnWin)
                if (go != null) go.SetActive(false);

        if (reactorCore != null) {
            reactorCore.SetCoreBlue();
            reactorCore.SetLightColor(Color.cyan);
        }
    }

    public void Grab() {
        if (isFocused || commands.PuzzleSolved) return;

        CachePlayerRefs();
        isFocused = true;

        if (playerController != null) playerController.enabled = false;
        if (playerInput != null)      playerInput.enabled = false;
        if (crosshairObject != null)  crosshairObject.SetActive(false);
        if (quitToMenu != null)       quitToMenu.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        display.Refresh(commands.GetPromptString(), !commands.PuzzleSolved);
    }

    private void ReleaseFocus() {
        isFocused = false;

        if (playerController != null) playerController.enabled = true;
        if (playerInput != null)      playerInput.enabled = true;
        if (crosshairObject != null)  crosshairObject.SetActive(true);
        if (quitToMenu != null)       quitToMenu.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CachePlayerRefs() {
        if (playerController != null) return;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            playerController = player.GetComponent<FirstPersonController>();
            playerInput = player.GetComponent<PlayerInput>();
        }

        if (crosshairObject == null) {
            var cc = FindFirstObjectByType<CustomCursor>();
            if (cc != null) crosshairObject = cc.gameObject;
        }

        if (quitToMenu == null) quitToMenu = FindFirstObjectByType<QuitToMenu>();
    }

    public void SetActive(bool active) => terminalActive = active;
    public bool IsSolved() => commands != null && commands.PuzzleSolved;
    public bool IsFocused() => isFocused;

    private void AttachClickProxy() {
        if (GetComponent<Collider>() != null && CompareTag("Clickable"))
            return;

        foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
            if (child == transform) continue;
            if (TryAttachProxy(child.gameObject)) return;
        }

        Transform t = transform.parent;
        while (t != null) {
            if (TryAttachProxy(t.gameObject)) return;

            foreach (Transform sibling in t) {
                if (sibling == transform) continue;
                if (TryAttachProxy(sibling.gameObject)) return;
                foreach (Transform grandchild in sibling)
                    if (TryAttachProxy(grandchild.gameObject)) return;
            }

            t = t.parent;
        }

        foreach (var col in FindObjectsByType<Collider>(FindObjectsSortMode.None)) {
            if (col.CompareTag("Clickable") && col.gameObject.name.ToLower().Contains("terminal")) {
                if (TryAttachProxy(col.gameObject)) return;
            }
        }
    }

    private bool TryAttachProxy(GameObject go)
    {
        if (go.GetComponent<Collider>() != null && go.CompareTag("Clickable")) {
            var existing = go.GetComponent<TerminalClickProxy>();
            if (existing == null)
                existing = go.AddComponent<TerminalClickProxy>();
            existing.SetTerminal(this);
            return true;
        }
        return false;
    }
}
