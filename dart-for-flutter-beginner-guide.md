# 🎯 Dart for Flutter — Beginner to Practical Guide

> **Purpose:** Learn the Dart you actually need to read, write, debug, and maintain Flutter applications.
>
> You already understand general programming concepts, so this guide focuses on **Dart syntax, Dart habits, and how Dart appears inside Flutter** rather than teaching programming from zero.

---

# 1. Dart and Flutter: understand the difference first

```text
DART
│
├── Programming language
├── Variables
├── Functions
├── Classes
├── Objects
├── Lists / Maps
├── async / await
└── Null safety
        │
        ▼
FLUTTER
│
├── UI framework
├── Widgets
├── Screens
├── Navigation
├── Forms
├── State
├── Animations
└── Mobile app
```

A simple comparison:

| Technology | Role |
|---|---|
| Dart | Programming language |
| Flutter | Framework/UI toolkit |
| Android Studio / VS Code | Development environment |
| Gradle | Android build system used underneath Flutter's Android project |
| APK | Installable Android package |
| AAB | Android App Bundle for Play Store distribution |

You write:

```dart
String name = 'Alex';
```

because that is **Dart**.

You write:

```dart
Text('Hello')
```

because `Text` is a **Flutter widget written in Dart**.

---

# 2. The minimum Dart you need for Flutter

For Flutter development, become comfortable with:

```text
Variables
Data types
final / const
Functions
Named parameters
Classes
Constructors
Objects
Lists
Maps
Null safety
Enums
Inheritance
Interfaces
Mixins — later
Futures
async / await
Streams — later
Exceptions
Imports
Generics
Records / pattern matching — useful, but not first priority
```

You do **not** need to memorize the entire Dart language before starting Flutter.

---

# 3. Your first Dart program

```dart
void main() {
  print('Hello Dart');
}
```

Every normal standalone Dart program starts from:

```dart
main()
```

Flutter also begins from `main()`:

```dart
void main() {
  runApp(const MyApp());
}
```

So:

```text
main()
   ↓
runApp()
   ↓
Flutter starts the widget tree
```

---

# 4. Comments

Single line:

```dart
// This is a comment.
```

Multi-line:

```dart
/*
  This is
  a multi-line comment.
*/
```

Documentation comment:

```dart
/// Returns the user's display name.
String getName() {
  return 'Alex';
}
```

Use comments to explain **why**, not to narrate obvious code.

Bad:

```dart
// Set count to zero.
int count = 0;
```

Useful:

```dart
// Start at page 0 because PageView uses zero-based indexing.
int currentPage = 0;
```

---

# 5. Variables

## Explicit type

```dart
String name = 'Alex';
int age = 22;
double price = 99.50;
bool isActive = true;
```

## Type inference with `var`

```dart
var name = 'Alex';
var age = 22;
```

Dart understands:

```text
name → String
age  → int
```

After inference:

```dart
var age = 22;
```

you cannot later do:

```dart
age = 'twenty two';
```

because `age` was inferred as `int`.

---

# 6. `final`, `const`, and `var`

This is very important in Flutter.

## `var`

Value can be reassigned:

```dart
var count = 1;
count = 2;
```

## `final`

Value is assigned once.

```dart
final username = 'Alex';
```

Later:

```dart
username = 'Sam'; // ❌ Not allowed
```

`final` can receive a value at runtime:

```dart
final currentTime = DateTime.now();
```

## `const`

Compile-time constant:

```dart
const pi = 3.14159;
const appName = 'Gem App';
```

You cannot do:

```dart
const currentTime = DateTime.now(); // ❌
```

because `DateTime.now()` is only known at runtime.

### Flutter connection

You will constantly see:

```dart
const Text('Hello')
```

and:

```dart
const SizedBox(height: 16)
```

Using `const` where possible allows Flutter to reuse immutable widget configurations more efficiently and makes your intent explicit.

### Easy memory rule

```text
var   → can change
final → set once at runtime
const → known and fixed at compile time
```

---

# 7. Basic data types

## String

```dart
String name = 'Alex';
```

Both work:

```dart
'Hello'
"Hello"
```

## Integer

```dart
int quantity = 5;
```

## Double

```dart
double price = 125.50;
```

## Boolean

```dart
bool isLoggedIn = true;
```

## num

Can hold either an integer or double:

```dart
num value = 10;
value = 10.5;
```

Usually prefer the more specific `int` or `double` when you know it.

---

# 8. String interpolation

Instead of:

```dart
String message = 'Hello ' + name;
```

Dart commonly uses:

