<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile :header="`${getProject.name} (${getProject.alias})`">
                <template slot="profile-buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            To Project On Unified Portal Page
                        </div>
                        <el-button type="info" plain circle class="rounded-button"
                                   @click="$router.push(`/unifiedPortal?host=${getUnifiedPortalHost}&link=components/project/${projectId}`)">
                            <span><fa icon="code"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
                <template slot="profile-content">
                    <ProfileBlock icon="code" header="Project Info">
                        <template slot="profile-block-content">
                            <Row v-if="getProject.description" name="Description" :value="getProject.description"/>
                            <Row name="Project Status" :value="getProject.isActive ? 'Active' : 'Inactive'"/>
                            <Row name="GitHub Owner" v-if="gitHubTokenSetup && getProject.gitHubRepoId > 0">
                                <template slot="description">
                                    <div style="display: flex; align-items: center">
                                        <el-link type="primary" :href="getProject.gitHubRepository.owner.profileUrl"
                                                 target="_blank" :underline="false" class="github-owner-link">
                                            <div class="user-avatar-and-name">
                                                <img :src="getProject.gitHubRepository.owner.avatarUrl" alt="avatar"
                                                     class="avatar"/>
                                                <div style="margin-left: 10px">
                                                    <strong style="font-size: 16px">
                                                        {{ getProject.gitHubRepository.owner.login }}
                                                    </strong>
                                                </div>
                                            </div>
                                        </el-link>
                                    </div>
                                </template>
                            </Row>
                            <Row name="GitHub Repository" v-if="gitHubTokenSetup && getProject.gitHubRepoId > 0">
                                <template slot="description">
                                    <div style="display: flex; align-items: center">
                                        <div>
                                            <el-link :href="getProject.gitHubRepository.repositoryUrl"
                                                     target="_blank"
                                                     type="primary" :underline="false">
                                                <strong style="font-size: 16px">
                                                    {{ getProject.gitHubRepository.fullName }}
                                                </strong>
                                            </el-link>
                                        </div>
                                        <div style="margin-left: 5px; margin-bottom: 15px">
                                            <span :class="getPrivateStatusIconColor()" style="font-size: 8px">
                                                <fa :icon="getPrivateStatusIcon()"/>
                                            </span>
                                        </div>
                                    </div>
                                </template>
                            </Row>
                            <Row name="License" v-if="getProject.gitHubRepository.license"
                                 :value="getProject.gitHubRepository.license"/>
                        </template>
                    </ProfileBlock>
                </template>
            </Profile>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<style scoped src="@/styles/user.css">
</style>

<style scoped>
    .avatar {
        width: 32px;
        height: 32px;
        border-radius: 5px;
    }

    .is-private {
        color: #F56C6C;
    }

    .is-public {
        color: #0C7C59;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { GET_PROJECT_PROFILE_ENDPOINT } from "@/constants/endpoints";
    import { GET_HTTP_REQUEST, RESET_PROJECT_STORE_STATE, STORE_PROJECT_DATA_REQUEST, } from "@/constants/actions";
    import { UNIFIED_PORTAL } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import Row from "@/components/page/Row";
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";

    export default {
        name: "Project",
        components: {
            LoadingContainer,
            Row,
            Profile,
            ProfileBlock
        },
        data() {
            return {
                loadingIsActive: true,
                projectId: null,
                gitHubTokenSetup: null,
                dialogProjectFormStatus: {
                    visible: false
                },
                dialogProjectDeleteStatus: {
                    visible: false
                },
                languages: []
            };
        },
        computed: {
            ...mapGetters([
                "getProject",
                "getUnifiedPortalHost"
            ])
        },
        methods: {
            loadProject() {
                this.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: UNIFIED_PORTAL,
                    endpoint: format(GET_PROJECT_PROFILE_ENDPOINT, {
                        id: this.projectId
                    }),
                    config: getConfiguration()
                }).then(response => {
                    this.languages = [];
                    if (response.data.gitHubRepository && response.data.gitHubRepository.languages) {
                        for (let key in response.data.gitHubRepository.languages) {
                            this.languages.push({
                                name: key,
                                percent: response.data.gitHubRepository.languages[key]
                            });
                        }
                    }

                    this.loadingIsActive = false;
                    this.$store.commit(STORE_PROJECT_DATA_REQUEST, response.data);
                    this.gitHubTokenSetup = response.data.gitHubTokenSetup;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load project profile",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            getPrivateStatusIconColor() {
                return this.getProject.gitHubRepository.private ? "is-private" : "is-public";
            },
            getPrivateStatusIcon() {
                return this.getProject.gitHubRepository.private ? "lock" : "lock-open";
            }
        },
        mounted() {
            this.projectId = this.$route.params.id;
            this.loadProject();
        },
        beforeRouteUpdate(to, from, next) {
            this.projectId = to.params.id;
            this.loadProject();
            next();
        },
        beforeRouteLeave(to, from, next) {
            if (to.path !== "/unifiedPortal") {
                this.$store.commit(RESET_PROJECT_STORE_STATE);
            }
            next();
        }
    };
</script>