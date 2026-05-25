# .NET (ASP.NET Core) - Complete Beginner's Guide

## Overview
**.NET** is a framework for building web applications. Think of it like LEGO blocks - you use pre-built pieces to create apps quickly. **ASP.NET Core** is the part that helps you build websites and APIs (web services).

---

## 1. C# Essentials (The Language)

C# is the programming language you write in .NET. Think of it like English for talking to computers.

### What is a Class?
A class is like a blueprint for creating objects. For example, a blueprint for a User:

```csharp
public class User
{
    public int Id { get; set; }        // Id number (like ID card number)
    public string Name { get; set; }   // Name of the person
}
```

**What does this mean?**
- `public class User` = Create a blueprint called "User" that anyone can use
- `public int Id` = The user has an ID that is a number (integer)
- `public string Name` = The user has a name that is text (string)
- `{ get; set; }` = You can read (get) and write (set) these values

### Using the User class:
```csharp
// Create a new user
User myUser = new User();
myUser.Id = 1;
myUser.Name = "John";

// Or create it all at once
User anotherUser = new User { Id = 2, Name = "Alice" };
```

### What is Async/Await?
Async means "not waiting" - let the computer do other things while waiting for a response.

```csharp
// Without async - the app waits for database
public User GetUser(int id)
{
    // Get user from database (this takes time!)
    User user = database.GetUserFromDatabase(id);
    return user;
}

// With async - the app doesn't wait, it does other things
public async Task<User> GetUserAsync(int id)
{
    // The computer can help other users while this waits
    User user = await database.GetUserFromDatabaseAsync(id);
    return user;
}
```

**Why is async important?**
- Without async: App waits → other users have to wait too → slow app
- With async: App doesn't wait → other users get served → fast app


---

## 2. .NET CLI & SDK (Command Line Tools)

CLI means "Command Line Interface" - it's how you talk to .NET using text commands.

### What is SDK?
SDK = Software Development Kit. It's all the tools you need to create .NET applications. Think of it like a toolbox with hammers, screwdrivers, etc.

### Basic Commands (What they do):

**Create a new project:**
```bash
dotnet new webapi -n MyApp
```
**What happens:** 
- Creates a new folder called "MyApp"
- Adds all the files you need to start a web API
- Like starting with a template instead of blank paper

**Run your project:**
```bash
dotnet run
```
**What happens:**
- Starts your application
- You can visit it at: http://localhost:5000
- Your app is now live!

**Build your project:**
```bash
dotnet build
```
**What happens:**
- Converts your C# code into machine language
- Checks for errors
- Creates the executable file

**Add a package (library):**
```bash
dotnet add package Newtonsoft.Json
```
**What happens:**
- Downloads a library called "Newtonsoft.Json"
- Lets you use it in your project
- Like downloading a plugin


---

## 3. ASP.NET Core (The Web Framework)

ASP.NET Core is the tool that helps you create websites and web APIs. It's like a kitchen - it provides everything you need to make a website.

### What is Program.cs?
Program.cs is like the "instruction manual" for your application. It tells the app what to do when it starts.

```csharp
// Program.cs - The start of your application
var builder = WebApplication.CreateBuilder(args);
// ^ This is like "prepare the kitchen"

var app = builder.Build();
// ^ This is like "setup the kitchen equipment"

// Create a simple endpoint
app.MapGet("/hello", () => "Hello World!");
// ^ When someone visits /hello, say "Hello World!"

app.Run();
// ^ Start the application!
```

**What is an endpoint?**
An endpoint is a URL on your website. When someone visits that URL, your code runs.

**Example:**
```
User visits: http://localhost:5000/hello
Your code runs: () => "Hello World!"
Result: User sees "Hello World!" on screen
```


---

## 4. Web API (Creating Endpoints)

### What is an API?
API = Application Programming Interface. It's a way for the internet to talk to your app. Like a waiter in a restaurant - you order food, the waiter gets it for you.

### The 4 Basic Operations:

#### **1. GET - Retrieve/Read Data**
Get data from the server. Like asking "What's my username?"

```csharp
[HttpGet("/user/{id}")]
public string GetUser(int id)
{
    return "John";  // Returns the user's name
}
```

**When someone visits:** http://localhost:5000/user/1
**They get back:** "John"

#### **2. POST - Create New Data**
Send new data to the server. Like saying "Create a new user"

```csharp
[HttpPost("/user")]
public string CreateUser(string name)
{
    // Save the new user to database
    return "User created: " + name;
}
```

**When someone sends:** POST request with name = "Alice"
**They get back:** "User created: Alice"

