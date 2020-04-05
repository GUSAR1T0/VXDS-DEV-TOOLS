import Vue from "vue";
import Router from "vue-router";
import Home from "@/views/Home.vue";

Vue.use(Router);

let sections = {
    HOME: "Home Page",
    AUTHORIZATION: "Authorization"
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
            path: "/user",
            beforeEnter(to, from, next) {
                window.open(`${to.query.unifiedPortalHost}/user`, "_blank");
                next(from.fullPath);
            }
        },
        {
            path: "/unifiedPortal",
            beforeEnter(to, from, next) {
                window.open(to.query.unifiedPortalHost, "_blank");
                next(from.fullPath);
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
