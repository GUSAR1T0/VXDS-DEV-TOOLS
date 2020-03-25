import Vue from "vue";
import Vuex from "vuex";
import initialization from "@/store/initialization";
import httpClient from "@/store/httpClient";
import authentication from "@/store/authentication";
import userProfile from "@/store/userProfile";
import userRole from "@/store/userRole";
import lookup from "@/store/lookup";
import settings from "@/store/settings";
import project from "@/store/project";
import incident from "@/store/incident";

Vue.use(Vuex);

let modules = {initialization, httpClient, authentication, userProfile, userRole, lookup, settings, project, incident};
export default new Vuex.Store({
    modules
});