```dart
String message = 'Hello $name';
```

Expressions:

```dart
print('Total: ${price * quantity}');
```

Flutter example:

```dart
Text('Welcome $username')
```

This will appear everywhere.

---

# 9. Operators

Arithmetic:

```dart
+   -   *   /   %
```

Example:

```dart
int total = 10 + 5;
```

Comparison:

```dart
==  !=  >  <  >=  <=
```

Logical:

```dart
&&   ||   !
```

Example:

```dart
if (isLoggedIn && isActive) {
  print('Allowed');
}
```

Null-aware operators will be covered later because they are especially important in Flutter.

---

# 10. `if`, `else if`, `else`

```dart
if (age >= 18) {
  print('Adult');
} else {
  print('Minor');
}
```

Multiple conditions:

```dart
if (score >= 75) {
  print('A');
} else if (score >= 65) {
  print('B');
} else {
  print('C');
}
```

Flutter example:

```dart
if (isLoading) {
  return const CircularProgressIndicator();
}

return const Text('Loaded');
```

---

# 11. Conditional expression

Short `if/else`:

```dart
String status = isActive ? 'Active' : 'Inactive';
```

Flutter:

```dart
Text(isLoggedIn ? 'Logout' : 'Login')
```

Use it for short conditions only.

Do not turn very complicated logic into giant nested ternary expressions.

---

# 12. `switch`

```dart
switch (role) {
  case 'admin':
    print('Admin panel');
  case 'user':
    print('User panel');
  default:
    print('Unknown role');
}
```

Modern Dart also supports expressive switch patterns, but basic `switch` understanding is enough to begin Flutter.

---

# 13. Loops

## `for`

```dart
for (int i = 0; i < 5; i++) {
  print(i);
}
```

## `for-in`

Very common:

```dart
final names = ['Alex', 'Sam', 'Maya'];

for (final name in names) {
  print(name);
}
```

## `while`

```dart
int count = 0;

while (count < 5) {
  count++;
}
```

In Flutter UI code, you will often use collection operations such as `.map()` rather than manually building widgets with traditional loops.

---

# 14. Functions

Basic function:

```dart
void greet() {
  print('Hello');
}
```

Return a value:

```dart
int add(int a, int b) {
  return a + b;
}
```

Use:

```dart
final answer = add(5, 3);
```

---

# 15. Arrow functions

Short functions can use:

```dart
int add(int a, int b) => a + b;
```

Equivalent to:

```dart
int add(int a, int b) {
  return a + b;
}
```

Flutter code often uses arrow functions:

```dart
onPressed: () => print('Pressed')
```

---

# 16. Anonymous functions

A function can exist without a name:

```dart
() {
  print('Hello');
}
```

You constantly see these in Flutter:

```dart
ElevatedButton(
  onPressed: () {
    print('Button pressed');
  },
  child: const Text('Press Me'),
)
```

This means:

```text
onPressed
   ↓
receive a function
   ↓
Flutter calls it when button is pressed
```

---

# 17. Function parameters

## Positional parameters

```dart
void greet(String name, int age) {
  print('$name is $age');
}

greet('Alex', 22);
```

Order matters.

---

# 18. Named parameters — extremely important in Flutter

```dart
void createUser({
  required String name,
  required int age,
}) {
  print('$name - $age');
}
```

Call:

```dart
createUser(
  name: 'Alex',
  age: 22,
);
```

Flutter constructors use named parameters heavily:

```dart
Container(
  width: 200,
  height: 100,
  padding: const EdgeInsets.all(16),
)
```

This makes UI code readable.

---

# 19. Optional named parameters

```dart
void greet({
  required String name,
  String? message,
}) {
  print('$name - $message');
}
```

`message` may be null.

Another option:

```dart
void greet({
  required String name,
  String message = 'Hello',
}) {
  print('$message $name');
}
```

Now it has a default.

---

# 20. Lists

A list is an ordered collection.

```dart
List<String> names = [
  'Alex',
  'Maya',
  'Sam',
];
```

Or:

```dart
final names = <String>[
  'Alex',
  'Maya',
  'Sam',
];
```

Access:

```dart
print(names[0]);
```

Add:

```dart
names.add('Nina');
```

Remove:

```dart
names.remove('Sam');
```

Length:

```dart
print(names.length);
```

---

# 21. `.map()` — essential for Flutter

Suppose:

```dart
final names = ['Alex', 'Maya', 'Sam'];
```

You can transform each item:

```dart
final upperNames = names.map((name) {
  return name.toUpperCase();
}).toList();
```

