import Vue from "vue";
import Vuex from "vuex";
import initialization from "@/store/initialization";
import httpClient from "@/store/httpClient";
import authentication from "@/store/authentication";
import lookup from "@/store/lookup";
import userProfile from "@/store/userProfile";
import project from "@/store/project";

Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        initialization,
        httpClient,
        authentication,
        lookup,
        userProfile,
        project
    }
});
