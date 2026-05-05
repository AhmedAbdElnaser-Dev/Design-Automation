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
const remember = ref(true)

const emailRules = [(v: string) => !!v || 'Email or username is required']
const passwordRules = [(v: string) => !!v || 'Password is required']

async function submit() {
  if (!formValid.value) return
  try {
    await auth.login({ emailOrUserName: emailOrUserName.value, password: password.value })
    const redirect = (route.query.redirect as string) || '/'
    router.replace(redirect)
  } catch {
    // Error surfaced via auth.error
  }
}

function loginWithAutodesk() {
  const apiBase = import.meta.env.VITE_API_BASE_URL ?? '/api'
  const redirect = (route.query.redirect as string) || '/'
  window.location.href = `${apiBase}/auth/autodesk/login?redirect=${encodeURIComponent(redirect)}`
}
</script>

<template>
  <div class="login">
    <header class="login__head">
      <div class="login__brand">
        <div class="login__brand-mark">
          <v-icon icon="mdi-cube-outline" size="20" color="white" />
        </div>
        <span class="login__brand-name">Design Automation</span>
      </div>

      <h1 class="login__title">Welcome back</h1>
      <p class="login__sub">Sign in to your workspace to continue.</p>
    </header>

    <v-alert
      v-if="auth.error"
      type="error"
      variant="tonal"
      density="compact"
      class="mb-5"
      closable
      @click:close="auth.error = null"
    >
      {{ auth.error }}
    </v-alert>

    <v-form v-model="formValid" @submit.prevent="submit" class="login__form">
      <label class="field-label" for="login-email">Email or username</label>
      <v-text-field
        id="login-email"
        v-model="emailOrUserName"
        placeholder="you@company.com"
        prepend-inner-icon="mdi-account-outline"
        :rules="emailRules"
        autocomplete="username"
        autofocus
      />

      <div class="d-flex justify-space-between align-center mt-4 mb-1">
        <label class="field-label mb-0" for="login-password">Password</label>
        <a class="login__link" href="#" @click.prevent>Forgot password?</a>
      </div>
      <v-text-field
        id="login-password"
        v-model="password"
        :type="showPassword ? 'text' : 'password'"
        placeholder="Enter your password"
        prepend-inner-icon="mdi-lock-outline"
        :append-inner-icon="showPassword ? 'mdi-eye-off-outline' : 'mdi-eye-outline'"
        @click:append-inner="showPassword = !showPassword"
        :rules="passwordRules"
        autocomplete="current-password"
      />

      <v-checkbox
        v-model="remember"
        label="Keep me signed in"
        density="compact"
        hide-details
        color="primary"
        class="login__remember"
      />

      <v-btn
        type="submit"
        color="primary"
        size="large"
        block
        :loading="auth.loading"
        :disabled="!formValid || auth.loading"
        class="login__submit"
      >
        Sign in
        <template #append>
          <v-icon icon="mdi-arrow-right" />
        </template>
      </v-btn>
    </v-form>

    <div class="login__divider">
      <span>or continue with</span>
    </div>

    <v-btn
      size="large"
      block
      variant="outlined"
      class="login__autodesk"
      @click="loginWithAutodesk"
    >
      <span class="login__autodesk-mark">
        <v-icon icon="mdi-cube-outline" size="14" color="white" />
      </span>
      Autodesk Account
    </v-btn>

    <p class="login__foot">
      Don't have an account?
      <a class="login__link" href="#" @click.prevent>Talk to your admin</a>
    </p>
  </div>
</template>

<style scoped>
.login {
  display: flex;
  flex-direction: column;
}

.login__head {
  margin-bottom: 28px;
}