Flutter:

```dart
Column(
  children: names.map((name) {
    return Text(name);
  }).toList(),
)
```

Flow:

```text
List<String>
   ↓ map()
each String
   ↓
Text widget
   ↓
List<Widget>
```

You will use this constantly.

---

# 22. Useful list methods

```dart
final numbers = [1, 2, 3, 4, 5];
```

Filter:

```dart
final even = numbers.where((number) => number.isEven).toList();
```

Find:

```dart
final firstLarge = numbers.firstWhere((number) => number > 3);
```

Check:

```dart
final hasFive = numbers.contains(5);
```

Any:

```dart
final hasLarge = numbers.any((number) => number > 4);
```

Every:

```dart
final allPositive = numbers.every((number) => number > 0);
```

---

# 23. Sets

A set contains unique values.

```dart
Set<String> tags = {
  'ruby',
  'sapphire',
  'emerald',
};
```

Duplicate values are not retained like separate list entries.

Useful when uniqueness matters.

---

# 24. Maps

A map stores key-value pairs.

```dart
Map<String, dynamic> user = {
  'name': 'Alex',
  'age': 22,
  'verified': true,
};
```

Read:

```dart
print(user['name']);
```

This is especially important because JSON API responses often become Dart maps.

JSON:

```json
{
  "name": "Alex",
  "age": 22
}
```

Dart often sees something similar to:

```dart
Map<String, dynamic>
```

---

# 25. `dynamic`

Example:

```dart
dynamic value = 'Hello';
value = 10;
value = true;
```

It turns off much of Dart's static type protection.

You will often see:

```dart
Map<String, dynamic>
```

because JSON values can have different types.

But do **not** make your entire app use `dynamic`.

Prefer strongly typed models.

---

# 26. Null safety — one of Dart's most important ideas

Dart is null-safe.

This:

```dart
String name = 'Alex';
```

means:

```text
name MUST contain a String
name CANNOT be null
```

This is invalid:

```dart
String name = null; // ❌
```

If null is allowed:

```dart
String? name;
```

The `?` means:

```text
String OR null
```

---

# 27. Null-aware access `?.`

Suppose:

```dart
String? name;
```

This can be unsafe:

```dart
print(name.length);
```

because `name` could be null.

Use:

```dart
print(name?.length);
```

Meaning:

```text
if name exists → get length
if name is null → result is null
```

---

# 28. Null fallback `??`

```dart
String? username;
```

Use:

```dart
final displayName = username ?? 'Guest';
```

Meaning:

```text
username exists?
   ├─ yes → use username
   └─ no  → use 'Guest'
```

Flutter:

```dart
Text(user.profileName ?? 'Unnamed User')
```

---

# 29. Null assertion `!`

```dart
String? name;
```

This:

```dart
name!
```

means:

> "I guarantee this is not null."

If you are wrong, the app can crash.

Avoid casually adding `!` just to silence compiler errors.

Bad habit:

```dart
user!.profile!.image!.url!
```

Better:

- model nullability accurately
- check for null
- provide a fallback
- make required data non-nullable where appropriate

---

# 30. `late`

Sometimes a non-null value will be assigned after object creation.

```dart
late String token;
```

Later:

```dart
token = 'abc123';
```

Then:

```dart
print(token);
```

If you read a `late` variable before assigning it, you can get a runtime error.

Use `late` intentionally, not as a universal fix for null-safety errors.

---

# 31. Classes

Flutter is heavily class-based.

```dart
class User {
  String name;
  int age;

  User(this.name, this.age);
}
```

Create object:

```dart
final user = User('Alex', 22);
```

Use:

```dart
print(user.name);
```

---

# 32. Constructors

Flutter uses constructors everywhere.

Long form:

```dart
class User {
  final String name;
  final int age;

  User(String name, int age)
      : name = name,
        age = age;
}
```

Dart shorthand:

```dart
class User {
  final String name;
  final int age;

  User(this.name, this.age);
}
```

Named parameters:

```dart
class User {
  final String name;
  final int age;

  User({
    required this.name,
    required this.age,
  });
}
```

Create:

```dart
final user = User(
  name: 'Alex',
  age: 22,
);
```

This style closely matches Flutter widget constructors.

---

# 33. Why Flutter code looks like nested constructors

Example:

```dart
Container(
  padding: const EdgeInsets.all(16),
  child: Text(
    'Hello',
    style: const TextStyle(
      fontSize: 20,
      fontWeight: FontWeight.bold,
    ),
  ),
)
```

This is simply Dart object construction.

