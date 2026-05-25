// ============================================================
// ES6+ JAVASCRIPT COMPLETE GUIDE
// All major concepts in one practical codebase
// ============================================================

// ============================================================
// 1. BASIC SYNTAX
// ============================================================

console.log("=== 1. BASIC SYNTAX ===\n");

// Variables: let, const, var (avoid var in modern JS)
const APP_NAME = "MyApp";  // Constant - cannot be reassigned
let userName = "Alice";     // Block-scoped, can be reassigned
let age = 25;

console.log(`App: ${APP_NAME}, User: ${userName}, Age: ${age}`);

// Template literals (backticks allow multiline and interpolation)
const message = `
Hello ${userName}!
You are ${age} years old.
App name: ${APP_NAME}
`;
console.log(message);

// Arrow functions (ES6)
const greet = (name) => `Hello, ${name}!`;
console.log(greet("Bob"));

// Destructuring - Extract values from objects/arrays
const person = { name: "Charlie", email: "charlie@example.com", city: "NYC" };
const { name, email } = person;  // Extract specific properties
console.log(`Name: ${name}, Email: ${email}`);

const numbers = [1, 2, 3, 4, 5];
const [first, second, ...rest] = numbers;  // rest operator (...)
console.log(`First: ${first}, Second: ${second}, Rest: ${rest}`);

// Spread operator - Expand arrays/objects
const arr1 = [1, 2];
const arr2 = [3, 4];
const combined = [...arr1, ...arr2];
console.log(`Combined array: ${combined}`);

const obj1 = { x: 1 };
const obj2 = { y: 2 };
const mergedObj = { ...obj1, ...obj2 };
console.log("Merged object:", mergedObj);

// ============================================================
// 2. CONTROL FLOWS
// ============================================================

console.log("\n=== 2. CONTROL FLOWS ===\n");

// if/else statements
const score = 85;
if (score >= 90) {
  console.log("Grade: A");
} else if (score >= 80) {
  console.log("Grade: B");
} else if (score >= 70) {
  console.log("Grade: C");
} else {
  console.log("Grade: F");
}

// Switch statement
const day = 3;
switch (day) {
  case 1:
    console.log("Monday");
    break;
  case 2:
    console.log("Tuesday");
    break;
  case 3:
    console.log("Wednesday");
    break;
  default:
    console.log("Other day");
}

// Ternary operator (conditional shorthand)
const status = age >= 18 ? "Adult" : "Minor";
console.log(`Status: ${status}`);

// Logical operators && (AND), || (OR), ! (NOT)
const hasLicense = true;
const hasInsurance = false;
const canDrive = hasLicense && hasInsurance;
console.log(`Can drive: ${canDrive}`);

const isVIP = false;
const isStudent = true;
const getDiscount = isVIP || isStudent;
console.log(`Get discount: ${getDiscount}`);

// for loop
console.log("\nFor loop:");
for (let i = 0; i < 3; i++) {
  console.log(`  Iteration ${i}`);
}

// while loop
console.log("\nWhile loop:");
let counter = 0;
while (counter < 3) {
  console.log(`  Counter: ${counter}`);
  counter++;
}

// do...while loop (runs at least once)
console.log("\nDo-while loop:");
let num = 0;
do {
  console.log(`  Number: ${num}`);
  num++;
} while (num < 3);

// for...of loop (iterate over values)
console.log("\nFor-of loop:");
const fruits = ["apple", "banana", "cherry"];
for (const fruit of fruits) {
  console.log(`  - ${fruit}`);
}

// for...in loop (iterate over keys/indices)
console.log("\nFor-in loop:");
const user = { name: "Diana", age: 30, city: "LA" };
for (const key in user) {
  console.log(`  ${key}: ${user[key]}`);
}

// ============================================================
// 3. FUNCTIONS AND STORING METHODS (Objects & Classes)
// ============================================================

console.log("\n=== 3. FUNCTIONS AND STORING METHODS ===\n");

// Function declaration
function add(a, b) {
  return a + b;
}
console.log(`Add 5 + 3: ${add(5, 3)}`);

// Arrow function (concise syntax)
const multiply = (a, b) => a * b;
console.log(`Multiply 4 * 7: ${multiply(4, 7)}`);

