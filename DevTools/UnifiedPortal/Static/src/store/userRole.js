import { PREPARE_USER_ROLE_FORM, RESET_USER_ROLE_STORE_STATE, STORE_USER_ROLE_DATA_REQUEST } from "@/constants/actions";

let definePermissions = (sourcePortalPermissions, newPortalPermissions) => {
    sourcePortalPermissions.splice(0, sourcePortalPermissions.length);
    if (newPortalPermissions) {
        let bits = newPortalPermissions.toString(2).split("").reverse();
        for (let i = 0; i < bits.length; i++) {
            if (bits[i] === "1") {
                sourcePortalPermissions.push(Math.pow(2, i).toString());
            }
        }
    }
};

export default {
    state: {
        userRole: {
            id: 0,
            name: "",
            portalPermissions: []
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
            if (userRole) {
                definePermissions(state.userRole.portalPermissions, userRole.portalPermissions);
            }
        },
        [PREPARE_USER_ROLE_FORM]: state => {
            state.userRoleForm = JSON.parse(JSON.stringify({
                id: state.userRole.id,
                name: state.userRole.name,
                portalPermissions: state.userRole.portalPermissions
            }));
        },
        [RESET_USER_ROLE_STORE_STATE]: state => {
            state.userRole = {
                id: 0,
                name: "",
                portalPermissions: []
            };
            state.userRoleForm = {};
        }
    }
};
