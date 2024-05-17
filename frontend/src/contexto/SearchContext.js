import React, { createContext, useState } from 'react';

export const SearchContext = createContext();

export const SearchProvider = ({ children }) => {
    const [searchResults, setSearchResults] = useState([]);
    const [isLoading, setIsLoading] = useState(false);

    return (
        <SearchContext.Provider value={{ searchResults, setSearchResults, isLoading, setIsLoading }}>
            {children}
        </SearchContext.Provider>
    );
};
