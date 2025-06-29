import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IdentityService } from '../identity.service';
import { ToastrService } from 'ngx-toastr';
import { ActiveAccount } from '../../shared/Models/ActiveAccount';

@Component({
  selector: 'app-active',
  templateUrl: './active.component.html',
  styleUrl: './active.component.scss'
})
export class ActiveComponent implements OnInit {

  activeParam = new ActiveAccount();
  constructor(private route:ActivatedRoute ,  private _service:IdentityService , private _toast:ToastrService , private router: Router) {}
  ngOnInit(): void {
    console.log(this.activeParam);
    debugger
    this.route.queryParams.subscribe(param=>{
      this.activeParam.email=param['email'];
      this.activeParam.token=param['code']
      console.log(this.activeParam);
      if(this.activeParam.email && this.activeParam.token){
        this.ActiveAccount();
      }
  });

  }

  ActiveAccount() {
    this._service.active(this.activeParam).subscribe({
      next: (value) => {
        console.log(value);
        this._toast.success('Account Activated Successfully', 'Success');
        this.router.navigate(['/account/login']);
      },
      error: (err) => {
        console.log(err);
        this._toast.error(err.error.message, 'Error');
      }
    });
  }
  // this._service.active(this.activeParam).subscribe((res)=>{
  //   debugger
  //   console.log(res);
  //   this._toast.success("Your Account is active now" ,"SUCCESS");
  //     this.router.navigate(['/account/Login']);
  // },
  // (err)=>{
  //   debugger
  //   console.log(err);
  //   this._toast.error("Your account is not active ,token is expire","ERROR")},
  //   ()=>{})

  


}
