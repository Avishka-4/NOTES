# Next.js - Simple Explanation with Code

Let me break down **everything** in the simplest way possible.

---

## **1. What is Next.js?**

Next.js is a **framework** that makes building websites easier. Instead of writing everything from scratch, Next.js gives you ready-made tools.

Think of it like: **HTML + React + Routing + Database Connection = Next.js**

---

## **2. File-Based Routing (Creating Pages)**

### **The Problem:**
In regular websites, you need to manually create routes.

### **The Solution - Next.js:**
Just create files, and they automatically become pages!

```
📁 pages/
  ├── index.js      → Website URL: /
  ├── about.js      → Website URL: /about
  ├── contact.js    → Website URL: /contact
  └── blog/
      └── post.js   → Website URL: /blog/post
```

### **Code Example:**

```javascript
// pages/index.js (This is your home page)
export default function Home() {
  return <h1>Welcome to my website!</h1>;
}
```

**What happens:**
- You create a file called `index.js` in `pages/` folder
- Automatically, your website homepage is `/` (yoursite.com/)
- You don't need to manually link it!

```javascript
// pages/about.js (This is your about page)
export default function About() {
  return <h1>About Us</h1>;
}
```

**Result:** Now you have `yoursite.com/about/`

---

## **3. Dynamic Pages (Pages with Variables)**

Sometimes you want a page that shows **different content** based on the URL.

Example: `yoursite.com/blog/post-1`, `yoursite.com/blog/post-2`

```
📁 pages/
  └── blog/
      └── [id].js   → Matches /blog/1, /blog/2, /blog/3, etc.
```

### **Code Example:**

```javascript
// pages/blog/[id].js
export default function BlogPost({ id }) {
  return <h1>This is blog post number: {id}</h1>;
}

export async function getServerSideProps(context) {
  const id = context.params.id;  // Get the ID from URL
  return {
    props: { id }
  };
}
```

**What happens:**
- User visits `yoursite.com/blog/5`
- Next.js gets the number `5` from the URL
- Shows: "This is blog post number: 5"

---

## **4. Rendering Modes (How Pages Load)**

### **A. SSR - Server-Side Rendering**

**What:** Page builds on **every request** (every time someone visits)

**When to use:** When data changes frequently (weather, stock prices)

```javascript
// pages/weather.js
export async function getServerSideProps() {
  const weather = await fetch('https://weather-api.com');
  const data = await weather.json();
  
  return {
    props: { data }
  };
}

export default function Weather({ data }) {
  return <h1>Temperature: {data.temp}°C</h1>;
}
```

**Timeline:**
```
User visits page → Server fetches weather → Page loads
```

---

### **B. SSG - Static Site Generation**

**What:** Page builds **once** at build time (when you deploy)

**When to use:** When data doesn't change (blog posts, documentation)

```javascript
// pages/blog/[slug].js
export async function getStaticProps(context) {
  const slug = context.params.slug;
  const post = await fetch(`https://api.com/blog/${slug}`);
  const data = await post.json();
  
  return {
    props: { data },
    revalidate: 3600  // Rebuild every 1 hour
  };
}

export async function getStaticPaths() {
  return {
    paths: [
      { params: { slug: 'hello-world' } },
      { params: { slug: 'second-post' } }
    ],
    fallback: false
  };
}

export default function BlogPost({ data }) {
  return <h1>{data.title}</h1>;
}
```

**Timeline:**
```
Build time → Creates HTML files → User gets instant page
```

---

### **C. ISR - Incremental Static Regeneration**

**What:** Build page once, but **refresh it** every X seconds

**When to use:** When data changes occasionally

```javascript
export async function getStaticProps() {
  const data = await fetch('https://api.com/data');
  
  return {
    props: { data },
    revalidate: 60  // Rebuild every 60 seconds
  };
}
```

**Timeline:**
```
Build → Page cached → Someone visits within 60 sec → Shows old page
But after 60 sec, Next.js rebuilds it in background
```

---

### **D. CSR - Client-Side Rendering**

**What:** Page builds in the **browser** when user visits

**When to use:** Personal dashboards (user-specific content)

```javascript
import { useState, useEffect } from 'react';

