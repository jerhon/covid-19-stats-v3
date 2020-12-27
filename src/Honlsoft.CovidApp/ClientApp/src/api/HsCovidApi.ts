import {NationClient, StateClient} from "./hs-covid-19-v1";

const baseUrl = "/api";

export function getNationClient() {
    return new NationClient(baseUrl);
}

export function getStateClient() {
    return new StateClient(baseUrl);
}