// Default parameters
const sayHello = (name = "Guest") => `Hello, ${name}!`;
console.log(sayHello());
console.log(sayHello("Eve"));

// Rest parameters (collect multiple arguments into an array)
const sum = (...nums) => nums.reduce((acc, num) => acc + num, 0);
console.log(`Sum of 1,2,3,4,5: ${sum(1, 2, 3, 4, 5)}`);

// Storing methods in OBJECTS (Object literal)
const calculator = {
  value: 0,
  
  // Method
  add(num) {
    this.value += num;
    return this;  // Return 'this' for method chaining
  },
  
  // Method
  subtract(num) {
    this.value -= num;
    return this;
  },
  
  // Method
  multiply(num) {
    this.value *= num;
    return this;
  },
  
  // Method
  getResult() {
    return this.value;
  }
};

// Method chaining
const result = calculator.add(10).multiply(2).subtract(5).getResult();
console.log(`Calculator result: ${result}`);

// STORING METHODS in CLASSES (ES6)
class BankAccount {
  constructor(accountHolder, initialBalance) {
    this.accountHolder = accountHolder;
    this.balance = initialBalance;
    this.transactions = [];
  }

  // Instance method
  deposit(amount) {
    if (amount <= 0) {
      throw new Error("Deposit amount must be positive");
    }
    this.balance += amount;
    this.transactions.push({ type: "deposit", amount, date: new Date() });
    return this.balance;
  }

  // Instance method
  withdraw(amount) {
    if (amount > this.balance) {
      throw new Error("Insufficient funds");
    }
    if (amount <= 0) {
      throw new Error("Withdrawal amount must be positive");
    }
    this.balance -= amount;
    this.transactions.push({ type: "withdraw", amount, date: new Date() });
    return this.balance;
  }

  // Instance method
  getBalance() {
    return this.balance;
  }

  // Static method (called on class, not instance)
  static bankName() {
    return "MyBank";
  }
}

const account = new BankAccount("Frank", 1000);
account.deposit(500);
account.withdraw(200);
console.log(`Account holder: ${account.accountHolder}, Balance: ${account.getBalance()}`);
console.log(`Bank name: ${BankAccount.bankName()}`);

// HIGHER-ORDER FUNCTIONS (functions that take/return functions)
const applyOperation = (a, b, operation) => {
  return operation(a, b);
};

console.log(`Apply add: ${applyOperation(10, 5, add)}`);
console.log(`Apply multiply: ${applyOperation(10, 5, multiply)}`);

// Function that returns a function (Closure)
const createMultiplier = (multiplier) => {
  return (num) => num * multiplier;
};

const double = createMultiplier(2);
const triple = createMultiplier(3);
console.log(`Double 5: ${double(5)}`);
console.log(`Triple 5: ${triple(5)}`);

// ============================================================
// 4. POINTERS / REFERENCES
// ============================================================

console.log("\n=== 4. POINTERS / REFERENCES ===\n");

// Primitives (passed by value - copy)
let x = 10;
let y = x;
y = 20;
console.log(`x: ${x}, y: ${y}`);  // x is still 10 (independent copy)

// Objects/Arrays (passed by reference - same memory location)
const obj1 = { value: 10 };
const obj2 = obj1;  // Both point to same object
obj2.value = 20;
console.log(`obj1.value: ${obj1.value}, obj2.value: ${obj2.value}`);  // Both are 20

// Creating independent copy of object (shallow copy)
const obj3 = { ...obj1 };  // Spread operator creates new object
obj3.value = 30;
console.log(`obj1.value: ${obj1.value}, obj3.value: ${obj3.value}`);  // Different now

// Arrays - reference behavior
const array1 = [1, 2, 3];
const array2 = array1;  // Points to same array
array2[0] = 99;
console.log(`array1: ${array1}, array2: ${array2}`);  // Both changed

// Array independent copy
const array3 = [...array1];  // Spread operator creates new array
array3[0] = 100;
console.log(`array1: ${array1}, array3: ${array3}`);  // Different

