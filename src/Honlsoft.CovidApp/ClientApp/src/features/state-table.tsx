import {shallowEqual, useDispatch, useSelector} from "react-redux";
import * as stateSlice from "./state.slice";
import React from "react";
import { Table, TableCell, TableContainer, TableRow} from "@material-ui/core";
import TableBody from "@material-ui/core/TableBody";


export function StateTable() {
    const state = useSelector(stateSlice.selector, shallowEqual);
    
    if ( !state.data || !state.data.stateInfo || !state.data.aggregate || !state.data.stateInfo.dataPoints ) {
        return (<div>Loading</div>)
    }

    let latest = state.data.stateInfo.dataPoints[0];

    const values = [
        { name: "positive", description: "Positive" },
        { name: "negative", description: "Negative" },
        { name: "death", description: "Deaths" },
        { name: "hospitalized", description: "Hospitalized" }];
    
    const rows = values.filter((v) => latest[v.name as keyof typeof latest] !== undefined)
        .map((v) => <TableRow key={v.name}>
            <TableCell>{v.description}</TableCell>
            <TableCell align="right">{latest[v.name as keyof typeof latest]}</TableCell>
        </TableRow>)

    return (<TableContainer>
                <Table>
                    <TableBody>
                        {rows}
                    </TableBody>
                </Table>
            </TableContainer>)
}