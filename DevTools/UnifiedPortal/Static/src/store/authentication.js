import HttpClient from "@/extensions/httpClient";
import { getTokens, setTokens, removeTokens } from "@/extensions/tokens";
import { getHeaders } from "@/extensions/utils";
import apis from "@/constants/apis";
import {
    LOGOUT_REQUEST,
    ON_LOAD_REQUEST,
    REFRESH_REQUEST,
    SIGN_IN_REQUEST,
    SIGN_UP_REQUEST
} from "@/constants/actions";

export default {
    state: {
        firstName: "",
        lastName: "",
        isAuthenticated: false
    },
    getters: {
        isAuthenticated: state => {
            return state.isAuthenticated;
        },
        getFullName: state => {
            return `${state.firstName} ${state.lastName}`;
        }
    },
    mutations: {
        [SIGN_IN_REQUEST]: (state, data) => {
            state.isAuthenticated = true;
            state.firstName = data.firstName;
            state.lastName = data.lastName;
        },
        [LOGOUT_REQUEST]: state => {
            state.isAuthenticated = false;
            state.firstName = "";
            state.lastName = "";
        }
    },
    actions: {
        [ON_LOAD_REQUEST]: ({commit, dispatch}) => {
            return new Promise((resolve, reject) => {
                const {accessToken, refreshToken} = getTokens();
                if (accessToken && refreshToken) {
                    let client = new HttpClient();
                    client.axios.get(apis.GetUserData, getHeaders(accessToken)).then(response => {
                        commit(SIGN_IN_REQUEST, response.data);
                        resolve();
                    }).catch(() => {
                        dispatch(REFRESH_REQUEST);
                        resolve();
                    });
                } else {
                    removeTokens();
                    reject();
                }
            });
        },
        [SIGN_IN_REQUEST]: ({commit}, signInForm) => {
            return new Promise((resolve, reject) => {
                let client = new HttpClient();
                client.axios.post(apis.SignIn, signInForm).then(firstResponse => {
                    setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                    client.axios.get(apis.GetUserData, getHeaders(firstResponse.data.accessToken)).then(secondResponse => {
                        commit(SIGN_IN_REQUEST, secondResponse.data);
                        resolve();
                    }).catch(error => {
                        removeTokens();
                        reject(error);
                    });
                }).catch(error => {
                    removeTokens();
                    reject(error);
                });
            });
        },
        [SIGN_UP_REQUEST]: ({commit}, signUpForm) => {
            return new Promise((resolve, reject) => {
                let client = new HttpClient();
                client.axios.post(apis.SignUp, signUpForm).then(firstResponse => {
                    setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                    client.axios.get(apis.GetUserData, getHeaders(firstResponse.data.accessToken)).then(secondResponse => {
                        commit(SIGN_IN_REQUEST, secondResponse.data);
                        resolve();
                    }).catch(error => {
                        removeTokens();
                        reject(error);
                    });
                }).catch(error => {
                    removeTokens();
                    reject(error);
                });
            });
        },
        [REFRESH_REQUEST]: ({commit}, tokens) => {
            return new Promise((resolve, reject) => {
                if (tokens.accessToken && tokens.refreshToken) {
                    let client = new HttpClient();
                    client.post(apis.Refresh, tokens).then(firstResponse => {
                        setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                        client.axios.get(apis.GetUserData, getHeaders(firstResponse.data.accessToken)).then(secondResponse => {
                            commit(SIGN_IN_REQUEST, secondResponse.data);
                            resolve();
                        }).catch(error => {
                            removeTokens();
                            reject(error);
                        });
                    }).catch(error => {
                        removeTokens();
                        reject(error);
                    });
                } else {
                    removeTokens();
                    reject();
                }
            });
        },
        [LOGOUT_REQUEST]: ({commit}) => {
            return new Promise((resolve, reject) => {
                const {accessToken, refreshToken} = getTokens();
                if (accessToken && refreshToken) {
                    let client = new HttpClient();
                    client.axios.post(apis.Logout, null, getHeaders(accessToken)).then(() => {
                        commit(LOGOUT_REQUEST);
                        removeTokens();
                        resolve();
                    }).catch(error => {
                        removeTokens();
                        reject(error);
                    });
                } else {
                    removeTokens();
                    reject();
                }
            });
        }
    }
};