# ▲ Next.js — Beginner to Production Guide

> **Goal:** Start with zero Next.js knowledge and finish with a practical mental model for building, deploying, debugging, and maintaining a real application.
>
> **Style used in this guide:** TypeScript + App Router + npm.
>
> **Companion guide:** `nestjs-beginner-to-production.md`

---

## 🧭 What you will learn

By the end, you should understand:

- what Next.js is and where it fits in a web application
- how React and Next.js relate
- how to create and run a project
- how the `app/` folder works
- pages, layouts, routing, dynamic routes, loading and errors
- Server Components vs Client Components
- props, state, events, hooks and forms
- styling with CSS/Tailwind
- fetching data from APIs
- Route Handlers and when to use them
- environment variables
- authentication concepts
- connecting a Next.js frontend to a NestJS backend
- testing, building and deployment
- performance, SEO, accessibility and security basics
- how to manage a Next.js application after launch

---

# 1. First: what is Next.js?

A normal modern web application can be pictured like this:

```text
User's browser
      │
      ▼
┌───────────────┐
│   Next.js     │  ← pages, buttons, forms, UI
│   Frontend    │
└───────┬───────┘
        │ HTTP / JSON
        ▼
┌───────────────┐
│   NestJS API  │  ← business logic, auth, validation
└───────┬───────┘
        │
        ▼
┌───────────────┐
│ PostgreSQL    │
└───────────────┘
```

**React** is mainly a library for building user interfaces from components.

**Next.js** is a React framework. It gives React an application structure and adds features such as:

- routing
- server rendering
- Server Components
- layouts
- data fetching
- API/Route Handlers
- image and font optimization
- metadata and SEO support
- production build tooling

A useful beginner mental model is:

> **React teaches you how to make UI components. Next.js teaches you how to organize those components into a complete web application.**

---

# 2. Prerequisites

You do **not** need to master everything before starting.

You should gradually become comfortable with:

| Topic | Minimum knowledge |
|---|---|
| HTML | elements, forms, links |
| CSS | selectors, flexbox, grid |
| JavaScript | variables, functions, arrays, objects, async/await |
| TypeScript | basic types and interfaces |
| React | components, props, state, events |
| Git | commit, push, pull |
| HTTP | GET, POST, PATCH, DELETE |

For current Next.js learning material, use a modern Node.js release. The official learning material currently requires Node.js 20.x or newer.

Check:

```bash
node -v
npm -v
```

---

# 3. JavaScript and TypeScript concepts you need most

## 3.1 Variables

```ts
const name = "Alex";
let count = 0;
```

Use `const` unless the variable itself needs to be reassigned.

## 3.2 Objects

```ts
const user = {
  id: 1,
  name: "Alex",
  email: "alex@example.com",
};
```

Access:

```ts
console.log(user.name);
```

## 3.3 Arrays

```ts
const users = ["Alex", "Maya", "Sam"];

const upper = users.map((user) => user.toUpperCase());
```

## 3.4 Functions

```ts
function add(a: number, b: number): number {
  return a + b;
}
```

Arrow form:

```ts
const add = (a: number, b: number) => a + b;
```

## 3.5 Async/await

Web applications constantly wait for databases and APIs.

```ts
async function loadUsers() {
  const response = await fetch("https://example.com/api/users");
  const users = await response.json();

  return users;
}
```

## 3.6 Interfaces

```ts
interface User {
  id: number;
  name: string;
  email: string;
}
```

Now TypeScript can protect you:

```ts
const user: User = {
  id: 1,
  name: "Alex",
  email: "alex@example.com",
};
```

---

# 4. React in five minutes

Before Next.js, understand this model:

```text
Component
   ├── receives props
   ├── may contain state
   ├── handles events
   └── returns UI
```

## 4.1 Component

```tsx
export default function Welcome() {
  return <h1>Hello!</h1>;
}
```

## 4.2 Props

```tsx
type GreetingProps = {
  name: string;
};

function Greeting({ name }: GreetingProps) {
  return <h2>Hello {name}</h2>;
}
```

