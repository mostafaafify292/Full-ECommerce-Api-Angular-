import { AfterViewInit, Component } from '@angular/core';
import { IActiveAccount } from '../../shared/Models/ActiveAccount';
import { ActivatedRoute } from '@angular/router';
import { IdentityService } from '../identity.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-active',
  templateUrl: './active.component.html',
  styleUrl: './active.component.scss'
})
export class ActiveComponent implements AfterViewInit {

  activeParam:IActiveAccount={
    email:'',
    token:''
  }
  constructor(private route:ActivatedRoute ,  private _service:IdentityService , private _toast:ToastrService){}
  ngAfterViewInit(): void {
    this.route.queryParams.subscribe(param=>{
      this.activeParam.email=param['email'];
      this.activeParam.token=param['code']
  });
  this._service.active(this.activeParam).subscribe({
    next:(value)=>{
      console.log(value);
      this._toast.success("Your Account is active now" ,"SUCCESS")
      
    },error:(err)=>{
      console.log(err);
      this._toast.error("Your account is not active ,token is expire","ERROR")
    }
  })
  }



}
