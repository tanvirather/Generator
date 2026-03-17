import { IndexedDbService } from './indexed-db-service';
import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import 'fake-indexeddb/auto';

describe('IndexedDbService', () => {
  let service: IndexedDbService;
  const dbName = 'test-db';
  const storeName = 'test-store';

  beforeEach(async () => {
    service = new IndexedDbService();
    await service.open(dbName, 1, [storeName]);
  });

  afterEach(async () => {
    await service.clear(storeName);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add data', async () => {
    const data = { id: 1, name: 'Test 1' };
    const id = await service.add(storeName, data);
    expect(id).toBe(1);

    const retrieved = await service.get<{ id: number; name: string }>(storeName, 1);
    expect(retrieved).toEqual(data);
  });

  it('should put data (update)', async () => {
    const data = { id: 1, name: 'Test 1' };
    await service.add(storeName, data);

    const updatedData = { id: 1, name: 'Updated Test' };
    await service.put(storeName, updatedData);

    const retrieved = await service.get<{ id: number; name: string }>(storeName, 1);
    expect(retrieved).toEqual(updatedData);
  });

  it('should get all data', async () => {
    await service.add(storeName, { name: 'Item 1' });
    await service.add(storeName, { name: 'Item 2' });

    const all = await service.getAll<{ name: string }>(storeName);
    expect(all.length).toBe(2);
    expect(all[0].name).toBe('Item 1');
    expect(all[1].name).toBe('Item 2');
  });

  it('should delete data', async () => {
    await service.add(storeName, { id: 1, name: 'Item 1' });
    await service.delete(storeName, 1);

    const retrieved = await service.get(storeName, 1);
    expect(retrieved).toBeUndefined();
  });

  it('should clear all data', async () => {
    await service.add(storeName, { name: 'Item 1' });
    await service.add(storeName, { name: 'Item 2' });
    await service.clear(storeName);

    const all = await service.getAll(storeName);
    expect(all.length).toBe(0);
  });

  it('should throw error if db is not opened', async () => {
    const closedService = new IndexedDbService();
    await expect(closedService.add(storeName, { name: 'Item' })).rejects.toThrow('Database is not opened. Call open() first.');
  });
});
