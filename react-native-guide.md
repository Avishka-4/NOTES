# React Native: Basic to Advanced — A Practical App Development Guide

*Current as of 2026 — built around React Native 0.84/0.85, the New Architecture (default), React 19.2, Hermes V1, and Expo SDK 55.*

This guide walks from "I've never opened a mobile project" to "I can ship and maintain a production app." Each section builds on the last. You don't need prior mobile experience, but you should be comfortable with JavaScript (ES2022+: `async/await`, destructuring, optional chaining) and basic React (components, props, the `useState` hook).

---

## Part 1 — The Mental Model

### What React Native actually is

React Native lets you write apps in JavaScript/TypeScript and React, and renders them using *real native UI components* — not a webview. Your `<View>` becomes a real Android `ViewGroup` or iOS `UIView`. This is why RN apps feel native rather than like wrapped websites.

Your JavaScript runs in a separate thread from the UI. Historically these communicated over an asynchronous "bridge," which was a bottleneck. The **New Architecture** (now the default) replaces this with JSI (a direct C++ interface), Fabric (the new rendering system), and TurboModules (lazy-loaded native modules). The practical result is faster startup, smoother rendering, and lower memory use. You rarely touch these internals directly, but knowing they exist explains why some old libraries break and why "New Architecture support" matters when choosing dependencies.

**How the threads work in practice:**

```
┌─────────────────────────────┐     JSI (C++ direct call)    ┌──────────────────────────┐
│   JavaScript Thread         │ ◄──────────────────────────► │   UI / Native Thread     │
│  - Your React code          │                               │  - Renders real Views    │
│  - Business logic           │                               │  - Handles gestures      │
│  - Data fetching            │                               │  - Animations (Reanimated│
└─────────────────────────────┘                               └──────────────────────────┘
```

The old Bridge serialized everything to JSON and passed it asynchronously. JSI lets JS call native functions directly and synchronously — that's why animations and gestures are so much smoother in the New Architecture.

### React Native vs. Expo

React Native is the framework. **Expo** is a toolchain and set of libraries built on top of it that handles the painful parts: project scaffolding, native build configuration, device APIs, cloud builds, and over-the-air updates. For nearly all new projects in 2026, start with Expo. You can always "eject" to raw native code later if you genuinely need to, but most apps never do.

**Comparison at a glance:**

| | Bare React Native | Expo Managed | Expo with Prebuild |
|---|---|---|---|
| Setup time | Hours (Xcode + Android Studio) | Minutes | Minutes |
| Native code access | Full | Via Expo modules | Full (after prebuild) |
| OTA updates | Manual setup | Built-in (EAS Update) | Built-in |
| Best for | Libraries, deep native work | Most apps | Apps needing custom native |

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

**What happens when you run `npx expo start`:**

```
Metro bundler starts
  └─► Watches your files for changes
  └─► Bundles JS and assets on demand
  └─► Prints a QR code

You scan with Expo Go
  └─► App downloads the bundle from Metro over Wi-Fi
  └─► Renders on your real device
  └─► Hot reloads on every save (< 1 second)
```

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

**Your first screen (`app/index.tsx`) might look like this:**

```tsx
import { View, Text, StyleSheet } from "react-native";

export default function HomeScreen() {
  return (
    <View style={styles.container}>
      <Text style={styles.heading}>Welcome to MyApp 👋</Text>
      <Text style={styles.subtitle}>Edit app/index.tsx to get started</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center",
    backgroundColor: "#f5f5f5",
  },
  heading: {
    fontSize: 24,
    fontWeight: "bold",
    marginBottom: 8,
  },
  subtitle: {
    fontSize: 16,
    color: "#666",
  },
});
```

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
| `<ul>` / `<li>` | `<FlatList>` | Performant scrollable lists |
| `<div style="overflow:scroll">` | `<ScrollView>` | Small scrollable containers |

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

```tsx
// ❌ WRONG — throws "Text strings must be rendered within a <Text> component"
<View>
  Hello world
</View>

// ✅ CORRECT
<View>
  <Text>Hello world</Text>
</View>
```

### Props — passing data in

Props are how you pass data from a parent component down to a child. The child declares what it expects (its "interface"), and the parent provides those values.

```tsx
// Define the component with typed props
function Avatar({ name, size, avatarUrl }: { name: string; size: number; avatarUrl?: string }) {
  return (
    <View style={{ alignItems: "center" }}>
      {avatarUrl && (
        <Image source={{ uri: avatarUrl }} style={{ width: size, height: size, borderRadius: size / 2 }} />
      )}
      <Text style={{ fontSize: size * 0.4 }}>{name}</Text>
    </View>
  );
}

// Parent uses the component and provides props
export default function ProfileScreen() {
  return (
    <View>
      <Avatar name="Ada Lovelace" size={80} avatarUrl="https://example.com/ada.jpg" />
      <Avatar name="Alan Turing" size={60} /> {/* avatarUrl is optional */}
    </View>
  );
}
```

Props flow one direction: parent to child. A child never mutates its props.

### State — data that changes over time

State is local, private data owned by a component. When it changes, React re-renders the component with the new values.

