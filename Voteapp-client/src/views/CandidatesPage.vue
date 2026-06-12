<template>
  <div>
    <div class="page-header">
      <div class="container">
        <h1><i class="bi bi-hand-index-thumb me-2"></i>Кандидаты</h1>
        <p>Ознакомьтесь с программой каждого кандидата и сделайте свой выбор</p>
      </div>
    </div>

    <div class="container pb-5">
      <h2 class="text-center page-title"><i class="bi bi-hand-index-thumb"></i> Отдайте свой голос</h2>
      <p class="text-center page-subtitle">Выберите кандидата, которому доверяете</p>

      <div v-if="!isAuthenticated" class="alert alert-info mb-4">
        <i class="bi bi-info-circle-fill me-2"></i>
        Чтобы проголосовать, необходимо
        <router-link to="/login" class="alert-link fw-semibold">войти</router-link>
        или
        <router-link to="/register" class="alert-link fw-semibold">зарегистрироваться</router-link>.
      </div>
      <div v-if="hasVoted" class="alert alert-success text-center mb-4">
        <i class="bi bi-check-circle-fill me-2"></i>
        Вы уже проголосовали. Спасибо за участие!
        <router-link to="/candidates/my-vote" class="alert-link fw-semibold ms-2">Посмотреть мой голос →</router-link>
      </div>

      <transition name="toast">
        <div v-if="notification.show" :class="['alert', notification.type, 'alert-dismissible fade show notification-toast']">
          <i :class="notification.icon"></i> {{ notification.message }}
          <button type="button" class="btn-close" @click="notification.show = false"></button>
        </div>
      </transition>

      <div class="row g-4">
        <div v-for="(c, index) in candidates" :key="c.id" class="col-md-6 col-lg-4">
          <div class="card vote-card h-100" :class="{ 'border border-success border-2': votedForId === c.id }">
            <div class="card-gradient-bar" :style="{ background: gradients[index % gradients.length] }"></div>
            <div class="card-body d-flex flex-column">
              <span v-if="votedForId === c.id" class="badge bg-success mb-3" style="width:fit-content;">
                <i class="bi bi-check-circle-fill me-1"></i>Вы проголосовали
              </span>
              <div class="d-flex align-items-center mb-3">
                <div class="option-icon" :style="{ background: gradients[index % gradients.length] }">
                  <i v-if="index === 0" class="bi bi-trophy-fill"></i>
                  <span v-else style="color:white;font-weight:700;">{{ index + 1 }}</span>
                </div>
                <div class="ms-3">
                  <h5 class="card-title mb-1 fw-bold">{{ c.name }}</h5>
                  <small class="text-muted">{{ c.description || 'Независимый кандидат' }}</small>
                </div>
              </div>

              <span class="badge badge-district mb-3" style="width:fit-content;">
                <i class="bi bi-geo-alt-fill me-1"></i>г. Владикавказ
              </span>

              <p class="card-text text-muted flex-grow-1" style="font-size:.9rem;line-height:1.6;">
                {{ c.description || 'Биография отсутствует' }}
              </p>

              <div class="mt-auto">
                <div class="d-flex justify-content-between align-items-center mb-2">
                  <span class="badge" :style="{ background: gradients[index % gradients.length] }">
                    <i class="bi bi-people-fill me-1"></i>{{ c.voteCount }} голосов
                  </span>
                  <small class="text-muted fw-semibold">{{ getPercentage(c).toFixed(1) }}%</small>
                </div>
                <div class="progress progress-custom mb-3">
                  <div class="progress-bar" :style="{ width: getPercentage(c).toFixed(1) + '%', background: gradients[index % gradients.length] }"></div>
                </div>
                <router-link :to="`/candidates/${c.id}`" class="btn btn-gradient w-100" :style="{ background: gradients[index % gradients.length] }">
                  <i class="bi bi-eye-fill me-1"></i> Подробнее
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

const API_URL = '/api/vote'

export default {
  name: 'CandidatesPage',
  data() {
    return {
      candidates: [],
      hasVoted: false,
      votedForId: null,
      notification: { show: false, type: '', message: '', icon: '' },
      gradients: [
        'var(--gradient-1)',
        'var(--gradient-2)',
        'var(--gradient-3)',
        'var(--gradient-4)'
      ]
    }
  },
  computed: {
    isAuthenticated() {
      return !!localStorage.getItem('voterName')
    },
    totalVotes() {
      return this.candidates.reduce((sum, c) => sum + (c.voteCount || 0), 0)
    }
  },
  mounted() {
    this.fetchCandidates()
    this.checkVoted()
  },
  methods: {
    showNotification(type, message, icon) {
      this.notification = { show: true, type, message, icon }
      setTimeout(() => { this.notification.show = false }, 4000)
    },
    getPercentage(c) {
      return this.totalVotes > 0 ? (c.voteCount / this.totalVotes * 100) : 0
    },
    async fetchCandidates() {
      try {
        const res = await axios.get(`${API_URL}/options`)
        this.candidates = res.data
      } catch (error) {
        this.showNotification('alert-danger', 'Ошибка загрузки кандидатов', 'bi bi-exclamation-triangle-fill')
      }
    },
    async checkVoted() {
      const voterName = localStorage.getItem('voterName')
      if (voterName) {
        try {
          const res = await axios.get(`${API_URL}/check-vote/${voterName}`)
          this.hasVoted = res.data.hasVoted
          if (this.hasVoted) {
            const votedFor = localStorage.getItem('votedFor')
            if (votedFor) this.votedForId = parseInt(votedFor)
          }
        } catch (error) {
          console.error('Error checking vote:', error)
        }
      }
    }
  }
}
</script>

<style scoped>
.card-title {
  font-weight: 700;
  font-size: 1.1rem;
}

.vote-card:nth-child(1) { animation-delay: 0.05s; }
.vote-card:nth-child(2) { animation-delay: 0.15s; }
.vote-card:nth-child(3) { animation-delay: 0.25s; }
.vote-card:nth-child(4) { animation-delay: 0.35s; }
.vote-card:nth-child(5) { animation-delay: 0.45s; }
.vote-card:nth-child(6) { animation-delay: 0.55s; }
</style>
