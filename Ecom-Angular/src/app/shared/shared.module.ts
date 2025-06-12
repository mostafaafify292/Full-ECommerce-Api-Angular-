import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PaginationComponent } from './Component/pagination/pagination.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [PaginationComponent],
  imports: [CommonModule, PaginationModule.forRoot(), RouterModule],
  exports: [PaginationModule, PaginationComponent],
})
export class SharedModule {}
