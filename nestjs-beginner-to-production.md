# 🐈 NestJS — Beginner to Production Guide

> **Goal:** Learn NestJS from first principles and finish with a practical understanding of building, securing, deploying, debugging, and maintaining a real backend API.
>
> **Style used in this guide:** TypeScript + REST + PostgreSQL + Prisma-style examples + npm.
>
> **Companion guide:** `nextjs-beginner-to-production.md`

---

## 🧭 What you will learn

By the end, you should understand:

- what a backend does
- Node.js vs Express vs NestJS
- NestJS project structure
- modules, controllers, services and dependency injection
- decorators
- REST APIs and HTTP
- DTOs and request validation
- middleware, pipes, guards, interceptors and exception filters
- configuration and environment variables
- PostgreSQL and Prisma integration concepts
- authentication and authorization
- Swagger/OpenAPI
- CORS and connecting a Next.js frontend
- testing
- logging and error handling
- production builds and deployment
- migrations and database safety
- how to maintain and scale a NestJS application

---

# 1. First: what is NestJS?

A complete application often looks like:

```text
User
 │
 ▼
Next.js frontend
 │
 │ HTTP request
 ▼
NestJS backend
 │
 ├── validate request
 ├── authenticate user
 ├── apply business rules
 ├── read/write data
 │
 ▼
PostgreSQL
```

**Node.js** lets JavaScript/TypeScript run outside the browser.

**Express** is a lightweight HTTP server framework.

**NestJS** is a structured backend framework built for Node.js. It gives you conventions and architecture for larger applications.

A beginner mental model:

> **NestJS is where you build the application's server-side API and business logic.**

Nest uses architectural ideas such as:

```text
Modules
Controllers
Providers / Services
Dependency Injection
Decorators
```

---

# 2. What does a backend actually do?

Suppose a user presses:

```text
Create Task
```

The frontend sends:

```http
POST /tasks
Content-Type: application/json
```

```json
{
  "title": "Learn NestJS"
}
```

The NestJS backend might:

```text
1. receive request
2. validate title
3. identify user
4. check permission
5. apply business rules
6. save task in PostgreSQL
7. return JSON response
```

Response:

```json
{
  "id": 42,
  "title": "Learn NestJS",
  "completed": false
}
```

---

# 3. Prerequisites

You mainly need:

| Topic | Minimum knowledge |
|---|---|
| JavaScript | functions, objects, arrays, async/await |
| TypeScript | types, classes, interfaces |
| HTTP | GET, POST, PATCH, DELETE |
| JSON | objects and arrays |
| SQL | basic table/row concepts |
| Git | basic version control |

Current NestJS documentation requires a modern Node.js version; Node.js 20+ is the safe baseline for the standard current setup.

Check:

```bash
node -v
npm -v
```

---

# 4. Important TypeScript concepts

NestJS uses TypeScript heavily.

## Classes

```ts
class UserService {
  getUser() {
    return "Alex";
  }
}
```

## Constructor

```ts
class UserService {
  constructor(private readonly name: string) {}
}
```

## Types

```ts
type User = {
  id: number;
  name: string;
};
```

## Async functions

```ts
async function getUser() {
  const user = await database.user.findFirst();
  return user;
}
```

## Decorators

NestJS uses decorators everywhere.

```ts
@Controller("users")
export class UsersController {}
```

You can initially think of a decorator as:

> metadata that tells NestJS what role a class or method has.

---

# 5. Create your first NestJS project

Install Nest CLI:

```bash
npm install -g @nestjs/cli
```

Create project:

```bash
nest new taskflow-api
```

Choose npm.

Then:

```bash
cd taskflow-api
npm run start:dev
```

By default, the generated app commonly listens on port 3000.

Because our Next.js frontend will use port 3000, change NestJS to port **3001**.

`src/main.ts`

```ts
import { NestFactory } from "@nestjs/core";
import { AppModule } from "./app.module";

async function bootstrap() {
  const app = await NestFactory.create(AppModule);

  await app.listen(process.env.PORT ?? 3001);
}

bootstrap();
```

Run:

```bash
npm run start:dev
```

Now:

```text
NestJS: http://localhost:3001
Next.js: http://localhost:3000
```

---

# 6. Understand the generated files

Typical structure:

```text
src/
├── app.controller.spec.ts
├── app.controller.ts
├── app.module.ts
├── app.service.ts
└── main.ts
```

Mental model:

```text
main.ts
  │
  ▼
AppModule
  │
  ├── Controllers
  └── Providers/Services
```

### `main.ts`

Starts the application.

### `app.module.ts`

Root module that connects application parts.

### `app.controller.ts`

Handles HTTP requests.

### `app.service.ts`

Contains reusable/application logic.

---

# 7. The three most important NestJS pieces

If you understand these, NestJS becomes much easier.

```text
Module
   │
   ├── Controller
   │      ↓
   │   receives HTTP request
   │
   └── Service
          ↓
       does the work
```

## Controller

