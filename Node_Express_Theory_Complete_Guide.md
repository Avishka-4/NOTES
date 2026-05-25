# Node.js + Express.js Theory Explained with Code
## Complete Beginner's Guide

---

## 0. WHAT IS NODE.JS?

### Theory: The Big Picture

**JavaScript** originally ran only in web browsers. **Node.js** lets JavaScript run on your computer's server (backend).

**Before Node.js:**
- Frontend (browser): JavaScript
- Backend (server): PHP, Python, Java, etc.

**After Node.js:**
- Frontend (browser): JavaScript
- Backend (server): JavaScript (Node.js)

### Why This Matters

You can now build **entire applications** with ONE language: JavaScript.

```
Client (Browser)        Server (Node.js)
   JavaScript      →      JavaScript
   React/Vue             Express, database
      ↑                        ↑
      └────────────────────────┘
         Same language!
```

### Code: Your First Node.js Program

```javascript
// hello.js - This runs on SERVER, not in browser

console.log("Hello from Node.js!");
console.log("This runs on the server side");

// Run it:
// node hello.js
// Output:
// Hello from Node.js!
// This runs on the server side
```

### Difference: Node.js vs Browser JavaScript

```javascript
// Browser JavaScript
window.location.href = "/"; // ✅ Can access browser
const element = document.getElementById("id"); // ✅ Can access DOM
fetch("/api/data"); // ✅ Can make requests

// Node.js JavaScript
window.location.href = "/"; // ❌ No window object
const element = document.getElementById("id"); // ❌ No DOM
fetch("/api/data"); // ❌ Use require() instead

// But Node.js has special features
const fs = require('fs'); // ✅ Read/write files
const http = require('http'); // ✅ Create web server
process.env.PORT; // ✅ Access environment variables
```

---

## 1. NODE CORE

### Theory: What is Node Core?

Node comes with built-in modules that let you work with files, paths, events, and more.

### 1.1 File System (fs)

**What it does:** Read, write, delete, and manage files on your server.

```javascript
// Read a file
const fs = require('fs');

// SYNCHRONOUS (blocks code) - avoid in production
const content = fs.readFileSync('data.txt', 'utf8');
console.log(content);

// ASYNCHRONOUS (non-blocking) - better
fs.readFile('data.txt', 'utf8', (error, data) => {
  if (error) {
    console.log("File not found");
    return;
  }
  console.log(data);
});

// PROMISES (modern way)
fs.promises.readFile('data.txt', 'utf8')
  .then(data => console.log(data))
  .catch(error => console.log("Error:", error.message));

// ASYNC/AWAIT (cleanest)
async function readData() {
  try {
    const data = await fs.promises.readFile('data.txt', 'utf8');
    console.log(data);
  } catch (error) {
    console.log("Error:", error.message);
  }
}
readData();
```

### Theory: Why Asynchronous?

**Synchronous (blocking):**
```javascript
// Imagine each line takes time
readFile(); // ⏳ Wait 5 seconds...
console.log("Done"); // Only prints AFTER file is read

// While waiting, server can't handle other requests!
```

**Asynchronous (non-blocking):**
```javascript
readFile(() => {
  console.log("Done"); // Prints when file is ready
});
console.log("Started"); // Prints immediately

// While waiting, server can handle other requests!
```

### Code: Common fs Operations

```javascript
const fs = require('fs');

// WRITE a file
fs.writeFile('message.txt', 'Hello World', (err) => {
  if (err) console.log("Write error");
  else console.log("File written");
});

// APPEND to a file
fs.appendFile('log.txt', 'New log entry\n', (err) => {
  if (err) console.log("Append error");
});

// DELETE a file
fs.unlink('temporary.txt', (err) => {
  if (err) console.log("Delete error");
  else console.log("File deleted");
});

// CHECK if file exists
fs.exists('data.txt', (exists) => {
  console.log(exists ? "File exists" : "File not found");
});

// GET file information
fs.stat('data.txt', (err, stats) => {
  console.log("File size:", stats.size, "bytes");
  console.log("Created:", stats.birthtime);
});

// LIST files in directory
fs.readdir('./', (err, files) => {
  console.log("Files:", files);
});
```

### 1.2 Path Module

**What it does:** Handle file paths correctly for all operating systems.