Use it:

```tsx
<Greeting name="Alex" />
```

## 4.3 State

State is data that can change while a user interacts with the page.

```tsx
"use client";

import { useState } from "react";

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

# 5. Create your first Next.js application

Run:

```bash
npx create-next-app@latest taskflow-web
```

For a beginner, choose the recommended modern options:

```text
TypeScript: Yes
ESLint: Yes
Tailwind CSS: Yes
src/ directory: optional
App Router: Yes
Import alias: keep @/*
```

Then:

```bash
cd taskflow-web
npm run dev
```

Open:

```text
http://localhost:3000
```

Important commands:

```bash
npm run dev
npm run build
npm run start
npm run lint
```

Meaning:

| Command | Purpose |
|---|---|
| `npm run dev` | development server |
| `npm run build` | create production build |
| `npm run start` | run production build |
| `npm run lint` | detect code-quality problems |

---

# 6. Understand the project structure

A typical project:

```text
taskflow-web/
│
├── app/
│   ├── layout.tsx
│   ├── page.tsx
│   └── globals.css
│
├── public/
│   └── images/
│
├── components/
│   ├── Navbar.tsx
│   └── Button.tsx
│
├── lib/
│   └── api.ts
│
├── .env.local
├── next.config.ts
├── package.json
├── tsconfig.json
└── eslint.config.mjs
```

Think of them like this:

```text
app/          → routes and route-related UI
components/   → reusable UI
lib/          → reusable non-UI logic
public/       → static files
.env.local    → local secrets/config
package.json  → dependencies and scripts
```

---

# 7. The App Router

Modern Next.js uses **file-system routing**.

A folder is normally a URL segment.

```text
app/
├── page.tsx
├── about/
│   └── page.tsx
└── dashboard/
    └── page.tsx
```

This creates:

```text
/            → app/page.tsx
/about       → app/about/page.tsx
/dashboard   → app/dashboard/page.tsx
```

## 7.1 Home page

`app/page.tsx`

```tsx
export default function HomePage() {
  return (
    <main>
      <h1>TaskFlow</h1>
      <p>Manage your work simply.</p>
    </main>
  );
}
```

## 7.2 About page

`app/about/page.tsx`

```tsx
export default function AboutPage() {
  return <h1>About TaskFlow</h1>;
}
```

---

# 8. Navigation

Use Next.js `Link` instead of ordinary `<a>` for internal navigation.

```tsx
import Link from "next/link";

export default function Navbar() {
  return (
    <nav>
      <Link href="/">Home</Link>
      <Link href="/tasks">Tasks</Link>
      <Link href="/about">About</Link>
    </nav>
  );
}
```

---

# 9. Layouts

A layout wraps pages.

`app/layout.tsx`

```tsx
import "./globals.css";
import Navbar from "@/components/Navbar";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Navbar />
        {children}
      </body>
    </html>
  );
}
```

If the URL changes from `/tasks` to `/profile`, the shared layout does not have to be recreated like a completely separate HTML website.

You can also create nested layouts:

```text
app/
└── dashboard/
    ├── layout.tsx
    ├── page.tsx
    └── settings/
        └── page.tsx
```

---

# 10. Dynamic routes

Suppose you need:

```text
/tasks/10
/tasks/20
/tasks/300
```

You do not create 300 folders.

Create:

```text
app/
└── tasks/
    └── [id]/
        └── page.tsx
```

Conceptually, `[id]` means:

> "Whatever value appears here becomes a route parameter."

Example:

```tsx
type Props = {
  params: Promise<{ id: string }>;
};

export default async function TaskPage({ params }: Props) {
  const { id } = await params;

  return <h1>Task {id}</h1>;
}
```

---

# 11. Special route files

Next.js gives certain filenames special meaning.

```text
app/tasks/
├── page.tsx
├── layout.tsx
├── loading.tsx
├── error.tsx
└── not-found.tsx
```

### `page.tsx`

The actual page.

### `layout.tsx`

Shared UI around pages.

### `loading.tsx`

Temporary UI while content is loading.

```tsx
export default function Loading() {
  return <p>Loading tasks...</p>;
}
```

### `error.tsx`

Handles rendering errors in a route segment.

Because error boundaries are interactive, an error component is normally a Client Component.

### `not-found.tsx`

UI for missing content / 404-style cases.

---

# 12. Server Components vs Client Components

This is one of the most important concepts in modern Next.js.

## Server Component

By default, components in the App Router are Server Components.

```tsx
export default async function TasksPage() {
  const tasks = await getTasks();

  return <div>{/* render tasks */}</div>;
}
```

Good for:

- fetching data
- using server-only credentials
- reducing browser JavaScript
- rendering mostly non-interactive content

## Client Component

Add:

```tsx
"use client";
```

when the component requires browser interaction.

```tsx
"use client";