// Deep copy (for nested objects)
const nested = { user: { name: "Grace", age: 28 } };
const deepCopy = JSON.parse(JSON.stringify(nested));
deepCopy.user.name = "Helen";
console.log(`Original: ${nested.user.name}, Copy: ${deepCopy.user.name}`);

// ============================================================
// 5. EXCEPTION HANDLING
// ============================================================

console.log("\n=== 5. EXCEPTION HANDLING ===\n");

// Try-catch block
try {
  const data = JSON.parse('invalid json');
} catch (error) {
  console.log(`Caught error: ${error.message}`);
}

// Try-catch-finally (finally always runs)
try {
  const result = 10 / 0;  // In JS, this gives Infinity (not error)
  if (!isFinite(result)) {
    throw new Error("Division by zero");
  }
  console.log(`Result: ${result}`);
} catch (error) {
  console.log(`Error caught: ${error.message}`);
} finally {
  console.log("Cleanup code (always runs)");
}

// Custom error class
class ValidationError extends Error {
  constructor(message) {
    super(message);
    this.name = "ValidationError";
  }
}

// Using custom error
function validateEmail(email) {
  if (!email.includes("@")) {
    throw new ValidationError(`Invalid email: ${email}`);
  }
  return true;
}

try {
  validateEmail("invalid-email");
} catch (error) {
  if (error instanceof ValidationError) {
    console.log(`Validation failed: ${error.message}`);
  } else {
    console.log(`Unexpected error: ${error.message}`);
  }
}

// Throwing custom errors in functions
function divide(a, b) {
  try {
    if (b === 0) {
      throw new Error("Cannot divide by zero");
    }
    return a / b;
  } catch (error) {
    console.log(`Error in divide: ${error.message}`);
    return null;
  }
}

console.log(`Divide 10 by 2: ${divide(10, 2)}`);
console.log(`Divide 10 by 0: ${divide(10, 0)}`);

// Promise-based error handling
const fetchData = (success) => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (success) {
        resolve({ data: "Success!", status: 200 });
      } else {
        reject(new Error("Failed to fetch data"));
      }
    }, 100);
  });
};

// Using .catch() for promises
fetchData(true)
  .then((response) => console.log(`Promise resolved: ${response.data}`))
  .catch((error) => console.log(`Promise rejected: ${error.message}`));

// Using async/await (modern error handling)
async function getUserData() {
  try {
    const response = await fetchData(false);
    console.log(`User data: ${response.data}`);
  } catch (error) {
    console.log(`Async error caught: ${error.message}`);
  }
}

getUserData();

// ============================================================
// BONUS: ARRAY METHODS (Essential ES6+ concepts)
// ============================================================

console.log("\n=== BONUS: ARRAY METHODS ===\n");

const items = [1, 2, 3, 4, 5];

// map() - transform each element
const doubled = items.map((item) => item * 2);
console.log(`Doubled: ${doubled}`);

// filter() - keep elements matching condition
const evenNumbers = items.filter((item) => item % 2 === 0);
console.log(`Even numbers: ${evenNumbers}`);

// reduce() - combine into single value
const total = items.reduce((sum, item) => sum + item, 0);
console.log(`Total: ${total}`);

// find() - get first matching element
const firstEven = items.find((item) => item % 2 === 0);
console.log(`First even: ${firstEven}`);

// some() - check if any element matches
const hasLargeNumber = items.some((item) => item > 3);
console.log(`Has number > 3: ${hasLargeNumber}`);

// every() - check if all elements match
const allPositive = items.every((item) => item > 0);
console.log(`All positive: ${allPositive}`);

// ============================================================
// BONUS: DESTRUCTURING ADVANCED
// ============================================================

console.log("\n=== BONUS: ADVANCED DESTRUCTURING ===\n");

// Object destructuring with renaming
const employee = { name: "Ivy", role: "Developer", salary: 80000 };
const { name: empName, role: jobTitle } = employee;
console.log(`${empName} works as ${jobTitle}`);

// Nested destructuring
const company = {
  name: "TechCorp",
  ceo: { name: "Jack", age: 45 }
};
const { ceo: { name: ceoName } } = company;
console.log(`CEO: ${ceoName}`);

console.log("\n=== END OF GUIDE ===");
