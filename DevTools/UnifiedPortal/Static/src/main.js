import Vue from "vue";
import App from "./App.vue";
import router from "./plugins/router";
import store from "./plugins/store";
import "./plugins/element/element";
import "./plugins/fontawesome";
import { SET_PATH_FOR_REDIRECTION } from "@/constants/actions";

Vue.config.productionTip = false;

new Vue({
    router,
    store,
    render: h => h(App),
    beforeCreate() {
        this.$store.dispatch(SET_PATH_FOR_REDIRECTION, window.location.pathname);
    }
}).$mount("#app");
