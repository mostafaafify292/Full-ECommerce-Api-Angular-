import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [NavBarComponent],
  imports: [RouterModule, CommonModule, RouterModule, BrowserAnimationsModule],
  exports: [NavBarComponent],
})
export class CoreModule {}