```text
"What request came in?"
```

## Service

```text
"What should the application do?"
```

## Module

```text
"Which pieces belong together?"
```

---

# 8. Controllers

Controllers receive requests.

Generate one:

```bash
nest generate controller tasks
```

Short form:

```bash
nest g controller tasks
```

Example:

`src/tasks/tasks.controller.ts`

```ts
import { Controller, Get } from "@nestjs/common";

@Controller("tasks")
export class TasksController {
  @Get()
  findAll() {
    return [
      {
        id: 1,
        title: "Learn NestJS",
        completed: false,
      },
    ];
  }
}
```

Request:

```http
GET /tasks
```

Response:

```json
[
  {
    "id": 1,
    "title": "Learn NestJS",
    "completed": false
  }
]
```

---

# 9. Route parameters

Request:

```text
GET /tasks/25
```

Controller:

```ts
import { Controller, Get, Param } from "@nestjs/common";

@Controller("tasks")
export class TasksController {
  @Get(":id")
  findOne(@Param("id") id: string) {
    return {
      id,
      title: "Example task",
    };
  }
}
```

`:id` means:

```text
anything in this URL position becomes id
```

---

# 10. Request body

For:

```http
POST /tasks
```

with:

```json
{
  "title": "Build API"
}
```

use:

```ts
import { Body, Controller, Post } from "@nestjs/common";

@Controller("tasks")
export class TasksController {
  @Post()
  create(@Body() body: { title: string }) {
    return body;
  }
}
```

This works, but a real NestJS project should normally use a DTO.

---

# 11. Services

Controllers should not contain all business logic.

Generate:

```bash
nest g service tasks
```

`src/tasks/tasks.service.ts`

```ts
import { Injectable } from "@nestjs/common";

@Injectable()
export class TasksService {
  findAll() {
    return [
      {
        id: 1,
        title: "Learn NestJS",
        completed: false,
      },
    ];
  }
}
```

Controller:

```ts
import { Controller, Get } from "@nestjs/common";
import { TasksService } from "./tasks.service";

@Controller("tasks")
export class TasksController {
  constructor(private readonly tasksService: TasksService) {}

  @Get()
  findAll() {
    return this.tasksService.findAll();
  }
}
```

Now the responsibility is clearer:

```text
Controller
→ HTTP

Service
→ application logic
```

---

# 12. Dependency Injection

This line is extremely important:

```ts
constructor(private readonly tasksService: TasksService) {}
```

You did **not** write:

```ts
const tasksService = new TasksService();
```

NestJS creates and supplies the dependency.

That is **Dependency Injection (DI)**.

Why useful?

- easier testing
- looser coupling
- centralized lifecycle
- replace implementations
- cleaner architecture

Mental model:

```text
Controller says:
"I require TasksService."

NestJS says:
"I know how to create it. Here it is."
```

---

# 13. Modules

Generate:

```bash
nest g module tasks
```

`src/tasks/tasks.module.ts`

```ts
import { Module } from "@nestjs/common";
import { TasksController } from "./tasks.controller";
import { TasksService } from "./tasks.service";

@Module({
  controllers: [TasksController],
  providers: [TasksService],
})
export class TasksModule {}
```

Register it in:

`src/app.module.ts`

```ts
import { Module } from "@nestjs/common";
import { TasksModule } from "./tasks/tasks.module";

@Module({
  imports: [TasksModule],
})
export class AppModule {}
```

For a real app:

```text
src/
├── auth/
├── users/
├── tasks/
├── notifications/
└── common/
```

Each feature can have its own module.

---

# 14. Generate a resource quickly

Nest CLI can scaffold a feature:

```bash
nest g resource tasks
```

It can generate common CRUD pieces.

This is useful, but do not let code generation replace understanding.

You should still know what:

```text
module
controller
service
DTO
entity/model
```

mean.

---

# 15. REST API design

A clean task API:

| Operation | Method | Route |
|---|---|---|
| list tasks | GET | `/tasks` |
| get task | GET | `/tasks/:id` |
| create task | POST | `/tasks` |
| update task | PATCH | `/tasks/:id` |
| delete task | DELETE | `/tasks/:id` |

Controller skeleton:

```ts
@Controller("tasks")
export class TasksController {
  @Get()
  findAll() {}

  @Get(":id")
  findOne() {}

  @Post()
  create() {}

  @Patch(":id")
  update() {}

  @Delete(":id")
  remove() {}
}
```

---

# 16. DTOs

DTO = **Data Transfer Object**.

A DTO defines the shape of incoming data.

`src/tasks/dto/create-task.dto.ts`

```ts
export class CreateTaskDto {
  title: string;
}
```

Controller:

```ts
@Post()
create(@Body() createTaskDto: CreateTaskDto) {
  return this.tasksService.create(createTaskDto);
}
```

A DTO is more than a TypeScript convenience when combined with runtime validation.

---

# 17. Validation

Install:

```bash
npm install class-validator class-transformer
```

