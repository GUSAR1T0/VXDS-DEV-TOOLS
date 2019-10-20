import { ON_LOAD_LOOKUP_REQUEST } from "@/constants/actions";
import HttpClient from "@/extensions/httpClient";
import { LOCALHOST } from "@/constants/servers";
import { GET_LOOKUP_VALUES_ENDPOINT } from "@/constants/endpoints";

export default {
    state: {
        environmentVariables: {},
        lookupValues: {}
    },
    getters: {
        getEnvironmentVariables: state => {
            return state.environmentVariables;
        },
        getLookupValues: state => key => {
            return state.lookupValues[key];
        }
    },
    mutations: {
        [ON_LOAD_LOOKUP_REQUEST]: (state, allValues) => {
            state.environmentVariables = allValues.environmentVariables;
            state.lookupValues = allValues.lookupValues;
        }
    },
    actions: {
        [ON_LOAD_LOOKUP_REQUEST]: ({commit}) => {
            return new Promise((resolve, reject) => {
                HttpClient.init().get(LOCALHOST, GET_LOOKUP_VALUES_ENDPOINT)
                    .then(response => {
                        commit(ON_LOAD_LOOKUP_REQUEST, response.data);
                        resolve();
                    })
                    .catch(error => reject(error));
            });
        }
    }
};