```tsx
import { useState } from "react";
import { View, Text, Pressable, StyleSheet } from "react-native";

export default function Counter() {
  // useState returns [currentValue, setterFunction]
  // 0 is the initial value
  const [count, setCount] = useState(0);

  return (
    <View style={styles.container}>
      <Text style={styles.count}>Count: {count}</Text>

      {/* Increment */}
      <Pressable
        style={styles.button}
        onPress={() => setCount(count + 1)}
      >
        <Text style={styles.buttonText}>+ Increment</Text>
      </Pressable>

      {/* Decrement */}
      <Pressable
        style={[styles.button, styles.buttonSecondary]}
        onPress={() => setCount(count - 1)}
      >
        <Text style={styles.buttonText}>- Decrement</Text>
      </Pressable>

      {/* Reset */}
      <Pressable
        style={[styles.button, styles.buttonDanger]}
        onPress={() => setCount(0)}
      >
        <Text style={styles.buttonText}>Reset</Text>
      </Pressable>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, alignItems: "center", justifyContent: "center", gap: 12 },
  count: { fontSize: 32, fontWeight: "bold", marginBottom: 16 },
  button: { backgroundColor: "#007AFF", paddingHorizontal: 24, paddingVertical: 12, borderRadius: 8 },
  buttonSecondary: { backgroundColor: "#5856D6" },
  buttonDanger: { backgroundColor: "#FF3B30" },
  buttonText: { color: "#fff", fontWeight: "600", fontSize: 16 },
});
```

When state changes, the component re-renders. This is the heart of React. The rule: never mutate state directly (`count++`), always use the setter.

```tsx
// ❌ WRONG — React won't detect this change and won't re-render
count++;

// ✅ CORRECT — triggers a re-render
setCount(count + 1);

// ✅ ALSO CORRECT — use the functional form when new state depends on old
setCount(prev => prev + 1);
```

### Other essential hooks

**`useEffect` — run code after render**

```tsx
import { useEffect, useState } from "react";

export default function UserProfile({ userId }: { userId: string }) {
  const [user, setUser] = useState(null);

  useEffect(() => {
    // This runs after every render where userId changes
    let cancelled = false; // prevents setting state after unmount

    async function loadUser() {
      const response = await fetch(`https://api.example.com/users/${userId}`);
      const data = await response.json();
      if (!cancelled) setUser(data);
    }

    loadUser();

    // Cleanup function — called before the next effect or on unmount
    return () => { cancelled = true; };
  }, [userId]); // ← dependency array: re-run only when userId changes

  return <Text>{user?.name ?? "Loading..."}</Text>;
}
```

**`useRef` — stable reference that doesn't cause re-renders**

```tsx
import { useRef } from "react";
import { TextInput, Pressable, Text, View } from "react-native";

export default function SearchBar() {
  // inputRef.current will point to the actual TextInput element
  const inputRef = useRef<TextInput>(null);

  return (
    <View>
      <TextInput ref={inputRef} placeholder="Search..." />
      <Pressable onPress={() => inputRef.current?.focus()}>
        <Text>Focus input</Text>
      </Pressable>
    </View>
  );
}
```

**`useMemo` / `useCallback` — memoization**

```tsx
import { useMemo, useCallback } from "react";

// useMemo: memoize an expensive computed value
const sortedItems = useMemo(
  () => items.slice().sort((a, b) => a.name.localeCompare(b.name)),
  [items] // only re-sort when items changes
);

// useCallback: memoize a function reference (useful when passing to child components)
const handlePress = useCallback((id: string) => {
  navigation.navigate("Detail", { id });
}, [navigation]);
```

Reach for these only when you have a measured performance reason, not by default. Premature memoization adds complexity.

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
      <Text style={styles.body}>Card body text goes here.</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    padding: 16,
    borderRadius: 12,
    backgroundColor: "#fff",
    // Shadow on iOS
    shadowColor: "#000",
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    // Shadow on Android
    elevation: 3,
    marginHorizontal: 16,
    marginVertical: 8,
  },
  title: {
    fontSize: 18,
    fontWeight: "600",
    marginBottom: 4,
    color: "#111",
  },
  body: {
    fontSize: 14,
    color: "#555",
    lineHeight: 20,
  },
});
```

**Why `StyleSheet.create` instead of plain objects?**

`StyleSheet.create` validates your styles and sends them to native once (as IDs), rather than re-serializing the object on every render. It also gives you autocomplete in TypeScript.

**Combining styles — use an array:**

```tsx
// Apply multiple styles; the last one wins on conflicts
<Text style={[styles.base, styles.bold, isError && styles.error]}>
  Hello
</Text>
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
// Row layout — two items side by side
<View style={{ flex: 1, flexDirection: "row", justifyContent: "space-between", alignItems: "center" }}>
  <Text>Left</Text>
  <Text>Right</Text>
</View>

// Column layout with even spacing
<View style={{ flex: 1, flexDirection: "column", justifyContent: "space-evenly", padding: 16 }}>
  <Text>Top</Text>
  <Text>Middle</Text>
  <Text>Bottom</Text>
</View>

// flex: 1 means "take all remaining space"
// flex: 2 means "take twice as much as flex: 1 siblings"
<View style={{ flexDirection: "row" }}>
  <View style={{ flex: 1, backgroundColor: "red" }} />   {/* 1/3 of width */}
  <View style={{ flex: 2, backgroundColor: "blue" }} />  {/* 2/3 of width */}
</View>
```

