import axios from 'axios'

const TOKEN_KEY = 'da.auth.token'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '/api',
  headers: { 'Content-Type': 'application/json' },
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem(TOKEN_KEY)
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error?.response?.status === 401) {
      const { useAuthStore } = await import('@/stores/auth')
      const router = (await import('@/router')).default
      const auth = useAuthStore()
      auth.logout()
      if (router.currentRoute.value.name !== 'login') {
        router.replace({ name: 'login', query: { redirect: router.currentRoute.value.fullPath } })
      }
    }
    return Promise.reject(error)
  },
)

export default api
