import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { CustomerService } from '../../../core/services/customer.service';
import { ProductService } from '../../../core/services/product.service';
import { SaleService } from '../../../core/services/sale.service';
import { SaleDetailService } from '../../../core/services/sale-detail.service';

@Component({
  selector: 'app-new-sale',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './new-sale.html',
  styleUrls: ['./new-sale.css']
})
export class NewSaleComponent implements OnInit {

  customers: any[] = [];
  products: any[] = [];

  selectedCustomerId: number | null = null;
  selectedProductId: number | null = null;

  saleDetails: any[] = [];
  total: number = 0;

  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private customerService: CustomerService,
    private productService: ProductService,
    private saleService: SaleService,
    private saleDetailService: SaleDetailService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCustomers();
    this.loadProducts();
  }

  loadCustomers(): void {
    this.customerService.getAll().subscribe({
      next: (data) => {
        this.customers = data;
      },
      error: (err) => {
        console.error('ERROR CARGANDO CLIENTES:', err);
      }
    });
  }

  loadProducts(): void {
    this.productService.getAll().subscribe({
      next: (data) => {
        this.products = data;
      },
      error: (err) => {
        console.error('ERROR CARGANDO PRODUCTOS:', err);
      }
    });
  }

  addProduct(): void {
    if (!this.selectedProductId) return;

    const product = this.products.find(p => p.id === this.selectedProductId);
    if (!product) return;

    const existingItem = this.saleDetails.find(item => item.productId === product.id);

    if (existingItem) {
      existingItem.quantity += 1;
      existingItem.subtotal = existingItem.quantity * existingItem.price;
    } else {
      this.saleDetails.push({
        productId: product.id,
        name: product.name,
        quantity: 1,
        price: product.price,
        subtotal: product.price
      });
    }

    this.calculateTotal();
    this.selectedProductId = null;
  }

  updateQuantity(item: any): void {
    if (item.quantity < 1) {
      item.quantity = 1;
    }

    item.subtotal = item.quantity * item.price;
    this.calculateTotal();
  }

  removeItem(index: number): void {
    this.saleDetails.splice(index, 1);
    this.calculateTotal();
  }

  calculateTotal(): void {
    this.total = this.saleDetails.reduce((sum, item) => sum + item.subtotal, 0);
  }

  getUserIdFromToken(): number {
    const token = localStorage.getItem('token');
    if (!token) return 0;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      console.log('PAYLOAD COMPLETO DEL TOKEN:', payload);

      for (const key in payload) {
        if (key.toLowerCase().includes('nameidentifier')) {
          return Number(payload[key]);
        }
      }

      return 0;
    } catch (error) {
      console.error('ERROR LEYENDO TOKEN:', error);
      return 0;
    }
  }

  saveSale(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedCustomerId) {
      this.errorMessage = 'Debes seleccionar un cliente.';
      return;
    }

    if (this.saleDetails.length === 0) {
      this.errorMessage = 'Debes agregar al menos un producto.';
      return;
    }

    const userId = this.getUserIdFromToken();
    console.log('USER ID DEL TOKEN:', userId);

    if (userId === 0) {
      this.errorMessage = 'No se pudo obtener el usuario desde el token.';
      return;
    }

    const saleDto = {
      id: 0,
      customerId: this.selectedCustomerId,
      total: this.total,
      userId: userId
    };

    console.log('VENTA A ENVIAR:', saleDto);

    this.saleService.create(saleDto).subscribe({
      next: (saleResponse) => {
        console.log('RESPUESTA DE VENTA:', saleResponse);

        const saleId = saleResponse.id;
        let savedDetails = 0;

        for (const item of this.saleDetails) {
          const detailDto = {
            id: 0,
            saleId: saleId,
            productId: item.productId,
            quantity: item.quantity,
            price: item.price,
            subtotal: item.subtotal
          };

          console.log('DETALLE A ENVIAR:', detailDto);

          this.saleDetailService.create(detailDto).subscribe({
            next: () => {
              savedDetails++;

              if (savedDetails === this.saleDetails.length) {
                this.successMessage = 'Venta guardada correctamente.';
                this.errorMessage = '';

                this.selectedCustomerId = null;
                this.selectedProductId = null;
                this.saleDetails = [];
                this.total = 0;

                setTimeout(() => {
                  this.router.navigate(['/sales/list']);
                }, 1000);
              }
            },
            error: (err) => {
              console.error('ERROR GUARDANDO DETALLE:', err);
              this.errorMessage = 'La venta se creó, pero falló un detalle.';
            }
          });
        }
      },
      error: (err) => {
        console.error('ERROR AL GUARDAR VENTA:', err);
        this.errorMessage = 'No se pudo guardar la venta.';
        this.successMessage = '';
      }
    });
  }
}