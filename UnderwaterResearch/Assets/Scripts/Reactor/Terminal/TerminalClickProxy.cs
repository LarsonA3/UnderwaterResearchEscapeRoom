using UnityEngine;

public class TerminalClickProxy : MonoBehaviour
{
    private Terminal terminal;

    public void SetTerminal(Terminal t) => terminal = t;

    public void Grab() {
        if (terminal != null) terminal.Grab();
    }
}
