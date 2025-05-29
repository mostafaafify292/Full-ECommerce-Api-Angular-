import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ShopService } from './shop.service';
import { IPagination } from '../shared/Models/Pagination';
import { IProduct } from '../shared/Models/Product';
import { ICategory } from '../shared/Models/Category';
import { ProductParams } from '../shared/Models/ProductParams';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  constructor(
    private shopService: ShopService,
    private toast: ToastrService,
  ) {}
  ngOnInit(): void {
    this.getAllProduct();
    this.getCategory();
  }

  //Get Product

  product: IProduct[];
  Category: ICategory[];
  TotalCount: number;
  ProductParams = new ProductParams();

  getAllProduct() {
    this.shopService.getProduct(this.ProductParams).subscribe({
      next: (value: IPagination) => {
        this.product = value.data;
        this.TotalCount = value.totalCount;
        this.ProductParams.pageNumber = value.pageNumber;
        this.ProductParams.pageSize = value.pageSize;
        this.toast.success('Product Loaded Successfully', 'SUCCESS');
      },
    });
  }

  //Pagination
  OnChangePage(event: any) {
    this.ProductParams.pageNumber = event;
    console.log(this.ProductParams.pageNumber);
    console.log(this.ProductParams.pageSize);
    this.getAllProduct();
  }

  //Get Category

  getCategory() {
    this.shopService.getCategory().subscribe((value) => {
      this.Category = value;
      console.log(this.Category);
    });
  }

  SelectedId(categoryId: number) {
    this.ProductParams.CategoryId = categoryId;
    this.getAllProduct();
  }

  //Sorting By Price
  SortingOption = [
    { name: 'Price', value: 'Name' },
    { name: 'Price:min-max', value: 'PriceAsn' },
    { name: 'Price:max-min', value: 'PriceDes' },
  ];

  SotingByPrice(sort: Event) {
    this.ProductParams.SortSelected = (sort.target as HTMLInputElement).value;
    this.getAllProduct();
  }

  //Filtering By Word

  OnSearch(Search: string) {
    this.ProductParams.search = Search;
    this.getAllProduct();
  }
  //Reset All Value
  @ViewChild('search') searchInput!: ElementRef;
  @ViewChild('SortSelected') selected!: ElementRef;
  ResetValue() {
    this.ProductParams.search = '';
    this.ProductParams.SortSelected = this.SortingOption[0].value;
    this.ProductParams.CategoryId = 0;
    this.searchInput.nativeElement.value = '';
    this.selected.nativeElement.selectedIndex = 0;
    this.getAllProduct();
  }
}
