using System;
using System.Collections.Generic;
using System.Linq;

// ============================================================
// C# COMPLETE GUIDE
// All major concepts in one practical codebase
// ============================================================

class Program
{
    static void Main()
    {
        Console.WriteLine("========== C# COMPLETE GUIDE ==========\n");

        // ============================================================
        // 1. BASIC SYNTAX
        // ============================================================

        Console.WriteLine("=== 1. BASIC SYNTAX ===\n");

        // Variables with different data types
        string appName = "MyApp";          // String
        int age = 25;                      // Integer
        double salary = 50000.50;          // Decimal number
        bool isActive = true;              // Boolean
        char grade = 'A';                  // Single character
        
        Console.WriteLine($"App: {appName}");
        Console.WriteLine($"Age: {age}, Salary: {salary}, Grade: {grade}");
        Console.WriteLine($"Is Active: {isActive}\n");

        // String interpolation (using $ prefix)
        string message = $"Hello, {appName}! You are {age} years old.";
        Console.WriteLine(message);

        // String concatenation
        string fullMessage = "Welcome to " + appName + " application";
        Console.WriteLine(fullMessage);

        // Constants (cannot be changed)
        const int MAX_USERS = 100;
        const string COMPANY = "TechCorp";
        Console.WriteLine($"Max users: {MAX_USERS}, Company: {COMPANY}\n");

        // Nullable types
        int? nullableAge = null;
        int? actualAge = 30;
        Console.WriteLine($"Nullable age: {nullableAge}, Actual age: {actualAge}\n");

        // Arrays
        int[] numbers = { 1, 2, 3, 4, 5 };
        string[] fruits = new string[] { "apple", "banana", "cherry" };
        Console.WriteLine($"Numbers: {string.Join(", ", numbers)}");
        Console.WriteLine($"Fruits: {string.Join(", ", fruits)}\n");

        // List (dynamic array)
        List<string> cities = new List<string> { "NYC", "LA", "Chicago" };
        cities.Add("Boston");
        Console.WriteLine($"Cities: {string.Join(", ", cities)}\n");

        // Dictionary (key-value pairs)
        Dictionary<string, int> scores = new Dictionary<string, int>
        {
            { "Alice", 95 },
            { "Bob", 87 },
            { "Charlie", 92 }
        };
        Console.WriteLine($"Alice's score: {scores["Alice"]}\n");

        // Tuples (multiple values)
        (string name, int id, double gpa) student = ("Diana", 101, 3.8);
        Console.WriteLine($"Student: {student.name}, ID: {student.id}, GPA: {student.gpa}\n");

        // ============================================================
        // 2. CONTROL FLOWS
        // ============================================================

        Console.WriteLine("\n=== 2. CONTROL FLOWS ===\n");

        // if/else statement
        int score = 85;
        if (score >= 90)
        {
            Console.WriteLine("Grade: A");
        }
        else if (score >= 80)
        {
            Console.WriteLine("Grade: B");
        }
        else if (score >= 70)
        {
            Console.WriteLine("Grade: C");
        }
        else
        {
            Console.WriteLine("Grade: F");
        }

        // Switch statement
        int day = 3;
        switch (day)
        {
            case 1:
                Console.WriteLine("Monday");
                break;
            case 2:
                Console.WriteLine("Tuesday");
                break;
            case 3:
                Console.WriteLine("Wednesday");
                break;
            default:
                Console.WriteLine("Other day");
                break;
        }

        // Switch expression (C# 8.0+)
        string dayName = day switch
        {
            1 => "Monday",
            2 => "Tuesday",
            3 => "Wednesday",
            _ => "Other day"
        };
        Console.WriteLine($"Day using switch expression: {dayName}\n");

        // Ternary operator
        string status = age >= 18 ? "Adult" : "Minor";
        Console.WriteLine($"Status: {status}");

        // Logical operators: && (AND), || (OR), ! (NOT)
        bool hasLicense = true;
        bool hasInsurance = false;
        bool canDrive = hasLicense && hasInsurance;
        Console.WriteLine($"Can drive: {canDrive}");

        bool isVIP = false;
        bool isStudent = true;
        bool getDiscount = isVIP || isStudent;
        Console.WriteLine($"Get discount: {getDiscount}\n");

        // for loop
        Console.WriteLine("For loop:");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"  Iteration {i}");
        }

        // while loop
        Console.WriteLine("\nWhile loop:");
        int counter = 0;
        while (counter < 3)
        {
            Console.WriteLine($"  Counter: {counter}");
            counter++;
        }

        // do-while loop (runs at least once)
        Console.WriteLine("\nDo-while loop:");
        int num = 0;
        do
        {
            Console.WriteLine($"  Number: {num}");
            num++;
        } while (num < 3);

        // foreach loop
        Console.WriteLine("\nForEach loop:");
        foreach (string fruit in fruits)
        {
            Console.WriteLine($"  - {fruit}");
        }

        // foreach with List
        Console.WriteLine("\nForEach with List:");
        foreach (var score_item in scores)
        {
            Console.WriteLine($"  {score_item.Key}: {score_item.Value}");
        }

        // break and continue
        Console.WriteLine("\nBreak and Continue:");
        for (int i = 0; i < 5; i++)
        {
            if (i == 2) continue;  // Skip iteration
            if (i == 4) break;     // Exit loop
            Console.WriteLine($"  Iteration {i}");
        }

        // ============================================================
        // 3. FUNCTIONS AND STORING METHODS
        // ============================================================

        Console.WriteLine("\n\n=== 3. FUNCTIONS AND STORING METHODS ===\n");

        // Simple function
        int result1 = Add(5, 3);
        Console.WriteLine($"Add(5, 3) = {result1}");

        // Function with no return
        PrintGreeting("Eve");

        // Function with multiple return values (tuple)
        (int sum, int product) = Calculate(4, 5);
        Console.WriteLine($"Sum: {sum}, Product: {product}");

        // Function with default parameters
        string greeting = SayHello("Frank", "Sir");
        Console.WriteLine(greeting);
        string greeting2 = SayHello("Grace");  // Uses default parameter
        Console.WriteLine(greeting2);

        // Function with params (variable number of arguments)
        int sumAll = SumNumbers(1, 2, 3, 4, 5);
        Console.WriteLine($"Sum of 1,2,3,4,5: {sumAll}\n");

        // ============================================================
        // STORING METHODS IN CLASSES
        // ============================================================

        Console.WriteLine("--- Storing Methods in Classes ---\n");

        // Calculator class with methods
        Calculator calc = new Calculator();
        calc.SetValue(10);
        calc.Add(5);
        calc.Multiply(2);
        calc.Subtract(3);
        Console.WriteLine($"Calculator result: {calc.GetValue()}");

        // Banking class with methods
        BankAccount account = new BankAccount("Helen", 1000);
        account.Deposit(500);
        account.Withdraw(200);
        Console.WriteLine($"Account holder: {account.GetAccountHolder()}, Balance: ${account.GetBalance()}\n");

        // Static method (called on class, not instance)
        Console.WriteLine($"Bank name: {BankAccount.GetBankName()}\n");

        // ============================================================
        // 4. POINTERS AND REFERENCES
        // ============================================================

        Console.WriteLine("\n=== 4. POINTERS AND REFERENCES ===\n");

        // Value types (int, double, bool, etc.) - pass by value
        int x = 10;
        int y = x;
        y = 20;
        Console.WriteLine($"Value types - x: {x}, y: {y}");  // x is still 10
        Console.WriteLine("(Value types are independent copies)\n");

        // Reference types (objects, arrays, lists) - pass by reference
        Person person1 = new Person { Name = "Ivan", Age = 28 };
        Person person2 = person1;  // Both point to same object
        person2.Name = "John";
        Console.WriteLine($"Reference types - person1.Name: {person1.Name}, person2.Name: {person2.Name}");
        Console.WriteLine("(Reference types point to same memory)\n");

        // Creating independent copy of object
        Person person3 = new Person { Name = person1.Name, Age = person1.Age };
        person3.Name = "Jack";
        Console.WriteLine($"Independent copy - person1.Name: {person1.Name}, person3.Name: {person3.Name}\n");

        // Arrays - reference behavior
        int[] array1 = { 1, 2, 3 };
        int[] array2 = array1;  // Points to same array
        array2[0] = 99;
        Console.WriteLine($"Arrays - array1[0]: {array1[0]}, array2[0]: {array2[0]}");
        Console.WriteLine("(Arrays point to same memory)\n");

        // Creating independent copy of array
        int[] array3 = (int[])array1.Clone();
        array3[0] = 100;
        Console.WriteLine($"Array copy - array1[0]: {array1[0]}, array3[0]: {array3[0]}\n");

        // UNSAFE POINTERS (advanced - requires unsafe context)
        Console.WriteLine("--- Unsafe Pointers (Advanced) ---");
        unsafe
        {
            int ptrValue = 50;
            int* ptr = &ptrValue;  // Get address of variable
            Console.WriteLine($"Value: {ptrValue}, Pointer value: {*ptr}");  // Dereference pointer
            *ptr = 60;  // Change value through pointer
            Console.WriteLine($"After pointer change: {ptrValue}\n");
        }

        // ============================================================
        // 5. EXCEPTION HANDLING
        // ============================================================

        Console.WriteLine("\n=== 5. EXCEPTION HANDLING ===\n");

        // try-catch block
        try
        {
            int divisor = 0;
            int divisionResult = 10 / divisor;
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Caught exception: {ex.Message}");
        }

        // try-catch-finally (finally always runs)
        try
        {
            int[] arr = { 1, 2, 3 };
            int element = arr[10];  // Out of bounds
        }
        catch (IndexOutOfRangeException ex)
        {
            Console.WriteLine($"Index out of range: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Cleanup code (finally block always runs)\n");
        }

        // Catching multiple exceptions
        try
        {
            string input = "abc";
            int convertedValue = int.Parse(input);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Format error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }

        // Custom exception class
        try
        {
            ValidateEmail("invalid-email");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation failed: {ex.Message}\n");
        }

        // Function that throws custom exception
        try
        {
            int divideResult = Divide(10, 0);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Division error: {ex.Message}\n");
        }

        // Using statements for resource management
        Console.WriteLine("--- Using Statement (Resource Management) ---");
        using (var file = new FileSimulator("data.txt"))
        {
            file.WriteData("Hello, World!");
        }  // Automatically disposed/cleaned up here
        Console.WriteLine();

        Console.WriteLine("\n========== END OF GUIDE ==========");
    }

    // ============================================================
    // FUNCTION DEFINITIONS
    // ============================================================

    // Simple function - returns value
    static int Add(int a, int b)
    {
        return a + b;
    }

    // Function with no return value
    static void PrintGreeting(string name)
    {
        Console.WriteLine($"Hello, {name}!");
    }

    // Function returning multiple values (tuple)
    static (int, int) Calculate(int a, int b)
    {
        return (a + b, a * b);  // Returns sum and product
    }

    // Function with default parameters
    static string SayHello(string name, string title = "Friend")
    {
        return $"Hello, {title} {name}!";
    }

    // Function with params (variable number of arguments)
    static int SumNumbers(params int[] nums)
    {
        int total = 0;
        foreach (int num in nums)
        {
            total += num;
        }
        return total;
    }

    // Function that throws exception
    static int Divide(int a, int b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero!");
        }
        return a / b;
    }

    // Function that validates and throws custom exception
    static void ValidateEmail(string email)
    {
        if (!email.Contains("@"))
        {
            throw new ValidationException($"Invalid email: {email}");
        }
    }
}

