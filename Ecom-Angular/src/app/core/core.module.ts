import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatBadgeModule} from '@angular/material/badge';
@NgModule({
  declarations: [NavBarComponent],
  imports: [RouterModule, CommonModule, RouterModule, BrowserAnimationsModule,MatIconModule,MatButtonModule,MatBadgeModule],
  exports: [NavBarComponent],
})
export class CoreModule {}
