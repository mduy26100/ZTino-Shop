# Development Guide

This guide covers setting up and working with the ZTino Manager project locally.

## Prerequisites

- **Node.js**: v18.0.0 or higher
- **npm**: v9.0.0 or higher (comes with Node.js)
- **Backend API**: The ZTino backend must be running

## Initial Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd ztino-manager
```

### 2. Install Dependencies

```bash
npm install
```

### 3. Configure Environment

Create a `.env` file in the project root:

```env
VITE_API_URL=http://localhost:<BACKEND_PORT>/api/<API_VERSION>
```

Replace the URL with your actual backend API address.

### 4. Start Development Server

```bash
npm run dev
```

The application will be available at `http://localhost:<ztino-manager-port>`

## Development Workflow

### Scripts

| Command | Description |
|---------|-------------|
| `npm run dev` | Start Vite dev server with HMR |
| `npm run build` | Build for production |
| `npm run preview` | Preview production build locally |
| `npm run lint` | Run ESLint |

### Hot Module Replacement (HMR)

The dev server supports HMR - changes to components, styles, and hooks will update in the browser without full page reload.

## Code Organization

### Adding a New Feature

1. Create feature folder: `src/features/[feature-name]/`
2. Add subfolders: `api/`, `hooks/`, `components/`
3. Create API functions in `api/[entity].api.js`
4. Create hooks in `hooks/[entity]/use[Action][Entity].js`
5. Create components in `components/[entity]/`
6. Export from `index.js` files at each level

### Adding a New Page

1. Create page component: `src/pages/[Domain]/[PageName].jsx`
2. Export from `src/pages/index.js`
3. Add route in `src/routes/AppRouter.jsx`
4. Add menu item in `src/layouts/config/menuItems.jsx` (if navigable)

### Creating a Custom Hook

Follow the naming convention:
- `useGet*` - for fetching data
- `useCreate*` - for creating entities
- `useUpdate*` - for updating entities
- `useDelete*` - for deleting entities

Each hook should manage:
- `isLoading` state
- `error` state
- Action function that calls the API

## Styling Guidelines

### TailwindCSS

Primary styling uses TailwindCSS utility classes:

```jsx
<div className="p-4 bg-white rounded-lg shadow-sm">
  <h1 className="text-xl font-bold text-gray-800">Title</h1>
</div>
```

### Ant Design

Use Ant Design components for complex UI elements:

```jsx
import { Button, Table, Modal, Form } from 'antd';
```

### Icons

Use Heroicons (outline style):

```jsx
import { TrashIcon } from '@heroicons/react/24/outline';
```

## Linting

The project uses ESLint with:
- React Hooks rules
- React Refresh rules

Run linting:

```bash
npm run lint
```

## Building for Production

```bash
npm run build
```

Output is generated in `dist/` folder.

Preview the build:

```bash
npm run preview
```

## Troubleshooting

### "Network Error" on API calls

- Verify `VITE_API_URL` in `.env` is correct
- Ensure backend server is running
- Check for CORS issues on backend

### "401 Unauthorized" redirect loop

- Clear browser localStorage
- Verify backend is returning valid JWT
- Check token format matches expected claims

### HMR not working

- Check console for compilation errors
- Try restarting dev server
- Clear browser cache

## Related Documentation

- [Architecture](./architecture.md) - Design patterns
- [Environment](./environment.md) - Configuration details
- [Folder Structure](./folder-structure.md) - Where things go
