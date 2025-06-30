import { Component, Input, input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrl: './address.component.scss'
})
export class AddressComponent {
@Input()address : FormGroup

UpdateAddress() {
  if (this.address.valid) {
    console.log('Address is valid:', this.address.value);
  } else {
    console.log('Address is invalid:', this.address.errors);
  }
}

}