DTO:

```ts
import { IsString, MinLength } from "class-validator";

export class CreateTaskDto {
  @IsString()
  @MinLength(3)
  title: string;
}
```

Enable validation globally in `main.ts`:

```ts
import { ValidationPipe } from "@nestjs/common";

async function bootstrap() {
  const app = await NestFactory.create(AppModule);

  app.useGlobalPipes(
    new ValidationPipe({
      whitelist: true,
      transform: true,
    })
  );

  await app.listen(process.env.PORT ?? 3001);
}
```

Now this:

```json
{
  "title": ""
}
```

can be rejected before bad data reaches your business logic.

Important:

> Frontend validation improves UX. Backend validation protects the system.

---

# 18. Parse and validate route values

A route parameter arrives as text.

Instead of manually converting:

```ts
const numericId = Number(id);
```

you can use a pipe:

```ts
@Get(":id")
findOne(@Param("id", ParseIntPipe) id: number) {
  return this.tasksService.findOne(id);
}
```

Now Nest rejects invalid integer IDs.

---

# 19. Request lifecycle mental model

A simplified NestJS request can pass through layers like:

```text
Incoming request
      ↓
Middleware
      ↓
Guard
      ↓
Interceptor (before)
      ↓
Pipe / validation
      ↓
Controller
      ↓
Service
      ↓
Interceptor (after)
      ↓
Response
```

If an exception occurs, an exception filter may handle it.

Do not memorize this immediately. Learn what problem each tool solves.

---

# 20. Middleware

Middleware runs early in request processing.

Possible uses:

- logging
- request IDs
- some request preprocessing

Conceptually:

```ts
use(req, res, next) {
  console.log(req.method, req.url);
  next();
}
```

For many application concerns, Nest-specific guards/interceptors are more expressive than generic middleware.

---

# 21. Pipes

Pipes are mainly for:

- transformation
- validation

Examples:

```text
"42" → 42
invalid UUID → reject
invalid body → reject
```

Common built-ins include:

```text
ValidationPipe
ParseIntPipe
ParseBoolPipe
ParseUUIDPipe
```

---

# 22. Guards

Guards decide whether a request may continue.

Perfect for:

- authentication
- roles
- permissions

Mental model:

```text
Request
  ↓
Guard asks "allowed?"
  ├── no  → reject
  └── yes → controller
```

Examples:

```text
JwtAuthGuard
RolesGuard
AdminGuard
```

---

# 23. Interceptors

Interceptors wrap execution.

Useful for:

- logging execution time
- transforming responses
- caching
- tracing
- consistent response formatting

Mental model:

```text
before controller
      ↓
 controller
      ↓
after controller
```

---

# 24. Exception filters

They control how errors become HTTP responses.

Example situations:

```text
Database record missing
Authentication failed
Unexpected internal error
```

Nest already provides standard HTTP exceptions:

```ts
throw new NotFoundException("Task not found");
```

Use correct status semantics:

```text
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
500 Internal Server Error
```

Do not return `200 OK` for every failure.

---

# 25. Configuration and `.env`

Install:

```bash
npm install @nestjs/config
```

`.env`

```env
PORT=3001
DATABASE_URL=postgresql://user:password@localhost:5432/taskflow
JWT_SECRET=replace-this
```

`app.module.ts`

```ts
import { Module } from "@nestjs/common";
import { ConfigModule } from "@nestjs/config";

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
    }),
  ],
})
export class AppModule {}
```

Read:

```ts
process.env.PORT
```

or inject Nest's configuration service for more structured configuration.

Never commit production secrets.

Add `.env` to `.gitignore`.

---

# 26. Database concepts

Before Prisma, understand the database itself.

Example table:

```text
tasks
┌────┬───────────────────┬───────────┐
│ id │ title             │ completed │
├────┼───────────────────┼───────────┤
│ 1  │ Learn NestJS      │ false     │
│ 2  │ Build API         │ true      │
└────┴───────────────────┴───────────┘
```

Important terms:

```text
table
row
column
primary key
foreign key
index
constraint
transaction
migration
```

ORMs are useful, but you still need basic relational database knowledge.

---

# 27. What is Prisma?

Prisma is an ORM/data-access toolkit.

Instead of writing raw SQL every time:

```sql
SELECT * FROM tasks WHERE id = 10;
```

you can use generated typed APIs conceptually like:

```ts
prisma.task.findUnique({
  where: { id: 10 },
});
```

Prisma does **not** replace PostgreSQL.

```text
NestJS
   ↓
Prisma
   ↓
PostgreSQL
```

---

# 28. Prisma setup — current architecture

Current Prisma versions use a generated client and a project-level `prisma.config.ts` for datasource configuration.

Install the relevant Prisma packages for PostgreSQL according to the current Prisma documentation. A modern setup commonly includes:

```bash
npm install -D prisma
npm install @prisma/client @prisma/adapter-pg pg
```

Initialize:

```bash
npx prisma init --output ../src/generated/prisma
```

You will typically get:

```text
prisma/
└── schema.prisma

prisma.config.ts
.env
```

A current-style schema can look like:

`prisma/schema.prisma`

```prisma
generator client {
  provider = "prisma-client"
  output   = "../src/generated/prisma"
}

datasource db {
  provider = "postgresql"
}

model Task {
  id        Int      @id @default(autoincrement())
  title     String
  completed Boolean  @default(false)
  createdAt DateTime @default(now())
  updatedAt DateTime @updatedAt
}
```

`prisma.config.ts` is where current Prisma versions configure the connection URL.

Example:

```ts
import "dotenv/config";
import { defineConfig, env } from "prisma/config";

export default defineConfig({
  schema: "prisma/schema.prisma",
  migrations: {
    path: "prisma/migrations",
  },
  datasource: {
    url: env("DATABASE_URL"),
  },
});
```

`.env`

```env
DATABASE_URL="postgresql://postgres:password@localhost:5432/taskflow"
```

---

# 29. Migrations

A migration records a deliberate database schema change.

Example:

```text
Before:
tasks(id, title)

After:
tasks(id, title, completed)
```

For development:

```bash
npx prisma migrate dev --name create_tasks
```

Then generate client code if needed:

```bash
npx prisma generate
```

Important production rule:

> Do not casually use destructive schema synchronization against production.

Use controlled migrations, backups, review and rollback planning.

---

# 30. Prisma service concept

A NestJS service can expose the Prisma client to other services.

The current Prisma PostgreSQL setup uses a driver adapter. The exact generated import path depends on your configured Prisma output.

Conceptual example:

```ts
import { Injectable } from "@nestjs/common";
import { PrismaClient } from "./generated/prisma/client";
import { PrismaPg } from "@prisma/adapter-pg";

@Injectable()
export class PrismaService extends PrismaClient {
  constructor() {
    const adapter = new PrismaPg({
      connectionString: process.env.DATABASE_URL as string,
    });

    super({ adapter });
  }
}
```

Then feature services can receive `PrismaService` through dependency injection.

---

# 31. Database-backed TasksService

Conceptually:

```ts
@Injectable()
export class TasksService {
  constructor(private readonly prisma: PrismaService) {}

  findAll() {
    return this.prisma.task.findMany({
      orderBy: {
        createdAt: "desc",
      },
    });
  }

  create(dto: CreateTaskDto) {
    return this.prisma.task.create({
      data: {
        title: dto.title,
      },
    });
  }
}
```

This produces a clean separation:

```text
Controller
   ↓
Service
   ↓
Prisma
   ↓
PostgreSQL
```

---

# 32. CRUD service

A realistic shape:

```ts
@Injectable()
export class TasksService {
  constructor(private readonly prisma: PrismaService) {}

  findAll() {
    return this.prisma.task.findMany();
  }

  async findOne(id: number) {
    const task = await this.prisma.task.findUnique({
      where: { id },
    });

    if (!task) {
      throw new NotFoundException("Task not found");
    }

    return task;
  }

  create(dto: CreateTaskDto) {
    return this.prisma.task.create({
      data: dto,
    });
  }

  update(id: number, dto: UpdateTaskDto) {
    return this.prisma.task.update({
      where: { id },
      data: dto,
    });
  }

  remove(id: number) {
    return this.prisma.task.delete({
      where: { id },
    });
  }
}
```

In production, consider what happens when `update` or `delete` targets a missing row and translate database errors into correct HTTP responses.

---

# 33. Update DTO

Example:

```ts
import { IsBoolean, IsOptional, IsString, MinLength } from "class-validator";

export class UpdateTaskDto {
  @IsOptional()
  @IsString()
  @MinLength(3)
  title?: string;

  @IsOptional()
  @IsBoolean()
  completed?: boolean;
}
```

This allows partial updates.

---

# 34. Complete controller shape

```ts
import {
  Body,
  Controller,
  Delete,
  Get,
  Param,
  ParseIntPipe,
  Patch,
  Post,
} from "@nestjs/common";

@Controller("tasks")
export class TasksController {
  constructor(private readonly tasksService: TasksService) {}

  @Get()
  findAll() {
    return this.tasksService.findAll();
  }

  @Get(":id")
  findOne(@Param("id", ParseIntPipe) id: number) {
    return this.tasksService.findOne(id);
  }

  @Post()
  create(@Body() dto: CreateTaskDto) {
    return this.tasksService.create(dto);
  }

  @Patch(":id")
  update(
    @Param("id", ParseIntPipe) id: number,
    @Body() dto: UpdateTaskDto
  ) {
    return this.tasksService.update(id, dto);
  }

  @Delete(":id")
  remove(@Param("id", ParseIntPipe) id: number) {
    return this.tasksService.remove(id);
  }
}
```

---

# 35. Authentication

Authentication answers:

```text
Who is making this request?
```

A typical password login:

