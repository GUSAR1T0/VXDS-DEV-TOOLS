<template>
    <div class="settings-tab code-services">
        <div class="github">
            <h2 class="block-header"><span><fa :icon="['fab', 'github']"/> GitHub</span></h2>
            <el-divider class="block-divider"/>
            <div>
                <strong class="field-header">Personal Access Token</strong>
                <div class="field-input">
                    <el-input clearable show-password v-model="token"/>
                    <el-button type="primary" :disabled="!token" class="inline-field-element"
                               @click="setGitHubToken" ref="setupGitHubTokenButton">
                        <strong>Save</strong>
                    </el-button>
                    <el-button :disabled="!codeServicesSettings.gitHubUser" class="inline-field-element"
                               @click="resetGitHubToken" ref="resetGitHubTokenButton">
                        <strong>Reset</strong>
                    </el-button>
                </div>
                <div v-if="codeServicesSettings.gitHubUser">
                    <div v-if="codeServicesSettings.gitHubUser.isValid" class="field-input validation">
                        <div>The stored token is valid for</div>
                        <el-link type="success" :href="codeServicesSettings.gitHubUser.profileUrl"
                                 target="_blank" class="inline-field-element" :underline="false">
                            <div class="user-avatar-and-name">
                                <img :src="codeServicesSettings.gitHubUser.avatarUrl" alt="avatar"
                                     class="avatar"/>
                                <div class="inline-field-element">
                                    <strong>
                                        {{ codeServicesSettings.gitHubUser.name }}
                                        ({{ codeServicesSettings.gitHubUser.login }})
                                    </strong>
                                </div>
                            </div>
                        </el-link>
                    </div>
                    <div v-else class="field-input validation">
                        <div>The stored token is not valid</div>
                    </div>
                </div>
                <div v-else>
                    <div class="field-input validation">
                        <div>No saved token is in the system</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped src="@/styles/settings.css">
</style>

<style scoped src="@/styles/user.css">
</style>

<style scoped>
    .avatar {
        width: 32px;
        height: 32px;
        border-radius: 5px;
    }

    .validation {
        align-items: center;
        margin-left: 10px;
    }
</style>

<script>
    import { SETUP_GITHUB_TOKEN_REQUEST } from "@/constants/actions";
    import { renderErrorNotificationMessage } from "@/extensions/utils";
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";

    export default {
        name: "CodeServices",
        props: {
            codeServicesSettings: Object
        },
        data() {
            return {
                token: ""
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission"
            ]),
            hasPermissionToManageSettings() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_SETTINGS);
            }
        },
        methods: {
            setGitHubToken() {
                this.$refs.setupGitHubTokenButton.loading = true;
                this.$store.dispatch(SETUP_GITHUB_TOKEN_REQUEST, this.token).then(gitHubUser => {
                    this.$refs.setupGitHubTokenButton.loading = false;
                    this.token = "";
                    this.codeServicesSettings.gitHubUser = gitHubUser;

                    if (gitHubUser) {
                        if (gitHubUser.isValid) {
                            this.$notify.success({
                                title: "Setup is performed",
                                message: "New GitHub Personal Access Token is valid"
                            });
                        } else {
                            this.$notify.warning({
                                title: "Setup is performed",
                                message: "New GitHub Personal Access Token is invalid"
                            });
                        }
                    }
                }).catch(error => {
                    this.$refs.setupGitHubTokenButton.loading = false;

                    this.$notify.error({
                        title: "Failed to setup GitHub token",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            resetGitHubToken() {
                this.$refs.resetGitHubTokenButton.loading = true;
                this.$store.dispatch(SETUP_GITHUB_TOKEN_REQUEST, "").then(gitHubUser => {
                    this.$refs.resetGitHubTokenButton.loading = false;
                    this.token = "";
                    this.codeServicesSettings.gitHubUser = gitHubUser;

                    if (!gitHubUser) {
                        this.$notify.success({
                            title: "Reset is performed",
                            message: "The previous GitHub Personal Access Token was removed"
                        });
                    }
                }).catch(error => {
                    this.$refs.resetGitHubTokenButton.loading = false;

                    this.$notify.error({
                        title: "Failed to reset GitHub token",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
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