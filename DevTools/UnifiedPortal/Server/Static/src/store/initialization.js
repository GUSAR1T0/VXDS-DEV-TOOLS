import { RESET_PATH_FOR_REDIRECTION, SET_PATH_FOR_REDIRECTION } from "@/constants/actions";

export default {
    state: {
        pathForRedirection: ""
    },
    getters: {
        getPathForRedirection: state => {
            return state.pathForRedirection;
        }
    },
    mutations: {
        [SET_PATH_FOR_REDIRECTION]: (state, pathForRedirection) => {
            state.pathForRedirection = pathForRedirection;
        }
    },
    actions: {
        [SET_PATH_FOR_REDIRECTION]: ({commit}, pathForRedirection) => {
            commit(SET_PATH_FOR_REDIRECTION, pathForRedirection);
        },
        [RESET_PATH_FOR_REDIRECTION]: ({commit}) => {
            commit(SET_PATH_FOR_REDIRECTION, "");
        }
    }
};