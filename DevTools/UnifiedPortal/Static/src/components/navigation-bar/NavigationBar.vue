<template>
    <div class="nav">
        <el-menu class="el-nav-menu-vertical" :collapse-transition="false" :collapse="true" :router="true"
                 :default-active="$route.path">
            <el-menu-item class="el-nav-menu-vertical-header" index="/">
                <img alt="VXDESIGN.STORE: DEVELOPMENT TOOLS logo" src="@/assets/logo.png" width="40px"
                     height="40px" class="el-nav-menu-logo">
            </el-menu-item>
            <!-- B: Pages -->
            <el-submenu class="el-nav-menu-vertical-pages" index="Pages">
                <template slot="title">
                    <fa class="fa-submenu" icon="align-justify"/>
                </template>
                <el-menu-item-group>
                    <span slot="title" class="el-nav-menu-vertical-group-title">Pages</span>
                    <PagesSubMenu/>
                </el-menu-item-group>
            </el-submenu>
            <!-- E: Pages -->
            <!-- B: User -->
            <el-submenu class="el-nav-menu-vertical-user" index="User">
                <template slot="title">
                    <fa class="fa-submenu" icon="user-circle"/>
                </template>
                <el-menu-item-group>
                    <span slot="title" class="el-nav-menu-vertical-group-title">User</span>
                    <UserSubMenu/>
                </el-menu-item-group>
            </el-submenu>
            <!-- E: User -->
            <!-- B: More -->
            <el-submenu class="el-nav-menu-vertical-footer" index="More">
                <template slot="title">
                    <fa class="fa-submenu" icon="ellipsis-h"/>
                </template>
                <el-menu-item-group>
                    <span slot="title" class="el-nav-menu-vertical-group-title">More</span>
                    <MoreSubMenu/>
                </el-menu-item-group>
            </el-submenu>
            <!-- E: More -->
        </el-menu>
    </div>
</template>

<style>
    div.nav > ul > li.el-nav-menu-vertical-pages.el-submenu > div.el-submenu__title {
        padding: 0 29px !important;
    }

    div.nav > ul > li.el-nav-menu-vertical-footer.el-submenu > div.el-submenu__title,
    div.nav > ul > li.el-nav-menu-vertical-user.el-submenu > div.el-submenu__title {
        padding: 0 28px !important;
    }

    div.nav > ul > li.el-nav-menu-vertical-header.el-menu-item {
        padding: 0 20px !important;
    }

    div.nav > ul > li.el-nav-menu-vertical-pages.el-submenu > div.el-submenu__title,
    div.nav > ul > li.el-nav-menu-vertical-footer.el-submenu > div.el-submenu__title,
    div.nav > ul > li.el-nav-menu-vertical-user.el-submenu > div.el-submenu__title,
    div.nav > ul > li.el-nav-menu-vertical-header.el-menu-item {
        height: 64px !important;
        line-height: 64px !important;
    }

    .el-menu-item.is-active {
        color: #F56C6C !important;
    }
</style>

<style scoped src="@/styles/navigation-bar.css" id="navmenu">
</style>

<style scoped src="@/styles/submenu.css">
    .el-nav-menu-vertical-group-title {
        text-transform: uppercase;
        font-weight: bold;
    }
</style>

<script>
    import PagesSubMenu from "@/components/navigation-bar/submenu/PagesSubMenu.vue";
    import UserSubMenu from "@/components/navigation-bar/submenu/UserSubMenu.vue";
    import MoreSubMenu from "@/components/navigation-bar/submenu/MoreSubMenu.vue";

    export default {
        name: "NavigationBar",
        components: {
            PagesSubMenu,
            UserSubMenu,
            MoreSubMenu
        },
        methods: {
            resetPagePosition() {
                window.scrollTo({ top: 0, behavior: "smooth" });
            }
        },
        mounted() {
            let navmenu = document.getElementsByClassName("el-nav-menu-vertical");
            if (navmenu) {
                window.onscroll = function () {
                    navmenu[0].style.top = (window.pageYOffset || document.documentElement.scrollTop) + "px";
                };
            }
        },
        updated() {
            this.resetPagePosition();
        },
        beforeRouteUpdate(to, from, next) {
            this.resetPagePosition();
            next();
        }
    };
</script>