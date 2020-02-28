import {
    GET_HTTP_REQUEST,
    LOAD_SETTINGS_REQUEST,
    PUT_HTTP_REQUEST,
    SETUP_GITHUB_TOKEN_REQUEST
} from "@/constants/actions";
import { LOCALHOST } from "@/constants/servers";
import { getConfiguration } from "@/extensions/utils";
import { LOAD_SETTINGS_ENDPOINT, SETUP_GITHUB_TOKEN_ENDPOINT } from "@/constants/endpoints";
import format from "string-format";

export default {
    state: {
        codeServicesSettings: {
            gitHubUser: undefined
        },
    },
    getters: {
        getCodeServicesSettings: state => {
            return state.codeServicesSettings;
        }
    },
    mutations: {
        [LOAD_SETTINGS_REQUEST]: (state, data) => {
            state.codeServicesSettings.gitHubUser = data.codeServicesSettings.gitHubUser;
        },
        [SETUP_GITHUB_TOKEN_REQUEST]: (state, data) => {
            state.codeServicesSettings.gitHubUser = data;
        }
    },
    actions: {
        [LOAD_SETTINGS_REQUEST]: ({commit, dispatch}) => {
            return new Promise((resolve, reject) => {
                dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: LOAD_SETTINGS_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    commit(LOAD_SETTINGS_REQUEST, response.data);
                    resolve();
                }).catch(error => reject(error));
            });
        },
        [SETUP_GITHUB_TOKEN_REQUEST]: ({commit, dispatch}, token) => {
            return new Promise((resolve, reject) => {
                dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(SETUP_GITHUB_TOKEN_ENDPOINT, {
                        token: token
                    }),
                    config: getConfiguration()
                }).then(response => {
                    commit(SETUP_GITHUB_TOKEN_ENDPOINT, response.data);
                    resolve(response.data);
                }).catch(error => reject(error));
            });
        }
    }
};