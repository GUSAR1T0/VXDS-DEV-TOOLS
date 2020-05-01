<template>
    <Profile header="Settings" style="padding: 20px">
        <template slot="profile-content">
            <el-tabs :stretch="true" v-model="active" style="margin-top: -20px">
                <el-tab-pane name="environment">
                    <span slot="label"><fa icon="network-wired"/> | Environment</span>
                    <Environment/>
                </el-tab-pane>
                <el-tab-pane name="code-services">
                    <span slot="label"><fa icon="code-branch"/> | Code Services</span>
                    <CodeServices/>
                </el-tab-pane>
            </el-tabs>
        </template>
    </Profile>
</template>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";

    import Profile from "@/components/page/Profile";
    import CodeServices from "@/components/settings/CodeServices";
    import Environment from "@/components/settings/Environment";

    export default {
        name: "Settings",
        components: {
            Profile,
            CodeServices,
            Environment
        },
        data() {
            return {
                active: "environment"
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
            ]),
            hasPermissionToManageSettings() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_SETTINGS);
            }
        },
        mounted() {
            if (!this.hasPermissionToManageSettings) {
                this.$router.push(`/403?from=${this.$route.path}`);
            }

            if (this.$route.query.tab) {
                this.active = this.$route.query.tab;
            }
        },
        beforeRouteUpdate(to, from, next) {
            if (!this.hasPermissionToManageSettings) {
                next(`/403?from=${to.path}`);
                return;
            }

            if (to.query.tab) {
                this.active = to.query.tab;
            }

            next();
        }
    };
</script>