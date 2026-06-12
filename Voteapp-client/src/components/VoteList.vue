<template>
  <div class="container">
    <h2 class="text-center"><i class="bi bi-hand-index-thumb"></i> Cast Your Vote</h2>
    <p class="text-center text-muted mb-4">Choose your favorite programming language</p>

    <transition name="toast">
      <div v-if="notification.show" :class="['alert', notification.type, 'alert-dismissible fade show notification-toast']">
        <i :class="notification.icon"></i> {{ notification.message }}
        <button type="button" class="btn-close" @click="notification.show = false"></button>
      </div>
    </transition>

    <div v-if="hasVoted" class="alert alert-success text-center mb-4">
      <i class="bi bi-check-circle-fill"></i> You have already voted! Thank you for your participation.
    </div>

    <div class="row g-4">
      <div class="col-md-6 col-lg-3" v-for="(option, index) in options" :key="option.id">
        <div class="card vote-card h-100" :style="{ '--card-index': index }">
          <div class="card-gradient-bar" :style="{ background: optionColors[index % optionColors.length] }"></div>
          <div class="card-body d-flex flex-column">
            <div class="d-flex align-items-center mb-3">
              <div class="option-icon" :style="{ background: optionColors[index % optionColors.length] }">
                <i :class="optionIcons[index % optionIcons.length]"></i>
              </div>
              <div class="ms-3">
                <h5 class="card-title mb-1">{{ option.name }}</h5>
                <span class="badge" :style="{ background: optionColors[index % optionColors.length] }">
                  <i class="bi bi-people-fill"></i> {{ option.voteCount }} votes
                </span>
              </div>
            </div>
            <p class="card-text text-muted flex-grow-1">{{ option.description }}</p>
            <button
              class="btn btn-gradient w-100 mt-auto"
              :style="{ background: optionColors[index % optionColors.length] }"
              @click="vote(option.id)"
              :disabled="hasVoted"
            >
              <i class="bi bi-check2-circle"></i> Vote
            </button>
          </div>
        </div>
      </div>
    </div>

    <div class="modal fade" id="voteModal" tabindex="-1" data-bs-backdrop="static">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content glass-modal">
          <div class="modal-header border-0">
            <h5 class="modal-title"><i class="bi bi-check2-circle text-primary"></i> Confirm Your Vote</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <div class="text-center mb-4">
              <div class="selected-option-badge" v-if="selectedOption">
                <i :class="selectedOptionIcon"></i>
                <h4 class="mt-2">{{ selectedOption.name }}</h4>
              </div>
            </div>
            <div class="mb-3">
              <label class="form-label"><i class="bi bi-person-fill"></i> Your Name</label>
              <input
                type="text"
                class="form-control form-control-lg"
                v-model="voterName"
                placeholder="Enter your name"
              >
            </div>
          </div>
          <div class="modal-footer border-0">
            <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">
              <i class="bi bi-x-lg"></i> Cancel
            </button>
            <button type="button" class="btn btn-primary btn-gradient-confirm" @click="confirmVote">
              <i class="bi bi-check2-all"></i> Confirm Vote
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { Modal } from 'bootstrap'

const API_URL = '/api/vote'

