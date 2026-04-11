import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SaleService } from '../../../core/services/sale.service';
import { CustomerService } from '../../../core/services/customer.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './list.html',
  styleUrls: ['./list.css'],
})
export class List implements OnInit {
  sales: any[] = [];
  customers: any[] = [];
  errorMessage: string = '';
  loading: boolean = true;
  role: string = '';

  constructor(
    private saleService: SaleService,
    private customerService: CustomerService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.role = this.getUserRoleFromToken();

    setTimeout(() => {
      this.loadData();
    }, 0);
  }

  getUserRoleFromToken(): string {
    const token = localStorage.getItem('token');
    if (!token) return '';

    const payload = JSON.parse(atob(token.split('.')[1]));

    return (
      payload['role'] ||
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
      ''
    );
  }

  loadData(): void {
    this.errorMessage = '';

    this.customerService.getAll().subscribe({
      next: (customersData: any[]) => {
        this.customers = customersData;

        this.saleService.getAll().subscribe({
          next: (salesData: any[]) => {
            console.log('VENTAS CARGADAS:', salesData);
            this.sales = salesData;
            this.loading = false;
            this.cdr.detectChanges();
          },
          error: (err: any) => {
            console.error('ERROR CARGANDO VENTAS:', err);
            this.errorMessage = 'No se pudieron cargar las ventas.';
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
  }

  getCustomerName(customerId: number): string {
    const customer = this.customers.find(c => c.id === customerId);
    return customer ? customer.name : 'Sin cliente';
  }
}