import { useState } from "react";

export default function LikeButton() {
  const [liked, setLiked] = useState(false);

  return (
    <button onClick={() => setLiked(!liked)}>
      {liked ? "Liked" : "Like"}
    </button>
  );
}
```

Use Client Components when you need things such as:

- `useState`
- `useEffect`
- click handlers
- browser APIs
- local interactive state

### Easy rule

```text
Need browser interaction?
      │
      ├─ No  → Server Component is usually fine
      │
      └─ Yes → Client Component
```

Do not put `"use client"` everywhere. Keep the client boundary as small as practical.

---

# 13. Styling

You have several choices.

## Global CSS

`app/globals.css`

```css
body {
  margin: 0;
  font-family: Arial, sans-serif;
}
```

## CSS Modules

`TaskCard.module.css`

```css
.card {
  border: 1px solid #ddd;
  padding: 16px;
  border-radius: 12px;
}
```

`TaskCard.tsx`

```tsx
import styles from "./TaskCard.module.css";

export default function TaskCard() {
  return <article className={styles.card}>Learn Next.js</article>;
}
```

## Tailwind CSS

```tsx
export default function Button() {
  return (
    <button className="rounded-lg px-4 py-2 font-medium shadow">
      Save
    </button>
  );
}
```

For fast product development, Tailwind is a practical choice, but learning basic CSS first will make Tailwind much easier to understand.

---

# 14. Reusable components

Avoid repeating UI.

Instead of this:

```tsx
<button className="...">Save</button>
<button className="...">Delete</button>
<button className="...">Edit</button>
```

Create:

```tsx
type ButtonProps = {
  children: React.ReactNode;
  onClick?: () => void;
};

export default function Button({ children, onClick }: ButtonProps) {
  return (
    <button onClick={onClick} className="rounded-lg border px-4 py-2">
      {children}
    </button>
  );
}
```

Now reuse:

```tsx
<Button>Save</Button>
<Button>Delete</Button>
```

A real application usually grows into reusable layers:

```text
components/
├── ui/
│   ├── Button.tsx
│   ├── Input.tsx
│   └── Modal.tsx
└── tasks/
    ├── TaskCard.tsx
    └── TaskForm.tsx
```

---

# 15. Forms

A Client Component example:

```tsx
"use client";

import { FormEvent, useState } from "react";

export default function TaskForm() {
  const [title, setTitle] = useState("");

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    console.log({ title });
  }

  return (
    <form onSubmit={handleSubmit}>
      <input
        value={title}
        onChange={(event) => setTitle(event.target.value)}
        placeholder="Task title"
      />

      <button type="submit">Create</button>
    </form>
  );
}
```

Flow:

```text
User types
   ↓
state changes
   ↓
user submits
   ↓
handleSubmit()
   ↓
