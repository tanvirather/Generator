import { LocalStorageService } from './local-storage-service';
import { describe, it, expect, beforeEach, afterEach } from 'vitest';

/**
 * @vitest-environment jsdom
 */

describe('LocalStorageService', () => {
  let service: LocalStorageService;

  beforeEach(() => {
    service = new LocalStorageService();
    localStorage.clear();
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should set and get string item', () => {
    const key = 'test-key';
    const value = 'test-value';
    service.setItem(key, value);
    expect(service.getItem<string>(key)).toBe(value);
  });

  it('should set and get object item', () => {
    const key = 'test-object';
    const value = { name: 'Test', age: 30 };
    service.setItem(key, value);
    expect(service.getItem<{ name: string; age: number }>(key)).toEqual(value);
  });

  it('should set and get array item', () => {
    const key = 'test-array';
    const value = [1, 2, 3];
    service.setItem(key, value);
    expect(service.getItem<number[]>(key)).toEqual(value);
  });

  it('should return null for non-existent key', () => {
    expect(service.getItem('non-existent')).toBeNull();
  });

  it('should remove item', () => {
    const key = 'test-remove';
    service.setItem(key, 'value');
    service.removeItem(key);
    expect(service.getItem(key)).toBeNull();
  });

  it('should clear all items', () => {
    service.setItem('key1', 'value1');
    service.setItem('key2', 'value2');
    service.clear();
    expect(service.length).toBe(0);
  });

  it('should return key at index', () => {
    service.setItem('key1', 'value1');
    const key = service.key(0);
    expect(key).toBe('key1');
  });

  it('should return correct length', () => {
    service.setItem('key1', 'value1');
    service.setItem('key2', 'value2');
    expect(service.length).toBe(2);
  });
});