```javascript
const path = require('path');

// Join paths correctly (works on Windows and Mac/Linux)
const filePath = path.join(__dirname, 'data', 'users.txt');
console.log(filePath);
// Output: /home/user/project/data/users.txt (or C:\... on Windows)

// Get filename from path
const filename = path.basename('/home/user/data/users.txt');
console.log(filename); // "users.txt"

// Get directory from path
const dir = path.dirname('/home/user/data/users.txt');
console.log(dir); // "/home/user/data"

// Get file extension
const ext = path.extname('document.pdf');
console.log(ext); // ".pdf"

// Get absolute path
const absolute = path.resolve('files/data.txt');
console.log(absolute); // Full path like /home/user/project/files/data.txt
```

### 1.3 Events

**What it does:** Create and listen for custom events.

```javascript
const EventEmitter = require('events');

// Create an emitter
const emitter = new EventEmitter();

// LISTEN for an event
emitter.on('userSignup', (userName) => {
  console.log(`User ${userName} signed up!`);
});

// TRIGGER an event
emitter.emit('userSignup', 'Alice');
// Output: User Alice signed up!

// Multiple listeners
emitter.on('userSignup', (userName) => {
  console.log(`Send welcome email to ${userName}`);
});

emitter.emit('userSignup', 'Bob');
// Output:
// User Bob signed up!
// Send welcome email to Bob

// Listen once (auto removes listener)
emitter.once('firstLogin', (user) => {
  console.log(`Welcome ${user}!`);
});

emitter.emit('firstLogin', 'Charlie'); // Prints
emitter.emit('firstLogin', 'David');   // Doesn't print
```

---

## 2. NPM / MODULES

### Theory: What is NPM?

**NPM** (Node Package Manager) is a library of 1+ million code packages. Instead of writing everything yourself, you use pre-built packages.

### Code: Using Packages

```javascript
// Install a package (in terminal)
// npm install express

// Use it in your code
const express = require('express');

// Or modern way (ES6 imports)
import express from 'express';
```

### 2.1 package.json

**What it is:** A file that describes your project and its dependencies.

```json
{
  "name": "my-api",
  "version": "1.0.0",
  "description": "My first Node.js API",
  "main": "server.js",
  "scripts": {
    "start": "node server.js",
    "dev": "nodemon server.js",
    "test": "jest"
  },
  "dependencies": {
    "express": "^4.18.0",
    "mongoose": "^6.0.0",
    "dotenv": "^16.0.0"
  },
  "devDependencies": {
    "nodemon": "^2.0.0"
  }
}
```

**What each part means:**
- `name`: Your project name
- `version`: Current version
- `scripts`: Commands you can run with `npm run`
- `dependencies`: Packages needed for production
- `devDependencies`: Packages only needed for development (like testing tools)

### Code: Running Scripts

```bash
# Run start script
npm start
# Runs: node server.js

# Run custom script
npm run dev
# Runs: nodemon server.js (auto-restarts server on file changes)

# Install a new package
npm install package-name
# Adds to dependencies in package.json

# Install for development only
npm install --save-dev package-name
# Adds to devDependencies
```

### 2.2 CommonJS (require) vs ES6 Modules (import)

```javascript
// CommonJS (older, still widely used)
const express = require('express');
const fs = require('fs');

function myFunction() {
  return "Hello";
}

module.exports = myFunction;

// -----

// ES6 Modules (newer, cleaner)
import express from 'express';
import fs from 'fs';

export function myFunction() {
  return "Hello";
}

// Or as default export
export default myFunction;
```

---

## 3. EXPRESS BASICS

### Theory: What is Express?

Express is a **web framework** that makes building servers easier. It handles:
- Receiving requests from clients
- Routing (different URLs → different code)
- Sending responses back to clients

### Code: Your First Express Server

```javascript
const express = require('express');
const app = express();

// When someone visits localhost:3000/
app.get('/', (req, res) => {
  res.send('Hello World!');
});

// Start the server on port 3000
app.listen(3000, () => {
  console.log('Server running on http://localhost:3000');
});

// Run: node server.js
// Visit: http://localhost:3000/
// See: "Hello World!"
```

### Theory: req, res, and next

- **req** (request): Data coming FROM the client
- **res** (response): What you send BACK to the client
- **next**: Call this to pass control to the next middleware/route

```javascript
// REQUEST has lots of useful properties
app.get('/user/:id', (req, res) => {
  console.log(req.params.id);      // URL parameter
  console.log(req.query.name);     // Query string (example.com?name=Alice)
  console.log(req.body);           // Data sent in body
  console.log(req.headers);        // HTTP headers
  console.log(req.method);         // GET, POST, etc.
  console.log(req.url);            // The URL path
});

// RESPONSE methods
app.get('/test', (req, res) => {
  res.send('Text response');       // Send text
  res.json({ name: 'Alice' });     // Send JSON
  res.status(404).send('Not found'); // With status code
  res.redirect('/home');           // Redirect to another URL
  res.render('view.html');         // Render HTML file
});
```