#### **3. PUT - Update Existing Data**
Change existing data. Like saying "Update my username"

```csharp
[HttpPut("/user/{id}")]
public string UpdateUser(int id, string newName)
{
    // Update user's name in database
    return "User " + id + " updated to " + newName;
}
```

**When someone sends:** PUT request to /user/1 with newName = "Bob"
**They get back:** "User 1 updated to Bob"

#### **4. DELETE - Remove Data**
Delete data. Like saying "Remove this user"

```csharp
[HttpDelete("/user/{id}")]
public string DeleteUser(int id)
{
    // Remove user from database
    return "User " + id + " deleted";
}
```

**When someone sends:** DELETE request to /user/1
**They get back:** "User 1 deleted"

### Real Example with All Four:

```csharp
public class UsersController
{
    // GET: Get a user by ID
    [HttpGet("/users/{id}")]
    public string GetUser(int id)
    {
        return "User ID: " + id + ", Name: John";
    }

    // POST: Create a new user
    [HttpPost("/users")]
    public string CreateUser(string name)
    {
        return "Created user: " + name;
    }

    // PUT: Update a user
    [HttpPut("/users/{id}")]
    public string UpdateUser(int id, string newName)
    {
        return "Updated user " + id + " to " + newName;
    }

    // DELETE: Delete a user
    [HttpDelete("/users/{id}")]
    public string DeleteUser(int id)
    {
        return "Deleted user " + id;
    }
}
```

**How to test these:**
```bash
# GET request
curl http://localhost:5000/users/1

# POST request
curl -X POST http://localhost:5000/users -d "name=John"

# PUT request
curl -X PUT http://localhost:5000/users/1 -d "newName=Bob"

# DELETE request
curl -X DELETE http://localhost:5000/users/1
```


---

## 5. Routing & Binding (URLs and Data)

### What is Routing?
Routing is how URLs are connected to code. Like a GPS - it finds the right code for the right URL.

**Example:**
```
URL: http://localhost:5000/users/1
Routing: Takes you to the GetUser(1) method
```

### Simple Routing:

```csharp
// When user visits: http://localhost:5000/hello
[HttpGet("/hello")]
public string SayHello()
{
    return "Hello!";
}

// When user visits: http://localhost:5000/users/5
[HttpGet("/users/{id}")]
public string GetUser(int id)
{
    return "Getting user number: " + id;
}
```

### What is Model Binding?
Model binding is automatic conversion. When someone sends data to your API, C# automatically converts it into an object.

**Example:**
```csharp
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// When user SENDS this JSON:
// { "name": "John", "email": "john@example.com" }
// C# automatically creates a User object with those values

[HttpPost("/users")]
public string CreateUser(User user)  // Automatic conversion happens here!
{
    return "Created user: " + user.Name + " with email: " + user.Email;
}
```

### Different Ways to Send Data:

**1. In the URL (Route Parameter):**
```csharp
[HttpGet("/users/{id}")]
public string GetUser(int id)
{
    return "User ID: " + id;
}
// Visit: http://localhost:5000/users/5
// id = 5
```

**2. In the URL (Query Parameter):**
```csharp
[HttpGet("/search")]
public string Search(string keyword)
{
    return "Searching for: " + keyword;
}
// Visit: http://localhost:5000/search?keyword=john
// keyword = "john"
```

**3. In the Body (JSON):**
```csharp
[HttpPost("/users")]
public string CreateUser(User user)
{
    return "Created: " + user.Name;
}
// Send JSON: { "name": "John", "email": "john@example.com" }
```


---

## 6. Dependency Injection (Sharing Tools)

### What is Dependency Injection?
Dependency Injection means "passing tools to where they're needed" instead of creating them yourself.

**Without Dependency Injection (Bad Way):**
```csharp
public class UserController
{
    public string GetUser(int id)
    {
        // Creating the service every time - wasteful!
        UserService service = new UserService();
        return service.GetUser(id);
    }
}
```

**Problems:**
- Creates a new service every time = slow
- Wasteful = uses more memory
- Hard to test

**With Dependency Injection (Good Way):**
```csharp
// Step 1: Create a service
public class UserService
{
    public string GetUser(int id)
    {
        return "User: " + id;
    }
}

// Step 2: Register the service in Program.cs
builder.Services.AddScoped<UserService>();
// ^ Says: "When someone needs UserService, give them one"

// Step 3: Use it in your controller
public class UserController
{
    private UserService _service;

    // The service is automatically given to you!
    public UserController(UserService service)
    {
        _service = service;
    }

    public string GetUser(int id)
    {
        // Use the service that was given to you
        return _service.GetUser(id);
    }
}
```

