import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7265/api/auth';

  constructor(private http: HttpClient) {}

  // 🔐 LOGIN
  login(data: any) {
    return this.http.post<any>(`${this.apiUrl}/login`, data)
      .pipe(
        tap(res => {
          console.log("RESPUESTA LOGIN:", res);

          // ⚠️ Ajusta esto según tu backend
          const token = res.token || res.accessToken;

          if (token) {
            this.saveToken(token);
          } else {
            console.error("No se encontró el token en la respuesta");
          }
        })
      );
  }

  //  GUARDAR TOKEN
  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  // 📦 OBTENER TOKEN
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // 🎭 OBTENER ROL
  getRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);

    
      return decoded.role 
        || decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
        || null;

    } catch (error) {
      console.error("Error decodificando token:", error);
      return null;
    }
  }

  // LOGOUT
  logout() {
    localStorage.removeItem('token');
  }
}