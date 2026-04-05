import { importProvidersFrom, inject, Injectable } from "@angular/core";
import { HttpInterceptorFn, HttpRequest,HttpHandler, HttpInterceptor } from "@angular/common/http";

@Injectable()
export class JwtInterceptor implements HttpInterceptor{
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = localStorage.getItem('token');
    if (token) {
      const cloned = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
      return next.handle(cloned);
    } else {
      return next.handle(req);
    }
  }
}