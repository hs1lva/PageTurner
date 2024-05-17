import { useContext } from 'react';
import ApiService from '../services/ApiService';
import { url_server } from '../contexto/url_servidor';
import { SearchContext } from '../contexto/SearchContext'; // Certifique-se de que o caminho está correto

const apiService = new ApiService(url_server());

const useSearch = () => {
    const { searchResults, setSearchResults, isLoading, setIsLoading } = useContext(SearchContext);

    const search = async (query) => {
        if (!query) return;
        setIsLoading(true);
        try {
            const data = await apiService.searchBooks(query);
            setSearchResults(data.docs);
        } catch (error) {
            console.error(error);
        } finally {
            setIsLoading(false);
        }
    };

    return { search, searchResults, isLoading };
};

export default useSearch;
