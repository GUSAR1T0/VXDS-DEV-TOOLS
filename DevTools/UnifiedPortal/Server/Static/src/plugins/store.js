import Vue from "vue";
import Vuex from "vuex";
import initialization from "@/store/initialization";
import httpClient from "@/store/httpClient";
import authentication from "@/store/authentication";
import userProfile from "@/store/userProfile";
import userRole from "@/store/userRole";
import lookup from "@/store/lookup";
import project from "@/store/project";
import incident from "@/store/incident";
import notification from "@/store/notification";

Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        initialization,
        httpClient,
        authentication,
        userProfile,
        userRole,
        lookup,
        project,
        incident,
        notification
    }
});