export default {
  name: 'VoteList',
  data() {
    return {
      options: [],
      selectedOption: null,
      voterName: '',
      hasVoted: false,
      notification: { show: false, type: '', message: '', icon: '' },
      optionColors: [
        'linear-gradient(135deg, #667eea, #764ba2)',
        'linear-gradient(135deg, #4facfe, #00f2fe)',
        'linear-gradient(135deg, #43e97b, #38f9d7)',
        'linear-gradient(135deg, #fa709a, #fee140)'
      ],
      optionIcons: [
        'bi bi-code-slash',
        'bi bi-filetype-py',
        'bi bi-filetype-java',
        'bi bi-filetype-js'
      ]
    }
  },
  computed: {
    selectedOptionIcon() {
      if (!this.selectedOption) return ''
      const idx = this.options.findIndex(o => o.id === this.selectedOption.id)
      return this.optionIcons[idx % this.optionIcons.length]
    }
  },
  mounted() {
    this.fetchOptions()
    this.checkPreviousVote()
  },
  methods: {
    showNotification(type, message, icon) {
      this.notification = { show: true, type, message, icon }
      setTimeout(() => { this.notification.show = false }, 4000)
    },
    async fetchOptions() {
      try {
        const response = await axios.get(`${API_URL}/options`)
        this.options = response.data
      } catch (error) {
        this.showNotification('alert-danger', 'Failed to load voting options', 'bi bi-exclamation-triangle-fill')
      }
    },
    async checkPreviousVote() {
      const storedName = localStorage.getItem('voterName')
      if (storedName) {
        try {
          const response = await axios.get(`${API_URL}/check-vote/${storedName}`)
          this.hasVoted = response.data.hasVoted
          if (this.hasVoted) {
            this.voterName = storedName
          }
        } catch (error) {
          console.error('Error checking vote:', error)
        }
      }
    },
    vote(optionId) {
      this.selectedOption = this.options.find(opt => opt.id === optionId)
      const modal = new Modal(document.getElementById('voteModal'))
      modal.show()
    },
    async confirmVote() {
      if (!this.voterName.trim()) {
        this.showNotification('alert-warning', 'Please enter your name', 'bi bi-exclamation-circle-fill')
        return
      }
      try {
        const response = await axios.post(`${API_URL}/vote`, {
          voteOptionId: this.selectedOption.id,
          voterName: this.voterName
        })
        if (response.data.success) {
          localStorage.setItem('voterName', this.voterName)
          this.hasVoted = true
          this.showNotification('alert-success', 'Vote recorded successfully!', 'bi bi-check-circle-fill')
          this.fetchOptions()
          const modal = Modal.getInstance(document.getElementById('voteModal'))
          modal.hide()
          this.voterName = ''
          this.selectedOption = null
        } else {
          this.showNotification('alert-danger', response.data.message, 'bi bi-exclamation-triangle-fill')
        }
      } catch (error) {
        this.showNotification('alert-danger', 'Error recording vote', 'bi bi-exclamation-triangle-fill')
      }
    }
  }
}
</script>

<style scoped>
.vote-card {
  animation: cardFadeIn 0.5s ease both;
  animation-delay: calc(var(--card-index) * 0.1s);
  padding-top: 0;
  overflow: hidden;
  cursor: default;
}

@keyframes cardFadeIn {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.card-gradient-bar {
  height: 4px;
  width: 100%;
  border-radius: var(--radius-md) var(--radius-md) 0 0;
}

.option-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.option-icon i {
  font-size: 1.4rem;
  color: white;
}

.card-title {
  font-weight: 700;
  font-size: 1.1rem;
}

.btn-gradient {
  color: white;
  border: none;
  font-weight: 600;
  padding: 0.6rem;
  border-radius: var(--radius-sm);
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.15);
}

.btn-gradient:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.2);
  color: white;
}

.btn-gradient:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.glass-modal {
  background: rgba(255, 255, 255, 0.98);
  border: 1px solid rgba(255, 255, 255, 0.5);
  border-radius: var(--radius-lg);
}

.selected-option-badge {
  display: inline-block;
  padding: 1rem 2rem;
  border-radius: var(--radius-md);
  background: linear-gradient(135deg, #667eea15, #764ba215);
}

.selected-option-badge i {
  font-size: 2.5rem;
  color: #667eea;
}

.selected-option-badge h4 {
  font-weight: 700;
  margin: 0;
}

.btn-gradient-confirm {
  background: var(--gradient-primary, linear-gradient(135deg, #667eea, #764ba2));
  border: none;
  padding: 0.6rem 1.5rem;
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
</style>
