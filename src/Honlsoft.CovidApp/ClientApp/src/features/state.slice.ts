import { createAsyncThunk } from "@reduxjs/toolkit";
import { GetStateDataDto, AggregateDto } from "../api/hs-covid-19/api";
import { createAsyncSlice } from "../AsyncSlice";
import { getStateClient } from "../api";

export interface StateSliceState {
    notRequested: boolean;
    pending: boolean;
    error: boolean;
    complete: boolean;
    data?: { stateInfo: GetStateDataDto, aggregate: AggregateDto };
}

export const requestStateInfo = createAsyncThunk('state/request', async (state: string) => {
    const api = getStateClient();
    
    const aggregate = (await api.stateGetAggregate(state, undefined)).data;
    const stateInfo = (await api.stateGetStateData(state)).data;
    
    return { aggregate, stateInfo }
});

export const stateSlice = createAsyncSlice('state', requestStateInfo);

export const reducer = { state: stateSlice.reducer };
export const selector = (state: any) => state.state as StateSliceState;
export const selectStateName = (state: any) => selector(state)?.data?.stateInfo?.name;