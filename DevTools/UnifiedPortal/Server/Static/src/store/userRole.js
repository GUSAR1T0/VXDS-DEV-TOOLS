import { PREPARE_USER_ROLE_FORM, RESET_USER_ROLE_STORE_STATE, STORE_USER_ROLE_DATA_REQUEST } from "@/constants/actions";

export default {
    state: {
        userRole: {
            id: 0,
            name: "",
            permissions: []
        },
        userRoleForm: {}
    },
    getters: {
        getUserRole: state => {
            return state.userRole;
        },
        getUserRoleForm: state => {
            return state.userRoleForm;
        }
    },
    mutations: {
        [STORE_USER_ROLE_DATA_REQUEST]: (state, userRole) => {
            state.userRole.id = userRole ? userRole.id : 0;
            state.userRole.name = userRole ? userRole.name : "";
            state.userRole.permissions = userRole ? userRole.permissions : [];
        },
        [PREPARE_USER_ROLE_FORM]: state => {
            state.userRoleForm = JSON.parse(JSON.stringify({
                id: state.userRole.id,
                name: state.userRole.name,
                permissions: state.userRole.permissions
            }));
        },
        [RESET_USER_ROLE_STORE_STATE]: state => {
            state.userRole = {
                id: 0,
                name: "",
                permissions: []
            };
            state.userRoleForm = {};
        }
    }
};
