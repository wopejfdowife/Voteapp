<template>
  <div id="app">
    <nav class="navbar navbar-expand-lg navbar-dark">
      <div class="container">
        <router-link class="navbar-brand" to="/">
          <i class="bi bi-check2-circle"></i> ГолосРСО
        </router-link>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav ms-auto">
            <li class="nav-item">
              <router-link class="nav-link" to="/" :class="{ active: $route.path === '/' }">
                <i class="bi bi-house-fill"></i> Главная
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/candidates" :class="{ active: $route.path.startsWith('/candidates') && $route.path !== '/candidates/my-vote' }">
                <i class="bi bi-hand-index-thumb"></i> Кандидаты
              </router-link>
            </li>
            <li v-if="isAuthenticated" class="nav-item">
              <router-link class="nav-link" to="/candidates/my-vote" :class="{ active: $route.path === '/candidates/my-vote' }">
                <i class="bi bi-person-check-fill"></i> Мой голос
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/about" :class="{ active: $route.path === '/about' }">
                <i class="bi bi-info-circle-fill"></i> О системе
              </router-link>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#" @click.prevent="goToAdmin">
                <i class="bi bi-gear-fill"></i> Админ
              </a>
            </li>
            <li v-if="isAuthenticated" class="nav-item">
              <span class="nav-link user-badge">
                <i class="bi bi-person-circle"></i> {{ currentUser }}
              </span>
            </li>
            <li v-if="isAuthenticated" class="nav-item">
              <button class="btn btn-link nav-link" @click="logout">
                <i class="bi bi-box-arrow-right"></i> Выйти
              </button>
            </li>
            <li v-if="!isAuthenticated" class="nav-item">
              <router-link class="nav-link" to="/login" :class="{ active: $route.path === '/login' }">
                <i class="bi bi-box-arrow-in-right"></i> Войти
              </router-link>
            </li>
            <li v-if="!isAuthenticated" class="nav-item ms-lg-2">
              <router-link class="nav-link btn-register px-3" to="/register" :class="{ active: $route.path === '/register' }">
                <i class="bi bi-person-plus-fill"></i> Регистрация
              </router-link>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <main>
      <router-view v-slot="{ Component }">
        <transition name="page" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>

    <div v-if="showAdminModal" class="modal-overlay" @click.self="showAdminModal = false">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content auth-card p-4">
          <div class="modal-header border-0 pb-0">
            <h5 class="modal-title"><i class="bi bi-shield-lock-fill text-primary me-2"></i>Доступ к админ-панели</h5>
            <button type="button" class="btn-close" @click="showAdminModal = false"></button>
          </div>
          <div class="modal-body">
            <p class="text-muted mb-3" style="font-size:.9rem;">Введите пароль для входа в панель управления.</p>
            <div class="input-group mb-3">
              <span class="input-group-text"><i class="bi bi-lock-fill"></i></span>
              <input
                ref="adminPassword"
                type="password"
                class="form-control"
                v-model="adminPasswordInput"
                placeholder="Пароль"
                @keyup.enter="checkAdminPassword"
              />
            </div>
            <div v-if="adminError" class="alert alert-danger py-2 small">
              <i class="bi bi-exclamation-triangle-fill me-1"></i>{{ adminError }}
            </div>
          </div>
          <div class="modal-footer border-0 justify-content-center pt-0">
            <button type="button" class="btn btn-outline-secondary" @click="showAdminModal = false">
              <i class="bi bi-x-lg me-1"></i>Отмена
            </button>
            <button type="button" class="btn btn-primary" @click="checkAdminPassword">
              <i class="bi bi-box-arrow-in-right me-1"></i>Войти
            </button>
          </div>
        </div>
      </div>
    </div>

    <footer>
      <div class="container">
        <div class="row align-items-center">
          <div class="col-md-6">
            <h6 class="text-white mb-2"><i class="bi bi-check2-circle me-2"></i>ГолосРСО</h6>
            <p class="mb-0 footer-text">Электронная система голосования Республики Северная Осетия – Алания</p>
          </div>
          <div class="col-md-6 text-md-end">
            <p class="mb-0 footer-text">
              г. Владикавказ, 2024<br/>
              <a href="#">Политика конфиденциальности</a>
            </p>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script>
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'