---

## 4. ROUTING

### Theory: What is Routing?

Routing means **matching URLs to code**. Different URLs run different functions.

```
GET /              → Show homepage
GET /users         → Show all users
GET /users/5       → Show user with ID 5
POST /users        → Create new user
PUT /users/5       → Update user 5
DELETE /users/5    → Delete user 5
```

### Code: Basic Routes

```javascript
const express = require('express');
const app = express();

// GET request
app.get('/', (req, res) => {
  res.send('Home page');
});

// GET with URL parameter
app.get('/users/:id', (req, res) => {
  const userId = req.params.id;
  res.send(`User profile for ID: ${userId}`);
});
// GET /users/5 → "User profile for ID: 5"
// GET /users/123 → "User profile for ID: 123"

// GET with query parameters
app.get('/search', (req, res) => {
  const query = req.query.q;
  const page = req.query.page;
  res.send(`Searching for "${query}" on page ${page}`);
});
// GET /search?q=javascript&page=2 → "Searching for "javascript" on page 2"

// POST request
app.post('/users', (req, res) => {
  res.send('User created!');
});

// PUT request (update)
app.put('/users/:id', (req, res) => {
  res.send(`User ${req.params.id} updated!`);
});

// DELETE request
app.delete('/users/:id', (req, res) => {
  res.send(`User ${req.params.id} deleted!`);
});

app.listen(3000);
```

### Code: Route Parameters vs Query Parameters

```javascript
// URL parameters (part of the URL structure)
app.get('/users/:id', (req, res) => {
  console.log(req.params.id); // From URL path
  // GET /users/5 → params.id = "5"
});

// Query parameters (optional filters)
app.get('/users', (req, res) => {
  console.log(req.query.name);   // From ?name=value
  console.log(req.query.age);    // From ?age=value
  // GET /users?name=Alice&age=25 → query.name = "Alice", query.age = "25"
});
```

### Code: Route Groups (Routers)

As your app grows, put related routes in a separate file:

```javascript
// routes/users.js
const express = require('express');
const router = express.Router();

router.get('/', (req, res) => {
  res.json([{ id: 1, name: 'Alice' }]);
});

router.get('/:id', (req, res) => {
  res.json({ id: req.params.id, name: 'User' });
});

router.post('/', (req, res) => {
  res.json({ message: 'User created' });
});

module.exports = router;

// -----

// server.js
const express = require('express');
const userRoutes = require('./routes/users');
const app = express();

// All routes in userRoutes get /users prefix
app.use('/users', userRoutes);

// GET /users → gets all users
// GET /users/5 → gets user 5
// POST /users → creates user

app.listen(3000);
```

---

## 5. MIDDLEWARE

### Theory: What is Middleware?

Middleware is code that runs **between receiving a request and sending a response**. It can:
- Read/modify the request
- Read/modify the response
- Call the next middleware
- Stop the request (send response without continuing)

```
Request comes in
    ↓
Middleware 1
    ↓
Middleware 2
    ↓
Route handler
    ↓
Response sent out
```

### Code: Understanding Middleware

```javascript
const express = require('express');
const app = express();

// MIDDLEWARE 1: Log all requests
app.use((req, res, next) => {
  console.log(`[${new Date().toLocaleTimeString()}] ${req.method} ${req.url}`);
  next(); // Pass to next middleware
});

// MIDDLEWARE 2: Check if authenticated
app.use((req, res, next) => {
  const isAuthenticated = true; // Your logic
  
  if (isAuthenticated) {
    next(); // Continue to route
  } else {
    res.status(401).send('Not authenticated');
    // Never call next() - stop here
  }
});

// ROUTE HANDLER
app.get('/dashboard', (req, res) => {
  res.send('Welcome to dashboard!');
});

app.listen(3000);
```

### Code: Common Middleware Examples