**A real-world header layout:**

```tsx
<View style={{
  flexDirection: "row",
  alignItems: "center",
  justifyContent: "space-between",
  paddingHorizontal: 16,
  paddingVertical: 12,
  backgroundColor: "#fff",
  borderBottomWidth: 1,
  borderBottomColor: "#eee",
}}>
  <Pressable onPress={() => router.back()}>
    <Text style={{ fontSize: 16, color: "#007AFF" }}>← Back</Text>
  </Pressable>
  <Text style={{ fontSize: 18, fontWeight: "600" }}>Screen Title</Text>
  <Pressable onPress={handleSettings}>
    <Text style={{ fontSize: 16, color: "#007AFF" }}>Settings</Text>
  </Pressable>
</View>
```

### Handling different screen sizes and notches

```tsx
import { SafeAreaView } from "react-native-safe-area-context";
import { useWindowDimensions } from "react-native";

export default function ResponsiveScreen() {
  const { width, height } = useWindowDimensions();
  const isLandscape = width > height;

  return (
    // SafeAreaView keeps content away from notches and the home indicator
    <SafeAreaView style={{ flex: 1 }}>
      <View style={{
        flexDirection: isLandscape ? "row" : "column",
        flex: 1,
        padding: 16,
      }}>
        <View style={{ flex: 1, backgroundColor: "#f0f0f0" }}>
          <Text>Panel A</Text>
        </View>
        <View style={{ flex: isLandscape ? 2 : 1, backgroundColor: "#e0e0e0" }}>
          <Text>Panel B</Text>
        </View>
      </View>
    </SafeAreaView>
  );
}
```

For larger apps, many teams adopt a styling library like **NativeWind** (Tailwind syntax for RN) or a component system to stay consistent — optional, but worth knowing it exists.

---

## Part 5 — Lists and Rendering Data

Never render long lists with `.map()` inside a `ScrollView` — it renders everything at once and destroys performance. Use a virtualized list that only renders what's visible.

```tsx
import { FlatList, Text, View, StyleSheet, Pressable } from "react-native";

type Item = { id: string; name: string; description: string };

const data: Item[] = [
  { id: "1", name: "Apple", description: "A red fruit" },
  { id: "2", name: "Banana", description: "A yellow fruit" },
  { id: "3", name: "Cherry", description: "A small red fruit" },
  // imagine hundreds more...
];

function ItemCard({ item, onPress }: { item: Item; onPress: () => void }) {
  return (
    <Pressable style={styles.card} onPress={onPress}>
      <Text style={styles.name}>{item.name}</Text>
      <Text style={styles.desc}>{item.description}</Text>
    </Pressable>
  );
}

export default function FruitList() {
  return (
    <FlatList
      data={data}
      keyExtractor={(item) => item.id}        // must be unique and stable
      renderItem={({ item }) => (
        <ItemCard
          item={item}
          onPress={() => console.log("Pressed", item.id)}
        />
      )}
      // Separator between items
      ItemSeparatorComponent={() => <View style={styles.separator} />}
      // Message when data is empty
      ListEmptyComponent={<Text style={styles.empty}>No fruits found.</Text>}
      // Header above the list
      ListHeaderComponent={<Text style={styles.header}>All Fruits</Text>}
      // Performance hint: tell FlatList the height of each item
      getItemLayout={(_, index) => ({ length: 72, offset: 72 * index, index })}
      // How many items to render outside the visible area
      windowSize={5}
    />
  );
}

const styles = StyleSheet.create({
  card: { padding: 16, backgroundColor: "#fff" },
  name: { fontSize: 16, fontWeight: "600" },
  desc: { fontSize: 14, color: "#666", marginTop: 2 },
  separator: { height: 1, backgroundColor: "#eee" },
  empty: { textAlign: "center", padding: 32, color: "#999" },
  header: { fontSize: 22, fontWeight: "bold", padding: 16 },
});
```

**`FlatList` vs `SectionList`** — use `SectionList` when your data has categories:

```tsx
import { SectionList } from "react-native";

const sections = [
  { title: "Fruits", data: ["Apple", "Banana"] },
  { title: "Vegetables", data: ["Carrot", "Broccoli"] },
];

<SectionList
  sections={sections}
  keyExtractor={(item, index) => item + index}
  renderItem={({ item }) => <Text>{item}</Text>}
  renderSectionHeader={({ section }) => (
    <Text style={{ fontWeight: "bold", backgroundColor: "#f5f5f5", padding: 8 }}>
      {section.title}
    </Text>
  )}
/>
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
    _layout.tsx      # stack navigator for the posts section
    index.tsx        # "/posts"
    [id].tsx         # dynamic route, e.g. "/posts/42"
  (tabs)/            # tab group (parentheses = group, not URL segment)
    _layout.tsx      # tab bar definition
    home.tsx         # "/home"
    explore.tsx      # "/explore"
```

**Setting up a stack + tabs layout (`app/_layout.tsx`):**

