import { Component, Input, input } from '@angular/core';
import { IProduct } from '../../shared/Models/Product';

@Component({
  selector: 'app-shop-item',
  templateUrl: './shop-item.component.html',
  styleUrl: './shop-item.component.scss',
})
export class ShopItemComponent {
  @Input() Product: IProduct;
}
