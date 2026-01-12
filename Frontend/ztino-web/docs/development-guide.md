# Development Guide

This document provides guidelines and best practices for developers working on the ZTino Web project.

## Getting Started

### Prerequisites

1. **Node.js**: Version 18 or higher
2. **npm**: Comes with Node.js
3. **IDE**: VS Code recommended with extensions:
   - ESLint
   - Tailwind CSS IntelliSense
   - ES7+ React/Redux/React-Native snippets

### Initial Setup

```bash
# Install dependencies
npm install

# Copy environment template
cp .env.example .env

# Start development server
npm run dev
```

## Code Organization

### Feature Module Structure

When creating a new feature, follow this structure:

```
src/features/new-feature/
├── api/
│   ├── feature.api.js    # API service functions
│   └── index.js          # Barrel export
├── hooks/
│   ├── useFeatureHook.js # Custom hooks
│   └── index.js          # Barrel export
├── components/
│   ├── FeatureComponent.jsx
│   └── index.js          # Barrel export
└── index.js              # Main barrel export
```

### Barrel File Pattern

Always create `index.js` files to re-export modules:

```javascript
// src/features/cart/hooks/index.js
export { useCreateCart } from './useCreateCart';
export { useGetMyCart } from './useGetMyCart';
export { useGetCartById } from './useGetCartById';
```

This enables cleaner imports:
```javascript
// Instead of
import { useGetMyCart } from '../features/cart/hooks/useGetMyCart';

// Use
import { useGetMyCart } from '../features/cart';
```

## Component Guidelines

### File Naming

| Type | Convention | Example |
|------|------------|---------|
| Components | PascalCase | `ProductCard.jsx` |
| Pages | PascalCase + `Page` suffix | `ProductDetailPage.jsx` |
| Hooks | camelCase + `use` prefix | `useGetProducts.js` |
| Utilities | camelCase | `localStorage.js` |
| API files | camelCase + `.api` suffix | `product.api.js` |

### Component Structure

```javascript
import React from 'react';
import PropTypes from 'prop-types';
// External imports first

import { useFeatureHook } from '../hooks';
// Internal imports

const ComponentName = ({ prop1, prop2 }) => {
  // Hooks at the top
  const { data, loading } = useFeatureHook();
  
  // Event handlers
  const handleClick = () => { ... };
  
  // Conditional rendering
  if (loading) return <Skeleton />;
  
  // Main render
  return (
    <div className="...">
      ...
    </div>
  );
};

ComponentName.propTypes = {
  prop1: PropTypes.string.isRequired,
  prop2: PropTypes.number,
};

export default ComponentName;
```

## Custom Hook Guidelines

### Using Base Hooks

All feature hooks **must** use base hooks from `src/hooks/utils/`.

#### Query Hooks

```javascript
import { useQuery } from '../../../hooks/utils';
import { getCategories } from '../api';

export const useGetCategories = () => {
    return useQuery('categories', getCategories, { 
        initialData: [] 
    });
};
```

#### Mutation Hooks

```javascript
import { useMutation } from '../../../hooks/utils';
import { createProduct } from '../api';

export const useCreateProduct = () => {
    const { mutate, isLoading, error } = useMutation(createProduct);
    return { create: mutate, isLoading, error };
};
```

#### Query with Dynamic Key

```javascript
export const useGetProductDetailBySlug = (slug) => {
    return useQuery(
        slug ? `product-${slug}` : null,  // null disables fetch
        () => getProductDetailBySlug(slug),
        { enabled: !!slug }
    );
};
```

#### Mutation with Cache Invalidation

```javascript
import { useMutation, invalidateCartCacheByAuth } from '../../../hooks/utils';
import { useAuth } from '../../../contexts';

export const useCreateCart = () => {
    const { isAuthenticated } = useAuth();
    
    const { mutate, isLoading } = useMutation(async (data) => {
        const response = await createCart(data);
        invalidateCartCacheByAuth(isAuthenticated, response?.cartId);
        return response;
    });
    
    return { create: mutate, isLoading };
};
```

### ❌ Anti-Pattern: Manual State Management

Do NOT manually implement loading/error state:

```javascript
// DON'T DO THIS
export const useGetProducts = () => {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    
    useEffect(() => {
        fetchProducts()
            .then(setData)
            .catch(setError)
            .finally(() => setLoading(false));
    }, []);
    
    return { data, loading, error };
};
```

### ✅ Correct Pattern: Base Hooks

```javascript
// DO THIS
export const useGetProducts = () => {
    return useQuery('products', fetchProducts, { initialData: [] });
};
```

## Styling Guidelines

### Tailwind CSS First

Use Tailwind utility classes as the primary styling method:

```jsx
<div className="flex items-center gap-4 p-4 bg-white rounded-lg shadow-md">
  <img className="w-16 h-16 object-cover rounded" src={image} alt="" />
  <div className="flex-1">
    <h3 className="text-lg font-semibold text-gray-900">{title}</h3>
    <p className="text-sm text-gray-500">{description}</p>
  </div>
</div>
```

### Ant Design Components

For complex UI components (forms, tables, modals), use Ant Design:

```jsx
import { Button, Form, Input, message } from 'antd';

const LoginForm = () => (
  <Form onFinish={handleSubmit}>
    <Form.Item name="email" rules={[{ required: true }]}>
      <Input placeholder="Email" />
    </Form.Item>
    <Button type="primary" htmlType="submit">
      Login
    </Button>
  </Form>
);
```

### CSS Modules (When Needed)

For complex component-specific styles, use CSS modules:

```jsx
import styles from './Component.module.css';

<div className={styles.customContainer}>...</div>
```

## API Integration

### Creating New API Endpoints

1. Add endpoint constant to `src/constants/apiEndpoints.js`
2. Create API function in feature's `api/` folder
3. Create custom hook to consume the API
4. Update barrel exports

### Error Handling

Always handle errors gracefully:

```jsx
const { data, loading, error } = useGetProducts();

if (error) {
  return (
    <Alert 
      type="error" 
      message={error.error?.message || 'Something went wrong'} 
    />
  );
}
```

## Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `VITE_API_URL` | Backend API base URL | `http://localhost:<BACKEND_PORT>/api/<API_VERSION>` |

Access in code:
```javascript
const apiUrl = import.meta.env.VITE_API_URL;
```

## Version Control

### Commit Message Convention

```
<type>: <description>

[optional body]
```

Types:
- `feat`: New feature
- `fix`: Bug fix
- `refactor`: Code refactoring
- `docs`: Documentation
- `style`: Formatting changes
- `chore`: Maintenance tasks

### Branch Naming

- `feature/feature-name` - New features
- `fix/bug-description` - Bug fixes
- `refactor/area-name` - Refactoring

## Available Scripts

| Command | Description |
|---------|-------------|
| `npm run dev` | Start dev server at `localhost:<ztino-web-port>` |
| `npm run build` | Build production bundle to `dist/` |
| `npm run preview` | Preview production build locally |
| `npm run lint` | Run ESLint checks |

## Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| API calls failing | Check `VITE_API_URL` in `.env` |
| Styles not applying | Clear browser cache, restart dev server |
| Module not found | Check barrel exports in `index.js` files |
| Auth not persisting | Check localStorage in browser DevTools |

## Related Documentation

- [Architecture](./architecture.md) - System design overview
- [Folder Structure](./folder-structure.md) - Directory organization
- [API Communication](./api-communication.md) - Backend integration