### How it works:
```
1. You need a UserService
2. Instead of creating it yourself, you ask for it
3. .NET automatically creates it and gives it to you
4. You use it
```

### Using an Interface (Even Better):

```csharp
// Step 1: Create an interface (contract)
public interface IUserService
{
    string GetUser(int id);
}

// Step 2: Create a class that follows the interface
public class UserService : IUserService
{
    public string GetUser(int id)
    {
        return "User: " + id;
    }
}

// Step 3: Register in Program.cs
builder.Services.AddScoped<IUserService, UserService>();
// ^ Says: "When someone needs IUserService, give them UserService"

// Step 4: Use it
public class UserController
{
    private IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    public string GetUser(int id)
    {
        return _service.GetUser(id);
    }
}
```

### Benefits:
✅ Don't create services yourself - .NET does it
✅ Faster and uses less memory
✅ Easy to test (you can replace with fake data)
✅ Can change services without changing controllers


---

## 7. Entity Framework (Database Access)

### What is Entity Framework?
Entity Framework (EF) is a tool that helps you work with databases easily. Instead of writing SQL, you write C# code.

**Without Entity Framework (Hard Way):**
```csharp
// You write SQL
string sql = "SELECT * FROM Users WHERE Id = 1";
// Then convert it to C# objects
// Lots of work and confusing!
```

**With Entity Framework (Easy Way):**
```csharp
// Just write C# - Entity Framework converts it to SQL automatically!
var user = dbContext.Users.FirstOrDefault(u => u.Id == 1);
```

### Step 1: Create a User Model

```csharp
// This is like a blueprint for the Users table in the database
public class User
{
    public int Id { get; set; }           // ID number (automatically a primary key)
    public string Name { get; set; }      // User's name
    public string Email { get; set; }     // User's email
}
```

### Step 2: Create a Database Context

```csharp
// This is like a connection to your database
public class AppDbContext : DbContext
{
    // This says: "I have a Users table in the database"
    public DbSet<User> Users { get; set; }

    // This tells Entity Framework how to connect to the database
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=localhost;Database=mydb;User=sa;Password=123");
        // ^ Connection string = address to your database
    }
}
```

### Step 3: Use It

**CREATE (Add a new user):**
```csharp
var dbContext = new AppDbContext();

var newUser = new User
{
    Name = "John",
    Email = "john@example.com"
};

dbContext.Users.Add(newUser);  // Add to the list
dbContext.SaveChangesAsync();   // Save to database
```

**READ (Get users):**
```csharp
// Get one user
var user = dbContext.Users.FirstOrDefault(u => u.Id == 1);
// ^ Find the first user where Id = 1

// Get all users
var allUsers = dbContext.Users.ToList();
// ^ Get all users from database

// Get users that match a condition
var users = dbContext.Users.Where(u => u.Name == "John").ToList();
// ^ Get all users named "John"
```

**UPDATE (Change a user):**
```csharp
var user = dbContext.Users.FirstOrDefault(u => u.Id == 1);
user.Name = "Bob";  // Change the name
dbContext.SaveChangesAsync();  // Save to database
```

**DELETE (Remove a user):**
```csharp
var user = dbContext.Users.FirstOrDefault(u => u.Id == 1);
dbContext.Users.Remove(user);  // Mark for deletion
dbContext.SaveChangesAsync();  // Delete from database
```

### What is a Migration?
A migration is like taking a snapshot of your database structure. If you change your model, migrations help update the database.

```bash
# Create a migration (snapshot of changes)
dotnet ef migrations add AddUserTable

# Apply the migration to the database
dotnet ef database update
```

**Simple Explanation:**
```
You change your User class
↓
Create a migration (records what changed)
↓
Apply the migration (updates the database)
```


---

## 8. Auth & Identity (Login/Security)

### What is Authentication?
Authentication means "proving who you are" - like showing your ID card. Login is authentication.

### Simple Login Example:

```csharp
// Step 1: Create a login request (what the user sends)
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

// Step 2: Create a login endpoint
[HttpPost("/login")]
public string Login(LoginRequest request)
{
    // Check if username and password are correct
    if (request.Username == "admin" && request.Password == "password123")
    {
        return "Login successful!";
    }
    else
    {
        return "Login failed!";
    }
}
```

**How it works:**
```
User enters username: "admin"
User enters password: "password123"
↓
Your code checks if they're correct
↓
If correct → "Login successful!"
If wrong → "Login failed!"
```

### What is a JWT Token?
JWT is a token that proves you're logged in. Like a ticket - you show the ticket to enter an event.

