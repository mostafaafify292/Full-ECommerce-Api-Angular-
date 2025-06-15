import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../../../basket/basket.service';
import { IBasketTotal } from '../../../Models/Basket';

@Component({
  selector: 'app-order-total',
  templateUrl: './order-total.component.html',
  styleUrl: './order-total.component.scss'
})
export class OrderTotalComponent implements OnInit {
  constructor(private basketService:BasketService){}
  basketTotal:IBasketTotal
  ngOnInit(): void {
    this.basketService.basketTotal$.subscribe({
      next:(value)=>{
        this.basketTotal=value
      },
      error(err){
        console.log(err)
      }
    })
  }

}
