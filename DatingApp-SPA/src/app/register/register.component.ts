import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyServiceService } from '../_services/AlertifyService.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Input() valuesfromHome: any;
  @Output() cancelRegister = new EventEmitter();

  constructor( private authService: AuthService, private alertify: AlertifyServiceService) { }

  ngOnInit() {
  }

  register() {
   this.authService.register(this.model).subscribe(() => {
    console.log('Registration Successful');
    this.alertify.success('Registration Successful');
   }, error => {
     console.log(error);
     this.alertify.error(error);
   }
   );
  }

  cancel() {
   this.cancelRegister.emit(false);
  }
}
