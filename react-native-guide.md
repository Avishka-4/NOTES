# React Native: Basic to Advanced — A Practical App Development Guide

*Current as of 2026 — built around React Native 0.84/0.85, the New Architecture (default), React 19.2, Hermes V1, and Expo SDK 55.*

This guide walks from "I've never opened a mobile project" to "I can ship and maintain a production app." Each section builds on the last. You don't need prior mobile experience, but you should be comfortable with JavaScript (ES2022+: `async/await`, destructuring, optional chaining) and basic React (components, props, the `useState` hook).

---

## Part 1 — The Mental Model

### What React Native actually is

React Native lets you write apps in JavaScript/TypeScript and React, and renders them using *real native UI components* — not a webview. Your `<View>` becomes a real Android `ViewGroup` or iOS `UIView`. This is why RN apps feel native rather than like wrapped websites.

Your JavaScript runs in a separate thread from the UI. Historically these communicated over an asynchronous "bridge," which was a bottleneck. The **New Architecture** (now the default) replaces this with JSI (a direct C++ interface), Fabric (the new rendering system), and TurboModules (lazy-loaded native modules). The practical result is faster startup, smoother rendering, and lower memory use. You rarely touch these internals directly, but knowing they exist explains why some old libraries break and why "New Architecture support" matters when choosing dependencies.

### React Native vs. Expo

React Native is the framework. **Expo** is a toolchain and set of libraries built on top of it that handles the painful parts: project scaffolding, native build configuration, device APIs, cloud builds, and over-the-air updates. For nearly all new projects in 2026, start with Expo. You can always "eject" to raw native code later if you genuinely need to, but most apps never do.

---

## Part 2 — Setup and Your First App

### Prerequisites

- **Node.js** — current LTS. Check the Expo docs for the exact minimum, since it bumps regularly.
- A code editor (VS Code is the common choice).
- The **Expo Go** app on your physical phone (iOS or Android) — the fastest way to see your app while learning.
- Optional for later: Xcode (Mac only, for iOS) and Android Studio (for the Android emulator).

### Create and run a project

```bash
npx create-expo-app@latest MyApp
cd MyApp
npx expo start
```

This scaffolds a project using **Expo Router** (file-based routing, the default) and boots the Metro bundler, printing a QR code. Scan it with Expo Go and your app loads in seconds. Every save hot-reloads the app. This skips the Xcode/Android Studio configuration that used to make day one take hours.

### Project anatomy

```
MyApp/
  app/              # Screens — file-based routing lives here
    _layout.tsx     # Root layout (wraps all screens)
    index.tsx       # The "/" route
  assets/           # Images, fonts
  components/        # Your reusable components
  app.json          # App config (name, icon, plugins)
  package.json
  tsconfig.json
```

Use TypeScript from the start. It's the professional standard, catches whole categories of bugs before runtime, and the templates set it up for you.

---

## Part 3 — Core Building Blocks

### Components and JSX

A component is a function that returns UI. React Native gives you primitive components instead of HTML tags:

| Web | React Native | Purpose |
|-----|-------------|---------|
| `<div>` | `<View>` | Container / layout box |
| `<p>`, `<span>` | `<Text>` | All text (text *must* be inside `<Text>`) |
| `<img>` | `<Image>` | Images |
| `<input>` | `<TextInput>` | Text entry |
| `<button>` | `<Pressable>` | Touchable elements |

```tsx
import { View, Text } from "react-native";

export default function Greeting() {
  return (
    <View>
      <Text>Hello, world</Text>
    </View>
  );
}
```

A key gotcha: every string must be wrapped in `<Text>`. Putting raw text directly inside a `<View>` throws an error.

### Props — passing data in

```tsx
function Avatar({ name, size }: { name: string; size: number }) {
  return <Text style={{ fontSize: size }}>{name}</Text>;
}

// Used as:
<Avatar name="Ada" size={24} />
```

Props flow one direction: parent to child. A child never mutates its props.

### State — data that changes over time

```tsx
import { useState } from "react";
import { View, Text, Pressable } from "react-native";

export default function Counter() {
  const [count, setCount] = useState(0);

  return (
    <View>
      <Text>Count: {count}</Text>
      <Pressable onPress={() => setCount(count + 1)}>
        <Text>Increment</Text>
      </Pressable>
    </View>
  );
}
```

When state changes, the component re-renders. This is the heart of React. The rule: never mutate state directly (`count++`), always use the setter.

### Other essential hooks

