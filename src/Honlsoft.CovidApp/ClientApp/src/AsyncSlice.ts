import {AsyncThunk, createSlice} from "@reduxjs/toolkit";

/** Represents the async state of the async thunk. */
export interface AsyncSliceState<DataType> {
    notRequested: boolean,
    pending: boolean,
    done: boolean,
    error: boolean,
    data?: DataType
}

/** Creates a slice for an async thunk that tracks the state of the thunk. */
export function createAsyncSlice<Returned, ThunkArg = void, ThunkApiConfig = {}>(name: string, thunk: AsyncThunk<Returned, ThunkArg, ThunkApiConfig>) {
    return createSlice(
        {
            name,
            initialState: {
                notRequested: true,
                pending: false,
                done: false,
                error: false,
            } as AsyncSliceState<Returned>,
            reducers: {},
            extraReducers: {
                [thunk.fulfilled.toString()]: (state, action) => {
                    state.data = action.payload;
                    state.pending = false
                    state.notRequested = false
                    state.done = true;
                },
                [thunk.pending.toString()]: (state, action) => {
                    state.pending = true
                    state.notRequested = false
                },
                [thunk.rejected.toString()]: (state, action) => {
                    state.error = true
                    state.pending = false;
                    state.notRequested = false;
                }
            }
        }
    );
}