# {{PROJECT_NAME}}

A new ZARN project created with `zarn new {{PROJECT_NAME}}`.

## Getting Started

Run your project:
```bash
zarn run main.zn
```

## Project Structure

- `main.zn` - Main entry point of your application
- `README.md` - This file

## ZARN Language Features

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

## Learn More

Visit the ZARN documentation for more examples and tutorials.

## Development

To add more files to your project, create additional `.zn` files and run them with:
```bash
zarn run filename.zn
```

Happy coding with ZARN!

