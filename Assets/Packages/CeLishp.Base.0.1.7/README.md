# CeLishp: Extensible & Modular scripting language
CeLishp is a framework for implementing a (domain-specific) scripting language into a project.

[Online documentation can be found here.](https://celishp.skyesprung.com)

## Background
Have you ever found yourself implementing yet another simple scripting language for a project, when you've done that too many times already? This project aims to provide a reusable framework for simply adding such a language, where you only need to replace the components you need, and provide a value and function inventory.

## Usage
CeLishp provides a set of interfaces that can/must be implemented in order to construct a scripting language.
All functions and values derive from *IInterpretable*, where input values derive from *IInputValue* and functions from *INaryFunction*.

### Quick Start
To get started, the types provided in *CeLishp.Interpreter.Implementation* for values and functions as well as the Syntax Provider *CeLishp.Parser.Implementation.SimpleLispSyntax* can be used.
A minimal program using these could look like this:

```csharp
using CeLishp.Interpreter;
using CeLishp.Interpreter.Implementation;
using CeLishp.Parser;
using CeLishp.Parser.Implementation;

// function inventory must be provided, in this case there are only two.
var functionInventory = new Dictionary<string, INaryFunction>()
{
    { "add", new NaryAddition() }, // NaryAddition is assigned to the keyword "add"
    { "sub", new NarySubtraction() } // NarySubtraction is assigned to the keyword "sub"
};

// value inventory is the possible value keywords
var valueInventory = new Dictionary<string, IInputValue>()
{
    { "seven", new Constant<float>(7f) }, // We define "seven" as meaning a constant value of 7
};

// SimpleLispSyntax is our SyntaxProvider, it provides a very minimal Lisp-like syntax with support for numeric literals
ISyntaxProvider provider = new SimpleLispSyntax();
Console.Write("Formula: ");

// get source code as string, e.g. from stdin
var sourceCode = Console.ReadLine();

// generate a syntax tree from the input, as long as it is not null
var synTree = provider.GenerateTree(sourceCode!);

// parse the syntax tree into an interpretable form using our function and value inventory
var exTree = provider.ParseTree(synTree, functionInventory, valueInventory);

// an Interpreter is needed to run our interpretable tree
var interpreter = new Interpreter(exTree);

// we let our interpreter run the formula once, specifying the output type as a float
Console.WriteLine($"Result of our formula = {interpreter.RunTree<float>()}");
```
This program could, for example, take the input `(add (sub seven 3.2) 4.3)` and produce the output value 9.1

### Custom values
In most, if not all cases, you'd want to get some kind of value to use in your calculations.
This can be done by implementing *IInputValue*, and adding this to our valueInventory. For example, if we want to add random numbers, we would
create a new class as follows:
```csharp
using CeLishp.Interpreter;

public class RandomNumber : IInputValue
{
    public object Run(IInterpretable input)
    {
        throw new NotImplementedException();
    }
    // GetValue in our case simply returns a random float between 0f and 1f
    public object GetValue()
    {
        var random = new Random();
        return random.NextSingle();
    }
}
```

Then, we add this along with a keyword to our valueInventory:
```csharp
var valueInventory = new Dictionary<string, IInputValue>()
{
    { "rand", new RandomNumber() },
};
```
Then, we proceed as shown above. Now, whenever "rand" appears as a keyword in our formula, a random number is substituted at runtime.

### Custom functions
Custom functions derive from *INaryFunction*. If we want to add e.g. a conditional value selector, we would add the following class:
```csharp
using CeLishp.Interpreter;

public class Conditional : INaryFunction
{
    public object Run(object[] input)
    {
        throw new NotImplementedException();
    }

    public object RunNary(object[] inputs)
    {
        if (inputs == null || inputs.Length != 3)
            throw new ArgumentException("Conditional must have exactly three inputs");
        if (inputs[0] is not bool b)
            throw new ArgumentException("Conditional must have a boolean as first argument!");
        if (b)
            return inputs[0];
        return inputs[1];
    }
}
```
We then simply add this to our functionInventory:
```csharp
var functionInventory = new Dictionary<string, INaryFunction>()
{
    { "add", new NaryAddition() },
    { "sub", new NarySubtraction() },
    { "if", new Conditional() },
};
```
Now, our source code can contain conditionals.

### Custom Syntax
To implement a custom syntax, we must implement *ISyntaxProvider*. 