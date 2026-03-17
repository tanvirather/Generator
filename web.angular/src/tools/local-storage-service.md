# LocalStorageService

A utility service for interacting with the browser's `localStorage` API, with built-in support for automatic JSON serialization/deserialization.

## Features

- **Automatic JSON conversion**: Automatically stringifies values on `setItem` and parses them on `getItem`.
- **Type Safety**: Supports generic types for `getItem` to ensure type-safe data retrieval.
- **Fallbacks**: Gracefully handles parsing errors by returning the raw string if JSON parsing fails.

## Methods

### `setItem(key: string, value: any): void`
Stores a value in `localStorage`. If the value is not a string, it is automatically stringified using `JSON.stringify`.

### `getItem<T>(key: string): T | null`
Retrieves a value from `localStorage`. It attempts to parse the value using `JSON.parse`. If parsing fails, it returns the raw string.

### `removeItem(key: string): void`
Removes a specific item from `localStorage`.

### `clear(): void`
Removes all items from `localStorage`.

### `key(index: number): string | null`
Returns the name of the nth key in the storage.

### `length: number` (Getter)
Returns the number of items stored in the `localStorage`.

## Usage Example

```typescript
const storage = new LocalStorageService();

// Storing an object
storage.setItem('user', { id: 1, name: 'John Doe' });

// Retrieving an object
const user = storage.getItem<{ id: number; name: string }>('user');
console.log(user?.name); // 'John Doe'

// Storing a simple string
storage.setItem('theme', 'dark');
const theme = storage.getItem<string>('theme');
console.log(theme); // 'dark'
```
