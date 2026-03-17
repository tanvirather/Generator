export class SessionStorageService {
  setItem(key: string, value: any): void {
    const stringValue = typeof value === 'string' ? value : JSON.stringify(value);
    sessionStorage.setItem(key, stringValue);
  }

  getItem<T>(key: string): T | null {
    const value = sessionStorage.getItem(key);
    if (value === null) {
      return null;
    }

    try {
      return JSON.parse(value) as T;
    } catch (e) {
      return value as unknown as T;
    }
  }

  removeItem(key: string): void {
    sessionStorage.removeItem(key);
  }

  clear(): void {
    sessionStorage.clear();
  }

  key(index: number): string | null {
    return sessionStorage.key(index);
  }

  get length(): number {
    return sessionStorage.length;
  }
}
