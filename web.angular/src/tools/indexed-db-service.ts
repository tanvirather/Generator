export class IndexedDbService {
  private db: IDBDatabase | null = null;

  async open(dbName: string, version: number, storeNames: string[]): Promise<IDBDatabase> {
    return new Promise((resolve, reject) => {
      const request = indexedDB.open(dbName, version);

      request.onupgradeneeded = (event) => {
        const db = (event.target as IDBOpenDBRequest).result;
        storeNames.forEach((storeName) => {
          if (!db.objectStoreNames.contains(storeName)) {
            db.createObjectStore(storeName, { keyPath: 'id', autoIncrement: true });
          }
        });
      };

      request.onsuccess = (event) => {
        this.db = (event.target as IDBOpenDBRequest).result;
        resolve(this.db);
      };

      request.onerror = (event) => {
        reject((event.target as IDBOpenDBRequest).error);
      };
    });
  }

  async add(storeName: string, data: any): Promise<IDBValidKey> {
    return this.performTransaction(storeName, 'readwrite', (store) => store.add(data));
  }

  async put(storeName: string, data: any): Promise<IDBValidKey> {
    return this.performTransaction(storeName, 'readwrite', (store) => store.put(data));
  }

  async get<T>(storeName: string, id: IDBValidKey): Promise<T> {
    return this.performTransaction(storeName, 'readonly', (store) => store.get(id));
  }

  async getAll<T>(storeName: string): Promise<T[]> {
    return this.performTransaction(storeName, 'readonly', (store) => store.getAll());
  }

  async delete(storeName: string, id: IDBValidKey): Promise<void> {
    return this.performTransaction(storeName, 'readwrite', (store) => store.delete(id));
  }

  async clear(storeName: string): Promise<void> {
    return this.performTransaction(storeName, 'readwrite', (store) => store.clear());
  }

  private async performTransaction<T>(
    storeName: string,
    mode: IDBTransactionMode,
    operation: (store: IDBObjectStore) => IDBRequest<T>
  ): Promise<T> {
    if (!this.db) {
      throw new Error('Database is not opened. Call open() first.');
    }

    return new Promise((resolve, reject) => {
      const transaction = this.db!.transaction(storeName, mode);
      const store = transaction.objectStore(storeName);
      const request = operation(store);

      request.onsuccess = () => resolve(request.result);
      request.onerror = () => reject(request.error);
    });
  }
}
