import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { IProduct } from '../../shared/Models/Product';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
})
export class ProductDetailsComponent implements OnInit {
  constructor(
    private shopService: ShopService,
    private route: ActivatedRoute,
  ) {}

  product: IProduct;
  mainImage: string;

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    this.shopService
      .getProductDetails(parseInt(this.route.snapshot.paramMap.get('id')))
      .subscribe({
        next: (value: any) => {
          this.product = value;
          this.mainImage = this.product.photos[0].imageName;
          console.log(this.product);
        },
        error: (err) => {
          console.error('Failed to load product:', err);
        },
      });
  }

  replaceImage(src: string) {
    this.mainImage = src;
  }
}
