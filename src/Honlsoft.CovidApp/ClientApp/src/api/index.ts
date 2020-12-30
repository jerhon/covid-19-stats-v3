import { NationApiFactory, StateApiFactory } from "./hs-covid-19/api"

const baseUrl = "";

export function getNationClient() {
    return NationApiFactory(undefined, '', undefined);
}

export function getStateClient() {

    return StateApiFactory(undefined, '', undefined);
}