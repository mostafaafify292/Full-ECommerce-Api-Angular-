import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrl: './success.component.scss'
})
export class SuccessComponent implements OnInit {
  constructor(private route:ActivatedRoute){}

ngOnInit(): void {
  this.route.queryParams.subscribe(params => {
      this.orderId = params['orderId'];
    });
}
orderId:number=0;
}
