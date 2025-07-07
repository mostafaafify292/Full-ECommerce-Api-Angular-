import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { IOrder } from '../shared/Models/Order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  constructor(private http:HttpClient) { }

  BaseURL=environment.baseURL;

  getCurrentOrderForUser(id:number){
    return this.http.get<IOrder>(this.BaseURL+"Orders/get-order-by-id/"+id)
  }
  getAllOrdersForUser(){
    return this.http.get<IOrder[]>(this.BaseURL+"Orders/get-orders-for-user");
  }
}
