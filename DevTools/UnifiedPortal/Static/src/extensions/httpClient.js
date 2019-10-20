import axios from "axios";
import { getTokens } from "@/extensions/tokens";
import { REFRESH_REQUEST } from "@/constants/actions";
import { LOCALHOST, SYRINX } from "@/constants/servers";
import store from "@/plugins/store";

function getHostAndApi(env = {}, server) {
    let host, api;
    if (server === LOCALHOST) {
        host = "";
        api = "api";
    } else if (server === SYRINX) {
        host = env.syrinx.host;
        api = env.syrinx.api;
    } else {
        throw Error(`Unknown server type: ${server}`);
    }
    return {host, api};
}

export default class HttpClient {
    constructor() {
        this.axios = axios.create();
    }

    static init() {
        return new HttpClient();
    }

    get(server, endpoint, config = null) {
        let {host, api} = getHostAndApi(store.getters.getEnvironmentVariables, server);
        return this.axios.get(`${host}/${api}/${endpoint}`, config);
    }

    post(server, endpoint, data = null, config = undefined) {
        let {host, api} = getHostAndApi(store.getters.getEnvironmentVariables, server);
        return this.axios.post(`${host}/${api}/${endpoint}`, data, config);
    }

    put(server, endpoint, data = null, config = undefined) {
        let {host, api} = getHostAndApi(store.getters.getEnvironmentVariables, server);
        return this.axios.put(`${host}/${api}/${endpoint}`, data, config);
    }

    handleUnauthorizedResponse = (dispatch) => {
        this.axios.interceptors.response.use(response => response, error => {
            const {accessToken, refreshToken} = getTokens();
            if (error.response.status === 401 && accessToken && refreshToken) {
                dispatch(REFRESH_REQUEST);
            }
            return Promise.resolve();
        });
        return this;
    };
}