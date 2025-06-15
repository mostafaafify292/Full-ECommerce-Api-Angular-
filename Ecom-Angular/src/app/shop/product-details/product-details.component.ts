import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { IProduct } from '../../shared/Models/Product';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
})
export class ProductDetailsComponent implements OnInit {
  constructor(
    private shopService: ShopService,
    private route: ActivatedRoute,
    private toast :ToastrService,
    private basketService:BasketService
  ) {}
  quantity:number = 1
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
        },
        error: (err) => {
          console.error('Failed to load product:', err);
        },
      });
  }

  replaceImage(src: string) {
    this.mainImage = src;
  }

  incrementBasket(){
    if (this.quantity<10) {
      this.quantity++;
      this.toast.success("item has been added to the basket", "SUCCESS")
    }else{
      this.toast.warning("you can't add more that 10 items","Enough")
    }
  }
  decrementBasket(){
       if (this.quantity > 1) {
      this.quantity--;
      this.toast.success("item has been Decrement", "SUCCESS")
    }else{
      this.toast.error("you can't Decrement more than 1 items","ERROR")
    }
  }
  addToBasket(){
    debugger
    this.basketService.addItemToBasket(this.product,this.quantity)
  }
}