send data to backend
```

---

# 16. HTTP and REST basics

Your Next.js application will often communicate with a backend.

Typical operations:

| Action | HTTP method | Example |
|---|---|---|
| Get tasks | GET | `/tasks` |
| Get one task | GET | `/tasks/12` |
| Create task | POST | `/tasks` |
| Update task | PATCH | `/tasks/12` |
| Delete task | DELETE | `/tasks/12` |

Data is commonly transferred as JSON:

```json
{
  "id": 12,
  "title": "Learn Next.js",
  "completed": false
}
```

---

# 17. Fetch data from an API

Suppose NestJS runs at:

```text
http://localhost:3001
```

and has:

```text
GET /tasks
```

Create:

`lib/tasks.ts`

```ts
export type Task = {
  id: number;
  title: string;
  completed: boolean;
};

export async function getTasks(): Promise<Task[]> {
  const response = await fetch(`${process.env.API_URL}/tasks`, {
    cache: "no-store",
  });

  if (!response.ok) {
    throw new Error("Failed to load tasks");
  }

  return response.json();
}
```

Then:

`app/tasks/page.tsx`

```tsx
import { getTasks } from "@/lib/tasks";

export default async function TasksPage() {
  const tasks = await getTasks();

  return (
    <main>
      <h1>Tasks</h1>

      <ul>
        {tasks.map((task) => (
          <li key={task.id}>{task.title}</li>
        ))}
      </ul>
    </main>
  );
}
```

The important mental model:

```text
TasksPage
   ↓
getTasks()
   ↓
HTTP GET
   ↓
NestJS
   ↓
Database
   ↓
JSON response
   ↓
render page
```

---

# 18. Caching: understand the idea before the details

Caching means:

> Reuse previously obtained data instead of doing the expensive work every time.

For data that must always be fresh, you may use:

```ts
fetch(url, {
  cache: "no-store",
});
```

For data that changes less often, use the caching/revalidation features appropriate to the current Next.js version and your route.

Do not memorize every caching API on day one. First understand the design question:

```text
Does this data need to be fresh on every request?
```

Examples:

```text
User bank balance          → very fresh
Public documentation       → can often be cached
Product inventory          → depends on business requirements
Profile picture URL        → usually cacheable
```

---

# 19. Route Handlers

Next.js can also expose HTTP endpoints.

Example:

```text
app/api/hello/route.ts
```

```ts
export async function GET() {
  return Response.json({
    message: "Hello from Next.js",
  });
}
```

Request:

```text
GET /api/hello
```

### But if you already have NestJS...

Use NestJS for your main backend domain logic.

A clean architecture can be:

```text
Next.js
  ├── UI
  ├── server rendering
  ├── frontend-specific server logic
  └── optional lightweight Route Handlers

NestJS
  ├── REST API
  ├── authentication/authorization
  ├── business rules
  ├── database access
  ├── queues/jobs
  └── integrations
```

Do not duplicate the same business logic in both frameworks.

---

# 20. Environment variables

Never hard-code credentials in source code.

Create:

`.env.local`

```env
API_URL=http://localhost:3001
NEXT_PUBLIC_APP_NAME=TaskFlow
```

Use server-only variable:

```ts
process.env.API_URL
```

Variables prefixed with:

```text
NEXT_PUBLIC_
```

can be exposed to browser-side code.

So this is dangerous:

```env
NEXT_PUBLIC_DATABASE_PASSWORD=secret
```

Never expose database passwords, private API keys, signing secrets or server credentials to the browser.

---

# 21. Authentication — mental model

Authentication asks:

> **Who are you?**

Authorization asks:

> **What are you allowed to do?**

Typical login flow:

```text
User
 │
 │ email + password
 ▼
Next.js login page
 │
 │ POST /auth/login
 ▼
NestJS
 │
 ├─ check user
 ├─ verify password
 └─ issue session/token
 │
 ▼
