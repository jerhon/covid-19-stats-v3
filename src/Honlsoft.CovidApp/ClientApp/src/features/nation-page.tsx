import React from "react";
import {UsMap} from "./us-map/us-map";
import selector, { actions } from "./nation.slice"
import {useDispatch, useSelector} from "react-redux";

export function NationPage() {
    
    const dispatch = useDispatch();
    const selectedState = useSelector(selector);
    
    return (<div>
        <UsMap stateOptions={{}}
               onStateClicked={(e) => dispatch(actions.setSelected({ selected: e.state }))}
               selected={selectedState?.selected} />
    </div>)
}