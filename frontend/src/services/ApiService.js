import axios from 'axios';

class ApiService {
  constructor(baseURL, token) {
    this.http = axios.create({
      baseURL,
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  async get(endpoint, id) {
    try {
      const response = await this.http.get(`${endpoint}/${id}`);
      return response.data;
    } catch (error) {
      throw new Error(`GET request failed: ${error.response.status}`);
    }
  }

  async post(endpoint, data = {}, params = {}) {
    try {
      const response = await this.http.post(endpoint, data, { params });
      return response.data;
    } catch (error) {
      console.log(error)
     // throw new Error(`POST request failed: ${error.response.status}`);
    }
  }

  async put(endpoint, data = {}, params = {}) {
    try {
      const response = await this.http.put(endpoint, data, { params });
      return response.data;
    } catch (error) {
      throw new Error(`PUT request failed: ${error.response.status}`);
    }
  }

  async delete(endpoint, params = {}) {
    try {
      const response = await this.http.delete(endpoint, { params });
      return response.data;
    } catch (error) {
      throw new Error(`DELETE request failed: ${error.response.status}`);
    }
  }
}

export default ApiService;