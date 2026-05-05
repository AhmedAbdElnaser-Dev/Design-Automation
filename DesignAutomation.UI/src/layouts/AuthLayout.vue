<script setup lang="ts">
</script>

<template>
  <v-app>
    <v-main class="auth-main">
      <v-container class="fill-height pa-0" fluid>
        <v-row class="fill-height ma-0" no-gutters>
          <v-col cols="12" md="6" class="auth-hero d-none d-md-flex align-center justify-center">
            <div class="text-center px-8">
              <v-icon icon="mdi-cube-outline" size="96" color="white" class="mb-4" />
              <h1 class="text-h3 text-white font-weight-bold mb-2">Design Automation</h1>
              <p class="text-subtitle-1 text-white opacity-80">Autodesk Platform Services workspace</p>
            </div>
          </v-col>
          <v-col cols="12" md="6" class="d-flex align-center justify-center pa-6">
            <router-view />
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<style scoped>
.auth-hero {
  background: linear-gradient(135deg, #1976d2 0%, #0d47a1 100%);
}
.auth-main {
  background-color: #f5f7fa;
}
</style>
