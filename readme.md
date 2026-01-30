# 🔨 Auction Management System

[![.NET Framework](https://img.shields.io/badge/.NET-Enterprise-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![StyleCop](https://img.shields.io/badge/Linter-StyleCop-blue)](https://github.com/StyleCop/StyleCop)
[![Architecture](https://img.shields.io/badge/Pattern-N--Tier-green)](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/n-tier)

A robust, enterprise-grade auction management platform built with a strictly decoupled N-Tier architecture. The system handles complex bidding logic, user management, and high-integrity data persistence.

---

## 🏛️ Architectural Design

The solution is organized into distinct layers to ensure high maintainability, testability, and adherence to the **Single Responsibility Principle**.



### 1. 🖥️ Presentation Layer (`AuctionManagementApplication`)
* The entry point of the application.
* Handles user interactions and orchestrates calls to the Service Layer.

### 2. ⚙️ Service Layer (`ServiceLayer`)
* **Interfaces & Services:** Acts as a mediator between the UI and the Data layers.
* Contains the business workflows and encapsulates high-level application logic.

### 3. 🧠 Domain Model (`DomainModel`)
* **Core Entities:** The heart of the system containing business objects (Users, Auctions, Bids).
* **Validators:** Integrated business rule validation to ensure data integrity before persistence.

### 4. 💾 Data Access Layer (`DataMapper`)
* **DAO (Data Access Objects):** Implements the logic to communicate with the database.
* **Migrations:** Manages version control for the database schema.
* **Interfaces:** Decouples the data persistence implementation from the rest of the system.

### 5. 🧪 Quality Assurance (`Test*` Layers)
* Separate test projects for each layer: `TestDataMapper`, `TestDomainModel`, and `TestServiceLayer`.
* Built to support Unit Testing and Integration Testing across the entire stack.

---

## 📁 Project Structure

```text
.
├── AuctionManagementApplication/  # UI/Entry Point
├── ServiceLayer/                  # Business Workflows & Interfaces
├── DomainModel/                   # Entities & Business Rule Validation
├── DataMapper/                    # Persistence (DAO, Migrations, Interfaces)
├── packages/                      # External dependencies (StyleCop, etc.)
└── Test*/                         # Comprehensive Test Suite for all layers