```tsx
import { Stack } from "expo-router";

export default function RootLayout() {
  return (
    <Stack>
      <Stack.Screen name="(tabs)" options={{ headerShown: false }} />
      <Stack.Screen name="profile" options={{ title: "My Profile" }} />
      <Stack.Screen
        name="modal"
        options={{ presentation: "modal" }}
      />
    </Stack>
  );
}
```

**Tab bar (`app/(tabs)/_layout.tsx`):**

```tsx
import { Tabs } from "expo-router";

export default function TabLayout() {
  return (
    <Tabs screenOptions={{ tabBarActiveTintColor: "#007AFF" }}>
      <Tabs.Screen name="home" options={{ title: "Home", tabBarIcon: /* icon */ undefined }} />
      <Tabs.Screen name="explore" options={{ title: "Explore" }} />
      <Tabs.Screen name="profile" options={{ title: "Profile" }} />
    </Tabs>
  );
}
```

Navigate declaratively with `<Link>` or imperatively with the router:

```tsx
import { Link, useRouter, useLocalSearchParams } from "expo-router";

// Declarative — renders as a pressable
<Link href="/profile">Go to profile</Link>
<Link href={{ pathname: "/posts/[id]", params: { id: "42" } }}>Post 42</Link>

// Imperative — use in event handlers
const router = useRouter();
router.push("/posts/42");               // push onto the stack
router.replace("/login");              // replace current screen (no back)
router.back();                          // go back

// Reading route params in the destination screen
const { id } = useLocalSearchParams<{ id: string }>();
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
import { TextInput, Pressable, Text, View, StyleSheet, KeyboardAvoidingView, Platform } from "react-native";

export default function LoginForm() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState<{ email?: string; password?: string }>({});

  function validate() {
    const newErrors: typeof errors = {};
    if (!email.includes("@")) newErrors.email = "Enter a valid email";
    if (password.length < 8) newErrors.password = "Password must be at least 8 characters";
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  function handleSubmit() {
    if (!validate()) return;
    console.log("Submit", { email, password });
  }

  return (
    // KeyboardAvoidingView shifts the form up when the keyboard appears
    <KeyboardAvoidingView
      behavior={Platform.OS === "ios" ? "padding" : "height"}
      style={{ flex: 1 }}
    >
      <View style={styles.form}>
        <Text style={styles.label}>Email</Text>
        <TextInput
          style={[styles.input, errors.email && styles.inputError]}
          value={email}
          onChangeText={setEmail}
          placeholder="you@example.com"
          keyboardType="email-address"
          autoCapitalize="none"
          autoCorrect={false}
          returnKeyType="next"
        />
        {errors.email && <Text style={styles.errorText}>{errors.email}</Text>}

        <Text style={styles.label}>Password</Text>
        <TextInput
          style={[styles.input, errors.password && styles.inputError]}
          value={password}
          onChangeText={setPassword}
          placeholder="Min 8 characters"
          secureTextEntry  // hides the text, no autocorrect
          returnKeyType="done"
          onSubmitEditing={handleSubmit}
        />
        {errors.password && <Text style={styles.errorText}>{errors.password}</Text>}

        <Pressable
          style={({ pressed }) => [styles.button, pressed && styles.buttonPressed]}
          onPress={handleSubmit}
        >
          <Text style={styles.buttonText}>Log In</Text>
        </Pressable>
      </View>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  form: { padding: 24, gap: 8 },
  label: { fontSize: 14, fontWeight: "600", color: "#333" },
  input: {
    borderWidth: 1, borderColor: "#ddd", borderRadius: 8,
    padding: 12, fontSize: 16, backgroundColor: "#fff",
  },
  inputError: { borderColor: "#FF3B30" },
  errorText: { color: "#FF3B30", fontSize: 12 },
  button: { backgroundColor: "#007AFF", padding: 14, borderRadius: 8, alignItems: "center", marginTop: 8 },
  buttonPressed: { opacity: 0.75 },
  buttonText: { color: "#fff", fontWeight: "600", fontSize: 16 },
});
```

For anything beyond a couple of fields, use **React Hook Form** with a schema validator like **Zod**. It handles validation, errors, and submission cleanly and avoids re-rendering on every keystroke:

```tsx
import { useForm, Controller } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { TextInput, Pressable, Text, View } from "react-native";

// Define validation schema
const schema = z.object({
  email: z.string().email("Invalid email"),
  password: z.string().min(8, "Password must be at least 8 characters"),
});

type FormData = z.infer<typeof schema>;

export default function RHFLoginForm() {
  const { control, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const onSubmit = (data: FormData) => {
    console.log("Valid data:", data);
  };

  return (
    <View style={{ padding: 24, gap: 12 }}>
      <Controller
        control={control}
        name="email"
        render={({ field: { onChange, value } }) => (
          <TextInput
            value={value}
            onChangeText={onChange}
            placeholder="Email"
            keyboardType="email-address"
            autoCapitalize="none"
          />
        )}
      />
      {errors.email && <Text style={{ color: "red" }}>{errors.email.message}</Text>}

      <Controller
        control={control}
        name="password"
        render={({ field: { onChange, value } }) => (
          <TextInput value={value} onChangeText={onChange} placeholder="Password" secureTextEntry />
        )}
      />
      {errors.password && <Text style={{ color: "red" }}>{errors.password.message}</Text>}

      <Pressable onPress={handleSubmit(onSubmit)}>
        <Text>Submit</Text>
      </Pressable>
    </View>
  );
}
```

