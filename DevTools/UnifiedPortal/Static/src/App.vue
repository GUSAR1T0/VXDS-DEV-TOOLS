<template>
    <div id="app">
        <el-container class="app-container">
            <NavigationBar v-if="isAuthenticated"/>
            <el-container>
                <el-header class="app-header" height="auto">
                    <Header/>
                    <HeaderDivider :name="$route.meta.pageName"/>
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
    import HeaderDivider from "@/components/page/HeaderDivider.vue";
    import Footer from "@/components/page/Footer.vue";
    import { mapGetters } from "vuex";
    import { ON_LOAD_REQUEST } from "@/constants/actions";

    export default {
        components: {
            NavigationBar,
            Header,
            HeaderDivider,
            Footer
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

            this.$store.dispatch(ON_LOAD_REQUEST, window.location.pathname).then(redirectTo => {
                this.$router.push(redirectTo).catch(() => {});
                loading.close();
            }).catch(() => {
                this.$router.push("/auth").catch(() => {});
                loading.close();
            });
        }
    };
</script>