- **`useEffect`** — run side effects (data fetching, subscriptions) after render. Mind the dependency array; getting it wrong causes infinite loops or stale data.
- **`useRef`** — hold a mutable value that doesn't trigger re-renders (e.g., a reference to a `TextInput` to focus it).
- **`useMemo` / `useCallback`** — memoize expensive computations or stable function references. Reach for these only when you have a measured performance reason, not by default.

---

## Part 4 — Styling and Layout

### StyleSheet

React Native uses JavaScript objects for styling, not CSS files. Properties are camelCased (`backgroundColor`, not `background-color`).

```tsx
import { StyleSheet, View, Text } from "react-native";

export default function Card() {
  return (
    <View style={styles.card}>
      <Text style={styles.title}>Title</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    padding: 16,
    borderRadius: 12,
    backgroundColor: "#fff",
  },
  title: {
    fontSize: 18,
    fontWeight: "600",
  },
});
```

### Flexbox is the layout engine

Everything lays out with Flexbox, with two differences from the web:
- `flexDirection` defaults to **`column`** (web defaults to row).
- There are no units like `px` — numbers are density-independent pixels.

The core four properties carry most layouts:
- `flexDirection`: `column` | `row`
- `justifyContent`: alignment along the main axis
- `alignItems`: alignment along the cross axis
- `flex`: how much space a child takes relative to siblings

```tsx
<View style={{ flex: 1, flexDirection: "row", justifyContent: "space-between", alignItems: "center" }}>
  <Text>Left</Text>
  <Text>Right</Text>
</View>
```

### Handling different screen sizes and notches

Wrap screens in `SafeAreaView` (or use the `react-native-safe-area-context` library) so content doesn't sit under notches or the status bar. Use `Dimensions` or the `useWindowDimensions` hook for responsive sizing, and percentages or `flex` rather than hardcoded widths.

For larger apps, many teams adopt a styling library like **NativeWind** (Tailwind syntax for RN) or a component system to stay consistent — optional, but worth knowing it exists.

---

## Part 5 — Lists and Rendering Data

Never render long lists with `.map()` inside a `ScrollView` — it renders everything at once and destroys performance. Use a virtualized list that only renders what's visible.

```tsx
import { FlatList, Text, View } from "react-native";

const data = [{ id: "1", name: "Apple" }, { id: "2", name: "Banana" }];

export default function Fruits() {
  return (
    <FlatList
      data={data}
      keyExtractor={(item) => item.id}
      renderItem={({ item }) => (
        <View><Text>{item.name}</Text></View>
      )}
    />
  );
}
```

For very large or complex lists, the community library **FlashList** (from Shopify) is significantly faster than `FlatList`. Always provide a stable, unique `keyExtractor`.

---

## Part 6 — Navigation

Modern Expo projects use **Expo Router**, where the file system *is* the navigation structure. Each file in `app/` becomes a screen; folders become navigation groups.

```
app/
  _layout.tsx        # defines the navigator (stack, tabs)
  index.tsx          # "/"
  profile.tsx        # "/profile"
  posts/
    [id].tsx         # dynamic route, e.g. "/posts/42"
```

Navigate declaratively with `<Link>` or imperatively with the router:

```tsx
import { Link, useRouter } from "expo-router";

// Declarative
<Link href="/profile">Go to profile</Link>

// Imperative
const router = useRouter();
router.push("/posts/42");
```

Common patterns you'll set up in `_layout.tsx`:
- **Stack** — push/pop screens with a back gesture (detail views).
- **Tabs** — bottom tab bar (the main sections of an app).
- **Modal** — screens that slide up over the current one.

Under the hood Expo Router builds on React Navigation, so deeper concepts (params, headers, gestures) transfer.

---

## Part 7 — Forms and User Input

```tsx
import { useState } from "react";
import { TextInput, Pressable, Text, View } from "react-native";

export default function LoginForm() {
  const [email, setEmail] = useState("");

  return (
    <View>
      <TextInput
        value={email}
        onChangeText={setEmail}
        placeholder="Email"
        keyboardType="email-address"
        autoCapitalize="none"
      />
      <Pressable onPress={() => console.log(email)}>
        <Text>Submit</Text>
      </Pressable>
    </View>
  );
}
```

For anything beyond a couple of fields, use **React Hook Form** with a schema validator like **Zod**. It handles validation, errors, and submission cleanly and avoids re-rendering on every keystroke. Also handle the keyboard: wrap inputs in `KeyboardAvoidingView` so it doesn't cover the field being typed in.

---

## Part 8 — Networking and Data

Basic fetching uses the standard `fetch` API inside `useEffect`:

