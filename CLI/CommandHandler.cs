using System;
using System.IO;
using System.Diagnostics;
using ZARN.Lexer;
using ZARN.Parser;
using ZARN.Interpreter;

namespace ZARN.CLI
{
    public class CommandHandler
    {
        private readonly string _currentDirectory;
        private readonly string _executablePath;

        public CommandHandler()
        {
            _currentDirectory = Directory.GetCurrentDirectory();
            _executablePath = Process.GetCurrentProcess().MainModule?.FileName ?? "";
        }

        public void HandleCommand(string[] args)
        {
            if (args.Length == 0)
            {
                StartRepl();
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "run":
                    HandleRunCommand(args);
                    break;
                case "new":
                    HandleNewCommand(args);
                    break;
                case "install":
                    HandleInstallCommand(args);
                    break;
                case "--version":
                case "-v":
                    ShowVersion();
                    break;
                case "--help":
                case "-h":
                    ShowHelp();
                    break;
                default:
                    // If it's not a recognized command, try to run it as a file
                    if (File.Exists(command))
                    {
                        RunZarnFile(command);
                    }
                    else
                    {
                        Console.WriteLine($"Unknown command: {command}");
                        Console.WriteLine("Use 'zarn --help' for available commands.");
                    }
                    break;
            }
        }

        private void HandleRunCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: zarn run <file.zn>");
                return;
            }

