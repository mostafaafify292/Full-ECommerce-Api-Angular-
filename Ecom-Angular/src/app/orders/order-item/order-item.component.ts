import { Component, OnInit } from '@angular/core';
import { IOrder } from '../../shared/Models/Order';
import { ActivatedRoute } from '@angular/router';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-item',
  templateUrl: './order-item.component.html',
  styleUrl: './order-item.component.scss'
})
export class OrderItemComponent implements OnInit {
  order:IOrder
  id:number=0;

constructor(private route:ActivatedRoute , private _orderService:OrdersService){}

ngOnInit(): void {
  this.route.queryParams.subscribe(params=>
  {
    this.id= params['id'];
  })
this._orderService.getCurrentOrderForUser(this.id).subscribe({
  next:response=>{
    this.order=response
    console.log(this.order);
  },
  error:err=>{
    console.log(err)
  }
})

}


}
