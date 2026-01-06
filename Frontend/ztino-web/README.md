# ZTino Web

A modern, production-ready e-commerce frontend application built with ReactJS and Vite. 

## OVERVIEW

This project serves as the customer-facing storefront for the ZTino Shop platform, allowing users to browse products, manage carts, and complete purchases seamlessly.

## Tech Stack

| Category | Technology |
|----------|------------|
| **Framework** | React 19 |
| **Build Tool** | Vite 7 |
| **Routing** | React Router v6 |
| **UI Library** | Ant Design 5.27 |
| **Styling** | Tailwind CSS 3.4 |
| **HTTP Client** | Axios 1.13 |
| **Icons** | Heroicons 2.2 |

## Getting Started

### Prerequisites

- Node.js (v18+ recommended)
- npm or yarn

### Installation

```bash
# Clone the repository
git clone <repository-url>
cd Frontend/ztino-web

# Install dependencies
npm install
```

### Environment Configuration

Create a `.env` file in the project root:

```env
VITE_API_URL=http://localhost:<BACKEND_PORT>/api/<API_VERSION>
```

### Development

```bash
# Start development server
npm run dev

# Run linting
npm run lint
```

The application will be available at `http://localhost:<ztino-web-port>`.

### Production Build

```bash
# Build for production
npm run build

# Preview production build locally
npm run preview
```

## Project Structure Overview

```
ztino-web/
├── src/
│   ├── config/          # Axios client and app configuration
│   ├── constants/       # API endpoints and constants
│   ├── contexts/        # React Context providers (Auth)
│   ├── features/        # Feature-based modules
│   │   ├── auth/        # Authentication (login, register)
│   │   ├── cart/        # Shopping cart functionality
│   │   ├── home/        # Homepage components
│   │   └── product/     # Product listing and details
│   ├── layouts/         # Page layouts (MainLayout)
│   ├── pages/           # Route page components
│   ├── routes/          # Routing configuration
│   └── utils/           # Helper utilities
├── docs/                # Technical documentation
└── public/              # Static assets
```

## Key Features

- **User Authentication**: Login and registration with JWT-based authentication
- **Product Browsing**: Category navigation, product listing, and detail views
- **Shopping Cart**: Add to cart functionality with guest and authenticated user support
- **Responsive Design**: Mobile-first approach with Tailwind CSS

## Documentation

For detailed technical documentation, see the [docs/](docs/) folder:

| Document | Description |
|----------|-------------|
| [Architecture](docs/architecture.md) | System design and core concepts |
| [Folder Structure](docs/folder-structure.md) | Detailed directory breakdown |
| [Routing](docs/routing.md) | Navigation and route configuration |
| [State Management](docs/state-management.md) | Data flow and context usage |
| [API Communication](docs/api-communication.md) | Backend integration patterns |
| [Features](docs/features/) | Feature-specific documentation |
| [Development Guide](docs/development-guide.md) | Coding conventions and guidelines |

## Scripts

| Command | Description |
|---------|-------------|
| `npm run dev` | Start development server with HMR |
| `npm run build` | Build production bundle |
| `npm run preview` | Preview production build |
| `npm run lint` | Run ESLint |

## License

This project is private and proprietary.
