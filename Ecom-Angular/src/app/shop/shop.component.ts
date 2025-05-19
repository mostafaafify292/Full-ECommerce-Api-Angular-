import { Component, OnInit } from '@angular/core';
import { ShopService } from './shop.service';
import { IPagination } from '../shared/Models/Pagination';
import { IProduct } from '../shared/Models/Product';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  constructor(private shopService: ShopService) {}
  ngOnInit(): void {
    this.getAllProduct();
  }
  product: IProduct[];
  getAllProduct() {
    this.shopService.getProduct().subscribe({
      next: (value: IPagination) => {
        this.product = value.data;
      },
    });
  }
}
