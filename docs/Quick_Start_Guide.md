# ZARN Quick Start Guide

Welcome to ZARN! This guide will get you up and running with the ZARN programming language in just a few minutes.

## Installation

### Prerequisites
- .NET 8.0 SDK or later
- Any text editor or IDE

### Building ZARN
1. Clone or download the ZARN source code
2. Open a terminal in the ZARN directory
3. Run: `dotnet build`
4. The interpreter will be built and ready to use

## Your First ZARN Program

### Hello World
Create a file called `hello.za`:

```za
say("Hello, World!");
say("Welcome to ZARN!");
```

Run it with:
```bash
dotnet run hello.za
```

### Interactive Mode (REPL)
Start the interactive interpreter:
```bash
dotnet run
```

Try these commands:
```za
zarn> say("Hello!");
Hello!
zarn> x = 10;
zarn> y = 20;
zarn> say("Sum: " + (x + y));
Sum: 30
zarn> exit
```

## Basic Syntax

### Variables
```za
// Numbers
age = 25;
price = 19.99;

// Strings
name = "Alice";
message = "Hello, " + name + "!";

// Lists
numbers = [1, 2, 3, 4, 5];
mixed = [1, "hello", 3.14];
```

### Functions
```za
// Simple function
fun greet(name) {
    say("Hello, " + name + "!");
}

// Function with return value
fun add(a, b) {
    giveback a + b;
}

// Call functions
greet("World");
result = add(5, 3);
say("5 + 3 = " + result);
```

### Control Flow
```za
// If statements
if age >= 18 {
    say("You are an adult");
} else {
    say("You are a minor");
}

// While loops
counter = 1;
while counter <= 5 {
    say("Count: " + counter);
    counter = counter + 1;
}
```

### Lists and Built-in Functions
```za
// Create and manipulate lists
fruits = ["apple", "banana"];
say("Fruits: " + fruits);
say("Count: " + len(fruits));

// Add items
append(fruits, "cherry");
say("After append: " + fruits);

// Access items
first = get(fruits, 0);
say("First fruit: " + first);

// Modify items
set(fruits, 1, "blueberry");
say("After modification: " + fruits);
```

## Example Programs

### Calculator
```za
fun calculator() {
    a = input("Enter first number: ");
    b = input("Enter second number: ");
    
    // Simple conversion (add 0 to convert string to number)
    num1 = a + 0;
    num2 = b + 0;
    
    say("Sum: " + (num1 + num2));
    say("Difference: " + (num1 - num2));
    say("Product: " + (num1 * num2));
    say("Quotient: " + (num1 / num2));
}

calculator();
```

### List Processing
```za
// Find maximum in a list
fun findMax(numbers) {
    if len(numbers) == 0 {
        giveback nothing;
    }
    
    max = get(numbers, 0);
    i = 1;
    while i < len(numbers) {
        current = get(numbers, i);
        if current > max {
            max = current;
        }
        i = i + 1;
    }
    giveback max;
}

// Test the function
testNumbers = [3, 7, 2, 9, 1, 5];
maximum = findMax(testNumbers);
say("Numbers: " + testNumbers);
say("Maximum: " + maximum);
```

### Recursive Function
```za
// Factorial calculation
fun factorial(n) {
    if n <= 1 {
        giveback 1;
    } else {
        giveback n * factorial(n - 1);
    }
}

// Calculate factorials
i = 1;
while i <= 5 {
    result = factorial(i);
    say("Factorial of " + i + " = " + result);
    i = i + 1;
}
```

## Built-in Functions Reference

| Function | Description | Example |
|----------|-------------|---------|
| `say(value)` | Print value to console | `say("Hello");` |
| `input(prompt)` | Get user input | `name = input("Name: ");` |
| `len(collection)` | Get length | `len([1,2,3])` returns `3` |
| `get(list, index)` | Get element | `get([1,2,3], 0)` returns `1` |
| `set(list, index, value)` | Set element | `set(list, 0, 99);` |
| `append(list, value)` | Add element | `append(list, "new");` |

## Tips for Getting Started

1. **Start Simple**: Begin with basic variable assignments and function calls
2. **Use the REPL**: The interactive mode is great for experimenting
3. **Read Error Messages**: ZARN provides helpful error messages with line numbers
4. **Practice with Lists**: Lists are powerful - practice the built-in list functions
5. **Write Functions**: Break your code into small, reusable functions

## Common Patterns

### Input Validation
```za
fun getPositiveNumber(prompt) {
    while True {
        input_str = input(prompt);
        number = input_str + 0; // Convert to number
        if number > 0 {
            giveback number;
        }
        say("Please enter a positive number.");
    }
}
```

### List Filtering
```za
fun filterPositive(numbers) {
    result = [];
    i = 0;
    while i < len(numbers) {
        num = get(numbers, i);
        if num > 0 {
            append(result, num);
        }
        i = i + 1;
    }
    giveback result;
}
```

### Menu System
```za
fun showMenu() {
    while True {
        say("1. Option A");
        say("2. Option B");
        say("3. Exit");
        
        choice = input("Choose an option: ");
        
        if choice == "1" {
            say("You chose Option A");
        } else {
            if choice == "2" {
                say("You chose Option B");
            } else {
                if choice == "3" {
                    say("Goodbye!");
                    giveback;
                } else {
                    say("Invalid choice!");
                }
            }
        }
    }
}
```

## Next Steps

- Read the full [ZARN Language Specification](ZARN_Language_Specification.md)
- Explore the example programs in the `examples/` directory
- Try the test programs in the `tests/` directory
- Experiment with the REPL to learn the language interactively

Happy coding with ZARN!