```javascript
const express = require('express');
const app = express();

// BUILT-IN MIDDLEWARE: Parse JSON in request body
app.use(express.json());
// Now req.body contains parsed JSON

// BUILT-IN MIDDLEWARE: Parse URL-encoded form data
app.use(express.urlencoded({ extended: true }));
// Now req.body contains form data

// CUSTOM MIDDLEWARE: Add user info
app.use((req, res, next) => {
  req.user = { id: 1, name: 'Alice' };
  next();
});

// Now all routes have access to req.user
app.get('/profile', (req, res) => {
  res.json(req.user); // { id: 1, name: 'Alice' }
});

// MIDDLEWARE: Only for specific routes
const checkAuth = (req, res, next) => {
  if (req.headers.authorization) {
    next();
  } else {
    res.status(401).send('Unauthorized');
  }
};

app.get('/public', (req, res) => {
  res.send('Anyone can see this');
});

app.get('/private', checkAuth, (req, res) => {
  res.send('Only authorized users see this');
});

// ERROR MIDDLEWARE: Must be last, with 4 parameters
app.use((error, req, res, next) => {
  console.log('Error:', error);
  res.status(500).json({ message: 'Server error' });
});

app.listen(3000);
```

---

## 6. REST API DESIGN

### Theory: What is REST?

REST (Representational State Transfer) is a style for building APIs. It uses HTTP methods and URLs to represent actions.

```
HTTP Method + URL = Action

GET /users              Read all users
GET /users/5            Read user 5
POST /users             Create new user
PUT /users/5            Update user 5
DELETE /users/5         Delete user 5
```

### Code: RESTful API Example

```javascript
const express = require('express');
const app = express();
app.use(express.json());

// Fake database
let users = [
  { id: 1, name: 'Alice', email: 'alice@example.com' },
  { id: 2, name: 'Bob', email: 'bob@example.com' }
];

// GET - Read all
app.get('/api/users', (req, res) => {
  res.json(users);
});

// GET - Read one
app.get('/api/users/:id', (req, res) => {
  const user = users.find(u => u.id === parseInt(req.params.id));
  if (!user) {
    return res.status(404).json({ error: 'User not found' });
  }
  res.json(user);
});

// POST - Create
app.post('/api/users', (req, res) => {
  const { name, email } = req.body;
  
  if (!name || !email) {
    return res.status(400).json({ error: 'Name and email required' });
  }
  
  const newUser = {
    id: users.length + 1,
    name,
    email
  };
  
  users.push(newUser);
  res.status(201).json(newUser); // 201 = Created
});

// PUT - Update
app.put('/api/users/:id', (req, res) => {
  const user = users.find(u => u.id === parseInt(req.params.id));
  if (!user) {
    return res.status(404).json({ error: 'User not found' });
  }
  
  if (req.body.name) user.name = req.body.name;
  if (req.body.email) user.email = req.body.email;
  
  res.json(user);
});

// DELETE - Delete
app.delete('/api/users/:id', (req, res) => {
  const index = users.findIndex(u => u.id === parseInt(req.params.id));
  if (index === -1) {
    return res.status(404).json({ error: 'User not found' });
  }
  
  const deleted = users.splice(index, 1);
  res.json(deleted[0]);
});

app.listen(3000);
```

### Theory: HTTP Status Codes

```
2xx - Success
  200 OK                - Request successful
  201 Created           - Resource created
  204 No Content        - Success but no data to return

3xx - Redirection
  301 Moved Permanently - Resource moved to new URL
  307 Temporary Redirect

4xx - Client Error
  400 Bad Request       - Invalid data sent
  401 Unauthorized      - Not authenticated
  403 Forbidden         - Authenticated but not allowed
  404 Not Found         - Resource doesn't exist

5xx - Server Error
  500 Internal Server Error - Server error
  503 Service Unavailable   - Server temporarily down
```

### Code: Using Status Codes

```javascript
app.get('/users/:id', (req, res) => {
  const user = findUser(req.params.id);
  
  if (!user) {
    return res.status(404).json({ error: 'Not found' });
  }
  
  res.status(200).json(user); // 200 is default, optional
});

app.post('/users', (req, res) => {
  if (!req.body.name) {
    return res.status(400).json({ error: 'Bad request' });
  }
  
  const user = createUser(req.body);
  res.status(201).json(user); // 201 = Created
});
```

---

## 7. DATABASES

### Theory: What is a Database?

A database stores data permanently. Without a database, your data disappears when the server restarts.

**Common databases:**
- **SQL**: PostgreSQL, MySQL, SQL Server
  - Tables with rows and columns
  - Structured data
  
- **NoSQL**: MongoDB
  - Collections with documents (JSON-like)
  - Flexible structure

### 7.1 MongoDB with Mongoose

Mongoose makes MongoDB easier to use in Node.js.

