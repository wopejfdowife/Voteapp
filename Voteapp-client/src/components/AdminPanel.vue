<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">Admin Panel</h2>
    <div class="row">
      <div class="col-md-6">
        <div class="card">
          <div class="card-header bg-warning">
            <h5 class="mb-0">Add New Option</h5>
          </div>
          <div class="card-body">
            <div class="mb-3">
              <label class="form-label">Option Name</label>
              <input type="text" class="form-control" v-model="newOption.name">
            </div>
            <div class="mb-3">
              <label class="form-label">Description</label>
              <textarea class="form-control" v-model="newOption.description" rows="3"></textarea>
            </div>
            <button class="btn btn-primary" @click="addOption" :disabled="!newOption.name">
              Add Option
            </button>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="card">
          <div class="card-header bg-danger text-white">
            <h5 class="mb-0">Manage Options</h5>
          </div>
          <div class="card-body">
            <div class="list-group">
              <div v-for="option in options" :key="option.id" class="list-group-item">
                <div class="d-flex justify-content-between align-items-center">
                  <div>
                    <strong>{{ option.name }}</strong>
                    <small class="text-muted d-block">{{ option.description }}</small>
                  </div>
                  <button class="btn btn-danger btn-sm" @click="deleteOption(option.id)">
                    Delete
                  </button>
                </div>
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
  name: 'AdminPanel',
  data() {
    return {
      options: [],
      newOption: {
        name: '',
        description: ''
      }
    }
  },
  mounted() {
    this.fetchOptions()
  },
  methods: {
    async fetchOptions() {
      try {
        const response = await axios.get(`${API_URL}/options`)
        this.options = response.data
      } catch (error) {
        console.error('Error fetching options:', error)
      }
    },
    async addOption() {
      try {
        const response = await axios.post(`${API_URL}/option`, this.newOption)
        if (response.data.success) {
          alert('Option added successfully!')
          this.newOption = { name: '', description: '' }
          this.fetchOptions()
        }
      } catch (error) {
        console.error('Error adding option:', error)
        alert('Error adding option')
      }
    },
    async deleteOption(id) {
      if (confirm('Are you sure you want to delete this option?')) {
        try {
          const response = await axios.delete(`${API_URL}/option/${id}`)
          if (response.data.success) {
            alert('Option deleted!')
            this.fetchOptions()
          }
        } catch (error) {
          console.error('Error deleting option:', error)
          alert('Error deleting option')
        }
      }
    }
  }
}
</script>