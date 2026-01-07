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

## 🐳 Docker

### Quick Start with Docker

The recommended way to run ztino-manager is via Docker Compose from the project root:

```bash
# Production (Nginx + static files)
docker compose up ztino-manager --build

# Development (Vite dev server with HMR)
docker compose -f docker-compose.dev.yml up ztino-manager --build
```

### Production Mode

In production, the app is built and served by Nginx:

```bash
# Standalone build
docker build -t ztino-manager .

# Run container
docker run -d -p 3001:80 --name ztino-manager ztino-manager
```

**Access:** http://localhost:3001

### Development Mode (Hot Reload)

Development mode uses bind mounts for instant code updates:

```yaml
# From docker-compose.dev.yml
volumes:
  - ./Frontend/ztino-manager:/app        # Source code mounted
  - manager_node_modules:/app/node_modules  # Isolated node_modules
```

**How it works:**
1. Edit files on your local machine
2. Changes sync instantly to container
3. Vite detects changes and hot-reloads browser
4. No rebuild required!

**Access:** http://localhost:5174

### Vite Host Configuration

> [!IMPORTANT]
> Vite binds to `localhost` by default. Inside Docker, we use `--host 0.0.0.0` to allow external access:
> ```bash
> npm run dev -- --host 0.0.0.0
> ```

### Nginx Reverse Proxy

In production, Nginx serves the app and proxies API requests:

```nginx
# API requests are proxied to backend
location /api/ {
    proxy_pass http://backend:8080/api/;
}

# All other routes serve index.html (SPA routing)
location / {
    try_files $uri $uri/ /index.html;
}
```

This eliminates CORS issues by serving frontend and API from the same origin.

### Multi-Stage Dockerfile

| Stage | Base Image | Purpose |
|-------|------------|---------|
| `build` | `node:20-alpine` | Install deps, build static files |
| `runtime` | `nginx:alpine` | Serve static files (~25MB) |

### Environment Variables in Docker

| Variable | Build Time | Description |
|----------|------------|-------------|
| `VITE_API_URL` | Yes | API base URL (default: `/api/v1`) |

> [!NOTE]
> Vite environment variables are embedded at **build time**, not runtime.
> In production, use `/api/v1` (relative path) for Nginx proxy.

For more details, see the [Docker Guide](../../docs/docker.md).

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
