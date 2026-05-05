export interface User {
  id: string
  userName: string | null
  email: string | null
  fullName: string | null
  phoneNumber: string | null
  createdAt: string
  roles: string[]
}
