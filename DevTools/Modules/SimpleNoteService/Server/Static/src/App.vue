<template>
    <div id="app">
        <el-container class="app-container" v-if="!loadingIsActive">
            <NavigationBar v-if="isAuthenticated && hasPermissionToModule"/>
            <el-container>
                <el-header class="app-header" height="auto">
                    <Header/>
                    <HorizontalDivider :name="$route.meta.sectionName"/>
                </el-header>
                <el-main class="app-main">
                    <router-view v-if="isReadyToLoadContent && (!isAuthenticated || hasPermissionToModule)"/>
                    <div v-else-if="isAuthenticated && !hasPermissionToModule">
                        <ErrorPage
                                :error-number="403"
                                problem-title="We couldn't give you access to the portal"
                                description="If you have a problem, please, inform your administrator about that."
                        >
                            <template slot="button">
                                <el-button type="danger" @click="logoutDialogStatus.visible = true">
                                    <span><fa class="fa-submenu-item" icon="sign-out-alt"/><strong> | Sign Out</strong></span>
                                </el-button>
                            </template>
                        </ErrorPage>
                        <ConfirmationDialog :dialog-status="logoutDialogStatus"
                                            confirmation-text="Are you sure that you want to sign out?"
                                            :cancel-click-action="() => logoutDialogStatus.visible = false"
                                            :submit-click-action="logoutAction"/>
                    </div>
                </el-main>
                <el-footer class="app-footer" height="auto">
                    <Footer/>
                </el-footer>
            </el-container>
        </el-container>
    </div>
</template>

<style src="@/styles/app.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import {
        LOGOUT_REQUEST,
        ON_LOAD_LOOKUP_REQUEST,
        REFRESH_REQUEST,
        RESET_PATH_FOR_REDIRECTION
    } from "@/constants/actions";
    import { PORTAL_PERMISSION } from "@/constants/permissions";

    import NavigationBar from "@/components/navigation-bar/NavigationBar.vue";
    import Header from "@/components/page/Header.vue";
    import HorizontalDivider from "@/components/page/HorizontalDivider.vue";
    import Footer from "@/components/page/Footer.vue";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog.vue";
    import ErrorPage from "@/components/errors/ErrorPage";

    export default {
        components: {
            NavigationBar,
            Header,
            HorizontalDivider,
            Footer,
            ConfirmationDialog,
            ErrorPage
        },
        data() {
            return {
                loadingIsActive: true,
                logoutDialogStatus: {
                    visible: false
                },
                isReadyToLoadContent: true
            };
        },
        computed: {
            ...mapGetters([
                "isAuthenticated",
                "getFullName",
                "hasPortalPermission"
            ]),
            hasPermissionToModule() {
                return this.hasPortalPermission(PORTAL_PERMISSION.ACCESS_TO_MODULE);
            }
        },
        methods: {
            logoutAction(button) {
                button.loading = true;
                this.isReadyToLoadContent = false;
                this.$store.dispatch(LOGOUT_REQUEST).then(() => {
                    button.loading = false;
                    this.logoutDialogStatus.visible = false;
                    this.$router.push("/auth").then(() => this.isReadyToLoadContent = true).catch(() => {
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
            const loading = this.$loading({
                lock: true,
                text: "Loading",
                spinner: "el-icon-loading",
                background: "rgba(255, 255, 255, 1)",
                customClass: "main-loading-spinner-custom"
            });

            let completeLoading = () => {
                loading.close();
                this.loadingIsActive = false;
            };

            this.$store.dispatch(ON_LOAD_LOOKUP_REQUEST).then(() => {
                this.$store.dispatch(REFRESH_REQUEST, this.$store.getters.getPathForRedirection).then(redirectTo => {
                    if (!this.isAuthenticated && !(redirectTo === "/auth" || redirectTo === "/500")) {
                        redirectTo = "/auth";
                    } else if (this.isAuthenticated && redirectTo === "/auth") {
                        redirectTo = "/";
                    }

                    this.$router.push(redirectTo).then(() => completeLoading()).catch(() => completeLoading());
                }).catch(() => {
                    this.$router.push("/auth").then(() => completeLoading()).catch(() => completeLoading());
                });
                this.$store.dispatch(RESET_PATH_FOR_REDIRECTION);
            }).catch(() => {
                completeLoading();
                this.$router.push("/500");
            });
        }
    };
</script>