```tsx
useEffect(() => {
  let active = true;
  fetch("https://api.example.com/items")
    .then((res) => res.json())
    .then((data) => { if (active) setItems(data); })
    .catch(console.error);
  return () => { active = false; };
}, []);
```

That manual pattern gets tedious fast (loading states, errors, caching, refetching). The professional approach is **TanStack Query** (React Query), which handles caching, background refetching, retries, and loading/error states for you:

```tsx
import { useQuery } from "@tanstack/react-query";

const { data, isLoading, error } = useQuery({
  queryKey: ["items"],
  queryFn: () => fetch("/api/items").then((r) => r.json()),
});
```

For the backend itself, **Supabase** pairs well with Expo and gives you a Postgres database, authentication, file storage, and a JS client without running your own server. Firebase is another common choice.

---

## Part 9 — State Management

Don't reach for a global state library on day one. Most state should be local (`useState`) or server cache (React Query). Escalate only when you have state that's genuinely shared across distant parts of the tree.

The progression:

1. **`useState`** — local component state. Start here.
2. **`useReducer`** — when one component's state has complex transitions (e.g., a multi-step form). It centralizes update logic into a reducer function.
3. **React Context** — share values (theme, current user, auth) without prop-drilling. Good for low-frequency updates; can cause excess re-renders if overused for rapidly changing data.
4. **A dedicated library** — for large apps with lots of shared, frequently changing state:
   - **Zustand** — minimal, hooks-based, the common modern default.
   - **Redux Toolkit** — heavier, more structure, strong for very large teams/apps.

Rule of thumb: server data → React Query; client UI state → Zustand or Context; everything else → local state.

---

## Part 10 — Storage and Persistence

- **AsyncStorage** — simple key-value storage for small, non-sensitive data (settings, flags). It's async and unencrypted.
- **expo-secure-store** — encrypted storage for sensitive data like auth tokens. Use this, never AsyncStorage, for secrets.
- **MMKV** — a much faster synchronous key-value store; popular when AsyncStorage performance matters.
- **SQLite (expo-sqlite)** or **WatermelonDB** — for structured relational data and offline-first apps with lots of records.

A common beginner mistake is storing JWTs or passwords in AsyncStorage. Use SecureStore for anything sensitive.

---

## Part 11 — Native Device Features

Expo ships modules for most device capabilities — install only what you need with `npx expo install`:

- `expo-camera` — camera access
- `expo-image-picker` — pick photos from the library
- `expo-location` — GPS and geolocation
- `expo-notifications` — push and local notifications
- `expo-haptics` — vibration feedback
- `expo-av` / `expo-video` — audio and video
- `expo-file-system` — read/write files

Each requires declaring **permissions** (in `app.json` and at runtime) and handling the case where the user denies them. Always design for the denial path, not just the happy path.

---

## Part 12 — Performance Optimization

Performance problems usually come from a handful of sources. In rough priority order:

1. **List rendering** — use `FlatList`/`FlashList`, stable keys, and `getItemLayout` where possible. This is the #1 source of jank.
2. **Unnecessary re-renders** — wrap pure components in `React.memo`, stabilize callbacks with `useCallback`, and memoize expensive values with `useMemo`. Profile before optimizing; premature memoization adds complexity for nothing.
3. **Heavy work on the JS thread** — move expensive computations off the render path. Animations should run on the UI thread.
4. **Animations** — use **Reanimated** (and **Gesture Handler**), which run animations on the native/UI thread so they stay at 60fps even if JS is busy. Avoid animating with `setState` in a loop.
5. **Images** — use `expo-image` for caching and efficient loading; size images appropriately rather than shipping huge files.
6. **Bundle size and startup** — Hermes (the default JS engine) precompiles to bytecode for faster startup; keep dependencies lean.

Use the built-in performance monitor and tools like Flipper or the React DevTools profiler to find the actual bottleneck rather than guessing.

---

## Part 13 — Going Native (Advanced)

Most apps never need this, but when an existing library doesn't cover a native capability, you can write your own native module.

- **Expo Modules API** — the modern, recommended way to write native modules in Swift/Kotlin. Modules written with this API support the New Architecture by default with no extra work.
- **Config plugins** — modify native project configuration (entitlements, `Info.plist`, Gradle) declaratively from JS, so you keep the benefits of managed builds.
- **The bare workflow / prebuild** — when you need full control over the `ios/` and `android/` directories. `npx expo prebuild` generates the native projects from your config.

When evaluating any third-party native library, check its New Architecture support first — run `npx expo-doctor`, which validates your dependencies against the React Native Directory and flags unmaintained or incompatible packages.

---

## Part 14 — Testing

