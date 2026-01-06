# ZTino Manager

A modern admin dashboard for managing the ZTino e-commerce platform. Built with React 19, Vite, and Ant Design.

## Overview

ZTino Manager is the administrative frontend application for the ZTino Shop ecosystem. It provides a centralized interface for managers to control products, categories, colors, sizes, variants, and product images. The application implements role-based access control, requiring the "Manager" role for access.

## Tech Stack

| Category | Technology |
|----------|------------|
| Framework | React 19 |
| Build Tool | Vite 7 |
| UI Library | Ant Design 5 |
| Styling | TailwindCSS 3 |
| HTTP Client | Axios |
| Routing | React Router DOM 6 |
| Icons | Heroicons |
| Rich Text | Jodit React |
| Auth | JWT (jwt-decode) |

## Quick Start

### Prerequisites

- Node.js (v18+ recommended)
- npm or yarn
- Backend API running (see environment configuration)

### Installation

```bash
# Clone and navigate to the project
cd ztino-manager

# Install dependencies
npm install

# Configure environment
# Create .env file with:
# VITE_API_URL=http://localhost:<BACKEND_PORT>/api/<API_VERSION>

# Start development server
npm run dev
```

The application will be available at `http://localhost:<ztino-manager-port>`

### Available Scripts

| Command | Description |
|---------|-------------|
| `npm run dev` | Start development server |
| `npm run build` | Build for production |
| `npm run preview` | Preview production build |
| `npm run lint` | Run ESLint |

## Project Structure

```
src/
├── config/          # Axios client and global configuration
├── constants/       # API endpoints and constants
├── contexts/        # React contexts (AuthContext)
├── features/        # Feature modules (auth, product)
│   ├── auth/        # Authentication feature
│   └── product/     # Product management feature
├── layouts/         # Layout components (MainLayout, Sider, Header)
├── pages/           # Page components
├── routes/          # Router configuration
└── utils/           # Utility functions (JWT, localStorage)
```

## Key Features

- **Authentication**: JWT-based auth with role validation (Manager role required)
- **Product Management**: Full CRUD for products with image upload
- **Category Management**: Hierarchical category system
- **Color & Size Management**: Product attribute management
- **Product Variants**: Color-size combination management
- **Responsive Layout**: Mobile-friendly admin interface

## Documentation

For detailed documentation, see the `docs/` directory:

- [Architecture Overview](docs/architecture.md)
- [Folder Structure](docs/folder-structure.md)
- [Routing & Navigation](docs/routing.md)
- [Authentication](docs/authentication.md)
- [API Communication](docs/api-communication.md)
- [Features Guide](docs/features/README.md)
- [Environment Configuration](docs/environment.md)
- [Development Guide](docs/development-guide.md)

## Environment Variables

| Variable | Description |
|----------|-------------|
| `VITE_API_URL` | Backend API base URL |

## Architecture Highlights

- **Feature-based Architecture**: Code organized by business domain
- **Custom Hooks Pattern**: Reusable data fetching and mutation logic
- **Centralized API Layer**: Consistent error handling and auth token injection
- **Context-based State**: Auth state managed via React Context
- **Barrel Exports**: Clean import paths via index.js files

## License

Private - Internal use only.
