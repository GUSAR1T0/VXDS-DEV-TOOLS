import axios from "axios";
import apis from "@/constants/apis";

function getConfig(accessToken) {
    return {
        headers: {
            "Authorization": "Bearer " + accessToken
        }
    };
}

function invoke(state, response, data) {
    state.isAuthenticated = true;
    state.firstName = response.data.firstName;
    state.lastName = response.data.lastName;
    if (data && data.complete) {
        data.complete(`${state.firstName} ${state.lastName}`);
    }
}

function revoke(state, data) {
    const reset = () => {
        localStorage.removeItem("access-token");
        localStorage.removeItem("refresh-token");
        state.isAuthenticated = false;
        state.firstName = "";
        state.lastName = "";
        if (data && data.complete) {
            data.complete();
        }
    };

    const accessToken = localStorage.getItem("access-token");
    axios.post(apis.Logout, getConfig(accessToken))
        .then(reset)
        .catch(reset);
}

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
        login(state, data) {
            if (data && data.accessToken && data.refreshToken) {
                localStorage.setItem("access-token", data.accessToken);
                localStorage.setItem("refresh-token", data.refreshToken);
            } else {
                data = {};
                data.accessToken = localStorage.getItem("access-token");
            }

            if (data.accessToken) {
                let client = axios.create();
                client.interceptors.response.use(response => response, error => {
                    const accessToken = localStorage.getItem("access-token");
                    const refreshToken = localStorage.getItem("refresh-token");
                    if (error.response.status === 401 && accessToken && refreshToken) {
                        client.post(apis.Refresh, {
                            accessToken: accessToken,
                            refreshToken: refreshToken
                        }).then(response => {
                            localStorage.setItem("access-token", response.data.accessToken);
                            localStorage.setItem("refresh-token", response.data.refreshToken);

                            client.get(apis.GetUserData, getConfig(response.data.accessToken))
                                .then(response => invoke(state, response, data))
                                .catch(() => revoke(state, data));
                        }).catch(() => revoke(state, data));
                    }
                    return Promise.reject(error);
                });

                client.get(apis.GetUserData, getConfig(data.accessToken))
                    .then(response => invoke(state, response, data))
                    .catch(() => revoke(state, data));
            }
        },
        logout: (state, data) => revoke(state, data)
    }
};