Think:

```text
TextStyle(...)
     ↓ given to
Text(...)
     ↓ given to
Container(...)
```

Flutter UI can look unusual at first, but it is mostly:

```text
classes + constructors + named parameters
```

---

# 34. Methods

A function inside a class is a method.

```dart
class Counter {
  int value = 0;

  void increment() {
    value++;
  }
}
```

Use:

```dart
final counter = Counter();

counter.increment();

print(counter.value);
```

---

# 35. Getters

```dart
class Product {
  final double price;
  final int quantity;

  Product({
    required this.price,
    required this.quantity,
  });

  double get total => price * quantity;
}
```

Use:

```dart
print(product.total);
```

No parentheses are needed because it is a getter.

---

# 36. Private members

Dart uses `_` at the beginning.

```dart
String _token = 'secret';
```

At library/file scope, leading underscore makes a declaration private to its Dart library.

Flutter state classes often appear as:

```dart
class _HomePageState extends State<HomePage> {
}
```

The underscore is why the state class is private.

---

# 37. Inheritance

```dart
class Animal {
  void speak() {
    print('Sound');
  }
}

class Dog extends Animal {
  @override
  void speak() {
    print('Woof');
  }
}
```

Flutter uses inheritance constantly:

```dart
class MyApp extends StatelessWidget
```

Meaning:

```text
MyApp
   ↓ inherits from
StatelessWidget
```

Then you override:

```dart
@override
Widget build(BuildContext context) {
  ...
}
```

---

# 38. `abstract` classes

```dart
abstract class PaymentService {
  Future<void> pay(double amount);
}
```

A concrete implementation:

```dart
class CardPaymentService implements PaymentService {
  @override
  Future<void> pay(double amount) async {
    print('Paid $amount');
  }
}
```

Useful in architecture and testing.

---

# 39. `extends` vs `implements`

## `extends`

Inherit implementation/behavior:

```dart
class Child extends Parent {}
```

## `implements`

Promise to provide an interface:

```dart
class ApiRepository implements UserRepository {
  ...
}
```

You will see both in larger Flutter applications.

---

# 40. Enums

Instead of fragile strings:

```dart
String status = 'pending';
```

use:

```dart
enum OrderStatus {
  pending,
  paid,
  shipped,
  delivered,
}
```

Then:

```dart
OrderStatus status = OrderStatus.pending;
```

This prevents typos like:

```text
pendng
peding
Pending
```

Flutter example:

```dart
switch (status) {
  case OrderStatus.pending:
    ...
  case OrderStatus.paid:
    ...
  case OrderStatus.shipped:
    ...
  case OrderStatus.delivered:
    ...
}
```

---

# 41. Static members

```dart
class AppConstants {
  static const apiBaseUrl = 'https://api.example.com';
}
```

Use:

```dart
AppConstants.apiBaseUrl
```

Useful for constants and utility APIs.

Do not turn every piece of application state into static variables.

---

# 42. Factory constructors — learn after basic constructors

You may see:

```dart
factory User.fromJson(Map<String, dynamic> json) {
  return User(
    name: json['name'] as String,
    age: json['age'] as int,
  );
}
```

Use:

```dart
final user = User.fromJson(json);
```

This is common for converting API JSON into Dart model objects.

---

# 43. Model class example

```dart
class Product {
  final int id;
  final String name;
  final double price;

  const Product({
    required this.id,
    required this.name,
    required this.price,
  });

  factory Product.fromJson(Map<String, dynamic> json) {
    return Product(
      id: json['id'] as int,
      name: json['name'] as String,
      price: (json['price'] as num).toDouble(),
    );
  }
}
```

Now instead of:

```dart
product['name']
```

everywhere, you get:

```dart
product.name
product.price
```

Strongly typed models make larger Flutter apps much easier to maintain.

---

# 44. `toJson()`

For sending data:

```dart
Map<String, dynamic> toJson() {
  return {
    'id': id,
    'name': name,
    'price': price,
  };
}
```

Flow:

```text
Dart object
   ↓ toJson()
Map<String, dynamic>
   ↓ jsonEncode()
JSON
   ↓
API
```

---

# 45. Imports

Import your own file:

```dart
import 'package:my_app/models/user.dart';
```

Dart core functionality is automatically available for many basic types.

Other library:

```dart
import 'dart:convert';
```

Flutter:

```dart
import 'package:flutter/material.dart';
```

Package:

```dart
import 'package:http/http.dart' as http;
```

---

# 46. Packages

Dart and Flutter dependencies are managed through:

