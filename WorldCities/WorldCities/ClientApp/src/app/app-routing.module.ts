import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CitiesComponent } from './cities/cities.component';
import { CountriesComponent } from './countries/countries.component';
import { CityEditComponent } from './cities/city-edit.component';
import { CountryEditComponent } from './countries/country-edit.component';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: 'cities',
    component: CitiesComponent
  },
  {
    path: 'city/:id',
    component: CityEditComponent,
    canActivate: [AuthorizeGuard]
  },
  {
    path: 'city',
    component: CityEditComponent,
    canActivate: [AuthorizeGuard]
  },
  {
    path: 'countries',
    component: CountriesComponent
  },
  {
    path: 'country/:id',
    component: CountryEditComponent,
    canActivate: [AuthorizeGuard]
  },
  {
    path: 'country',
    component: CountryEditComponent,
    canActivate: [AuthorizeGuard]
  }
];
@NgModule({
  imports: [RouterModule.forRoot(routes, {
    initialNavigation: 'enabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
