import {create} from 'zustand'
import { decodeToken } from "react-jwt";

const useAuthStore = create((set) => {
    const initialToken = localStorage.getItem('authToken');
    const initialUser = initialToken ? decodeToken(initialToken) : null;

    return {
        token: initialToken,
        user: initialUser,

        setToken: (newToken) => {
            localStorage.setItem('authToken', newToken);
            const user = decodeToken(newToken);
            set({ token: newToken, user });
        },
        getUser: () => {
            const state = useAuthStore.getState();
            return state.user;
        },
        isAuthenticated: () => {
            const token = useAuthStore.getState().token;
            return token && token !== 'null' && token !== 'undefined';
        },
        logout: () => {
            localStorage.removeItem('authToken');
            set({ token: null, user: null });
        },
    };
});

export default useAuthStore;