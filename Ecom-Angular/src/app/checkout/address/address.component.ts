import { Component, Input, input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrl: './address.component.scss'
})
export class AddressComponent {
@Input()address : FormGroup
constructor(private _service : CheckoutService){}
UpdateAddress() {
  if (this.address.valid) {

    console.log('Address form is valid:', this.address.value);
    this._service.updateAddress(this.address.value).subscribe({
      next: (response) => {
        console.log('Address updated successfully:', response);
      },
      error: (error) => {
        console.error('Error updating address:', error);
      }
    });
  } 
}

}
