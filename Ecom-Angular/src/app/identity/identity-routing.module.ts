import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import path from 'path';
import { RegisterComponent } from './register/register.component';
import { ActiveComponent } from './active/active.component';

const routes: Routes = [
  {path:'Register',component:RegisterComponent},
  {path:'active' ,component:ActiveComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
