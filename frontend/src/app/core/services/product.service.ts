import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = 'https://localhost:7265/api/Product';

  constructor(private http: HttpClient) { }

  private getAuthHeaders() {
    const token = localStorage.getItem('token');

    return {
      Authorization: `Bearer ${token}`
    };
  }

  getAll(name?: string, categoryId?: number | null): Observable<any[]> {
    let params = new HttpParams();

    if (name && name.trim() !== '') {
      params = params.set('name', name.trim());
    }

    if (categoryId !== null && categoryId !== undefined) {
      params = params.set('categoryId', categoryId);
    }

    return this.http.get<any[]>(this.apiUrl, {
      headers: this.getAuthHeaders(),
      params
    });
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  create(product: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, product, {
      headers: this.getAuthHeaders()
    });
  }

  update(id: number, product: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, product, {
      headers: this.getAuthHeaders()
    });
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }
}