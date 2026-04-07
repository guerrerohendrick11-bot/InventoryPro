#  InventoryPro

Sistema de gestión de inventario desarrollado con **ASP.NET Core Web API** y **Angular**.

Este proyecto permite administrar productos, clientes y ventas con autenticación basada en JWT.

---

##  Tecnologías utilizadas

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger

### Frontend
- Angular (Standalone Components)
- TypeScript
- HTML / CSS
- HttpClient

---

##  Autenticación

- Login con JWT
- Token almacenado en localStorage
- Protección de endpoints con `[Authorize]`
- Interceptor en Angular para enviar el token automáticamente

---

##  Módulo de Productos
✔ Listar productos  
✔ Crear productos  
✔ Editar productos  
✔ Eliminar productos  
✔ Filtros por nombre y categoría  
✔ Conexión completa con backend  

---

##  Módulo de Ventas

✔ Selección de cliente  
✔ Agregado de productos a carrito  
✔ Cálculo automático de total  
✔ Creación de venta  
✔ Registro de detalles de venta  

---

##  Módulo de Clientes

✔ Listado de clientes  
✔ CRUD completo desde backend  

---

##  Arquitectura

- Backend basado en servicios (Services + Interfaces)
- Uso de DTOs para transferencia de datos
- Frontend modular con Angular
- Separación por módulos: Auth, Products, Sales, Users

---

##  Configuración

### Backend

1. Configurar cadena de conexión en `appsettings.json`
2. Ejecutar migraciones:
```bash
dotnet ef database update
