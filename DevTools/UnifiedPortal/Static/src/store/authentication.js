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
    ON_LOAD_ACCOUNT_REQUEST,
    LOGOUT_REQUEST,
    REFRESH_REQUEST,
    SIGN_IN_REQUEST,
    SIGN_UP_REQUEST
} from "@/constants/actions";
import { SYRINX } from "@/constants/servers";

export default {
    state: {
        isAuthenticated: false,
        id: null,
        firstName: "",
        lastName: "",
        color: "",
        userPermissions: 0,
        userRolePermissions: 0
    },
    getters: {
        isAuthenticated: state => {
            return state.isAuthenticated;
        },
        getUserId: state => {
            return state.id;
        },
        getFullName: state => {
            return getUserFullName(state.firstName, state.lastName);
        },
        getInitials: state => {
            return getUserInitials(state.firstName, state.lastName);
        },
        getColor: state => {
            return state.color;
        },
        hasUserPermission: state => permission => {
            return permission === 0 || (state.userPermissions & permission) !== 0;
        },
        hasUserRolePermission: state => permission => {
            return permission === 0 || (state.userRolePermissions & permission) !== 0;
        }
    },
    mutations: {
        [SIGN_IN_REQUEST]: (state, data) => {
            state.isAuthenticated = true;
            state.id = data.id;
            state.firstName = data.firstName;
            state.lastName = data.lastName;
            state.color = data.color;
            state.userPermissions = data.userPermissions;
            state.userRolePermissions = data.userRolePermissions;
        },
        [LOGOUT_REQUEST]: state => {
            state.isAuthenticated = false;
            state.id = null;
            state.firstName = "";
            state.lastName = "";
            state.color = "";
            state.userPermissions = 0;
            state.userRolePermissions = 0;
        }
    },
    actions: {
        [ON_LOAD_ACCOUNT_REQUEST]: ({commit, dispatch}, redirectTo) => {
            return new Promise((resolve, reject) => {
                const {accessToken, refreshToken} = getTokens();
                if (accessToken && refreshToken) {
                    HttpClient.init().get(SYRINX, GET_USER_DATA_ENDPOINT, getConfiguration(accessToken))
                        .then(response => {
                            commit(SIGN_IN_REQUEST, response.data);
                            resolve(redirectTo);
                        })
                        .catch(() => {
                            dispatch(REFRESH_REQUEST).then(() => resolve(redirectTo)).catch(() => reject());
                        });
                } else {
                    removeTokens();
                    reject();
                }
            });
        },
        [SIGN_IN_REQUEST]: ({commit}, signInForm) => {
            return new Promise((resolve, reject) => {
                let client = HttpClient.init();
                client.post(SYRINX, SIGN_IN_ENDPOINT, signInForm).then(firstResponse => {
                    setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                    client.get(SYRINX, GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken))
                        .then(secondResponse => {
                            commit(SIGN_IN_REQUEST, secondResponse.data);
                            resolve();
                        })
                        .catch(error => {
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
                let client = HttpClient.init();
                client.post(SYRINX, SIGN_UP_ENDPOINT, signUpForm).then(firstResponse => {
                    setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                    client.get(SYRINX, GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken))
                        .then(secondResponse => {
                            commit(SIGN_IN_REQUEST, secondResponse.data);
                            resolve();
                        })
                        .catch(error => {
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
                    let client = HttpClient.init();
                    client.post(SYRINX, REFRESH_ENDPOINT, {accessToken, refreshToken}).then(firstResponse => {
                        setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                        client.get(SYRINX, GET_USER_DATA_ENDPOINT, getConfiguration(firstResponse.data.accessToken))
                            .then(secondResponse => {
                                commit(SIGN_IN_REQUEST, secondResponse.data);
                                resolve();
                            })
                            .catch(error => {
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
                    let client = HttpClient.init();
                    client.post(SYRINX, LOGOUT_ENDPOINT, null, getConfiguration(accessToken)).then(() => {
                        commit(LOGOUT_REQUEST);
                        removeTokens();
                        resolve();
                    }).catch(() => {
                        dispatch(REFRESH_REQUEST).then(() => {
                            const {accessToken} = getTokens();
                            client.post(SYRINX, LOGOUT_ENDPOINT, null, getConfiguration(accessToken))
                                .then(() => {
                                    commit(LOGOUT_REQUEST);
                                    removeTokens();
                                    resolve();
                                })
                                .catch(error => {
                                    removeTokens();
                                    reject(error);
                                });
                        }).catch(error => {
                            removeTokens();
                            reject(error);
                        });
                    });
                } else {
                    removeTokens();
                    resolve();
                }
            });
        }
    }
};