import Vue from "vue";
import Vuex from "vuex";
import initialization from "@/store/initialization";
import authentication from "@/store/authentication";
import userProfile from "@/store/userProfile";
import lookup from "@/store/lookup";

Vue.use(Vuex);

let modules = {initialization, authentication, userProfile, lookup};
export default new Vuex.Store({
    modules
});