Next.js stores/uses authenticated session
```

Common approaches:

- Auth.js
- Clerk
- Auth0
- custom authentication in NestJS using sessions/JWT

For a serious application, understand cookie security, token expiry, refresh strategies, CSRF and XSS before designing authentication yourself.

---

# 22. Local state vs server state

Not every piece of data belongs in `useState`.

## Local UI state

Examples:

```text
Is modal open?
Current tab?
Current text input?
```

Use:

```ts
useState()
```

## Server state

Examples:

```text
Users
Products
Orders
Tasks
```

This data comes from an API/database.

Depending on your application, you can use:

- Server Components and `fetch`
- React query libraries such as TanStack Query for client-heavy applications

Do not introduce a global state library just because the project exists.

---

# 23. Build our mini application: TaskFlow

We will create:

```text
/
├── /
├── /tasks
└── /tasks/[id]
```

The NestJS backend provides:

```text
GET    /tasks
GET    /tasks/:id
POST   /tasks
PATCH  /tasks/:id
DELETE /tasks/:id
```

Suggested frontend structure:

```text
taskflow-web/
├── app/
│   ├── layout.tsx
│   ├── page.tsx
│   └── tasks/
│       ├── page.tsx
│       └── [id]/
│           └── page.tsx
│
├── components/
│   ├── Navbar.tsx
│   └── tasks/
│       ├── CreateTaskForm.tsx
│       └── TaskCard.tsx
│
└── lib/
    └── tasks.ts
```

---

# 24. Task card

```tsx
type TaskCardProps = {
  id: number;
  title: string;
  completed: boolean;
};

export default function TaskCard({
  id,
  title,
  completed,
}: TaskCardProps) {
  return (
    <article className="rounded-xl border p-4">
      <h2>{title}</h2>
      <p>{completed ? "Completed" : "Pending"}</p>
      <small>ID: {id}</small>
    </article>
  );
}
```

Use it:

```tsx
<TaskCard
  id={task.id}
  title={task.title}
  completed={task.completed}
/>
```

---

# 25. Create a task from the browser

`components/tasks/CreateTaskForm.tsx`

```tsx
"use client";

import { FormEvent, useState } from "react";

export default function CreateTaskForm() {
  const [title, setTitle] = useState("");
  const [message, setMessage] = useState("");

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const response = await fetch(
      `${process.env.NEXT_PUBLIC_API_URL}/tasks`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ title }),
      }
    );

    if (!response.ok) {
      setMessage("Could not create task.");
      return;
    }

    setTitle("");
    setMessage("Task created.");
  }

  return (
    <form onSubmit={handleSubmit}>
      <input
        value={title}
        onChange={(event) => setTitle(event.target.value)}
        placeholder="New task"
      />

      <button type="submit">Create</button>

      <p>{message}</p>
    </form>
  );
}
```

`.env.local`

```env
API_URL=http://localhost:3001
NEXT_PUBLIC_API_URL=http://localhost:3001
```

Why two variables?

```text
API_URL
→ server-only code can use it

NEXT_PUBLIC_API_URL
→ browser code can use it
```

For production architectures, you may prefer to avoid directly exposing the backend URL and instead use a same-origin/backend-for-frontend pattern. Learn the simple direct model first.

---

# 26. Input validation

Validation must happen at more than one level.

```text
Frontend validation
→ improves user experience

Backend validation
→ protects the application
```

Never trust browser validation as your security boundary.

Example client check:

```ts
if (title.trim().length < 3) {
  setMessage("Title must contain at least 3 characters.");
  return;
}
```

The NestJS backend should independently enforce the same real business rule.

---

# 27. Handling errors

Do not write code as though every API request succeeds.

```ts
try {
  const response = await fetch(url);

  if (!response.ok) {
    throw new Error("Request failed");
  }

  const data = await response.json();
} catch (error) {
  console.error(error);
}
```

Your UI should have intentional states:

```text
loading
success
empty
error
unauthorized
not found
```

Good products design all six.

---

# 28. Loading UI

Example:

`app/tasks/loading.tsx`

```tsx
export default function Loading() {
  return (
    <main>
      <h1>Tasks</h1>
      <p>Loading...</p>
    </main>
  );
}
```

Later, replace simple text with skeleton components.

---

# 29. Images

Use Next.js image tooling for application images when appropriate.

```tsx
import Image from "next/image";

