import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityService } from '../identity.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  formGroup: FormGroup
  constructor(private fb: FormBuilder, private _service: IdentityService, private _toast: ToastrService, private route: Router) { }

  ngOnInit(): void {
    this.formValidation();  
  }

  formValidation(){
    this.formGroup = this.fb.group({
      userName : ['',[Validators.required,Validators.minLength(6)]],
      email :['',[Validators.required,Validators.email]],
      displayName:['',[Validators.required]],
      password:['',[Validators.required,Validators.pattern(/^(?=.*[0-9])(?=.*[#$@!.\-])[A-Za-z\d#$@!.\-]{8,}$/)]]
    })
  }
  get _username() {
    return this.formGroup.get('userName');
  }
  get _email() {
    return this.formGroup.get('email');
  }
  get _displayName() {
    return this.formGroup.get('displayName');
  }
  get _password() {
    return this.formGroup.get('password');
  }
  Submit(){
    if(this.formGroup.valid){
      this._service.register(this.formGroup.value).subscribe({
        next:(value)=>{
          console.log(value);
          this._toast.success("Register success , Please confirm your email ",'success'.toUpperCase())
          this.route.navigate(['/account/login']);
        },
        error:(err:any)=>{
          console.log(err);
          this._toast.error(err.error.message ,'error'.toUpperCase())
        }
      })
    }
  }

}
