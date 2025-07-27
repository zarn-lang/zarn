# ZARN Programming Language Specification

**Version:** 1.0  
**Author:** Manus AI  
**Date:** January 2025

## Table of Contents

1. [Introduction](#introduction)
2. [Language Overview](#language-overview)
3. [Syntax and Grammar](#syntax-and-grammar)
4. [Data Types](#data-types)
5. [Variables and Assignment](#variables-and-assignment)
6. [Operators](#operators)
7. [Control Flow](#control-flow)
8. [Functions](#functions)
9. [Built-in Functions](#built-in-functions)
10. [Lists and Collections](#lists-and-collections)
11. [Error Handling](#error-handling)
12. [Examples](#examples)
13. [Implementation Details](#implementation-details)
14. [Future Enhancements](#future-enhancements)

## Introduction

ZARN is a modern interpreted programming language designed to combine Python-like simplicity with the power and performance of the .NET ecosystem. Created with beginners in mind, ZARN provides an intuitive syntax while maintaining the capability to build real applications. The language is implemented in C# and runs on .NET 8+, making it cross-platform and highly performant.

The primary design goals of ZARN include:

- **Simplicity**: Easy-to-learn syntax that reduces cognitive overhead for new programmers
- **Readability**: Code that reads like natural language and is self-documenting
- **Flexibility**: Support for multiple programming paradigms including procedural and functional programming
- **Performance**: Built on the robust .NET runtime for excellent performance characteristics
- **Extensibility**: Architecture designed to support future enhancements and library integration

ZARN fills a unique niche in the programming language ecosystem by providing the accessibility of scripting languages like Python while leveraging the mature .NET platform for deployment and performance. This makes it an ideal choice for educational environments, rapid prototyping, and building production applications that require both ease of development and runtime efficiency.

## Language Overview

ZARN is a dynamically typed, interpreted language that supports imperative programming with functional programming features. The language uses a familiar C-style syntax with simplified keywords that make code more readable and approachable for beginners.

### Key Features

**Dynamic Typing**: Variables in ZARN do not require explicit type declarations. The interpreter automatically determines and manages types at runtime, allowing for flexible and rapid development.

**Automatic Memory Management**: Built on the .NET garbage collector, ZARN automatically handles memory allocation and deallocation, eliminating common sources of bugs and security vulnerabilities.

**First-Class Functions**: Functions in ZARN are first-class citizens that can be assigned to variables, passed as arguments, and returned from other functions, enabling powerful functional programming patterns.

**Interactive Development**: ZARN provides both file execution and an interactive REPL (Read-Eval-Print Loop) mode, making it excellent for learning, experimentation, and rapid prototyping.

**Cross-Platform Compatibility**: Running on .NET 8+, ZARN applications can execute on Windows, macOS, and Linux without modification.

### File Extension and Execution

ZARN source files use the `.za` extension and can be executed directly from the command line. The interpreter supports both batch execution of complete programs and interactive line-by-line execution in REPL mode.

```bash
# Execute a ZARN file
zarn myprogram.za

# Start interactive REPL mode
zarn
```

## Syntax and Grammar

ZARN uses a clean, readable syntax that borrows the best elements from popular programming languages while maintaining its own identity. The language is case-sensitive and uses semicolons as statement terminators, though they are optional in many contexts.

### Comments

ZARN supports single-line comments using the `//` syntax:

```za
// This is a single-line comment
say("Hello, World!"); // Comment at end of line
```

### Statement Termination

Statements in ZARN can be terminated with semicolons or newlines. While semicolons are recommended for clarity, the parser is flexible enough to handle most cases without them:

```za
// Both of these are valid
say("Hello");
say("World")
```

### Whitespace and Indentation

ZARN is not whitespace-sensitive like Python. Indentation is used purely for readability and does not affect program execution. However, consistent indentation is strongly recommended for code maintainability.

### Keywords

ZARN uses a minimal set of keywords to keep the language simple:

- `fun` - Function definition
- `if` - Conditional statement
- `else` - Alternative branch
- `while` - Loop construct
- `giveback` - Return statement
- `nothing` - Null value
- `this` - Self-reference (reserved for future use)
- `class` - Class definition (reserved for future use)
- `use` - Import statement (reserved for future use)
- `try` - Exception handling (reserved for future use)
- `catch` - Exception handling (reserved for future use)




## Data Types

ZARN supports several fundamental data types that cover the most common programming needs. The type system is designed to be intuitive and forgiving, with automatic conversions where appropriate.

### Numbers

ZARN uses a unified number type that handles both integers and floating-point values seamlessly. Internally, all numbers are represented as double-precision floating-point values, which provides excellent range and precision for most applications.

```za
// Integer values
x = 42;
y = -17;

// Floating-point values
pi = 3.14159;
temperature = -273.15;

// Scientific notation (future enhancement)
// large = 1.23e6;
```

The unified number system eliminates the complexity of multiple numeric types while maintaining sufficient precision for scientific and financial calculations. Arithmetic operations automatically handle the conversion between what would traditionally be integer and floating-point operations.

### Strings

Strings in ZARN are sequences of Unicode characters enclosed in double quotes. They support concatenation and can contain any valid Unicode character, making them suitable for international applications.

```za
// Basic string literals
name = "Alice";
greeting = "Hello, World!";

// String concatenation
message = "Hello, " + name + "!";

// Empty strings
empty = "";
```

String concatenation is performed using the `+` operator, which automatically converts non-string operands to their string representation. This makes it easy to build complex strings from various data types without explicit conversion functions.

### Booleans

Boolean values represent truth and falsehood in logical operations. ZARN uses the standard `True` and `False` values, though the language also supports truthiness evaluation for other types.

```za
// Boolean literals
isActive = True;
isComplete = False;

// Boolean expressions
result = x > 10;
canProceed = isActive && !isComplete;
```

### Lists

Lists are ordered collections that can contain elements of any type, including other lists. They are dynamic and can grow or shrink during program execution.

```za
// Homogeneous lists
numbers = [1, 2, 3, 4, 5];
names = ["Alice", "Bob", "Charlie"];

// Heterogeneous lists
mixed = [1, "hello", 3.14, True];

// Nested lists
matrix = [[1, 2], [3, 4], [5, 6]];

// Empty lists
empty = [];
```

Lists support zero-based indexing and provide built-in functions for common operations like appending, accessing, and modifying elements.

### Nothing (Null)

The `nothing` value represents the absence of a value or an uninitialized state. It is similar to `null` in other languages but uses a more intuitive keyword.

```za
// Explicit nothing assignment
result = nothing;

// Uninitialized variables default to nothing
// (though explicit initialization is recommended)
```

## Variables and Assignment

ZARN uses dynamic typing with automatic variable creation. Variables do not need to be declared before use; they are created automatically when first assigned a value.

### Variable Naming

Variable names in ZARN must follow these rules:

- Must start with a letter (a-z, A-Z) or underscore (_)
- Can contain letters, digits (0-9), and underscores
- Are case-sensitive
- Cannot be keywords

```za
// Valid variable names
userName = "alice";
user_age = 25;
_private = "hidden";
counter1 = 0;

// Invalid variable names (would cause errors)
// 1counter = 0;    // Cannot start with digit
// user-name = "";  // Hyphens not allowed
// if = 5;          // Cannot use keywords
```

### Assignment Semantics

Variable assignment in ZARN creates a new binding between a name and a value. If the variable already exists, the assignment updates its value. If it doesn't exist, a new variable is created in the current scope.

```za
// Initial assignment creates the variable
count = 0;

// Subsequent assignments update the value
count = count + 1;
count = count * 2;

// Variables can change type
value = 42;        // Number
value = "hello";   // Now a string
value = [1, 2, 3]; // Now a list
```

This flexibility makes ZARN particularly suitable for rapid prototyping and exploratory programming, where the exact structure of data may evolve during development.

### Scope Rules

ZARN uses lexical scoping with function-level scope boundaries. Variables defined within a function are local to that function, while variables defined at the top level are global.

```za
// Global variable
globalVar = "I am global";

fun myFunction() {
    // Local variable
    localVar = "I am local";
    
    // Can access global variables
    say(globalVar);
    
    // Local variables shadow globals with the same name
    globalVar = "I am local now";
    say(globalVar); // Prints local value
}

myFunction();
say(globalVar); // Still prints original global value
```

## Operators

ZARN provides a comprehensive set of operators for arithmetic, comparison, logical operations, and assignment. The operator precedence follows mathematical conventions to ensure intuitive expression evaluation.

### Arithmetic Operators

| Operator | Description | Example | Result |
|----------|-------------|---------|--------|
| `+` | Addition | `5 + 3` | `8` |
| `-` | Subtraction | `5 - 3` | `2` |
| `*` | Multiplication | `5 * 3` | `15` |
| `/` | Division | `6 / 3` | `2` |
| `-` (unary) | Negation | `-5` | `-5` |

Arithmetic operators work with numeric values and follow standard mathematical rules. Division always produces a floating-point result, even when dividing integers.

```za
// Basic arithmetic
sum = 10 + 5;        // 15
difference = 10 - 5; // 5
product = 10 * 5;    // 50
quotient = 10 / 5;   // 2.0

// Operator precedence
result = 2 + 3 * 4;  // 14 (not 20)
result = (2 + 3) * 4; // 20
```

### Comparison Operators

| Operator | Description | Example | Result |
|----------|-------------|---------|--------|
| `==` | Equal to | `5 == 5` | `True` |
| `!=` | Not equal to | `5 != 3` | `True` |
| `>` | Greater than | `5 > 3` | `True` |
| `>=` | Greater than or equal | `5 >= 5` | `True` |
| `<` | Less than | `3 < 5` | `True` |
| `<=` | Less than or equal | `3 <= 5` | `True` |

Comparison operators return boolean values and can be used with any comparable types. String comparison is lexicographic, and numeric comparison follows mathematical rules.

### Logical Operators

| Operator | Description | Example | Result |
|----------|-------------|---------|--------|
| `&&` | Logical AND | `True && False` | `False` |
| `\|\|` | Logical OR | `True \|\| False` | `True` |
| `!` | Logical NOT | `!True` | `False` |

Logical operators support short-circuit evaluation, meaning the second operand is only evaluated if necessary to determine the result.

```za
// Short-circuit evaluation
result = False && expensiveFunction(); // expensiveFunction() not called
result = True || expensiveFunction();  // expensiveFunction() not called
```

### String Concatenation

The `+` operator is overloaded for string concatenation. When at least one operand is a string, the other operand is automatically converted to a string representation.

```za
// String concatenation
greeting = "Hello, " + "World!";
message = "The answer is " + 42;
complex = "Value: " + (10 + 5) + " units";
```


## Control Flow

ZARN provides essential control flow constructs that enable complex program logic while maintaining readability and simplicity. The control flow statements use familiar syntax patterns that will be immediately recognizable to programmers from other languages.

### Conditional Statements (if/else)

The `if` statement allows conditional execution of code blocks based on boolean expressions. The condition is evaluated, and if it's truthy, the associated block is executed.

```za
// Basic if statement
if x > 10 {
    say("x is greater than 10");
}

// if-else statement
if temperature > 30 {
    say("It's hot outside");
} else {
    say("It's not too hot");
}

// Nested if-else (else-if pattern)
if score >= 90 {
    say("Grade: A");
} else {
    if score >= 80 {
        say("Grade: B");
    } else {
        if score >= 70 {
            say("Grade: C");
        } else {
            say("Grade: F");
        }
    }
}
```

The condition in an `if` statement undergoes truthiness evaluation. Values that are considered falsy include `False`, `nothing`, `0`, and empty strings. All other values are considered truthy.

### Loops (while)

ZARN currently supports `while` loops for repetitive execution. The loop continues as long as the condition remains truthy.

```za
// Basic while loop
counter = 0;
while counter < 5 {
    say("Counter: " + counter);
    counter = counter + 1;
}

// While loop with complex condition
running = True;
attempts = 0;
while running && attempts < 10 {
    // Perform some operation
    attempts = attempts + 1;
    
    // Update condition
    if attempts >= 5 {
        running = False;
    }
}
```

While loops provide the foundation for all iterative operations in ZARN. More specialized loop constructs like `for` loops are planned for future versions but can be easily implemented using `while` loops in the current version.

### Loop Control

Currently, ZARN does not have explicit `break` and `continue` statements. Loop control is achieved by manipulating the loop condition or using early returns in functions.

```za
// Simulating break behavior
i = 0;
found = False;
while i < 100 && !found {
    if someCondition(i) {
        found = True; // Exit loop
    } else {
        i = i + 1;
    }
}

// Using function return for early exit
fun findValue(list, target) {
    i = 0;
    while i < len(list) {
        if get(list, i) == target {
            giveback i; // Early return exits function and loop
        }
        i = i + 1;
    }
    giveback -1; // Not found
}
```

## Functions

Functions in ZARN are first-class citizens that encapsulate reusable code blocks. They support parameters, return values, and lexical scoping, making them powerful tools for code organization and abstraction.

### Function Definition

Functions are defined using the `fun` keyword followed by the function name, parameter list, and body block.

```za
// Function with no parameters
fun sayHello() {
    say("Hello, World!");
}

// Function with parameters
fun greet(name) {
    say("Hello, " + name + "!");
}

// Function with multiple parameters
fun add(a, b) {
    giveback a + b;
}

// Function with complex logic
fun factorial(n) {
    if n <= 1 {
        giveback 1;
    } else {
        giveback n * factorial(n - 1);
    }
}
```

Function names follow the same rules as variable names and should be descriptive of the function's purpose. Parameters are local variables that receive the values passed when the function is called.

### Function Calls

Functions are called by specifying the function name followed by parentheses containing the arguments.

```za
// Calling functions
sayHello();                    // No arguments
greet("Alice");               // One argument
result = add(5, 3);           // Multiple arguments, capturing return value
factorial_5 = factorial(5);   // Recursive function call
```

The number of arguments passed must match the number of parameters defined in the function. ZARN performs runtime checking to ensure argument count matches parameter count.

### Return Values

Functions can return values using the `giveback` statement. If no `giveback` statement is executed, the function returns `nothing`.

```za
// Function that returns a value
fun multiply(x, y) {
    giveback x * y;
}

// Function that may return different types
fun processInput(input) {
    if input == "" {
        giveback nothing;
    } else {
        giveback "Processed: " + input;
    }
}

// Function with multiple return points
fun findMax(a, b, c) {
    if a >= b && a >= c {
        giveback a;
    }
    if b >= c {
        giveback b;
    }
    giveback c;
}
```

The `giveback` statement immediately exits the function and returns the specified value to the caller. This makes it useful for early returns and guard clauses.

### Recursion

ZARN fully supports recursive function calls, enabling elegant solutions to problems that have recursive structure.

```za
// Classic recursive factorial
fun factorial(n) {
    if n <= 1 {
        giveback 1;
    } else {
        giveback n * factorial(n - 1);
    }
}

// Fibonacci sequence
fun fibonacci(n) {
    if n <= 1 {
        giveback n;
    } else {
        giveback fibonacci(n - 1) + fibonacci(n - 2);
    }
}

// Tree traversal example
fun sumTree(node) {
    if node == nothing {
        giveback 0;
    } else {
        leftSum = sumTree(get(node, 1));   // Left child
        rightSum = sumTree(get(node, 2));  // Right child
        giveback get(node, 0) + leftSum + rightSum; // Value + children
    }
}
```

Recursive functions must have a base case to prevent infinite recursion. The ZARN runtime will eventually throw a stack overflow error if recursion becomes too deep.

### Lexical Scoping and Closures

ZARN implements lexical scoping, which means functions can access variables from their defining scope even when called from a different scope.

```za
// Closure example
fun makeCounter() {
    count = 0;
    
    fun increment() {
        count = count + 1;
        giveback count;
    }
    
    giveback increment;
}

// Usage
counter1 = makeCounter();
counter2 = makeCounter();

say(counter1()); // 1
say(counter1()); // 2
say(counter2()); // 1 (independent counter)
```

This closure behavior enables powerful programming patterns like factory functions, decorators, and functional programming techniques.

## Built-in Functions

ZARN provides a set of built-in functions that handle common operations like input/output, list manipulation, and utility functions. These functions are automatically available in all ZARN programs without requiring imports.

### Input/Output Functions

#### say(value)

The `say` function outputs a value to the console followed by a newline. It automatically converts non-string values to their string representation.

```za
say("Hello, World!");        // String output
say(42);                     // Number output: "42"
say([1, 2, 3]);             // List output: "[1, 2, 3]"
say(nothing);               // Outputs: "nothing"
```

#### input(prompt)

The `input` function displays a prompt to the user and returns their input as a string.

```za
name = input("What is your name? ");
age = input("How old are you? ");
say("Hello, " + name + "! You are " + age + " years old.");
```

### List Manipulation Functions

#### len(collection)

Returns the length of a list or string.

```za
numbers = [1, 2, 3, 4, 5];
say(len(numbers));          // 5
say(len("hello"));          // 5
say(len([]));              // 0
```

#### get(list, index)

Retrieves an element from a list at the specified index. Indices are zero-based.

```za
fruits = ["apple", "banana", "cherry"];
say(get(fruits, 0));        // "apple"
say(get(fruits, 2));        // "cherry"
```

#### set(list, index, value)

Sets the value of an element in a list at the specified index.

```za
numbers = [1, 2, 3];
set(numbers, 1, 99);
say(numbers);               // [1, 99, 3]
```

#### append(list, value)

Adds a new element to the end of a list.

```za
colors = ["red", "green"];
append(colors, "blue");
say(colors);                // ["red", "green", "blue"]
```

### Type Conversion and Utility

Built-in functions automatically handle type conversion where appropriate. For example, `say` converts all values to strings, and arithmetic operators convert strings to numbers when possible.

```za
// Automatic string conversion
number = 42;
text = "The answer is " + number;  // Converts 42 to "42"

// List operations with mixed types
mixed = [1, "hello", 3.14];
append(mixed, True);
say(mixed);                 // [1, "hello", 3.14, True]
```

## Lists and Collections

Lists are the primary collection type in ZARN, providing ordered storage for multiple values. They are dynamic, heterogeneous, and support a variety of operations for manipulation and access.

### List Creation and Initialization

Lists can be created using square bracket notation with comma-separated values.

```za
// Empty list
empty = [];

// Homogeneous lists
numbers = [1, 2, 3, 4, 5];
names = ["Alice", "Bob", "Charlie"];

// Heterogeneous lists
mixed = [1, "hello", 3.14, True, nothing];

// Nested lists
matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];
```

### List Access and Modification

Elements in lists can be accessed using square bracket notation with zero-based indexing.

```za
fruits = ["apple", "banana", "cherry"];

// Access elements
first = fruits[0];          // "apple"
last = fruits[2];           // "cherry"

// Using get function (alternative syntax)
second = get(fruits, 1);    // "banana"

// Modify elements
fruits[1] = "blueberry";    // Direct assignment
set(fruits, 2, "date");     // Using set function

say(fruits);                // ["apple", "blueberry", "date"]
```

### Dynamic List Operations

Lists can grow and shrink during program execution, making them suitable for dynamic data storage.

```za
// Start with empty list
shopping = [];

// Add items
append(shopping, "milk");
append(shopping, "bread");
append(shopping, "eggs");

say("Shopping list: " + shopping);
say("Items to buy: " + len(shopping));

// Process items
i = 0;
while i < len(shopping) {
    item = get(shopping, i);
    say("Buy: " + item);
    i = i + 1;
}
```

### Nested Lists and Complex Data Structures

Lists can contain other lists, enabling the creation of complex data structures like matrices, trees, and records.

```za
// Matrix operations
matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];

// Access matrix elements
element = get(get(matrix, 1), 2);  // matrix[1][2] = 6

// Person records using lists
person1 = ["Alice", 30, "Engineer"];
person2 = ["Bob", 25, "Designer"];
people = [person1, person2];

// Access person data
name = get(get(people, 0), 0);     // "Alice"
age = get(get(people, 0), 1);      // 30
```

### List Algorithms and Patterns

Common algorithms can be implemented using ZARN's list operations and control flow.

```za
// Linear search
fun findInList(list, target) {
    i = 0;
    while i < len(list) {
        if get(list, i) == target {
            giveback i;
        }
        i = i + 1;
    }
    giveback -1;  // Not found
}

// List reversal
fun reverseList(original) {
    reversed = [];
    i = len(original) - 1;
    while i >= 0 {
        append(reversed, get(original, i));
        i = i - 1;
    }
    giveback reversed;
}

// List filtering
fun filterPositive(numbers) {
    positive = [];
    i = 0;
    while i < len(numbers) {
        num = get(numbers, i);
        if num > 0 {
            append(positive, num);
        }
        i = i + 1;
    }
    giveback positive;
}
```


## Error Handling

ZARN provides runtime error detection and reporting to help developers identify and fix issues in their programs. The interpreter performs various checks during execution and provides meaningful error messages when problems occur.

### Runtime Error Types

**Type Errors**: Occur when operations are performed on incompatible types.

```za
// This would cause a runtime error
result = "hello" - 5;  // Cannot subtract number from string
```

**Index Errors**: Occur when accessing list elements with invalid indices.

```za
numbers = [1, 2, 3];
value = get(numbers, 10);  // Index out of bounds error
```

**Function Call Errors**: Occur when calling functions with incorrect argument counts.

```za
fun add(a, b) {
    giveback a + b;
}

result = add(5);  // Error: Expected 2 arguments but got 1
```

**Variable Errors**: Occur when accessing undefined variables (though ZARN's auto-creation reduces these).

```za
say(undefinedVariable);  // Error: Undefined variable
```

### Error Messages

ZARN provides descriptive error messages that include line numbers and context to help developers locate and fix issues quickly.

```
Error: Runtime error at line 15: List index out of bounds.
Error: Runtime error at line 8: Expected 2 arguments but got 1.
Error: Unexpected character '%' at line 12, column 5
```

## Examples

This section provides comprehensive examples demonstrating ZARN's capabilities across different programming domains.

### Basic Calculator

```za
// Simple calculator program
fun calculator() {
    say("ZARN Calculator");
    say("==============");
    
    while True {
        operation = input("Enter operation (+, -, *, /) or 'quit': ");
        
        if operation == "quit" {
            say("Goodbye!");
            giveback;
        }
        
        if operation == "+" || operation == "-" || operation == "*" || operation == "/" {
            a = input("Enter first number: ");
            b = input("Enter second number: ");
            
            // Convert strings to numbers (simplified)
            num1 = parseNumber(a);
            num2 = parseNumber(b);
            
            if operation == "+" {
                result = num1 + num2;
            } else {
                if operation == "-" {
                    result = num1 - num2;
                } else {
                    if operation == "*" {
                        result = num1 * num2;
                    } else {
                        if num2 == 0 {
                            say("Error: Division by zero!");
                            result = nothing;
                        } else {
                            result = num1 / num2;
                        }
                    }
                }
            }
            
            if result != nothing {
                say("Result: " + result);
            }
        } else {
            say("Invalid operation!");
        }
    }
}

// Helper function to parse numbers (simplified)
fun parseNumber(str) {
    // In a real implementation, this would handle string-to-number conversion
    // For now, assume valid numeric input
    giveback str + 0;  // Simple conversion trick
}

calculator();
```

### List Processing and Data Analysis

```za
// Student grade analysis program
fun analyzeGrades() {
    // Sample student data: [name, grade1, grade2, grade3]
    students = [
        ["Alice", 85, 92, 78],
        ["Bob", 76, 84, 88],
        ["Charlie", 92, 89, 95],
        ["Diana", 68, 72, 75]
    ];
    
    say("Student Grade Analysis");
    say("=====================");
    
    // Calculate averages for each student
    i = 0;
    while i < len(students) {
        student = get(students, i);
        name = get(student, 0);
        grade1 = get(student, 1);
        grade2 = get(student, 2);
        grade3 = get(student, 3);
        
        average = (grade1 + grade2 + grade3) / 3;
        
        say(name + " - Average: " + average);
        
        // Determine letter grade
        if average >= 90 {
            say("  Letter Grade: A");
        } else {
            if average >= 80 {
                say("  Letter Grade: B");
            } else {
                if average >= 70 {
                    say("  Letter Grade: C");
                } else {
                    say("  Letter Grade: F");
                }
            }
        }
        
        i = i + 1;
    }
    
    // Calculate class statistics
    totalAverage = calculateClassAverage(students);
    say("");
    say("Class Average: " + totalAverage);
}

fun calculateClassAverage(students) {
    totalSum = 0;
    totalGrades = 0;
    
    i = 0;
    while i < len(students) {
        student = get(students, i);
        grade1 = get(student, 1);
        grade2 = get(student, 2);
        grade3 = get(student, 3);
        
        totalSum = totalSum + grade1 + grade2 + grade3;
        totalGrades = totalGrades + 3;
        
        i = i + 1;
    }
    
    giveback totalSum / totalGrades;
}

analyzeGrades();
```

### Text Processing and Word Count

```za
// Simple text analysis program
fun analyzeText() {
    text = input("Enter text to analyze: ");
    
    say("Text Analysis Results");
    say("====================");
    
    // Character count
    charCount = len(text);
    say("Character count: " + charCount);
    
    // Word count (simplified - counts spaces + 1)
    wordCount = countWords(text);
    say("Word count: " + wordCount);
    
    // Find most common character
    mostCommon = findMostCommonChar(text);
    say("Most common character: " + mostCommon);
}

fun countWords(text) {
    if len(text) == 0 {
        giveback 0;
    }
    
    spaceCount = 0;
    i = 0;
    while i < len(text) {
        // Note: This is simplified - real implementation would need
        // character-by-character access
        i = i + 1;
    }
    
    // Simplified word counting
    giveback 1; // Placeholder implementation
}

fun findMostCommonChar(text) {
    // Simplified implementation
    giveback "a"; // Placeholder
}

analyzeText();
```

### Game: Number Guessing

```za
// Number guessing game
fun numberGuessingGame() {
    say("Welcome to the Number Guessing Game!");
    say("I'm thinking of a number between 1 and 100.");
    
    // Generate random number (simplified)
    secretNumber = 42; // In real implementation, would be random
    
    attempts = 0;
    maxAttempts = 7;
    won = False;
    
    while attempts < maxAttempts && !won {
        guess = input("Enter your guess: ");
        attempts = attempts + 1;
        
        // Convert string to number (simplified)
        guessNum = parseNumber(guess);
        
        if guessNum == secretNumber {
            say("Congratulations! You guessed it in " + attempts + " attempts!");
            won = True;
        } else {
            if guessNum < secretNumber {
                say("Too low! Try again.");
            } else {
                say("Too high! Try again.");
            }
            
            remaining = maxAttempts - attempts;
            if remaining > 0 {
                say("You have " + remaining + " attempts left.");
            }
        }
    }
    
    if !won {
        say("Sorry! The number was " + secretNumber + ". Better luck next time!");
    }
}

numberGuessingGame();
```

## Implementation Details

ZARN is implemented as a tree-walking interpreter written in C# and targeting .NET 8+. The implementation follows standard interpreter design patterns with clear separation between lexical analysis, parsing, and execution phases.

### Architecture Overview

The ZARN interpreter consists of several key components:

**Lexer (Tokenizer)**: Converts source code into a stream of tokens, handling keywords, identifiers, operators, literals, and punctuation. The lexer supports Unicode strings and provides detailed error reporting with line and column information.

**Parser**: Implements a recursive descent parser that converts tokens into an Abstract Syntax Tree (AST). The parser handles operator precedence, associativity, and provides meaningful syntax error messages.

**AST Nodes**: Represent different language constructs as tree nodes, including expressions (binary operations, function calls, literals) and statements (variable declarations, control flow, function definitions).

**Interpreter**: Implements the visitor pattern to traverse and execute the AST. The interpreter manages the runtime environment, variable scoping, function calls, and built-in function execution.

**Environment**: Manages variable storage and scoping using a chain of environment objects. Supports lexical scoping with automatic variable creation for dynamic typing.

### Performance Characteristics

Running on the .NET runtime provides ZARN with several performance advantages:

- **Just-In-Time Compilation**: The .NET JIT compiler optimizes frequently executed code paths
- **Garbage Collection**: Automatic memory management eliminates memory leaks and reduces development complexity
- **Cross-Platform**: Runs efficiently on Windows, macOS, and Linux
- **Interoperability**: Can potentially integrate with existing .NET libraries and frameworks

### File Format and Execution

ZARN source files use the `.za` extension and are stored as UTF-8 encoded text files. The interpreter can execute files directly or provide an interactive REPL environment for experimentation and learning.

```bash
# Execute a ZARN file
dotnet run myprogram.za

# Start REPL mode
dotnet run
```

### Error Handling and Debugging

The interpreter provides comprehensive error reporting with:

- **Lexical Errors**: Character-level errors with precise location information
- **Syntax Errors**: Parse errors with context about expected tokens
- **Runtime Errors**: Execution errors with line numbers and descriptive messages
- **Stack Traces**: Function call information for debugging recursive functions

## Future Enhancements

ZARN is designed with extensibility in mind, and several enhancements are planned for future versions:

### Language Features

**Object-Oriented Programming**: Classes, inheritance, and method dispatch will enable more sophisticated program organization and code reuse patterns.

**Module System**: Import and export mechanisms will allow code organization across multiple files and enable library development.

**Exception Handling**: Try-catch blocks will provide structured error handling for more robust applications.

**Advanced Data Types**: Maps/dictionaries, sets, and tuples will expand the available data structures for complex applications.

**Pattern Matching**: Destructuring assignment and pattern matching will enable more expressive data processing.

### Standard Library

**File I/O**: Reading and writing files will enable data persistence and file processing applications.

**Network Operations**: HTTP client capabilities will enable web service integration and API consumption.

**Mathematical Functions**: Trigonometry, logarithms, and statistical functions will support scientific computing applications.

**String Processing**: Regular expressions and advanced string manipulation will enhance text processing capabilities.

**Date and Time**: Comprehensive date/time handling will support business applications and scheduling systems.

### Development Tools

**Syntax Highlighting**: Editor support for popular IDEs and text editors will improve the development experience.

**Debugger**: Step-through debugging with breakpoints and variable inspection will aid in development and troubleshooting.

**Package Manager**: A package management system will enable easy distribution and consumption of third-party libraries.

**Documentation Generator**: Automatic documentation generation from code comments will support library development.

### Performance Optimizations

**Bytecode Compilation**: Compiling to bytecode instead of tree-walking interpretation will improve execution speed.

**Optimization Passes**: Dead code elimination, constant folding, and other optimizations will enhance performance.

**Native Compilation**: AOT compilation to native code will enable deployment scenarios requiring maximum performance.

### Integration Capabilities

**.NET Interoperability**: Direct access to .NET libraries will leverage the extensive .NET ecosystem.

**C Interoperability**: P/Invoke support will enable integration with native libraries and system APIs.

**Web Framework**: Built-in web server capabilities will enable web application development.

**GUI Framework**: Desktop application development using modern UI frameworks like Avalonia or .NET MAUI.

---

**ZARN Programming Language Specification v1.0**  
*Created by Manus AI - January 2025*

This specification represents the current state of the ZARN programming language and will be updated as new features are implemented and the language evolves. For the latest information and updates, please refer to the official ZARN repository and documentation.