```javascript
// Install: npm install mongoose

const mongoose = require('mongoose');

// Connect to database
mongoose.connect('mongodb://localhost:27017/myapp', {
  useNewUrlParser: true,
  useUnifiedTopology: true
});

// Define schema (structure of data)
const userSchema = new mongoose.Schema({
  name: { type: String, required: true },
  email: { type: String, required: true },
  age: { type: Number },
  createdAt: { type: Date, default: Date.now }
});

// Create model from schema
const User = mongoose.model('User', userSchema);

// CREATE
async function createUser() {
  const user = new User({
    name: 'Alice',
    email: 'alice@example.com',
    age: 25
  });
  
  await user.save();
  console.log('User created:', user);
}

// READ
async function getUsers() {
  const users = await User.find(); // All users
  const user = await User.findById('user-id'); // Specific user
  const user = await User.findOne({ email: 'alice@example.com' }); // Find by property
  console.log(users);
}

// UPDATE
async function updateUser() {
  const user = await User.findByIdAndUpdate(
    'user-id',
    { name: 'Bob', age: 30 },
    { new: true } // Return updated user
  );
  console.log('User updated:', user);
}

// DELETE
async function deleteUser() {
  const user = await User.findByIdAndDelete('user-id');
  console.log('User deleted:', user);
}
```

### 7.2 Using Database in API

```javascript
const express = require('express');
const mongoose = require('mongoose');
const app = express();

app.use(express.json());

// Connection
mongoose.connect('mongodb://localhost:27017/api', {
  useNewUrlParser: true,
  useUnifiedTopology: true
});

// Schema
const userSchema = new mongoose.Schema({
  name: { type: String, required: true },
  email: { type: String, required: true }
});

const User = mongoose.model('User', userSchema);

// GET all users
app.get('/api/users', async (req, res) => {
  try {
    const users = await User.find();
    res.json(users);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// GET one user
app.get('/api/users/:id', async (req, res) => {
  try {
    const user = await User.findById(req.params.id);
    if (!user) return res.status(404).json({ error: 'Not found' });
    res.json(user);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// POST - Create user
app.post('/api/users', async (req, res) => {
  try {
    const user = new User(req.body);
    await user.save();
    res.status(201).json(user);
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// PUT - Update user
app.put('/api/users/:id', async (req, res) => {
  try {
    const user = await User.findByIdAndUpdate(
      req.params.id,
      req.body,
      { new: true, runValidators: true }
    );
    if (!user) return res.status(404).json({ error: 'Not found' });
    res.json(user);
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// DELETE user
app.delete('/api/users/:id', async (req, res) => {
  try {
    const user = await User.findByIdAndDelete(req.params.id);
    if (!user) return res.status(404).json({ error: 'Not found' });
    res.json({ message: 'User deleted' });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

app.listen(3000, () => {
  console.log('Server running on port 3000');
});
```

---

## 8. AUTHENTICATION & AUTHORIZATION

### Theory: The Difference

- **Authentication**: "Who are you?" (login)
- **Authorization**: "Can you do this?" (permissions)

```
Login with credentials → Authenticated → Can access protected routes
Check if admin → Authorized → Can access admin-only routes
```

### 8.1 Passwords: Hashing with bcrypt

Never store passwords as plain text!

```javascript
// Install: npm install bcrypt

const bcrypt = require('bcrypt');

// HASH password when user signs up
async function hashPassword(plainPassword) {
  const hashedPassword = await bcrypt.hash(plainPassword, 10);
  // 10 = salt rounds (higher = slower but more secure)
  console.log('Hashed:', hashedPassword);
  return hashedPassword;
}

// COMPARE password when user logs in
async function checkPassword(plainPassword, hashedPassword) {
  const isCorrect = await bcrypt.compare(plainPassword, hashedPassword);
  console.log('Password correct:', isCorrect);
  return isCorrect;
}

// Example:
hashPassword('mypassword123'); // → $2b$10$...
// User saves hashed version to database
// When logging in, user sends 'mypassword123'
// You compare with saved hash
```

### 8.2 JWT (JSON Web Tokens)

JWT is a way to send authenticated users without sessions.

```javascript
// Install: npm install jsonwebtoken

const jwt = require('jsonwebtoken');

const SECRET = 'your-secret-key';

// CREATE token when user logs in
function createToken(userId) {
  const token = jwt.sign(
    { id: userId },      // Payload
    SECRET,              // Secret key
    { expiresIn: '24h' } // Expires in 24 hours
  );
  return token;
}

// VERIFY token when user sends it
function verifyToken(token) {
  try {
    const decoded = jwt.verify(token, SECRET);
    return decoded; // { id: userId }
  } catch (error) {
    return null; // Invalid token
  }
}

// Example
const token = createToken(123);
console.log('Token:', token); // eyJhbGciOiJIUzI1NiIs...

const decoded = verifyToken(token);
console.log('User ID:', decoded.id); // 123
```