---

## Part 8 — Networking and Data

Basic fetching uses the standard `fetch` API inside `useEffect`:

```tsx
import { useEffect, useState } from "react";
import { View, Text, ActivityIndicator } from "react-native";

type Post = { id: number; title: string; body: string };

export default function PostList() {
  const [posts, setPosts] = useState<Post[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let active = true; // prevents state update if component unmounts

    async function load() {
      try {
        const res = await fetch("https://jsonplaceholder.typicode.com/posts?_limit=10");
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        const data = await res.json();
        if (active) setPosts(data);
      } catch (e) {
        if (active) setError(String(e));
      } finally {
        if (active) setLoading(false);
      }
    }

    load();
    return () => { active = false; };
  }, []);

  if (loading) return <ActivityIndicator style={{ flex: 1 }} />;
  if (error) return <Text style={{ color: "red", padding: 16 }}>Error: {error}</Text>;

  return (
    <View>
      {posts.map(post => (
        <View key={post.id} style={{ padding: 16, borderBottomWidth: 1, borderColor: "#eee" }}>
          <Text style={{ fontWeight: "600" }}>{post.title}</Text>
          <Text style={{ color: "#666" }}>{post.body}</Text>
        </View>
      ))}
    </View>
  );
}
```

That manual pattern gets tedious fast (loading states, errors, caching, refetching). The professional approach is **TanStack Query** (React Query), which handles caching, background refetching, retries, and loading/error states for you:

```tsx
// 1. Setup the provider in your root layout
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60,     // data is "fresh" for 1 minute
      retry: 2,                  // retry failed requests twice
    },
  },
});

export default function RootLayout() {
  return (
    <QueryClientProvider client={queryClient}>
      {/* your app screens */}
    </QueryClientProvider>
  );
}

// 2. Use in any screen
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";

export default function PostsScreen() {
  const { data: posts, isLoading, error, refetch } = useQuery({
    queryKey: ["posts"],   // cache key — changing this triggers a new fetch
    queryFn: () => fetch("/api/posts").then(r => r.json()),
  });

  if (isLoading) return <ActivityIndicator />;
  if (error) return <Text>Error loading posts</Text>;

  return <FlatList data={posts} renderItem={...} />;
}

// 3. Mutations (POST, PUT, DELETE)
const queryClient = useQueryClient();

const createPost = useMutation({
  mutationFn: (newPost) => fetch("/api/posts", {
    method: "POST",
    body: JSON.stringify(newPost),
  }),
  onSuccess: () => {
    // Invalidate the posts query so the list refreshes
    queryClient.invalidateQueries({ queryKey: ["posts"] });
  },
});

// Call it:
createPost.mutate({ title: "Hello", body: "World" });
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

**`useReducer` example — multi-step form:**

```tsx
type Step = "email" | "password" | "profile";

type State = {
  step: Step;
  email: string;
  password: string;
  name: string;
};

type Action =
  | { type: "SET_EMAIL"; email: string }
  | { type: "SET_PASSWORD"; password: string }
  | { type: "SET_NAME"; name: string }
  | { type: "NEXT_STEP" };

function reducer(state: State, action: Action): State {
  switch (action.type) {
    case "SET_EMAIL": return { ...state, email: action.email };
    case "SET_PASSWORD": return { ...state, password: action.password };
    case "SET_NAME": return { ...state, name: action.name };
    case "NEXT_STEP":
      if (state.step === "email") return { ...state, step: "password" };
      if (state.step === "password") return { ...state, step: "profile" };
      return state;
    default: return state;
  }
}

export function useSignupForm() {
  return useReducer(reducer, { step: "email", email: "", password: "", name: "" });
}
```

**Zustand store example:**

```tsx
import { create } from "zustand";

type AuthStore = {
  user: { id: string; name: string } | null;
  token: string | null;
  login: (user: AuthStore["user"], token: string) => void;
  logout: () => void;
};

export const useAuthStore = create<AuthStore>((set) => ({
  user: null,
  token: null,
  login: (user, token) => set({ user, token }),
  logout: () => set({ user: null, token: null }),
}));

// Consume in any component — no provider needed:
function ProfileButton() {
  const { user, logout } = useAuthStore();
  return (
    <Pressable onPress={logout}>
      <Text>Logged in as {user?.name}</Text>
    </Pressable>
  );
}
```

Rule of thumb: server data → React Query; client UI state → Zustand or Context; everything else → local state.

---

## Part 10 — Storage and Persistence

- **AsyncStorage** — simple key-value storage for small, non-sensitive data (settings, flags). It's async and unencrypted.
- **expo-secure-store** — encrypted storage for sensitive data like auth tokens. Use this, never AsyncStorage, for secrets.
- **MMKV** — a much faster synchronous key-value store; popular when AsyncStorage performance matters.
- **SQLite (expo-sqlite)** or **WatermelonDB** — for structured relational data and offline-first apps with lots of records.

```tsx
import AsyncStorage from "@react-native-async-storage/async-storage";
import * as SecureStore from "expo-secure-store";

