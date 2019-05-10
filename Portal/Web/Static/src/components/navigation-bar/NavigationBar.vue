<template>
    <div class="nav">
        <el-menu class="el-nav-menu-vertical" :collapse="isCollapse" :router="true" :default-active="$route.path">
            <CollapsibleSubMenu class="el-nav-menu-vertical-header" name="portal">
                <template slot="icon">
                    <img alt="VXDESIGN.STORE: DEVELOPMENT TOOLS logo" src="@/assets/logo.png" width="40px"
                         height="40px" class="el-nav-menu-logo">
                </template>
                <template slot="menu">
                    <PortalSubMenu/>
                </template>
            </CollapsibleSubMenu>
            <CollapsibleSubMenu class="el-nav-menu-vertical-user" name="user">
                <template slot="icon">
                    <fa class="fa-submenu" icon="user-circle"/>
                </template>
                <template slot="menu">
                    <UserSubMenu/>
                </template>
            </CollapsibleSubMenu>
            <CollapsibleSubMenu class="el-nav-menu-vertical-footer" name="more">
                <template slot="icon">
                    <fa class="fa-submenu" icon="ellipsis-h"/>
                </template>
                <template slot="menu">
                    <MoreSubMenu/>
                </template>
            </CollapsibleSubMenu>
        </el-menu>
    </div>
</template>

<style>
    div.nav > ul > li.el-nav-menu-vertical-footer.el-submenu > div.el-submenu__title,
    div.nav > ul > li.el-nav-menu-vertical-user.el-submenu > div.el-submenu__title {
        padding: 0 28px !important;
    }

    div.nav > ul > li.el-nav-menu-vertical-header.el-submenu > div.el-submenu__title {
        padding: 0 20px !important;
    }

    .el-submenu__title {
        height: 64px !important;
        line-height: 64px !important;
    }

    .el-menu-item {
        height: 56px !important;
        line-height: 56px !important;
    }
</style>

<style scoped>
    .el-nav-menu-vertical {
        position: relative;
        min-height: 100%;
        width: 80px;
    }

    .el-nav-menu-vertical:not(.el-menu--collapse) {
        width: 250px;
    }

    .el-nav-menu-vertical-footer {
        position: absolute !important;
        width: 100%;
        bottom: 0;
    }

    .fa-submenu {
        font-size: 24px;
    }

    .el-nav-menu-logo {
        margin-right: -8px;
    }
</style>

<script>
    import { mapState } from "vuex";

    import CollapsibleSubMenu from "@/components/navigation-bar/submenu/CollapsibleSubMenu.vue";
    import PortalSubMenu from "@/components/navigation-bar/submenu/PortalSubMenu.vue";
    import UserSubMenu from "@/components/navigation-bar/submenu/UserSubMenu.vue";
    import MoreSubMenu from "@/components/navigation-bar/submenu/MoreSubMenu.vue";

    export default {
        name: "NavigationBar",
        computed: {
            ...mapState({
                isCollapse: state => state.navigationBar.isNavigationBarCollapse
            })
        },
        components: {
            CollapsibleSubMenu,
            PortalSubMenu,
            UserSubMenu,
            MoreSubMenu
        }
    };
</script>