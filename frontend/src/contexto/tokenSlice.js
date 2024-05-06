import { createSlice } from "@reduxjs/toolkit";

export const tokenSlice = createSlice({
    name: "token",
    initialState: {
        value: ''
    },
    reducers: {
        setter: (state, action) => {
            state.value = action.payload
            console.log("tentativa de guardar o token de uma forma segura: \n" + state.value)
            console.log(state)
        }
    }
})
export const { setter } = tokenSlice.actions
export default tokenSlice.reducer