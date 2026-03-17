# IndexedDbService

A wrapper service for the IndexedDB API providing a Promise-based interface for database operations.

## Features

- **Promise-based**: Simplifies complex asynchronous operations using `async`/`await`.
- **Automatic Setup**: Database opening handles `onupgradeneeded` automatically to create object stores.
- **Transaction Safety**: All operations are wrapped in transactions for data integrity.

## Methods

### `open(dbName: string, version: number, storeNames: string[]): Promise<IDBDatabase>`
Opens a database connection and creates specified object stores if they don't exist.

### `add(storeName: string, data: any): Promise<IDBValidKey>`
Adds a new record to the specified store.

### `put(storeName: string, data: any): Promise<IDBValidKey>`
Updates a record or adds it if it doesn't exist.

### `get<T>(storeName: string, id: IDBValidKey): Promise<T>`
Retrieves a record by its ID.

### `getAll<T>(storeName: string): Promise<T[]>`
Retrieves all records from the specified store.

### `delete(storeName: string, id: IDBValidKey): Promise<void>`
Deletes a record by its ID.

### `clear(storeName: string): Promise<void>`
Removes all records from the specified store.

## Usage Example

```typescript
const dbService = new IndexedDbService();

// Initialize the database
await dbService.open('MyDatabase', 1, ['users', 'settings']);

// Add a record
await dbService.add('users', { name: 'Alice', email: 'alice@example.com' });

// Get all records
const allUsers = await dbService.getAll<any>('users');
console.log(allUsers);
```
