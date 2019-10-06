<template>
    <div class="user">
        <div v-if="isAuthenticated">
            <el-menu-item>
                <fa class="fa-submenu-item" icon="user-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">{{ getFullName }}</span>
            </el-menu-item>
            <el-menu-item @click="logoutAction">
                <fa class="fa-submenu-item" icon="sign-out-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">Sign Out</span>
            </el-menu-item>
        </div>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { LOGOUT_REQUEST } from "@/constants/actions";

    export default {
        name: "UserSubMenu",
        computed: {
            ...mapGetters([
                "isAuthenticated",
                "getFullName"
            ])
        },
        methods: {
            logoutAction() {
                this.$store.dispatch(LOGOUT_REQUEST).then(() => {
                    this.$router.push("/auth").catch(() => {});
                    this.$notify.info({
                        title: "Info",
                        message: "You are logged out"
                    });
                }).catch(() => {
                    this.$router.push("/auth").catch(() => {});
                });
            }
        }
    };
</script>