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
        <div v-else>
            <el-menu-item index="/auth">
                <fa class="fa-submenu-item" icon="sign-in-alt"/>
                <span slot="title" class="el-nav-menu-vertical-item">Authorization</span>
            </el-menu-item>
        </div>
    </div>
</template>

<style scoped src="@/styles/submenu.css">
</style>

<script>
    import { mapGetters, mapMutations } from "vuex";

    export default {
        name: "UserSubMenu",
        computed: {
            ...mapGetters([
                "isAuthenticated",
                "getFullName"
            ]),
            ...mapMutations([
                "logout"
            ])
        },
        methods: {
            logoutAction() {
                this.$store.commit("logout", {
                    complete: () => {
                        this.$router.push("/");
                        this.$notify.info({
                            title: "Info",
                            message: "You are logged out"
                        });
                    }
                });
            }
        }
    };
</script>