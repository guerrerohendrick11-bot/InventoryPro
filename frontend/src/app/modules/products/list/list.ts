import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../core/services/product.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './list.html',
  styleUrls: ['./list.css']
})
export class ListComponent implements OnInit {

  products: any[] = [];

  searchName: string = '';
  searchCategoryId: number | null = null;

  constructor(
    private productService: ProductService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.productService.getAll(this.searchName, this.searchCategoryId).subscribe({
      next: (data: any[]) => {
        console.log('PRODUCTOS:', data);
        this.products = data;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('ERROR AL CARGAR PRODUCTOS:', err);
      }
    });
  }

  searchProducts(): void {
    this.getProducts();
  }

  clearFilters(): void {
    this.searchName = '';
    this.searchCategoryId = null;
    this.getProducts();
  }
}