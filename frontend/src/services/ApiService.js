import axios from 'axios';
import { toast } from 'react-toastify';
class ApiService {
    constructor(baseURL, token) {
        this.http = axios.create({
            baseURL,
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        this.http.interceptors.response.use(
            response => response,
            error => {
                if (!error.response) {
                    // Handle network error
                    toast.error("Servidor indisponível. Tente novamente mais tarde.");
                } else if (error.response.status >= 500) {
                    // Handle server errors
                    toast.error("Erro no servidor. Tente novamente mais tarde.");
                }
                return Promise.reject(error);
            }
        );
    }


    async get(endpoint, id) {
        try {
            if (id != null) {
                const response = await this.http.get(`${endpoint}/${id}`);
                return response.data;
            }
            const response = await this.http.get(endpoint);
            return response.data;
        } catch (error) {
            throw new Error(`GET request failed: ${error.response.status}`);
        }
    }

    async searchBooks(titulo) {
        try {
            const response = await this.http.get(`/api/Livro/PesquisarLivro/OpenLibrary/Titulo/${encodeURIComponent(titulo)}`);
            return response.data;
        } catch (error) {
            throw new Error(`GET request failed: ${error.response?.status}`);
        }
    }


    async getEstante(endpoint, id) {
        try {
            const response = await this.http.get(endpoint);
            return response.data;
        } catch (error) {
            throw new Error(`GET request failed: ${error.response.status}`);
        }
    }

    async post(endpoint, data = {}, params = {}) {
        try {
            const response = await this.http.post(endpoint, data, {params});
            return response.data;
        } catch (error) {
            // Log the error and rethrow it for the calling function to handle
            console.error(`POST request failed: ${error}`);
            throw error;  // Rethrow the error to be caught in the calling function
        }
    }

    async postLivroOL(data) {
        try {
            const response = await this.http.post('/api/Livro/PostLivroOL', data);
            return response.data;
        } catch (error) {
            throw new Error(`POST request failed: ${error.response?.status}`);
        }
    }

    async put(endpoint, data = {}, params = {}) {
        try {
            const response = await this.http.put(endpoint, data, {params});
            return response.data;
        } catch (error) {
            throw new Error(`PUT request failed: ${error.response.status}`);
        }
    }

    async delete(endpoint, params = {}) {
        try {
            const response = await this.http.delete(endpoint, {params});
            return response.data;
        } catch (error) {
            throw new Error(`DELETE request failed: ${error.response.status}`);
        }
    }
}

export default ApiService;