export default function Dashboard() {
  const [data, setData] = useState(null);

  useEffect(() => {
    // Fetch when page loads in browser
    fetch('/api/user-data')
      .then(res => res.json())
      .then(data => setData(data));
  }, []);

  if (!data) return <p>Loading...</p>;
  return <h1>Your dashboard: {data.name}</h1>;
}
```

**Timeline:**
```
User visits → Empty page loads → Browser fetches data → Page shows
```

---

## **5. API Routes (Your Own API)**

You can create your own API endpoints (like a backend).

```
📁 pages/
  └── api/
      ├── users.js        → API: /api/users
      ├── products.js     → API: /api/products
      └── users/
          └── [id].js     → API: /api/users/1, /api/users/2
```

### **Code Example:**

```javascript
// pages/api/hello.js
export default function handler(req, res) {
  if (req.method === 'GET') {
    res.status(200).json({ message: 'Hello World' });
  }
}
```

**What happens:**
- User visits `yoursite.com/api/hello`
- Gets response: `{ message: 'Hello World' }`

### **Dynamic API Routes:**

```javascript
// pages/api/users/[id].js
export default function handler(req, res) {
  const id = req.query.id;  // Get ID from URL
  
  if (req.method === 'GET') {
    res.json({ userId: id, name: 'John' });
  }
  
  if (req.method === 'POST') {
    // Save data to database
    res.json({ success: true });
  }
}
```

**Usage:**
```
GET /api/users/5 → { userId: 5, name: 'John' }
POST /api/users/5 → Saves data
```

---

## **6. Data Fetching**

### **Simple Fetch:**

```javascript
// pages/posts.js
export async function getStaticProps() {
  // Fetch from API
  const res = await fetch('https://jsonplaceholder.typicode.com/posts');
  const posts = await res.json();
  
  return {
    props: { posts }
  };
}

export default function Posts({ posts }) {
  return (
    <div>
      {posts.map(post => (
        <h2 key={post.id}>{post.title}</h2>
      ))}
    </div>
  );
}
```

**What happens:**
1. Next.js fetches posts from API
2. Passes them as `props` to the component
3. Component displays the posts

---

## **7. Server Components vs Client Components**

### **Server Component (Default)**

Runs on the **server** only. Can access databases, secrets.

```javascript
// pages/users.js (This is a SERVER component)
async function getUsers() {
  const users = await database.getUsers();  // This works!
  return users;
}

export default async function UserList() {
  const users = await getUsers();
  
  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

---

### **Client Component**

Runs in the **browser**. Can use hooks like `useState`, `useEffect`.

```javascript
// 'use client' means this is a CLIENT component
'use client';

import { useState } from 'react';

export default function Counter() {
  const [count, setCount] = useState(0);
  
  return (
    <button onClick={() => setCount(count + 1)}>
      Count: {count}
    </button>
  );
}
```

---

## **8. Images & Fonts**

### **Next.js Image (Optimized)**

```javascript
import Image from 'next/image';

export default function Hero() {
  return (
    <Image
      src="/my-image.jpg"
      alt="Hero"
      width={800}
      height={600}
      priority  // Load immediately
    />
  );
}
```

**Benefits:**
- Automatically optimized (smaller file size)
- Lazy loading (loads only when visible)
- Responsive (works on all devices)

### **Next.js Fonts (Optimized)**

```javascript
import { Inter } from 'next/font/google';

const inter = Inter();

export default function Page() {
  return <p className={inter.className}>Hello World</p>;
}
```

**Benefits:**
- Fonts load super fast
- No extra requests to Google

---

## **9. Middleware (Security & Redirects)**

Run code **before** a page loads.

```javascript
// middleware.js (in root folder)
import { NextResponse } from 'next/server';

export function middleware(request) {
  // Check if user is logged in
  const token = request.cookies.get('token');
  
  if (!token && request.nextUrl.pathname === '/dashboard') {
    // Redirect to login
    return NextResponse.redirect(new URL('/login', request.url));
  }
  
  return NextResponse.next();
}

export const config = {
  matcher: ['/dashboard/:path*']  // Run only for /dashboard
};
```

**What happens:**
1. User tries to visit `/dashboard`
2. Middleware checks for login token
3. If no token → Redirects to `/login`
4. If token exists → Shows dashboard

---

## **10. SEO & Metadata**

Add titles, descriptions for Google search.

```javascript
// pages/blog/[slug].js
export async function generateMetadata({ params }) {
  const post = await fetch(`https://api.com/blog/${params.slug}`);
  const data = await post.json();
  
  return {
    title: data.title,
    description: data.excerpt,
    openGraph: {
      images: [data.image]
    }
  };
}

