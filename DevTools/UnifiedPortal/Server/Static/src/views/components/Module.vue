<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile :header="`${module.name} (${module.alias})`">
                <template slot="profile-buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            To Modules Page
                        </div>
                        <el-button type="info" plain circle @click="$router.push('/components/modules')"
                                   class="rounded-button">
                            <span><fa icon="cubes"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top" v-if="module.status === 18">
                        <div slot="content">
                            Run This Module
                        </div>
                        <el-button type="primary" plain circle v-if="hasPermissionToManageModules"
                                   class="rounded-button" @click="runModule">
                            <span><fa icon="forward"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top" v-if="module.status === 15">
                        <div slot="content">
                            Stop This Module
                        </div>
                        <el-button type="primary" plain circle v-if="hasPermissionToManageModules"
                                   class="rounded-button" @click="stopModule">
                            <span><fa icon="pause"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Upload Module Configuration
                        </div>
                        <el-button type="primary" plain circle v-if="hasPermissionToManageModules"
                                   class="rounded-button" @click="uploadDialog.visible = true">
                            <span><fa icon="upload"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top" 
                                v-if="module.status === 15 || module.status === 18"
                    >
                        <div slot="content">
                            Downgrade This Module
                        </div>
                        <el-button type="danger" plain circle v-if="hasPermissionToManageModules"
                                   class="rounded-button" @click="downgradeDialog.visible = true">
                            <span><fa icon="backspace"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top"
                                v-if="module.status === 15 || module.status === 18"
                    >
                        <div slot="content">
                            Uninstall This Module
                        </div>
                        <el-button type="danger" plain circle v-if="hasPermissionToManageModules"
                                   class="rounded-button" @click="uninstallDialog.visible = true">
                            <span><fa icon="trash-alt"/></span>
                        </el-button>
                    </el-tooltip>

                    <ModuleConfigurationUploadForm
                            :dialog-status="uploadDialog"
                            :success="successUpload"
                            :closed="loadModule"
                    />
                </template>
                <template slot="profile-content">
                    <ProfileBlock icon="cubes" header="Module Info">
                        <template slot="profile-block-content">
                            <DoubleRow>
                                <template slot="first">
                                    <Row name="Version" :value="module.version" half/>
                                </template>
                                <template slot="second">
                                    <Row name="Module Status" :value="getStatusName(module.status)" half/>
                                </template>
                            </DoubleRow>
                            <Row name="Responsible User">
                                <template slot="description">
                                    <UserAvatarAndFullNameWithLink
                                            :first-name="module.firstName"
                                            :last-name="module.lastName"
                                            :color="module.color"
                                            :user-id="module.userId"
                                    />
                                </template>
                            </Row>
                            <Row name="Host">
                                <template slot="description">
                                    <el-link :href="`/system/settings?tab=environment&hostId=${module.hostId}`"
                                             type="primary" :underline="false" v-if="hasPermissionToOpenSettingsPage">
                                        <HostFullNameSimplified
                                                :name="module.hostName"
                                                :domain="module.hostDomain"
                                                :operating-system="module.hostOperatingSystem"
                                        />
                                    </el-link>
                                    <div v-else>
                                        <HostFullNameSimplified
                                                :name="module.hostName"
                                                :domain="module.hostDomain"
                                                :operating-system="module.hostOperatingSystem"
                                        />
                                    </div>
                                </template>
                            </Row>
                        </template>
                    </ProfileBlock>
                    <ProfileBlock icon="cog" header="Configurations">
                        <template slot="profile-block-content">
                            <el-tabs :stretch="true" v-if="module.configurations" v-model="module.version">
                                <el-tab-pane v-for="item in module.configurations" :key="item.id"
                                             :label="`ver. ${item.version}`" :name="item.version">
                                    <Row name="Author" style="padding-top: 10px">
                                        <template slot="description">
                                            <el-link :href="`mailto:${item.email}`" type="primary" :underline="false">
                                                <strong style="font-size: 16px">
                                                    {{ item.author }} &lt;{{ item.email}}&gt;
                                                </strong>
                                            </el-link>
                                        </template>
                                    </Row>
                                    <Row name="File Description"
                                         :value="`${item.file.name}, ${getFileExtension(item.file.extension)} file`"
                                    />
                                    <Row name="File Upload Time" :value="item.file.time"/>
                                    <Row name="File Content">
                                        <template slot="description">
                                            <code class="code">{{ item.file.content }}</code>
                                        </template>
                                    </Row>
                                </el-tab-pane>
                            </el-tabs>
                        </template>
                    </ProfileBlock>
                </template>
            </Profile>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<style scoped>
    .code {
        font-family: monospace;
        white-space: pre-wrap;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { GET_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_MODULE_PROFILE_ENDPOINT, RUN_MODULE_ENDPOINT, STOP_MODULE_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import Row from "@/components/page/Row";
    import DoubleRow from "@/components/page/DoubleRow";
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";
    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";
    import HostFullNameSimplified from "@/components/hosts/HostFullNameSimplified";
    import ModuleConfigurationUploadForm from "@/components/modules/ModuleConfigurationUploadForm";

    export default {
        name: "Module",
        components: {
            LoadingContainer,
            Row,
            DoubleRow,
            Profile,
            ProfileBlock,
            UserAvatarAndFullNameWithLink,
            HostFullNameSimplified,
            ModuleConfigurationUploadForm
        },
        data() {
            return {
                loadingIsActive: true,
                moduleId: null,
                module: {},
                uploadDialog: {
                    visible: false
                },
                downgradeDialog: {
                    visible: false
                },
                uninstallDialog: {
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues",
                "hasPortalPermission"
            ]),
            hasPermissionToOpenSettingsPage() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_SETTINGS);
            },
            hasPermissionToManageModules() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_MODULES);
            }
        },
        methods: {
            loadModule() {
                this.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_MODULE_PROFILE_ENDPOINT, {
                        id: this.moduleId
                    }),
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.module = response.data;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load module profile",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            getStatusName(statusId) {
                let statuses = this.getLookupValues("moduleStatuses").filter(item => parseInt(item.value) === statusId);
                return statuses && statuses.length > 0 ? statuses[0].name : "—";
            },
            getFileExtension(extensionId) {
                let extensions = this.getLookupValues("fileExtensions").filter(item => parseInt(item.value) === extensionId);
                return extensions && extensions.length > 0 ? extensions[0].name : "—";
            },
            successUpload(id) {
                this.uploadDialog.visible = false;
                if (id !== this.moduleId) {
                    this.$router.push(`/components/module/${id}`);
                }
            },
            runModule() {
                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(RUN_MODULE_ENDPOINT, {
                        id: this.moduleId
                    }),
                    config: getConfiguration()
                }).then(() => {
                    this.loadingIsActive = false;
                    this.$notify.success({
                        title: "This module was updated",
                        message: "The status of module was triggered for running"
                    });
                    this.loadModule();
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to run module",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            stopModule() {
                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(STOP_MODULE_ENDPOINT, {
                        id: this.moduleId
                    }),
                    config: getConfiguration()
                }).then(() => {
                    this.loadingIsActive = false;
                    this.$notify.success({
                        title: "This module was updated",
                        message: "The status of module was triggered for stopping"
                    });
                    this.loadModule();
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to stop module",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        },
        mounted() {
            this.moduleId = parseInt(this.$route.params.id);
            this.loadModule();
        },
        beforeRouteUpdate(to, from, next) {
            this.moduleId = parseInt(to.params.id);
            this.loadModule();
            next();
        }
    };
</script>