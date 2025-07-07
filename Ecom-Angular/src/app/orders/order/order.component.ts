import { Component, OnInit } from '@angular/core';
import { IOrder, IOrderItem } from '../../shared/Models/Order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent implements OnInit {
  orders:IOrder[]=[];

constructor(private orderService:OrdersService) { }

  ngOnInit(): void {
    this.orderService.getAllOrdersForUser().subscribe({
      next:(response)=>{
        this.orders=response;
        console.log(response);
      },
      error:(error)=>{
        console.log(error);
      }
    });
  }

getFirstImageOrderItem(order:IOrderItem[]){
  return order.length > 0 ? order[0].mainImage : null;
}
}
