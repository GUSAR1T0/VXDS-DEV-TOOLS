import HttpClient from "@/extensions/httpClient";
import { getTokens, setTokens, removeTokens } from "@/extensions/tokens";
import { getConfiguration, getUserFullName, getUserInitials } from "@/extensions/utils";
import {
    GET_USER_DATA_ENDPOINT,
    LOGOUT_ENDPOINT,
    REFRESH_ENDPOINT,
    SIGN_IN_ENDPOINT,
    SIGN_UP_ENDPOINT
} from "@/constants/endpoints";
import {
    LOGOUT_REQUEST,
    ON_LOAD_REQUEST,
    REFRESH_REQUEST,
    SIGN_IN_REQUEST,
    SIGN_UP_REQUEST
} from "@/constants/actions";
import { SYRINX } from "@/constants/servers";

export default {
    state: {
        isAuthenticated: false,
        email: "",
        firstName: "",
        lastName: "",
        color: ""
    },
    getters: {
        isAuthenticated: state => {
            return state.isAuthenticated;
        },
        getEmail: state => {
            return state.email;
        },
        getFullName: state => {
            return getUserFullName(state.firstName, state.lastName);
        },
        getInitials: state => {
            return getUserInitials(state.firstName, state.lastName);
        },
        getColor: state => {
            return state.color;
        }
    },
    mutations: {
        [SIGN_IN_REQUEST]: (state, data) => {
            state.isAuthenticated = true;
            state.email = data.email;
            state.firstName = data.firstName;
            state.lastName = data.lastName;
            state.color = data.color;
        },
        [LOGOUT_REQUEST]: state => {
            state.isAuthenticated = false;
            state.email = "";
            state.firstName = "";
            state.lastName = "";
            state.color = "";
        }
    },
    actions: {
        [ON_LOAD_REQUEST]: ({commit, dispatch}, redirectTo) => {
            return new Promise((resolve, reject) => {
                const {accessToken, refreshToken} = getTokens();
                if (accessToken && refreshToken) {
                    HttpClient.init().then(client => {
                        client.get(SYRINX, GET_USER_DATA_ENDPOINT, getConfiguration(accessToken)).then(response => {
                            commit(SIGN_IN_REQUEST, response.data);
                            resolve(redirectTo);
                        }).catch(() => {
                            dispatch(REFRESH_REQUEST).then(() => {
                                resolve(redirectTo);
                            }).catch(() => {
                                reject();
                            });
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
        [SIGN_IN_REQUEST]: ({commit}, signInForm) => {
            return new Promise((resolve, reject) => {
                HttpClient.init().then(client => {
                    client.post(SYRINX, SIGN_IN_ENDPOINT, signInForm, undefined).then(firstResponse => {
                        setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                        client.get(SYRINX, GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken))
                            .then(secondResponse => {
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
                }).catch(error => {
                    removeTokens();
                    reject(error);
                });
            });
        },
        [SIGN_UP_REQUEST]: ({commit}, signUpForm) => {
            return new Promise((resolve, reject) => {
                HttpClient.init().then(client => {
                    client.post(SYRINX, SIGN_UP_ENDPOINT, signUpForm, undefined).then(firstResponse => {
                        setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                        client.get(SYRINX, GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken))
                            .then(secondResponse => {
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
                }).catch(error => {
                    removeTokens();
                    reject(error);
                });
            });
        },
        [REFRESH_REQUEST]: ({commit}) => {
            return new Promise((resolve, reject) => {
                const {accessToken, refreshToken} = getTokens();
                if (accessToken && refreshToken) {
                    HttpClient.init().then(client => {
                        client.post(SYRINX, REFRESH_ENDPOINT, {accessToken, refreshToken}, undefined).then(firstResponse => {
                            setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                            client.get(SYRINX, GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken))
                                .then(secondResponse => {
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
        [LOGOUT_REQUEST]: ({commit, dispatch}) => {
            return new Promise((resolve, reject) => {
                const {accessToken, refreshToken} = getTokens();
                if (accessToken && refreshToken) {
                    HttpClient.init().then(client => {
                        client.post(SYRINX, LOGOUT_ENDPOINT, null, getConfiguration(accessToken)).then(() => {
                            commit(LOGOUT_REQUEST);
                            removeTokens();
                            resolve();
                        }).catch(() => {
                            dispatch(REFRESH_REQUEST).then(() => {
                                const {accessToken} = getTokens();
                                client.post(SYRINX, LOGOUT_ENDPOINT, null, getConfiguration(accessToken)).then(() => {
                                    commit(LOGOUT_REQUEST);
                                    removeTokens();
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
                    }).catch(error => {
                        removeTokens();
                        reject(error);
                    });
                } else {
                    removeTokens();
                    resolve();
                }
            });
        }
    }
};