import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SaleService {

  private apiUrl = 'https://localhost:7265/api/Sales';

  constructor(private http: HttpClient) {}

  private getAuthHeaders() {
    const token = localStorage.getItem('token');

    return {
      Authorization: `Bearer ${token}`
    };
  }

  create(sale: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, sale, {
      headers: this.getAuthHeaders()
    });
  }

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl, {
      headers: this.getAuthHeaders()
    });
  }

  getById(saleId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${saleId}`, {
      headers: this.getAuthHeaders()
    });
  }
}