```text
POST /auth/login
      ↓
find user
      ↓
compare password hash
      ↓
issue secure session/token
      ↓
future protected requests identify user
```

Never store plain-text passwords.

Use a modern password hashing approach/library appropriate to your environment.

---

# 36. JWT mental model

JWT = JSON Web Token.

Very simplified flow:

```text
User logs in
   ↓
Server verifies credentials
   ↓
Server signs token
   ↓
Client sends token later
   ↓
Guard verifies token
   ↓
Request allowed
```

A JWT usually contains claims, not secret user data.

Example concept:

```json
{
  "sub": "user-123",
  "role": "USER"
}
```

Do not treat "JWT" as an entire authentication design. You still need decisions around:

- storage
- expiry
- refresh
- revocation
- cookies vs headers
- CSRF
- XSS
- key rotation

---

# 37. Authorization

Authentication:

```text
"You are user 123."
```

Authorization:

```text
"User 123 may edit task 50."
```

Never rely on the frontend to enforce authorization.

Bad security:

```text
Hide Delete button for non-admin
```

Correct security:

```text
Frontend hides button for UX
+
NestJS rejects unauthorized DELETE request
```

---

# 38. Guards for protected routes

Conceptually:

```ts
@UseGuards(AuthGuard)
@Get("profile")
getProfile() {
  return ...
}
```

For roles:

```ts
@Roles("ADMIN")
@UseGuards(AuthGuard, RolesGuard)
@Delete(":id")
remove() {
  ...
}
```

The exact authentication strategy can vary, but the backend must be the authority.

---

# 39. CORS

During development:

```text
Next.js → localhost:3000
NestJS  → localhost:3001
```

These are different origins because the ports differ.

Enable CORS in NestJS:

```ts
async function bootstrap() {
  const app = await NestFactory.create(AppModule);

  app.enableCors({
    origin: "http://localhost:3000",
    credentials: true,
  });

  await app.listen(process.env.PORT ?? 3001);
}
```

For production, explicitly configure allowed frontend origins.

Avoid:

```ts
origin: "*"
```

when your authentication/cookie/security model requires a restricted origin.

---

# 40. Connect Next.js to NestJS

Now the system looks like:

```text
Browser
   ↓
Next.js :3000
   ↓
GET http://localhost:3001/tasks
   ↓
NestJS :3001
   ↓
TasksService
   ↓
Prisma
   ↓
PostgreSQL
```

This is the main architecture to understand.

The frontend should not know how the database works.

The backend should not know how a React button looks.

---

# 41. Swagger / OpenAPI

API documentation becomes essential as the project grows.

Install the Nest OpenAPI integration according to current docs.

A Swagger UI can provide:

```text
GET /tasks
POST /tasks
PATCH /tasks/{id}
DELETE /tasks/{id}
```

plus schemas and the ability to test endpoints.

This becomes especially useful when:

- frontend and backend are developed separately
- mobile apps also use the API
- third parties integrate with the API
- you want generated client types

---

# 42. API versioning

Eventually you may need:

```text
/api/v1/tasks
/api/v2/tasks
```

Versioning helps you evolve public APIs without instantly breaking old clients.

Do not version everything prematurely, but understand the need before publishing APIs used by external clients.

---

# 43. Logging

Beginner:

```ts
console.log("Task created");
```

Production:

use structured logging and include useful context:

```text
timestamp
level
requestId
route
userId when appropriate
duration
error code
```

Never log:

```text
passwords
JWT signing secrets
database passwords
full payment credentials
sensitive personal data unnecessarily
```

Nest includes logging facilities, and external observability tools can collect production errors/metrics.

---

# 44. Testing

NestJS architecture is designed to be testable.

Think in layers.

## Unit test

Test `TasksService` without real HTTP.

Mock the database dependency.

## Controller test

Test controller behavior with mocked service.

## End-to-end test

Start the application and call actual HTTP routes.

Examples:

```text
POST /tasks with valid body → 201
POST /tasks with invalid body → 400
GET /tasks/999999 → 404
unauthenticated protected route → 401
```

---

# 45. Why dependency injection helps testing

Production:

```text
TasksService
   ↓
Real PrismaService
```

Test:

```text
TasksService
   ↓
Fake PrismaService
```

Your service logic can be tested without connecting to a real production database.

This is one of the strongest reasons to understand providers and DI properly.

---

# 46. Security checklist

At minimum:

- validate every external input
- authenticate protected endpoints
- authorize every sensitive operation
- hash passwords
- use HTTPS
- protect secrets
- restrict CORS
- rate-limit sensitive endpoints
- configure secure cookies if using cookies
- prevent SQL injection by using safe query APIs/parameterization
- validate uploads
- limit file sizes
- do not expose stack traces to users in production
- update dependencies
- log security-relevant events safely

Consider common web risks from OWASP when moving to production.

---

# 47. Database safety

Never assume the database is disposable.

For production:

```text
migrations
backups
restore testing
least-privilege DB user
indexes
constraints
transactions
monitoring
```

