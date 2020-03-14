<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile :header="`${getProject.name} (${getProject.alias})`">
                <template slot="profile-buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            To Projects Page
                        </div>
                        <el-button type="info" plain circle @click="$router.push('/components/projects')"
                                   class="rounded-button">
                            <span><fa icon="code"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Edit This Project
                        </div>
                        <el-button type="primary" plain circle v-if="hasPermissionToManageProjects"
                                   @click="openDialogToUpdate" class="rounded-button">
                            <span><fa icon="edit"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Delete This Project
                        </div>
                        <el-button type="danger" plain circle v-if="hasPermissionToManageProjects"
                                   @click="dialogProjectDeleteStatus.visible = true" class="rounded-button">
                            <span><fa icon="trash-alt"/></span>
                        </el-button>
                    </el-tooltip>

                    <ProjectEditForm v-if="hasPermissionToManageProjects"
                                     :dialog-status="dialogProjectFormStatus"
                                     :project="getProject"
                                     :project-form="getProjectForm"
                                     :closed="submitProjectAction"
                                     :git-hub-repo-id-options="getOptions"
                                     :git-hub-token-setup="gitHubTokenSetup"/>

                    <ConfirmationDialog v-if="hasPermissionToManageProjects"
                                        :dialog-status="dialogProjectDeleteStatus"
                                        :confirmation-text="confirmationTextOnDelete"
                                        :cancel-click-action="() => dialogProjectDeleteStatus.visible = false"
                                        :submit-click-action="deleteProject"
                                        :closed="submitProjectAction"/>
                </template>
                <template slot="profile-content">
                    <ProfileBlock icon="code" header="Project Info">
                        <template slot="profile-block-content">
                            <Row v-if="getProject.description" name="Description" :value="getProject.description"/>
                            <Row name="Project Status" :value="getProject.isActive ? 'Active' : 'Inactive'"/>
                        </template>
                    </ProfileBlock>
                    <ProfileBlock :icon="['fab', 'github']" header="GitHub Repository"
                                  v-if="gitHubTokenSetup && getProject.gitHubRepoId > 0">
                        <template slot="profile-block-content">
                            <Blocks>
                                <template slot="first">
                                    <Row name="Owner" half>
                                        <template slot="description">
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
                                        </template>
                                    </Row>
                                    <Row name="Repository Name" half>
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
                                    <Row name="Repository Description" :value="getProject.gitHubRepository.description"
                                         half/>
                                    <Row name="Counts" half>
                                        <template slot="description">
                                            <el-table :data="[getProject.gitHubRepository]" style="width: 100%" border>
                                                <el-table-column align="center">
                                                    <template slot="header">
                                                        <fa icon="star"/>
                                                    </template>
                                                    <template slot-scope="scope">
                                                        <strong>{{ scope.row.stargazersCount }}</strong>
                                                    </template>
                                                </el-table-column>
                                                <el-table-column align="center">
                                                    <template slot="header">
                                                        <fa icon="code-branch"/>
                                                    </template>
                                                    <template slot-scope="scope">
                                                        <strong>{{ scope.row.forksCount }}</strong>
                                                    </template>
                                                </el-table-column>
                                                <el-table-column align="center">
                                                    <template slot="header">
                                                        <fa icon="tasks"/>
                                                    </template>
                                                    <template slot-scope="scope">
                                                        <strong>{{ scope.row.openIssuesCount }}</strong>
                                                    </template>
                                                </el-table-column>
                                            </el-table>
                                        </template>
                                    </Row>
                                    <Row name="License" v-if="getProject.gitHubRepository.license"
                                         :value="getProject.gitHubRepository.license" half/>
                                </template>
                                <template slot="second" v-if="languages.length > 0">
                                    <div class="languages">
                                        <strong>Repository Languages</strong>
                                        <!--                                        <ProjectChart :chart="chart" :options="options" v-if="!loadingIsActive"/>-->
                                        <!--                                        <div v-else/>-->
                                    </div>
                                    <div style="text-align: center">
                                        <div v-for="language in languages" :key="language.name" class="language">
                                            <el-badge :value="`${language.percent}%`" type="info">
                                                <el-tag type="primary" effect="plain" hit>
                                                    <strong style="font-size: 16px">{{ language.name }}</strong>
                                                </el-tag>
                                            </el-badge>
                                        </div>
                                    </div>
                                </template>
                            </Blocks>
                        </template>
                    </ProfileBlock>
                </template>
            </Profile>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/profile.css">
