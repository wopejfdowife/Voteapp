<template>
  <div v-if="candidate">
    <div class="page-header">
      <div class="container">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb mb-2">
            <li class="breadcrumb-item"><router-link to="/candidates">Кандидаты</router-link></li>
            <li class="breadcrumb-item active" aria-current="page">{{ candidate.name }}</li>
          </ol>
        </nav>
        <h1>{{ candidate.name }}</h1>
        <p>{{ candidate.description || 'Независимый кандидат' }} · г. Владикавказ</p>
      </div>
    </div>

    <div class="container pb-5">
      <div class="row g-4">
        <div class="col-lg-8">
          <div class="card mb-4">
            <div class="card-header bg-gradient-primary text-white">
              <h5 class="mb-0"><i class="bi bi-person-lines-fill me-2"></i>Биография</h5>
            </div>
            <div class="card-body">
              <p style="line-height:1.8;color:#444;">{{ candidate.description || 'Биография暂时 отсутствует.' }}</p>
            </div>
          </div>

          <div class="card">
            <div class="card-header bg-gradient-info text-white">
              <h5 class="mb-0"><i class="bi bi-file-text-fill me-2"></i>Предвыборная программа</h5>
            </div>
            <div class="card-body">
              <p style="line-height:1.8;color:#444;">{{ candidate.description || 'Программа暂时 отсутствует.' }}</p>
            </div>
          </div>
        </div>

        <div class="col-lg-4">
          <div class="card mb-4 text-center">
            <div class="card-body">
              <div class="candidate-avatar" style="background:var(--gradient-primary);">
                <i class="bi bi-person-fill" style="font-size:2.5rem;"></i>
              </div>
              <h5 class="fw-bold mb-1">{{ candidate.name }}</h5>
              <p class="text-muted mb-2">{{ candidate.description || 'Независимый кандидат' }}</p>
              <span class="badge badge-district">
                <i class="bi bi-geo-alt-fill me-1"></i>г. Владикавказ
              </span>
              <hr />
              <div class="summary-value">{{ candidate.voteCount }}</div>
              <div class="summary-label">голос(ов) получено</div>
            </div>
          </div>

          <div class="card">
            <div class="card-body">
              <div v-if="!isAuthenticated">
                <h6 class="fw-bold mb-3"><i class="bi bi-lock-fill me-1"></i> Хотите проголосовать?</h6>
                <p class="text-muted mb-3" style="font-size:.9rem;">Для участия в голосовании необходимо войти в систему.</p>
                <router-link to="/login" class="btn btn-primary w-100 mb-2">
                  <i class="bi bi-box-arrow-in-right me-2"></i>Войти
                </router-link>
                <router-link to="/register" class="btn btn-outline-primary w-100">
                  <i class="bi bi-person-plus-fill me-2"></i>Регистрация
                </router-link>
              </div>
              <div v-else-if="votedForThis">
                <div class="text-center">
                  <i class="bi bi-patch-check-fill text-success mb-3" style="font-size:2.5rem;"></i>
                  <h6 class="fw-bold text-success">Вы проголосовали!</h6>
                  <p class="text-muted mb-3" style="font-size:.85rem;">Ваш голос отдан за этого кандидата.</p>
                  <router-link to="/candidates/my-vote" class="btn btn-outline-primary w-100">
                    <i class="bi bi-eye-fill me-2"></i>Мой голос
                  </router-link>
                </div>
              </div>
              <div v-else-if="hasVoted">
                <div class="text-center">
                  <i class="bi bi-info-circle-fill text-warning mb-3" style="font-size:2rem;"></i>
                  <p class="text-muted mb-3" style="font-size:.9rem;">Вы уже проголосовали за другого кандидата. Изменить голос невозможно.</p>
                  <router-link to="/candidates/my-vote" class="btn btn-outline-primary w-100">
                    <i class="bi bi-eye-fill me-2"></i>Посмотреть мой голос
                  </router-link>
                </div>
              </div>
              <div v-else>
                <h6 class="fw-bold mb-3"><i class="bi bi-check2-circle me-1"></i> Проголосовать</h6>
                <p class="text-muted mb-3" style="font-size:.85rem;">
                  Голосование осуществляется однократно. Изменить голос после подачи невозможно.
                </p>
                <button type="button" class="btn btn-vote w-100 py-3" data-bs-toggle="modal" data-bs-target="#voteModal">
                  <i class="bi bi-check2-circle me-2"></i>Отдать голос
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="mt-4">
        <router-link to="/candidates" class="btn btn-outline-secondary">
          <i class="bi bi-arrow-left me-2"></i>Назад к списку
        </router-link>
      </div>
    </div>

    <div class="modal fade" id="voteModal" tabindex="-1" data-bs-backdrop="static">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content auth-card">
          <div class="modal-header border-0">
            <h5 class="modal-title"><i class="bi bi-check2-circle text-primary me-2"></i>Подтвердите голос</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" @click="closeModal"></button>
          </div>
          <div class="modal-body text-center">
            <div class="py-3 mb-3" style="background:linear-gradient(135deg, #667eea15, #764ba215); border-radius:12px;">
              <i class="bi bi-person-fill" style="font-size:2.5rem; color:#667eea;"></i>
              <h4 class="fw-bold mt-2 mb-0">{{ candidate.name }}</h4>
              <p class="text-muted mb-0">{{ candidate.description || 'Независимый кандидат' }}</p>
            </div>
            <p class="text-muted">Вы уверены, что хотите проголосовать за этого кандидата? Это действие нельзя отменить.</p>
          </div>
          <div class="modal-footer border-0 justify-content-center">
            <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal" @click="closeModal">
              <i class="bi bi-x-lg me-1"></i>Отмена
            </button>
            <button type="button" class="btn btn-primary" @click="confirmVote" :disabled="voting">
              <i class="bi bi-check2-all me-1"></i>Подтвердить голос
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div v-else class="container py-5 text-center">
    <div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Загрузка...</span>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { Modal } from 'bootstrap'