export default function Logo() {
  return (
    <Image
      src="/logo.png"
      alt="TaskFlow logo"
      width={120}
      height={40}
    />
  );
}
```

Put the file in:

```text
public/logo.png
```

Always provide meaningful `alt` text for meaningful images.

---

# 30. Metadata and SEO

Example:

```tsx
export const metadata = {
  title: "TaskFlow",
  description: "Simple task management",
};
```

You can define metadata globally or per route.

Important SEO concepts:

- descriptive page titles
- useful descriptions
- semantic HTML
- crawlable content
- good performance
- canonical URLs when needed
- Open Graph/social metadata

---

# 31. Accessibility

Accessibility is not an optional finishing step.

Prefer:

```tsx
<button>Save</button>
```

over:

```tsx
<div onClick={save}>Save</div>
```

Use:

- semantic elements
- proper labels
- keyboard navigation
- visible focus states
- sufficient contrast
- meaningful alt text
- correct heading hierarchy

Example:

```tsx
<label htmlFor="title">Task title</label>
<input id="title" name="title" />
```

---

# 32. Useful application organization

As the application grows:

```text
app/
components/
lib/
types/
hooks/
```

One reasonable example:

```text
src/
├── app/
│   ├── (auth)/
│   ├── dashboard/
│   └── tasks/
│
├── components/
│   ├── ui/
│   ├── layout/
│   └── tasks/
│
├── lib/
│   ├── api/
│   ├── auth/
│   └── utils/
│
├── hooks/
└── types/
```

Do not over-engineer folder structure before the application needs it.

---

# 33. Route groups

Parentheses can organize routes without adding a URL segment.

```text
app/
├── (marketing)/
│   ├── about/
│   └── pricing/
└── (app)/
    ├── dashboard/
    └── settings/
```

The parentheses are useful for code organization and layout boundaries.

---

# 34. Search parameters

A URL like:

```text
/tasks?status=pending&page=2
```

contains search parameters.

These are useful for:

- pagination
- filtering
- sorting
- search

A major benefit of URL-based state is shareability:

```text
/tasks?status=completed
```

can be bookmarked and shared.

---

# 35. Do you need Redux?

Usually, **not at the beginning**.

Start with:

```text
Server Components
props
useState
URL state
context when appropriate
```

Add a state-management library only when you can clearly describe the problem it solves.

Possible later tools:

- Zustand
- Redux Toolkit
- TanStack Query for server state

---

# 36. Testing

Think in layers.

## Unit tests

Test a small function/component.

## Integration tests

Test multiple pieces working together.

## End-to-end tests

Test like a real user:

```text
open login
→ log in
→ create task
→ see task
→ edit task
→ delete task
```

Common tools include:

- Vitest/Jest
- React Testing Library
- Playwright

Your production confidence should not depend only on manually clicking around.

---

# 37. Before production: run a production build

Always test:

```bash
npm run build
```

Then:

```bash
npm run start
```

Development mode and production mode are not identical.

If `npm run build` fails, your application is not ready to deploy.

---

# 38. Deployment

The simplest Next.js deployment experience is commonly Vercel.

Typical Git workflow:

```text
local project
   ↓
GitHub repository
   ↓
Vercel project
   ↓
production deployment
```

You can also deploy Next.js using other Node/container/cloud hosting options depending on the features your application uses.

Production environment variables must be configured on the hosting platform. Your local `.env.local` does not magically appear on the server.

---

# 39. Development, staging and production

Professional applications often have separate environments:

```text
Development
→ your computer

Staging / Preview
→ safe environment for testing

Production
→ real users
```

Do not use your production database for casual development experiments.

Example:

```text
Development API: http://localhost:3001
Staging API:     https://api-staging.example.com
Production API:  https://api.example.com
```

---

# 40. Git workflow

A simple solo workflow:

```bash
git checkout -b feature/task-form

# make changes

