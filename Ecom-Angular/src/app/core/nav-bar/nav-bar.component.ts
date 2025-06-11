import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {
  
  constructor(private basketService:BasketService){}
  ngOnInit(): void {
    const basketId = localStorage.getItem('basketId')
        console.log(basketId)
    this.basketService.GetBasket(basketId).subscribe({
      next(value){
            console.log(value)
      },
      error(err){
            console.log(err)
      }
    });

  }
}
