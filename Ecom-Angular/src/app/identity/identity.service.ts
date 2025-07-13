import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { ActiveAccount } from '../shared/Models/ActiveAccount';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  constructor(private _httpClient:HttpClient) { }
  baseURL = environment.baseURL;

  register(form:any){
    return this._httpClient.post(this.baseURL+"Account/Register",form)
  }
  active(param:ActiveAccount){
    return this._httpClient.post(this.baseURL+"Account/active-account",param)
  }
  Login(form:any){
    return this._httpClient.post(this.baseURL+ "Account/login" , form , {
      withCredentials: true})
  }
  forgetPassword(email:string){
    return this._httpClient.get(this.baseURL+"Account/send-email-forget-password?email="+email)
  }
  resetPassword(form:any){
    return this._httpClient.post(this.baseURL+"Account/reset-password" , form)
  }
    isAuthenticated(){
      debugger;
    return this._httpClient.get(this.baseURL + 'account/isUserAuth');
  }
}
