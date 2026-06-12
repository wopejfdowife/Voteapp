<template>
  <div class="container">
    <h2 class="text-center"><i class="bi bi-gear-fill"></i> Admin Panel</h2>
    <p class="text-center text-muted mb-4">Manage voting options</p>

    <transition name="toast">
      <div v-if="notification.show" :class="['alert', notification.type, 'alert-dismissible fade show notification-toast']">
        <i :class="notification.icon"></i> {{ notification.message }}
        <button type="button" class="btn-close" @click="notification.show = false"></button>
      </div>
    </transition>

    <div class="row g-4">
      <div class="col-lg-5">
        <div class="card">
          <div class="card-header bg-gradient-warning text-dark">
            <h5 class="mb-0"><i class="bi bi-plus-circle-fill"></i> Add New Option</h5>
          </div>
          <div class="card-body">
            <div class="mb-3">
              <label class="form-label"><i class="bi bi-type-bold"></i> Option Name</label>
              <input
                type="text"
                class="form-control"
                v-model="newOption.name"
                placeholder="e.g. Rust, Go, TypeScript"
              >
            </div>
            <div class="mb-3">
              <label class="form-label"><i class="bi bi-text-paragraph"></i> Description</label>
              <textarea
                class="form-control"
                v-model="newOption.description"
                rows="4"
                placeholder="Describe this option..."
              ></textarea>
            </div>
            <button
              class="btn btn-primary w-100"
              @click="addOption"
              :disabled="!newOption.name"
            >
              <i class="bi bi-plus-lg"></i> Add Option
            </button>
          </div>
        </div>
      </div>

      <div class="col-lg-7">
        <div class="card">
          <div class="card-header bg-gradient-danger text-white">
            <h5 class="mb-0"><i class="bi bi-list-check"></i> Manage Options</h5>
          </div>
          <div class="card-body">
            <div v-if="options.length === 0" class="text-center text-muted py-4">
              <i class="bi bi-inbox" style="font-size: 2rem;"></i>
              <p class="mt-2">No options available</p>
            </div>
            <transition-group name="list" tag="div" class="list-group">
              <div
                v-for="option in options"
                :key="option.id"
                class="list-group-item d-flex justify-content-between align-items-center"
              >
                <div>
                  <strong class="option-name">{{ option.name }}</strong>
                  <small class="text-muted d-block mt-1">{{ option.description }}</small>
                  <span class="badge bg-light text-dark mt-2">
                    <i class="bi bi-people-fill"></i> {{ option.voteCount }} votes
                  </span>
                </div>
                <button class="btn btn-danger btn-sm" @click="confirmDelete(option)">
                  <i class="bi bi-trash-fill"></i> Delete
                </button>
              </div>
            </transition-group>
          </div>
        </div>
      </div>
    </div>

    <div class="modal fade" id="deleteModal" tabindex="-1">
      <div class="modal-dialog modal-dialog-centered modal-sm">
        <div class="modal-content">
          <div class="modal-body text-center py-4">
            <i class="bi bi-exclamation-triangle-fill text-danger" style="font-size: 3rem;"></i>
            <h5 class="mt-3">Delete "{{ deletingOption?.name }}"?</h5>
            <p class="text-muted">This action cannot be undone.</p>
          </div>
          <div class="modal-footer border-0 justify-content-center">
            <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">
              <i class="bi bi-x-lg"></i> Cancel
            </button>
            <button type="button" class="btn btn-danger" @click="deleteOption">
              <i class="bi bi-trash-fill"></i> Delete
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
  name: 'AdminPanel',
  data() {
    return {
      options: [],
      newOption: {
        name: '',
        description: ''
      },
      deletingOption: null,
      notification: { show: false, type: '', message: '', icon: '' }
    }
  },
  mounted() {
    this.fetchOptions()
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
        this.showNotification('alert-danger', 'Failed to load options', 'bi bi-exclamation-triangle-fill')
      }
    },
    async addOption() {
      try {
        const response = await axios.post(`${API_URL}/option`, this.newOption)
        if (response.data.success) {
          this.showNotification('alert-success', 'Option added successfully!', 'bi bi-check-circle-fill')
          this.newOption = { name: '', description: '' }
          this.fetchOptions()
        }
      } catch (error) {
        this.showNotification('alert-danger', 'Error adding option', 'bi bi-exclamation-triangle-fill')
      }
    },
    confirmDelete(option) {
      this.deletingOption = option
      const modal = new Modal(document.getElementById('deleteModal'))
      modal.show()
    },
    async deleteOption() {
      try {
        const response = await axios.delete(`${API_URL}/option/${this.deletingOption.id}`)
        if (response.data.success) {
          this.showNotification('alert-success', 'Option deleted successfully!', 'bi bi-check-circle-fill')
          this.fetchOptions()
          const modal = Modal.getInstance(document.getElementById('deleteModal'))
          modal.hide()
          this.deletingOption = null
        }
      } catch (error) {
        this.showNotification('alert-danger', 'Error deleting option', 'bi bi-exclamation-triangle-fill')
      }
    }
  }
}
</script>

<style scoped>
.bg-gradient-warning {
  background: linear-gradient(135deg, #feca57, #ff9f43) !important;
}

.bg-gradient-danger {
  background: linear-gradient(135deg, #ff6b6b, #ee5a24) !important;
}

.option-name {
  font-size: 1.05rem;
  color: #2d3436;
}

.list-enter-active {
  transition: all 0.4s ease;
}

.list-leave-active {
  transition: all 0.3s ease;
}

.list-enter-from {
  opacity: 0;
  transform: translateX(30px);
}

.list-leave-to {
  opacity: 0;
  transform: translateX(-30px);
}

.list-move {
  transition: transform 0.4s ease;
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