A practical testing pyramid for RN:

- **Unit tests** — **Jest** for pure functions and logic. Fast, run constantly.
- **Component tests** — **React Native Testing Library** to render components and assert on what the user sees and does, rather than implementation details.
- **End-to-end tests** — **Maestro** (simple, modern, increasingly the default) or **Detox** drives the actual app on a device/emulator through real user flows.

```tsx
import { render, screen, fireEvent } from "@testing-library/react-native";

test("increments", () => {
  render(<Counter />);
  fireEvent.press(screen.getByText("Increment"));
  expect(screen.getByText("Count: 1")).toBeTruthy();
});
```

Start with unit tests for business logic and component tests for critical UI; add E2E for your most important flows (login, checkout).

---

## Part 15 — Building and Shipping with EAS

**EAS (Expo Application Services)** is the production deployment platform. It removes the need for local Xcode/Android Studio setup.

- **EAS Build** — compiles your iOS and Android binaries in the cloud.
- **EAS Submit** — one command to upload to the App Store and Play Store.
- **EAS Update** — over-the-air JavaScript and asset updates, with channels and rollbacks, so you can ship JS fixes instantly without an App Store review.

Typical flow:

```bash
npm install -g eas-cli
eas login
eas build:configure          # creates eas.json with build profiles
eas build --profile production --platform all
eas submit --profile production --platform all
```

`eas.json` defines build profiles — commonly `development`, `preview`, and `production` — each with its own environment variables, distribution settings, and credentials.

Key concept: **bump the runtime version when you change native code** (a new native module, an Expo SDK upgrade, edits to `android/` or `ios/`). Pure JavaScript changes ship via `eas update` and do *not* need a runtime bump or store review.

You can wire all of this into CI (e.g., GitHub Actions) by installing `eas-cli`, authenticating with an `EXPO_TOKEN`, and running build/update commands on push to your release branches.

---

## Part 16 — Project Structure and Best Practices

A maintainable layout that scales:

```
app/                 # screens / routes (Expo Router)
components/           # reusable UI components
features/             # feature-based folders (auth, feed, profile)
services/             # API clients, storage wrappers
hooks/                # custom hooks
lib/ or utils/        # pure helpers
constants/            # colors, config
types/                # shared TypeScript types
```

Habits that pay off:
- **TypeScript everywhere**, strict mode on.
- A **service layer** so screens don't call `fetch` directly — they call typed functions, which makes testing and refactoring far easier.
- **Environment variables** for API URLs and keys (never hardcode secrets; use EAS secrets for build-time values).
- **A consistent component pattern** — small, focused, single-responsibility components.
- **Lint and format** with ESLint + Prettier, enforced in CI.

---

## Part 17 — Common Pitfalls

- Putting raw text outside a `<Text>` component.
- Using `ScrollView` + `.map()` for long lists instead of `FlatList`/`FlashList`.
- Storing secrets in AsyncStorage instead of SecureStore.
- Forgetting the `useEffect` dependency array (stale data or infinite loops).
- Hardcoding dimensions instead of using Flexbox/percentages, breaking on other screen sizes.
- Ignoring the permission-denied path for camera/location/notifications.
- Installing native libraries without checking New Architecture support.
- Reaching for Redux/global state before you actually need it.
- Animating with `setState` instead of Reanimated, causing dropped frames.
- Not handling loading and error states in data fetching.

---

## Part 18 — A Suggested Learning Path

1. **Week 1** — Build a static screen: components, props, `useState`, StyleSheet, Flexbox.
2. **Week 2** — Add a `FlatList` of data, multiple screens with Expo Router, and a form.
3. **Week 3** — Fetch from a real API with React Query; add loading/error states; persist a setting with AsyncStorage.
4. **Week 4** — Add auth (Supabase), SecureStore for tokens, and one device feature (camera or notifications).
5. **Week 5** — Add Reanimated animations, write tests, profile and fix a performance issue.
6. **Week 6** — Configure EAS, build a real binary, and ship a TestFlight / internal track release.

Build one complete app end-to-end rather than collecting tutorials. A task manager, a small social feed, or a habit tracker each touches every concept above.

### Where to keep learning

- **Official React Native docs** — reactnative.dev
- **Expo docs** — docs.expo.dev (the most relevant for the modern stack)
- **React Native Directory** — reactnative.directory (check library compatibility)
- **TanStack Query, Reanimated, React Hook Form, Zustand** — each has excellent docs worth reading directly.

---

*The single biggest accelerator is shipping. A small, finished, deployed app teaches more than a dozen half-built tutorials.*
