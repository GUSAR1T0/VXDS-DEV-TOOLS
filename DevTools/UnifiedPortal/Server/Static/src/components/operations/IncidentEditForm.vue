<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">
            {{ incident.exists ? "Update" : "Initialize" }} Incident
        </span>
        <el-table :data="incident.operation.id > -1 ? [incident.operation] : []" style="width: 100%" border>
            <el-table-column label="Status" min-width="100" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 32px">
                        <fa :icon="defineOperationResultIcon(scope.row)"
                            :class="defineOperationResultClass(scope.row)"
                        />
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="Operation ID / Scope / Context" min-width="900">
                <template slot-scope="scope">
                    <div style="font-size: 22px; padding-bottom: 5px">
                        <strong>{{ scope.row.id }}</strong>
                    </div>
                    <div style="font-size: 18px">
                        <strong>{{ scope.row.scope }}</strong>
                    </div>
                    <div style="font-size: 16px">
                        <strong>({{ scope.row.contextName }})</strong>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="Performed by User" min-width="300">
                <template slot-scope="scope">
                    <UserAvatarAndFullNameWithLink
                            :first-name="getOperationFirstName(scope.row.firstName)"
                            :last-name="scope.row.lastName"
                            :color="scope.row.color"
                            :user-id="scope.row.userId"
                    />
                </template>
            </el-table-column>
            <el-table-column label="Operation Date / Time" min-width="400">
                <template slot-scope="scope">
                    <div style="font-size: 16px">
                        <strong>Start Time</strong>: {{ scope.row.startTime }}
                    </div>
                    <div style="font-size: 16px">
                        <strong>Stop Time</strong>:
                        {{ scope.row.stopTime ? scope.row.stopTime : "—" }}
                    </div>
                </template>
            </el-table-column>
        </el-table>
        <el-divider/>
        <el-form :model="incidentForm" :rules="incidentRules" ref="incidentForm" label-width="200px">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="authorId" label="Incident Author">
                        <el-select v-model="incidentForm.authorId" filterable remote reserve-keyword disabled
                                   :remote-method="filterByAuthorIds" :loading="incidentAuthorIdsSearchLoading"
                                   clearable style="width: 100%">
                            <el-option v-for="item in incidentAuthorIdOptions" :key="item.id"
                                       :label="item.fullName" :value="item.id"/>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="assigneeId" label="Incident Assignee">
                        <el-select v-model="incidentForm.assigneeId" filterable remote reserve-keyword
                                   :remote-method="filterByAssigneeIds" :loading="incidentAssigneeIdsSearchLoading"
                                   clearable style="width: 100%">
                            <el-option v-for="item in incidentAssigneeIdOptions" :key="item.id"
                                       :label="item.fullName" :value="item.id"/>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="status" label="Incident Status">
                        <el-select v-model="incidentForm.status" filterable reserve-keyword
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in getLookupValues('incidentStatuses')" :key="item.value"
                                       :label="item.name" :value="item.value"/>
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
                                <el-popover placement="top" width="400" v-model="incidentSubmitVisible">
                                    <p>Before applying, write some comment if it's needed:</p>
                                    <el-input v-model="incidentForm.comment" clearable type="textarea" maxlength="255"
                                              show-word-limit/>
                                    <el-button type="primary" ref="incidentFormButton" native-type="submit"
                                               @click="submitForm('incidentForm')"
                                               style="width: 100%; margin-top: 20px">
                                        <strong>Apply</strong>
                                    </el-button>
                                    <el-button type="primary" slot="reference"
                                               :disabled="!incidentForm.authorId || !incidentForm.assigneeId || !incidentForm.status"
                                               style="width: 100%">
                                        <strong>Submit</strong>
                                    </el-button>
                                </el-popover>
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

<style scoped src="@/styles/status.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { GET_HTTP_REQUEST, POST_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import {
        INITIALIZE_INCIDENT_PROFILE_ENDPOINT,
        SEARCH_USERS_ENDPOINT,
        UPDATE_INCIDENT_PROFILE_ENDPOINT
    } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { UNAUTHORIZED, UNASSIGNED } from "@/constants/formatPattern";

    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";

    export default {
        name: "IncidentEditForm",
        props: {
            incident: Object,
            incidentForm: Object,
            dialogStatus: Object,
            closed: Function,
            incidentAuthorIdOptions: Array,
            incidentAssigneeIdOptions: Array
        },
        components: {
            UserAvatarAndFullNameWithLink
        },
        data() {
            return {
                incidentRules: {},
                incidentAuthorIdsSearchLoading: false,
                incidentAssigneeIdsSearchLoading: false,
                incidentSubmitVisible: false,
                isSuccessful: false
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            cancel() {
                this.dialogStatus.visible = false;
            },
            submitForm(formName) {
                this.incidentSubmitVisible = false;
                this.$refs.incidentFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.incidentFormButton.loading = false;
                        return false;
                    }

                    if (this.incident.exists) {
                        this.$store.dispatch(PUT_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(UPDATE_INCIDENT_PROFILE_ENDPOINT, {
                                id: this.incidentForm.operation.id
                            }),
                            data: {
                                authorId: this.incidentForm.authorId,
                                assigneeId: this.incidentForm.assigneeId,
                                status: parseInt(this.incidentForm.status),
                                comment: this.incidentForm.comment
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.dialogStatus.visible = false;
                            this.$refs.incidentFormButton.loading = false;

                            this.$notify.success({
                                title: "Incident was updated",
                                message: "Profile changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.incidentFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to update operation incident",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    } else {
                        this.$store.dispatch(POST_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(INITIALIZE_INCIDENT_PROFILE_ENDPOINT, {
                                id: this.incidentForm.operation.id
                            }),
                            data: {
                                authorId: this.incidentForm.authorId,
                                assigneeId: this.incidentForm.assigneeId,
                                status: parseInt(this.incidentForm.status),
                                comment: this.incidentForm.comment
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.dialogStatus.visible = false;
                            this.$refs.incidentFormButton.loading = false;
                            this.$router.push(`/system/operation/${this.incidentForm.operation.id}/incident`);

                            this.$notify.success({
                                title: "Incident was initialized",
                                message: "Profile changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.incidentFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to initialize operation incident",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    }
                });
            },
            filterByAuthorIds(query) {
                if (query !== "") {
                    this.incidentAuthorIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_USERS_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.incidentAuthorIdsSearchLoading = false;
                        this.incidentAuthorIdOptions = response.data;
                    }).catch(error => {
                        this.incidentAuthorIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of authors",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.incidentAuthorIdOptions = [];
                }
            },
            filterByAssigneeIds(query) {
                if (query !== "") {
                    this.incidentAssigneeIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_USERS_ENDPOINT, {
                            pattern: query,
                            zeroUserName: UNASSIGNED
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.incidentAssigneeIdsSearchLoading = false;
                        this.incidentAssigneeIdOptions = response.data;
                    }).catch(error => {
                        this.incidentAssigneeIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of assignees",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.incidentAssigneeIdOptions = [];
                }
            },
            defineOperationResultIcon(row) {
                if (row.isSuccessful === true) {
                    return "check-circle";
                } else if (row.isSuccessful === false) {
                    return "times-circle";
                } else {
                    return "";
                }
            },
            defineOperationResultClass(row) {
                if (row.isSuccessful === true) {
                    return "success";
                } else if (row.isSuccessful === false) {
                    return "error";
                } else {
                    return "";
                }
            },
            getOperationFirstName(firstName) {
                return firstName ? firstName : UNAUTHORIZED;
            }
        }
    };
</script>