# SessionStorageService

A utility service for interacting with the browser's `sessionStorage` API, with built-in support for automatic JSON serialization/deserialization. Data stored here persists for the duration of the page session.

## Features

- **Automatic JSON conversion**: Automatically stringifies values on `setItem` and parses them on `getItem`.
- **Type Safety**: Supports generic types for `getItem` to ensure type-safe data retrieval.
- **Fallbacks**: Gracefully handles parsing errors by returning the raw string if JSON parsing fails.

## Methods

### `setItem(key: string, value: any): void`
Stores a value in `sessionStorage`. If the value is not a string, it is automatically stringified using `JSON.stringify`.

### `getItem<T>(key: string): T | null`
Retrieves a value from `sessionStorage`. It attempts to parse the value using `JSON.parse`. If parsing fails, it returns the raw string.

### `removeItem(key: string): void`
Removes a specific item from `sessionStorage`.

### `clear(): void`
Removes all items from `sessionStorage`.

### `key(index: number): string | null`
Returns the name of the nth key in the storage.

### `length: number` (Getter)
Returns the number of items stored in the `sessionStorage`.

## Usage Example

```typescript
const storage = new SessionStorageService();

// Storing data for the current session
storage.setItem('user_session', { loginAt: new Date(), id: '123' });

// Retrieving session data
const sessionInfo = storage.getItem<{ loginAt: string; id: string }>('user_session');
console.log(sessionInfo?.id); // '123'
```
