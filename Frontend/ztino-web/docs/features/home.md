# Home Feature

This document describes the homepage components and layout.

## Overview

The home feature contains components that make up the landing page, showcasing products, categories, and promotional content.

## Structure

```
src/features/home/
├── components/
│   ├── HeroSection.jsx
│   ├── CategorySection.jsx
│   ├── NewArrivalsSection.jsx
│   ├── EditorialSection.jsx
│   ├── ServicesSection.jsx
│   ├── ProductCard.jsx
│   └── index.js
└── index.js
```

## Components

### HeroSection

**Purpose**: Main banner/hero area at the top of the homepage.

**Features**:
- Large hero image or carousel
- Marketing headline and tagline
- Call-to-action button(s)
- Responsive sizing

### CategorySection

**Purpose**: Showcase main product categories.

**Features**:
- Category cards with images
- Category names and descriptions
- Links to category listing pages
- Grid/carousel layout

### NewArrivalsSection

**Purpose**: Display recently added products.

**Features**:
- Product grid or carousel
- Uses `ProductCard` component
- "View All" link to product listing
- Limited product count for homepage

### EditorialSection

**Purpose**: Editorial/lifestyle content.

**Features**:
- Featured imagery
- Brand story or promotional content
- Visual storytelling elements

### ServicesSection

**Purpose**: Highlight service offerings.

**Features**:
- Service icons and descriptions
- Free shipping info
- Return policy
- Customer support info

### ProductCard

**Purpose**: Reusable product display card.

**Features**:
- Product thumbnail
- Product name
- Price display
- Hover effects
- Link to product detail

> **Note**: This is a simplified version for homepage use. The more detailed version is in `src/features/product/components/product/ProductCard.jsx`.

## Page

### HomePage (`src/pages/Home/HomePage.jsx`)

Composes all home feature components:

```
HomePage
├── HeroSection
├── CategorySection
├── NewArrivalsSection
├── EditorialSection
└── ServicesSection
```

## Layout Considerations

- Full-width sections with contained content
- Responsive breakpoints for mobile/tablet/desktop
- Consistent spacing between sections
- Uses both Tailwind CSS and Ant Design components

## Related Documentation

- [Product Feature](./product.md) - ProductCard details
- [Architecture](../architecture.md) - Component composition
