# ZARN Programming Language with CLI Support

ZARN is a modern interpreted programming language designed for simplicity and power, now with complete CLI support similar to Node.js, Python, or React tools.

## Quick Start

### Installation

**Windows (run as Administrator):**
```cmd
install.bat
```

**Linux/macOS (run with sudo):**
```bash
sudo ./install.sh
```

### Usage

```bash
# Interactive REPL
zarn

# Run a file
zarn run script.zn
zarn script.zn

# Create new project
zarn new myproject

# Get help
zarn --help
```

## CLI Commands

| Command | Description | Example |
|---------|-------------|---------|
| `zarn` | Start interactive REPL | `zarn` |
| `zarn run <file>` | Run a ZARN file | `zarn run hello.zn` |
| `zarn <file>` | Run a ZARN file (shorthand) | `zarn hello.zn` |
| `zarn new <name>` | Create new project | `zarn new myapp` |
| `zarn install` | Install ZARN globally | `zarn install` |
| `zarn --version` | Show version | `zarn --version` |
| `zarn --help` | Show help | `zarn --help` |

## Language Features

### Variables
```zarn
name = "ZARN";
x = 10;
y = 20.5;
isActive = nothing;
```

### Functions
```zarn
fun greet(name) {
    say("Hello " + name);
    giveback "Done";
}

result = greet("World");
```

### Control Flow
```zarn
if x > 10 {
    say("Greater than 10");
} else {
    say("Not greater");
}

while x < 20 {
    say(x);
    x = x + 1;
}
```

### Lists
```zarn
myList = [1, "two", 3.0];
append(myList, 4);
set(myList, 1, "new_two");
say(get(myList, 0));
say(len(myList));
```

### Built-in Functions
- `say(text)` - Print text to console
- `input(prompt)` - Get user input
- `len(list)` - Get length of list or string
- `get(list, index)` - Get element from list
- `set(list, index, value)` - Set element in list
- `append(list, value)` - Add element to list

## Project Structure

```
ZARN-CLI/
├── CLI/                 # CLI command handling
│   └── CommandHandler.cs
├── Templates/           # Project templates
│   ├── main.zn
│   └── README.md
├── examples/            # Example programs
│   ├── hello.zn
│   ├── comprehensive_demo.zn
│   └── final_demo.zn
├── Lexer/              # Tokenization
├── Parser/             # Syntax analysis
├── AST/                # Abstract syntax tree
├── Interpreter/        # Code execution
├── BuiltIns/           # Built-in functions
├── install.bat         # Windows installer
├── install.sh          # Linux/macOS installer
└── CLI-INSTALLATION-GUIDE.md
```

## Development Workflow

1. **Create Project**: `zarn new myapp`
2. **Develop**: Edit `.zn` files
3. **Test**: `zarn run main.zn`
4. **Debug**: Use `zarn` REPL for interactive testing

## Examples

### Hello World
```zarn
say("Hello, World!");
name = input("What's your name? ");
say("Nice to meet you, " + name + "!");
```

### Fibonacci Function
```zarn
fun fibonacci(n) {
    if n <= 1 {
        giveback n;
    }
    giveback fibonacci(n - 1) + fibonacci(n - 2);
}

result = fibonacci(10);
say("Fibonacci(10) = " + result);
```

### List Processing
```zarn
numbers = [1, 2, 3, 4, 5];
total = 0;
i = 0;

while i < len(numbers) {
    total = total + get(numbers, i);
    i = i + 1;
}

say("Sum of numbers: " + total);
```

## Installation Details

### Global Installation

The `zarn install` command:

**Windows:**
- Copies ZARN.exe to a PATH directory
- Creates `zarn.bat` wrapper script
- Copies templates to installation directory

**Linux/macOS:**
- Copies runtime files to `/usr/local/share/zarn/`
- Creates wrapper script at `/usr/local/bin/zarn`
- Copies templates for project creation

### Manual Installation

If automatic installation fails:

1. Build the project: `dotnet build`
2. Copy the `bin/Debug/net8.0/` contents to a directory in your PATH
3. Create a wrapper script that calls `dotnet ZARN.dll`

## Requirements

- .NET 8.0 SDK or Runtime
- Windows 10+, macOS 10.15+, or Linux (Ubuntu 18.04+)

## File Extensions

ZARN uses `.zn` file extension for source files.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test with `dotnet build` and `dotnet test`
5. Submit a pull request

## License

MIT License - see LICENSE file for details.

## Support

- Documentation: See `CLI-INSTALLATION-GUIDE.md`
- Issues: Report bugs and feature requests
- Community: Join discussions and get help

---

**ZARN Team - 2025**  
*Making programming simple and powerful*

