<template>
  <div id="app">
    <nav class="navbar navbar-expand-lg">
      <div class="container">
        <router-link class="navbar-brand" to="/">
          <i class="bi bi-check2-circle"></i> VoteApp
        </router-link>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav ms-auto">
            <li class="nav-item">
              <router-link class="nav-link" to="/" exact-active-class="active">
                <i class="bi bi-hand-index-thumb"></i> Vote
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/statistics" active-class="active">
                <i class="bi bi-bar-chart-fill"></i> Statistics
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/admin" active-class="active">
                <i class="bi bi-gear-fill"></i> Admin
              </router-link>
            </li>
          </ul>
        </div>
      </div>
    </nav>
    <main class="container py-4">
      <router-view v-slot="{ Component }">
        <transition name="page" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>
  </div>
</template>

<script>
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'

export default {
  name: 'App'
}
</script>

<style>
:root {
  --gradient-primary: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  --gradient-secondary: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
  --gradient-accent: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
  --gradient-bg: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  --glass-bg: rgba(255, 255, 255, 0.85);
  --glass-border: rgba(255, 255, 255, 0.3);
  --shadow-sm: 0 2px 8px rgba(0, 0, 0, 0.06);
  --shadow-md: 0 4px 20px rgba(0, 0, 0, 0.08);
  --shadow-lg: 0 8px 40px rgba(0, 0, 0, 0.12);
  --radius-sm: 8px;
  --radius-md: 12px;
  --radius-lg: 16px;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  background: var(--gradient-bg);
  min-height: 100vh;
  color: #2d3436;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

#app {
  position: relative;
  min-height: 100vh;
  font-family: inherit;
  color: inherit;
}

.navbar {
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 50%, #0f3460 100%);
  padding: 0.875rem 0;
  box-shadow: 0 4px 30px rgba(0, 0, 0, 0.15);
}

.navbar-brand {
  font-weight: 800;
  font-size: 1.5rem;
  color: #fff !important;
  letter-spacing: -0.5px;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.navbar-brand i {
  background: var(--gradient-accent);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  font-size: 1.6rem;
}

.nav-link {
  color: rgba(255, 255, 255, 0.7) !important;
  font-weight: 500;
  padding: 0.5rem 1rem !important;
  border-radius: var(--radius-sm);
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 0.4rem;
  font-size: 0.9rem;
}

.nav-link i {
  font-size: 1.1rem;
  transition: transform 0.3s ease;
}

.nav-link:hover {
  color: #fff !important;
  background: rgba(255, 255, 255, 0.1);
}

.nav-link:hover i {
  transform: translateY(-1px);
}

.nav-link.active {
  color: #fff !important;
  background: rgba(255, 255, 255, 0.15);
  position: relative;
}

.nav-link.active::after {
  content: '';
  position: absolute;
  bottom: 2px;
  left: 50%;
  transform: translateX(-50%);
  width: 60%;
  height: 3px;
  background: var(--gradient-accent);
  border-radius: 3px;
}

.navbar-toggler {
  border-color: rgba(255, 255, 255, 0.3);
}

.navbar-toggler-icon {
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 30 30'%3E%3Cpath stroke='rgba(255,255,255,0.8)' stroke-width='2' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
}

.page-enter-active,
.page-leave-active {
  transition: all 0.35s ease;
}

.page-enter-from {
  opacity: 0;
  transform: translateY(20px) scale(0.98);
}

.page-leave-to {
  opacity: 0;
  transform: translateY(-20px) scale(0.98);
}

.card {
  border: none;
  border-radius: var(--radius-md);
  box-shadow: var(--shadow-sm);
  transition: all 0.3s ease;
  backdrop-filter: blur(10px);
  background: var(--glass-bg);
  border: 1px solid var(--glass-border);
}

.card:hover {
  box-shadow: var(--shadow-md);
  transform: translateY(-2px);
}

.card-header {
  border-bottom: none;
  border-radius: var(--radius-md) var(--radius-md) 0 0 !important;
  font-weight: 600;
}

.btn {
  border-radius: var(--radius-sm);
  font-weight: 600;
  padding: 0.5rem 1.25rem;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.btn-primary {
  background: var(--gradient-primary);
  border: none;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.5);
}

.btn-success {
  background: var(--gradient-accent);
  border: none;
  box-shadow: 0 4px 15px rgba(79, 172, 254, 0.4);
}

.btn-success:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(79, 172, 254, 0.5);
}

.btn-danger {
  background: linear-gradient(135deg, #ff6b6b, #ee5a24);
  border: none;
  box-shadow: 0 4px 15px rgba(255, 107, 107, 0.3);
}

.btn-danger:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(255, 107, 107, 0.4);
}

.btn-warning {
  background: linear-gradient(135deg, #feca57, #ff9f43);
  border: none;
  color: #2d3436;
}

.btn-warning:hover {
  transform: translateY(-2px);
  color: #2d3436;
}

.badge {
  padding: 0.4rem 0.75rem;
  font-weight: 600;
  border-radius: 20px;
}

h2 {
  font-weight: 800;
  letter-spacing: -1px;
  background: var(--gradient-primary);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 1.5rem !important;
}

.form-control {
  border-radius: var(--radius-sm);
  border: 2px solid #e8ecf1;
  padding: 0.6rem 1rem;
  font-size: 0.95rem;
  transition: all 0.3s ease;
  background: white;
}

.form-control:focus {
  border-color: #667eea;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.15);
}

.form-label {
  font-weight: 600;
  color: #2d3436;
  margin-bottom: 0.4rem;
  font-size: 0.9rem;
}

.list-group-item {
  border: none;
  border-radius: var(--radius-sm) !important;
  margin-bottom: 0.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
  transition: all 0.3s ease;
  background: var(--glass-bg);
  border: 1px solid var(--glass-border);
}

.list-group-item:hover {
  transform: translateX(4px);
  box-shadow: var(--shadow-sm);
}
</style>
