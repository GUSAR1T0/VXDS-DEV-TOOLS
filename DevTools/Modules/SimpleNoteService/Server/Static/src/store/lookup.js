import {
    GET_HTTP_REQUEST,
    ON_LOAD_LOOKUP_REQUEST,
    SET_PERMISSION_GROUP_ID,
    STORE_ENVIRONMENT_VARIABLES_FOR_HTTP_CLIENT
} from "@/constants/actions";
import { LOCALHOST } from "@/constants/servers";
import { GET_LOOKUP_VALUES_ENDPOINT } from "@/constants/endpoints";

export default {
    state: {
        lookupValues: {}
    },
    getters: {
        getLookupValues: state => key => {
            return state.lookupValues[key];
        }
    },
    mutations: {
        [ON_LOAD_LOOKUP_REQUEST]: (state, lookupValues) => {
            state.lookupValues = lookupValues;
        }
    },
    actions: {
        [ON_LOAD_LOOKUP_REQUEST]: ({commit, dispatch}) => {
            return new Promise((resolve, reject) => {
                dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_LOOKUP_VALUES_ENDPOINT
                }).then(response => {
                    commit(ON_LOAD_LOOKUP_REQUEST, response.data.lookupValues);
                    commit(SET_PERMISSION_GROUP_ID, response.data.lookupValues.permissionGroupId);
                    commit(STORE_ENVIRONMENT_VARIABLES_FOR_HTTP_CLIENT, response.data.environmentVariables);
                    resolve();
                }).catch(error => reject(error));
            });
        }
    }
};