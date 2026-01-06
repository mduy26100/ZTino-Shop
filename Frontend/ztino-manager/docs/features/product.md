# Product Feature

The product feature provides comprehensive product management capabilities including categories, colors, sizes, variants, and images.

## Location

`src/features/product/`

## Structure

```
product/
├── api/
│   ├── product.api.js
│   ├── category.api.js
│   ├── color.api.js
│   ├── size.api.js
│   ├── productVariant.api.js
│   ├── productColor.api.js
│   ├── productImage.api.js
│   └── index.js
├── hooks/
│   ├── product/
│   ├── category/
│   ├── color/
│   ├── size/
│   ├── productVariant/
│   ├── productColor/
│   ├── productImages/
│   └── index.js
├── components/
│   ├── product/
│   ├── category/
│   ├── color/
│   ├── size/
│   ├── productVariant/
│   ├── productImage/
│   └── index.js
└── index.js
```

## Entities

### Product

Main product entity with basic information.

**API (`product.api.js`):**
| Function | Endpoint | Description |
|----------|----------|-------------|
| `getProducts()` | GET /products | List all products |
| `getProductDetailById(id)` | GET /products/:id | Get product with details |
| `createProduct(data)` | POST /admin/products | Create product (FormData) |
| `updateProduct(data)` | PUT /admin/products/:id | Update product (FormData) |
| `deleteProduct(id)` | DELETE /admin/products/:id | Delete product |

**Hooks:**
- `useGetProducts` - Fetch all products
- `useGetProductDetailById` - Fetch single product
- `useCreateProduct` - Create product mutation
- `useUpdateProduct` - Update product mutation
- `useDeleteProduct` - Delete product mutation

**Components:**
- `ProductTable` - Product listing table
- `UpsertProductModal` - Create/edit product modal
- `ProductOverview` - Product details display

---

### Category

Hierarchical product categories.

**API (`category.api.js`):**
| Function | Endpoint | Description |
|----------|----------|-------------|
| `getCategories()` | GET /categories | List all categories |
| `createCategory(data)` | POST /admin/categories | Create category |
| `updateCategory(data)` | PUT /admin/categories/:id | Update category |
| `deleteCategory(id)` | DELETE /admin/categories/:id | Delete category |

**Hooks:**
- `useGetCategories`
- `useCreateCategory`
- `useUpdateCategory`
- `useDeleteCategory`

**Components:**
- `CategoryTable` - Category listing with hierarchy
- `UpsertCategoryModal` - Create/edit category modal

---

### Color

Product color options.

**API (`color.api.js`):**
| Function | Endpoint | Description |
|----------|----------|-------------|
| `getColors()` | GET /colors | List all colors |
| `createColor(data)` | POST /admin/colors | Create color |
| `updateColor(data)` | PUT /admin/colors/:id | Update color |
| `deleteColor(id)` | DELETE /admin/colors/:id | Delete color |

**Hooks:**
- `useGetColors`
- `useCreateColor`
- `useUpdateColor`
- `useDeleteColor`

**Components:**
- `ColorCard` - Color display card
- `UpsertColorModal` - Create/edit color modal

---

### Size

Product size options.

**API (`size.api.js`):**
| Function | Endpoint | Description |
|----------|----------|-------------|
| `getSizes()` | GET /sizes | List all sizes |
| `createSize(data)` | POST /admin/sizes | Create size |
| `updateSize(data)` | PUT /admin/sizes/:id | Update size |
| `deleteSize(id)` | DELETE /admin/sizes/:id | Delete size |

**Hooks:**
- `useGetSizes`
- `useCreateSize`
- `useUpdateSize`
- `useDeleteSize`

**Components:**
- `SizeCard` - Size display card
- `UpsertSizeModal` - Create/edit size modal

---

### Product Variant

Color-size combinations for a product.

**API (`productVariant.api.js`):**
| Function | Endpoint | Description |
|----------|----------|-------------|
| `createProductVariant(data)` | POST /admin/product-variants | Create variant |
| `updateProductVariant(data)` | PUT /admin/product-variants/:id | Update variant |
| `deleteProductVariant(id)` | DELETE /admin/product-variants/:id | Delete variant |

**Hooks:**
- `useCreateProductVariant`
- `useUpdateProductVariant`
- `useDeleteProductVariant`

**Components:**
- `VariantTable` - Variant listing table
- `UpsertProductVariantModal` - Create/edit variant modal

---

### Product Color

Product-specific color associations.

**API (`productColor.api.js`):**
| Function | Endpoint | Description |
|----------|----------|-------------|
| `getColorsByProductId(id)` | GET /admin/product-colors/:id | Get colors for product |
| `createProductColor(data)` | POST /admin/product-colors | Add color to product |
| `deleteProductColor(id)` | DELETE /admin/product-colors/:id | Remove color from product |

**Hooks:**
- `useGetColorsByProductId`
- `useCreateProductColor`
- `useDeleteProductColor`

---

### Product Images

Product image management.

**API (`productImage.api.js`):**
| Function | Endpoint | Description |
|----------|----------|-------------|
| `getProductImages(colorId)` | GET /admin/product-images | Get images by color |
| `createProductImages(data)` | POST /admin/product-images | Upload images |
| `updateProductImage(data)` | PUT /admin/product-images/:id | Update image |
| `deleteProductImage(id)` | DELETE /admin/product-images/:id | Delete image |

**Hooks:**
- `useGetProductImages`
- `useCreateProductImages`
- `useUpdateProductImage`
- `useDeleteProductImage`

**Components:**
- `ProductImageModal` - Image management modal

## Pages Using This Feature

| Page | Path | Description |
|------|------|-------------|
| ProductPage | `/products` | Product listing |
| ProductDetailPage | `/products/:id` | Product details and editing |
| CategoryPage | `/categories` | Category management |
| ColorPage | `/colors` | Color management |
| SizePage | `/sizes` | Size management |

## Related Documentation

- [API Communication](../api-communication.md) - API layer details
- [Folder Structure](../folder-structure.md) - File locations