Important:

> A backup that has never been tested for restoration is not enough confidence.

---

# 48. Transactions

Suppose an order operation does:

```text
1. create order
2. reduce inventory
3. create payment record
```

You may need all three to succeed together.

A transaction gives the idea:

```text
all succeed
OR
all roll back
```

Use transactions when partial completion would leave inconsistent business state.

---

# 49. Indexes

If you constantly search:

```sql
WHERE email = ?
```

an appropriate index can greatly improve lookup performance.

But indexes also cost storage and slow some writes.

Do not add indexes blindly. Measure queries and understand access patterns.

---

# 50. Pagination

Never return millions of rows from:

```text
GET /tasks
```

Support pagination:

```text
GET /tasks?page=1&limit=20
```

or cursor-based pagination for large/changing datasets.

Response could include:

```json
{
  "data": [],
  "meta": {
    "page": 1,
    "limit": 20,
    "total": 137
  }
}
```

---

# 51. Search, cache, queues — when the app grows

Do not add these on day one.

Possible later architecture:

```text
NestJS
├── PostgreSQL      → source of truth
├── Redis           → cache / ephemeral data
├── Search engine   → specialized search
├── Queue           → background jobs
└── Object storage  → images/files
```

Examples:

```text
Redis        → sessions, cache, rate-limit counters
Queue        → send emails, process images
S3/R2        → uploaded files
Meilisearch  → product/search experience
```

Add infrastructure when a concrete requirement justifies it.

---

# 52. Background jobs

HTTP request:

```text
User uploads video
```

Bad design:

```text
request waits 4 minutes while everything is processed
```

Better architecture:

```text
request
  ↓
save job
  ↓
queue
  ↓
worker processes asynchronously
```

The API can return a job ID/status while background workers handle expensive tasks.

---

# 53. Application structure as it grows

A scalable feature-first structure:

```text
src/
├── main.ts
├── app.module.ts
│
├── auth/
│   ├── auth.module.ts
│   ├── auth.controller.ts
│   ├── auth.service.ts
│   ├── guards/
│   └── dto/
│
├── users/
│   ├── users.module.ts
│   ├── users.controller.ts
│   ├── users.service.ts
│   └── dto/
│
├── tasks/
│   ├── tasks.module.ts
│   ├── tasks.controller.ts
│   ├── tasks.service.ts
│   └── dto/
│
├── prisma/
│   ├── prisma.module.ts
│   └── prisma.service.ts
│
└── common/
    ├── decorators/
    ├── guards/
    ├── interceptors/
    └── filters/
```

Organize around features rather than creating giant global folders containing hundreds of unrelated controllers/services.

---

# 54. Keep layers clean

A useful rule:

### Controller should handle

```text
route
HTTP params
body
response semantics
```

### Service should handle

```text
business logic
orchestration
data operations
```

### Database layer should handle

```text
persistence
queries
transactions
```

Avoid:

```ts
@Controller("tasks")
export class TasksController {
  @Post()
  async create(@Body() body: any) {
    // 200 lines of database and business logic
  }
}
```

---

# 55. Build for production

Typical commands:

```bash
npm run build
npm run start:prod
```

The build produces compiled application output, commonly under `dist/`.

Before deployment:

```bash
npm run lint
npm test
npm run build
```

Then test important endpoints.

---

# 56. Deployment mental model

Local:

```text
NestJS → localhost:3001
PostgreSQL → localhost/cloud dev DB
```

Production:

```text
Internet
   ↓
HTTPS / load balancer
   ↓
NestJS container/service
   ↓
managed PostgreSQL
```

Possible deployment targets include:

- Railway
- Render
- Fly.io
- AWS
- Google Cloud
- Azure
- container platforms
- Kubernetes when scale/organization justifies it

The framework matters less than understanding:

```text
build
environment variables
networking
database connection
migrations
health checks
logs
scaling
```

---

# 57. Environment separation

Have distinct configuration for:

```text
development
test
staging
production
```

Do not point local development at production by accident.

Example conceptual environment values:

```text
Development DB → taskflow_dev
Test DB        → taskflow_test
Production DB  → taskflow_prod
```

Production credentials should be managed by the deployment platform's secret/config system.

---

# 58. Health checks

Your infrastructure needs a way to determine whether the service is healthy.

A simple endpoint concept:

```text
GET /health
```

Response:

```json
{
  "status": "ok"
}
```

More advanced health checks may test:

- database connectivity
- Redis
- queues
- external dependencies

Do not make every health check unnecessarily expensive.

---

# 59. Graceful shutdown

Production processes can receive shutdown signals during:

- deployment
- restart
- autoscaling
- container termination

Your application should stop accepting work and close important resources cleanly when possible.

This reduces corrupted/incomplete work during deployment.

---

# 60. CI/CD

A basic pipeline:

```text
push code
   ↓
install dependencies
   ↓
lint
   ↓
test
   ↓
build
   ↓
deploy
```