### 8.3 Authentication Middleware

```javascript
const express = require('express');
const jwt = require('jsonwebtoken');
const bcrypt = require('bcrypt');
const app = express();

app.use(express.json());

const SECRET = 'your-secret-key';
let users = [];

// MIDDLEWARE: Verify token
const authenticate = (req, res, next) => {
  const token = req.headers.authorization?.split(' ')[1];
  // Get token from: "Bearer <token>"
  
  if (!token) {
    return res.status(401).json({ error: 'No token' });
  }
  
  try {
    const decoded = jwt.verify(token, SECRET);
    req.userId = decoded.id;
    next();
  } catch (error) {
    res.status(401).json({ error: 'Invalid token' });
  }
};

// SIGNUP
app.post('/api/signup', async (req, res) => {
  const { email, password } = req.body;
  
  const hashedPassword = await bcrypt.hash(password, 10);
  const user = { id: Date.now(), email, password: hashedPassword };
  users.push(user);
  
  res.json({ message: 'User created' });
});

// LOGIN
app.post('/api/login', async (req, res) => {
  const { email, password } = req.body;
  
  const user = users.find(u => u.email === email);
  if (!user) return res.status(401).json({ error: 'Invalid email/password' });
  
  const isCorrect = await bcrypt.compare(password, user.password);
  if (!isCorrect) return res.status(401).json({ error: 'Invalid email/password' });
  
  const token = jwt.sign({ id: user.id }, SECRET, { expiresIn: '24h' });
  res.json({ token }); // Send token to client
});

// PROTECTED ROUTE
app.get('/api/profile', authenticate, (req, res) => {
  const user = users.find(u => u.id == req.userId);
  res.json(user);
});

app.listen(3000);
```

---

## 9. ERROR HANDLING

### Theory: Why Error Handling?

Without error handling, one mistake crashes your entire server. Good error handling keeps your server running.

### Code: Try-Catch

```javascript
async function example() {
  try {
    // Code that might fail
    const data = await fetch('/api/users');
    const result = await data.json();
    console.log(result);
  } catch (error) {
    // Runs if error occurs
    console.log('Error:', error.message);
  } finally {
    // Runs regardless
    console.log('Done');
  }
}
```

### Code: Error Handling in Express

```javascript
const express = require('express');
const app = express();

// ROUTE WITH ERROR HANDLING
app.get('/api/users/:id', async (req, res) => {
  try {
    const user = await User.findById(req.params.id);
    if (!user) {
      return res.status(404).json({ error: 'User not found' });
    }
    res.json(user);
  } catch (error) {
    console.log('Error:', error);
    res.status(500).json({ error: 'Server error' });
  }
});

// GLOBAL ERROR HANDLER (must be last)
app.use((error, req, res, next) => {
  console.log('Unhandled error:', error);
  
  res.status(error.status || 500).json({
    error: {
      message: error.message || 'Server error',
      status: error.status || 500
    }
  });
});

app.listen(3000);
```

### Code: Async Error Wrapper

```javascript
// Create a wrapper to catch async errors automatically
const asyncHandler = (fn) => (req, res, next) => {
  Promise.resolve(fn(req, res, next)).catch(next);
};

// Use it
app.get('/api/users/:id', asyncHandler(async (req, res) => {
  const user = await User.findById(req.params.id);
  res.json(user); // Errors are automatically caught
}));
```

---

## 10. VALIDATION

### Theory: Always Validate Input

Never trust user input! Validate everything coming from clients.

### Code: Basic Validation

```javascript
app.post('/api/users', (req, res) => {
  const { name, email, age } = req.body;
  
  // Check if fields exist
  if (!name || !email) {
    return res.status(400).json({ error: 'Name and email required' });
  }
  
  // Check if email is valid
  if (!email.includes('@')) {
    return res.status(400).json({ error: 'Invalid email' });
  }
  
  // Check if age is a number
  if (age && isNaN(age)) {
    return res.status(400).json({ error: 'Age must be a number' });
  }
  
  // All valid
  res.json({ message: 'User created' });
});
```

### Code: Using Joi for Validation

