import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PaginationComponent } from './Component/pagination/pagination.component';
import { RouterModule } from '@angular/router';
import { OrderTotalComponent } from './Component/pagination/order-total/order-total.component';

@NgModule({
  declarations: [PaginationComponent, OrderTotalComponent],
  imports: [CommonModule, PaginationModule.forRoot(), RouterModule],
  exports: [PaginationModule, PaginationComponent, OrderTotalComponent],
})
export class SharedModule {}
