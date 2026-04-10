import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { SaleService } from '../../../core/services/sale.service';
import { SaleDetailService } from '../../../core/services/sale-detail.service';
import { ProductService } from '../../../core/services/product.service';
import { CustomerService } from '../../../core/services/customer.service';

@Component({
  selector: 'app-sale-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './detail.html',
  styleUrls: ['./detail.css']
})
export class SaleDetailComponent implements OnInit {

  saleId: number = 0;
  sale: any = null;
  details: any[] = [];
  products: any[] = [];
  customers: any[] = [];

  loading: boolean = true;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private saleService: SaleService,
    private saleDetailService: SaleDetailService,
    private productService: ProductService,
    private customerService: CustomerService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    setTimeout(() => {
      const idParam = this.route.snapshot.paramMap.get('id');

      console.log('ID PARAM:', idParam);

      if (!idParam) {
        this.errorMessage = 'No se recibió el id de la venta.';
        this.loading = false;
        this.cdr.detectChanges();
        return;
      }

      this.saleId = Number(idParam);

      if (isNaN(this.saleId) || this.saleId <= 0) {
        this.errorMessage = 'El id de la venta no es válido.';
        this.loading = false;
        this.cdr.detectChanges();
        return;
      }

      this.loadData();
    }, 0);
  }

  loadData(): void {
    this.loading = true;
    this.errorMessage = '';

    this.productService.getAll().subscribe({
      next: (productsData: any[]) => {
        console.log('PRODUCTOS:', productsData);
        this.products = productsData;

        this.customerService.getAll().subscribe({
          next: (customersData: any[]) => {
            console.log('CLIENTES:', customersData);
            this.customers = customersData;

            this.saleService.getById(this.saleId).subscribe({
              next: (saleData: any) => {
                console.log('VENTA:', saleData);
                this.sale = saleData;

                this.saleDetailService.getBySaleId(this.saleId).subscribe({
                  next: (detailsData: any[]) => {
                    console.log('DETALLES:', detailsData);
                    this.details = detailsData;
                    this.loading = false;
                    this.cdr.detectChanges();
                  },
                  error: (err: any) => {
                    console.error('ERROR CARGANDO DETALLES:', err);
                    this.errorMessage = 'No se pudieron cargar los detalles de la venta.';
                    this.loading = false;
                    this.cdr.detectChanges();
                  }
                });
              },
              error: (err: any) => {
                console.error('ERROR CARGANDO VENTA:', err);
                this.errorMessage = 'No se pudo cargar la venta.';
                this.loading = false;
                this.cdr.detectChanges();
              }
            });
          },
          error: (err: any) => {
            console.error('ERROR CARGANDO CLIENTES:', err);
            this.errorMessage = 'No se pudieron cargar los clientes.';
            this.loading = false;
            this.cdr.detectChanges();
          }
        });
      },
      error: (err: any) => {
        console.error('ERROR CARGANDO PRODUCTOS:', err);
        this.errorMessage = 'No se pudieron cargar los productos.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  getProductName(productId: number): string {
    const product = this.products.find(p => p.id === productId);
    return product ? product.name : 'Producto no encontrado';
  }

  getCustomerName(customerId: number): string {
    const customer = this.customers.find(c => c.id === customerId);
    return customer ? customer.name : 'Cliente no encontrado';
  }
}