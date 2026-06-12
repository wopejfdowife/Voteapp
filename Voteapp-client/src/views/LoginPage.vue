<template>
  <div>
    <div class="page-header">
      <div class="container text-center">
        <h1><i class="bi bi-box-arrow-in-right me-2"></i>Вход в систему</h1>
        <p>Войдите для участия в голосовании</p>
      </div>
    </div>

    <div class="container pb-5">
      <div class="row justify-content-center">
        <div class="col-lg-5 col-md-7">
          <div class="card auth-card p-4 p-md-5">
            <h2 class="text-center page-title mb-1"><i class="bi bi-box-arrow-in-right"></i> Вход</h2>
            <p class="text-center page-subtitle">Добро пожаловать в ГолосРСО</p>

            <div v-if="error" class="alert alert-danger">
              <i class="bi bi-exclamation-triangle-fill me-2"></i>{{ error }}
            </div>

            <form @submit.prevent="login">
              <div class="mb-3">
                <label class="form-label"><i class="bi bi-envelope-fill me-1"></i> Email</label>
                <div class="input-group">
                  <span class="input-group-text"><i class="bi bi-envelope"></i></span>
                  <input type="email" class="form-control" v-model="email" placeholder="example@mail.ru" required />
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label"><i class="bi bi-lock-fill me-1"></i> Пароль</label>
                <div class="input-group">
                  <span class="input-group-text"><i class="bi bi-lock"></i></span>
                  <input type="password" class="form-control" v-model="password" placeholder="Ваш пароль" required />
                </div>
              </div>

              <div class="mb-4">
                <div class="form-check">
                  <input type="checkbox" class="form-check-input" id="rememberMe" v-model="rememberMe" />
                  <label class="form-check-label" for="rememberMe">Запомнить меня</label>
                </div>
              </div>

              <button type="submit" class="btn btn-primary w-100 py-2" :disabled="loading">
                <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
                <i v-else class="bi bi-box-arrow-in-right me-2"></i>Войти
              </button>
            </form>

            <hr class="my-4" />
            <p class="text-center text-muted mb-0">
              Нет аккаунта?
              <router-link to="/register" class="fw-semibold">Зарегистрироваться</router-link>
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'LoginPage',
  data() {
    return {
      email: '',
      password: '',
      rememberMe: false,
      loading: false,
      error: ''
    }
  },
  methods: {
    async login() {
      this.loading = true
      this.error = ''
      // Simple client-side login simulation
      const name = this.email.split('@')[0]
      localStorage.setItem('voterName', name)
      this.$router.push('/')
    }
  }
}
</script>
