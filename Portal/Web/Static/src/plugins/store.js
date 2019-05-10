import Vue from "vue";
import Vuex from "vuex";
import navigationBar from "@/store/navigationBar";

Vue.use(Vuex);

let modules = {navigationBar};
export default new Vuex.Store({
    modules
});
