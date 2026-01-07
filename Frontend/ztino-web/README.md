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

## 🐳 Docker

### Quick Start with Docker

The recommended way to run ztino-web is via Docker Compose from the project root:

```bash
# Production (Nginx + static files)
docker compose up ztino-web --build

# Development (Vite dev server with HMR)
docker compose -f docker-compose.dev.yml up ztino-web --build
```

### Production Mode

In production, the app is built and served by Nginx:

```bash
# Standalone build
docker build -t ztino-web .

# Run container
docker run -d -p 3000:80 --name ztino-web ztino-web
```

**Access:** http://localhost:3000

### Development Mode (Hot Reload)

Development mode uses bind mounts for instant code updates:

```yaml
# From docker-compose.dev.yml
volumes:
  - ./Frontend/ztino-web:/app           # Source code mounted
  - web_node_modules:/app/node_modules  # Isolated node_modules
```

**How it works:**
1. Edit files on your local machine
2. Changes sync instantly to container
3. Vite detects changes and hot-reloads browser
4. No rebuild required!

**Access:** http://localhost:5173

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