```text
pubspec.yaml
```

Example:

```yaml
dependencies:
  flutter:
    sdk: flutter
  http: ^1.0.0
```

Then:

```bash
flutter pub get
```

Do not copy dependency versions blindly from old tutorials. Check the current package documentation and compatibility with your Flutter/Dart version.

---

# 47. `Future` — extremely important

A `Future` represents a result that arrives later.

Example:

```dart
Future<String> getUsername() async {
  return 'Alex';
}
```

Why?

Because operations like these take time:

```text
API request
database operation
file read
authentication
device storage
```

You cannot assume the result exists immediately.

---

# 48. `async` and `await`

```dart
Future<void> loadUser() async {
  final user = await fetchUser();

  print(user);
}
```

Mental model:

```text
call fetchUser()
      ↓
request is happening
      ↓
await result
      ↓
continue when result arrives
```

This is one of the most important concepts for Flutter.

---

# 49. Example API-style async flow

```dart
Future<List<Product>> fetchProducts() async {
  final response = await api.getProducts();

  return response;
}
```

Flutter screen:

```text
Screen opens
   ↓
fetchProducts()
   ↓
loading = true
   ↓
API request
   ↓
data arrives
   ↓
products updated
   ↓
loading = false
   ↓
UI rebuilds
```

---

# 50. `Future<T>` syntax

```dart
Future<String>
```

means:

```text
A future String
```

```dart
Future<User>
```

means:

```text
A future User
```

```dart
Future<List<Product>>
```

means:

```text
A future List of Product objects
```

```dart
Future<void>
```

means:

```text
Async operation with no meaningful return value
```

---

# 51. Error handling

```dart
try {
  final user = await fetchUser();
  print(user);
} catch (error) {
  print('Something went wrong: $error');
}
```

With cleanup:

```dart
try {
  await uploadFile();
} catch (error) {
  print(error);
} finally {
  print('Finished');
}
```

Flutter should present intentional error UI instead of silently failing.

---

# 52. Throwing exceptions

```dart
if (response.statusCode != 200) {
  throw Exception('Failed to load products');
}
```

Then a higher layer can catch and handle it.

---

# 53. Streams

A `Future` usually provides one eventual result.

A `Stream` provides values over time.

```text
Future
→ one result later

Stream
→ result
→ result
→ result
→ result ...
```

Examples:

```text
live chat messages
Firebase snapshots
location updates
sensor data
WebSocket messages
```

Basic:

```dart
Stream<int> countStream() async* {
  for (int i = 0; i < 5; i++) {
    yield i;
  }
}
```

Do not spend too much time on Streams before you are comfortable with Futures.

---

# 54. Generics

You already saw:

```dart
List<String>
```

`String` is the generic type.

Other examples:

```dart
List<User>
Future<Product>
Map<String, dynamic>
```

A custom generic:

```dart
class ApiResponse<T> {
  final T data;

  ApiResponse(this.data);
}
```

Then:

```dart
ApiResponse<User>
ApiResponse<List<Product>>
```

Generics let one structure safely work with different types.

---

# 55. Collection `if`

Very useful inside Flutter widget lists:

```dart
Column(
  children: [
    const Text('Profile'),

    if (isAdmin)
      const Text('Admin controls'),
  ],
)
```

This is Dart syntax, not special Flutter syntax.

---

# 56. Spread operator `...`

Suppose:

```dart
final menuItems = [
  const Text('Home'),
  const Text('Profile'),
];
```

Insert them into another list:

```dart
Column(
  children: [
    const Text('Menu'),
    ...menuItems,
  ],
)
```

This is common in Flutter widget trees.

---

# 57. Null-aware spread `...?`

```dart
List<Widget>? optionalWidgets;
```

Then:

```dart
Column(
  children: [
    const Text('Header'),
    ...?optionalWidgets,
  ],
)
```

If the list is null, Dart simply adds nothing.

---

# 58. Cascade operator `..`

You may see:

```dart
final controller = TextEditingController()
  ..text = 'Hello'
  ..selection = const TextSelection.collapsed(offset: 5);
```

This means perform multiple operations on the same object.

Do not worry if it looks unfamiliar at first.

---

# 59. `this`

Inside a class:

```dart
class User {
  final String name;

  User(this.name);
}
```

`this.name` means:

```text
the `name` field belonging to this object
```

Flutter constructors often use:

```dart
const ProfileCard({
  super.key,
  required this.name,
});
```

---

# 60. `super`

Suppose:

```dart
class ProfileCard extends StatelessWidget {
  const ProfileCard({
    super.key,
    required this.name,
  });

  final String name;

  @override
  Widget build(BuildContext context) {
    return Text(name);
  }
}
```

`super.key` forwards the `key` parameter to the parent `StatelessWidget` constructor.

You will see this in nearly every modern Flutter widget class.

---

# 61. What is `Key`?

You do not need deep knowledge initially.

A `Key` helps Flutter identify widgets when the widget tree changes.

Typical widget:

```dart
class ProductCard extends StatelessWidget {
  const ProductCard({
    super.key,
    required this.product,
  });

  final Product product;

  ...
}
```

Leave `super.key` in generated/reusable widget constructors even when you are not manually using keys yet.

---

# 62. Immutability

Flutter encourages immutable widget configurations.

Example:

```dart
class UserCard extends StatelessWidget {
  final String name;

  const UserCard({
    super.key,
    required this.name,
  });
}
```

Because `name` should not be reassigned inside this widget:

```dart
final String name;
```

This pattern is extremely common.

---

# 63. A Flutter widget is basically a Dart class

This:

```dart
class WelcomeText extends StatelessWidget {
  const WelcomeText({
    super.key,
    required this.name,
  });

  final String name;

  @override
  Widget build(BuildContext context) {
    return Text('Welcome $name');
  }
}
```

contains ordinary Dart concepts:

```text
class
extends
constructor
named parameter
required
final field
override
method
return value
string interpolation
```

That is why learning the Dart concepts in this guide makes Flutter code much easier to read.

---

# 64. Understanding Flutter callback syntax

Example:

```dart
onPressed: () {
  saveProduct();
},
```

Read it as:

```text
onPressed parameter
       ↓
receives an anonymous function
       ↓
that function calls saveProduct()
```

Short form:

```dart
onPressed: saveProduct,
```

works if the function signature matches.

---

# 65. Function as a value

Dart functions are values.

```dart
void sayHello() {
  print('Hello');
}

final action = sayHello;

action();
```

Flutter relies heavily on this:

```dart
final VoidCallback onTap;
```

Meaning:

```text
onTap must hold a function
that takes no arguments
and returns void
```

---

# 66. Callback component example

```dart
class SaveButton extends StatelessWidget {
  const SaveButton({
    super.key,
    required this.onSave,
  });

  final VoidCallback onSave;

  @override
  Widget build(BuildContext context) {
    return ElevatedButton(
      onPressed: onSave,
      child: const Text('Save'),
    );
  }
}
```

Use:

```dart
SaveButton(
  onSave: () {
    print('Saved');
  },
)
```

This is the basis of component communication.

---

# 67. Typedef

You can name a function type:

```dart
typedef ProductCallback = void Function(Product product);
```

Then:

```dart
final ProductCallback onSelected;
```

Useful when callback signatures become more complicated.

---

# 68. Extension methods

Dart can add convenient methods to existing types.

```dart
extension StringFormatting on String {
  String get capitalized {
    if (isEmpty) return this;

    return '${this[0].toUpperCase()}${substring(1)}';
  }
}
```

Use:

```dart
print('alex'.capitalized);
```

Extensions are useful, but do not create hundreds of magical helpers that make the codebase difficult to understand.

---

# 69. Records — useful modern Dart feature

A record can group values without creating a full class.

```dart
(String, int) getUserInfo() {
  return ('Alex', 22);
}
```

Use:

```dart
final (name, age) = getUserInfo();
```

Named fields:

```dart
({String name, int age}) getUserInfo() {
  return (
    name: 'Alex',
    age: 22,
  );
}
```

For core business models, classes are often clearer. Records are useful for small temporary grouped values.

---

# 70. Pattern matching — recognize it

Modern Dart supports patterns.

You may see:

```dart
if (value case int number) {
  print(number);
}
```

or destructuring:

```dart
final (name, age) = userInfo;
```

You do not need advanced pattern matching before learning Flutter fundamentals.

---

# 71. JSON decoding

Import:

```dart
import 'dart:convert';
```

Suppose:

```dart
final jsonText = '''
{
  "id": 1,
  "name": "Ruby"
}
''';
```

Decode:

```dart
final data = jsonDecode(jsonText);
```

Often:

```dart
final map = data as Map<String, dynamic>;
```

Then convert into a model:

```dart
final product = Product.fromJson(map);
```

---

# 72. JSON encoding

```dart
final data = {
  'name': 'Ruby',
  'price': 1000,
};

final jsonText = jsonEncode(data);
```

Typical API flow:

```text
Model
 ↓
toJson()
 ↓
Map
 ↓
jsonEncode()
 ↓
HTTP request body
```

HTTP libraries often handle some of this depending on the client and configuration.

---

# 73. Equality: `==`

Strings/numbers:

```dart
if (status == 'active') {
}
```

Do not confuse:

```text
=   assignment
==  equality comparison
```

Example:

```dart
name = 'Alex';
```

sets value.

```dart
name == 'Alex'
```

checks value.

---

# 74. Type checking

```dart
if (value is String) {
  print(value.length);
}
```

Negative:

```dart
if (value is! String) {
}
```

Cast:

```dart
final user = value as User;
```

Avoid unsafe casts unless you actually know the runtime type.

---

# 75. `required`

Very common:

```dart
class ProductCard {
  ProductCard({
    required this.name,
  });

  final String name;
}
```

This forces:

```dart
ProductCard(name: 'Ruby');
```

and prevents:

```dart
ProductCard(); // ❌
```

Use `required` when a value is necessary for a valid object/widget.

---

# 76. Linting and static analysis

Run:

```bash
dart analyze
```

For a Flutter app:

```bash
flutter analyze
```

Dart's analyzer catches:

- type errors
- invalid null handling
- unused variables
- style/lint issues
- suspicious code

Do not ignore analyzer warnings without understanding them.

---

# 77. Formatting

Use:

```bash
dart format .
```

Flutter projects can use the same Dart formatter.

Do not manually fight formatting conventions. Let the formatter keep the team consistent.

---

# 78. Testing basic Dart code

A function:

```dart
int add(int a, int b) => a + b;
```

Test concept:

```dart
test('adds two numbers', () {
  expect(add(2, 3), 5);
});
```

Tests become increasingly valuable as business logic grows.

Separate business logic from UI so it can be tested without rendering widgets.

---

# 79. Common Dart errors in Flutter

## Error: nullable value used where non-null expected

You have:

```dart
String? name;
```

but destination needs:

```dart
String
```

Do not immediately use `!`.

Ask:

```text
Should this actually be nullable?
Should I provide a fallback?
Should I check null first?
```

---

# 80. Type mismatch

Example:

```text
String is not assignable to int
```

Inspect where data comes from.

API values may need conversion:

```dart
final id = int.parse(idString);
```

or safe conversion:

```dart
final id = int.tryParse(idString);
```

---

# 81. `setState()` and Dart

You may see:

```dart
setState(() {
  count++;
});
```

Break it apart:

```text
setState(
   anonymous Dart function
)
```

Inside that function:

```dart
count++;
```

Flutter then knows the state changed and schedules a rebuild.

The syntax is Dart; the behavior is Flutter.

---

# 82. Reading a Flutter widget line by line

Code:

```dart
class ProductCard extends StatelessWidget {
  const ProductCard({
    super.key,
    required this.name,
    required this.price,
  });

  final String name;
  final double price;

  @override
  Widget build(BuildContext context) {
    return Card(
      child: ListTile(
        title: Text(name),
        subtitle: Text('\$$price'),
      ),
    );
  }
}
```

Translate to plain English:

```text
class ProductCard
→ create a new class named ProductCard

extends StatelessWidget
→ ProductCard is a kind of Flutter StatelessWidget

const ProductCard(...)
→ constructor

super.key
→ forward optional widget key to parent

required this.name
→ caller must provide name

required this.price
→ caller must provide price

final String name
→ immutable String field

final double price
→ immutable double field

@override
→ replacing a parent-class method

Widget build(...)
→ function that describes UI

return Card(...)
→ return a Flutter Card widget
```

Once you can translate code this way, Flutter becomes much less intimidating.

---

# 83. Recommended Dart learning order for Flutter

Do it in this sequence:

```text
1. Variables and types
2. final / const
3. if / switch
4. Functions
5. Named parameters
6. Lists and Maps
7. map / where
8. Null safety
9. Classes
10. Constructors
11. Inheritance
12. Models / JSON
13. Future
14. async / await
15. Exceptions
16. Generics
17. Streams
18. Advanced Dart features
```

Do not delay Flutter until step 18.

Start simple Flutter screens around step 8–10.

---

# 84. What to memorize vs understand

## Memorize enough to recognize

```text
final
const
required
?
!
??
async
await
extends
implements
@override
```

## Understand deeply

```text
types
nullability
functions
objects
constructors
callbacks
async behavior
lists/maps
model conversion
```

Syntax becomes automatic through practice.

---

# 85. Flutter-focused Dart cheat sheet