```javascript
// Install: npm install joi

const Joi = require('joi');
const express = require('express');
const app = express();

app.use(express.json());

// Define validation schema
const userSchema = Joi.object({
  name: Joi.string().min(2).max(30).required(),
  email: Joi.string().email().required(),
  age: Joi.number().min(0).max(120)
});

// Validation middleware
const validate = (schema) => {
  return (req, res, next) => {
    const { error } = schema.validate(req.body);
    
    if (error) {
      return res.status(400).json({ error: error.details[0].message });
    }
    
    next();
  };
};

// Use validation
app.post('/api/users', validate(userSchema), async (req, res) => {
  // Data is now guaranteed to be valid
  const user = new User(req.body);
  await user.save();
  res.status(201).json(user);
});

app.listen(3000);
```

### Code: Using express-validator

```javascript
// Install: npm install express-validator

const { body, validationResult } = require('express-validator');
const express = require('express');
const app = express();

app.use(express.json());

app.post('/api/users',
  // Validation rules
  body('name').isLength({ min: 2 }).trim().escape(),
  body('email').isEmail(),
  body('age').isInt({ min: 0, max: 120 }),
  
  // Handle validation errors
  (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
      return res.status(400).json({ errors: errors.array() });
    }
    
    // Validation passed
    res.json({ message: 'User created' });
  }
);

app.listen(3000);
```

---

## 11. FILE UPLOADS

### Theory: Handling File Uploads

Users can upload files. You need to:
1. Receive the file
2. Validate it
3. Save it somewhere (filesystem or cloud storage)

### Code: Using Multer

```javascript
// Install: npm install multer

const express = require('express');
const multer = require('multer');
const path = require('path');
const app = express();

// Configure storage
const storage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, 'uploads/'); // Save to 'uploads' folder
  },
  filename: (req, file, cb) => {
    const uniqueSuffix = Date.now() + '-' + Math.round(Math.random() * 1E9);
    cb(null, file.fieldname + '-' + uniqueSuffix + path.extname(file.originalname));
  }
});

const upload = multer({ storage });

// Upload route
app.post('/api/upload', upload.single('file'), (req, res) => {
  if (!req.file) {
    return res.status(400).json({ error: 'No file uploaded' });
  }
  
  res.json({
    message: 'File uploaded',
    filename: req.file.filename,
    path: req.file.path
  });
});

// Access uploaded file
app.get('/uploads/:filename', (req, res) => {
  const filepath = path.join(__dirname, 'uploads', req.params.filename);
  res.download(filepath);
});

app.listen(3000);
```

### Code: Validate File Type

```javascript
const upload = multer({
  storage,
  fileFilter: (req, file, cb) => {
    // Only allow images
    const allowedMimes = ['image/jpeg', 'image/png', 'image/gif'];
    
    if (allowedMimes.includes(file.mimetype)) {
      cb(null, true);
    } else {
      cb(new Error('Only image files are allowed'));
    }
  },
  limits: {
    fileSize: 5 * 1024 * 1024 // 5MB max
  }
});
```

---

## 12. SECURITY

### Theory: Common Security Issues

### 12.1 CORS (Cross-Origin Resource Sharing)

Control which websites can access your API.

```javascript
// Install: npm install cors

const cors = require('cors');
const express = require('express');
const app = express();

// Allow all origins
app.use(cors());

// Allow specific origins
app.use(cors({
  origin: ['http://localhost:3000', 'https://example.com'],
  credentials: true
}));

// CORS manually
app.use((req, res, next) => {
  res.header('Access-Control-Allow-Origin', 'https://example.com');
  res.header('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
  res.header('Access-Control-Allow-Headers', 'Content-Type');
  next();
});

app.listen(3000);
```

### 12.2 Rate Limiting

Prevent brute force attacks by limiting requests per IP.

```javascript
// Install: npm install express-rate-limit

const rateLimit = require('express-rate-limit');
const express = require('express');
const app = express();

// Limit login attempts
const loginLimiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 minutes
  max: 5, // 5 requests per IP
  message: 'Too many login attempts, try again later'
});

app.post('/api/login', loginLimiter, (req, res) => {
  // Login logic
  res.json({ message: 'Logged in' });
});

// Global rate limit
const globalLimiter = rateLimit({
  windowMs: 1 * 60 * 1000, // 1 minute
  max: 100 // 100 requests per minute per IP
});

app.use(globalLimiter);

app.listen(3000);
```

### 12.3 Helmet (Security Headers)

