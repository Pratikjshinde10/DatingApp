import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyServiceService } from '../_services/AlertifyService.service';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() cancelRegister = new EventEmitter();
  // registerForm: FormGroup;

  constructor( private authService: AuthService, private alertify: AlertifyServiceService) { }

  ngOnInit() {
    // this.registerForm = new FormGroup({
    //   username: new FormControl(),
    //   password: new FormControl(),
    //   confirmPasssord: new FormControl()
    // });
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
    // console.log(this.registerForm.value);
  }

  cancel() {
   this.cancelRegister.emit(false);
  }
}
