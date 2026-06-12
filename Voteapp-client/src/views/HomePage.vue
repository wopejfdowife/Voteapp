<template>
  <div>
    <div class="hero">
      <div class="container text-center">
        <h1><i class="bi bi-check2-circle me-2"></i>Система голосования РСО-Алания</h1>
        <p class="mt-3">Ваш голос определяет будущее республики. Голосуйте осознанно!</p>
        <div v-if="!isAuthenticated" class="mt-4 d-flex flex-wrap justify-content-center gap-3">
          <router-link to="/register" class="btn btn-success btn-lg px-4">
            <i class="bi bi-person-plus-fill me-2"></i>Зарегистрироваться
          </router-link>
          <router-link to="/login" class="btn btn-light-outline btn-lg px-4">
            <i class="bi bi-box-arrow-in-right me-2"></i>Войти
          </router-link>
        </div>
        <div v-else class="mt-4">
          <router-link to="/candidates" class="btn btn-success btn-lg px-4">
            <i class="bi bi-hand-index-thumb me-2"></i>Перейти к кандидатам
          </router-link>
        </div>
      </div>
    </div>

    <div class="container pb-5">
      <div class="row g-4 mb-5">
        <div class="col-md-4">
          <div class="stat-card">
            <div class="stat-icon"><i class="bi bi-person-badge-fill"></i></div>
            <div class="number">{{ totalCandidates }}</div>
            <div class="label">Кандидатов зарегистрировано</div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="stat-card">
            <div class="stat-icon"><i class="bi bi-check2-all"></i></div>
            <div class="number">{{ totalVotes }}</div>
            <div class="label">Голосов подано</div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="stat-card">
            <div class="stat-icon"><i class="bi bi-people-fill"></i></div>
            <div class="number">{{ totalVoters }}</div>
            <div class="label">Избирателей зарегистрировано</div>
          </div>
        </div>
      </div>

      <div class="row g-4 mb-5">
        <div class="col-lg-7">
          <div class="card">
            <div class="card-header bg-gradient-primary text-white">
              <h5 class="mb-0"><i class="bi bi-bar-chart-fill me-2"></i>Лидеры голосования</h5>
            </div>
            <div class="card-body">
              <div v-if="topCandidates.length > 0">
                <div v-for="(c, index) in topCandidates" :key="c.optionId" class="mb-4">
                  <div class="d-flex justify-content-between align-items-center mb-2">
                    <div class="d-flex align-items-center">
                      <div class="result-dot" :style="{ background: gradients[index % gradients.length] }"></div>
                      <strong class="ms-2">{{ c.optionName }}</strong>
                      <small class="text-muted ms-2">{{ c.description }}</small>
                    </div>
                    <span class="badge" :style="{ background: gradients[index % gradients.length] }">
                      {{ c.voteCount }} голосов ({{ c.percentage.toFixed(1) }}%)
                    </span>
                  </div>
                  <div class="progress progress-custom">
                    <div class="progress-bar" :style="{ width: c.percentage.toFixed(1) + '%', background: gradients[index % gradients.length] }"></div>
                  </div>
                </div>
              </div>
              <div v-else class="text-center text-muted py-4">
                <i class="bi bi-inbox" style="font-size:2rem;"></i>
                <p class="mt-2 mb-0">Пока нет голосов</p>
              </div>
            </div>
          </div>
        </div>

        <div class="col-lg-5">
          <div class="card">
            <div class="card-header bg-gradient-success text-center">
              <h5 class="mb-0"><i class="bi bi-trophy-fill me-2"></i>Лидер</h5>
            </div>
            <div class="card-body text-center">
              <div v-if="winner">
                <div class="py-3">
                  <div class="trophy">🏆</div>
                </div>
                <h3 class="winner-name">{{ winner.name || winner.optionName }}</h3>
                <p class="text-muted mb-3">{{ winner.description }}</p>
                <div class="d-flex justify-content-center gap-4">
                  <div>
                    <div class="summary-value">{{ winner.voteCount }}</div>
                    <div class="summary-label">голосов</div>
                  </div>
                </div>
              </div>
              <div v-else>
                <div class="py-3">
                  <div class="trophy muted">🏆</div>
                </div>
                <p class="text-muted mt-3 mb-0">Ожидаем первые голоса...</p>
              </div>
            </div>
          </div>

          <div class="card mt-4">
            <div class="card-header bg-gradient-info text-white">
              <h5 class="mb-0"><i class="bi bi-info-circle-fill me-2"></i>Сводка</h5>
            </div>
            <div class="card-body">
              <div class="row g-3">
                <div class="col-6">
                  <div class="summary-box">
                    <i class="bi bi-list-ul summary-icon"></i>
                    <div class="summary-value">{{ totalCandidates }}</div>
                    <div class="summary-label">Кандидатов</div>
                  </div>
                </div>
                <div class="col-6">
                  <div class="summary-box">
                    <i class="bi bi-people-fill summary-icon"></i>
                    <div class="summary-value">{{ totalVotes }}</div>
                    <div class="summary-label">Голосов</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="card cta-card p-5 text-center">
        <h4 class="fw-bold mb-3">Примите участие в голосовании!</h4>
        <p class="mb-4 opacity-75 mx-auto" style="max-width:600px;">
          Электронное голосование — быстро, удобно и безопасно. Каждый гражданин имеет право подать свой голос за кандидата,
          которому доверяет развитие нашей республики.
        </p>
        <router-link to="/candidates" class="btn btn-light btn-lg px-4 mx-auto">
          <i class="bi bi-hand-index-thumb me-2"></i>Список кандидатов
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