            string filePath = args[1];
            RunZarnFile(filePath);
        }

        private void HandleNewCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: zarn new <project_name>");
                return;
            }

            string projectName = args[1];
            CreateNewProject(projectName);
        }

        private void HandleInstallCommand(string[] args)
        {
            InstallGlobally();
        }

        private void RunZarnFile(string filePath)
        {
            try
            {
                // Make path absolute if it's relative
                if (!Path.IsPathRooted(filePath))
                {
                    filePath = Path.Combine(_currentDirectory, filePath);
                }

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Error: File '{filePath}' not found.");
                    return;
                }

                string source = File.ReadAllText(filePath);
                
                var lexer = new Lexer.Lexer(source);
                var tokens = lexer.ScanTokens();
                
                var parser = new Parser.Parser(tokens);
                var statements = parser.Parse();
                
                var interpreter = new Interpreter.Interpreter();
                interpreter.Interpret(statements);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Runtime Error: {ex.Message}");
            }
        }

        private void CreateNewProject(string projectName)
        {
            try
            {
                string projectPath = Path.Combine(_currentDirectory, projectName);
                
                if (Directory.Exists(projectPath))
                {
                    Console.WriteLine($"Error: Directory '{projectName}' already exists.");
                    return;
                }

                Directory.CreateDirectory(projectPath);
                
                // Get template path relative to executable
                string executableDir = Path.GetDirectoryName(_executablePath) ?? _currentDirectory;
                string templatePath = Path.Combine(executableDir, "Templates");
                
                if (!Directory.Exists(templatePath))
                {
                    // Fallback: create basic template inline
                    CreateBasicTemplate(projectPath, projectName);
                }
                else
                {
                    // Copy template files
                    CopyDirectory(templatePath, projectPath);
                    
                    // Replace placeholders in template files
                    ReplacePlaceholders(projectPath, projectName);
                }

                Console.WriteLine($"Created new ZARN project: {projectName}");
                Console.WriteLine($"To get started:");
                Console.WriteLine($"  cd {projectName}");
                Console.WriteLine($"  zarn run main.zn");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating project: {ex.Message}");
            }
        }

        private void CreateBasicTemplate(string projectPath, string projectName)
        {
            // Create main.zn
            string mainContent = $@"// Welcome to your new ZARN project: {projectName}
// This is the main entry point of your application

say(""Hello from {projectName}!"");
say(""ZARN is ready to go!"");

// Your code starts here
name = input(""What's your name? "");
say(""Nice to meet you, "" + name + ""!"");

// Example function
fun greet(person) {{
    say(""Greetings, "" + person + ""!"");
    giveback ""Greeting sent to "" + person;
}}

result = greet(name);
say(result);
";

            File.WriteAllText(Path.Combine(projectPath, "main.zn"), mainContent);

            // Create README.md
            string readmeContent = $@"# {projectName}

A new ZARN project created with `zarn new {projectName}`.

## Getting Started

Run your project:
```bash
zarn run main.zn
```

## Project Structure

- `main.zn` - Main entry point of your application
- `README.md` - This file

## ZARN Language Features

- Variables: `x = 10; name = ""ZARN"";`
- Functions: `fun greet(name) {{ say(""Hi "" + name); }}`
- Control Flow: `if x > 5 {{ ... }} else {{ ... }}`
- Loops: `while x < 10 {{ x = x + 1; }}`
- Lists: `numbers = [1, 2, 3]; append(numbers, 4);`
- Built-ins: `say()`, `input()`, `len()`, `get()`, `set()`, `append()`

## Learn More

Visit the ZARN documentation for more examples and tutorials.
";

            File.WriteAllText(Path.Combine(projectPath, "README.md"), readmeContent);
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceDir, file);
                string destFile = Path.Combine(destDir, relativePath);
                
                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
                File.Copy(file, destFile);
            }
        }

        private void ReplacePlaceholders(string projectPath, string projectName)
        {
            foreach (string file in Directory.GetFiles(projectPath, "*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(file).ToLower() is ".zn" or ".md" or ".txt")
                {
                    string content = File.ReadAllText(file);
                    content = content.Replace("{{PROJECT_NAME}}", projectName);
                    File.WriteAllText(file, content);
                }
            }
        }

        private void InstallGlobally()
        {
            try
            {
                string executableDir = Path.GetDirectoryName(_executablePath) ?? _currentDirectory;
                
                if (OperatingSystem.IsWindows())
                {
                    InstallOnWindows(executableDir);
                }
                else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                {
                    InstallOnUnix(executableDir);
                }
                else
                {
                    Console.WriteLine("Global installation is not supported on this platform.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Installation failed: {ex.Message}");
                Console.WriteLine("You may need to run this command as administrator/root.");
            }
        }

        private void InstallOnWindows(string executableDir)
        {
            // Try to install to a directory in PATH
            string[] pathDirs = {
                @"C:\Windows",
                Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "ZARN", "bin"),
                Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "ZARN", "bin")
            };

            string? installDir = null;
            foreach (string dir in pathDirs)
            {
                try
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    
                    // Test write access
                    string testFile = Path.Combine(dir, "zarn_test.tmp");
                    File.WriteAllText(testFile, "test");
                    File.Delete(testFile);
                    
                    installDir = dir;
                    break;
                }
                catch
                {
                    continue;
                }
            }

            if (installDir == null)
            {
                Console.WriteLine("Could not find a writable directory in PATH.");
                Console.WriteLine("Please run as administrator or manually add ZARN to your PATH.");
                return;
            }

            // Copy ZARN executable
            string zarnExe = Path.Combine(executableDir, "ZARN.exe");
            string targetExe = Path.Combine(installDir, "ZARN.exe");
            
            if (File.Exists(zarnExe))
            {
                File.Copy(zarnExe, targetExe, true);
            }

            // Create zarn.bat wrapper
            string batContent = $@"@echo off
