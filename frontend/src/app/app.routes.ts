import { Routes } from '@angular/router';
import { LoginComponent } from './modules/auth/login/login.component';
import { ListComponent } from './modules/products/list/list';
import { FormComponent } from './modules/products/form/form';
import { NewSaleComponent } from './modules/sales/new-sale/new-sale';
import { List as SalesList } from './modules/sales/list/list';
import { SaleDetailComponent } from './modules/sales/detail/detail';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'products', component: ListComponent },
  { path: 'products/create', component: FormComponent },
  { path: 'products/edit/:id', component: FormComponent },
  { path: 'sales/create', component: NewSaleComponent },
  { path: 'sales/list', component: SalesList },
  { path: 'sales/detail/:id', component: SaleDetailComponent }
];