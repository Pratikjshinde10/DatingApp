import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS, HttpHandler, HttpRequest } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';
import { stat } from 'fs';

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(error => {
         if ( error.status === 401) {
             return throwError(error.statusText);
         }
        if ( error instanceof HttpErrorResponse) {
            const applicationError = error.headers.get('Application-Error');
            if (applicationError) {
              console.error(applicationError);
              return throwError(applicationError);
            }
            const serverError = error.error;
            let modalStateErrors = '';
            if ( serverError && typeof serverError === 'object') {
              for ( const key in serverError) {
                if (serverError[key]) {
                  modalStateErrors += serverError[key] + '\n';
                }
              }
            }
            return throwError( modalStateErrors || serverError || 'Server Error');
        }
      })
    );
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
