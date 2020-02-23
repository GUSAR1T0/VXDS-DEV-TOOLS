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
            path: "/pages/about",
            name: "about",
            // route level code-splitting
            // this generates a separate chunk (about.[hash].js) for this route
            // which is lazy-loaded when the route is visited.
            component: () => import(/* webpackChunkName: "about" */ "../views/pages/About.vue"),
            meta: {
                pageName: "About"
            }
        },
        {
            path: "/system/settings",
            name: "settings",
            component: () => import(/* webpackChunkName: "settings" */ "../views/system/Settings.vue"),
            meta: {
                pageName: "Settings"
            }
        },
        {
            path: "/system/operations",
            name: "operations",
            component: () => import(/* webpackChunkName: "operations" */ "../views/system/Operations.vue"),
            meta: {
                pageName: "Operations"
            }
        },
        {
            path: "/403",
            name: "forbidden",
            component: () => import(/* webpackChunkName: "forbidden" */ "../views/errors/ForbiddenErrorPage.vue")
        },
        {
            path: "/404",
            name: "not-found",
            component: () => import(/* webpackChunkName: "not-found" */ "../views/errors/NotFoundErrorPage.vue")
        },
        {
            path: "/500",
            name: "internal-error",
            component: () => import(/* webpackChunkName: "internal-error" */ "../views/errors/InternalErrorPage.vue")
        },
        {
            path: "*",
            beforeEnter: (to, from, next) => {
                next(`/404?from=${to.path}`);
            }
        }
    ]
});
