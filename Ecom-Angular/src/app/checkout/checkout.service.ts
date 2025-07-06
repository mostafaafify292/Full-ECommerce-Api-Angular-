import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Delivery } from '../shared/Models/Delivery';
import { ICreateOrder } from '../shared/Models/Order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
baseURL=environment.baseURL;
  constructor(private _http:HttpClient) { }

  updateAddress(form:any){
    return this._http.put(this.baseURL+"account/update-address",form);
  }
  getAddress(){
    return this._http.get(this.baseURL+"account/get-address-for-user");
  }
  getDeliveryMethods() {
    return this._http.get<Delivery[]>(this.baseURL + "Orders/get-delivery");
  }
  CreateOrder(order:ICreateOrder) {
    return this._http.post(this.baseURL + "Orders/create-order", order);
  }
}
