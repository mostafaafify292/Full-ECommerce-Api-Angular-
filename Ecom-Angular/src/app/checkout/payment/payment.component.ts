import { Component, Input, OnInit } from '@angular/core';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from '../../basket/basket.service';
import { Basket, IBasket } from '../../shared/Models/Basket';
import { FormGroup } from '@angular/forms';
import { ICreateOrder } from '../../shared/Models/Order';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.scss'
})
export class PaymentComponent implements OnInit {
@Input() DeliveryMethod: FormGroup;
@Input() Address: FormGroup;

ngOnInit(): void {
  
}
  constructor(private _checkoutService: CheckoutService,
              private _toast: ToastrService,
              private _basketService: BasketService,
              private _router:Router
             ) { }

  CreateOrder() {
    const basket = this._basketService.GetCurrentValue();
    if (!basket) {
      this._toast.error('No items in the basket');
      return;
    }
    const order = this.GetOrderCreate(basket);
    this._checkoutService.CreateOrder(order).subscribe({
      next: (response) => {
        this._router.navigate(['/checkout/success'],{queryParams:{orderId:response.id}});
        this._toast.success('Order created successfully',"SUCCESS");
        this._basketService.deleteBasketItem(basket)
      },
      error: (error) => {
        console.error(error);
        this._toast.error('Something went wrong while creating the order', "ERROR");
      }
    });


  }            
  GetOrderCreate(basket: Basket): ICreateOrder {
    return {
      basketId: basket.id,
      deliveryMethodID: this.DeliveryMethod.value.delivery,
      shipAddress: this.Address.value
    }
  }


}
