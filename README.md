# TPI
# Programacion IV
# AUTOR: Evelyn Roscher

## ☕ Resumen del Proyecto

La cafetería de especialidad Coffee Blend busca incorporar un servicio de take away para optimizar tiempos y brindar una mejor experiencia a sus clientes. Actualmente, los pedidos sólo pueden realizarse de forma presencial, lo que genera esperas innecesarias.
Se desarrollará una página web que permita a los clientes realizar pedidos en línea. El sistema informará en tiempo real el tiempo estimado de preparación y retiro, mejorando la experiencia del cliente y la eficiencia del servicio.

**Tipos de Usuario:**
- Administrador: Gestiona pedidos, modifica el menú, controla tiempos de preparación y administra usuarios.
- Usuario: Realiza pedidos, consulta tiempos estimados y gestiona su cuenta personal.

---

## 🏗️ Arquitectura y Estructura del Proyecto

El software fue construido utilizando **.NET 10** bajo el patrón de diseño **Clean Architecture**, asegurando un alto grado de desacoplamiento, facilidad para realizar pruebas y mantenimiento a largo plazo. 

La solución está dividida en las siguientes capas:

1. **TPI.Domain:** Entidades principales de negocio (Order, Product, Payment, Customer, etc.) y reglas de negocio puras (enumeradores de estados).
2. **TPI.Application:** Casos de uso de la aplicación, interfaces de repositorios (Abstractions) y la lógica de los servicios orquestadores (`OrderService`, `PaymentService`, etc.).
3. **TPI.Infrastructure:** Detalles de implementación, conexión a la base de datos SQL Server mediante **Entity Framework Core**, y repositorios concretos.
4. **TPI.Presentation:** Controladores Web API (Endpoints), inyección de dependencias y configuración de **Swagger** para documentación y pruebas.

---

## 🛡️ Reglas de Negocio y Validaciones (Restricciones)

Para garantizar la integridad del sistema y simular un entorno de producción real, se implementaron diversas restricciones lógicas:

### 📦 Productos y Stock
- Al crear una orden, el sistema **descuenta automáticamente el stock** de los productos solicitados.
- Si una orden es cancelada, **el stock se devuelve** inmediatamente al inventario.
- No se puede crear una orden si no hay stock suficiente de un producto solicitado.

### 💳 Pagos (Payments)
- **Pago Único:** Solo se permite tener un pago activo por orden. Si un pago es rechazado, este sufre un *soft-delete*, permitiendo al cliente generar un nuevo intento de pago para la misma orden.
- **Coincidencia de Montos:** El monto del pago (`Amount`) debe ser **exactamente igual** al monto total de la orden.
- **Transferencias Bancarias:** Por políticas del comercio, el método de pago `BankTransfer` solo puede ser utilizado si el monto a pagar supera los `$100.000`.

### 📝 Órdenes (Orders) y Estados
- **Cancelar Orden (`Cancel`):** Se puede cancelar en cualquier momento antes de ser entregada. Devuelve el stock de los productos.
- **Marcar como Lista (`Ready`):** Indica que el café está listo para ser retirado. No se puede realizar sobre órdenes canceladas o ya entregadas.
- **Entregar Orden (`Deliver`):** Es el paso final. **Validación estricta:** Es obligatorio que la orden tenga un pago asociado y que el estado de ese pago sea **`Accepted`** (Aprobado). No se puede entregar mercadería sin un pago válido.

---

## 🚀 Flujo de Trabajo (Happy Path en Swagger)

A continuación se detalla el flujo ideal para probar el sistema desde la interfaz de Swagger:

1. **Crear Productos (Opcional):** 
   - Endpoint: `POST /api/Product`
   - Asegurate de que haya productos con stock en la base de datos para poder comprarlos (O podés usar los que ya tenías cargados).

2. **Crear la Orden:**
   - Endpoint: `POST /api/Order`
   - Creá una orden especificando el `CustomerId` y una lista de `OrderItems` (con su `ProductId` y `Quantity`).
   - *El sistema reservará el stock y calculará el total.* Copiá el `OrderId` que devuelve como respuesta.

