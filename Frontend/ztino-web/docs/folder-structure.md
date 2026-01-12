# Folder Structure

This document provides a detailed breakdown of the project directory structure and explains the purpose of each folder and key files.

## Root Directory

```
ztino-web/
├── src/                    # Application source code
├── docs/                   # Technical documentation
├── public/                 # Static assets served as-is
├── node_modules/           # Dependencies (git-ignored)
├── .env                    # Environment variables (git-ignored)
├── .gitignore              # Git ignore rules
├── package.json            # Dependencies and scripts
├── vite.config.js          # Vite build configuration
├── tailwind.config.js      # Tailwind CSS configuration
├── postcss.config.js       # PostCSS configuration
├── eslint.config.js        # ESLint configuration
└── index.html              # HTML entry point
```

## Source Directory (`src/`)

### `src/main.jsx`
Application entry point that renders the root component.

### `src/App.jsx`
Root component that composes the application provider hierarchy and router.

### `src/config/`
Application-wide configuration.

| File | Purpose |
|------|---------|
| `axiosClient.js` | Pre-configured Axios instance with request/response interceptors |
| `index.js` | Barrel export |

### `src/constants/`
Application constants and configuration values.

| File | Purpose |
|------|---------|
| `apiEndpoints.js` | Backend API endpoint paths |
| `index.js` | Barrel export |

### `src/contexts/`
React Context providers for global state.

| File | Purpose |
|------|---------|
| `AuthContext.jsx` | Authentication state provider (user, login, logout, hasRole) |
| `index.js` | Barrel export |

### `src/utils/`
Utility functions and helpers.

| File | Purpose |
|------|---------|
| `localStorage.js` | Token, user, and guest cart ID persistence |
| `jwtDecode.js` | JWT decoding, role extraction, expiration check |
| `index.js` | Barrel export |

### `src/hooks/`
Custom hook utilities and base hooks.

| Subdirectory | Purpose |
|--------------|---------|
| `utils/` | Base hooks for queries, mutations, and cache management |

#### `src/hooks/utils/`

| File | Purpose | Exports |
|------|---------|---------|
| `useMutation.js` | Mutation operations with loading/error state | `useMutation` |
| `useQuery.js` | Queries with caching and abort control | `useQuery`, `invalidateCache`, `clearGlobalCache` |
| `invalidateCartCache.js` | Cart-specific cache invalidation helper | `invalidateCartCacheByAuth` |
| `index.js` | Barrel exports | All utilities |

### `src/routes/`
Routing configuration and guards.

| File | Purpose |
|------|---------|
| `AppRouter.jsx` | Main router with all route definitions |
| `PrivateRoute.jsx` | Guard for authenticated-only routes |
| `PublicRoute.jsx` | Guard for non-authenticated routes (login, register) |
| `index.js` | Barrel export |

### `src/layouts/`
Page layout components.

```
layouts/
├── MainLayout.jsx          # Primary layout with header, content, footer
├── components/
│   ├── AppHeader.jsx       # Navigation header with menu and cart
│   ├── AppFooter.jsx       # Site footer
│   └── index.js            # Component exports
└── index.js                # Barrel export
```

### `src/pages/`
Route-level page components organized by domain.

```
pages/
├── Auth/
│   ├── LoginPage.jsx       # User login form
│   └── RegisterPage.jsx    # User registration form
├── Cart/
│   └── CartPage.jsx        # Shopping cart view
├── Error/
│   └── ErrorPage.jsx       # Error boundary fallback
├── Home/
│   └── HomePage.jsx        # Landing page
├── Product/
│   ├── ProductListingPage.jsx   # Category product grid
│   └── ProductDetailPage.jsx    # Single product view
└── index.js                # Barrel export
```

### `src/features/`
Feature modules following domain-driven structure.

```
features/
├── auth/                   # Authentication feature
│   ├── api/
│   │   ├── auth.api.js     # Login, register API calls
│   │   └── index.js
│   ├── hooks/
│   │   ├── auth/
│   │   │   ├── useLogin.js
│   │   │   └── useRegister.js
│   │   └── index.js
│   ├── components/
│   │   ├── auth/           # Login/register form components
│   │   └── profile/        # Profile-related components
│   └── index.js
│
├── cart/                   # Shopping cart feature
│   ├── api/
│   │   ├── cart.api.js     # Cart CRUD operations
│   │   └── index.js
│   ├── hooks/
│   │   ├── useGetMyCart.js
│   │   ├── useGetCartById.js
│   │   ├── useCreateCart.js
│   │   └── index.js
│   ├── components/
│   │   ├── CartItemCard.jsx
│   │   ├── CartItemList.jsx
│   │   ├── CartSummary.jsx
│   │   └── index.js
│   └── index.js
│
├── home/                   # Homepage feature
│   ├── components/
│   │   ├── HeroSection.jsx
│   │   ├── CategorySection.jsx
│   │   ├── NewArrivalsSection.jsx
│   │   ├── EditorialSection.jsx
│   │   ├── ServicesSection.jsx
│   │   ├── ProductCard.jsx
│   │   └── index.js
│   └── index.js
│
├── product/                # Product catalog feature
│   ├── api/
│   │   ├── product.api.js  # Product fetch APIs
│   │   ├── category.api.js # Category fetch API
│   │   └── index.js
│   ├── hooks/
│   │   ├── product/
│   │   │   ├── useGetProductsByCategoryId.js
│   │   │   └── useGetProductDetailBySlug.js
│   │   ├── category/
│   │   │   └── useGetCategories.js
│   │   └── index.js
│   ├── components/
│   │   ├── product/
│   │   │   ├── ProductCard.jsx
│   │   │   ├── ProductFilter.jsx
│   │   │   ├── ProductGallery.jsx
│   │   │   ├── ProductGrid.jsx
│   │   │   └── ProductInfo.jsx
│   │   └── index.js
│   └── index.js
│
└── index.js                # Feature barrel export
```

### `src/assets/`
Static assets imported into the application (images, fonts, etc.).

## Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| Components | PascalCase | `ProductCard.jsx` |
| Hooks | camelCase with `use` prefix | `useGetCategories.js` |
| API functions | camelCase | `getProductDetailBySlug` |
| Utility files | camelCase | `localStorage.js` |
| Barrel files | `index.js` | All directories |

## Related Documentation

- [Architecture](./architecture.md) - System design overview
- [Development Guide](./development-guide.md) - Coding conventions
