# ZARN Programming Language - CLI Installation & Usage Guide

## Overview

ZARN now includes a complete CLI experience similar to Node.js, Python, or React CLI tools. This guide covers installation, usage, and all available commands.

## Features

- **Global Installation**: Install ZARN globally and use from anywhere
- **File Execution**: Run ZARN files with `zarn run file.zn`
- **Project Creation**: Create new projects with `zarn new project-name`
- **Interactive REPL**: Start interactive mode with `zarn`
- **Cross-Platform**: Works on Windows, macOS, and Linux

## Installation

### Prerequisites

- .NET 8.0 SDK or Runtime installed on your system
- Administrator/sudo privileges for global installation

### Step 1: Build ZARN CLI

```bash
# Clone or extract ZARN project
cd ZARN-CLI

# Build the project
dotnet build
```

### Step 2: Install Globally

```bash
# Install ZARN globally (requires admin/sudo)
# Windows (run as Administrator)
dotnet run install

# Linux/macOS (run with sudo)
sudo dotnet run install
```

After installation, you can use `zarn` command from anywhere in your terminal.

## Commands

### 1. Interactive REPL Mode

Start the interactive ZARN interpreter:

```bash
zarn
```

**Example:**
```
$ zarn
ZARN Programming Language Interpreter v1.0
Type 'exit' to quit, 'help' for help.

zarn> say("Hello World");
Hello World
zarn> x = 10; y = 20; say(x + y);
30
zarn> exit
```

### 2. Run ZARN Files

Execute a ZARN script file:

```bash
zarn run <file.zn>
# or simply
zarn <file.zn>
```

**Examples:**
```bash
zarn run hello.zn
zarn run examples/demo.zn
zarn myproject/main.zn
```

### 3. Create New Project

Create a new ZARN project with template files:

```bash
zarn new <project-name>
```

**Example:**
```bash
zarn new my-awesome-app
cd my-awesome-app
zarn run main.zn
```

This creates:
- `main.zn` - Main entry point with example code
- `README.md` - Project documentation

### 4. Version Information

```bash
zarn --version
zarn -v
```

### 5. Help

```bash
zarn --help
zarn -h
```

## Project Structure

After running `zarn new myproject`, you get:

```
myproject/
├── main.zn          # Main entry point
└── README.md        # Project documentation
```

### Sample main.zn

```zarn
// Welcome to your new ZARN project: myproject
// This is the main entry point of your application

say("Hello from myproject!");
say("ZARN is ready to go!");

// Your code starts here
name = input("What's your name? ");
say("Nice to meet you, " + name + "!");

// Example function
fun greet(person) {
    say("Greetings, " + person + "!");
    giveback "Greeting sent to " + person;
}

result = greet(name);
say(result);

// Example with lists
numbers = [1, 2, 3, 4, 5];
say("Numbers: " + get(numbers, 0) + ", " + get(numbers, 1) + ", ...");
say("Total numbers: " + len(numbers));

// Example with control flow
if len(name) > 5 {
    say("You have a long name!");
} else {
    say("You have a short name!");
}

// Example with loops
counter = 1;
while counter <= 3 {
    say("Loop iteration: " + counter);
    counter = counter + 1;
}

say("Thanks for trying myproject!");
```

## Installation Details

### Windows Installation

The installer will:
1. Copy ZARN.exe to a directory in PATH (typically `C:\Windows` or `%APPDATA%\ZARN\bin`)
2. Create a `zarn.bat` wrapper script
3. Copy template files for `zarn new` command

### Linux/macOS Installation

The installer will:
1. Copy runtime files to `/usr/local/share/zarn/`
2. Create a wrapper script at `/usr/local/bin/zarn`
3. Copy template files for `zarn new` command

## Usage Examples

### Basic Script Execution

Create a file `hello.zn`:
```zarn
say("Hello, World!");
name = input("Enter your name: ");
say("Hello, " + name + "!");
```

Run it:
```bash
zarn run hello.zn
```

### Creating and Running a Project

```bash
# Create new project
zarn new calculator

# Navigate to project
cd calculator

# Edit main.zn with your code
# ... add calculator logic ...

# Run the project
zarn run main.zn
```

### Interactive Development

```bash
# Start REPL for quick testing
zarn

# Test functions interactively
zarn> fun add(a, b) { giveback a + b; }
zarn> result = add(5, 3);
zarn> say("5 + 3 = " + result);
5 + 3 = 8
zarn> exit
```

## File Extensions

ZARN CLI uses `.zn` file extension (changed from `.za` in previous versions).

## Troubleshooting

### Command Not Found

If `zarn` command is not found after installation:

**Windows:**
- Ensure the installation directory is in your PATH
- Try running from a new Command Prompt/PowerShell window

**Linux/macOS:**
- Ensure `/usr/local/bin` is in your PATH
- Try running `sudo zarn install` again
- Check if you have .NET runtime installed

### Permission Errors

**Windows:**
- Run Command Prompt as Administrator
- Try installing to a user directory instead

**Linux/macOS:**
- Use `sudo` for installation
- Ensure you have write permissions to `/usr/local/bin`

### .NET Runtime Errors

- Ensure .NET 8.0 runtime is installed
- On Linux, you might need to install `dotnet-runtime-8.0` package

## Development Workflow

1. **Create Project**: `zarn new myapp`
2. **Develop**: Edit `.zn` files in your favorite editor
3. **Test**: `zarn run main.zn`
4. **Debug**: Use `zarn` REPL for interactive testing
5. **Deploy**: Share your `.zn` files with other ZARN users

## Advanced Usage

### Running from Different Directories

```bash
# Run from anywhere
zarn run /path/to/project/main.zn
zarn run ../other-project/script.zn
zarn run C:\Projects\MyApp\main.zn
```

### Batch Processing

```bash
# Run multiple files
zarn run script1.zn
zarn run script2.zn
zarn run script3.zn
```

### Integration with IDEs

You can configure your IDE to run ZARN files:

**VS Code:**
- Create a task to run `zarn run ${file}`
- Set up syntax highlighting for `.zn` files

**Other Editors:**
- Configure build/run commands to use `zarn run`

## Next Steps

1. Try creating your first project with `zarn new hello-world`
2. Explore the ZARN language features in the generated template
3. Check out the examples in the installation directory
4. Join the ZARN community for support and updates

---

**ZARN Team - 2025**  
*Making programming simple and powerful*