```javascript
// Install: npm install helmet

const helmet = require('helmet');
const express = require('express');
const app = express();

// Add security headers
app.use(helmet());

// Customize
app.use(helmet({
  contentSecurityPolicy: {
    directives: {
      defaultSrc: ["'self'"],
      styleSrc: ["'self'", "'unsafe-inline'"]
    }
  }
}));

app.listen(3000);
```

### 12.4 Environment Variables

Never hardcode secrets!

```javascript
// Install: npm install dotenv

// .env file (don't commit to git!)
DATABASE_URL=mongodb://localhost:27017/myapp
JWT_SECRET=super-secret-key
API_KEY=abc123

// server.js
require('dotenv').config();

const dbUrl = process.env.DATABASE_URL;
const secret = process.env.JWT_SECRET;
const apiKey = process.env.API_KEY;

mongoose.connect(dbUrl);
const token = jwt.sign({ id: 1 }, secret);
```

---

## 13. COMPLETE EXAMPLE: FULL API

Here's a real API combining everything:

```javascript
const express = require('express');
const mongoose = require('mongoose');
const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');
const cors = require('cors');
const helmet = require('helmet');
require('dotenv').config();

const app = express();

// Middleware
app.use(helmet());
app.use(cors());
app.use(express.json());

// Database Connection
mongoose.connect(process.env.DATABASE_URL);

// Schema
const userSchema = new mongoose.Schema({
  name: { type: String, required: true },
  email: { type: String, required: true, unique: true },
  password: { type: String, required: true },
  createdAt: { type: Date, default: Date.now }
});

const User = mongoose.model('User', userSchema);

// Authenticate middleware
const authenticate = (req, res, next) => {
  const token = req.headers.authorization?.split(' ')[1];
  if (!token) return res.status(401).json({ error: 'No token' });
  
  try {
    const decoded = jwt.verify(token, process.env.JWT_SECRET);
    req.userId = decoded.id;
    next();
  } catch (error) {
    res.status(401).json({ error: 'Invalid token' });
  }
};

// Routes
app.post('/api/signup', async (req, res) => {
  try {
    const { name, email, password } = req.body;
    
    if (!name || !email || !password) {
      return res.status(400).json({ error: 'All fields required' });
    }
    
    const hashedPassword = await bcrypt.hash(password, 10);
    const user = new User({ name, email, password: hashedPassword });
    await user.save();
    
    res.status(201).json({ message: 'User created' });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

app.post('/api/login', async (req, res) => {
  try {
    const { email, password } = req.body;
    
    const user = await User.findOne({ email });
    if (!user) return res.status(401).json({ error: 'Invalid email/password' });
    
    const isCorrect = await bcrypt.compare(password, user.password);
    if (!isCorrect) return res.status(401).json({ error: 'Invalid email/password' });
    
    const token = jwt.sign({ id: user._id }, process.env.JWT_SECRET, { expiresIn: '24h' });
    res.json({ token });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

app.get('/api/profile', authenticate, async (req, res) => {
  try {
    const user = await User.findById(req.userId).select('-password');
    res.json(user);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

app.get('/api/users', async (req, res) => {
  try {
    const users = await User.find().select('-password');
    res.json(users);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Error handler
app.use((error, req, res, next) => {
  console.log('Error:', error);
  res.status(500).json({ error: 'Server error' });
});

app.listen(process.env.PORT || 3000, () => {
  console.log('Server running');
});
```

---

## QUICK REFERENCE

| Topic | What it does | Example |
|-------|-------------|---------|
| Node Core (fs) | Read/write files | `fs.readFile('data.txt')` |
| Path Module | Handle file paths | `path.join(__dirname, 'file.txt')` |
| Events | Create custom events | `emitter.emit('event', data)` |
| NPM | Install packages | `npm install express` |
| Express | Build web server | `app.get('/', (req, res) => {})` |
| Routing | Match URLs | `app.get('/users/:id')` |
| Middleware | Code between request and response | `app.use((req, res, next) => {})` |
| REST API | RESTful design | GET, POST, PUT, DELETE |
| MongoDB | Store data | `User.findById(id)` |
| JWT | Authentication tokens | `jwt.sign({ id }, secret)` |
| Bcrypt | Hash passwords | `bcrypt.hash(password, 10)` |
| Error Handling | Catch errors | `try { } catch (error) { }` |
| Validation | Check user input | `body('email').isEmail()` |
| Multer | Upload files | `upload.single('file')` |
| Helmet | Security headers | `app.use(helmet())` |
| CORS | Control access | `app.use(cors())` |

