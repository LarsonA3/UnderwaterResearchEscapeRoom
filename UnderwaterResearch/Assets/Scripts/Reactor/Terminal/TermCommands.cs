using System.Collections.Generic;
using System.Linq;

public class TermCommands
{
    private readonly TermFileSystem fs;
    private readonly TermDisplay display;

    private const string ReactorPassword = "DELTA-7742";

    private static readonly string[] CommandNames =
        { "help", "ls", "cd", "cat", "pwd", "clear", "whoami", "date", "echo", "reactor-restart" };

    public bool PuzzleSolved { get; private set; }

    public TermCommands(TermFileSystem fs, TermDisplay display) {
        this.fs = fs;
        this.display = display;
    }

    public bool Execute(string raw) {
        if (string.IsNullOrEmpty(raw)) return false;

        var parts = raw.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        string cmd = parts[0].ToLowerInvariant();
        string arg = parts.Length > 1 ? parts[1] : "";

        switch (cmd) {
            case "help":    CmdHelp(); break;
            case "ls":      CmdLs(arg); break;
            case "cd":      CmdCd(arg); break;
            case "cat":     CmdCat(arg); break;
            case "pwd":     display.Println(fs.CurrentPath); break;
            case "clear":   display.Clear(); break;
            case "whoami":  display.Println("operator"); break;
            case "date":    display.Println("Thu Mar 12 03:22:41 UTC 2026"); break;
            case "echo":    display.Println(raw.Length > 5 ? raw.Substring(5) : ""); break;
            case "reactor-restart": CmdReactorRestart(arg); break;
            default:
                display.Println($"bash: {cmd}: command not found");
                break;
        }

        return PuzzleSolved;
    }

    public string TabComplete(string inputBuffer) {
        string line = inputBuffer;
        var parts = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0) return inputBuffer;

        if (parts.Length == 1 && !line.EndsWith(" ")) {
            string partial = parts[0].ToLowerInvariant();
            var matches = CommandNames.Where(c => c.StartsWith(partial)).ToList();
            if (matches.Count == 1) return matches[0] + " ";
            return inputBuffer;
        }

        string arg = parts.Length > 1 ? parts[parts.Length - 1] : "";
        if (line.EndsWith(" ")) arg = "";

        string dirPart = "";
        string namePart = arg;
        int lastSlash = arg.LastIndexOf('/');
        if (lastSlash >= 0) {
            dirPart = arg.Substring(0, lastSlash + 1);
            namePart = arg.Substring(lastSlash + 1);
        }

        string searchDir = fs.ResolvePath(string.IsNullOrEmpty(dirPart) ? "." : dirPart);
        var candidates = fs.GetCompletions(searchDir, namePart);

        if (candidates.Count == 1) {
            return parts[0] + " " + dirPart + candidates[0];
        } else if (candidates.Count > 1) {
            display.Println(GetPromptString() + inputBuffer);
            display.Println(string.Join("  ", candidates));
        }

        return inputBuffer;
    }

    public string GetPromptString() {
        string path = fs.CurrentPath;
        if (path.StartsWith(fs.HomePath))
            path = "~" + path.Substring(fs.HomePath.Length);
        return $"operator@aquarius:{path}$ ";
    }

    private void CmdHelp() {
        display.Println("Available commands:");
        display.Println("  ls [dir]       List directory contents");
        display.Println("  cd <dir>       Change directory");
        display.Println("  cat <file>     Display file contents");
        display.Println("  pwd            Print working directory");
        display.Println("  clear          Clear screen");
        display.Println("  whoami         Current user");
        display.Println("  date           Show date");
        display.Println("  help           This message");
    }

    private void CmdLs(string arg) {
        string target = fs.ResolvePath(string.IsNullOrEmpty(arg) ? "." : arg);

        if (!fs.DirectoryExists(target)) {
            display.Println($"ls: cannot access '{arg}': No such file or directory");
            return;
        }

        var children = fs.ListDirectory(target);
        if (children.Count == 0)
            display.Println("  (empty)");
        else
            display.Println(string.Join("  ", children));
    }

    private void CmdCd(string arg) {
        if (string.IsNullOrEmpty(arg) || arg == "~") {
            fs.CurrentPath = fs.HomePath;
            return;
        }

        string target = fs.ResolvePath(arg);
        if (fs.DirectoryExists(target))
            fs.CurrentPath = target;
        else
            display.Println($"bash: cd: {arg}: No such directory");
    }

    private void CmdCat(string arg) {
        if (string.IsNullOrEmpty(arg)) {
            display.Println("cat: missing operand");
            return;
        }

        string target = fs.ResolvePath(arg);
        if (fs.TryReadFile(target, out string contents)) {
            foreach (string line in contents.Split('\n'))
                display.Println(line);
        } else {
            display.Println($"cat: {arg}: No such file or directory");
        }
    }

    private void CmdReactorRestart(string password) {
        if (string.IsNullOrEmpty(password)) {
            display.Println("Usage: reactor-restart <auth-code>");
            display.Println("Hint: check system logs for the restart procedure.");
            return;
        }
        if (password == ReactorPassword) {
            display.Println("");
            display.Println("Reactor restart sequence initiated...");
            display.Println("████████████████████████████ 100%");
            display.Println("REACTOR ONLINE. Core temp stabilizing.");
            display.Println("");
            display.Println("Good work, operator.");
            PuzzleSolved = true;
        } else {
            display.Println("reactor-restart: authentication failed (invalid code)");
        }
    }
}
