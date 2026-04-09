using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerminalCodePad : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private string correctCode = "7392";
	[SerializeField] private int maxDigits = 4;

	[Header("UI References")]
	[SerializeField] private TextMeshProUGUI displayText;
	[SerializeField] private Image[] underlineSlots;

	[Header("Colors")]
	[SerializeField] private Color defaultUnderlineColor = Color.white;
	[SerializeField] private Color filledUnderlineColor = new Color(0f, 1f, 0.12f);

	private string currentInput = "";
	private Color originalDisplayColor;

	private void Start()
	{
		originalDisplayColor = new Color(0f, 1f, 0.12f);
		UpdateDisplay();
	}

	private void OnEnable()
	{
		if (displayText != null)
		{
			displayText.gameObject.SetActive(true);
			currentInput = "";
			displayText.color = new Color(0f, 1f, 0.12f);
			displayText.fontSize = 200;
			UpdateDisplay();
		}
	}

	public void OnNumberPressed(string digit)
	{
		if (currentInput.Length >= maxDigits) return;
		currentInput += digit;
		UpdateDisplay();
	}

	public void OnBackspacePressed()
	{
		if (currentInput.Length == 0) return;
		currentInput = currentInput.Substring(0, currentInput.Length - 1);
		UpdateDisplay();
	}

	public void OnClearPressed()
	{
		currentInput = "";
		displayText.color = originalDisplayColor;
		UpdateDisplay();
	}

	public void OnSubmitPressed()
	{
		if (currentInput == correctCode)
			OnCorrectCode();
		else
			StartCoroutine(WrongCodeFlash());
	}

	public void ClosePanel()
	{
		displayText.gameObject.SetActive(false);
		gameObject.SetActive(false);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void UpdateDisplay()
	{
		if (displayText == null) return;

		string[] slots = new string[maxDigits];
		for (int i = 0; i < maxDigits; i++)
			slots[i] = i < currentInput.Length ? currentInput[i].ToString() : "_";

		displayText.text = string.Join(" ", slots);

		for (int i = 0; i < underlineSlots.Length; i++)
			underlineSlots[i].color = i < currentInput.Length
				? filledUnderlineColor
				: defaultUnderlineColor;

		if (currentInput.Length == maxDigits)
			OnSubmitPressed();
	}

	private void OnCorrectCode()
	{
		displayText.text = "CORRECT";
		displayText.color = new Color(0f, 1f, 0.12f);
		displayText.fontSize = 130;
		Debug.Log("Code accepted! Trigger unlock.");
		StartCoroutine(CloseAfterDelay());
	}

	private System.Collections.IEnumerator CloseAfterDelay()
	{
		yield return new WaitForSeconds(1.5f);
		displayText.gameObject.SetActive(false);
		ClosePanel();
	}

	private System.Collections.IEnumerator WrongCodeFlash()
	{
		displayText.text = "DENIED";
		displayText.color = Color.red;
		displayText.fontSize = 165;
		yield return new WaitForSeconds(2f);
		currentInput = "";
		displayText.color = new Color(0f, 1f, 0.12f);
		displayText.fontSize = 200;
		UpdateDisplay();
	}
}