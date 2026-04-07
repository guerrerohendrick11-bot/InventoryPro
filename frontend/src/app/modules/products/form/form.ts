import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form.html',
  styleUrls: ['./form.css']
})
export class FormComponent implements OnInit {

  productForm!: FormGroup;
  errorMessage: string = '';
  successMessage: string = '';

  productId: number | null = null;
  isEditMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(1)]],
      stock: [0, [Validators.required, Validators.min(0)]],
      categoryId: [1, [Validators.required, Validators.min(1)]],
      isActive: [true]
    });

    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.productId = Number(idParam);
      this.isEditMode = true;
      this.loadProduct(this.productId);
    }
  }

  loadProduct(id: number): void {
    this.productService.getById(id).subscribe({
      next: (product) => {
        console.log('PRODUCTO CARGADO:', product);

        this.productForm.patchValue({
          name: product.name,
          price: product.price,
          stock: product.stock,
          categoryId: product.categoryId,
          isActive: product.isActive
        });
      },
      error: (err) => {
        console.error('ERROR AL CARGAR PRODUCTO:', err);
        this.errorMessage = 'No se pudo cargar el producto.';
      }
    });
  }

  onSubmit(): void {
    if (this.productForm.invalid) {
      this.errorMessage = 'Completa correctamente todos los campos.';
      return;
    }

    const productData = {
      id: this.productId ?? 0,
      ...this.productForm.value
    };

    if (this.isEditMode && this.productId !== null) {
      this.productService.update(this.productId, productData).subscribe({
        next: () => {
          this.successMessage = 'Producto actualizado correctamente.';
          this.errorMessage = '';

          setTimeout(() => {
            this.router.navigate(['/products']);
          }, 800);
        },
        error: (err) => {
          console.error('ERROR AL ACTUALIZAR PRODUCTO:', err);
          this.errorMessage = 'No se pudo actualizar el producto.';
        }
      });
    } else {
      this.productService.create(productData).subscribe({
        next: () => {
          this.successMessage = 'Producto creado correctamente.';
          this.errorMessage = '';

          setTimeout(() => {
            this.router.navigate(['/products']);
          }, 800);
        },
        error: (err) => {
          console.error('ERROR AL CREAR PRODUCTO:', err);
          this.errorMessage = 'No se pudo crear el producto.';
        }
      });
    }
  }

  deleteProduct(): void {
    if (this.productId === null) return;

    const confirmDelete = confirm('¿Seguro que deseas eliminar este producto?');

    if (!confirmDelete) return;

    this.productService.delete(this.productId).subscribe({
      next: () => {
        this.successMessage = 'Producto eliminado correctamente.';
        this.errorMessage = '';

        setTimeout(() => {
          this.router.navigate(['/products']);
        }, 800);
      },
      error: (err) => {
        console.error('ERROR AL ELIMINAR PRODUCTO:', err);
        this.errorMessage = 'No se pudo eliminar el producto.';
      }
    });
  }
}