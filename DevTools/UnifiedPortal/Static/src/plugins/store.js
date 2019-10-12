import Vue from "vue";
import Vuex from "vuex";
import authentication from "@/store/authentication";

Vue.use(Vuex);

let modules = {authentication};
export default new Vuex.Store({
    modules
});