export default {
  name: 'App',
  data() {
    return {
      currentUser: localStorage.getItem('voterName') || '',
      showAdminModal: false,
      adminPasswordInput: '',
      adminError: ''
    }
  },
  computed: {
    isAuthenticated() {
      return !!this.currentUser
    }
  },
  methods: {
    goToAdmin() {
      this.adminPasswordInput = ''
      this.adminError = ''
      this.showAdminModal = true
      this.$nextTick(() => {
        if (this.$refs.adminPassword) this.$refs.adminPassword.focus()
      })
    },
    checkAdminPassword() {
      if (this.adminPasswordInput === 'burmalda') {
        this.showAdminModal = false
        this.adminPasswordInput = ''
        this.adminError = ''
        this.$router.push('/admin')
      } else {
        this.adminError = 'Неверный пароль!'
      }
    },
    logout() {
      localStorage.removeItem('voterName')
      localStorage.removeItem('votedFor')
      this.currentUser = ''
      this.$router.push('/')
    }
  },
  watch: {
    '$route'() {
      this.currentUser = localStorage.getItem('voterName') || ''
    }
  },
  created() {
    this.currentUser = localStorage.getItem('voterName') || ''
  }
}
</script>

<style>
:root {
  --gradient-primary: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  --gradient-secondary: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
  --gradient-accent: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
  --gradient-bg: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  --gradient-1: linear-gradient(135deg, #667eea, #764ba2);
  --gradient-2: linear-gradient(135deg, #4facfe, #00f2fe);
  --gradient-3: linear-gradient(135deg, #43e97b, #38f9d7);
  --gradient-4: linear-gradient(135deg, #fa709a, #fee140);
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
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

main {
  flex: 1;
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
  text-decoration: none;
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

.btn-register {
  background: var(--gradient-accent) !important;
  border: none !important;
  color: #fff !important;
  font-weight: 600;
  border-radius: var(--radius-sm);
  box-shadow: 0 4px 15px rgba(79, 172, 254, 0.4);
}

.btn-register:hover {
  transform: translateY(-2px);
  color: #fff !important;
}

.user-badge {
  color: rgba(255, 255, 255, 0.9) !important;
  cursor: default;
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
}

.card-header {
  border-bottom: none;
  border-radius: var(--radius-md) var(--radius-md) 0 0 !important;
  font-weight: 600;
}

.bg-gradient-primary {
  background: var(--gradient-primary) !important;
  color: #fff !important;
}

.bg-gradient-success {
  background: var(--gradient-3) !important;
  color: #1a3a2a !important;
}

.bg-gradient-info {
  background: var(--gradient-2) !important;
  color: #fff !important;
}

.bg-gradient-warning {
  background: linear-gradient(135deg, #feca57, #ff9f43) !important;
  color: #2d3436 !important;
}

.bg-gradient-danger {
  background: linear-gradient(135deg, #ff6b6b, #ee5a24) !important;
  color: #fff !important;
}

.btn {
  border-radius: var(--radius-sm);
  font-weight: 600;
  padding: 0.5rem 1.25rem;
  transition: all 0.3s ease;
}

.btn-primary {
  background: var(--gradient-primary);
  border: none;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
  color: #fff;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.5);
  background: var(--gradient-primary);
  color: #fff;
}

.btn-success, .btn-vote {
  background: var(--gradient-accent);
  border: none;
  box-shadow: 0 4px 15px rgba(79, 172, 254, 0.4);
  color: #fff;
}

.btn-success:hover, .btn-vote:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(79, 172, 254, 0.5);
  color: #fff;
}

.btn-outline-primary {
  border: 2px solid #667eea;
  color: #667eea;
  background: transparent;
}

.btn-outline-primary:hover {
  background: var(--gradient-primary);
  border-color: transparent;
  color: #fff;
  transform: translateY(-2px);
}

.btn-gradient {
  color: white;
  border: none;
  font-weight: 600;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.15);
}

.btn-gradient:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.2);
  color: white;
}

.form-control, .form-select {
  border-radius: var(--radius-sm);
  border: 2px solid #e8ecf1;
  padding: 0.6rem 1rem;
  transition: all 0.3s ease;
  background: white;
}

.form-control:focus, .form-select:focus {
  border-color: #667eea;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.15);
}

.form-label {
  font-weight: 600;
  color: #2d3436;
  margin-bottom: 0.4rem;
  font-size: 0.9rem;
}

.input-group-text {
  border: 2px solid #e8ecf1;
  border-right: none;
  background: #f8f9fc;
  border-radius: var(--radius-sm) 0 0 var(--radius-sm);
}

.input-group .form-control,
.input-group .form-select {
  border-left: none;
}

.badge {
  padding: 0.4rem 0.75rem;
  font-weight: 600;
  border-radius: 20px;
}

.badge-district {
  background: linear-gradient(135deg, #667eea20, #764ba220);
  color: #667eea;
  font-size: 0.78rem;
}

.alert {
  border-radius: var(--radius-md);
  border: none;
  box-shadow: var(--shadow-sm);
}

.page-title {
  font-weight: 800;
  letter-spacing: -1px;
  background: var(--gradient-primary);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 0.5rem !important;
}

.page-subtitle {
  color: #636e72;
  margin-bottom: 2rem;
}

.page-header {
  background: var(--gradient-primary);
  color: white;
  padding: 3rem 0;
  margin-bottom: 2.5rem;
  box-shadow: var(--shadow-md);
}

.page-header h1 {
  font-weight: 800;
  letter-spacing: -0.5px;
  margin-bottom: 0.5rem;
}

.page-header p {
  opacity: 0.85;
  margin-bottom: 0;
}

.page-header .breadcrumb-item a {
  color: rgba(255, 255, 255, 0.7);
  text-decoration: none;
}

.page-header .breadcrumb-item.active {
  color: #fff;
}

.vote-card {
  overflow: hidden;
  padding-top: 0;
  animation: cardFadeIn 0.5s ease both;
}

@keyframes cardFadeIn {
  from { opacity: 0; transform: translateY(30px); }
  to { opacity: 1; transform: translateY(0); }
}

.card-gradient-bar {
  height: 4px;
  width: 100%;
}

.option-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  color: white;
  font-weight: 700;
  font-size: 1.1rem;
}

.progress-custom {
  height: 12px;
  border-radius: 20px;
  background: #eef0f4;
  overflow: hidden;
}

.progress-custom .progress-bar {
  border-radius: 20px;
  transition: width 1.2s cubic-bezier(0.4, 0, 0.2, 1);
}

.stat-card {
  background: var(--glass-bg);
  border: 1px solid var(--glass-border);
  border-radius: var(--radius-md);
  padding: 1.75rem;
  text-align: center;
  box-shadow: var(--shadow-sm);
  transition: all 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-md);
}

.stat-card .number {
  font-size: 2.5rem;
  font-weight: 800;
  background: var(--gradient-primary);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.stat-card .label {
  color: #636e72;
  font-size: 0.85rem;
  font-weight: 500;
  margin-top: 0.25rem;
}

.stat-card .stat-icon {
  font-size: 1.75rem;
  color: #667eea;
  margin-bottom: 0.5rem;
}

.summary-box {
  background: linear-gradient(135deg, #f5f7fa, #e4e9f2);
  border-radius: var(--radius-md);
  padding: 1.25rem;
  text-align: center;
  transition: all 0.3s ease;
}

.summary-box:hover {
  transform: translateY(-3px);
  box-shadow: var(--shadow-sm);
}

.summary-icon {
  font-size: 1.5rem;
  color: #667eea;
}

.summary-value {
  font-size: 2rem;
  font-weight: 800;
  color: #2d3436;
  margin: 0.25rem 0;
}

.summary-label {
  font-size: 0.8rem;
  color: #636e72;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.result-dot {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  flex-shrink: 0;
}

.winner-name {
  font-weight: 800;
  font-size: 1.8rem;
  background: linear-gradient(135deg, #f6d365, #fda085);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.trophy {
  font-size: 5rem;
  display: inline-block;
  animation: trophyBounce 2s ease-in-out infinite;
  filter: drop-shadow(0 4px 20px rgba(255, 215, 0, 0.3));
}

.trophy.muted {
  filter: grayscale(1) opacity(0.3);
  animation: none;
}

@keyframes trophyBounce {
  0%, 100% { transform: translateY(0) scale(1); }
  50% { transform: translateY(-10px) scale(1.05); }
}

.auth-card {
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-lg);
}

.candidate-avatar {
  width: 90px;
  height: 90px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  margin: 0 auto 1rem;
}

.voted-badge {
  border-left: 4px solid #38f9d7;
}

footer {
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 50%, #0f3460 100%);
  color: rgba(255, 255, 255, 0.7);
  padding: 2rem 0;
  margin-top: 4rem;
}

footer a {
  color: #4facfe;
  text-decoration: none;
}

footer a:hover {
  color: #00f2fe;
}

.footer-text {
  font-size: .85rem;
}

.notification-toast {
  position: fixed;
  top: 80px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 9999;
  min-width: 320px;
  border-radius: var(--radius-md);
  box-shadow: var(--shadow-lg);
  border: none;
  font-weight: 500;
}

.toast-enter-active {
  animation: slideDown 0.4s ease;
}

.toast-leave-active {
  animation: slideDown 0.3s ease reverse;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateX(-50%) translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) translateY(0);
  }
}

.feature-icon {
  width: 36px;
  height: 36px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--gradient-3);
  color: white;
  flex-shrink: 0;
}

.dev-card .dev-icon {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  margin: 0 auto 1rem;
  font-size: 2rem;
}

.tech-badge {
  background: linear-gradient(135deg, #667eea15, #764ba215);
  color: #667eea;
  font-size: 0.85rem;
  padding: 8px 14px;
  border-radius: 20px;
  font-weight: 600;
}

.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  backdrop-filter: blur(4px);
  animation: fadeIn 0.2s ease;
}

.modal-overlay .modal-content {
  border: 1px solid var(--glass-border);
  box-shadow: var(--shadow-lg);
  width: 100%;
  max-width: 420px;
  background: #fff;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
</style>
