import React from "react";
import {UsMap} from "./us-map/us-map";
import {useDispatch, useSelector} from "react-redux";
import {Card, CardContent, CardHeader, Divider, Grid, Typography} from "@material-ui/core";
import * as statementSlice from "./state.slice";

export function NationPage() {
    const dispatch = useDispatch();
    const stateName = useSelector(statementSlice.selectStateName);
    
    return (<Grid container>
        <Grid item xs={12} md={9}>
            <UsMap stateOptions={{}}
                   onStateClicked={(e) => dispatch(statementSlice.requestStateInfo(e.state))}
                   selected={stateName} />
        </Grid>
        <Grid item xs={12} md={3}>
            <Card variant="elevation" elevation={1}>
                <CardHeader title={stateName} />
                <Divider />
                <CardContent>
                    Here's the data!
                </CardContent>
            </Card>
        </Grid>
    </Grid>)
}