export const setToken = (token) => {
  localStorage.setItem("accessToken", token)
}

export const getToken = () => {
  return localStorage.getItem("accessToken")
}

export const removeToken = () => {
  localStorage.removeItem("accessToken")
}

export const setUser = (user) => {
  localStorage.setItem("user", JSON.stringify(user))
}

export const getUser = () => {
  const user = localStorage.getItem("user")
  return user ? JSON.parse(user) : null
}

export const removeUser = () => {
  localStorage.removeItem("user")
}

export const clearAuth = () => {
  removeToken()
  removeUser()
}

export const setGuestCartId = (cartId) => {
  localStorage.setItem("guestCartId", cartId)
}

export const getGuestCartId = () => {
  return localStorage.getItem("guestCartId")
}

export const removeGuestCartId = () => {
  localStorage.removeItem("guestCartId")
}
