# Routing & Navigation

This document describes the routing system and navigation structure of the ZTino Manager application.

## Overview

The application uses **React Router DOM v6** with the `createBrowserRouter` API for declarative routing. Routes are protected by authentication and role-based guards.

## Route Structure

| Path | Page | Auth Required | Description |
|------|------|---------------|-------------|
| `/login` | LoginPage | No | Public login page |
| `/` | - | Yes | Redirects to `/dashboard` |
| `/dashboard` | Dashboard | Yes (Manager) | Main dashboard |
| `/products` | ProductPage | Yes (Manager) | Product listing |
| `/products/:id` | ProductDetailPage | Yes (Manager) | Product details/edit |
| `/categories` | CategoryPage | Yes (Manager) | Category management |
| `/colors` | ColorPage | Yes (Manager) | Color management |
| `/sizes` | SizePage | Yes (Manager) | Size management |

## Router Configuration

The router is defined in `src/routes/AppRouter.jsx`:

- Uses `createBrowserRouter` with `createRoutesFromElements`
- Nested routes under `/` share the `MainLayout`
- Error handling via `errorElement`

## Route Guards

### PrivateRoute

Location: `src/routes/PrivateRoute.jsx`

Protects routes that require authentication and Manager role:

1. Shows loading spinner while auth initializes
2. Redirects to `/login` if not authenticated
3. Redirects to `/login` if user lacks "Manager" role
4. Renders children if authorized

### PublicRoute

Location: `src/routes/PublicRoute.jsx`

Handles routes accessible only to unauthenticated users:

1. Shows loading spinner while auth initializes
2. Redirects to `/dashboard` if already authenticated
3. Renders children if not authenticated

## Layout Structure

Protected routes are wrapped in `MainLayout`:

```
MainLayout
├── AppSider (navigation)
├── AppHeader
├── Content
│   └── [Page Component via Outlet]
└── Footer
```

## Navigation Menu

The sidebar navigation is configured in `src/layouts/config/menuItems.jsx`:

| Menu Item | Path | Status |
|-----------|------|--------|
| Dashboard | `/dashboard` | Active |
| Products | (submenu) | Active |
| → Products | `/products` | Active |
| → Categories | `/categories` | Active |
| → Colors | `/colors` | Active |
| → Sizes | `/sizes` | Active |
| Orders | `/orders` | Coming Soon |
| Customers | `/users` | Coming Soon |
| Reports | `/reports` | Coming Soon |
| Settings | `/settings` | Coming Soon |

Menu items are filtered based on user roles via `getMenuItems(userRoles)`.

## Adding New Routes

To add a new route:

1. Create the page component in `src/pages/[Domain]/`
2. Export it from `src/pages/index.js`
3. Add the route in `src/routes/AppRouter.jsx`
4. Add menu item in `src/layouts/config/menuItems.jsx` (if navigable)

## Related Documentation

- [Authentication](./authentication.md) - Auth guards explained
- [Folder Structure](./folder-structure.md) - File locations