git add .
git commit -m "Add task creation form"
git push -u origin feature/task-form
```

Then merge after reviewing/testing.

Good commit:

```text
Add task creation form validation
```

Bad commit:

```text
changes
```

---

# 41. Dependency management

Your project dependencies are recorded in:

```text
package.json
package-lock.json
```

Install:

```bash
npm install package-name
```

Remove:

```bash
npm uninstall package-name
```

Inspect outdated packages:

```bash
npm outdated
```

Do not blindly upgrade every major version in a production application. Read migration notes and test.

---

# 42. Logs and monitoring

After deployment, "it worked on my machine" is no longer enough.

Monitor:

- build failures
- runtime exceptions
- slow requests
- frontend errors
- failed API requests
- Core Web Vitals
- unusual authentication failures

Possible services include platform logs plus tools such as Sentry or other observability platforms.

Never log secrets or passwords.

---

# 43. Security basics

At minimum:

- keep secrets server-side
- validate all backend input
- escape/render user content safely
- use HTTPS
- use secure cookies appropriately
- keep dependencies updated
- enforce authorization on the server
- use rate limiting for sensitive APIs
- do not trust hidden buttons as authorization
- protect file uploads
- configure security headers where appropriate

A user hiding an "Admin" button in the browser does **not** make an admin endpoint secure. The backend must enforce permission.

---

# 44. Performance basics

Before optimizing, measure.

High-value principles:

- prefer Server Components when interactivity is unnecessary
- reduce unnecessary browser JavaScript
- optimize images
- avoid huge client bundles
- fetch independent data in parallel where possible
- use caching intentionally
- paginate large data sets
- avoid unnecessary re-renders
- use loading/streaming where it improves experience

Test production builds rather than judging performance only from development mode.

---

# 45. Debugging workflow

When something breaks, avoid random editing.

Use:

```text
1. Read the exact error.
2. Identify the file and line.
3. Reproduce consistently.
4. Decide: frontend, network, backend, or database?
5. Inspect browser console.
6. Inspect Network tab.
7. Inspect server logs.
8. Check environment variables.
9. Make one controlled change.
10. Test again.
```

### Example

Browser says:

```text
POST http://localhost:3001/tasks 400
```

That means:

```text
Next.js reached NestJS
but NestJS rejected the request
```

So inspect:

- request JSON
- Nest DTO validation
- backend logs

If you see:

```text
ERR_CONNECTION_REFUSED
```

then the API may not be running or the URL/port may be wrong.

---

# 46. How Next.js and NestJS should work together

A clean production-oriented structure:

```text
Browser
   │
   ▼
Next.js
   │
   │ HTTPS / JSON
   ▼
NestJS
   │
   ├── authentication
   ├── validation
   ├── business logic
   └── database access
   │
   ▼
PostgreSQL
```

Possible repository structure:

```text
my-product/
├── web/       # Next.js
└── api/       # NestJS
```

Or two repositories:

```text
my-product-web
my-product-api
```

Both approaches can work. A monorepo becomes useful when shared tooling/types and coordinated deployments justify it.

---

# 47. A practical learning sequence

## Stage 1 — React foundations

Build:

```text
counter
todo list
simple form
filterable list
```

Learn:

- components
- props
- state
- events
- map
- conditional rendering

## Stage 2 — Next.js fundamentals

Build:

```text
3-page website
dynamic profile route
shared layout
loading page
not-found page
```

## Stage 3 — API integration

Build:

```text
task list from NestJS
create task
update task
delete task
```

## Stage 4 — Authentication

Add:

```text
register
login
logout
protected dashboard
role-based UI
```

## Stage 5 — Production

Add:

```text
validation
tests
logging
error monitoring
staging
deployment
CI
```

---

# 48. Seven-day beginner practice plan

### Day 1

Learn:

```text
React components
props
state
events
```

Build a counter and todo list.

### Day 2

Create Next.js project and understand:

```text
app/
page.tsx
layout.tsx
Link
```

### Day 3

Learn:

```text
dynamic routes
Server Components
Client Components
loading/error states
```

### Day 4

Build TaskFlow UI with mock data.

### Day 5

Connect to the NestJS API.

### Day 6

Implement create/update/delete.

### Day 7

Run:

```bash
npm run lint
npm run build
npm run start
```

Then deploy a preview version.

---

# 49. Beginner mistakes to avoid

### Mistake 1

Putting `"use client"` in every file.

**Better:** keep server rendering as the default unless browser interactivity is required.

### Mistake 2

Calling the database directly from browser code.

**Better:** database credentials and privileged database logic stay server-side.

### Mistake 3

Hard-coding API URLs everywhere.

**Better:** centralize API configuration/environment variables.

### Mistake 4

Putting the entire app in one component.

**Better:** split by responsibility.

### Mistake 5

Adding many libraries before understanding the problem.

**Better:** start with framework primitives.

### Mistake 6

Ignoring errors.

**Better:** explicitly design loading, empty and error states.

### Mistake 7

Testing only with `npm run dev`.

**Better:** regularly run the production build.

---

# 50. Next.js cheat sheet

```text
app/page.tsx                  → /
app/about/page.tsx            → /about
app/tasks/[id]/page.tsx       → /tasks/:id

