# ApiService

A general-purpose HTTP client service built on the browser's `fetch` API. It provides a simple, Promise-based interface for standard RESTful operations with automatic JSON response handling.

## Features

- **Standard REST Methods**: Supports `GET`, `POST`, `PUT`, and `DELETE`.
- **Automatic JSON Parsing**: Automatically parses response as JSON if the `content-type` header is `application/json`.
- **Error Handling**: Throws structured error messages for non-successful HTTP status codes.
- **Customizable**: Allows passing `RequestInit` options for headers, credentials, and more.

## Methods

### `get<T>(url: string, options?: RequestInit): Promise<T>`
Performs a `GET` request to the specified URL.

### `post<T>(url: string, body: any, options?: RequestInit): Promise<T>`
Performs a `POST` request with a JSON body.

### `put<T>(url: string, body: any, options?: RequestInit): Promise<T>`
Performs a `PUT` request with a JSON body.

### `delete<T>(url: string, options?: RequestInit): Promise<T>`
Performs a `DELETE` request.

## Usage Example

```typescript
const api = new ApiService();

// GET data
const users = await api.get<User[]>('/api/users');

// POST data
const newUser = await api.post<User>('/api/users', { name: 'New User' });

// Custom options
const data = await api.get<any>('/api/data', {
  headers: { 'Authorization': 'Bearer <token>' }
});
```