// Non-sensitive: user preferences
async function saveTheme(theme: "light" | "dark") {
  await AsyncStorage.setItem("theme", theme);
}

async function loadTheme() {
  const theme = await AsyncStorage.getItem("theme");
  return (theme as "light" | "dark") ?? "light";
}

// Sensitive: auth tokens
async function saveToken(token: string) {
  await SecureStore.setItemAsync("auth_token", token);
}

async function loadToken() {
  return await SecureStore.getItemAsync("auth_token");
}

async function clearToken() {
  await SecureStore.deleteItemAsync("auth_token");
}
```

A common beginner mistake is storing JWTs or passwords in AsyncStorage. Use SecureStore for anything sensitive.

**Persist Zustand to AsyncStorage:**

```tsx
import { create } from "zustand";
import { persist, createJSONStorage } from "zustand/middleware";
import AsyncStorage from "@react-native-async-storage/async-storage";

export const useSettingsStore = create(
  persist(
    (set) => ({
      theme: "light" as "light" | "dark",
      fontSize: 16,
      setTheme: (theme: "light" | "dark") => set({ theme }),
    }),
    {
      name: "settings-storage",
      storage: createJSONStorage(() => AsyncStorage),
    }
  )
);
```

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

**Full camera example with permission handling:**

```tsx
import { useState } from "react";
import { View, Text, Pressable, StyleSheet } from "react-native";
import { CameraView, useCameraPermissions } from "expo-camera";

export default function CameraScreen() {
  const [permission, requestPermission] = useCameraPermissions();
  const [facing, setFacing] = useState<"front" | "back">("back");

  // State 1: Loading permissions
  if (!permission) {
    return <View style={styles.container}><Text>Loading...</Text></View>;
  }

  // State 2: Permission denied — always handle this!
  if (!permission.granted) {
    return (
      <View style={styles.container}>
        <Text style={styles.message}>
          Camera access is required to use this feature.
        </Text>
        <Pressable style={styles.button} onPress={requestPermission}>
          <Text style={styles.buttonText}>Grant Access</Text>
        </Pressable>
      </View>
    );
  }

  // State 3: Permission granted — show camera
  return (
    <View style={styles.container}>
      <CameraView style={styles.camera} facing={facing}>
        <View style={styles.controls}>
          <Pressable
            style={styles.button}
            onPress={() => setFacing(f => f === "back" ? "front" : "back")}
          >
            <Text style={styles.buttonText}>Flip</Text>
          </Pressable>
        </View>
      </CameraView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, justifyContent: "center" },
  camera: { flex: 1 },
  controls: { position: "absolute", bottom: 40, alignSelf: "center" },
  message: { textAlign: "center", padding: 16, fontSize: 16 },
  button: { backgroundColor: "#007AFF", padding: 14, borderRadius: 8 },
  buttonText: { color: "#fff", fontWeight: "600" },
});
```

**Declare permissions in `app.json`:**

```json
{
  "expo": {
    "plugins": [
      ["expo-camera", { "cameraPermission": "Used to scan QR codes" }],
      ["expo-location", { "locationWhenInUsePermission": "Used to show nearby items" }],
      ["expo-notifications", {}]
    ]
  }
}
```

---

## Part 12 — Performance Optimization

Performance problems usually come from a handful of sources. In rough priority order:

1. **List rendering** — use `FlatList`/`FlashList`, stable keys, and `getItemLayout` where possible. This is the #1 source of jank.
2. **Unnecessary re-renders** — wrap pure components in `React.memo`, stabilize callbacks with `useCallback`, and memoize expensive values with `useMemo`. Profile before optimizing; premature memoization adds complexity for nothing.
3. **Heavy work on the JS thread** — move expensive computations off the render path. Animations should run on the UI thread.
4. **Animations** — use **Reanimated** (and **Gesture Handler**), which run animations on the native/UI thread so they stay at 60fps even if JS is busy. Avoid animating with `setState` in a loop.
5. **Images** — use `expo-image` for caching and efficient loading; size images appropriately rather than shipping huge files.
6. **Bundle size and startup** — Hermes (the default JS engine) precompiles to bytecode for faster startup; keep dependencies lean.

**Reanimated example — a smooth fade-in:**

```tsx
import Animated, { useSharedValue, useAnimatedStyle, withTiming, withSpring } from "react-native-reanimated";
import { useEffect } from "react";

export default function FadeInCard() {
  // Shared values live on the native thread
  const opacity = useSharedValue(0);
  const translateY = useSharedValue(20);

  useEffect(() => {
    // These animations run on the UI thread — no JS involvement during animation
    opacity.value = withTiming(1, { duration: 400 });
    translateY.value = withSpring(0, { damping: 15, stiffness: 100 });
  }, []);

  const animatedStyle = useAnimatedStyle(() => ({
    opacity: opacity.value,
    transform: [{ translateY: translateY.value }],
  }));

  return (
    <Animated.View style={[{ padding: 16, backgroundColor: "#fff", borderRadius: 12 }, animatedStyle]}>
      <Text>I faded in!</Text>
    </Animated.View>
  );
}
```

**`React.memo` to prevent unnecessary re-renders:**

```tsx
// Without memo: re-renders every time the parent re-renders
function ExpensiveRow({ name, score }: { name: string; score: number }) {
  return <Text>{name}: {score}</Text>;
}

