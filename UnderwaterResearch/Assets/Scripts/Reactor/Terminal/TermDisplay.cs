using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TermDisplay
{
    private readonly List<string> lines = new();
    private readonly int maxLines;
    private readonly TMP_Text output;

    public string InputBuffer { get; set; } = "";

    public TermDisplay(TMP_Text output, int maxLines = 19) {
        this.output = output;
        this.maxLines = maxLines;
    }

    public void Println(string text) {
        lines.Add(text);
        while (lines.Count > maxLines)
            lines.RemoveAt(0);
    }

    public void Clear() => lines.Clear();

    public void Refresh(string prompt, bool showPrompt) {
        if (output == null) return;

        var sb = new StringBuilder();

        foreach (var line in lines)
            sb.AppendLine(line);

        if (showPrompt) {
            float blink = Mathf.Repeat(Time.time * 2f, 1f);
            string cursor = blink < 0.5f ? "█" : " ";
            sb.Append(prompt);
            sb.Append(InputBuffer);
            sb.Append(cursor);
        }

        output.text = sb.ToString();
    }
}