const API_URL = '/api/vote'

export default {
  name: 'HomePage',
  data() {
    return {
      totalCandidates: 0,
      totalVotes: 0,
      totalVoters: 0,
      topCandidates: [],
      winner: null,
      gradients: [
        'var(--gradient-1)',
        'var(--gradient-2)',
        'var(--gradient-3)'
      ]
    }
  },
  computed: {
    isAuthenticated() {
      return !!localStorage.getItem('voterName')
    }
  },
  mounted() {
    this.fetchStats()
  },
  methods: {
    async fetchStats() {
      try {
        const [optionsRes, totalRes, resultsRes, winnerRes] = await Promise.all([
          axios.get(`${API_URL}/options`),
          axios.get(`${API_URL}/total-votes`),
          axios.get(`${API_URL}/results`),
          axios.get(`${API_URL}/winner`)
        ])
        this.totalCandidates = optionsRes.data.length
        this.totalVotes = totalRes.data.totalVotes
        this.totalVoters = totalRes.data.totalVotes
        this.topCandidates = resultsRes.data
          .sort((a, b) => b.voteCount - a.voteCount)
          .slice(0, 3)
        const w = winnerRes.data
        if (w.winner) {
          this.winner = {
            name: w.winner.name,
            optionName: w.winner.optionName || w.winner.name,
            voteCount: w.winner.voteCount,
            description: w.winner.description,
            percentage: w.percentage
          }
        } else if (w.message) {
          this.winner = null
        }
      } catch (error) {
        console.error('Error fetching stats:', error)
      }
    }
  }
}
</script>

<style scoped>
.hero {
  background: var(--gradient-primary);
  color: white;
  padding: 5rem 0;
  margin-bottom: 3rem;
  box-shadow: var(--shadow-lg);
}

.hero h1 {
  font-size: 2.5rem;
  font-weight: 800;
  letter-spacing: -1px;
}

.hero p {
  font-size: 1.15rem;
  opacity: 0.9;
}

.hero .btn-light-outline {
  border: 2px solid rgba(255, 255, 255, 0.6);
  color: #fff;
  background: transparent;
}

.hero .btn-light-outline:hover {
  background: rgba(255, 255, 255, 0.15);
  color: #fff;
  border-color: #fff;
}

.cta-card {
  background: var(--gradient-primary);
  color: white;
  border: none;
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-lg);
}
</style>
