# Environment Configuration

This document describes the environment configuration for the ZTino Manager application.

## Environment Variables

The application uses Vite's built-in environment variable system. Variables must be prefixed with `VITE_` to be exposed to the client.

### Required Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `VITE_API_URL` | Backend API base URL | `http://localhost:<BACKEND_PORT>/api/<API_VERSION>` |

## Configuration Files

### Development

Create a `.env` file in the project root:

```env
VITE_API_URL=http://localhost:<BACKEND_PORT>/api/<API_VERSION>
```

### Production

Create a `.env.production` file for production builds:

```env
VITE_API_URL=https://<BACKEND_DOMAIN>/api/<API_VERSION>
```

## File Priority

Vite loads environment files in this order (later overrides earlier):

1. `.env` - Always loaded
2. `.env.local` - Local overrides (git-ignored)
3. `.env.[mode]` - Mode-specific (development, production)
4. `.env.[mode].local` - Mode-specific local overrides

## Accessing Variables

Environment variables are accessed via `import.meta.env`:

```javascript
const API_URL = import.meta.env.VITE_API_URL;
```

## Security Notes

- Never commit `.env` files with sensitive data
- The `.gitignore` excludes `.env` files
- Only `VITE_` prefixed variables are exposed to the client bundle
- Sensitive backend secrets should never be in frontend environment variables

## Build Time vs Runtime

Environment variables are **embedded at build time**:

- Values are replaced during `vite build`
- Cannot be changed without rebuilding
- For dynamic configuration, consider fetching from backend

## Related Documentation

- [Development Guide](./development-guide.md) - Local setup
- [API Communication](./api-communication.md) - How API URL is used