**How JWT works:**
```
1. User logs in
2. Server creates a JWT token (like a ticket)
3. User keeps the token
4. User sends the token with every request
5. Server checks the token to verify the user
```

**Simple JWT Example:**
```csharp
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

[HttpPost("/login")]
public string Login(LoginRequest request)
{
    // Check credentials
    if (request.Username == "admin" && request.Password == "password123")
    {
        // Create a JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("your-secret-key-12345");
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.Name, request.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),  // Token expires in 1 hour
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        
        return tokenString;  // Send the token to the user
    }
    
    return "Login failed!";
}
```

### Protected Endpoints (Requires Login)

```csharp
// This endpoint requires a valid token
[Authorize]  // Only logged-in users can access
[HttpGet("/profile")]
public string GetProfile()
{
    // Get the username from the token
    var username = User.FindFirst(ClaimTypes.Name)?.Value;
    return "Hello " + username + "!";
}
```

**How it works:**
```
User wants to visit /profile
↓
Check: Do they have a valid token?
↓
If YES → Show profile
If NO → "Unauthorized - please login"
```

### Registration (Create Account)

```csharp
[HttpPost("/register")]
public string Register(LoginRequest request)
{
    // Save the new user to the database
    var newUser = new User
    {
        Username = request.Username,
        Password = request.Password  // Never store plain password! Hash it first
    };
    
    dbContext.Users.Add(newUser);
    dbContext.SaveChangesAsync();
    
    return "Account created!";
}
```


---

## 9. Validation (Checking Data)

### What is Validation?
Validation means checking if the data is correct before using it. Like checking if a email looks like an email.

### Simple Validation Rules:

```csharp
public class User
{
    [Required]  // Must provide this field
    public string Name { get; set; }

    [EmailAddress]  // Must be a valid email
    public string Email { get; set; }

    [Range(0, 120)]  // Must be between 0 and 120
    public int Age { get; set; }

    [StringLength(10)]  // Maximum 10 characters
    public string City { get; set; }
}
```

### How Validation Works:

```csharp
[HttpPost("/users")]
public string CreateUser(User user)
{
    // Check if the user data is valid
    if (!ModelState.IsValid)
    {
        // If not valid, return error messages
        return "Error: " + string.Join(", ", ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage));
    }

    // If valid, create the user
    return "User created: " + user.Name;
}
```

**Example:**
```
User sends: { name: "", email: "not-an-email", age: 200 }
↓
Validation checks:
  - Name is empty ✗ (Required failed)
  - Email is not valid ✗ (EmailAddress failed)
  - Age is 200 ✗ (Range 0-120 failed)
↓
Result: "Error: Name is required, Email is not valid, Age must be between 0 and 120"
```

### Common Validation Rules:

| Rule | What it checks | Example |
|------|---|---|
| `[Required]` | Field is not empty | Name must be provided |
| `[EmailAddress]` | Valid email format | john@example.com |
| `[Range(min, max)]` | Number between min and max | Age between 0-120 |
| `[StringLength(max)]` | String not longer than max | Name max 100 chars |
| `[Phone]` | Valid phone number | 123-456-7890 |
| `[Url]` | Valid URL | https://example.com |

### Custom Error Messages:

```csharp
public class User
{
    [Required(ErrorMessage = "Please enter your name")]
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Please enter a valid email")]
    public string Email { get; set; }

    [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
    public int Age { get; set; }
}
```


---

## 10. Error Handling (Managing Mistakes)

### What are HTTP Status Codes?
Status codes tell you if something worked or what went wrong. Like traffic lights - green = good, red = problem.

### Common Status Codes:

```
200 OK                  - Request worked perfectly ✓
201 Created             - Resource was created ✓
400 Bad Request         - Something wrong with the data ✗
401 Unauthorized        - You need to login ✗
404 Not Found           - Resource doesn't exist ✗
500 Server Error        - Something broke on the server ✗
```

### Simple Error Handling:

```csharp
[HttpGet("/users/{id}")]
public string GetUser(int id)
{
    // Check if ID is valid
    if (id <= 0)
    {
        return "Error: ID must be greater than 0";
    }

    // Try to get the user
    var user = dbContext.Users.FirstOrDefault(u => u.Id == id);
    
    // Check if user exists
    if (user == null)
    {
        return "Error: User not found";
    }

    // Success!
    return "User: " + user.Name;
}
```

### Better Error Responses (with Status Codes):

