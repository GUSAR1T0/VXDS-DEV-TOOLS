export default {
    state: {
        isNavigationBarCollapse: true,
        user: {
            firstName: "",
            lastName: "",
            isAuthorized: false
        }
    },
    mutations: {
        collapseOrExpandNavigationBar: state => {
            state.isNavigationBarCollapse = !state.isNavigationBarCollapse;
        }
    },
    getters: {
        isAuthorized: state => {
            return state.user.isAuthorized;
        },
        fullname: state => {
            return `${state.user.firstName} ${state.user.lastName}`;
        }
    }
};