""{targetExe}"" %*
";
            File.WriteAllText(Path.Combine(installDir, "zarn.bat"), batContent);

            // Copy templates and examples if they exist
            string templatesSource = Path.Combine(executableDir, "Templates");
            string examplesSource = Path.Combine(executableDir, "examples");
            
            if (Directory.Exists(templatesSource))
            {
                string templatesTarget = Path.Combine(installDir, "Templates");
                if (Directory.Exists(templatesTarget))
                    Directory.Delete(templatesTarget, true);
                CopyDirectory(templatesSource, templatesTarget);
            }

            Console.WriteLine($"ZARN installed successfully to: {installDir}");
            Console.WriteLine("You can now use 'zarn' command from anywhere in the terminal.");
            
            if (installDir.Contains("AppData"))
            {
                Console.WriteLine($"Note: Make sure '{installDir}' is in your PATH environment variable.");
            }
        }

        private void InstallOnUnix(string executableDir)
        {
            string installDir = "/usr/local/bin";
            string sharedDir = "/usr/local/share/zarn";
            
            // Create shared directory for runtime files
            if (Directory.Exists(sharedDir))
                Directory.Delete(sharedDir, true);
            Directory.CreateDirectory(sharedDir);

            // Copy all runtime files to shared directory
            foreach (string file in Directory.GetFiles(executableDir))
            {
                if (Path.GetExtension(file).ToLower() is ".dll" or ".exe" or ".pdb" or ".json")
                {
                    string fileName = Path.GetFileName(file);
                    File.Copy(file, Path.Combine(sharedDir, fileName), true);
                }
            }

            // Copy templates and examples
            string templatesSource = Path.Combine(executableDir, "Templates");
            string examplesSource = Path.Combine(executableDir, "examples");
            
            if (Directory.Exists(templatesSource))
            {
                CopyDirectory(templatesSource, Path.Combine(sharedDir, "Templates"));
            }
            
            if (Directory.Exists(examplesSource))
            {
                CopyDirectory(examplesSource, Path.Combine(sharedDir, "examples"));
            }

            // Create wrapper script
            string wrapperScript = $@"#!/bin/bash
dotnet ""{Path.Combine(sharedDir, "ZARN.dll")}"" ""$@""
";
            
            string targetPath = Path.Combine(installDir, "zarn");
            File.WriteAllText(targetPath, wrapperScript);
            
            // Make executable
            Process.Start("chmod", $"+x {targetPath}").WaitForExit();

            Console.WriteLine($"ZARN installed successfully to: {targetPath}");
            Console.WriteLine("You can now use 'zarn' command from anywhere in the terminal.");
        }

        private void StartRepl()
        {
            Console.WriteLine("ZARN Programming Language Interpreter v1.0");
            Console.WriteLine("Type 'exit' to quit, 'help' for help.");
            Console.WriteLine();

            var interpreter = new Interpreter.Interpreter();

            while (true)
            {
                Console.Write("zarn> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.Trim().ToLower() == "exit")
                    break;

                if (input.Trim().ToLower() == "help")
                {
                    ShowReplHelp();
                    continue;
                }

                try
                {
                    var lexer = new Lexer.Lexer(input);
                    var tokens = lexer.ScanTokens();
                    
                    var parser = new Parser.Parser(tokens);
                    var statements = parser.Parse();
                    
                    interpreter.Interpret(statements);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void ShowVersion()
        {
            Console.WriteLine("ZARN Programming Language v1.0.0");
            Console.WriteLine("Built with .NET 8.0");
        }

        private void ShowHelp()
        {
            Console.WriteLine("ZARN Programming Language CLI");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  zarn                    Start interactive REPL");
            Console.WriteLine("  zarn run <file.zn>      Run a ZARN file");
            Console.WriteLine("  zarn new <project>      Create a new ZARN project");
            Console.WriteLine("  zarn install            Install ZARN globally");
            Console.WriteLine("  zarn --version          Show version information");
            Console.WriteLine("  zarn --help             Show this help message");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  zarn run hello.zn");
            Console.WriteLine("  zarn new my-app");
            Console.WriteLine("  zarn install");
        }

        private void ShowReplHelp()
        {
            Console.WriteLine("REPL Commands:");
            Console.WriteLine("  exit    - Exit the REPL");
            Console.WriteLine("  help    - Show this help");
            Console.WriteLine();
            Console.WriteLine("ZARN Language Examples:");
            Console.WriteLine("  say(\"Hello World\");");
            Console.WriteLine("  x = 10; y = 20; say(x + y);");
            Console.WriteLine("  fun add(a, b) { giveback a + b; }");
            Console.WriteLine("  numbers = [1, 2, 3]; say(len(numbers));");
        }
    }
}

