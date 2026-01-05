# 🛍️ ZTino-Shop

ZTino-Shop is a **full-stack E-Commerce web application** for clothing retail, built with **.NET 8 Web API** for the Backend and **ReactJS 19** for the Frontend.

- The project simulates a real-world e-commerce system and is developed with a clear focus on:
  - Not just making the code “work”
  - But emphasizing **system architecture**, **clear layer separation**, **scalability**, **maintainability**, and **well-structured documentation**

These are core principles required in **professional software development environments** and **production-ready projects**.

---

## 📂 Project Structure

```txt
ZTino-Shop/
│
├── Frontend/
│   ├── ztino-manager/        # Manager-facing frontend
│   │   └── README.md         # Detailed documentation
│   │
│   └── ztino-web/            # Customer-facing frontend
│       └── README.md         # Detailed documentation
│
├── Backend/
│   └── ZTino_Shop/           # .NET 8 Web API
│       └── README.md         # Detailed documentation
│
└── README.md                 # Project overview documentation
```
---

### 📚 Detailed Documentation

Each part of the system has its own README, including:

- Architecture & directory structure
- Technologies used
- Convention & best practices
- Running instructions
- Design decisions

👉 Please check the README in each directory for more details.

---

## 🎯 Project Goals

- Build an e-commerce system following the **client-server** model
- Apply popular architectures in practice:
  - **Backend:** Monolithic + Onion Architecture
  - **Frontend:** Feature-Based Architecture
- Implement authentication and authorization using **JWT**
- Clearly separate between:
  - Manager system
  - Customer website
- Train system design thinking and technical documentation writing

---

## 🏗️ System Architecture

### 1. System Overview

ZTino-Shop includes **3 main components**:

- **Backend (.NET 8 Web API)**
  Acts as the central component, processing business logic, authentication, authorization, and providing data through REST API.

- **ztino-manager (ReactJS)**  
  Manager-facing frontend, used to manage products, categories, orders, users, etc.

- **ztino-web (ReactJS)**  
  Customer-facing website, displays products, handles shopping cart, and places orders.

---

### 2. Backend Architecture

Backend is built using **Monolithic Architecture** combined with **Onion Architecture**.

- **Monolithic Architecture**
  - The entire backend system is deployed in a single application
  - Suitable for small and medium projects
  - Easy to develop, deploy, and manage

- **Onion Architecture**
  - Clearly separate layers
  - Reduce dependencies between components
  - Easy to maintain and extend in the future

**Core Principles:**
- Domain does not depend on Infrastructure
- Business logic is located at Application layer
- Infrastructure only implements details (EF Core, Database, External services)

---

### 3. Frontend Architecture

Both frontend (**ztino-manager** and **ztino-web**) are organized using **Feature-Based Architecture**.

**Key Features:**
- Code is organized by **feature** instead of file type
- Each feature can include:
  - UI components
  - API calls
  - Custom hooks
  - Private logic

**Benefits:**
- Easy to expand when the project grows
- Easy to read, maintain
- Reduce dependencies between frontend components

---

### 4. System Architecture

- RESTful API
- JWT Authentication
- Role-based Authorization:
  - Manager
  - User
- Client – Server Architecture
- Frontend and Backend are completely separated

> *Detailed architecture diagrams will be presented in the respective module documentation.*

---

## 🧰 Technologies used

### Backend
- .NET 8
- Entity Framework Core
- Microsoft SQL Server
- JWT Authentication

> Backend uses caching mechanisms and integrates APIs from third-party services to optimize performance and expand functionality.

### Frontend
- ReactJS
- Ant Design
- Tailwind CSS
- Axios

### Others
- RESTful API
- Git & GitHub

---

## 🚀 Quick Start

### Requirements
- Node.js
- .NET SDK 8
- SQL Server

### Steps to run the project
1. Clone repository to your machine
2. Configure and run Backend (.NET Web API)
3. Run frontend for Manager (`ztino-manager`)
4. Run frontend for User (`ztino-web`)

📌 **Note:**  
Detailed instructions for each part are provided in their respective README files:

- `Backend/ZTino_Shop/README.md`
- `Frontend/ztino-manager/README.md`
- `Frontend/ztino-web/README.md`

---

## 📊 Project Status

### Completed
- **Product:** Manage products, categories, and related information
- **Cart:** Shopping cart management (add/update/remove items, guest & authenticated carts)
- **Authentication & Authorization (JWT):** Login, role-based authorization (Manager / User)

### In Progress
- **Order:** Handle order processing and order management
- **Payment:** Integrate payment and complete the checkout process

### Future Plans
- Deploy production
- Set up CI/CD
- Optimize performance and security
- Complete technical documentation

---

## 👤 Author

- **Name:** Do Manh Duy  
- **Role:** Full-stack Developer (.NET & React)

### 🎯 Development Goals

- Develop an e-commerce system that can be **deployed and operated in a production environment**
- Apply popular architectures in practice to ensure **scalability, maintainability, and long-term upgrade**
- Approach professional software development processes from design, deployment to documentation
- Complete a real project to serve the goal of **portfolio and long-term development direction**

---

## 📄 Notes

ZTino-Shop is developed with the goal of being a **real product that can be used in practice**, not just a learning project.  
The system is designed to be scalable, integrable with additional features, and deployed on a production environment when needed.

The project is currently being refined and continuously upgraded.  
Any feedback, bug reports, or support requests are welcome:

- **Name:** Do Manh Duy 
- **Role:** Full-stack Developer (.NET & React)  
- **Email:** manhduy261000@gmail.com
- **GitHub:** https://github.com/mduy26100
- **LinkedIn:** https://www.linkedin.com/in/duy-do-manh-1a44b42a4/
- **Facebook:** https://www.facebook.com/zuynuxi/