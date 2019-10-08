<template>
    <div class="nav">
        <el-menu class="el-nav-menu-vertical" :collapse="isCollapse" :router="true" :default-active="$route.path">
            <CollapsibleMenuItem class="el-nav-menu-vertical-header" menuName="Home" index="/">
                <template slot="icon">
                    <img alt="VXDESIGN.STORE: DEVELOPMENT TOOLS logo" src="@/assets/logo.png" width="40px"
                         height="40px" class="el-nav-menu-logo">
                </template>
            </CollapsibleMenuItem>
            <!-- B: Pages -->
            <el-submenu class="el-nav-menu-vertical-pages" index="Pages">
                <template slot="title">
                    <fa class="fa-submenu" icon="align-justify"/>
                    <span v-if="!isCollapse" slot="title" class="el-nav-menu-vertical-title">Pages</span>
                </template>
                <el-menu-item-group v-if="isCollapse">
                    <span slot="title" class="el-nav-menu-vertical-group-title">Pages</span>
                    <PagesSubMenu/>
                </el-menu-item-group>
                <template v-else>
                    <PagesSubMenu/>
                </template>
            </el-submenu>
            <!-- E: Pages -->
            <!-- B: User -->
            <el-submenu class="el-nav-menu-vertical-user" index="User">
                <template slot="title">
                    <fa v-if="isAuthenticated" class="fa-submenu" icon="user-circle"/>
                    <fa v-else class="fa-submenu" icon="question-circle"/>
                    <span v-if="!isCollapse" slot="title" class="el-nav-menu-vertical-title">User</span>
                </template>
                <el-menu-item-group v-if="isCollapse">
                    <span slot="title" class="el-nav-menu-vertical-group-title">User</span>
                    <UserSubMenu/>
                </el-menu-item-group>
                <template v-else>
                    <UserSubMenu/>
                </template>
            </el-submenu>
            <!-- E: User -->
            <!-- B: More -->
            <el-submenu class="el-nav-menu-vertical-footer" index="More">
                <template slot="title">
                    <fa class="fa-submenu" icon="ellipsis-h"/>
                    <span v-if="!isCollapse" slot="title" class="el-nav-menu-vertical-title">More</span>
                </template>
                <el-menu-item-group v-if="isCollapse">
                    <span slot="title" class="el-nav-menu-vertical-group-title">More</span>
                    <MoreSubMenu/>
                </el-menu-item-group>
                <template v-else>
                    <MoreSubMenu/>
                </template>
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

<style scoped src="@/styles/navigation-bar.css">
</style>

<style scoped src="@/styles/submenu.css">
    .el-nav-menu-vertical-group-title {
        text-transform: uppercase;
        font-weight: bold;
    }
</style>

<script>
    import { mapGetters } from "vuex";

    import CollapsibleMenuItem from "@/components/navigation-bar/submenu/CollapsibleMenuItem.vue";
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
            PagesSubMenu,
            UserSubMenu,
            MoreSubMenu
        }
    };
</script>