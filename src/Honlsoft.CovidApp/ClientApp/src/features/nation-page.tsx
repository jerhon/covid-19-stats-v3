import React from "react";
import {UsMap} from "./us-map/us-map";
import {useDispatch, useSelector} from "react-redux";
import {Card, CardContent, CardHeader, Divider, Grid } from "@material-ui/core";
import * as statementSlice from "./state.slice";
import {makeStyles} from "@material-ui/core/styles";
import {StatisticHeatMap} from "./charts/statistic-heat-map";
import {StateTable} from "./state-table";


const useStyles = makeStyles({
    chart: {
        height: '250px',
        marginLeft: 'auto',
        marginRight: 'auto'
    },
    card: {
        marginTop: '12px',
        marginBottom: '12px',
    }
});

export function NationPage() {
    const dispatch = useDispatch();
    const stateName = useSelector(statementSlice.selectStateName) ?? "United States";
    const stateSelected = !!useSelector(statementSlice.selectStateName);
    const styles = useStyles();
    
    return (<Grid container>
        <Grid item container>
            <Grid item xs={12} md={8}>
                <UsMap stateOptions={{}}
                       onStateClicked={(e) => dispatch(statementSlice.requestStateInfo(e.state))}
                       selected={stateName} />
            </Grid>
            {stateSelected && <Grid item xs={12} md={4} alignItems="center" justify="center" >
                <Card className={styles.card} variant="outlined">
                    <CardHeader title={stateName} />
                    <Divider />
                    <CardContent>
                        <StateTable />
                    </CardContent>
                </Card>
            </Grid>}
        </Grid>
        {stateSelected && <Grid item container direction="row" spacing={4} >
            <Grid item md={6}>
                <Card className={styles.card} variant="outlined">
                    <CardHeader title="Positive Tests" />
                    <Divider />
                    <CardContent>
                        <div className={styles.chart}>
                            <StatisticHeatMap statistic="positiveIncrease" />
                        </div>
                    </CardContent>
                </Card>
            </Grid>
            <Grid item md={6}>
                <Card className={styles.card} variant="outlined">
                    <CardHeader title="Deaths" />
                    <Divider />
                    <CardContent>
                        <div className={styles.chart}>
                            <StatisticHeatMap statistic="deathIncrease" />
                        </div>
                    </CardContent>
                </Card>
            </Grid>
        </Grid>}
    </Grid>)
}