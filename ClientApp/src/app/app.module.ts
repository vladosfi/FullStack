import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CitiesComponent } from './cities/cities.component';

import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from'@angular/common/http';
import { RouterModule } from '@angular/router';


import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './angular-material.module';

@NgModule({
  declarations: [
    AppComponent,
    CitiesComponent,
    NavMenuComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'cities', component: CitiesComponent }
    ]),
    BrowserAnimationsModule,
    AngularMaterialModule,
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
