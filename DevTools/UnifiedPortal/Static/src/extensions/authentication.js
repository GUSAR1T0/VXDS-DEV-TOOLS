import store from "@/plugins/store";

export default {
    redirectIfAuthenticationIsNotRequired: (to, from, next) => {
        if (!store.getters.isAuthenticated) {
            next();
            return;
        }
        next("/");
    },
    redirectIfAuthenticationIsRequired: (to, from, next) => {
        if (store.getters.isAuthenticated) {
            next();
            return;
        }
        next("/auth");
    }
};