3. **Registrar un Pago:**
   - Endpoint: `POST /api/Payment`
   - Utilizá el `OrderId` del paso anterior en el cuerpo del JSON. 
   - Recordá que el `Amount` debe ser **exactamente igual al total de la orden**, y si usás el método de pago `BankTransfer` (valor numérico `0`), el monto debe ser mayor a 100000 (sino, utilizá `Cash`, valor numérico `1`). Copiá el ID del pago que devuelve la respuesta.

4. **Aprobar el Pago (Simulación del Admin):**
   - Endpoint: `POST /api/Payment/{id}/accept`
   - Pasale el ID del pago recién creado para cambiar su estado a `Accepted`.

5. **Marcar la Orden como Lista:**
   - Endpoint: `POST /api/Order/{id}/ready`
   - Pasale el `OrderId` a la ruta. El pedido ya está preparado.

6. **Entregar la Orden:**
   - Endpoint: `POST /api/Order/{id}/deliver`
   - Pasale el `OrderId`. Como la orden ya tiene un pago aprobado, el sistema permitirá completar la entrega exitosamente. **¡Flujo completado!**

---

## 🗂️ Resumen de Endpoints por Entidad

A continuación se presenta un resumen de los principales métodos de la API organizados por la entidad que gestionan:

### 👤 Customer (Cliente)
* `GET /api/Customer`: Obtiene el listado de clientes.
* `GET /api/Customer/{id}`: Obtiene la información de un cliente específico.
* `POST /api/Customer`: Registra un nuevo cliente en el sistema.
* `PUT /api/Customer/{id}`: Actualiza los datos personales de un cliente.
* `DELETE /api/Customer/{id}`: Elimina de forma lógica (soft-delete) a un cliente.

### 📦 Product (Producto)
* `GET /api/Product`: Lista todos los productos disponibles en el menú.
* `GET /api/Product/{id}`: Obtiene el detalle de un producto (precio, stock disponible, etc.).
* `POST /api/Product`: Crea un nuevo producto.
* `PUT /api/Product/{id}`: Actualiza la información de un producto.
* `DELETE /api/Product/{id}`: Elimina de forma lógica un producto.

### 📝 Order (Orden)
* `GET /api/Order`: Obtiene el historial de todas las órdenes.
* `GET /api/Order/{id}`: Obtiene el detalle completo de una orden, incluyendo los productos solicitados y el cliente.
* `POST /api/Order`: Crea una nueva orden, calculando el total y descontando el stock.
* `POST /api/Order/{id}/cancel`: Cancela una orden activa y restituye el stock de los productos correspondientes.
* `POST /api/Order/{id}/ready`: Avanza la orden al estado "Lista para retirar".
* `POST /api/Order/{id}/deliver`: Finaliza la orden marcándola como "Entregada" (requiere que el pago esté aprobado).
* `POST /api/Order/recalculate-totals`: Método administrativo interno para recalcular y actualizar los totales de las órdenes existentes.
* `POST /api/Order/obtenerCotizacion`: Obtiene de la API externa (Bluelytics) el precio del dólar oficial (`value_buy`) y lo guarda temporalmente en memoria.
* `GET /api/Order/getOrderEnDolares/{id}`: Devuelve información simplificada de la orden y sus productos con precios convertidos a dólares en base a la cotización obtenida previamente.

### 💳 Payment (Pago)
* `GET /api/Payment`: Lista todos los pagos registrados en el sistema.
* `GET /api/Payment/{id}`: Obtiene la información detallada de un pago (monto, estado, método).
* `GET /api/Payment/rejected`: Obtiene un listado de todos los pagos que fueron rechazados.
* `POST /api/Payment`: Registra un intento de pago vinculándolo a una orden específica.
* `POST /api/Payment/{id}/accept`: Aprueba un pago que se encontraba en revisión.
* `POST /api/Payment/{id}/reject`: Rechaza un pago (le aplica soft-delete), lo que permite al cliente intentar abonar nuevamente.