layout.tsx                    → shared layout
loading.tsx                   → loading UI
error.tsx                     → error boundary UI
not-found.tsx                 → not-found UI
route.ts                      → HTTP Route Handler

"use client"                 → make client boundary
process.env.NAME              → server environment variable
process.env.NEXT_PUBLIC_X     → browser-visible variable

npm run dev                   → development
npm run build                 → production build
npm run start                 → production server
npm run lint                  → lint
```

---

# 51. What "I know Next.js" should eventually mean

You do **not** need to memorize the entire framework.

You should be able to explain:

1. React vs Next.js.
2. How file-system routing works.
3. What `page.tsx` and `layout.tsx` do.
4. Server vs Client Components.
5. How data reaches a page.
6. How forms send data.
7. Where environment variables belong.
8. How authentication fits into the architecture.
9. How Next.js communicates with a backend.
10. How to build and deploy production code.
11. How to diagnose browser/network/server errors.
12. How to safely upgrade and maintain the project.

If you can build a CRUD application with authentication, validation, error handling and deployment **and explain why each piece exists**, you have moved beyond beginner level.

---

# 52. Recommended full-stack architecture for practice

```text
┌──────────────────────────────────────────┐
│                  USER                    │
└────────────────────┬─────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────┐
│                NEXT.JS                   │
│                                          │
│  Pages • Layouts • Components • Forms    │
│  Server rendering • Client interaction   │
└────────────────────┬─────────────────────┘
                     │ REST/JSON
                     ▼
┌──────────────────────────────────────────┐
│                 NESTJS                   │
│                                          │
│ Controllers • Services • Validation      │
│ Auth • Business logic • API              │
└────────────────────┬─────────────────────┘
                     │ Prisma
                     ▼
┌──────────────────────────────────────────┐
│               POSTGRESQL                 │
└──────────────────────────────────────────┘
```

Learn each layer separately, then connect them.

---

# 53. Official references

Use tutorials to learn, but use official documentation to verify framework behavior.

- Next.js documentation: https://nextjs.org/docs
- Next.js Learn: https://nextjs.org/learn
- React documentation: https://react.dev/
- TypeScript handbook: https://www.typescriptlang.org/docs/handbook/intro.html

---

# ✅ Final checkpoint

You are ready to build a real Next.js frontend when you can do this without copying an entire tutorial:

```text
Create project
   ↓
Create routes/layout
   ↓
Build reusable components
   ↓
Add interactive forms
   ↓
Fetch data from NestJS
   ↓
Handle loading/errors
   ↓
Protect authenticated pages
   ↓
Build
   ↓
Test
   ↓
Deploy
   ↓
Monitor and maintain
```

**Next:** open the companion NestJS guide and build the API that powers this frontend.