.login__brand {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 24px;
  font-weight: 600;
  letter-spacing: -0.01em;
  color: rgb(var(--v-theme-on-surface));
}
@media (min-width: 960px) {
  .login__brand { display: none; }
}
.login__brand-mark {
  width: 32px;
  height: 32px;
  border-radius: 9px;
  background: linear-gradient(135deg, #4F46E5, #312E81);
  display: grid;
  place-items: center;
  box-shadow: 0 4px 14px -6px rgba(79, 70, 229, 0.6);
}
.login__brand-name {
  font-size: 15px;
}

.login__title {
  font-size: 30px;
  line-height: 1.15;
  font-weight: 700;
  letter-spacing: -0.025em;
  color: rgb(var(--v-theme-on-surface));
  margin: 0 0 8px;
}
.login__sub {
  font-size: 14.5px;
  color: rgb(var(--v-theme-on-surface-variant));
  margin: 0;
}

.field-label {
  display: block;
  font-size: 13px;
  font-weight: 500;
  color: rgb(var(--v-theme-on-surface));
  margin-bottom: 6px;
  letter-spacing: 0;
}

.login__form :deep(.v-field) {
  border-radius: 10px;
  background-color: rgb(var(--v-theme-surface));
}
.login__form :deep(.v-field--variant-outlined .v-field__outline__start),
.login__form :deep(.v-field--variant-outlined .v-field__outline__end),
.login__form :deep(.v-field--variant-outlined .v-field__outline__notch::before),
.login__form :deep(.v-field--variant-outlined .v-field__outline__notch::after) {
  border-color: rgb(var(--v-theme-surface-variant));
}
.login__form :deep(.v-field__input) {
  font-size: 14.5px;
  min-height: 48px;
  padding-top: 12px;
  padding-bottom: 12px;
}
.login__form :deep(.v-field__prepend-inner > .v-icon),
.login__form :deep(.v-field__append-inner > .v-icon) {
  opacity: 0.6;
}

.login__remember {
  margin: 14px 0 6px -8px;
}
.login__remember :deep(.v-label) {
  font-size: 13.5px;
  opacity: 0.85;
}

.login__submit {
  font-weight: 600;
  letter-spacing: 0;
  height: 48px;
  margin-top: 4px;
  box-shadow: 0 4px 14px -6px rgba(79, 70, 229, 0.6) !important;
}
.login__submit:hover {
  box-shadow: 0 6px 20px -8px rgba(79, 70, 229, 0.7) !important;
}

.login__divider {
  display: flex;
  align-items: center;
  gap: 14px;
  margin: 24px 0;
  font-size: 11.5px;
  letter-spacing: 0.08em;
  color: rgb(var(--v-theme-on-surface-variant));
  text-transform: uppercase;
  font-weight: 500;
}
.login__divider::before,
.login__divider::after {
  content: '';
  flex: 1;
  height: 1px;
  background-color: rgb(var(--v-theme-surface-variant));
}

.login__autodesk {
  border-color: rgb(var(--v-theme-surface-variant));
  font-weight: 500;
  height: 48px;
  background-color: rgb(var(--v-theme-surface));
  transition: all 0.15s ease;
}
.login__autodesk:hover {
  border-color: rgb(var(--v-theme-primary));
  background-color: rgba(79, 70, 229, 0.04);
}
.login__autodesk-mark {
  display: inline-grid;
  place-items: center;
  width: 22px;
  height: 22px;
  border-radius: 6px;
  background: linear-gradient(135deg, #FF6B00, #E54B00);
  margin-right: 10px;
  box-shadow: 0 2px 6px -2px rgba(229, 75, 0, 0.5);
}

.login__link {
  color: rgb(var(--v-theme-primary));
  text-decoration: none;
  font-size: 13px;
  font-weight: 500;
  transition: color 0.15s ease;
}
.login__link:hover { color: rgb(var(--v-theme-primary-darken-1)); text-decoration: underline; }

.login__foot {
  text-align: center;
  margin: 24px 0 0;
  font-size: 13.5px;
  color: rgb(var(--v-theme-on-surface-variant));
}
</style>
