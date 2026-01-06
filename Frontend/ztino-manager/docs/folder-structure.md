# Folder Structure

This document describes the directory organization of the ZTino Manager project.

## Root Directory

```
ztino-manager/
├── public/              # Static assets served as-is
├── src/                 # Application source code
├── docs/                # Project documentation
├── index.html           # Entry HTML file
├── package.json         # Dependencies and scripts
├── vite.config.js       # Vite configuration
├── tailwind.config.js   # TailwindCSS configuration
├── postcss.config.js    # PostCSS configuration
├── eslint.config.js     # ESLint configuration
└── .env                 # Environment variables (not committed)
```

## Source Directory (`src/`)

### Entry Points

| File | Purpose |
|------|---------|
| `main.jsx` | Application bootstrap, renders App with React.StrictMode |
| `App.jsx` | Root component, wraps AuthProvider around AppRouter |
| `index.css` | Global styles and TailwindCSS imports |
| `App.css` | App-level styles |

### Core Directories

#### `src/config/`
Global configuration including the Axios HTTP client.

| File | Purpose |
|------|---------|
| `axiosClient.js` | Configured Axios instance with interceptors |
| `index.js` | Barrel exports |

#### `src/constants/`
Application-wide constants and configuration values.

| File | Purpose |
|------|---------|
| `apiEndpoints.js` | API endpoint path definitions |
| `index.js` | Barrel exports |

#### `src/contexts/`
React Context providers for global state.

| File | Purpose |
|------|---------|
| `AuthContext.jsx` | Authentication state and methods |
| `index.js` | Barrel exports |

#### `src/utils/`
Reusable utility functions.

| File | Purpose |
|------|---------|
| `jwtDecode.js` | Token decoding, role extraction, expiry check |
| `localStorage.js` | Token and user persistence helpers |
| `index.js` | Barrel exports |

#### `src/routes/`
Routing configuration and route guards.

| File | Purpose |
|------|---------|
| `AppRouter.jsx` | Route definitions and structure |
| `PrivateRoute.jsx` | Auth + role guard for protected routes |
| `PublicRoute.jsx` | Redirect guard for public routes |
| `index.js` | Barrel exports |

#### `src/layouts/`
Layout components for page structure.

```
layouts/
├── MainLayout.jsx       # Primary layout with sider and header
├── components/
│   ├── AppSider.jsx     # Navigation sidebar
│   ├── AppHeader.jsx    # Top header bar
│   └── index.js         # Component exports
├── config/
│   ├── menuItems.jsx    # Menu configuration
│   └── index.js         # Config exports
└── index.js             # Layout exports
```

#### `src/pages/`
Page-level components corresponding to routes.

```
pages/
├── Auth/
│   └── LoginPage.jsx    # Login page
├── Dashboard/
│   └── Dashboard.jsx    # Main dashboard
├── Error/
│   └── ErrorPage.jsx    # Error boundary page
├── Product/
│   ├── ProductPage.jsx       # Product listing
│   ├── ProductDetailPage.jsx # Single product view/edit
│   ├── CategoryPage.jsx      # Category management
│   ├── ColorPage.jsx         # Color management
│   └── SizePage.jsx          # Size management
└── index.js             # Barrel exports
```

#### `src/features/`
Feature modules containing domain-specific logic and components.

```
features/
├── auth/                # Authentication feature
│   ├── api/             # Auth API functions
│   ├── hooks/           # Auth hooks (useLogin)
│   ├── components/      # Auth UI components
│   └── index.js
├── product/             # Product management feature
│   ├── api/             # Product API functions
│   │   ├── product.api.js
│   │   ├── category.api.js
│   │   ├── color.api.js
│   │   ├── size.api.js
│   │   ├── productVariant.api.js
│   │   ├── productColor.api.js
│   │   ├── productImage.api.js
│   │   └── index.js
│   ├── hooks/           # Product hooks (CRUD for each entity)
│   │   ├── category/
│   │   ├── color/
│   │   ├── size/
│   │   ├── product/
│   │   ├── productVariant/
│   │   ├── productColor/
│   │   ├── productImages/
│   │   └── index.js
│   ├── components/      # Product UI components
│   │   ├── category/
│   │   ├── color/
│   │   ├── size/
│   │   ├── product/
│   │   ├── productVariant/
│   │   ├── productImage/
│   │   └── index.js
│   └── index.js
└── index.js             # All feature exports
```

## Import Convention

The project uses barrel exports (`index.js` files) to provide clean import paths:

```javascript
// Instead of deep imports:
import { useLogin } from '../features/auth/hooks/auth/useLogin';

// Use barrel imports:
import { useLogin } from '../features/auth';
```

## Related Documentation

- [Architecture Overview](./architecture.md) - Design philosophy
- [Features Guide](./features/README.md) - Feature module details