For database changes:

```text
review migration
   ↓
backup / safety plan
   ↓
apply migration
   ↓
deploy compatible code
```

Schema changes can be more dangerous than application code changes, so treat them carefully.

---

# 61. Monitoring a production API

Track:

```text
request rate
error rate
latency
CPU
memory
database connections
slow database queries
queue depth
authentication failures
```

A useful reliability question is:

> "If users report that the app is slow, can I identify which layer is slow?"

You want enough observability to answer:

```text
frontend?
network?
NestJS?
database?
external service?
```

---

# 62. Common backend failure diagnosis

## `404 Not Found`

Check:

```text
route spelling
controller prefix
HTTP method
module registration
```

## `400 Bad Request`

Check:

```text
DTO
ValidationPipe
body JSON
parameter parsing
```

## `401 Unauthorized`

Check:

```text
token/cookie missing?
expired?
signature?
auth guard?
```

## `403 Forbidden`

Usually:

```text
authenticated
but insufficient permission
```

## `500 Internal Server Error`

Inspect backend logs.

Possible causes:

```text
database unavailable
unexpected null
unhandled database error
bad environment variable
external service failure
```

---

# 63. Debugging workflow

Use this sequence:

```text
1. Reproduce.
2. Read status code.
3. Read backend stack/log.
4. Inspect request method/path/body.
5. Check DTO validation.
6. Check controller.
7. Check service.
8. Check database query.
9. Check environment/config.
10. Fix one thing and retest.
```

Do not randomly rewrite multiple layers at once.

---

# 64. Managing dependencies

Inspect:

```bash
npm outdated
```

Update deliberately.

For major upgrades:

```text
read Nest migration guide
read Prisma migration guide
create branch
upgrade
run lint
run unit tests
run integration tests
run build
test staging
deploy
monitor
```

Do not treat `npm update` as a complete upgrade strategy.

---

# 65. API contract management

Your frontend depends on the backend's contract.

Example:

```json
{
  "id": 1,
  "title": "Learn NestJS"
}
```

If backend suddenly changes to:

```json
{
  "taskId": 1,
  "taskName": "Learn NestJS"
}
```

the frontend can break.

Use:

- DTOs
- OpenAPI
- generated clients/types where useful
- versioning for external APIs
- backward-compatible changes when possible

Treat API schemas as contracts.

---

# 66. Error response consistency

Instead of every endpoint inventing random error shapes:

```json
{
  "error": "bad"
}
```

and:

```json
{
  "oops": true
}
```

define a consistent approach, for example:

```json
{
  "statusCode": 400,
  "message": "Title must contain at least 3 characters",
  "error": "Bad Request"
}
```

Consistency makes frontend handling and debugging easier.

---

# 67. What belongs in NestJS vs Next.js?

## NestJS

Best place for:

```text
business rules
database
authorization
REST API
background jobs
integrations
webhooks
server-side validation
```

## Next.js

Best place for:

```text
web UI
layouts
pages
browser interaction
frontend composition
server-rendered web experience
```

Do not put a critical permission rule only in Next.js.

The backend is the enforcement boundary.

---

# 68. Our complete TaskFlow architecture

```text
┌─────────────────────────────┐
│          Browser            │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│        Next.js Web          │
│ localhost:3000              │
└──────────────┬──────────────┘
               │ HTTP / JSON
               ▼
┌─────────────────────────────┐
│        NestJS API           │
│ localhost:3001              │
│                             │
│ TasksController             │
│       ↓                     │
│ TasksService                │
│       ↓                     │
│ PrismaService               │
└──────────────┬──────────────┘
               │ SQL protocol
               ▼
┌─────────────────────────────┐
│         PostgreSQL          │
└─────────────────────────────┘
```

Request example:

```text
Click "Create Task"
   ↓
Next.js POST /tasks
   ↓
Nest controller
   ↓
DTO validation
   ↓
TasksService
   ↓
Prisma
   ↓
PostgreSQL INSERT
   ↓
JSON response
   ↓
Next.js updates UI
```

If you understand that flow deeply, you understand the backbone of a large class of modern web applications.

---

# 69. Practical learning sequence

## Stage 1 — HTTP without database

Build:

```text
GET /tasks
GET /tasks/:id
POST /tasks
```

Use an in-memory array.

## Stage 2 — Architecture

Refactor into:

```text
TasksModule
TasksController
TasksService
DTOs
```

## Stage 3 — Validation

Add:

```text
ValidationPipe
class-validator
ParseIntPipe
```

## Stage 4 — Database

Add:

```text
PostgreSQL
Prisma
migrations
```

## Stage 5 — Authentication

Add:

```text
register
login
protected routes
roles
ownership checks
```

## Stage 6 — Production

Add:

```text
Swagger
tests
logging
health check
rate limiting
deployment
monitoring
CI/CD
```

---

# 70. Seven-day beginner practice plan

### Day 1

Learn:

```text
Node.js
HTTP
Nest project structure
controllers
```

