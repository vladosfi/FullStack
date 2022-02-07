import { Component, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { City } from './city';
import { environment } from 'src/environments/environment';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

@Component({
selector: 'app-cities',
templateUrl: './cities.component.html',
styleUrls: ['./cities.component.css']
})
export class CitiesComponent {
    public cities: City[] = [];
    public displayedColumns: string[] = ['id', 'name', 'lat', 'lon'];

    constructor(private http: HttpClient) {}
    baseUrl = environment.apiUrl;
    
    ngOnInit() {        
        this.http.get<City[]>(this.baseUrl + 'Cities')
        .subscribe({
            next: (result) => this.cities = result,
            error: (error) => console.error(error)
        });
    }
}