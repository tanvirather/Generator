export class ApiService {
  async get<T>(url: string, options?: RequestInit): Promise<T> {
    const response = await fetch(url, { method: 'GET', ...options });
    return this.handleResponse<T>(response);
  }

  async post<T>(url: string, body: any, options?: RequestInit): Promise<T> {
    const response = await fetch(url, {
      method: 'POST',
      body: JSON.stringify(body),
      headers: { 'Content-Type': 'application/json', ...options?.headers },
      ...options,
    });
    return this.handleResponse<T>(response);
  }

  async put<T>(url: string, body: any, options?: RequestInit): Promise<T> {
    const response = await fetch(url, {
      method: 'PUT',
      body: JSON.stringify(body),
      headers: { 'Content-Type': 'application/json', ...options?.headers },
      ...options,
    });
    return this.handleResponse<T>(response);
  }

  async delete<T>(url: string, options?: RequestInit): Promise<T> {
    const response = await fetch(url, { method: 'DELETE', ...options });
    return this.handleResponse<T>(response);
  }

  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`HTTP Error: ${response.status} - ${errorText}`);
    }
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      return response.json();
    }
    return (await response.text()) as any;
  }
}
