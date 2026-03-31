import { ApiService } from '../tools/api-service';

export class BaseStore<T = any> {
  protected apiService = new ApiService();
  protected readonly baseUrl: string;

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  async get(): Promise<T[]> {
    return this.getAll();
  }

  async getAll(): Promise<T[]> {
    return this.apiService.get<T[]>(this.baseUrl);
  }

  async getById(id: number | string): Promise<T> {
    return this.apiService.get<T>(`${this.baseUrl}/${id}`);
  }

  async create(data: T): Promise<T> {
    return this.apiService.post<T>(this.baseUrl, data);
  }

  async update(id: number | string, data: T): Promise<T> {
    return this.apiService.put<T>(`${this.baseUrl}/${id}`, data);
  }

  async delete(id: number | string): Promise<void> {
    return this.apiService.delete<void>(`${this.baseUrl}/${id}`);
  }
}

