import { ApiService } from './api-service';
import { describe, it, expect, beforeEach, vi } from 'vitest';

describe('ApiService', () => {
  let service: ApiService;

  beforeEach(() => {
    service = new ApiService();
    vi.stubGlobal('fetch', vi.fn());
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should perform GET request', async () => {
    const mockData = { name: 'Test' };
    const mockResponse = new Response(JSON.stringify(mockData), {
      status: 200,
      headers: { 'Content-Type': 'application/json' },
    });
    vi.mocked(fetch).mockResolvedValue(mockResponse);

    const data = await service.get<{ name: string }>('/test');
    expect(data).toEqual(mockData);
    expect(fetch).toHaveBeenCalledWith('/test', { method: 'GET' });
  });

  it('should perform POST request', async () => {
    const mockData = { id: 1 };
    const body = { name: 'New Test' };
    const mockResponse = new Response(JSON.stringify(mockData), {
      status: 201,
      headers: { 'Content-Type': 'application/json' },
    });
    vi.mocked(fetch).mockResolvedValue(mockResponse);

    const data = await service.post<{ id: number }>('/test', body);
    expect(data).toEqual(mockData);
    expect(fetch).toHaveBeenCalledWith('/test', {
      method: 'POST',
      body: JSON.stringify(body),
      headers: { 'Content-Type': 'application/json' },
    });
  });

  it('should perform PUT request', async () => {
    const mockData = { id: 1 };
    const body = { name: 'Updated Test' };
    const mockResponse = new Response(JSON.stringify(mockData), {
      status: 200,
      headers: { 'Content-Type': 'application/json' },
    });
    vi.mocked(fetch).mockResolvedValue(mockResponse);

    const data = await service.put<{ id: number }>('/test', body);
    expect(data).toEqual(mockData);
    expect(fetch).toHaveBeenCalledWith('/test', {
      method: 'PUT',
      body: JSON.stringify(body),
      headers: { 'Content-Type': 'application/json' },
    });
  });

  it('should perform DELETE request', async () => {
    const mockResponse = new Response(null, { status: 204 });
    vi.mocked(fetch).mockResolvedValue(mockResponse);

    await service.delete<void>('/test');
    expect(fetch).toHaveBeenCalledWith('/test', { method: 'DELETE' });
  });

  it('should throw error on failed response', async () => {
    const mockResponse = new Response('Not Found', { status: 404 });
    vi.mocked(fetch).mockResolvedValue(mockResponse);

    await expect(service.get('/test')).rejects.toThrow('HTTP Error: 404 - Not Found');
  });
});