// ============================================================
// CALCULATOR CLASS - Methods stored in class
// ============================================================

class Calculator
{
    private int value = 0;

    // Method to set value
    public void SetValue(int v)
    {
        value = v;
    }

    // Method to add
    public void Add(int num)
    {
        value += num;
    }

    // Method to subtract
    public void Subtract(int num)
    {
        value -= num;
    }

    // Method to multiply
    public void Multiply(int num)
    {
        value *= num;
    }

    // Method to get result
    public int GetValue()
    {
        return value;
    }
}

// ============================================================
// BANKACCOUNT CLASS - Full example with constructor & methods
// ============================================================

class BankAccount
{
    // Properties (private data, public access methods)
    private string accountHolder;
    private double balance;
    private List<Transaction> transactions;

    // Constructor
    public BankAccount(string holder, double initialBalance)
    {
        accountHolder = holder;
        balance = initialBalance;
        transactions = new List<Transaction>();
        transactions.Add(new Transaction { Type = "Initial", Amount = initialBalance });
    }

    // Instance method - Deposit
    public void Deposit(double amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be positive");
        }
        balance += amount;
        transactions.Add(new Transaction { Type = "Deposit", Amount = amount });
    }

    // Instance method - Withdraw
    public void Withdraw(double amount)
    {
        if (amount > balance)
        {
            throw new InvalidOperationException("Insufficient funds");
        }
        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be positive");
        }
        balance -= amount;
        transactions.Add(new Transaction { Type = "Withdraw", Amount = amount });
    }

    // Instance method - Get balance
    public double GetBalance()
    {
        return balance;
    }

    // Instance method - Get account holder
    public string GetAccountHolder()
    {
        return accountHolder;
    }

    // Static method (called on class, not instance)
    public static string GetBankName()
    {
        return "MyBank";
    }

    // Nested class for transactions
    private class Transaction
    {
        public string Type { get; set; }
        public double Amount { get; set; }
    }
}

// ============================================================
// PERSON CLASS - Simple class for reference type demo
// ============================================================

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// ============================================================
// CUSTOM EXCEPTION CLASS
// ============================================================

class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}

// ============================================================
// FILE SIMULATOR CLASS - For using statement demo
// ============================================================

class FileSimulator : IDisposable
{
    private string filename;

    public FileSimulator(string name)
    {
        filename = name;
        Console.WriteLine($"Opening file: {filename}");
    }

    public void WriteData(string data)
    {
        Console.WriteLine($"Writing to {filename}: {data}");
    }

    public void Dispose()
    {
        Console.WriteLine($"Closing file: {filename} (disposed)");
    }
}
