<template>
    <Profile header="Settings" style="padding: 20px">
        <template slot="profile-content">
            <el-tabs :stretch="true" style="margin-top: -20px">
                <el-tab-pane>
                    <span slot="label"><fa icon="network-wired"/> | Environment</span>
                    <Environment/>
                </el-tab-pane>
                <el-tab-pane>
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
        },
        beforeRouteUpdate(to, from, next) {
            if (!this.hasPermissionToManageSettings) {
                next(`/403?from=${to.path}`);
                return;
            }
            next();
        }
    };
</script>