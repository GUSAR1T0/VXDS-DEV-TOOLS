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
                    <UserSubMenu :page-status="pageStatus"/>
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
        <el-dialog :visible.sync="pageStatus.logoutDialogVisible" width="50%" center>
            <span slot="title" class="modal-title">Confirmation</span>
            <h1 class="logout-dialog-header">Are you sure that you want to sign out?</h1>
            <el-row type="flex" justify="center" align="middle" :gutter="20">
                <el-col :span="12">
                    <el-button type="danger" @click="pageStatus.logoutDialogVisible = false" plain style="width: 100%">
                        Cancel
                    </el-button>
                </el-col>
                <el-col :span="12">
                    <el-button type="danger" @click="logoutAction" style="width: 100%">
                        Submit
                    </el-button>
                </el-col>
            </el-row>
        </el-dialog>
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
</style>

<style scoped src="@/styles/modal.css">
</style>

<style scoped>
    .logout-dialog-header {
        text-align: center;
        margin-bottom: 55px;
    }

    .el-nav-menu-vertical-group-title {
        text-transform: uppercase;
        font-weight: bold;
    }
</style>

<script>
    import { LOGOUT_REQUEST } from "@/constants/actions";

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
        data() {
            return {
                pageStatus: {
                    logoutDialogVisible: false
                }
            };
        },
        methods: {
            resetPagePosition() {
                window.scrollTo({top: 0, behavior: "smooth"});
            },
            logoutAction() {
                this.pageStatus.logoutDialogVisible = false;
                this.$store.dispatch(LOGOUT_REQUEST).then(() => {
                    this.$router.push("/auth").catch(() => {
                    });
                    this.$notify.info({
                        title: "You are logged out",
                        message: "Waiting for you again"
                    });
                }).catch(() => {
                    this.$router.push("/auth").catch(() => {
                    });
                });
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