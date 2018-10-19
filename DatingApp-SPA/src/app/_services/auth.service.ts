import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseURL = environment.apiUrl + 'auth/';
jwthelper = new JwtHelperService();
decodedToken: any;

constructor( private http: HttpClient) {}

login(model: any) {
 return this.http.post(this.baseURL + 'login', model).pipe(
   map(( response: any) => {
     const user = response;
     if (user) {
        localStorage.setItem('token', user.token);
        console.log(this.decodedToken);
     }
   })
 );
}

register(model: any) {
 return this.http.post(this.baseURL + 'register', model);
  }

  loggedin() {
   const token = localStorage.getItem('token');
   return !this.jwthelper.isTokenExpired(token);
  }
}


