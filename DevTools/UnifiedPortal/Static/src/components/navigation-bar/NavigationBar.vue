<template>
    <div class="nav">
        <el-menu class="el-nav-menu-vertical" :collapse="isCollapse" :router="true" :default-active="$route.path">
            <CollapsibleMenuItem class="el-nav-menu-vertical-header" menuName="Home" index="/">
                <template slot="icon">
                    <img alt="VXDESIGN.STORE: DEVELOPMENT TOOLS logo" src="@/assets/logo.png" width="40px"
                         height="40px" class="el-nav-menu-logo">
                </template>
            </CollapsibleMenuItem>
            <CollapsibleSubMenu class="el-nav-menu-vertical-pages" menuName="Pages">
                <template slot="icon">
                    <fa class="fa-submenu" icon="align-justify"/>
                </template>
                <template slot="submenu">
                    <PagesSubMenu/>
                </template>
            </CollapsibleSubMenu>
            <CollapsibleSubMenu class="el-nav-menu-vertical-user" menuName="User">
                <template slot="icon">
                    <fa v-if="isAuthenticated" class="fa-submenu" icon="user-circle"/>
                    <fa v-else class="fa-submenu" icon="question-circle"/>
                </template>
                <template slot="submenu">
                    <UserSubMenu/>
                </template>
            </CollapsibleSubMenu>
            <CollapsibleSubMenu class="el-nav-menu-vertical-footer" menuName="More">
                <template slot="icon">
                    <fa class="fa-submenu" icon="ellipsis-h"/>
                </template>
                <template slot="submenu">
                    <MoreSubMenu/>
                </template>
            </CollapsibleSubMenu>
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

<style scoped src="@/styles/navigation-bar.css">
</style>

<script>
    import { mapGetters } from "vuex";

    import CollapsibleMenuItem from "@/components/navigation-bar/submenu/CollapsibleMenuItem.vue";
    import CollapsibleSubMenu from "@/components/navigation-bar/submenu/CollapsibleSubMenu.vue";
    import PagesSubMenu from "@/components/navigation-bar/submenu/PagesSubMenu.vue";
    import UserSubMenu from "@/components/navigation-bar/submenu/UserSubMenu.vue";
    import MoreSubMenu from "@/components/navigation-bar/submenu/MoreSubMenu.vue";

    export default {
        name: "NavigationBar",
        computed: {
            ...mapGetters({
                isAuthenticated: "isAuthenticated",
                isCollapse: "isNavigationBarCollapse"
            })
        },
        components: {
            CollapsibleMenuItem,
            CollapsibleSubMenu,
            PagesSubMenu,
            UserSubMenu,
            MoreSubMenu
        }
    };
</script>