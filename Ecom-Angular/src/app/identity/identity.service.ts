import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { IActiveAccount } from '../shared/Models/ActiveAccount';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  constructor(private _httpClient:HttpClient) { }
  baseURL = environment.baseURL;

  register(form:any){
    return this._httpClient.post(this.baseURL+"Account/Register",form)
  }
  active(param:IActiveAccount){
    return this._httpClient.post(this.baseURL+"Account/active-account",param)
  }
}
