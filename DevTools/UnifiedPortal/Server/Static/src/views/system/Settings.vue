<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <div class="settings">
                <el-tabs :stretch="true">
                    <el-tab-pane>
                        <span slot="label"><fa icon="cogs"/> | General</span>
                        <h1>TBD...</h1>
                    </el-tab-pane>
                    <el-tab-pane>
                        <span slot="label"><fa icon="code-branch"/> | Code Services</span>
                        <CodeServices :code-services-settings="codeServicesSettings"/>
                    </el-tab-pane>
                </el-tabs>
            </div>
        </template>
    </LoadingContainer>
</template>

<script>
    import { mapGetters } from "vuex";
    import { LOAD_SETTINGS_REQUEST } from "@/constants/actions";
    import { renderErrorNotificationMessage } from "@/extensions/utils";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import CodeServices from "@/components/settings/CodeServices";

    export default {
        name: "Settings",
        components: {
            LoadingContainer,
            CodeServices
        },
        data() {
            return {
                loadingIsActive: true,
                codeServicesSettings: {
                    gitHubUser: undefined
                }
            };
        },
        computed: {
            ...mapGetters([
                "getCodeServicesSettings"
            ])
        },
        methods: {
            loadSettings() {
                this.loadingIsActive = true;

                this.$store.dispatch(LOAD_SETTINGS_REQUEST).then(() => {
                    this.loadingIsActive = false;
                    this.codeServicesSettings.gitHubUser = this.getCodeServicesSettings.gitHubUser;
                }).catch(error => {
                    this.loadingIsActive = false;

                    this.$notify.error({
                        title: "Failed to load settings",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        },
        mounted() {
            this.loadSettings();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadSettings();
            next();
        }
    };
</script>