</style>

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

    .languages {
        text-align: center;
        font-size: 18px;
        margin-bottom: 30px;
    }

    .language {
        display: inline-block;
        padding: 15px 5px 15px 5px;
        margin-right: 40px;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { DELETE_PROJECT_PROFILE_ENDPOINT, GET_PROJECT_PROFILE_ENDPOINT } from "@/constants/endpoints";
    import {
        DELETE_HTTP_REQUEST,
        GET_HTTP_REQUEST,
        RESET_PROJECT_STORE_STATE,
        STORE_PROJECT_DATA_REQUEST,
        PREPARE_PROJECT_FORM
    } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";
    // import randomColor from "randomcolor";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import Row from "@/components/page/Row";
    import Blocks from "@/components/page/Blocks";
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";
    import ProjectEditForm from "@/components/projects/ProjectEditForm";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";
    // import ProjectChart from "@/extensions/projectChart";

    export default {
        name: "Project",
        components: {
            LoadingContainer,
            Row,
            Blocks,
            Profile,
            ProfileBlock,
            ProjectEditForm,
            ConfirmationDialog,
            // ProjectChart
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
                // chart: {
                //     labels: null,
                //     datasets: [ {
                //         borderWidth: 1,
                //         borderColor: "#fff",
                //         hoverBorderColor: "#fff",
                //         hoverBorderWidth: 5,
                //         weight: 5,
                //         data: null
                //     } ]
                // },
                // options: {
                //     responsive: true,
                //     maintainAspectRatio: false,
                //     rotation: Math.PI,
                //     circumference: Math.PI,
                //     legend: {
                //         labels: {
                //             fontFamily: "'Didact Gothic', 'Avenir', Helvetica, Arial, sans-serif",
                //             fontSize: 16
                //         }
                //     }
                // },
                languages: []
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getProject",
                "getProjectForm"
            ]),
            hasPermissionToManageProjects() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_PROJECTS);
            },
            confirmationTextOnDelete() {
                return `Are you sure that you want to delete project "${this.getProject.name}"?`;
            },
            getOptions() {
                return this.getProject.gitHubRepoId ? [ {
                    id: this.getProject.gitHubRepoId,
                    fullName: this.getProject.gitHubRepository.fullName
                } ] : [];
            }
        },
        methods: {
            loadProject() {
                this.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_PROJECT_PROFILE_ENDPOINT, {
                        id: this.projectId
                    }),
                    config: getConfiguration()
                }).then(response => {
                    // this.chart.labels = [];
                    // this.chart.datasets[0].backgroundColor = [];
                    // this.chart.datasets[0].data = [];
                    this.languages = [];
                    if (response.data.gitHubRepository && response.data.gitHubRepository.languages) {
                        for (let key in response.data.gitHubRepository.languages) {
                            // this.chart.labels.push(key);
                            // this.chart.datasets[0].backgroundColor.push(randomColor({
                            //     luminosity: "dark",
                            //     hue: "random",
                            //     format: "rgba",
                            //     alpha: 0.75
                            // }));
                            // this.chart.datasets[0].data.push(response.data.gitHubRepository.languages[key]);
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
            openDialogToUpdate() {
                this.$store.commit(PREPARE_PROJECT_FORM);
                this.dialogProjectFormStatus.visible = true;
            },
            submitProjectAction() {
                this.loadProject();
                this.$store.commit(RESET_PROJECT_STORE_STATE);
            },
            deleteProject(button) {
                button.loading = true;
                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_PROJECT_PROFILE_ENDPOINT, {
                        id: this.projectId
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.dialogProjectDeleteStatus.visible = false;

                    this.$notify.success({
                        title: "Project was deleted",
                        message: `Project "${this.getProject.name}" was removed`
                    });

                    this.$router.push("/components/projects");
                }).catch(error => {
                    button.loading = false;
                    this.dialogProjectDeleteStatus.visible = false;

                    this.$notify.error({
                        title: "Failed to delete project",
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
            this.loadProject(this.$route.params.id);
        },
        beforeRouteUpdate(to, from, next) {
            this.projectId = to.params.id;
            this.loadProject(to.params.id);
            next();
        }
    };
</script>