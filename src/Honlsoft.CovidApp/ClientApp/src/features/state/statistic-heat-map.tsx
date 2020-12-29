import {shallowEqual, useSelector} from "react-redux";
import * as statementSlice from "../state.slice";
import {Skeleton} from "@material-ui/lab";
import {ResponsiveCalendar} from "@nivo/calendar";
import {RedColorScale} from "../../utils/colorScales";
import React from "react";
import {CovidStateDailyRecord} from "../../api/hs-covid-19-v1";



function formatDate(date: string | Date | undefined) {
    if (date === undefined || date === null) {
        return '';
    } else if (date instanceof Date) {
        return date.getFullYear() + '-' + date.getMonth() + '-' + date.getDate() 
    } else {
        let t = date.toString().indexOf('T');
        if (t > 0) {
            return date.substring(0, t);
        }
    }
    return date;
}


interface StatisticHeatMapProps {
    statistic: keyof CovidStateDailyRecord
}

export function StatisticHeatMap({ statistic } : StatisticHeatMapProps) {
    const state = useSelector(statementSlice.selector, shallowEqual);

    if (!state?.data?.stateInfo?.dataPoints) {
        return <Skeleton />
    }

    let data =  state.data.stateInfo.dataPoints?.map((dp) => ({
        day: formatDate(dp.date),
        value: +(dp[statistic] ?? 0)
    })).filter((dp) => !!dp.day && !!dp.value)
    
    return (<ResponsiveCalendar
        data={data} from={data[data.length - 1].day}
        to={data[0].day}
        colors={RedColorScale}
        margin={{left: 24, top: 24, right: 24, bottom: 24}} />)
}