const API_URL = '/api/vote'

export default {
  name: 'CandidateDetail',
  props: {
    id: [String, Number]
  },
  data() {
    return {
      candidate: null,
      hasVoted: false,
      votedForThis: false,
      voting: false,
      voteError: ''
    }
  },
  computed: {
    isAuthenticated() {
      return !!localStorage.getItem('voterName')
    }
  },
  mounted() {
    this.fetchCandidate()
  },
  methods: {
    closeModal() {
      const modal = Modal.getInstance(document.getElementById('voteModal'))
      if (modal) modal.hide()
    },
    async fetchCandidate() {
      try {
        const res = await axios.get(`${API_URL}/options`)
        this.candidate = res.data.find(o => o.id === parseInt(this.id))
        if (!this.candidate) {
          this.$router.push('/candidates')
          return
        }
        await this.checkVoteStatus()
      } catch (error) {
        console.error('Error fetching candidate:', error)
      }
    },
    async checkVoteStatus() {
      const voterName = localStorage.getItem('voterName')
      if (voterName) {
        try {
          const checkRes = await axios.get(`${API_URL}/check-vote/${voterName}`)
          this.hasVoted = checkRes.data.hasVoted
          if (this.hasVoted) {
            const votedFor = localStorage.getItem('votedFor')
            this.votedForThis = votedFor === String(this.id)
          }
        } catch (error) {
          console.error('Error checking vote status:', error)
        }
      }
    },
    async confirmVote() {
      const voterName = localStorage.getItem('voterName')
      if (!voterName) {
        this.$router.push('/login')
        return
      }
      this.voting = true
      try {
        const res = await axios.post(`${API_URL}/vote`, {
          voteOptionId: this.candidate.id,
          voterName: voterName
        })
        if (res.data.success) {
          localStorage.setItem('votedFor', String(this.candidate.id))
          this.hasVoted = true
          this.votedForThis = true
          this.candidate.voteCount++
          this.closeModal()
        } else {
          this.voteError = res.data.message || 'Ошибка при голосовании'
        }
      } catch (error) {
        this.voteError = 'Ошибка при голосовании'
      } finally {
        this.voting = false
      }
    }
  }
}
</script>
