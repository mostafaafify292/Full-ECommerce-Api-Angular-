import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/Models/Pagination';
import { ICategory } from '../shared/Models/Category';
import { ProductParams } from '../shared/Models/ProductParams';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseURL = 'https://localhost:44330/api/';
  constructor(private http: HttpClient) {}

  //Get Product
  getProduct(productParms: ProductParams) {
    let param = new HttpParams();
    if (productParms.CategoryId) {
      param = param.append('categoryId', productParms.CategoryId);
    }
    if (productParms.SortSelected) {
      param = param.append('sort', productParms.SortSelected);
    }
    if (productParms.search) {
      param = param.append('Search', productParms.search);
    }
    param = param.append('PageSize', productParms.pageSize);
    param = param.append('pageNumber', productParms.pageNumber);
    return this.http.get<IPagination>(this.baseURL + 'Products/get-all', {
      params: param,
    });
  }

  //Get Category
  getCategory() {
    return this.http.get<ICategory[]>(this.baseURL + 'Categories/get-all');
  }
}
