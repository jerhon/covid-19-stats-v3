import {createAsyncThunk, createSlice, PayloadAction} from "@reduxjs/toolkit";

export interface NationState {
    selected?: string;
}


export const nationSlice = createSlice({
    name: 'nation',
    initialState: {} as NationState,
    reducers: {
        setSelected: (state, action: PayloadAction<Pick<NationState, "selected">>) => {
            state.selected = action.payload.selected;
        }       
    }
});

export const getLatestNationStats = createAsyncThunk('nation/latest', () => {
       
})

const selector = (state: any) => state.nation as NationState;

export const actions = nationSlice.actions;
export const reducer = { nation: nationSlice.reducer }
export default selector;