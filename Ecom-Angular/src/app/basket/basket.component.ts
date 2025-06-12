import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket.service';
import { IBasket, IBasketItem } from '../shared/Models/Basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss',
})
export class BasketComponent implements OnInit {
  constructor(private _service: BasketService) {}

  basket: IBasket;
  ngOnInit(): void {
    this._service.basket$.subscribe({
      next: (value) => {
        this.basket = value;
        console.log('basket details ', this.basket, 'value', value);
      },
      error(err) {
        console.log(err);
      },
    });
  }

  RemoveBasket(item: IBasketItem) {
    this._service.removeItemFromBasket(item);
  }
  incrementQuantity(item: IBasketItem) {
    this._service.incrementBasketItemQuantity(item);
  }
  DecrementQuantity(item: IBasketItem) {
    this._service.decrementBasketItemQuantity(item);
  }
}
