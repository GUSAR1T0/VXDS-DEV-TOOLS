import Vue from "vue";
import Router from "vue-router";
import Home from "@/views/Home.vue";

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
            }
        },
        {
            path: "/auth",
            name: "authorization",
            component: () => import(/* webpackChunkName: "authorization" */ "../views/Authorization.vue"),
            meta: {
                pageName: "Authorization"
            }
        },
        {
            path: "/user/:id?",
            name: "user",
            component: () => import(/* webpackChunkName: "user-profile" */ "../views/users/UserProfile.vue"),
            meta: {
                pageName: "User Profile"
            }
        },
        {
            path: "/users",
            name: "users",
            component: () => import(/* webpackChunkName: "users" */ "../views/users/Users.vue"),
            meta: {
                pageName: "Users"
            }
        },
        {
            path: "/users/roles",
            name: "roles",
            component: () => import(/* webpackChunkName: "user-roles" */ "../views/users/UserRoles.vue"),
            meta: {
                pageName: "User Roles"
            }
        },
        {
            path: "/settings",
            name: "settings",
            component: () => import(/* webpackChunkName: "settings" */ "../views/settings/Settings.vue"),
            meta: {
                pageName: "Settings"
            }
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
            }
        }
    ]
});
