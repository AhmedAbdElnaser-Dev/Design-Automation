<script setup lang="ts">
</script>

<template>
  <v-app>
    <v-main class="auth-main">
      <div class="auth-shell">
        <aside class="auth-hero d-none d-md-flex">
          <div class="auth-hero__bg" aria-hidden="true">
            <span class="orb orb--a" />
            <span class="orb orb--b" />
            <span class="orb orb--c" />
            <div class="grid" />
          </div>

          <div class="auth-hero__content">
            <header class="hero-top">
              <span class="hero-brand">Design Automation</span>
            </header>

            <div class="hero-mid">
              <h1 class="hero-title">
                Build, automate, ship<br />
                <span class="hero-title__accent">Autodesk workflows.</span>
              </h1>
              <p class="hero-sub">
                Manage cloud storage, model derivatives, and processing jobs from a single workspace built for
                engineering teams.
              </p>
            </div>

            <footer class="hero-bot">
              <p class="hero-foot">© {{ new Date().getFullYear() }} Design Automation</p>
            </footer>
          </div>
        </aside>

        <section class="auth-form">
          <div class="auth-form__inner">
            <router-view v-slot="{ Component }">
              <transition name="fade-up" mode="out-in">
                <component :is="Component" />
              </transition>
            </router-view>
          </div>
        </section>
      </div>
    </v-main>
  </v-app>
</template>

<style scoped>
.auth-main {
  --hero-bg: #4F46E5;
  --hero-bg-2: #312E81;
  background-color: rgb(var(--v-theme-background));
  min-height: 100dvh;
}

.auth-shell {
  display: grid;
  grid-template-columns: 1.05fr 1fr;
  min-height: 100dvh;
}
@media (max-width: 960px) {
  .auth-shell { grid-template-columns: 1fr; }
}

.auth-hero {
  position: relative;
  overflow: hidden;
  background: linear-gradient(135deg, var(--hero-bg) 0%, var(--hero-bg-2) 100%);
  color: #fff;
  padding: 48px 56px;
}
@media (min-width: 1280px) {
  .auth-hero { padding: 56px 72px; }
}

.auth-hero__bg {
  position: absolute;
  inset: 0;
  z-index: 0;
  pointer-events: none;
  overflow: hidden;
}

.auth-hero__content {
  position: relative;
  z-index: 1;
  display: grid;
  grid-template-rows: auto 1fr auto;
  height: 100%;
  gap: 24px;
}

.hero-top {
  display: flex;
  align-items: center;
}
.hero-brand {
  font-weight: 600;
  font-size: 13px;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: rgba(255,255,255,0.78);
}

.hero-mid {
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 24px;
  max-width: 640px;
}

.hero-title {
  font-size: clamp(34px, 3.6vw, 52px);
  line-height: 1.06;
  font-weight: 700;
  letter-spacing: -0.025em;
  margin: 0;
  text-wrap: balance;
}
.hero-title__accent {
  background: linear-gradient(90deg, #fff 0%, #C7D2FE 100%);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
}

.hero-sub {
  font-size: 16px;
  line-height: 1.6;
  color: rgba(255,255,255,0.78);
  margin: 0;
  max-width: 480px;
}

.hero-bot {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.hero-foot {
  margin: 0;
  font-size: 12px;
  color: rgba(255,255,255,0.5);
  letter-spacing: 0.02em;
}

.orb {
  position: absolute;
  border-radius: 50%;
  filter: blur(70px);
  opacity: 0.6;
  animation: float 18s ease-in-out infinite;
}
.orb--a { width: 460px; height: 460px; background: #6366F1; top: -120px; left: -160px; }
.orb--b { width: 400px; height: 400px; background: #A855F7; bottom: -140px; right: -120px; animation-delay: -6s; opacity: 0.55; }
.orb--c { width: 280px; height: 280px; background: #38BDF8; top: 40%; right: 10%; animation-delay: -12s; opacity: 0.42; }

@keyframes float {
  0%, 100% { transform: translate3d(0, 0, 0) scale(1); }
  50% { transform: translate3d(20px, -30px, 0) scale(1.08); }
}

.grid {
  position: absolute;
  inset: 0;
  background-image:
    linear-gradient(rgba(255,255,255,0.05) 1px, transparent 1px),
    linear-gradient(90deg, rgba(255,255,255,0.05) 1px, transparent 1px);
  background-size: 56px 56px;
  mask-image: radial-gradient(ellipse 80% 60% at 50% 50%, #000 30%, transparent 80%);
  -webkit-mask-image: radial-gradient(ellipse 80% 60% at 50% 50%, #000 30%, transparent 80%);
}

.auth-form {
  display: grid;
  place-items: center;
  padding: 40px 24px;
  background-color: rgb(var(--v-theme-background));
}
.auth-form__inner {
  width: 100%;
  max-width: 420px;
}

.fade-up-enter-active,
.fade-up-leave-active {
  transition: opacity 0.25s ease, transform 0.25s ease;
}
.fade-up-enter-from { opacity: 0; transform: translateY(8px); }
.fade-up-leave-to { opacity: 0; transform: translateY(-8px); }
</style>