```dart
// Variable
String name = 'Alex';

// Mutable inferred variable
var count = 0;

// Runtime immutable
final now = DateTime.now();

// Compile-time constant
const appName = 'My App';

// Nullable
String? imageUrl;

// Fallback
final image = imageUrl ?? 'default.png';

// Function
void save() {}

// Async function
Future<void> load() async {
  await fetchData();
}

// List
final products = <Product>[];

// Map
final json = <String, dynamic>{};

// Class
class User {
  final String name;

  const User({
    required this.name,
  });
}

// Enum
enum Status {
  loading,
  success,
  error,
}

// Callback
final VoidCallback onTap;

// Collection transform
final widgets = products
    .map((product) => Text(product.name))
    .toList();
```

---

# 86. Your first practice exercises

Do these without Flutter first.

## Exercise 1

Create:

```dart
class Product
```

with:

```text
id
name
price
isAvailable
```

---

## Exercise 2

Create a list of five products.

Filter only available products.

---

## Exercise 3

Create:

```dart
double calculateTotal(List<Product> products)
```

---

## Exercise 4

Create:

```dart
factory Product.fromJson(...)
```

and:

```dart
toJson()
```

---

## Exercise 5

Create:

```dart
Future<List<Product>> fetchProducts()
```

that waits briefly and returns mock products.

This prepares you directly for Flutter networking/state work.

---

# 87. What you should know before moving deeply into Flutter

You are ready when this code mostly makes sense:

```dart
class ProductService {
  Future<List<Product>> getProducts() async {
    try {
      final response = await fetchProducts();

      return response
          .where((product) => product.isAvailable)
          .toList();
    } catch (error) {
      throw Exception('Unable to load products');
    }
  }
}
```

You should recognize:

```text
class
Future<List<Product>>
method
async
try/catch
await
list
where
arrow callback
toList()
throw
```

You do not need to write it from memory yet.

---

# 88. Dart → Flutter translation table

| Dart concept | Where you see it in Flutter |
|---|---|
| Class | Every custom widget |
| Constructor | Widget configuration |
| Named parameter | `padding:`, `child:`, `onPressed:` |
| Function | event handlers |
| Callback | button taps |
| List | `children: []` |
| `.map()` | building repeated widgets |
| `Future` | API/database call |
| `async/await` | loading data |
| `enum` | screen/status state |
| Null safety | optional API/user fields |
| Inheritance | `extends StatelessWidget` |
| `final` | widget fields |
| `const` | immutable widgets |
| Map | decoded JSON |
| Class/model | API data |
| Exception | failed operation |

---

# 89. What NOT to do while learning Dart

Do not:

```text
❌ learn Dart for 3 months before opening Flutter
❌ use dynamic everywhere
❌ add ! whenever null safety complains
❌ copy code without understanding constructors
❌ ignore async/await
❌ keep API data as Map<String, dynamic> everywhere
❌ put all logic in widgets
❌ ignore analyzer errors
```

Instead:

```text
Learn a Dart concept
      ↓
use it inside Flutter
      ↓
see why it matters
      ↓
repeat
```

---

# 90. Final Dart learning path

```text
DART BASICS
variables
types
functions
collections
      ↓
DART FOR FLUTTER
named parameters
classes
constructors
null safety
callbacks
      ↓
DART FOR DATA
models
JSON
generics
      ↓
ASYNC DART
Future
async/await
exceptions
Streams
      ↓
REAL FLUTTER DEVELOPMENT
widgets
state
API calls
architecture
testing
```

---

# ✅ Final checkpoint

You understand enough Dart for Flutter when you can explain this:

```dart
class ProfileCard extends StatelessWidget {
  const ProfileCard({
    super.key,
    required this.name,
    this.imageUrl,
  });

  final String name;
  final String? imageUrl;

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Text(name),
        Text(imageUrl ?? 'No image'),
      ],
    );
  }
}
```

You should be able to say:

```text
ProfileCard is a class.
It inherits StatelessWidget.
It has a const constructor.
name is required.
imageUrl is optional and nullable.
Both fields are final.
build() returns the UI.
?? provides a fallback when imageUrl is null.
children is a List<Widget>.
```

If that explanation is clear to you, begin the companion **Flutter Framework Guide** immediately.

---

# 📚 Official references

- Dart: https://dart.dev/
- Dart language: https://dart.dev/language
- Dart tutorials: https://dart.dev/tutorials
- Effective Dart: https://dart.dev/effective-dart
- Flutter: https://docs.flutter.dev/

Use official documentation when language features or package versions differ from older tutorials.
