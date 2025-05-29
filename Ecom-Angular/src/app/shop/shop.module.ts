import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ShopItemComponent } from './shop-item/shop-item.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { RouterModule } from '@angular/router';
import { NgxImageZoomModule } from 'ngx-image-zoom';
import { ShopRoutingModule } from './shop-routing.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { loaderInterceptor } from '../core/interceptor/loader.interceptor';

@NgModule({
  declarations: [ShopComponent, ShopItemComponent, ProductDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    NgxImageZoomModule,
    ShopRoutingModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: loaderInterceptor,
      multi: true,
    },
  ],
  exports: [],
})
export class ShopModule {}
