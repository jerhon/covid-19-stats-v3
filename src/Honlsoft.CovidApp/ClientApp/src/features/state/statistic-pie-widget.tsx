import {useSelector} from "react-redux";
import * as statementSlice from "../state.slice";
import React from "react";
import {AggregateDataPointsDto} from "../../api";
import {CircularProgress} from "@material-ui/core";
import {Pie, ResponsivePie} from "@nivo/pie";


interface MyPieProperties
{
    data?: { id: string, value: number }[]
}

function MyPie({ data }: MyPieProperties) {
    
    const legends = [
        {
            anchor: 'bottom' as 'bottom',
            direction: 'row' as 'row',
            translateY: 56,
            itemWidth: 100,
            itemHeight: 18,
            itemTextColor: '#999',
            symbolSize: 18,
            symbolShape: 'square',
            effects: [
                {
                    on: 'hover' as 'hover',
                    style: {
                        itemTextColor: '#000',
                    },
                },
            ],
        },
    ]

    if (!data) {
        return (<CircularProgress /> );
    }

    return (<ResponsivePie 
             data={data}
             margin={{ top: 40, right: 80, bottom: 80, left: 80 }}
             innerRadius={0.25}
             padAngle={0.7}
             cornerRadius={0}
             colors={{ scheme: 'nivo' }}
             borderWidth={1}
             borderColor={{ from: 'color', modifiers: [ [ 'darker', 0.2 ] ] }}
             radialLabelsSkipAngle={0}
             radialLabelsTextColor="#333333"
             radialLabelsLinkColor={{ from: 'color' }}
             sliceLabelsSkipAngle={0}
             sliceLabelsTextColor="#333333"
             legends={legends} />
    )
}

export interface StatisticPieWidgetProps {
    statistic: keyof AggregateDataPointsDto;
}

export function StatisticPieWidget({ statistic }: StatisticPieWidgetProps) {
    const state = useSelector(statementSlice.selector);


    if (!state?.data?.aggregate || !state?.data?.aggregate?.total || !state?.data?.aggregate?.state ) {
        return (<MyPie />);
    } else {
        const total = state.data.aggregate.total;
        const current = state.data.aggregate.state;
        const data = [
            { id: 'Country', value: (total[statistic] ?? 0) - (current[statistic] ?? 0) },
            { id: state.data.aggregate.name ?? 'Unknown', value: current[statistic] ?? 0 }
        ]

        return (<MyPie data={data} />);
    }
}
