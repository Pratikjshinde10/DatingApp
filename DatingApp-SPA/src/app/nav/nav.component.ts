import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  constructor(private authservice: AuthService ) { }

  ngOnInit() {
  }

  login() {
    this.authservice.login(this.model).subscribe( next => {
     console.log('logged in successfully');
    }, error => {
      console.log(error);
    });
    console.log(this.model);
  }

  loggedin() {
    const token = localStorage.getItem('token');
    return !!token;
  }
  logout() {
    localStorage.removeItem('token');
    console.log('logged out');
  }

}
