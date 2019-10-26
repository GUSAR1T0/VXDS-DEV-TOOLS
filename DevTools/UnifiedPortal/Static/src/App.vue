<template>
    <div id="app">
        <el-container class="app-container" v-if="!loadingIsActive">
            <NavigationBar v-if="isAuthenticated"/>
            <el-container>
                <el-header class="app-header" height="auto">
                    <Header/>
                    <HorizontalDivider :name="$route.meta.pageName"/>
                </el-header>
                <el-main class="app-main">
                    <router-view/>
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
    import { ON_LOAD_LOOKUP_REQUEST, REFRESH_REQUEST, RESET_PATH_FOR_REDIRECTION } from "@/constants/actions";

    import NavigationBar from "@/components/navigation-bar/NavigationBar.vue";
    import Header from "@/components/page/Header.vue";
    import HorizontalDivider from "@/components/page/HorizontalDivider.vue";
    import Footer from "@/components/page/Footer.vue";

    export default {
        components: {
            NavigationBar,
            Header,
            HorizontalDivider,
            Footer
        },
        data() {
            return {
                loadingIsActive: true
            };
        },
        computed: {
            ...mapGetters([
                "isAuthenticated",
                "getFullName",
                "getReauthenticationTime"
            ])
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
                    if (!this.isAuthenticated && redirectTo !== "/auth") {
                        redirectTo = "/auth";
                    }
                    else if (this.isAuthenticated && redirectTo === "/auth") {
                        redirectTo = "/";
                    }

                    this.$router.push(redirectTo).then(() => completeLoading()).catch(() => {
                        // TODO: Error case handling -> bug #39
                        completeLoading();
                    });
                }).catch(() => {
                    this.$router.push("/auth").then(() => completeLoading()).catch(() => {
                        // TODO: Error case handling -> bug #39
                        completeLoading();
                    });
                });
                this.$store.dispatch(RESET_PATH_FOR_REDIRECTION);
            }).catch(() => {
                // TODO: Error case handling -> bug #39
            });
        }
    };
</script>
