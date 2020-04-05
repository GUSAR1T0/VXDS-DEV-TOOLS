import { getTokens, removeTokens, setTokens } from "@/extensions/tokens";
import { getConfiguration, getUserFullName, getUserInitials } from "@/extensions/utils";
import {
    GET_USER_DATA_ENDPOINT,
    LOGOUT_ENDPOINT,
    REFRESH_ENDPOINT,
    SIGN_IN_ENDPOINT,
    SIGN_UP_ENDPOINT
} from "@/constants/endpoints";
import {
    GET_HTTP_REQUEST,
    LOGOUT_REQUEST,
    POST_HTTP_REQUEST,
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
        email: "",
        color: "",
        userRoleId: 0,
        portalPermissions: 0
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
        getUserRoleId: state => {
            return state.userRoleId;
        },
        hasPortalPermission: state => permission => {
            return permission === 0 || (state.portalPermissions & permission) !== 0;
        },
        getUserCardData: state => {
            return {
                firstName: state.firstName,
                lastName: state.lastName,
                email: state.email,
                color: state.color
            };
        }
    },
    mutations: {
        [SIGN_IN_REQUEST]: (state, data) => {
            state.isAuthenticated = true;
            state.id = data.id;
            state.firstName = data.firstName;
            state.lastName = data.lastName;
            state.email = data.email;
            state.color = data.color;
            state.userRoleId = data.userRoleId;
            if (data.permissions) {
                let portalPermissions = data.permissions.filter(item => item.permissionGroupId === 1);
                state.portalPermissions = portalPermissions && portalPermissions.length > 0 ? portalPermissions[0].permissions : 0;
            }
        },
        [LOGOUT_REQUEST]: state => {
            state.isAuthenticated = false;
            state.id = null;
            state.firstName = "";
            state.lastName = "";
            state.email = "";
            state.color = "";
            state.userRoleId = 0;
            state.portalPermissions = 0;
        }
    },
    actions: {
        [SIGN_IN_REQUEST]: ({commit, dispatch}, signInForm) => {
            return new Promise((resolve, reject) => {
                dispatch(POST_HTTP_REQUEST, {
                    server: SYRINX,
                    endpoint: SIGN_IN_ENDPOINT,
                    data: signInForm,
                    ignoreReloadPage: true
                }).then(firstResponse => {
                    setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                    dispatch(GET_HTTP_REQUEST, {
                        server: SYRINX,
                        endpoint: GET_USER_DATA_ENDPOINT,
                        config: getConfiguration(firstResponse.data.accessToken),
                        ignoreReloadPage: true
                    }).then(secondResponse => {
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
        [SIGN_UP_REQUEST]: ({commit, dispatch}, signUpForm) => {
            return new Promise((resolve, reject) => {
                dispatch(POST_HTTP_REQUEST, {
                    server: SYRINX,
                    endpoint: SIGN_UP_ENDPOINT,
                    data: signUpForm,
                    ignoreReloadPage: true
                }).then(firstResponse => {
                    setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                    dispatch(GET_HTTP_REQUEST, {
                        server: SYRINX,
                        endpoint: GET_USER_DATA_ENDPOINT,
                        config: getConfiguration(firstResponse.data.accessToken),
                        ignoreReloadPage: true
                    }).then(secondResponse => {
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
        [REFRESH_REQUEST]: ({commit, dispatch}, redirectTo) => {
            return new Promise((resolve, reject) => {
                const {accessToken, refreshToken} = getTokens();
                if (accessToken && refreshToken) {
                    dispatch(POST_HTTP_REQUEST, {
                        server: SYRINX,
                        endpoint: REFRESH_ENDPOINT,
                        data: {accessToken, refreshToken},
                        ignoreReloadPage: true
                    }).then(firstResponse => {
                        setTokens(firstResponse.data.accessToken, firstResponse.data.refreshToken);
                        dispatch(GET_HTTP_REQUEST, {
                            server: SYRINX,
                            endpoint: GET_USER_DATA_ENDPOINT,
                            config: getConfiguration(firstResponse.data.accessToken),
                            ignoreReloadPage: true
                        }).then(secondResponse => {
                            commit(SIGN_IN_REQUEST, secondResponse.data);
                            resolve(redirectTo);
                        }).catch(error => {
                            removeTokens();
                            reject(error);
                        });
                    }).catch(error => {
                        if (error.response && error.response.status >= 400) {
                            removeTokens();
                            reject(error);
                        } else {
                            resolve("/500");
                        }
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
                    dispatch(POST_HTTP_REQUEST, {
                        server: SYRINX,
                        endpoint: LOGOUT_ENDPOINT,
                        config: getConfiguration(accessToken),
                        ignoreReloadPage: true
                    }).then(() => {
                        commit(LOGOUT_REQUEST);
                        removeTokens();
                        resolve();
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