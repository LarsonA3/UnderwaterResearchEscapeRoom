using System.Collections.Generic;

public class TermFileSystem
{
    public string CurrentPath { get; set; } = "/home/operator";
    public string HomePath => "/home/operator";

    private readonly Dictionary<string, string> files = new();
    private readonly HashSet<string> directories = new();

    public void AddDirectory(string path) => directories.Add(path);
    public void AddFile(string path, string contents) => files[path] = contents;

    public bool DirectoryExists(string path) => directories.Contains(path);
    public bool FileExists(string path) => files.ContainsKey(path);

    public bool TryReadFile(string path, out string contents) =>
        files.TryGetValue(path, out contents);

    public List<string> ListDirectory(string dirPath) {
        string prefix = dirPath == "/" ? "/" : dirPath + "/";
        var children = new SortedSet<string>();

        foreach (var d in directories) {
            if (d == dirPath) continue;
            if (!d.StartsWith(prefix)) continue;
            string remainder = d.Substring(prefix.Length);
            if (!remainder.Contains('/'))
                children.Add(remainder + "/");
        }
        foreach (var f in files.Keys) {
            if (!f.StartsWith(prefix)) continue;
            string remainder = f.Substring(prefix.Length);
            if (!remainder.Contains('/'))
                children.Add(remainder);
        }

        return new List<string>(children);
    }

    public List<string> GetCompletions(string dirPath, string partialName) {
        string prefix = dirPath == "/" ? "/" : dirPath + "/";
        var candidates = new List<string>();

        foreach (var d in directories) {
            if (d == dirPath) continue;
            if (!d.StartsWith(prefix)) continue;
            string remainder = d.Substring(prefix.Length);
            if (!remainder.Contains('/') && remainder.StartsWith(partialName))
                candidates.Add(remainder + "/");
        }
        foreach (var f in files.Keys) {
            if (!f.StartsWith(prefix)) continue;
            string remainder = f.Substring(prefix.Length);
            if (!remainder.Contains('/') && remainder.StartsWith(partialName))
                candidates.Add(remainder);
        }

        return candidates;
    }

    public string ResolvePath(string input) {
        if (input == "~") return HomePath;
        if (input.StartsWith("~/")) input = HomePath + input.Substring(1);

        string working = input.StartsWith("/") ? input : CurrentPath + "/" + input;

        var parts = working.Split('/', System.StringSplitOptions.RemoveEmptyEntries);
        var stack = new List<string>();
        foreach (var p in parts) {
            if (p == ".") continue;
            if (p == "..") {
                if (stack.Count > 0) stack.RemoveAt(stack.Count - 1);
                continue;
            }
            stack.Add(p);
        }
        return "/" + string.Join("/", stack);
    }

    public static TermFileSystem CreateDefault() {
        var fs = new TermFileSystem();

        fs.AddDirectory("/home");
        fs.AddDirectory("/home/operator");
        fs.AddDirectory("/home/operator/logs");
        fs.AddDirectory("/home/operator/reactor");

        fs.AddFile("/home/operator/README.txt",
            "Welcome, Operator.\n" +
            "\n" +
            "The reactor is offline. You need to restart it.\n" +
            "Check the logs/ folder for what happened.\n" +
            "Check the reactor/ folder for how to fix it.");

        fs.AddFile("/home/operator/notes.txt",
            "Personal notes - Dr. Cherry\n" +
            "---\n" +
            "The auth codes were changed last week.\n" +
            "Jerry said the new code was some Greek\n" +
            "letter + a number. Check the config file.");

        fs.AddFile("/home/operator/logs/incident.log",
            "[03-12 02:58] ALERT: Core pressure anomaly\n" +
            "[03-12 02:59] REACTOR OFFLINE\n" +
            "[03-12 03:01] Manual restart required\n" +
            "[03-12 03:01] Use: reactor-restart <auth-code>");

        fs.AddFile("/home/operator/reactor/restart-procedure.txt",
            "REACTOR RESTART PROCEDURE\n" +
            "========================\n" +
            "1. Clear personnel from core chamber.\n" +
            "2. From this terminal, run:\n" +
            "\n" +
            "   reactor-restart <AUTH-CODE>\n" +
            "\n" +
            "3. Auth code is in: reactor/config.bak");

        fs.AddFile("/home/operator/reactor/config.bak",
            "# Reactor Configuration Backup\n" +
            "core.mode=standard\n" +
            "core.pressure_limit=4200\n" +
            "auth.restart_code=DELTA-7742\n" +
            "auth.emergency_override=DISABLED");

        return fs;
    }
}
