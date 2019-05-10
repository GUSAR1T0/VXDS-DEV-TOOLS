import Vue from "vue";
import App from "./App.vue";
import router from "./plugins/router";
import store from "./plugins/store";
import "./plugins/element";
import "./plugins/fontawesome";

Vue.config.productionTip = false;

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount("#app");
