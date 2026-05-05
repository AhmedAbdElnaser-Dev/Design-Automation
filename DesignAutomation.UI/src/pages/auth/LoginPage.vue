<script setup lang="ts">
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const router = useRouter()
const route = useRoute()

const emailOrUserName = ref('')
const password = ref('')
const showPassword = ref(false)
const formValid = ref(false)

const emailRules = [(v: string) => !!v || 'Email or username is required']
const passwordRules = [(v: string) => !!v || 'Password is required']

async function submit() {
  if (!formValid.value) return
  try {
    await auth.login({ emailOrUserName: emailOrUserName.value, password: password.value })
    const redirect = (route.query.redirect as string) || '/'
    router.replace(redirect)
  } catch {
    // Error is surfaced via auth.error
  }
}

function loginWithAutodesk() {
  const apiBase = import.meta.env.VITE_API_BASE_URL ?? '/api'
  const redirect = (route.query.redirect as string) || '/'
  window.location.href = `${apiBase}/auth/autodesk/login?redirect=${encodeURIComponent(redirect)}`
}
</script>

<template>
  <v-card class="pa-2 pa-sm-6 login-card" max-width="420" width="100%" elevation="2" rounded="lg">
    <v-card-title class="text-h5 font-weight-bold pb-1">Welcome back</v-card-title>
    <v-card-subtitle class="pb-4">Sign in to continue to Design Automation</v-card-subtitle>

    <v-card-text>
      <v-alert
        v-if="auth.error"
        type="error"
        variant="tonal"
        density="compact"
        class="mb-4"
        closable
        @click:close="auth.error = null"
      >
        {{ auth.error }}
      </v-alert>

      <v-form v-model="formValid" @submit.prevent="submit">
        <v-text-field
          v-model="emailOrUserName"
          label="Email or username"
          prepend-inner-icon="mdi-account-outline"
          :rules="emailRules"
          variant="outlined"
          density="comfortable"
          autocomplete="username"
          autofocus
        />
        <v-text-field
          v-model="password"
          label="Password"
          :type="showPassword ? 'text' : 'password'"
          prepend-inner-icon="mdi-lock-outline"
          :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
          @click:append-inner="showPassword = !showPassword"
          :rules="passwordRules"
          variant="outlined"
          density="comfortable"
          autocomplete="current-password"
          class="mt-2"
        />

        <v-btn
          type="submit"
          color="primary"
          size="large"
          block
          :loading="auth.loading"
          :disabled="!formValid || auth.loading"
          class="mt-2"
        >
          Sign in
        </v-btn>
      </v-form>

      <div class="d-flex align-center my-5">
        <v-divider />
        <span class="px-3 text-caption text-medium-emphasis">OR</span>
        <v-divider />
      </div>

      <v-btn
        size="large"
        block
        variant="outlined"
        prepend-icon="mdi-cube-outline"
        @click="loginWithAutodesk"
      >
        Continue with Autodesk
      </v-btn>
    </v-card-text>
  </v-card>
</template>

<style scoped>
.login-card {
  background-color: white;
}
</style>
