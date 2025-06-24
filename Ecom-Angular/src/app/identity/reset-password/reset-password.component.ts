import { Component, OnInit } from '@angular/core';
import { ResetPassword } from '../../shared/Models/ResetPassword';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityService } from '../identity.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})

export class ResetPasswordComponent implements OnInit {
  formGroup: FormGroup;
  ResetValue = new ResetPassword();
  constructor(private router: ActivatedRoute,
     private fb: FormBuilder,
     private _service: IdentityService,
    private route:Router,
    private toast: ToastrService
    ) {}
  ngOnInit(): void {
    this.router.queryParams.subscribe((param) => {
      this.ResetValue.email = param['email'];
      this.ResetValue.token = param['code'];
    });
    this.FormValidation()
  }

  FormValidation() {
    this.formGroup = this.fb.group({
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^(?=.*[0-9])(?=.*[#$@!.\-])[A-Za-z\d#$@!.\-]{8,}$/
          ),
        ],
      ],
      confirmPassword: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^(?=.*[0-9])(?=.*[#$@!.\-])[A-Za-z\d#$@!.\-]{8,}$/
          ),
        ],
      ],
    
    },
    {validator:this.PasswordMatchValidation}
  );
  }

  PasswordMatchValidation(form: FormGroup) {
    const passwordControl = form.get('password'); 
    const confirmPasswordControl = form.get('confirmPassword');
    if (passwordControl?.value === confirmPasswordControl?.value) {
      confirmPasswordControl?.setErrors(null);
    } else {
      confirmPasswordControl?.setErrors({ passwordMisMatch: true });
    }
  }

  get _password() {
    return this.formGroup.get('password');
  }
  get _confirmPassword() {
    return this.formGroup.get('confirmPassword');
  }

  OnSubmit(){
    if(this.formGroup.valid){
      this.ResetValue.password=this.formGroup.value.password
      this._service.resetPassword(this.ResetValue).subscribe({
        next:(value)=>{
          this.toast.success(value['message'], 'Success');
          this.route.navigateByUrl('/account/Login');
        },error:(err)=> {
          this.toast.error(err?.error?.message, 'Error');
          this.route.navigateByUrl('/account/Login')
        },
      })
    }
  }
}
