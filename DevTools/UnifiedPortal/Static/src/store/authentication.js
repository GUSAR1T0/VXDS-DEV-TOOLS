import HttpClient from "@/extensions/httpClient";
import { getTokens, setTokens, removeTokens } from "@/extensions/tokens";
import { getConfiguration } from "@/extensions/utils";
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
        [ON_LOAD_REQUEST]: ({commit, dispatch}, redirectTo) => {
            return new Promise((resolve, reject) => {
                const {accessToken, refreshToken} = getTokens();
                if (accessToken && refreshToken) {
                    HttpClient.init().then(client => {
                        client.get(GET_USER_DATA_ENDPOINT, getConfiguration(accessToken)).then(response => {
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
                    client.post(SIGN_IN_ENDPOINT, signInForm).then(firstResponse => {
                        setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                        client.get(GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken)).then(secondResponse => {
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
                    client.post(SIGN_UP_ENDPOINT, signUpForm).then(firstResponse => {
                        setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                        client.get(GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken)).then(secondResponse => {
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
                        client.post(REFRESH_ENDPOINT, {accessToken, refreshToken}).then(firstResponse => {
                            setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                            client.get(GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken)).then(secondResponse => {
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
                        client.post(LOGOUT_ENDPOINT, null, getConfiguration(accessToken)).then(() => {
                            commit(LOGOUT_REQUEST);
                            removeTokens();
                            resolve();
                        }).catch(() => {
                            dispatch(REFRESH_REQUEST).then(() => {
                                const {accessToken} = getTokens();
                                client.post(LOGOUT_ENDPOINT, null, getConfiguration(accessToken)).then(() => {
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