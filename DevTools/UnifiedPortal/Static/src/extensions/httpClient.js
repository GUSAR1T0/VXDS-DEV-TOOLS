import axios from "axios";
import { getTokens } from "@/extensions/tokens";
import { REFRESH_REQUEST } from "@/constants/actions";

export default class HttpClient {
    constructor(syrinxHost, syrinxApi) {
        this.syrinxHost = syrinxHost;
        this.syrinxApi = syrinxApi;
        this.axios = axios.create();
    }

    static async init() {
        // TODO: Move out, add to store or make as global
        let env = (await axios.get("/env")).data;
        let syrinxHost = env["SYRINX_HOST"];
        let syrinxApi = env["SYRINX_API"];
        if (!syrinxHost || !syrinxApi) {
            throw Error("No info about Syrinx server host URL and/or API entry-point");
        }
        return new HttpClient(syrinxHost, syrinxApi);
    }

    get(endpoint, config = null) {
        return this.axios.get(`${this.syrinxHost}/${this.syrinxApi}/${endpoint}`, config);
    }

    post(endpoint, data = null, config = null) {
        return this.axios.post(`${this.syrinxHost}/${this.syrinxApi}/${endpoint}`, data, config);
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