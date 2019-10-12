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
    import NavigationBar from "@/components/navigation-bar/NavigationBar.vue";
    import Header from "@/components/page/Header.vue";
    import HorizontalDivider from "@/components/page/HorizontalDivider.vue";
    import Footer from "@/components/page/Footer.vue";
    import { mapGetters } from "vuex";
    import { ON_LOAD_REQUEST } from "@/constants/actions";

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
                "getFullName"
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

            this.$store.dispatch(ON_LOAD_REQUEST, window.location.pathname).then(redirectTo => {
                this.$router.push(redirectTo).then(() => completeLoading()).catch(() => completeLoading());
            }).catch(() => {
                this.$router.push("/auth").then(() => completeLoading()).catch(() => completeLoading());
            });
        }
    };
</script>
