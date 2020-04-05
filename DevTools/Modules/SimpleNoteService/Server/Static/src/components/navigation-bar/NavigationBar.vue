<template>
    <div class="nav">
        <el-menu class="el-nav-menu-vertical" :collapse-transition="false" :collapse="true" :router="true"
                 :default-active="$route.path">
            <el-menu-item class="el-nav-menu-vertical-header" index="/">
                <img alt="VXDESIGN.STORE: DEVELOPMENT TOOLS logo" src="@/assets/logo.png" width="40px"
                     height="40px" class="el-nav-menu-logo">
            </el-menu-item>
            <!-- B: Account -->
            <el-submenu class="el-nav-menu-vertical-user" index="Account">
                <template slot="title">
                    <fa class="fa-submenu" icon="user-circle"/>
                </template>
                <el-menu-item-group>
                    <span slot="title" class="el-nav-menu-vertical-group-title">Account</span>
                    <AccountSubMenu :logout-dialog-status="logoutDialogStatus"/>
                </el-menu-item-group>
            </el-submenu>
            <!-- E: Account -->
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
        <ConfirmationDialog :dialog-status="logoutDialogStatus"
                            confirmation-text="Are you sure that you want to sign out?"
                            :cancel-click-action="() => logoutDialogStatus.visible = false"
                            :submit-click-action="logoutAction"/>
    </div>
</template>

<style>
    div.nav > ul > li.el-nav-menu-vertical-footer.el-submenu > div.el-submenu__title,
    div.nav > ul > li.el-nav-menu-vertical-user.el-submenu > div.el-submenu__title {
        padding: 0 28px !important;
        text-align: center;
    }

    div.nav > ul > li.el-nav-menu-vertical-header.el-menu-item {
        padding: 0 20px !important;
    }

    div.nav > ul > li.el-nav-menu-vertical-footer.el-submenu > div.el-submenu__title,
    div.nav > ul > li.el-nav-menu-vertical-user.el-submenu > div.el-submenu__title,
    div.nav > ul > li.el-nav-menu-vertical-header.el-menu-item {
        height: 64px !important;
        line-height: 64px !important;
    }
</style>

<style scoped src="@/styles/navigation-bar.css">
</style>

<style scoped src="@/styles/submenu.css">
</style>

<style scoped src="@/styles/modal.css">
</style>

<style scoped>
    .el-nav-menu-vertical-group-title {
        text-transform: uppercase;
        font-weight: bold;
    }
</style>

<script>
    import { LOGOUT_REQUEST } from "@/constants/actions";

    import AccountSubMenu from "@/components/navigation-bar/submenu/AccountSubMenu.vue";
    import MoreSubMenu from "@/components/navigation-bar/submenu/MoreSubMenu.vue";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";

    export default {
        name: "NavigationBar",
        components: {
            AccountSubMenu,
            MoreSubMenu,
            ConfirmationDialog
        },
        data() {
            return {
                logoutDialogStatus: {
                    visible: false
                }
            };
        },
        methods: {
            resetPagePosition() {
                window.scrollTo({top: 0, behavior: "smooth"});
            },
            logoutAction(button) {
                button.loading = true;
                this.$store.dispatch(LOGOUT_REQUEST).then(() => {
                    button.loading = false;
                    this.logoutDialogStatus.visible = false;
                    this.$router.push("/auth").catch(() => {
                        this.$router.push("/500");
                    });
                    this.$notify.success({
                        title: "You are logged out",
                        message: "Waiting for you again"
                    });
                }).catch(() => {
                    button.loading = false;
                    this.logoutDialogStatus.visible = false;
                    this.$router.push("/auth").catch(() => {
                        this.$router.push("/500");
                    });
                });
            }
        },
        mounted() {
            let navmenu = document.getElementsByClassName("el-nav-menu-vertical");
            if (navmenu) {
                window.onscroll = function () {
                    if (navmenu[0]) {
                        navmenu[0].style.top = (window.pageYOffset || document.documentElement.scrollTop) + "px";
                    }
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