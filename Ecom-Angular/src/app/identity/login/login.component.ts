import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityService } from '../identity.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

formGroup:FormGroup
 returnUrl: string; 
modalRef?: BsModalRef;
forgetForm: FormGroup;

constructor(private fb: FormBuilder,
   private _service: IdentityService,
   private router: Router,
    private route: ActivatedRoute,
   private modalService: BsModalService,
  private toastr: ToastrService) {

    this.forgetForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });
}

  ngOnInit(): void {
   this.formValidation()

   this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/shop';
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
        this.router.navigateByUrl(this.returnUrl);
      },
      error:(err)=>{
        console.log(err);
        this.toastr.error(err.error.message, 'Error');

      }
    });
  }
}

openModal(template: TemplateRef<any>) {
  this.modalRef = this.modalService.show(template);
}

onSendResetLink() {
  const email = this.forgetForm.value.email;

  if (this.forgetForm.valid) {
    this._service.forgetPassword(email).subscribe({
      next:(value)=>{
        console.log(value);
        this.toastr.success("Email sent successfully", 'Success');
      },
      error:(err)=>{
        console.log(err);
        this.toastr.error("An Error occurred while sending email", 'Error');
      }
    });
  }
  console.log("Reset link sent to:", email);
  this.modalRef?.hide();
}




}

