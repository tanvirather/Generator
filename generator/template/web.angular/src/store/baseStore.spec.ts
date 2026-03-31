import { describe, it, expect, beforeEach, vi } from 'vitest';
import { BaseStore } from './baseStore';
import { ApiService } from '../tools/api-service';

describe('BaseStore', () => {
  let store: BaseStore<any>;
  let apiService: ApiService;
  const baseUrl = 'https://api.example.com/items';

  beforeEach(() => {
    store = new BaseStore<any>(baseUrl);
    apiService = (store as any).apiService;

    // Mock methods on apiService
    vi.spyOn(apiService, 'get');
    vi.spyOn(apiService, 'post');
    vi.spyOn(apiService, 'put');
    vi.spyOn(apiService, 'delete');
  });

  it('should be created', () => {
    expect(store).toBeTruthy();
  });

  it('should call apiService.get with baseUrl when getAll() is called', async () => {
    const mockData = [{ id: 1, name: 'Item 1' }];
    vi.mocked(apiService.get).mockResolvedValue(mockData);

    const result = await store.getAll();

    expect(apiService.get).toHaveBeenCalledWith(baseUrl);
    expect(result).toEqual(mockData);
  });

  it('should call getAll() when get() is called', async () => {
    const mockData = [{ id: 1, name: 'Item 1' }];
    const getAllSpy = vi.spyOn(store, 'getAll').mockResolvedValue(mockData);

    const result = await store.get();

    expect(getAllSpy).toHaveBeenCalled();
    expect(result).toEqual(mockData);
  });

  it('should call apiService.get with baseUrl/id when getById() is called', async () => {
    const mockData = { id: 1, name: 'Item 1' };
    const id = 1;
    vi.mocked(apiService.get).mockResolvedValue(mockData);

    const result = await store.getById(id);

    expect(apiService.get).toHaveBeenCalledWith(`${baseUrl}/${id}`);
    expect(result).toEqual(mockData);
  });

  it('should call apiService.post with baseUrl and data when create() is called', async () => {
    const mockData = { name: 'New Item' };
    const returnedData = { id: 2, ...mockData };
    vi.mocked(apiService.post).mockResolvedValue(returnedData);

    const result = await store.create(mockData);

    expect(apiService.post).toHaveBeenCalledWith(baseUrl, mockData);
    expect(result).toEqual(returnedData);
  });

  it('should call apiService.put with baseUrl/id and data when update() is called', async () => {
    const id = 1;
    const mockData = { name: 'Updated Item' };
    const returnedData = { id, ...mockData };
    vi.mocked(apiService.put).mockResolvedValue(returnedData);

    const result = await store.update(id, mockData);

    expect(apiService.put).toHaveBeenCalledWith(`${baseUrl}/${id}`, mockData);
    expect(result).toEqual(returnedData);
  });

  it('should call apiService.delete with baseUrl/id when delete() is called', async () => {
    const id = 1;
    vi.mocked(apiService.delete).mockResolvedValue(undefined);

    await store.delete(id);

    expect(apiService.delete).toHaveBeenCalledWith(`${baseUrl}/${id}`);
  });
});
