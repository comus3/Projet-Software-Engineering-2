# Projet-Software-Engineering-2

The **KitBox Project** is a desktop application designed and developed using .NET MAUI (Multi-platform App UI) with a MySQL database for storage. The project was carried out collaboratively by a group of students over six months as part of a Software Engineering course.

---

## Table of Contents
- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Class Diagram](#class-diagram)
- [Project Structure](#project-structure)
- [Class Responsibilities](#class-responsibilities)
- [User Manual](#user-manual)
- [Getting Started](#getting-started)
- [Contributors](#contributors)

---

## Overview

The **KitBox Project** serves as a showcase for implementing a robust desktop application using modern technologies, adhering to software engineering principles, and delivering a polished product over an extended timeline. It emphasizes modular design, database integration, and a rich user interface.

### Project description

The application aims to help the fictional modular casing shop **KitBox** to manage their inventory, orders, and suppliers efficiently, ensuring streamlined operations and accurate data management.

---

## Technologies Used

- **Framework**: .NET MAUI (Multi-platform App UI)
- **Database**: MySQL
- **Development Environment**: Visual Studio
- **Languages**: C#, SQL

---

## Features

1. **Desktop Application**: A cross-platform, user-friendly interface built with .NET MAUI.
2. **Database Integration**: Centralized data storage and management using MySQL.
3. **Collaborative Design**: Developed by a team of students leveraging design and development best practices.
4. **Class Diagram**: Structured class relationships and interactions ensure modular and maintainable code.

---

## Class Diagram

The class diagram showcases the relationships and interactions between different components of the system. It serves as the backbone of the application, ensuring clarity in the design phase and maintainability in development.

<img src="diagrammedeclasse.svg" alt="Class Diagram for KitBox Project">

**Highlights**:
- **Classes**:
  - Core application logic encapsulated in modular classes.
  - Clear distinction between data models, services, and user interface layers.
- **Relationships**:
  - Dependency relationships linking every part of a KitBox, for knowing what parts we need to order.
  - Association between UI controllers and business logic.

---

## Class Responsibilities

The system is designed using modular classes to ensure maintainability and clarity. Here's a summary of key classes:

#### Core Functionality:

- **Model**:  
  Manages core data operations such as loading, updating, and deleting data. It serves as the central hub for interacting with the database, ensuring data integrity and centralized functionality for manipulating `DataTable` and related objects.

- **SharedData**:  
  Stores global or shared variables used across the system, such as `SelectedPartCode`. This class provides access to system-wide constants or state information.

#### Service Classes:

- **FetchingServices**:  
  Provides services for retrieving data from the database. Includes methods like `FetchAvailableDimensions`, `FetchArmoirePieces`, and other utility functions that gather specific datasets required by various components.

- **LinkingServices**:  
  Handles linking or associating components such as `Armoire` and `Piece`. Provides methods to create relationships between objects within the database, ensuring the proper linking of system entities.

- **StockServices**:  
  Manages stock-related operations, including stock reservation, availability checks, updates, and removing items from inventory. This class is crucial for inventory management in the system.

#### Entities:

- **Casier** and **Armoire**:  
  Represent physical components within the KitBox system, such as compartments (`Casier`) and cabinets (`Armoire`). These classes store primary attributes of each component and include methods for managing their lifecycle and operations.

- **Piece**:  
  Represents individual parts or pieces within the KitBox system. This class is linked to operations such as creation, updates, and deletion of parts in the system.

- **Supplier**:  
  Represents a supplier in the system. It contains attributes like `tableName` and `primaryKey`, which are used for database interactions related to suppliers.

- **Command**:  
  Manages customer order-related data, handling information about specific orders in the system and facilitating order processing.

#### Utility Classes:

- **Logger**:  
  A utility class that handles logging events and errors to a file. It includes methods like `WriteToFile` for tracking system operations and recording any issues or important system events.

- **MySqlConnection** and **DataTable**:  
  These classes manage database connectivity (`MySqlConnection`) and facilitate interaction with database tables (`DataTable`). They are essential for performing CRUD (Create, Read, Update, Delete) operations on the database.

- **Displayer**:  
  Responsible for rendering and presenting data to users. It ensures that the data retrieved from various services is displayed in a readable and user-friendly format.

#### Extensions:

- **RTCasier**, **RTArmoire**, **RTSupplier**:  
  These are **Relation Table** classes, which serve as runtime classes to manage relationships between entities in the system.

#### Support Classes:

- **DevTools** and **AppServices**:  
  These are support classes that provide additional application-level services.

---
## User Manual

### About the Secretary

The secretary has several roles within the system:

- **View and Modify Parts**:
  - View the list of all parts available in the store.
  - Modify the prices of parts (2).
  - Search for parts using the search bar (1).
  
- **Access the Order History**:
  - Access the order history through the "insight" page (3).
  
**User Story**:  
As a secretary, I need to manage parts and view order history so I can maintain pricing and track sales activities.

**Actions**:
1. **Search**: Type in the search bar to filter parts by code, stock number, or price.
2. **Modify Price**: Click on a part to modify its price.
3. **View Order History**: Click the "insight" button to access the order history.

---

### About the Supplier

The supplier has responsibilities for managing the supplier details and parts offered by each supplier:

- **View and Modify Supplier Information**:
  - View and modify supplier names, addresses, and telephone numbers (5).
  - Add a new supplier (9).
  
- **Manage Supplier Parts**:
  - See the parts offered by a supplier (6).
  - Add a new part for a supplier (7).
  - Modify the price and lead time of each part offered (8).

**User Story**:  
As a supplier, I need to manage my details and the parts I offer to ensure accurate product information and pricing.

**Actions**:
4. **View Suppliers**: Consult the list of suppliers and use the search bar to filter by name, address, telephone, or ID.
5. **Edit Supplier**: Click "Edit" to modify supplier information.
6. **View Supplier Parts**: Click a supplier to see their parts.
7. **Add Part**: Click "Add a Part" to enter details for new parts.
8. **Edit Part**: Modify part prices and lead times by clicking "Edit."
9. **Add Supplier**: Click "Add a Supplier" and fill out the required form.

---

### About the Stock Manager

The stock manager is able to:

- **Consult Stock Details**: View stock information (stock, reserved, awaiting) for each part.
- **Set and Edit Stock**: Set minimum stock levels (1) and edit stock levels (2).
- **Order Parts from Suppliers**: Order parts from a supplier (3) and input arrival stock (4).

**User Story**:  
As a stock manager, I need to manage stock levels and order parts to ensure there is enough inventory for customer orders.

**Actions**:
1. **Set Minimum Stock**: Change minimum stock levels by typing in the field and clicking "Save."
2. **Edit Stock**: Click "Edit" to update stock details.
3. **Order Parts**: Click "Order," select a supplier, and confirm the part quantity.
4. **Input Arrival Stock**: Check the checkbox on the input arrival page to add new stock.

---

### About the Shopkeeper

The shopkeeper is responsible for completing orders once payment has been made.

**User Story**:  
As a shopkeeper, I need to finalize completed orders once payment is received.

---

### About the Ordermaker

The ordermaker can verify the content of pending orders, review parts, and finalize them once they are fully assembled.

**User Story**:  
As an ordermaker, I need to check the parts in pending orders and confirm when they are complete for processing.

---

### About the Customer

The customer can create and register a custom cabinet order by entering the required dimensions and customizing features like height, color, and door options.

**User Story**:  
As a customer, I want to create a custom cabinet by specifying the dimensions and features and then place my order.

**Actions**:
- Enter the cabinet dimensions and select features like height, color, and whether to include a door.
- Review the basket tab and finalize or cancel the order.

---

## Getting Started

### Prerequisites
- .NET 8.0
- MySQL 8.0 or later
- Visual Studio 2022

### Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/petitbato/Projet-Software-Engineering-2.git
