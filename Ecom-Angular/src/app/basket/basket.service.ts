import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/Models/Basket';
import { IProduct } from '../shared/Models/Product';
import { v4 as uuidv4 } from 'uuid';
import { Delivery } from '../shared/Models/Delivery';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  constructor(private http: HttpClient) {}
  baseURL = 'https://localhost:44330/api/';
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketSourceTotal = new BehaviorSubject<IBasketTotal>(null);
  basketTotal$ =this.basketSourceTotal.asObservable();
  shipPrice:number=0;
  
  SetShippingPrice(delivery:Delivery) {
    this.shipPrice = delivery.price;
    this.calulateTotal();
  }
  calulateTotal(){
    const basket = this.GetCurrentValue();
    const shipping =this.shipPrice;
    const subTotal =basket.basketItems.reduce((a,c)=>{
      return(c.price*c.quantity)+a
    },0)
    const total = shipping+subTotal;
    this.basketSourceTotal.next({shipping,subTotal,total})
  }

  GetBasket(id: string) {
    return this.http.get(this.baseURL + 'Basket/get-basket-item/' + id).pipe(
      map((value: IBasket) => {
        this.basketSource.next(value);
        this.calulateTotal();
        return value;
      }),
    );
  }

  SetBasket(basket: IBasket) {
    return this.http
      .post(this.baseURL + 'Basket/update-basket/', basket)
      .subscribe({
        next: (value: IBasket) => {
          this.basketSource.next(value);
          this.calulateTotal();
        },
        error(err) {
          console.log('error***', err);
        },
      });
  }

  GetCurrentValue() {
    return this.basketSource.value;
  }

  addItemToBasket(product: IProduct, quantity: number = 1) {
    debugger;
    const itemToAdd = this.MapProductToBasketItem(product, quantity);
    let basket = this.GetCurrentValue();
    if (!basket || basket?.id == 'null' ) {
      basket = this.CreateBasket();
    }

    basket.basketItems = this.AddOrUpdate(
      basket.basketItems,
      itemToAdd,
      quantity,
    );
    return this.SetBasket(basket);
  }

  private AddOrUpdate(
    basketItems: IBasketItem[],
    itemToAdd: IBasketItem,
    quantity: number,
  ): IBasketItem[] {
    const index = basketItems.findIndex((i) => i.id === itemToAdd.id);
    if (index == -1) {
      itemToAdd.quantity = quantity;
      basketItems.push(itemToAdd);
    } else {
      basketItems[index].quantity += quantity;
    }
    return basketItems;
  }

  private CreateBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basketId', basket.id);
    return basket;
  }

  private MapProductToBasketItem(
    product: IProduct,
    quantity: number,
  ): IBasketItem {
    return {
      id: product.id,
      category: product.categoryName,
      name: product.name,
      imageURL: product.photos[0]?.imageName,
      price: product.newPrice,
      quantity: quantity,
      description: product.description,
    };
  }
  incrementBasketItemQuantity(item: IBasketItem) {
    const basket = this.GetCurrentValue();
    const itemIndex = basket.basketItems.findIndex((i) => i.id === item.id);
    basket.basketItems[itemIndex].quantity++;
    this.SetBasket(basket);
  }

  decrementBasketItemQuantity(item: IBasketItem) {
    const basket = this.GetCurrentValue();
    const itemIndex = basket.basketItems.findIndex((i) => i.id === item.id);
    if (basket.basketItems[itemIndex].quantity > 1) {
      basket.basketItems[itemIndex].quantity--;

      this.SetBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.GetCurrentValue();
    if (basket.basketItems.some((i) => i.id === item.id)) {
      basket.basketItems = basket.basketItems.filter((i) => i.id !== item.id);
      if (basket.basketItems.length > 0) {
        this.SetBasket(basket);
      } else {
        this.deleteBasketItem(basket);
      }
    }
  }
  deleteBasketItem(basket: IBasket) {
    return this.http
      .delete(this.baseURL + 'Basket/delete-basket/' + basket.id)
      .subscribe({
        next: (value) => {
            const emptyBasket: IBasket = { id: basket.id, basketItems: [] , paymentIntentId: "", clientSecret: "" };
            this.basketSource.next(emptyBasket);
          localStorage.removeItem('basketId');
        },
        error(err) {
          console.log(err);
        },
      });
  }

  CreatePaymentIntent(deliveryMethodId: number= 2) {
    debugger;
    const basket = this.GetCurrentValue();
    return this.http.post(`${this.baseURL}Payments/Create?basketId=${basket.id}&deliveryId=${deliveryMethodId}`, {}).pipe(
      map((value:IBasket)=>{
        this.basketSource.next(value);
      })
    );

  }


  isAuthenticated(){
    return this.http.get(this.baseURL + 'account/isUserAuth');
  }
}
