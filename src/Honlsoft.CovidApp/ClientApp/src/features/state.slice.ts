import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {StateApi, GetStateDataDto, AggregateDto} from "../api";
import {createAsyncSlice} from "../AsyncSlice";

export interface StateSliceState {
    notRequested: boolean;
    pending: boolean;
    error: boolean;
    complete: boolean;
    data?: { stateInfo: GetStateDataDto, aggregate: AggregateDto };
}

export const requestStateInfo = createAsyncThunk('state/request', async (state: string) => {
    const api = new StateApi(undefined, '');
    
    const aggregate = (await api.apiV1StatesStateAbbreviationAggregateGet(state)).data;
    const stateInfo = (await api.apiV1StatesStateAbbreviationGet(state)).data;
    
    return { aggregate, stateInfo }
});

export const stateSlice = createAsyncSlice('state', requestStateInfo);

export const actions = { requestStateInfo }
export const reducer = { state: stateSlice.reducer };
export const selector = (state: any) => state.state as StateSliceState;
