import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { authApi } from '@/services/api/auth'
import { usersApi } from '@/services/api/users'
import type { LoginRequest, RegisterRequest } from '@/services/types/auth'
import type { User } from '@/services/types/user'

const TOKEN_KEY = 'da.auth.token'
const EXPIRES_KEY = 'da.auth.expiresAt'
const USER_KEY = 'da.auth.user'

function readUser(): User | null {
  const raw = localStorage.getItem(USER_KEY)
  if (!raw) return null
  try { return JSON.parse(raw) as User } catch { return null }
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem(TOKEN_KEY))
  const expiresAt = ref<string | null>(localStorage.getItem(EXPIRES_KEY))
  const user = ref<User | null>(readUser())
  const loading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => {
    if (!token.value || !expiresAt.value) return false
    return new Date(expiresAt.value).getTime() > Date.now()
  })

  const isAdmin = computed(() => user.value?.roles?.includes('Admin') ?? false)

  function setSession(t: string, exp: string, u: User) {
    token.value = t
    expiresAt.value = exp
    user.value = u
    localStorage.setItem(TOKEN_KEY, t)
    localStorage.setItem(EXPIRES_KEY, exp)
    localStorage.setItem(USER_KEY, JSON.stringify(u))
  }

  function setToken(t: string, exp: string) {
    token.value = t
    expiresAt.value = exp
    localStorage.setItem(TOKEN_KEY, t)
    localStorage.setItem(EXPIRES_KEY, exp)
  }

  function clearSession() {
    token.value = null
    expiresAt.value = null
    user.value = null
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(EXPIRES_KEY)
    localStorage.removeItem(USER_KEY)
  }

  async function login(payload: LoginRequest) {
    loading.value = true
    error.value = null
    try {
      const res = await authApi.login(payload)
      setSession(res.token, res.expiresAt, res.user)
      return res
    } catch (e) {
      const msg = (e as { response?: { data?: { message?: string } } })?.response?.data?.message
      error.value = msg ?? 'Login failed.'
      throw e
    } finally {
      loading.value = false
    }
  }

  async function register(payload: RegisterRequest) {
    loading.value = true
    error.value = null
    try {
      const res = await authApi.register(payload)
      setSession(res.token, res.expiresAt, res.user)
      return res
    } catch (e) {
      const msg = (e as { response?: { data?: { message?: string } } })?.response?.data?.message
      error.value = msg ?? 'Registration failed.'
      throw e
    } finally {
      loading.value = false
    }
  }

  async function refreshUser() {
    if (!isAuthenticated.value) return
    const u = await usersApi.me()
    user.value = u
    localStorage.setItem(USER_KEY, JSON.stringify(u))
    return u
  }

  function logout() {
    clearSession()
  }

  return {
    token,
    expiresAt,
    user,
    loading,
    error,
    isAuthenticated,
    isAdmin,
    login,
    register,
    refreshUser,
    setToken,
    logout,
  }
})
