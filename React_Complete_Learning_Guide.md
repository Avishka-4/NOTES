# React.js: Complete Learning Guide (Basics to Advanced)

A beginner-friendly guide to all React concepts with simple code examples.

---

## 📌 TABLE OF CONTENTS
1. [Core: JSX & Components](#1-core-jsx--components)
2. [Props & State](#2-props--state)
3. [Event Handling](#3-event-handling)
4. [Conditional Rendering](#4-conditional-rendering)
5. [Hooks](#5-hooks)
6. [Context API](#6-context-api)
7. [React Router](#7-react-router)
8. [Data Fetching](#8-data-fetching)
9. [State Management](#9-state-management)
10. [Performance Optimization](#10-performance-optimization)
11. [Testing](#11-testing)
12. [Build Tooling](#12-build-tooling)

---

## 1. CORE: JSX & COMPONENTS

### What is JSX?
JSX is JavaScript + HTML mixed together. React converts it to JavaScript.

### Functional Components (Recommended)

```jsx
// Simple Hello Component
function Hello() {
  return <h1>Hello, World!</h1>;
}

// Using the component
export default Hello;
```

### JSX Rules

```jsx
// ✅ CORRECT
function Card() {
  return (
    <div>
      <h2>My Card</h2>
      <p>This is a paragraph</p>
    </div>
  );
}

// ❌ WRONG - Multiple elements without wrapper
function BadCard() {
  return (
    <h2>Title</h2>
    <p>Content</p>  // Error!
  );
}

// ✅ Use Fragment to avoid extra div
import { Fragment } from 'react';

function BetterCard() {
  return (
    <Fragment>
      <h2>Title</h2>
      <p>Content</p>
    </Fragment>
  );
}

// Or use shorthand <>...</>
function ShorthandCard() {
  return (
    <>
      <h2>Title</h2>
      <p>Content</p>
    </>
  );
}
```

### Component Composition

```jsx
function Welcome() {
  return <h1>Welcome to React!</h1>;
}

function App() {
  return (
    <div>
      <Welcome />
      <Welcome />
      <Welcome />
    </div>
  );
}

export default App;
```

---

## 2. PROPS & STATE

### Props: Pass Data to Components

Props are like function arguments. They're read-only.

```jsx
// Parent Component passes data
function App() {
  return (
    <Greeting name="Alice" />
  );
}

// Child Component receives props
function Greeting(props) {
  return <h1>Hello, {props.name}!</h1>;
}
```

### Destructuring Props

```jsx
// ✅ Cleaner way
function Greeting({ name, age }) {
  return (
    <div>
      <h1>Hello, {name}!</h1>
      <p>Age: {age}</p>
    </div>
  );
}

// Usage
<Greeting name="Alice" age={25} />
```

### useState Hook: Adding State

State is data that can change. When state changes, the component re-renders.

```jsx
import { useState } from 'react';

function Counter() {
  // count = current value, setCount = function to update it
  const [count, setCount] = useState(0);

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(count + 1)}>
        Increment
      </button>
    </div>
  );
}
```

### Multiple State Values

```jsx
import { useState } from 'react';

function UserForm() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');

  return (
    <div>
      <input 
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="Enter name"
      />
      <input 
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Enter email"
      />
      <p>Name: {name}, Email: {email}</p>
    </div>
  );
}
```

### Object State

```jsx
import { useState } from 'react';

function UserProfile() {
  const [user, setUser] = useState({
    name: 'Alice',
    age: 25,
    city: 'New York'
  });

  // Update single property
  const updateName = (newName) => {
    setUser({
      ...user,  // Keep other properties
      name: newName  // Update only name
    });
  };

  return (
    <div>
      <p>Name: {user.name}</p>
      <p>Age: {user.age}</p>
      <button onClick={() => updateName('Bob')}>
        Change Name
      </button>
    </div>
  );
}
```

---

## 3. EVENT HANDLING

### Synthetic Events

React wraps browser events into synthetic events.

```jsx
function Button() {
  const handleClick = () => {
    alert('Button clicked!');
  };

  return <button onClick={handleClick}>Click me</button>;
}
```

### Common Events

```jsx
function EventExamples() {
  const handleClick = () => console.log('Clicked');
  const handleChange = (e) => console.log(e.target.value);
  const handleSubmit = (e) => {
    e.preventDefault();  // Stop form from reloading page
    console.log('Form submitted');
  };
  const handleKeyPress = (e) => {
    if (e.key === 'Enter') {
      console.log('Enter pressed');
    }
  };
  const handleHover = () => console.log('Hovering');

  return (
    <div>
      <button onClick={handleClick}>Click</button>
      <input onChange={handleChange} placeholder="Type..." />
      <input onKeyPress={handleKeyPress} />
      <div onMouseEnter={handleHover}>Hover me</div>
      <form onSubmit={handleSubmit}>
        <input type="text" />
        <button type="submit">Submit</button>
      </form>
    </div>
  );
}
```

### Passing Arguments to Event Handlers

```jsx
function TodoList() {
  const handleDelete = (id) => {
    console.log(`Deleted item ${id}`);
  };

  return (
    <button onClick={() => handleDelete(5)}>
      Delete Item 5
    </button>
  );
}
```

### Forms with Event Handling

```jsx
import { useState } from 'react';

function LoginForm() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Logging in:', email, password);
    // Send to server...
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Email"
      />
      <input
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        placeholder="Password"
      />
      <button type="submit">Login</button>
    </form>
  );
}
```

---

## 4. CONDITIONAL RENDERING

### if/else Logic

```jsx
function LoginStatus({ isLoggedIn }) {
  if (isLoggedIn) {
    return <h1>Welcome back!</h1>;
  } else {
    return <h1>Please log in</h1>;
  }
}

// Usage
<LoginStatus isLoggedIn={true} />
```

### Ternary Operator (Recommended for JSX)

```jsx
function LoginStatus({ isLoggedIn }) {
  return (
    <h1>
      {isLoggedIn ? 'Welcome back!' : 'Please log in'}
    </h1>
  );
}
```

### && Operator (Show or Hide)

```jsx
function Notification({ message, show }) {
  return (
    <div>
      {show && <p>{message}</p>}
    </div>
  );
}

// Usage
<Notification message="Success!" show={true} />
```

### switch Statement

```jsx
function StatusMessage({ status }) {
  switch(status) {
    case 'loading':
      return <p>Loading...</p>;
    case 'success':
      return <p>Success!</p>;
    case 'error':
      return <p>Error!</p>;
    default:
      return <p>Unknown status</p>;
  }
}
```

### Rendering Lists

```jsx
function TodoList() {
  const todos = ['Learn React', 'Build a project', 'Master state'];

  return (
    <ul>
      {todos.map((todo, index) => (
        <li key={index}>{todo}</li>
      ))}
    </ul>
  );
}
```

### Complex List with Objects

```jsx
function UserList() {
  const users = [
    { id: 1, name: 'Alice', role: 'Admin' },
    { id: 2, name: 'Bob', role: 'User' },
    { id: 3, name: 'Charlie', role: 'User' }
  ];

  return (
    <ul>
      {users.map((user) => (
        <li key={user.id}>
          {user.name} - {user.role}
        </li>
      ))}
    </ul>
  );
}
```

---

## 5. HOOKS

Hooks are special functions that let you "hook into" React features.

### useState (State Management)

Already covered in Section 2. It's the most important hook!

### useEffect (Side Effects)

useEffect runs code after the component renders. Perfect for API calls, subscriptions, etc.

```jsx
import { useState, useEffect } from 'react';

function Clock() {
  const [time, setTime] = useState(new Date().toLocaleTimeString());

  useEffect(() => {
    // This runs AFTER render
    const timer = setInterval(() => {
      setTime(new Date().toLocaleTimeString());
    }, 1000);

    // Cleanup function (optional)
    return () => clearInterval(timer);
  }, []);  // Empty dependency array = run once on mount

  return <h1>{time}</h1>;
}
```

### useEffect with Dependencies

```jsx
import { useState, useEffect } from 'react';

function SearchUsers({ query }) {
  const [results, setResults] = useState([]);

  useEffect(() => {
    // This runs whenever 'query' changes
    console.log('Searching for:', query);
    // Fetch data...
  }, [query]);  // Dependency array

  return <div>{results.length} results found</div>;
}
```

### useRef (Reference to DOM Elements)

```jsx
import { useRef } from 'react';

function TextInput() {
  const inputRef = useRef(null);

  const focusInput = () => {
    inputRef.current.focus();
  };

  return (
    <>
      <input ref={inputRef} type="text" />
      <button onClick={focusInput}>Focus Input</button>
    </>
  );
}
```

### useReducer (Complex State Logic)

For state that depends on previous state.

```jsx
import { useReducer } from 'react';

function Counter() {
  // reducer function
  const reducer = (state, action) => {
    switch(action.type) {
      case 'INCREMENT':
        return { count: state.count + 1 };
      case 'DECREMENT':
        return { count: state.count - 1 };
      case 'RESET':
        return { count: 0 };
      default:
        return state;
    }
  };

  const [state, dispatch] = useReducer(reducer, { count: 0 });

  return (
    <div>
      <p>Count: {state.count}</p>
      <button onClick={() => dispatch({ type: 'INCREMENT' })}>
        Increment
      </button>
      <button onClick={() => dispatch({ type: 'DECREMENT' })}>
        Decrement
      </button>
      <button onClick={() => dispatch({ type: 'RESET' })}>
        Reset
      </button>
    </div>
  );
}
```

### Custom Hooks

Create your own reusable logic!

```jsx
import { useState, useEffect } from 'react';

// Custom hook for window size
function useWindowSize() {
  const [size, setSize] = useState({
    width: window.innerWidth,
    height: window.innerHeight
  });

  useEffect(() => {
    const handleResize = () => {
      setSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  return size;
}

// Use the custom hook
function ResponsiveComponent() {
  const size = useWindowSize();

  return (
    <div>
      <p>Width: {size.width}px</p>
      <p>Height: {size.height}px</p>
    </div>
  );
}
```

---

## 6. CONTEXT API

Share data across components without prop drilling.

### Creating Context

```jsx
import { createContext, useState } from 'react';

// Step 1: Create context
const ThemeContext = createContext();

// Step 2: Create provider component
function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');

  const toggleTheme = () => {
    setTheme(theme === 'light' ? 'dark' : 'light');
  };

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

export { ThemeContext, ThemeProvider };
```

### Using Context

```jsx
import { useContext } from 'react';
import { ThemeContext } from './ThemeProvider';

function App() {
  return (
    <ThemeProvider>
      <Header />
      <Content />
    </ThemeProvider>
  );
}

function Header() {
  const { theme, toggleTheme } = useContext(ThemeContext);

  return (
    <header style={{ background: theme === 'light' ? '#fff' : '#333' }}>
      <p>Current theme: {theme}</p>
      <button onClick={toggleTheme}>Toggle Theme</button>
    </header>
  );
}
```

### Multiple Contexts

```jsx
import { createContext } from 'react';

const AuthContext = createContext();
const NotificationContext = createContext();

function AppProviders({ children }) {
  return (
    <AuthContext.Provider value={{ user: 'Alice' }}>
      <NotificationContext.Provider value={{ message: 'Welcome!' }}>
        {children}
      </NotificationContext.Provider>
    </AuthContext.Provider>
  );
}
```

---

## 7. REACT ROUTER

Navigate between pages without reloading.

### Basic Routing

```jsx
import { BrowserRouter, Routes, Route } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
      </Routes>
    </BrowserRouter>
  );
}

function Home() {
  return <h1>Home Page</h1>;
}

function About() {
  return <h1>About Page</h1>;
}

function Contact() {
  return <h1>Contact Page</h1>;
}
```

### Navigation with Links

```jsx
import { Link } from 'react-router-dom';

function Navigation() {
  return (
    <nav>
      <Link to="/">Home</Link>
      <Link to="/about">About</Link>
      <Link to="/contact">Contact</Link>
    </nav>
  );
}
```

### Dynamic Routes (Parameters)

```jsx
import { useParams } from 'react-router-dom';

function UserProfile() {
  const { id } = useParams();

  return <h1>User Profile ID: {id}</h1>;
}

// In App.js
<Route path="/user/:id" element={<UserProfile />} />

// Link to it
<Link to="/user/123">View User 123</Link>
```

### Programmatic Navigation

```jsx
import { useNavigate } from 'react-router-dom';

function LoginForm() {
  const navigate = useNavigate();

  const handleLogin = () => {
    // After login
    navigate('/dashboard');
  };

  return <button onClick={handleLogin}>Login</button>;
}
```

---

## 8. DATA FETCHING

### Using fetch API with useEffect

```jsx
import { useState, useEffect } from 'react';

function PostList() {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetch('https://jsonplaceholder.typicode.com/posts')
      .then(response => response.json())
      .then(data => {
        setPosts(data.slice(0, 5));  // Get first 5
        setLoading(false);
      })
      .catch(err => {
        setError(err.message);
        setLoading(false);
      });
  }, []);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <ul>
      {posts.map(post => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
}
```

### Using async/await (Cleaner)

```jsx
import { useState, useEffect } from 'react';

function UserList() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await fetch('https://jsonplaceholder.typicode.com/users');
        const data = await response.json();
        setUsers(data);
      } catch (error) {
        console.log(error);
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, []);

  if (loading) return <p>Loading...</p>;

  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

### POST Request (Sending Data)

```jsx
import { useState } from 'react';

function CreatePost() {
  const [title, setTitle] = useState('');
  const [body, setBody] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const response = await fetch('https://jsonplaceholder.typicode.com/posts', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          title: title,
          body: body,
          userId: 1
        })
      });
      const data = await response.json();
      console.log('Post created:', data);
      setTitle('');
      setBody('');
    } catch (error) {
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Title"
      />
      <textarea
        value={body}
        onChange={(e) => setBody(e.target.value)}
        placeholder="Content"
      />
      <button type="submit" disabled={loading}>
        {loading ? 'Posting...' : 'Create Post'}
      </button>
    </form>
  );
}
```

---

## 9. STATE MANAGEMENT

### Redux Pattern with useReducer (Simplified)

```jsx
import { createContext, useReducer, useContext } from 'react';

// Create store context
const StoreContext = createContext();

// Reducer
function storeReducer(state, action) {
  switch(action.type) {
    case 'ADD_ITEM':
      return {
        items: [...state.items, action.payload]
      };
    case 'REMOVE_ITEM':
      return {
        items: state.items.filter(item => item.id !== action.payload)
      };
    default:
      return state;
  }
}

// Provider
export function StoreProvider({ children }) {
  const [state, dispatch] = useReducer(storeReducer, {
    items: []
  });

  return (
    <StoreContext.Provider value={{ state, dispatch }}>
      {children}
    </StoreContext.Provider>
  );
}

// Custom hook to use store
export function useStore() {
  return useContext(StoreContext);
}

// Usage
function ShoppingCart() {
  const { state, dispatch } = useStore();

  const addItem = (item) => {
    dispatch({ type: 'ADD_ITEM', payload: item });
  };

  return (
    <div>
      <button onClick={() => addItem({ id: 1, name: 'Apple' })}>
        Add Item
      </button>
      <ul>
        {state.items.map(item => (
          <li key={item.id}>{item.name}</li>
        ))}
      </ul>
    </div>
  );
}
```

### Zustand (Simpler State Management)

```jsx
import { create } from 'zustand';

// Create store
const useCountStore = create((set) => ({
  count: 0,
  increment: () => set((state) => ({ count: state.count + 1 })),
  decrement: () => set((state) => ({ count: state.count - 1 })),
  reset: () => set({ count: 0 })
}));

// Use in component
function Counter() {
  const { count, increment, decrement } = useCountStore();

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={increment}>+</button>
      <button onClick={decrement}>-</button>
    </div>
  );
}
```

---

## 10. PERFORMANCE OPTIMIZATION

### React.memo (Prevent Unnecessary Re-renders)

```jsx
import { memo } from 'react';

// Without memo - re-renders when parent re-renders
function UserCard({ user }) {
  console.log('UserCard rendered');
  return <div>{user.name}</div>;
}

// With memo - only re-renders if props change
const OptimizedUserCard = memo(function UserCard({ user }) {
  console.log('UserCard rendered');
  return <div>{user.name}</div>;
});
```

### useMemo (Memoize Values)

```jsx
import { useMemo, useState } from 'react';

function ExpensiveCalculation() {
  const [count, setCount] = useState(0);
  const [other, setOther] = useState(0);

  // This expensive calculation only runs when count changes
  const result = useMemo(() => {
    console.log('Calculating...');
    return count * 2 + 100;
  }, [count]);

  return (
    <div>
      <p>Result: {result}</p>
      <button onClick={() => setCount(count + 1)}>Increment Count</button>
      <button onClick={() => setOther(other + 1)}>
        Change Other (no recalculation)
      </button>
    </div>
  );
}
```

### useCallback (Memoize Functions)

```jsx
import { useCallback, useState } from 'react';
import { memo } from 'react';

const ChildButton = memo(function({ onClick }) {
  console.log('ChildButton rendered');
  return <button onClick={onClick}>Click me</button>;
});

function Parent() {
  const [count, setCount] = useState(0);

  // Without useCallback: new function created every render
  // With useCallback: same function unless dependencies change
  const handleClick = useCallback(() => {
    setCount(count + 1);
  }, [count]);

  return (
    <div>
      <p>Count: {count}</p>
      <ChildButton onClick={handleClick} />
    </div>
  );
}
```

### Code Splitting (Lazy Loading)

```jsx
import { lazy, Suspense } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

// Load component only when needed
const HeavyComponent = lazy(() => import('./HeavyComponent'));

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route 
          path="/heavy" 
          element={
            <Suspense fallback={<p>Loading...</p>}>
              <HeavyComponent />
            </Suspense>
          } 
        />
      </Routes>
    </BrowserRouter>
  );
}
```

---

## 11. TESTING

### Testing with Jest and React Testing Library

```jsx
// UserCard.jsx
export function UserCard({ user, onDelete }) {
  return (
    <div>
      <h2>{user.name}</h2>
      <p>{user.email}</p>
      <button onClick={onDelete}>Delete</button>
    </div>
  );
}

// UserCard.test.jsx
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { UserCard } from './UserCard';

describe('UserCard', () => {
  test('renders user information', () => {
    const user = { name: 'Alice', email: 'alice@example.com' };
    render(<UserCard user={user} onDelete={() => {}} />);

    expect(screen.getByText('Alice')).toBeInTheDocument();
    expect(screen.getByText('alice@example.com')).toBeInTheDocument();
  });

  test('calls onDelete when button is clicked', async () => {
    const user = { name: 'Alice', email: 'alice@example.com' };
    const handleDelete = jest.fn();

    render(<UserCard user={user} onDelete={handleDelete} />);

    const deleteButton = screen.getByText('Delete');
    await userEvent.click(deleteButton);

    expect(handleDelete).toHaveBeenCalled();
  });
});
```

### Testing Hooks

```jsx
import { renderHook, act } from '@testing-library/react';
import { useCounter } from './useCounter';

describe('useCounter', () => {
  test('increments count', () => {
    const { result } = renderHook(() => useCounter());

    act(() => {
      result.current.increment();
    });

    expect(result.current.count).toBe(1);
  });
});
```

---

## 12. BUILD TOOLING

### Project Setup with Vite

```bash
# Create project
npm create vite@latest my-app -- --template react
cd my-app

# Install dependencies
npm install

# Start development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview
```

### Environment Variables

```bash
# .env file
VITE_API_URL=https://api.example.com
VITE_API_KEY=your-secret-key
```

```jsx
// Access in component
const API_URL = import.meta.env.VITE_API_URL;
```

### Common npm Scripts

```json
{
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview",
    "test": "jest",
    "lint": "eslint .",
    "format": "prettier --write ."
  }
}
```

---

## 📚 QUICK REFERENCE

### Import Statements

```jsx
// Components
import React from 'react';

// Hooks
import { useState, useEffect, useRef, useContext, useReducer, useMemo, useCallback } from 'react';

// Router
import { BrowserRouter, Routes, Route, Link, useParams, useNavigate } from 'react-router-dom';

// Context
import { createContext } from 'react';
```

### Common Patterns

```jsx
// 1. Counter with useState
const [count, setCount] = useState(0);

// 2. Form handling
const [form, setForm] = useState({ name: '', email: '' });
const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

// 3. Fetch data
useEffect(() => {
  fetch(url).then(r => r.json()).then(setData);
}, []);

// 4. Conditional render
{condition && <Component />}
{condition ? <A /> : <B />}

// 5. Map list
{items.map((item) => <Item key={item.id} {...item} />)}

// 6. Toggle theme
const [dark, setDark] = useState(false);
<button onClick={() => setDark(!dark)}>Toggle</button>
```

---

## 🎯 LEARNING PATH

**Beginner (1-2 weeks)**
- Core: JSX & Components
- Props & State
- Event Handling
- Conditional Rendering
- Simple lists

**Intermediate (2-3 weeks)**
- Hooks (useState, useEffect)
- Forms & Validation
- API Calls
- React Router
- Context API

**Advanced (3-4 weeks)**
- useReducer & Custom Hooks
- State Management (Redux/Zustand)
- Performance Optimization
- Testing
- Build Tooling

---

## 🔗 RESOURCES

- **Official Docs**: https://react.dev
- **React Router**: https://reactrouter.com
- **Create React App**: https://create-react-app.dev
- **Vite**: https://vitejs.dev
- **Testing Library**: https://testing-library.com/react
- **Redux**: https://redux.js.org
- **Zustand**: https://github.com/pmndrs/zustand

---

Good luck with your React journey! 🚀
