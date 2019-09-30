import axios from "axios";
import { getTokens } from "@/extensions/tokens";
import { REFRESH_REQUEST } from "@/constants/actions";

export default class HttpClient {
    constructor() {
        this.axios = axios.create();
    }

    // eslint-disable-next-line no-unused-vars
    handleUnauthorizedResponse = ({commit, dispatch}) => {
        this.axios.interceptors.response.use(response => response, error => {
            const {accessToken, refreshToken} = getTokens();
            if (error.response.status === 401 && accessToken && refreshToken) {
                dispatch(REFRESH_REQUEST);
            }
            return Promise.reject(error);
        });
        return this;
    };
}