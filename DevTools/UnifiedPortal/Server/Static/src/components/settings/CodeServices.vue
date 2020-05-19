<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <div class="code-services">
                <ProfileBlock :icon="['fab', 'github']" header="GitHub">
                    <template slot="profile-block-content">
                        <h3 class="field-header">Personal Access Token</h3>
                        <div class="field-content">
                            <el-input clearable show-password v-model="token"/>
                            <el-button type="primary" :disabled="!token" class="inline-field-element"
                                       @click="setGitHubToken" ref="setupGitHubTokenButton">
                                <strong>Save</strong>
                            </el-button>
                            <el-button :disabled="!gitHubUser" class="inline-field-element"
                                       @click="resetGitHubToken" ref="resetGitHubTokenButton">
                                <strong>Reset</strong>
                            </el-button>
                        </div>
                        <div v-if="gitHubUser">
                            <div v-if="gitHubUser.isValid" class="field-content validation">
                                <div>The stored token is valid for</div>
                                <el-link type="success" :href="gitHubUser.profileUrl"
                                         target="_blank" class="inline-field-element" :underline="false">
                                    <div class="user-avatar-and-name">
                                        <img :src="gitHubUser.avatarUrl" alt="avatar"
                                             class="avatar"/>
                                        <div class="inline-field-element">
                                            <strong>
                                                {{ gitHubUser.name }}
                                                ({{ gitHubUser.login }})
                                            </strong>
                                        </div>
                                    </div>
                                </el-link>
                            </div>
                            <div v-else class="field-input validation no-user">
                                <div>The stored token is not valid</div>
                            </div>
                        </div>
                        <div v-else>
                            <div class="field-input validation no-user">
                                <div>No saved token is in the system</div>
                            </div>
                        </div>
                    </template>
                </ProfileBlock>
            </div>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/profile.css">
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
    
    .no-user {
        line-height: 52px;
    }
</style>

<script>
    import { GET_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { LOAD_CODE_SERVICES_SETTINGS_ENDPOINT, SETUP_GITHUB_TOKEN_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import ProfileBlock from "@/components/page/ProfileBlock";

    export default {
        name: "CodeServices",
        components: {
            LoadingContainer,
            ProfileBlock
        },
        data() {
            return {
                loadingIsActive: true,
                gitHubUser: {},
                token: ""
            };
        },
        methods: {
            loadSettings() {
                this.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: LOAD_CODE_SERVICES_SETTINGS_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.gitHubUser = response.data.gitHubUser;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load code services settings",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            setGitHubToken() {
                this.$refs.setupGitHubTokenButton.loading = true;
                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(SETUP_GITHUB_TOKEN_ENDPOINT, {
                        token: this.token
                    }),
                    config: getConfiguration()
                }).then(response => {
                    this.$refs.setupGitHubTokenButton.loading = false;
                    this.token = "";
                    this.gitHubUser = response.data;

                    if (response.data) {
                        if (response.data.isValid) {
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
                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(SETUP_GITHUB_TOKEN_ENDPOINT, {
                        token: ""
                    }),
                    config: getConfiguration()
                }).then(response => {
                    this.$refs.resetGitHubTokenButton.loading = false;
                    this.token = "";
                    this.gitHubUser = response.data;

                    if (!response.data) {
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
            this.loadSettings();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadSettings();
            next();
        }
    };
</script>