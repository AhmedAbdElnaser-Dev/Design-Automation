<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const router = useRouter()
const drawer = ref(true)
const rail = ref(false)

const initials = computed(() => {
  const name = auth.user?.fullName ?? auth.user?.userName ?? auth.user?.email ?? '?'
  return name.split(/\s+/).map(p => p.charAt(0)).slice(0, 2).join('').toUpperCase()
})

const displayName = computed(() => auth.user?.fullName ?? auth.user?.userName ?? auth.user?.email ?? '')

const navSections = [
  {
    title: 'Workspace',
    items: [
      { name: 'home', icon: 'mdi-view-dashboard-outline', label: 'Dashboard' },
      { name: 'home', icon: 'mdi-cube-outline', label: 'Buckets' },
      { name: 'home', icon: 'mdi-file-document-outline', label: 'Models' },
      { name: 'home', icon: 'mdi-cog-sync-outline', label: 'Workflows' },
    ],
  },
  {
    title: 'Manage',
    items: [
      { name: 'home', icon: 'mdi-account-group-outline', label: 'Team' },
      { name: 'home', icon: 'mdi-tune', label: 'Settings' },
    ],
  },
]

function logout() {
  auth.logout()
  router.replace({ name: 'login' })
}
</script>

<template>
  <v-app>
    <v-navigation-drawer
      v-model="drawer"
      :rail="rail"
      permanent
      width="260"
      class="app-drawer"
    >
      <div class="app-brand" :class="{ 'app-brand--rail': rail }">
        <div class="app-brand__mark">
          <v-icon icon="mdi-cube-outline" size="20" color="white" />
        </div>
        <span v-if="!rail" class="app-brand__name">Design Automation</span>
      </div>

      <div class="app-nav">
        <template v-for="(section, idx) in navSections" :key="idx">
          <div v-if="!rail" class="app-nav__heading">{{ section.title }}</div>
          <v-list nav class="app-nav__list">
            <v-list-item
              v-for="item in section.items"
              :key="item.label"
              :to="{ name: item.name }"
              :prepend-icon="item.icon"
              :title="rail ? '' : item.label"
              rounded="lg"
              color="primary"
              class="app-nav__item"
            />
          </v-list>
        </template>
      </div>

      <template #append>
        <div class="app-drawer__foot">
          <v-btn
            :icon="rail ? 'mdi-chevron-right' : 'mdi-chevron-left'"
            variant="text"
            size="small"
            @click="rail = !rail"
          />
        </div>
      </template>
    </v-navigation-drawer>

    <v-app-bar
      color="surface"
      height="64"
      class="app-bar"
      elevation="0"
      flat
    >
      <v-app-bar-nav-icon class="d-md-none" @click="drawer = !drawer" />

      <div class="app-bar__search d-none d-md-flex">
        <v-icon icon="mdi-magnify" size="18" />
        <input type="text" placeholder="Search…" />
        <kbd class="app-bar__kbd">⌘K</kbd>
      </div>

      <v-spacer />

      <v-btn icon="mdi-bell-outline" variant="text" size="small" />
      <v-btn icon="mdi-help-circle-outline" variant="text" size="small" class="me-2" />

      <v-menu offset="8">
        <template #activator="{ props }">
          <button class="user-chip" v-bind="props">
            <v-avatar size="32" class="user-chip__avatar">
              <span>{{ initials }}</span>
            </v-avatar>
            <div class="user-chip__meta d-none d-sm-flex">
              <span class="user-chip__name">{{ displayName }}</span>
              <span class="user-chip__role">{{ auth.user?.roles?.[0] ?? 'Member' }}</span>
            </div>
            <v-icon icon="mdi-chevron-down" size="16" class="d-none d-sm-flex" />
          </button>
        </template>

        <v-list min-width="240" rounded="lg" class="pa-1">
          <v-list-item
            :title="displayName"
            :subtitle="auth.user?.email ?? ''"
            class="mb-1"
          >
            <template #prepend>
              <v-avatar color="primary" size="36">
                <span class="text-white font-weight-bold">{{ initials }}</span>
              </v-avatar>
            </template>
          </v-list-item>
          <v-divider class="my-1" />
          <v-list-item prepend-icon="mdi-account-circle-outline" title="Profile" rounded="lg" />
          <v-list-item prepend-icon="mdi-tune" title="Settings" rounded="lg" />
          <v-divider class="my-1" />
          <v-list-item
            prepend-icon="mdi-logout"
            title="Sign out"
            base-color="error"
            rounded="lg"
            @click="logout"
          />
        </v-list>
      </v-menu>
    </v-app-bar>

    <v-main class="app-main">
      <div class="app-content">
        <router-view v-slot="{ Component }">
          <transition name="page-fade" mode="out-in">
            <component :is="Component" />
          </transition>
        </router-view>
      </div>
    </v-main>
  </v-app>
