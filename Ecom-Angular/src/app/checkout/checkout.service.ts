import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
baseURL=environment.baseURL;
  constructor(private _http:HttpClient) { }

  updateAddress(form:any){
    return this._http.put(this.baseURL+"account/update-address",form);
  }
}
