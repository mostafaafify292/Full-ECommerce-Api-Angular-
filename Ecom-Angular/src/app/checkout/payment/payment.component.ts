import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { loadStripe, Stripe, StripeCardElement } from '@stripe/stripe-js';

import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from '../../basket/basket.service';
import { Basket } from '../../shared/Models/Basket';
import { ICreateOrder } from '../../shared/Models/Order';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {
  @Input() DeliveryMethod: FormGroup;
  @Input() Address: FormGroup;
  @ViewChild('cardElement', { static: true }) cardElement: ElementRef;

  stripe: Stripe;
  card: StripeCardElement;
  cardErrors: string;

  constructor(
    private _checkoutService: CheckoutService,
    private _toast: ToastrService,
    private _basketService: BasketService,
    private _router: Router
  ) {}

  async ngOnInit() {
    // 1. تحميل Stripe
    this.stripe = await loadStripe('pk_test_51QHeJeG27fZ2e9pHAjYyfi00WgefDKs3EDcUDoWdaLVNaD2IsOFRCfGNSQDmrauEJbK0QJyLe5posdDz8B74LtDg00qz9Yk4wX');
    
    if (!this.stripe) {
    
      return;
    }

    // 2. تجهيز العناصر و الكارد
    const elements = this.stripe.elements();
    this.card = elements.create('card', {
      style: {
        base: {
          color: '#32325d',
          fontSize: '16px',
          '::placeholder': {
            color: '#aab7c4'
          }
        },
        invalid: {
          color: '#fa755a',
          iconColor: '#fa755a'
        }
      }
    });

    // 3. تركيب العنصر داخل العنصر من الـ HTML
    this.card.mount(this.cardElement.nativeElement);

    // 4. الاستماع للأخطاء
    this.card.on('change', (event) => {
      this.cardErrors = event.error?.message;
    });
  }

  async CreateOrder() {
    debugger;
    const basket = this._basketService.GetCurrentValue();
    if (!basket) {
      this._toast.error('No items in the basket');
      return;
    }
    console.log('Basket before payment:', basket);
    // 5. تنفيذ عملية الدفع
    const result = await this.stripe.confirmCardPayment(basket.clientSecret, {
      payment_method: {
        card: this.card,
        billing_details: {
          name: `${this.Address.value?.firstName} ${this.Address.value?.lastName}`
        }
      }
    });

    if (result.error) {
      this._toast.error(result.error.message, 'Payment Error');
    } else {
      if (result.paymentIntent.status === 'succeeded') {
        const order = this.GetOrderCreate(basket);
        this._checkoutService.CreateOrder(order).subscribe({
          next: (response) => {
            this._router.navigate(['/checkout/success'], { queryParams: { orderId: response.id } });
            this._toast.success('Order created successfully', 'SUCCESS');
            this._basketService.deleteBasketItem(basket);
          },
          error: (error) => {
            this._toast.error('Something went wrong while creating the order', 'ERROR');
          }
        });
      }
    }
  }

  GetOrderCreate(basket: Basket): ICreateOrder {
    return {
      basketId: basket.id,
      deliveryMethodID: this.DeliveryMethod.value.delivery,
      shipAddress: this.Address.value
    };
  }
}
