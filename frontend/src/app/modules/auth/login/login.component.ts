import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [ReactiveFormsModule]
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;
  errorMessage: string = '';

  // CONTROLAR VISIBILIDAD DE PASSWORD
  showPassword: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  // MÉTODO PARA MOSTRAR/OCULTAR PASSWORD
  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
  console.log("LOGIN CLICK");

  if (this.loginForm.invalid) return;

  this.authService.login(this.loginForm.value).subscribe({
    next: (res) => {
      console.log("RESPUESTA:", res);

      this.authService.saveToken(res.token);

      const role = this.authService.getRole();
      console.log("ROL DEL TOKEN:", role);

      this.router.navigate(['/products']).then(result => {
        console.log("NAVEGÓ:", result);
      });
    },
    error: (err) => {
      console.error(err);
      this.errorMessage = 'Usuario o contraseña incorrectos';
    }
  });
}
}