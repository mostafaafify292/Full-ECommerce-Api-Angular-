import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { IBasket } from '../shared/Models/Basket';

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
      }),
    );
  }

  SetBasket(basket: IBasket) {
    return this.http
      .post(this.baseURL + 'Basket/update-basket/', basket)
      .subscribe({
        next: (value: IBasket) => {
          this.basketSource.next(value);
        },
        error(err) {
          console.log(err);
        },
      });
  }

  GetCurrentValue() {
    return this.basketSource.value;
  }
}
