import Vue from "vue";
import Vuex from "vuex";
import authentication from "@/store/authentication";
import navigationBar from "@/store/navigationBar";

Vue.use(Vuex);

let modules = {authentication, navigationBar};
export default new Vuex.Store({
    modules
});
