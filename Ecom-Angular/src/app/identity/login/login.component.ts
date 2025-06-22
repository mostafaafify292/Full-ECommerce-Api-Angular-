import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityService } from '../identity.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

formGroup:FormGroup

constructor(private fb:FormBuilder , private _service:IdentityService ,private router: Router){}

  ngOnInit(): void {
   this.formValidation()
  }

formValidation(){
  this.formGroup = this.fb.group({
    email:['',[Validators.required , Validators.email]],
    password:['',[Validators.required , Validators.pattern(/^(?=.*[0-9])(?=.*[#$@!.\-])[A-Za-z\d#$@!.\-]{8,}$/)]]
  })
}

get _email() {
  return this.formGroup.get('email');
}
get _password() {
  return this.formGroup.get('password');
}
Submit(){
  
  console.log(this.formGroup.value)
  if (this.formGroup.valid) {
    this._service.Login(this.formGroup.value).subscribe({
      next:(value)=>{
        console.log(value);
        this.router.navigate(['/shop']);
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }
}

}

