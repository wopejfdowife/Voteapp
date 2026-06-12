<template>
  <div>
    <div class="page-header">
      <div class="container text-center">
        <h1><i class="bi bi-person-plus-fill me-2"></i>Регистрация избирателя</h1>
        <p>Создайте аккаунт для участия в голосовании</p>
      </div>
    </div>

    <div class="container pb-5">
      <div class="row justify-content-center">
        <div class="col-lg-6 col-md-8">
          <div class="card auth-card p-4 p-md-5">
            <h2 class="text-center page-title mb-1"><i class="bi bi-person-plus-fill"></i> Регистрация</h2>
            <p class="text-center page-subtitle">Станьте участником голосования</p>

            <div v-if="errors.length" class="alert alert-danger">
              <div v-for="err in errors" :key="err"><i class="bi bi-exclamation-triangle-fill me-2"></i>{{ err }}</div>
            </div>

            <form @submit.prevent="register">
              <div class="mb-3">
                <label class="form-label"><i class="bi bi-person-fill me-1"></i> ФИО</label>
                <div class="input-group">
                  <span class="input-group-text"><i class="bi bi-person"></i></span>
                  <input type="text" class="form-control" v-model="form.fullName" placeholder="Иванов Иван Иванович" required />
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label"><i class="bi bi-envelope-fill me-1"></i> Email</label>
                <div class="input-group">
                  <span class="input-group-text"><i class="bi bi-envelope"></i></span>
                  <input type="email" class="form-control" v-model="form.email" placeholder="example@mail.ru" required />
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label"><i class="bi bi-calendar-fill me-1"></i> Дата рождения</label>
                <div class="input-group">
                  <span class="input-group-text"><i class="bi bi-calendar"></i></span>
                  <input type="date" class="form-control" v-model="form.dateOfBirth" required />
                </div>
                <small class="text-muted">Участие разрешено с 18 лет</small>
              </div>

              <div class="mb-3">
                <label class="form-label"><i class="bi bi-geo-alt-fill me-1"></i> Район</label>
                <div class="input-group">
                  <span class="input-group-text"><i class="bi bi-geo-alt"></i></span>
                  <select class="form-select" v-model="form.district" required>
                    <option value="">— Выберите район —</option>
                    <option v-for="d in districts" :key="d" :value="d">{{ d }}</option>
                  </select>
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label"><i class="bi bi-lock-fill me-1"></i> Пароль</label>
                <div class="input-group">
                  <span class="input-group-text"><i class="bi bi-lock"></i></span>
                  <input type="password" class="form-control" v-model="form.password" placeholder="Минимум 6 символов с цифрой" required />
                </div>
              </div>

              <div class="mb-4">
                <label class="form-label"><i class="bi bi-lock-fill me-1"></i> Подтверждение пароля</label>
                <div class="input-group">
                  <span class="input-group-text"><i class="bi bi-lock-fill"></i></span>
                  <input type="password" class="form-control" v-model="form.confirmPassword" placeholder="Повторите пароль" required />
                </div>
              </div>

              <button type="submit" class="btn btn-primary w-100 py-2" :disabled="loading">
                <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
                <i v-else class="bi bi-person-check-fill me-2"></i>Зарегистрироваться
              </button>
            </form>

            <hr class="my-4" />
            <p class="text-center text-muted mb-0">
              Уже есть аккаунт?
              <router-link to="/login" class="fw-semibold">Войти</router-link>
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'RegisterPage',
  data() {
    return {
      form: {
        fullName: '',
        email: '',
        dateOfBirth: '',
        district: '',
        password: '',
        confirmPassword: ''
      },
      districts: [
        'Иристонский', 'Промышленный', 'Северо-Западный',
        'Центральный', 'Затеречный', 'Пригородный'
      ],
      errors: [],
      loading: false
    }
  },
  methods: {
    validate() {
      this.errors = []
      if (!this.form.fullName.trim()) this.errors.push('Введите ФИО')
      if (!this.form.email.trim()) this.errors.push('Введите email')
      if (!this.form.dateOfBirth) this.errors.push('Введите дату рождения')
      if (!this.form.district) this.errors.push('Выберите район')
      if (this.form.password.length < 6) this.errors.push('Пароль должен быть минимум 6 символов')
      if (this.form.password !== this.form.confirmPassword) this.errors.push('Пароли не совпадают')
      if (this.form.dateOfBirth) {
        const birthDate = new Date(this.form.dateOfBirth)
        const today = new Date()
        let age = today.getFullYear() - birthDate.getFullYear()
        const monthDiff = today.getMonth() - birthDate.getMonth()
        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) age--
        if (age < 18) this.errors.push('Для участия в голосовании необходимо быть старше 18 лет')
      }
      return this.errors.length === 0
    },
    async register() {
      if (!this.validate()) return
      this.loading = true
      // Simple client-side registration
      localStorage.setItem('voterName', this.form.fullName)
      this.$router.push('/')
    }
  }
}
</script>
