import {
    PREPARE_ACCOUNT_SPECIFIC_INFO_UPDATE_FORM,
    PREPARE_USER_GENERAL_INFO_UPDATE_FORM, RESET_USER_PROFILE_STORE_STATE,
    STORE_USER_PROFILE_DATA_REQUEST,
    STORE_USER_PROFILE_ID_REQUEST
} from "@/constants/actions";
import HttpClient from "@/extensions/httpClient";
import { LOCALHOST } from "@/constants/servers";
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
                id: null
            }
        },
        userGeneralInfoUpdateForm: {},
        accountSpecificInfoUpdateForm: {
            userRole: {
                id: null
            }
        }
    },
    getters: {
        getUserProfileId: state => {
            return state.user.id;
        },
        getUserProfile: state => {
            return state.user;
        },
        getUserGeneralInfoUpdateForm: state => {
            return state.userGeneralInfoUpdateForm;
        },
        getAccountSpecificInfoUpdateForm: state => {
            return state.accountSpecificInfoUpdateForm;
        },
        isAboutMe: state => currentUserId => {
            let userProfileId = state.user.id;
            if (userProfileId) {
                return currentUserId === userProfileId;
            }
            return true;
        }
    },
    mutations: {
        [STORE_USER_PROFILE_ID_REQUEST]: (state, id) => {
            state.user.id = id;
        },
        [STORE_USER_PROFILE_DATA_REQUEST]: (state, data) => {
            state.user.id = data.id;
            state.user.email = data.email;
            state.user.firstName = data.firstName;
            state.user.lastName = data.lastName;
            state.user.color = data.color;
            state.user.location = data.location;
            state.user.bio = data.bio;
            state.user.userRole = data.userRole ? data.userRole : {
                id: null
            };
        },
        [PREPARE_USER_GENERAL_INFO_UPDATE_FORM]: state => {
            state.userGeneralInfoUpdateForm = JSON.parse(JSON.stringify({
                firstName: state.user.firstName,
                lastName: state.user.lastName,
                email: state.user.email,
                color: state.user.color,
                location: state.user.location,
                bio: state.user.bio
            }));
        },
        [PREPARE_ACCOUNT_SPECIFIC_INFO_UPDATE_FORM]: state => {
            state.accountSpecificInfoUpdateForm = JSON.parse(JSON.stringify({
                userRole: {
                    id: state.user.userRole.id
                }
            }));
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
                id: null
            };
            state.userGeneralInfoUpdateForm = {};
            state.accountSpecificInfoUpdateForm = {
                userRole: {
                    id: null
                }
            };
        }
    },
    actions: {
        [STORE_USER_PROFILE_DATA_REQUEST]: ({commit}, query) => {
            return new Promise((resolve, reject) => {
                HttpClient.init().get(LOCALHOST, query, getConfiguration()).then(response => {
                    commit(STORE_USER_PROFILE_DATA_REQUEST, response.data);
                    resolve(response);
                }).catch(error => reject(error));
            });
        }
    }
}