</template>

<style scoped>
.app-drawer {
  background-color: rgb(var(--v-theme-surface)) !important;
  border-right: 1px solid rgb(var(--v-theme-surface-variant)) !important;
}

.app-brand {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 18px 20px;
  border-bottom: 1px solid rgb(var(--v-theme-surface-variant));
  height: 64px;
}
.app-brand--rail {
  justify-content: center;
  padding: 18px 0;
}
.app-brand__mark {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  background: linear-gradient(135deg, #4F46E5, #312E81);
  display: grid;
  place-items: center;
  flex-shrink: 0;
}
.app-brand__name {
  font-weight: 600;
  font-size: 15px;
  letter-spacing: -0.01em;
}

.app-nav {
  padding: 16px 12px;
}
.app-nav__heading {
  font-size: 11px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  color: rgb(var(--v-theme-on-surface-variant));
  padding: 12px 12px 6px;
}
.app-nav__list {
  background: transparent;
  padding: 0;
}
.app-nav__item {
  margin-bottom: 2px;
  min-height: 38px !important;
  font-weight: 500;
  font-size: 14px;
}

.app-drawer__foot {
  padding: 8px;
  border-top: 1px solid rgb(var(--v-theme-surface-variant));
  display: flex;
  justify-content: flex-end;
}

.app-bar {
  border-bottom: 1px solid rgb(var(--v-theme-surface-variant)) !important;
}

.app-bar__search {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgb(var(--v-theme-surface-light));
  border: 1px solid rgb(var(--v-theme-surface-variant));
  border-radius: 10px;
  padding: 0 12px;
  min-width: 280px;
  height: 36px;
  margin-left: 8px;
  transition: border-color 0.15s ease, background 0.15s ease;
}
.app-bar__search:focus-within {
  border-color: rgb(var(--v-theme-primary));
  background: rgb(var(--v-theme-surface));
}
.app-bar__search input {
  flex: 1;
  border: 0;
  background: transparent;
  outline: none;
  font: 500 13.5px var(--font-sans);
  color: rgb(var(--v-theme-on-surface));
}
.app-bar__search input::placeholder {
  color: rgb(var(--v-theme-on-surface-variant));
  font-weight: 400;
}
.app-bar__kbd {
  font: 500 11px var(--font-mono);
  background: rgb(var(--v-theme-surface));
  color: rgb(var(--v-theme-on-surface-variant));
  border: 1px solid rgb(var(--v-theme-surface-variant));
  padding: 2px 6px;
  border-radius: 4px;
}

.user-chip {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 4px 10px 4px 4px;
  border-radius: 999px;
  border: 1px solid transparent;
  background: transparent;
  cursor: pointer;
  transition: background 0.15s ease, border-color 0.15s ease;
}
.user-chip:hover {
  background: rgb(var(--v-theme-surface-light));
  border-color: rgb(var(--v-theme-surface-variant));
}
.user-chip__avatar {
  background: linear-gradient(135deg, #4F46E5, #818CF8);
  color: #fff;
  font-weight: 600;
  font-size: 13px;
}
.user-chip__meta {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  line-height: 1.2;
}
.user-chip__name {
  font-size: 13px;
  font-weight: 600;
  color: rgb(var(--v-theme-on-surface));
}
.user-chip__role {
  font-size: 11px;
  color: rgb(var(--v-theme-on-surface-variant));
}

.app-main {
  background-color: rgb(var(--v-theme-background)) !important;
}
.app-content {
  padding: 28px 32px;
  max-width: 1440px;
  margin: 0 auto;
}
@media (max-width: 600px) {
  .app-content { padding: 20px 16px; }
}

.page-fade-enter-active,
.page-fade-leave-active {
  transition: opacity 0.18s ease, transform 0.18s ease;
}
.page-fade-enter-from { opacity: 0; transform: translateY(6px); }
.page-fade-leave-to { opacity: 0; }
</style>
