export default {
    state: {
        isNavigationBarCollapse: true
    },
    getters: {
        isNavigationBarCollapse: state => {
            return state.isNavigationBarCollapse;
        }
    },
    mutations: {
        collapseOrExpandNavigationBar: state => {
            state.isNavigationBarCollapse = !state.isNavigationBarCollapse;
        }
    }
};