// With memo: only re-renders when name or score actually change
const ExpensiveRow = React.memo(function ExpensiveRow({ name, score }: { name: string; score: number }) {
  return <Text>{name}: {score}</Text>;
});
```

Use the built-in performance monitor and tools like Flipper or the React DevTools profiler to find the actual bottleneck rather than guessing.

---

## Part 13 — Going Native (Advanced)

Most apps never need this, but when an existing library doesn't cover a native capability, you can write your own native module.

- **Expo Modules API** — the modern, recommended way to write native modules in Swift/Kotlin. Modules written with this API support the New Architecture by default with no extra work.
- **Config plugins** — modify native project configuration (entitlements, `Info.plist`, Gradle) declaratively from JS, so you keep the benefits of managed builds.
- **The bare workflow / prebuild** — when you need full control over the `ios/` and `android/` directories. `npx expo prebuild` generates the native projects from your config.

**Creating a config plugin (`my-plugin.js`):**

```js
// A config plugin that adds a custom permission to Info.plist on iOS
const { withInfoPlist } = require("@expo/config-plugins");

module.exports = function withMyPlugin(config) {
  return withInfoPlist(config, (config) => {
    config.modResults.NSFaceIDUsageDescription = "Used for biometric login";
    return config;
  });
};
```

**Add it to `app.json`:**

```json
{
  "expo": {
    "plugins": ["./my-plugin.js"]
  }
}
```

When evaluating any third-party native library, check its New Architecture support first — run `npx expo-doctor`, which validates your dependencies against the React Native Directory and flags unmaintained or incompatible packages.

---

## Part 14 — Testing

A practical testing pyramid for RN:

- **Unit tests** — **Jest** for pure functions and logic. Fast, run constantly.
- **Component tests** — **React Native Testing Library** to render components and assert on what the user sees and does, rather than implementation details.
- **End-to-end tests** — **Maestro** (simple, modern, increasingly the default) or **Detox** drives the actual app on a device/emulator through real user flows.

```tsx
// Counter.test.tsx
import { render, screen, fireEvent } from "@testing-library/react-native";
import Counter from "./Counter";

describe("Counter", () => {
  it("starts at zero", () => {
    render(<Counter />);
    expect(screen.getByText("Count: 0")).toBeTruthy();
  });

  it("increments on press", () => {
    render(<Counter />);
    fireEvent.press(screen.getByText("+ Increment"));
    expect(screen.getByText("Count: 1")).toBeTruthy();
  });

  it("decrements on press", () => {
    render(<Counter />);
    fireEvent.press(screen.getByText("+ Increment"));
    fireEvent.press(screen.getByText("+ Increment"));
    fireEvent.press(screen.getByText("- Decrement"));
    expect(screen.getByText("Count: 1")).toBeTruthy();
  });

  it("resets to zero", () => {
    render(<Counter />);
    fireEvent.press(screen.getByText("+ Increment"));
    fireEvent.press(screen.getByText("Reset"));
    expect(screen.getByText("Count: 0")).toBeTruthy();
  });
});
```

**Testing async data fetching with mock:**

```tsx
import { render, screen, waitFor } from "@testing-library/react-native";
import PostList from "./PostList";

// Mock the fetch function
global.fetch = jest.fn(() =>
  Promise.resolve({
    ok: true,
    json: () => Promise.resolve([{ id: 1, title: "Test Post", body: "Body" }]),
  })
) as jest.Mock;

test("renders posts", async () => {
  render(<PostList />);
  await waitFor(() => {
    expect(screen.getByText("Test Post")).toBeTruthy();
  });
});
```

**Maestro E2E test (`.maestro/login.yaml`):**

```yaml
appId: com.yourapp.id
---
- launchApp
- assertVisible: "Email"
- tapOn: "Email"
- inputText: "test@example.com"
- tapOn: "Password"
- inputText: "password123"
- tapOn: "Log In"
- assertVisible: "Welcome"
```

Run it with: `maestro test .maestro/login.yaml`

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

**`eas.json` — build profiles:**

```json
{
  "cli": { "version": ">= 7.0.0" },
  "build": {
    "development": {
      "developmentClient": true,
      "distribution": "internal",
      "env": { "API_URL": "https://dev.api.example.com" }
    },
    "preview": {
      "distribution": "internal",
      "env": { "API_URL": "https://staging.api.example.com" }
    },
    "production": {
      "autoIncrement": true,
      "env": { "API_URL": "https://api.example.com" }
    }
  },
  "submit": {
    "production": {
      "ios": { "appleId": "you@example.com", "ascAppId": "123456789" },
      "android": { "serviceAccountKeyPath": "./google-service-account.json" }
    }
  }
}
```

**OTA updates with EAS Update:**

```bash
# Ship a JS fix without going through App Store review
eas update --branch production --message "Fix crash on profile screen"
```

**CI/CD with GitHub Actions (`.github/workflows/deploy.yml`):**

```yaml
name: EAS Build & Deploy
on:
  push:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: 20 }
      - run: npm ci
      - uses: expo/expo-github-action@v8
        with:
          eas-version: latest
          token: ${{ secrets.EXPO_TOKEN }}
      - run: eas build --profile production --platform all --non-interactive
      - run: eas submit --profile production --platform all --non-interactive