```csharp
[HttpGet("/users/{id}")]
public object GetUser(int id)
{
    // Validation error
    if (id <= 0)
    {
        return new { status = 400, error = "ID must be greater than 0" };
    }

    var user = dbContext.Users.FirstOrDefault(u => u.Id == id);
    
    // Not found error
    if (user == null)
    {
        return new { status = 404, error = "User not found" };
    }

    // Success
    return new { status = 200, data = user };
}
```

### Try-Catch (Handle Unexpected Errors):

```csharp
[HttpPost("/users")]
public string CreateUser(User user)
{
    try
    {
        // Try to create the user
        dbContext.Users.Add(user);
        dbContext.SaveChangesAsync();
        
        return "User created successfully";
    }
    catch (Exception ex)
    {
        // If something goes wrong, handle it
        return "Error: " + ex.Message;
    }
}
```

**What happens:**
```
Try: Create user
↓
If successful → "User created successfully"
If error → "Error: [error message]"
```

### Global Error Handler (Catch All Errors):

```csharp
// In Program.cs
app.UseExceptionHandler((errorApp) =>
{
    errorApp.Run(async (context) =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Send error response to user
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Something went wrong",
            message = exception?.Message
        });
    });
});
```

**What happens:**
```
Any unhandled error in your app
↓
Global handler catches it
↓
Sends a nice error message to the user
↓
Prevents app from crashing
```


---

## 11. Testing (Making Sure Code Works)

### What is Testing?
Testing means checking if your code works correctly before users use it. Like test-driving a car before buying it.

### Simple Test Example:

```csharp
// The code you want to test
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

// The test
public class CalculatorTests
{
    [Test]
    public void Add_2Plus3_Returns5()
    {
        // Arrange (setup)
        var calculator = new Calculator();
        
        // Act (do the action)
        var result = calculator.Add(2, 3);
        
        // Assert (check the result)
        Assert.That(result, Is.EqualTo(5));
        // ^ If result is 5, test passes. If not, test fails.
    }
}
```

### Testing Your API:

```csharp
public class UserControllerTests
{
    [Test]
    public void GetUser_ReturnsOkStatus()
    {
        // Arrange
        var controller = new UserController();
        
        // Act
        var result = controller.GetUser(1);
        
        // Assert
        Assert.That(result, Is.Not.Null);
    }
}
```

## Project Structure (How Files are Organized)

A typical .NET project is organized like this:

```
MyApp/
├── Program.cs                    # Application starts here
├── appsettings.json              # Configuration settings
├── Controllers/                  # API endpoints
│   └── UsersController.cs        # Handles /users requests
├── Models/                       # Data structures
│   └── User.cs                   # User class definition
├── Services/                     # Business logic
│   └── UserService.cs            # Logic for users
├── Data/                         # Database
│   └── AppDbContext.cs           # Database connection
└── Tests/                        # Tests
    └── UserServiceTests.cs       # Test the service
```

**Controllers/** - Where API endpoints are defined
```csharp
[HttpGet("/users")]
public List<User> GetAllUsers() { }
```

**Models/** - Data structures (like blueprints)
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

**Services/** - Business logic
```csharp
// The actual work happens here
public class UserService
{
    public User GetUser(int id) { }
}
```

**Data/** - Database access
```csharp
// Talk to the database
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
}
```

**Tests/** - Make sure everything works
```csharp
[Test]
public void GetUser_ReturnsUser() { }
```


---

## Quick Start (Build Your First App in 5 Minutes)

### Step 1: Create a Project
```bash
dotnet new webapi -n MyFirstApp
cd MyFirstApp
```

### Step 2: Create a User Model
Create a file: `Models/User.cs`
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Step 3: Create a Controller
Create a file: `Controllers/UsersController.cs`
```csharp
[HttpGet("/users")]
public string GetUsers()
{
    return "User 1: John, User 2: Alice";
}

[HttpPost("/users")]
public string CreateUser(string name)
{
    return "Created user: " + name;
}
```

### Step 4: Run Your App
```bash
dotnet run
```

### Step 5: Test It
Visit in your browser or use curl:
```bash
# Test GET
curl http://localhost:5000/users

# Test POST
curl -X POST http://localhost:5000/users?name=Bob
```

### Done! 🎉
Your app is running!

```
Your Computer
    ↓
http://localhost:5000
    ↓
Your API receives the request
    ↓
Returns response
    ↓
You see the result
```

---


### Next Steps:

1. **Install .NET SDK** - From dotnet.microsoft.com
2. **Create your first project** - `dotnet new webapi -n MyApp`
3. **Build a simple API** - GET, POST, PUT, DELETE endpoints
4. **Add a database** - Entity Framework with SQL Server
5. **Deploy it** - Docker or Azure

---
