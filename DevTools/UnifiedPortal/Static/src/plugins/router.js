import Vue from "vue";
import Router from "vue-router";
import Home from "@/views/Home.vue";
import authenticationBasedRouting from "@/extensions/authentication";

Vue.use(Router);

export default new Router({
    mode: "history",
    base: process.env.BASE_URL,
    routes: [
        {
            path: "/",
            name: "home",
            component: Home,
            meta: {
                pageName: "Home Page"
            },
            beforeEnter: authenticationBasedRouting.redirectIfAuthenticationIsRequired
        },
        {
            path: "/auth",
            name: "authorization",
            component: () => import(/* webpackChunkName: "authorization" */ "../views/Authorization.vue"),
            meta: {
                pageName: "Authorization"
            },
            beforeEnter: authenticationBasedRouting.redirectIfAuthenticationIsNotRequired
        },
        {
            path: "/user/:id?",
            name: "user",
            component: () => import(/* webpackChunkName: "user-profile" */ "../views/UserProfile.vue"),
            meta: {
                    pageName: "User Profile"
            },
            beforeEnter: authenticationBasedRouting.redirectIfAuthenticationIsRequired
        },
        {
            path: "/about",
            name: "about",
            // route level code-splitting
            // this generates a separate chunk (about.[hash].js) for this route
            // which is lazy-loaded when the route is visited.
            component: () => import(/* webpackChunkName: "about" */ "../views/About.vue"),
            meta: {
                pageName: "About"
            },
            beforeEnter: authenticationBasedRouting.redirectIfAuthenticationIsRequired
        }
    ]
});
