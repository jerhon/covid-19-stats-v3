import { createAsyncThunk } from "@reduxjs/toolkit";
import { GetStateDataDto, AggregateDto} from "../api/hs-covid-19-v1";
import { createAsyncSlice } from "../AsyncSlice";
import { getStateClient } from "../api/HsCovidApi";

export interface StateSliceState {
    notRequested: boolean;
    pending: boolean;
    error: boolean;
    complete: boolean;
    data?: { stateInfo: GetStateDataDto, aggregate: AggregateDto };
}

export const requestStateInfo = createAsyncThunk('state/request', async (state: string) => {
    const api = getStateClient();
    
    const aggregate = await api.getAggregate(state, undefined);
    const stateInfo = await api.getStateData(state);
    
    return { aggregate, stateInfo }
});

export const stateSlice = createAsyncSlice('state', requestStateInfo);

export const actions = { requestStateInfo }
export const reducer = { state: stateSlice.reducer };
export const selector = (state: any) => state.state as StateSliceState;
