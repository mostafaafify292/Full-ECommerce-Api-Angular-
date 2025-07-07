import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderComponent } from './order/order.component';
import { OrderItemComponent } from './order-item/order-item.component';

const routes: Routes = [
  {path:'',component:OrderComponent},
  {path:'item',component:OrderItemComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
