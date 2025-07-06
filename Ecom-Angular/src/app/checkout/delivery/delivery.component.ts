import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CheckoutService } from '../checkout.service';
import { Delivery } from '../../shared/Models/Delivery';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrl: './delivery.component.scss'
})

export class DeliveryComponent implements OnInit {
  @Input()delivery:FormGroup
  deliveries:Delivery[] = [];

constructor(private _service:CheckoutService , private basketService:BasketService) { }

setShippingPrice(){
  
  const deliveryMethod = this.deliveries.find(d => d.id === this.delivery.value.delivery);
  this.basketService.SetShippingPrice(deliveryMethod);
  console.log('Selected delivery method:', deliveryMethod);
}


ngOnInit(): void {
  this._service.getDeliveryMethods().subscribe({
    next:(value)=>{
      this.deliveries = value;
      console.log(this.deliveries);
    },
    error:(err)=>{
      console.error('Error fetching delivery methods:', err);
    }
  });

}



}
