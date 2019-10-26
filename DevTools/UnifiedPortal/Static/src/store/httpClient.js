import {
    DELETE_HTTP_REQUEST,
    GET_HTTP_REQUEST,
    POST_HTTP_REQUEST,
    PUT_HTTP_REQUEST,
    STORE_ENVIRONMENT_VARIABLES_FOR_HTTP_CLIENT
} from "@/constants/actions";
import axios from "axios";
import { LOCALHOST, SYRINX } from "@/constants/servers";

let printResponseErrors = error => {
    let message = "";
    if (error.response) {
        message += `\n -> STATUS: ${error.response.statusText} (${error.response.status})`;
        if (error.response.data) {
            message += `\n -> MESSAGE: ${error.response.data.message}`;
            message += `\n -> OPERATION ID: ${error.response.data.operationId}`;
        }
        if (error.response.config) {
            message += `\n -> URL: ${error.response.config.method.toUpperCase()} ${error.response.config.url}`;
            message += `\n -> REQUEST: ${JSON.stringify(error.response.config.data, null, 4)}`;
        }
    }

    // eslint-disable-next-line no-console
    console.error(`The error issue is caused: ${message ? message : "Unhandled exception"}`);
    return Promise.reject(error);
};

let getHostAndApi = (state, server) => {
    let host, api;
    if (server === LOCALHOST) {
        host = "";
        api = "api";
    } else if (server === SYRINX) {
        let env = state.environmentValues;
        host = env.syrinx.host;
        api = env.syrinx.api;
    } else {
        throw Error(`Unknown server type: ${server}`);
    }
    return {host, api};
};

let sendRequest = (state, payload, request) => {
    let client = axios.create();
    client.interceptors.response.use(undefined, error => {
        if (error.response.status === 401) {
            if (payload.ignoreReloadPage !== true) {
                window.location.reload();
            }
        }
        return printResponseErrors(error);
    });
    let {host, api} = getHostAndApi(state, payload.server);
    return request(client, host, api);
};

export default {
    state: {
        environmentValues: {}
    },
    mutations: {
        [STORE_ENVIRONMENT_VARIABLES_FOR_HTTP_CLIENT]: (state, environmentValues) => {
            state.environmentValues = environmentValues;
        }
    },
    actions: {
        [GET_HTTP_REQUEST]: ({state}, payload = {}) => {
            return sendRequest(state, payload, (client, host, api) => client.get(`${host}/${api}/${payload.endpoint}`, payload.config));
        },
        [POST_HTTP_REQUEST]: ({state}, payload = {}) => {
            return sendRequest(state, payload, (client, host, api) => client.post(`${host}/${api}/${payload.endpoint}`, payload.data, payload.config));
        },
        [PUT_HTTP_REQUEST]: ({state}, payload = {}) => {
            return sendRequest(state, payload, (client, host, api) => client.put(`${host}/${api}/${payload.endpoint}`, payload.data, payload.config));
        },
        [DELETE_HTTP_REQUEST]: ({state}, payload = {}) => {
            return sendRequest(state, payload, (client, host, api) => client.delete(`${host}/${api}/${payload.endpoint}`, payload.config));
        }
    }
};