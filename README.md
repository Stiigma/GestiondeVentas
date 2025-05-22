#  Gestión de Ventas

> Proyecto académico desarrollado como parte de la materia **Programación en Ambientes Visuales**  
> **Semestre:** Quinto  
> **Alumno:** Eduardo Castro

---

## Descripción General

**Gestión de Ventas** es un sistema estructurado por capas que permite la administración de compras e ingresos, ventas y sus detalles, control de stock de artículos, así como la gestión de personas, usuarios y proveedores.

La arquitectura está basada en el principio de **separación de responsabilidades**, utilizando patrones como **Repository** y **DTOs** para facilitar la escalabilidad y mantenibilidad del sistema.

---

##  Estructura del Proyecto

### GV.Domain
Contiene los modelos y contratos de repositorio que definen la lógica de negocio.

- `Models/`: Entidades como `Articulo`, `Usuario`, `Venta`, etc.
- `Repositorys/`: Interfaces que definen el contrato que debe cumplir cada repositorio.  
  Ejemplos:
  - `IArticuloRepository.cs`
  - `IVentasRepository.cs`
  - `IUsuarioRepository.cs`
  - `IPersonaRepository.cs`
  - ...

---

### GV.Infrastructure
Contiene las implementaciones reales de los repositorios e integración con la base de datos.

- `Data/ContextDB.cs`: Contexto de Entity Framework.
- `Repositories/`: Implementaciones concretas como:
  - `ArticuloRepository.cs`
  - `CategoriaRepository.cs`
  - `DetalleIngresoRepository.cs`
  - `UsuarioRepository.cs`
  - ...

---

### GV.Application
Capa de aplicación que orquesta los casos de uso, mapea DTOs y ofrece los servicios que usa la presentación.

- `DTOs/`: Objetos de transferencia para mantener aislado el dominio.
  - `ArticuloDTO.cs`, `IngresoDTO.cs`, `VentaDTO.cs`, etc.
- `Mapper/`: Clases auxiliares para transformar entre entidades y DTOs.
- `Servicios/`: Casos de uso que coordinan lógica entre repositorio y DTO.
  - `ServicioIngresos.cs`
  - `ServicioVentas.cs`

> ⚠️ **Nota:** Aunque lo ideal sería tener un servicio por cada funcionalidad del sistema, en esta práctica se limitaron a **dos servicios principales** (`Ingresos` y `Ventas`) por fines educativos y simplicidad, incluyendo en estos servicios también la lógica relacionada con `Usuarios`, `Personas`, etc.

---

### GestionDeVentas (Presentación)
Capa de presentación son HTML's con bootstrap y js para mantener un dinamismo con los datos
---

###  Base de Datos
- Se incluye el archivo `ControlDeVentas.bacpac` para importar la base de datos usada en pruebas.
- La solución contiene el archivo `GestionVentasDB.sln` compatible con Visual Studio.

---

##  Capturas de Pantallas y Funcionalidades

Este sistema permite:

- Registrar y editar ingresos.
- Asociar detalles de ingreso con productos.
- Registrar ventas y sus detalles.
- Calcular subtotales, impuestos (por default 16%) y totales.
- Activar/desactivar registros.
- Filtrar y buscar Articulos y proveedores activos.
---

##  Estado del Proyecto

- [x] Arquitectura en capas implementada.
- [x] Servicios de ingresos y ventas funcionando.
- [x] CRUD básico para ingresos y ventas.
- [ ] Control completo de stock (en progreso).
- [ ] Refactorización para separar más servicios (opcional).

---

## Requisitos para ejecutar

- Visual Studio 2022+
- SQL Server Management Studio
- .NET 6 o superior
- Restaurar la base de datos con el archivo `.bacpac`
- Configurar correctamente la cadena de conexión en `appsettings.json`

---

## Créditos

Desarrollado por: **Eduardo Castro**  
Materia: **Programación en Ambientes Visuales**  
Semestre: Quinto - FIAD, UABC  
Profesor: *Victor Rafael Velázquez*

---

