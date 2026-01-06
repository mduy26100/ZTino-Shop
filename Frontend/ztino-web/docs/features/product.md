# Product Feature

This document describes the product catalog functionality including listing, filtering, and detail views.

## Overview

The product feature handles product browsing, category navigation, and product detail display.

## Structure

```
src/features/product/
├── api/
│   ├── product.api.js    # Product fetch operations
│   ├── category.api.js   # Category fetch operations
│   └── index.js
├── hooks/
│   ├── product/
│   │   ├── useGetProductsByCategoryId.js
│   │   └── useGetProductDetailBySlug.js
│   ├── category/
│   │   └── useGetCategories.js
│   └── index.js
├── components/
│   ├── product/
│   │   ├── ProductCard.jsx
│   │   ├── ProductFilter.jsx
│   │   ├── ProductGallery.jsx
│   │   ├── ProductGrid.jsx
│   │   └── ProductInfo.jsx
│   └── index.js
└── index.js
```

## API Functions

### Product API (`src/features/product/api/product.api.js`)

| Function | Method | Endpoint | Purpose |
|----------|--------|----------|---------|
| `getProductsByCategoryId(categoryId)` | GET | `/products/category/:id` | List products in category |
| `getProductDetailBySlug(slug)` | GET | `/products/:slug` | Get single product details |

### Category API (`src/features/product/api/category.api.js`)

| Function | Method | Endpoint | Purpose |
|----------|--------|----------|---------|
| `getCategories()` | GET | `/categories` | List all categories |

## Hooks

### useGetCategories

Fetches all product categories for navigation.

**Returns**:
- `categories` - Array of category objects
- `loading` - Loading state
- `error` - Error object if failed

### useGetProductsByCategoryId

Fetches products filtered by category.

**Input**: Category ID

**Returns**:
- `products` - Array of product objects
- `loading` - Loading state
- `error` - Error object if failed

### useGetProductDetailBySlug

Fetches complete product details for the detail page.

**Input**: Product slug

**Returns**:
- `product` - Product object with variants, colors, sizes, images
- `loading` - Loading state
- `error` - Error object if failed

## Components

### ProductCard

Product tile for grid display:
- Product image
- Product name
- Price display
- Link to detail page

Located in both `src/features/product/components/product/` and `src/features/home/components/` (for homepage usage)

### ProductGrid

Grid layout container for product cards:
- Responsive column layout
- Maps over products to render `ProductCard`
- Loading and empty states

### ProductFilter

Sidebar filter component:
- Category filter
- Price range filter
- Sort options

### ProductGallery

Product image gallery for detail page:
- Main image display
- Thumbnail navigation
- Image zoom functionality

### ProductInfo

Product details section:
- Name and description
- Price display
- Color selection
- Size selection
- Quantity selector
- Add to cart button

## Pages

### ProductListingPage (`src/pages/Product/ProductListingPage.jsx`)

Category product listing:
- Uses `:slug` param for category filtering
- `ProductFilter` sidebar
- `ProductGrid` for products
- Breadcrumb navigation

### ProductDetailPage (`src/pages/Product/ProductDetailPage.jsx`)

Single product view:
- Uses `:slug` param for product lookup
- `ProductGallery` for images
- `ProductInfo` for details and add-to-cart
- Related products section

## Data Model

### Product Object

```javascript
{
  id: string,
  name: string,
  slug: string,
  description: string,
  price: number,
  productColors: [
    {
      id: string,
      colorName: string,
      colorCode: string,
      images: [...],
      sizes: [
        {
          id: string,
          sizeName: string,
          stock: number
        }
      ]
    }
  ]
}
```

### Category Object

```javascript
{
  id: string,
  name: string,
  slug: string,
  parentId: string | null
}
```

## Related Documentation

- [Routing](../routing.md) - Product route parameters
- [API Communication](../api-communication.md) - HTTP patterns
