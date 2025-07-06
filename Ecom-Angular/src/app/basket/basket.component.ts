import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket.service';
import { IBasket, IBasketItem } from '../shared/Models/Basket';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss',
})
export class BasketComponent implements OnInit {
  constructor(private _service: BasketService , private _toast : ToastrService , private router:Router) {}

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
    debugger
    this._service.removeItemFromBasket(item);
  }

  incrementQuantity(item: IBasketItem) {
    this._service.incrementBasketItemQuantity(item);
  }

  DecrementQuantity(item: IBasketItem) {
    debugger
    this._service.decrementBasketItemQuantity(item);
  }


  // proceedToCheckout(){
  //   this._service.isAuthenticated().subscribe({   
  //     next: (value) => {
  //     window.location.href = '/checkout';
  //   },
  //   error: (err) => {
  //     console.log(err);
  //      this._toast.error('Please login to proceed to checkout', 'Error');
  // setTimeout(() => {
  //    this.router.navigate(['/account/login']);

  // }, 3000);
  //   },
  //   });
  // }
}
