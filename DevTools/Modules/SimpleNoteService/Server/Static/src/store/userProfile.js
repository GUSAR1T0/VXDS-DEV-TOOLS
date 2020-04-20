import {
    GET_HTTP_REQUEST,
    RESET_USER_PROFILE_STORE_STATE,
    STORE_USER_PROFILE_DATA_REQUEST
} from "@/constants/actions";
import { UNIFIED_PORTAL } from "@/constants/servers";
import { getConfiguration } from "@/extensions/utils";

export default {
    state: {
        user: {
            id: null,
            email: "",
            firstName: "",
            lastName: "",
            color: "",
            location: "",
            bio: "",
            userRole: {
                id: 0,
                name: ""
            },
            isActivated: false
        }
    },
    getters: {
        getUserProfile: state => {
            return state.user;
        }
    },
    mutations: {
        [STORE_USER_PROFILE_DATA_REQUEST]: (state, data) => {
            state.user.id = data.id;
            state.user.email = data.email;
            state.user.firstName = data.firstName;
            state.user.lastName = data.lastName;
            state.user.color = data.color;
            state.user.location = data.location;
            state.user.bio = data.bio;
            state.user.userRole.id = data.userRole.id;
            state.user.userRole.name = data.userRole.name;
            state.user.isActivated = data.isActivated;
        },
        [RESET_USER_PROFILE_STORE_STATE]: state => {
            state.user.id = null;
            state.user.email = "";
            state.user.firstName = "";
            state.user.lastName = "";
            state.user.color = "";
            state.user.location = "";
            state.user.bio = "";
            state.user.userRole = {
                id: 0,
                name: ""
            };
            state.user.isActivated = false;
        }
    },
    actions: {
        [STORE_USER_PROFILE_DATA_REQUEST]: ({commit, dispatch}, query) => {
            return new Promise((resolve, reject) => {
                dispatch(GET_HTTP_REQUEST, {
                    server: UNIFIED_PORTAL,
                    endpoint: query,
                    config: getConfiguration()
                }).then(response => {
                    commit(STORE_USER_PROFILE_DATA_REQUEST, response.data);
                    resolve();
                }).catch(error => reject(error));
            });
        }
    }
};