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
            this.$store.dispatch(ON_LOAD_REQUEST).then(() => {
                this.$router.push("/");
            }).catch(() => {
                this.$router.push("/auth");
            });
        }
    };
</script>
