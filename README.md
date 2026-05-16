# HL Warehouse Stock Management

Warehouse stock and rack management plugin for nopCommerce with advanced rack positioning, pallet tracking, stock movement, and warehouse operation management.

---

## ✨ Features

### 📦 Rack Management
- Create and manage warehouse racks
- Multi-level rack structure
- Rack visibility control
- Rack function type support
  - Storage
  - Picking
  - Dispatch
  - Transit
- Rack sequence & display ordering
- Rack-level type mapping

---

### 🏷 Rack Level Type Management
- Dynamic rack level types
- Function-type based level filtering
- Visible/invisible level control
- Separate level configurations for:
  - Storage racks
  - Picking racks
  - Shared rack levels

Example:

| Function Type | Available Levels |
|---|---|
| Storage | A1, A2, B1 |
| Picking | P1, P2 |
| Shared | Common-01 |

---

### 📍 Product Position Management
- Assign products inside racks
- Product position support:
  - Left
  - Right
  - Center
  - Front
  - Back
- Rack-wise product placement
- Level-wise product allocation

---

### 🧾 Product Mapping
- Add products directly into rack levels
- Popup product selector
- Product search support
- Product category display
- Product specification display
- Duplicate product prevention

---

### 🔎 Rack Searching
Search racks using:
- Rack name
- Function type
- Product
- Level type
- Visibility status

---

### 📊 Product Information Display
Rack products show:
- Product name
- Category
- Specifications
- Rack position
- Rack level

---

### 🏗 Pallet Management
- Pallet-wise stock handling
- Pallet mapping with shipment
- Product pallet tracking
- Transit pallet management

---

### 🚚 Shipment Transit Management
- Shipment transit entries
- Shipment pallet allocation
- Transit stock movement
- Rack-to-rack product movement

---

### 🔄 Dynamic Level Filtering
Rack level types automatically filter based on selected rack function type.

Example:
- Selecting **Storage** only shows storage-compatible levels
- Selecting **Picking** only shows picking-compatible levels
- Shared visible levels appear in both

---

### 🛡 Validation & Safety
- Duplicate rack prevention
- Duplicate level mapping prevention
- Product duplication prevention inside same rack level
- Visibility-based filtering
- Permission-based access control

---

## 🧱 Technologies Used

- ASP.NET Core
- nopCommerce 4.90
- Entity Framework Core
- jQuery
- DataTables
- Bootstrap 4

---

## 📂 Main Modules

```text
Controllers/
 ├── RackController
 ├── ShipmentTransitController

Factories/
 ├── RackModelFactory

Services/
 ├── RackService

Domain/
 ├── Rack
 ├── RackLevel
 ├── RackProduct
 ├── RackLevelType

Views/
 ├── Rack
 ├── ShipmentTransit
```

---

## ⚙️ Rack Workflow

```text
Create Rack
   ↓
Select Function Type
   ↓
Select Rack Levels
   ↓
Save Rack
   ↓
Add Products to Levels
   ↓
Assign Product Positions
```

---

## 📌 Example Rack Structure

```text
Rack: STORAGE-A

 ├── Level A1
 │     ├── Product: Apple Juice
 │     └── Position: Left
 │
 ├── Level A2
 │     ├── Product: Mango Juice
 │     └── Position: Right
 │
 └── Level B1
       ├── Product: Orange Juice
       └── Position: Center
```

---

## 🔐 Permissions

Plugin includes admin permission support for:
- Rack management
- Rack level management
- Product mapping
- Shipment transit operations

---

## 🧩 Key Functionalities

### Rack Product Popup
- Product search
- Multi-product selection
- Product position selection
- Rack-level assignment

### Child Grid Support
- Expandable rack levels
- Nested rack product grids
- Dynamic refresh after insert/delete

### Auto Complete
- Rack auto-complete
- Product SKU auto-complete

---

## 🚀 Installation

1. Copy plugin into:

```text
Plugins/Nop.Plugin.Misc.CountingSequence
```

2. Restart application

3. Go to:

```text
Admin → Configuration → Local Plugins
```

4. Install plugin

---

## 🗄 Database Tables

```text
Rack
RackLevel
RackProduct
RackLevelType
ShipmentTransit
ShipmentTransitItem
Pallet
PalletProduct
```

---

## 📈 Future Enhancements

- Barcode scanning
- QR-based rack tracking
- Warehouse heatmap
- Product movement history
- Stock reservation
- FIFO/FEFO support
- Mobile warehouse operations
- Inventory forecasting

---

## 👨‍💻 Developed For

Warehouse and stock management operations using nopCommerce administrative infrastructure.

---

## 📄 License

Custom warehouse management plugin for nopCommerce.
