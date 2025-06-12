import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../basket/basket.service';
import { Observable } from 'rxjs';
import { IBasket } from '../../shared/Models/Basket';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {
  constructor(private basketService: BasketService) {}
  count: Observable<IBasket>;
  ngOnInit(): void {
    const basketId = localStorage.getItem('basketId');
    console.log(basketId);
    this.basketService.GetBasket(basketId).subscribe({
      next: (value) => {
        console.log(value);
        this.count = this.basketService.basket$;
        console.log('count of Basket =>>', this.count);
      },
      error(err) {
        console.log(err);
      },
    });
  }
}
