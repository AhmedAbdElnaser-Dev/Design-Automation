import type { User } from './user'

export interface LoginRequest {
  emailOrUserName: string
  password: string
}

export interface RegisterRequest {
  email: string
  password: string
  userName?: string
  fullName?: string
  phoneNumber?: string
}

export interface AuthResponse {
  token: string
  expiresAt: string
  user: User
}
