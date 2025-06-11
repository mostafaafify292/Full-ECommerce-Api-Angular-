import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Basket, IBasket, IBasketItem } from '../shared/Models/Basket';
import { IProduct } from '../shared/Models/Product';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  constructor(private http: HttpClient) {}
  baseURL = 'https://localhost:44330/api/';
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket = this.basketSource.asObservable();

  GetBasket(id: string) {
    return this.http.get(this.baseURL + 'Basket/get-basket-item/' + id).pipe(
      map((value: IBasket) => {
        this.basketSource.next(value);
        console.log(value)
      }),
    );
  }

  
  SetBasket(basket: IBasket) {
  console.log('Basket to send:', basket);
  return this.http
    .post(this.baseURL + 'Basket/update-basket/', basket)
    .subscribe({
      next: (value: any) => {
        // Normalize property names to PascalCase
        const normalizedBasket: IBasket = {
          Id: value.Id ?? value.id,
          basketItems: (value.basketItems ?? value.basketitems ?? []).map((item: any) => ({
            Id: item.Id ?? item.id,
            Name: item.Name ?? item.name,
            Quantity: item.Quantity ?? item.quantity,
            ImageURL: item.ImageURL ?? item.imageURL,
            Price: item.Price ?? item.price,
            Category: item.Category ?? item.category,
          })),
        };

        this.basketSource.next(normalizedBasket);
        console.log('Normalized Basket:', normalizedBasket);
      },
      error(err) {
        console.log('error***' + err);
      },
    });
}

  // SetBasket(basket: IBasket) {
  //   console.log('Basket to send:', basket);
  //   return this.http
  //     .post(this.baseURL + 'Basket/update-basket/', basket)
  //     .subscribe({
  //       next: (value: IBasket) => {
  //         this.basketSource.next(value);
  //         console.log(value)
  //       },
  //       error(err) {
  //         console.log('error***'+ err);
  //       },
  //     });
  // }

  GetCurrentValue() {
    console.log('GetCurrentValue'+this.basketSource)
    return this.basketSource.value;  
  }

  addItemToBasket(product : IProduct ,quantity:number =1){
    const itemToAdd = this.MapProductToBasketItem(product,quantity)
    console.log('product item to add'+product.id)
    const basket = this.GetCurrentValue()??this.CreateBasket()
    console.log(basket);
    basket.basketItems = this.AddOrUpdate(basket.basketItems ,itemToAdd,quantity)
    return this.SetBasket(basket);
  }

  private AddOrUpdate(basketItems: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = basketItems.findIndex(i => i.Id === itemToAdd.Id);
    console.log('Compare IDs:', basketItems[0], 'vs', itemToAdd);
    console.log('index => '+index);
    if (index == -1) {
      itemToAdd.Quantity =quantity;
      basketItems.push(itemToAdd);
    }else{
      basketItems[index].Quantity+=quantity;
    }
    return basketItems
  }

  private CreateBasket(): IBasket {
    const basket = new Basket()
    localStorage.setItem('basketId',basket.Id)
    console.log('CreatedBasket=>id = '+basket.Id)
    return basket;
  }

  private MapProductToBasketItem(product: IProduct, quantity: number) :IBasketItem{
    return{
      Id : product.id?.toString(),
      Category :product.categoryName,
      Name :product.name,
      ImageURL : product.photos[0]?.imageName,
      Price :product.newPrice,
      Quantity : quantity

    }
  }
}
