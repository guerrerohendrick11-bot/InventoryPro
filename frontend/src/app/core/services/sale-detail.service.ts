import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SaleDetailService {

  private apiUrl = 'https://localhost:7265/api/SaleDetails';

  constructor(private http: HttpClient) {}

  private getAuthHeaders() {
    const token = localStorage.getItem('token');

    return {
      Authorization: `Bearer ${token}`
    };
  }

  create(detail: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, detail, {
      headers: this.getAuthHeaders()
    });
  }

  getBySaleId(saleId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/by-sale/${saleId}`, {
      headers: this.getAuthHeaders()
    });
  }
}