export default function BlogPost({ data }) {
  return <h1>{data.title}</h1>;
}
```

**Result:**
- When shared on Facebook/Twitter → Shows nice preview
- Google search results → Shows title & description

---

## **11. Authentication (Login System)**

### **Simple Login Example:**

```javascript
// pages/api/login.js
export default function handler(req, res) {
  if (req.method === 'POST') {
    const { email, password } = req.body;
    
    // Check if email & password are correct
    if (email === 'user@example.com' && password === '123') {
      // Set cookie (marks user as logged in)
      res.setHeader('Set-Cookie', 'token=abc123; Path=/');
      res.json({ success: true });
    } else {
      res.status(401).json({ error: 'Wrong password' });
    }
  }
}
```

### **Protected Page:**

```javascript
// middleware.js
export function middleware(request) {
  const token = request.cookies.get('token');
  
  // If no token and trying to access /dashboard
  if (!token && request.nextUrl.pathname === '/dashboard') {
    return NextResponse.redirect(new URL('/login', request.url));
  }
}
```

---

## **12. Styling**

### **CSS Modules:**

```css
/* styles/Button.module.css */
.button {
  padding: 10px;
  background: blue;
  color: white;
}
```

```javascript
// pages/index.js
import styles from '@/styles/Button.module.css';

export default function Home() {
  return <button className={styles.button}>Click me</button>;
}
```

### **Tailwind CSS:**

```javascript
export default function Home() {
  return (
    <div className="bg-blue-500 p-4 rounded-lg">
      <h1 className="text-white font-bold">Hello</h1>
    </div>
  );
}
```

---

## **13. Layout (Shared UI)**

Create layout that appears on **every page**.

```javascript
// app/layout.js
export default function RootLayout({ children }) {
  return (
    <html>
      <body>
        <header>🔗 Navigation</header>
        {children}
        <footer>© 2024</footer>
      </body>
    </html>
  );
}
```

**Result:**
- Every page has the same header & footer
- `{children}` is replaced with page content

---

## **14. Deployment (Publishing)**

### **Deploy to Vercel (1-click):**

1. Push code to GitHub
2. Connect GitHub to Vercel
3. Done! Your site is live 🚀

### **Deploy to Docker:**

```dockerfile
# Dockerfile
FROM node:18
WORKDIR /app
COPY . .
RUN npm install && npm run build
CMD ["npm", "start"]
```

```bash
docker build -t my-app .
docker run -p 3000:3000 my-app
```

---

## **Quick Summary:**

| Feature | What it does |
|---------|------------|
| **File-based routing** | Create files → Automatic URLs |
| **SSR** | Build page every visit (fresh data) |
| **SSG** | Build once at deploy time (fast) |
| **ISR** | Build once, refresh every X seconds |
| **CSR** | Build in browser (user-specific) |
| **API Routes** | Your own backend API |
| **Middleware** | Security checks before page loads |
| **Images** | Auto-optimized images |
| **Metadata** | SEO titles & descriptions |
| **Styling** | CSS Modules or Tailwind |
| **Layout** | Shared header/footer on all pages |
| **Deploy** | Push to Vercel or Docker |

---

**That's it!** You now understand **everything** in Next.js! 🎉
