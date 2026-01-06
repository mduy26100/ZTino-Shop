# Routing

This document describes the routing configuration and navigation patterns used in the application.

## Overview

The application uses **React Router v6** with the data router pattern (`createBrowserRouter`). All routes are defined centrally in `src/routes/AppRouter.jsx`.

## Route Structure

| Path | Page | Layout | Guard | Description |
|------|------|--------|-------|-------------|
| `/` | `HomePage` | MainLayout | - | Landing page |
| `/login` | `LoginPage` | MainLayout | PublicRoute | User login |
| `/register` | `RegisterPage` | MainLayout | PublicRoute | User registration |
| `/products/:slug?` | `ProductListingPage` | MainLayout | - | Category product grid |
| `/product/:slug?` | `ProductDetailPage` | MainLayout | - | Single product details |
| `/cart` | `CartPage` | MainLayout | - | Shopping cart |

## Route Configuration

Routes are defined using `createRoutesFromElements` for JSX-based route definition:

```
AppRouter.jsx
├── MainLayout (parent, errorElement: ErrorPage)
│   ├── index -> HomePage
│   ├── /login -> PublicRoute -> LoginPage
│   ├── /register -> PublicRoute -> RegisterPage
│   ├── /products/:slug? -> ProductListingPage
│   ├── /product/:slug? -> ProductDetailPage
│   └── /cart -> CartPage
```

## Route Guards

### PublicRoute
**Location**: `src/routes/PublicRoute.jsx`

**Purpose**: Prevents authenticated users from accessing login/register pages.

**Behavior**: If user is authenticated, redirects to home page (`/`). Otherwise, renders children.

### PrivateRoute
**Location**: `src/routes/PrivateRoute.jsx`

**Purpose**: Protects routes that require authentication.

**Behavior**: If user is not authenticated, redirects to login page. Otherwise, renders children.

> **Note**: Currently, `PrivateRoute` is defined but not actively applied to any routes since cart and product pages are accessible to both guests and authenticated users.

## Dynamic Route Parameters

### `:slug` Parameter
Used for both product listing and product detail pages:

- **`/products/:slug?`**: Optional category slug for filtering products
- **`/product/:slug?`**: Product slug for detail view

Example URLs:
- `/products/shirts` - Products in "shirts" category
- `/product/blue-cotton-shirt` - Detail page for specific product

## Navigation Patterns

### Programmatic Navigation
Use the `useNavigate` hook from React Router:
```javascript
const navigate = useNavigate();
navigate('/cart');
```

### Link-based Navigation
Use the `Link` component:
```javascript
<Link to="/products/shoes">Shop Shoes</Link>
```

### Active Link Styling
The `AppHeader` component uses `useLocation` to determine the current path and apply active styles to navigation items.

## Layout System

All routes are wrapped by `MainLayout`, which provides:
- **AppHeader**: Navigation bar with logo, menu, search, and cart
- **Content**: Uses `Outlet` to render child routes
- **AppFooter**: Site footer with links and information
- **FloatButton.BackTop**: Scroll-to-top functionality

The layout also handles scroll restoration on route changes.

## Error Handling

Error boundary is configured at the layout level:
- **`errorElement`**: Renders `ErrorPage` for unhandled errors
- Location: `src/pages/Error/ErrorPage.jsx`

## Key Files

| File | Purpose |
|------|---------|
| `src/routes/AppRouter.jsx` | Main route definitions |
| `src/routes/PublicRoute.jsx` | Guard for unauthenticated routes |
| `src/routes/PrivateRoute.jsx` | Guard for authenticated routes |
| `src/routes/index.js` | Barrel export |

## Related Documentation

- [Architecture](./architecture.md) - Overall system design
- [State Management](./state-management.md) - AuthContext for route guards
