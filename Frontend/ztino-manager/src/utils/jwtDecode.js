import { jwtDecode } from "jwt-decode"

export const decodeToken = (token) => {
  try {
    return jwtDecode(token)
  } catch {
    return null
  }
}

export const getRolesFromToken = (token) => {
  const decoded = decodeToken(token)
  if (!decoded) return []

  const roles =
    decoded.role ??
    decoded.roles ??
    decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ??
    []

  return Array.isArray(roles) ? roles : [roles]
}

export const hasRole = (token, roleName) => {
  return getRolesFromToken(token).includes(roleName)
}

export const isTokenExpired = (token) => {
  const decoded = decodeToken(token)
  if (!decoded?.exp) return true

  return decoded.exp * 1000 < Date.now()
}