Build 3 GET endpoints.

### Day 2

Learn:

```text
services
modules
dependency injection
```

Move logic from controller to service.

### Day 3

Learn:

```text
POST
PATCH
DELETE
DTOs
ValidationPipe
```

Build in-memory CRUD.

### Day 4

Install PostgreSQL and connect Prisma.

Persist tasks.

### Day 5

Add:

```text
users
registration
login concepts
authorization
```

### Day 6

Connect the Next.js frontend.

Test CORS and CRUD end to end.

### Day 7

Add:

```text
Swagger
tests
logging
production build
deployment
```

---

# 71. Beginner mistakes to avoid

### Mistake 1

Putting everything in controllers.

**Better:** controllers handle transport; services handle logic.

### Mistake 2

Using `any` everywhere.

**Better:** define DTOs and types.

### Mistake 3

No validation.

**Better:** validate every request boundary.

### Mistake 4

Trusting the frontend for permissions.

**Better:** authorize in NestJS.

### Mistake 5

Hard-coding secrets.

**Better:** environment configuration.

### Mistake 6

Using production DB for development.

**Better:** separate environments.

### Mistake 7

Changing database schema manually with no migration history.

**Better:** controlled migrations.

### Mistake 8

Catching errors and returning fake success.

**Better:** correct HTTP status and meaningful logs.

### Mistake 9

Adding Redis/queues/microservices too early.

**Better:** earn complexity through real requirements.

---

# 72. NestJS cheat sheet

```text
@Controller()       → handles route group
@Get()              → GET endpoint
@Post()             → POST endpoint
@Patch()            → PATCH endpoint
@Delete()           → DELETE endpoint

@Body()             → request JSON body
@Param()            → route parameter
@Query()            → query string

@Injectable()        → injectable provider/service
@Module()            → groups application pieces

ValidationPipe      → validates/transforms input
Guard               → allow/deny request
Interceptor         → wrap request execution
Exception Filter    → customize exception handling
Middleware          → early request processing

nest g module x
nest g controller x
nest g service x
nest g resource x

npm run start:dev    → development
npm run build        → compile
npm run start:prod   → production
npm test             → tests
```

---

# 73. What "I know NestJS" should eventually mean

You should be able to explain:

1. What the backend does.
2. Controller vs service vs module.
3. What dependency injection solves.
4. How decorators configure Nest.
5. How HTTP requests become controller calls.
6. Why DTO validation is necessary.
7. Pipes vs guards vs interceptors vs filters.
8. How Nest communicates with PostgreSQL.
9. What migrations are.
10. Authentication vs authorization.
11. How to secure an API.
12. How Next.js communicates with NestJS.
13. How to test services and endpoints.
14. How to deploy and monitor the API.
15. How to diagnose a 400, 401, 403, 404 and 500.

If you can build, explain, test and deploy a secure CRUD API backed by PostgreSQL, you are beyond the beginner stage.

---

# 74. Recommended real-project stack

A practical full-stack setup:

```text
Frontend
  Next.js
  TypeScript
  Tailwind CSS

Backend
  NestJS
  TypeScript
  REST

Database
  PostgreSQL
  Prisma

Authentication
  JWT/session/provider depending on product requirements

Files
  S3-compatible object storage

Cache
  Redis when justified

Search
  PostgreSQL search first or dedicated search when justified

Deployment
  Next.js hosting + Node/container backend hosting

Monitoring
  platform logs + error/observability tooling
```

Start with the smallest version:

```text
Next.js + NestJS + PostgreSQL
```

Then add infrastructure when needed.

---

# 75. Official references

- NestJS documentation: https://docs.nestjs.com/
- NestJS first steps: https://docs.nestjs.com/first-steps
- NestJS validation: https://docs.nestjs.com/techniques/validation
- NestJS configuration: https://docs.nestjs.com/techniques/configuration
- NestJS database techniques: https://docs.nestjs.com/techniques/database
- NestJS Prisma recipe: https://docs.nestjs.com/recipes/prisma
- Prisma NestJS guide: https://www.prisma.io/docs/guides/frameworks/nestjs
- PostgreSQL: https://www.postgresql.org/docs/

Because Prisma's setup evolved substantially in recent major versions, use the current Prisma documentation when copying exact installation/generation commands.

---

# ✅ Final checkpoint

You are ready to build a real NestJS backend when you can independently follow this flow:

```text
Create project
   ↓
Create feature module
   ↓
Create controller
   ↓
Create service
   ↓
Create DTO
   ↓
Enable validation
   ↓
Connect PostgreSQL
   ↓
Create migration
   ↓
Implement CRUD
   ↓
Add authentication
   ↓
Add authorization
   ↓
Test
   ↓
Build
   ↓
Deploy
   ↓
Monitor and maintain
```

Then connect it to the companion Next.js application:

```text
Next.js → NestJS → PostgreSQL
```

That is the core full-stack architecture this pair of guides is designed to teach.
