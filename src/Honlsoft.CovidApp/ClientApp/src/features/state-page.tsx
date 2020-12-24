import React, {useEffect} from "react";
import {shallowEqual, useDispatch, useSelector} from "react-redux";
import * as statementSlice from "./state.slice";
import {makeStyles} from "@material-ui/core/styles";
import clsx from "clsx";
import {useParams} from "react-router-dom"
import { Paper, Table, TableCell, TableContainer, TableRow} from "@material-ui/core";
import TableBody from "@material-ui/core/TableBody";
import {StatisticPieWidget} from "./state/statistic-pie-widget";
import {StatisticHeatMap} from "./state/statistic-heat-map";

const useStyles = makeStyles({
        calendar: {
            height: 300,
        },
        calendarContainer: {
            height: 300
        },
        topDashboardItem: {
            flex: '0 0 auto',
            padding: '8px',
            margin: '16px',
            minWidth: '300px',
            height: '325px'
        },
        topDashboardItemHeader: {
            fontWeight: 'bold',
            fontSize: '1.3em'
        },
        topDashboard: {
            display: 'flex',
            flexDirection: 'row',
            justifyContent: 'space-evenly'
        }
    })


export function StatePage() {
    const { id } = useParams<{id: string}>();
    const dispatch = useDispatch();
    const state = useSelector(statementSlice.selector, shallowEqual);
    const classes = useStyles();
    useEffect(() => {
        if (state.notRequested) {
            dispatch(statementSlice.actions.requestStateInfo(id.toUpperCase()));
        }
    }, [state.notRequested, dispatch, statementSlice.actions.requestStateInfo, id])

    if ( !state.data || !state.data.stateInfo || !state.data.aggregate || !state.data.stateInfo.dataPoints ) {
        return (<div>Loading</div>)
    }

    let latest = state.data.stateInfo.dataPoints[0];

    const values = [
        { name: "positive", description: "Positive" },
        { name: "negative", description: "Negative" },
        { name: "death", description: "Deaths" }];

    const rows = values.map((v) => <TableRow key={v.name}>
        <TableCell>{v.description}</TableCell>
        <TableCell align="right">{latest[v.name as keyof typeof latest]}</TableCell>
    </TableRow>)
    
    return (<div className={clsx(classes.calendarContainer)}>
        <h1>{state.data.stateInfo.name}</h1>
        <div className={classes.topDashboard}>
            <Paper className={classes.topDashboardItem} elevation={1}>
                <div className={classes.topDashboardItemHeader}>Statistics (All Time)</div>
                <TableContainer>
                    <Table>
                        <TableBody>
                            {rows}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Paper>
            <Paper className={classes.topDashboardItem} elevation={1}>
                <div className={classes.topDashboardItemHeader}>Positives (Last 7 Days)</div>
                <StatisticPieWidget statistic="positive" />
            </Paper>
            <Paper className={classes.topDashboardItem} elevation={1}>
                <div className={classes.topDashboardItemHeader}>Deaths (Last 7 Days)</div>
                <StatisticPieWidget statistic="death" />
            </Paper>
        </div>
        <div className={classes.calendar}>
            <StatisticHeatMap statistic="deathIncrease" />
            <StatisticHeatMap statistic="positiveIncrease" />
        </div>
    </div>)
}