```

Key concept: **bump the runtime version when you change native code** (a new native module, an Expo SDK upgrade, edits to `android/` or `ios/`). Pure JavaScript changes ship via `eas update` and do *not* need a runtime bump or store review.

---

## Part 16 — Project Structure and Best Practices

A maintainable layout that scales:

```
app/                 # screens / routes (Expo Router)
components/           # reusable UI components
features/             # feature-based folders (auth, feed, profile)
  auth/
    components/       # AuthForm, AuthButton
    hooks/            # useLogin, useSignup
    services/         # authService.ts
    types.ts
services/             # API clients, storage wrappers
hooks/                # custom hooks
lib/ or utils/        # pure helpers
constants/            # colors, config
types/                # shared TypeScript types
```

**A typed service layer (`services/postsService.ts`):**

```ts
import type { Post } from "../types";

const API_URL = process.env.EXPO_PUBLIC_API_URL ?? "https://api.example.com";

export async function getPosts(): Promise<Post[]> {
  const res = await fetch(`${API_URL}/posts`, {
    headers: { "Content-Type": "application/json" },
  });
  if (!res.ok) throw new Error(`Failed to fetch posts: ${res.status}`);
  return res.json();
}

export async function createPost(payload: Omit<Post, "id">): Promise<Post> {
  const res = await fetch(`${API_URL}/posts`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error(`Failed to create post: ${res.status}`);
  return res.json();
}
```

Habits that pay off:
- **TypeScript everywhere**, strict mode on.
- A **service layer** so screens don't call `fetch` directly — they call typed functions, which makes testing and refactoring far easier.
- **Environment variables** for API URLs and keys (never hardcode secrets; use EAS secrets for build-time values). Prefix with `EXPO_PUBLIC_` to expose to client code.
- **A consistent component pattern** — small, focused, single-responsibility components.
- **Lint and format** with ESLint + Prettier, enforced in CI.

**`tsconfig.json` with strict mode:**

```json
{
  "extends": "expo/tsconfig.base",
  "compilerOptions": {
    "strict": true,
    "paths": {
      "@/*": ["./*"]
    }
  }
}
```

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

**Quick reference — common mistakes and fixes:**

```tsx
// ❌ Raw text in View
<View>Hello</View>
// ✅ Fix
<View><Text>Hello</Text></View>

// ❌ Long list with ScrollView
<ScrollView>{items.map(i => <Row key={i.id} item={i} />)}</ScrollView>
// ✅ Fix
<FlatList data={items} renderItem={({ item }) => <Row item={item} />} keyExtractor={i => i.id} />

// ❌ Token in AsyncStorage
AsyncStorage.setItem("token", jwt);
// ✅ Fix
SecureStore.setItemAsync("token", jwt);

// ❌ Missing dependency in useEffect
useEffect(() => { load(userId); }, []); // userId change won't re-trigger
// ✅ Fix
useEffect(() => { load(userId); }, [userId]);

// ❌ Animating with setState (runs on JS thread, causes jank)
setInterval(() => setX(x => x + 1), 16);
// ✅ Fix — Reanimated runs on UI thread
const x = useSharedValue(0);
x.value = withRepeat(withTiming(100), -1, true);
```

---

## Part 18 — A Suggested Learning Path

1. **Week 1** — Build a static screen: components, props, `useState`, StyleSheet, Flexbox.
2. **Week 2** — Add a `FlatList` of data, multiple screens with Expo Router, and a form.
3. **Week 3** — Fetch from a real API with React Query; add loading/error states; persist a setting with AsyncStorage.
4. **Week 4** — Add auth (Supabase), SecureStore for tokens, and one device feature (camera or notifications).
5. **Week 5** — Add Reanimated animations, write tests, profile and fix a performance issue.
6. **Week 6** — Configure EAS, build a real binary, and ship a TestFlight / internal track release.

Build one complete app end-to-end rather than collecting tutorials. A task manager, a small social feed, or a habit tracker each touches every concept above.

**A simple project idea — Habit Tracker — which touches everything:**

| Feature | Concept practiced |
|---|---|
| Habit list | FlatList, TypeScript types |
| Add habit form | React Hook Form, Zod validation |
| Mark complete | useState, AsyncStorage persistence |
| Streak counter | useReducer, useMemo |
| Reminder notifications | expo-notifications, permissions |
| Animated check | Reanimated, Gesture Handler |
| Daily reset | useEffect, background tasks |
| Share habit | expo-sharing, native modules |
| Deploy | EAS Build, EAS Update |

### Where to keep learning

- **Official React Native docs** — reactnative.dev
- **Expo docs** — docs.expo.dev (the most relevant for the modern stack)
- **React Native Directory** — reactnative.directory (check library compatibility)
- **TanStack Query, Reanimated, React Hook Form, Zustand** — each has excellent docs worth reading directly.

---

*The single biggest accelerator is shipping. A small, finished, deployed app teaches more than a dozen half-built tutorials.*
