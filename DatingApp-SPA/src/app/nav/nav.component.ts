import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyServiceService } from '../_services/AlertifyService.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  constructor(public authservice: AuthService, private alertify: AlertifyServiceService  ) { }

  ngOnInit() {
  }

  login() {
    this.authservice.login(this.model).subscribe( next => {
     console.log('logged in successfully');
     this.alertify.success('Logged in successfully');
    }, error => {
      console.log(error);
      this.alertify.error(error);
    });
    console.log(this.model);
  }

  loggedin() {
    return this.authservice.loggedin();
  }
  logout() {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
    console.log('logged out');
  }

}

