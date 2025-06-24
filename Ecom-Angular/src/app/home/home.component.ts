import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {



  constructor(private router: Router) {}
  features = [
    {
      icon: 'https://cdn-icons-png.flaticon.com/512/3523/3523881.png',
      title: 'Fast Delivery',
      text: 'Get your orders in no time with our express shipping.'
    },
    {
      icon: 'https://cdn-icons-png.flaticon.com/512/1041/1041916.png',
      title: 'Secure Payment',
      text: 'Your payment information is processed securely.'
    },
    {
      icon: 'https://cdn-icons-png.flaticon.com/512/1828/1828911.png',
      title: 'Customer Support',
      text: 'We’re here for you 24/7 to help with anything you need.'
    }
  ];
  
  products = [
    {
      id:1,
      img: 'https://m.media-amazon.com/images/I/81obPsMhewL.__AC_SX300_SY300_QL70_ML2_.jpg',
      name: 'Laptop',
      desc: 'Powerful laptop with sleek design.'
    },
    {
      id:4,
      img: 'https://m.media-amazon.com/images/I/71+WncNIt7L._AC_SX679_.jpg',
      name: 'Mobile Phone',
      desc: 'Latest smartphone with cutting-edge features.'
    },
    {
      id:6,
      img: 'https://m.media-amazon.com/images/I/51J5NMoqK7L.__AC_SX300_SY300_QL70_ML2_.jpg',
      name: 'Smartwatch',
      desc: 'Your perfect fitness companion.'
    },
    {
      id:5,
      img: 'https://m.media-amazon.com/images/I/517HiBnlavL.__AC_SX300_SY300_QL70_ML2_.jpg',
      name: 'AirPods',
      desc: 'Experience high-quality sound with these wireless earbuds.'
    }
  ];

  navigateToCategory(categoryId: number) {
    debugger;
    this.router.navigate(['/shop'], {
      queryParams: { category: categoryId }
    });
}
}