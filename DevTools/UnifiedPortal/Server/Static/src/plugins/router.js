import Vue from "vue";
import Router from "vue-router";
import Home from "@/views/Home.vue";

Vue.use(Router);

let sections = {
    HOME: "Home Page",
    AUTHORIZATION: "Authorization",
    ACCOUNT: "Account",
    COMPONENTS: "Components",
    PAGES: "Pages",
    SYSTEM: "System"
};

export default new Router({
    mode: "history",
    base: process.env.BASE_URL,
    routes: [
        {
            path: "/",
            name: "home",
            component: Home,
            meta: {
                sectionName: sections.HOME
            }
        },
        {
            path: "/auth",
            name: "authorization",
            component: () => import(/* webpackChunkName: "authorization" */ "../views/Authorization.vue"),
            meta: {
                sectionName: sections.AUTHORIZATION
            }
        },
        {
            path: "/user/:id?",
            name: "user",
            component: () => import(/* webpackChunkName: "user-profile" */ "../views/users/UserProfile.vue"),
            meta: {
                sectionName: sections.ACCOUNT
            }
        },
        {
            path: "/users",
            name: "users",
            component: () => import(/* webpackChunkName: "users" */ "../views/users/Users.vue"),
            meta: {
                sectionName: sections.ACCOUNT
            }
        },
        {
            path: "/users/roles",
            name: "roles",
            component: () => import(/* webpackChunkName: "user-roles" */ "../views/users/UserRoles.vue"),
            meta: {
                sectionName: sections.ACCOUNT
            }
        },
        {
            path: "/components/project/:id",
            name: "project",
            component: () => import(/* webpackChunkName: "project" */ "../views/components/Project.vue"),
            meta: {
                sectionName: sections.COMPONENTS
            }
        },
        {
            path: "/components/projects",
            name: "projects",
            component: () => import(/* webpackChunkName: "projects" */ "../views/components/Projects.vue"),
            meta: {
                sectionName: sections.COMPONENTS
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
                sectionName: sections.PAGES
            }
        },
        {
            path: "/system/settings",
            name: "settings",
            component: () => import(/* webpackChunkName: "settings" */ "../views/system/Settings.vue"),
            meta: {
                sectionName: sections.SYSTEM
            }
        },
        {
            path: "/system/operations",
            name: "operations",
            component: () => import(/* webpackChunkName: "operations" */ "../views/system/Operations.vue"),
            meta: {
                sectionName: sections.SYSTEM
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
