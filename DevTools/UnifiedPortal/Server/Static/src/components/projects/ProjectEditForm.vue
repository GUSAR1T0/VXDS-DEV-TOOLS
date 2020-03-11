<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">{{ project.id ? "Update" : "Create" }} Project</span>
        <el-form :model="projectForm" :rules="projectRules" ref="projectForm"
                 label-width="200px" @submit.native.prevent="submitForm('projectForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="name" label="Project Name">
                        <el-input v-model="projectForm.name" clearable/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="alias" label="Project Alias">
                        <el-input v-model="projectForm.alias" clearable
                                  onkeyup="this.value = this.value.toUpperCase();"
                        />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="description" label="Project Description">
                        <el-input v-model="projectForm.description" clearable type="textarea"/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center" v-if="gitHubTokenSetup">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="gitHubRepo" label="GitHub Repository">
                        <el-select v-model="projectForm.gitHubRepoId" filterable clearable remote reserve-keyword
                                   :remote-method="filterByGitHubRepoIds" style="width: 100%">
                            <el-option v-for="item in gitHubRepoIdOptions" :key="item.id" :label="item.fullName"
                                       :value="item.id"/>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="projectIsActive" label="Project Status">
                        <el-select v-model="projectForm.isActive" :value="projectForm.isActive"
                                   placeholder="Select" style="width: 100%">
                            <el-option label="Active" :value="true"/>
                            <el-option label="Inactive" :value="false"/>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item>
                        <el-row type="flex" justify="center" align="middle" :gutter="20">
                            <el-col :span="12">
                                <el-button @click="cancel" style="width: 100%">
                                    <strong>Cancel</strong>
                                </el-button>
                            </el-col>
                            <el-col :span="12">
                                <el-button type="primary" ref="projectFormButton" native-type="submit"
                                           style="width: 100%">
                                    <strong>Submit</strong>
                                </el-button>
                            </el-col>
                        </el-row>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<script>
    import { GET_HTTP_REQUEST, POST_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import {
        CREATE_PROJECT_PROFILE_ENDPOINT,
        SEARCH_GITHUB_REPOSITORIES_ENDPOINT,
        UPDATE_PROJECT_PROFILE_ENDPOINT
    } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    export default {
        name: "ProjectEditForm",
        props: {
            project: Object,
            projectForm: Object,
            dialogStatus: Object,
            closed: Function,
            gitHubRepoIdOptions: Array,
            gitHubTokenSetup: Boolean
        },
        data() {
            return {
                projectRules: {
                    name: [
                        {required: true, message: "Please, input project name", trigger: "change"},
                        {min: 1, max: 255, message: "Length should be from 1 to 255", trigger: "change"}
                    ],
                    alias: [
                        {required: true, message: "Please, input project alias", trigger: "change"},
                        {min: 1, max: 30, message: "Length should be from 1 to 30", trigger: "change"}
                    ],
                    description: [
                        {min: 1, max: 1024, message: "Length should be from 1 to 1024", trigger: "change"}
                    ],
                },
                gitHubRepoIdsSearchLoading: false
            };
        },
        methods: {
            cancel() {
                this.dialogStatus.visible = false;
            },
            submitForm(formName) {
                this.$refs.projectFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.projectFormButton.loading = false;
                        return false;
                    }

                    if (this.projectForm.id) {
                        this.$store.dispatch(PUT_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(UPDATE_PROJECT_PROFILE_ENDPOINT, {
                                id: this.projectForm.id
                            }),
                            data: {
                                name: this.projectForm.name,
                                alias: this.projectForm.alias,
                                description: this.projectForm.description,
                                gitHubRepoId: this.projectForm.gitHubRepoId,
                                isActive: this.projectForm.isActive
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.dialogStatus.visible = false;
                            this.$refs.projectFormButton.loading = false;

                            this.$notify.success({
                                title: "Project was updated",
                                message: "Profile changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.projectFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to update project",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    } else {
                        this.$store.dispatch(POST_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: CREATE_PROJECT_PROFILE_ENDPOINT,
                            data: {
                                name: this.projectForm.name,
                                alias: this.projectForm.alias,
                                description: this.projectForm.description,
                                gitHubRepoId: this.projectForm.gitHubRepoId,
                                isActive: this.projectForm.isActive
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.dialogStatus.visible = false;
                            this.$refs.projectFormButton.loading = false;

                            this.$notify.success({
                                title: "Project was created",
                                message: "Profile changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.projectFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to create project",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    }
                });
            },
            filterByGitHubRepoIds(query) {
                if (query !== "") {
                    this.gitHubRepoIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_GITHUB_REPOSITORIES_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.gitHubRepoIdsSearchLoading = false;
                        this.gitHubRepoIdOptions = response.data;
                    }).catch(error => {
                        this.gitHubRepoIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of GitHub repositories",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.gitHubRepoIdOptions = [];
                }
            }
        }
    };
</script>