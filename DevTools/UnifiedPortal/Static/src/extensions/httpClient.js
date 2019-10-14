import axios from "axios";
import { getTokens } from "@/extensions/tokens";
import { REFRESH_REQUEST } from "@/constants/actions";
import { LOCALHOST, SYRINX } from "@/constants/servers";

function getHostAndApi(env = {}, server = LOCALHOST) {
    let host, api;
    if (server === LOCALHOST) {
        host = "";
        api = env["LOCALHOST_API"];
    } else if (server === SYRINX) {
        host = env["SYRINX_HOST"];
        api = env["SYRINX_API"];
    } else {
        throw Error(`Unknown server type: ${server}`);
    }
    return {host, api};
}

export default class HttpClient {
    constructor(env) {
        this.env = env;
        this.axios = axios.create();
    }

    static async init() {
        // TODO: Move out, add to store or make as global
        let env = (await axios.get("/env")).data;
        if (!env["LOCALHOST_API"]) {
            throw Error("No info about localhost server API entry-point");
        }
        if (!env["SYRINX_HOST"] || !env["SYRINX_API"]) {
            throw Error("No info about Syrinx server host URL and/or API entry-point");
        }
        return new HttpClient(env);
    }

    get(server, endpoint, config = null) {
        let {host, api} = getHostAndApi(this.env, server);
        return this.axios.get(`${host}/${api}/${endpoint}`, config);
    }

    post(server, endpoint, data = null, config = undefined) {
        let {host, api} = getHostAndApi(this.env, server);
        return this.axios.post(`${host}/${api}/${endpoint}`, data, config);
    }
    
    put(server, endpoint, data = null, config = undefined) {
        let {host, api} = getHostAndApi(this.env, server);
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