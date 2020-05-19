<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile header="Incident Inspection">
                <template slot="profile-buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Reload This Page
                        </div>
                        <el-button type="info" plain circle @click="$router.go(0)"
                                   class="rounded-button">
                            <span><fa icon="sync-alt"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            To Operations Page
                        </div>
                        <el-button type="info" plain circle @click="$router.push('/system/operations')"
                                   class="rounded-button">
                            <span><fa icon="file-alt"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top"
                                v-if="getIncident.operation.id > -1">
                        <div slot="content">
                            Edit This Incident
                        </div>
                        <el-button type="primary" plain circle
                                   @click="openDialogToUpdate" class="rounded-button">
                            <span><fa icon="edit"/></span>
                        </el-button>
                    </el-tooltip>

                    <IncidentEditForm
                            :dialog-status="dialogIncidentFormStatus"
                            :incident="getIncident"
                            :incident-form="getIncidentForm"
                            :closed="submitIncidentAction"
                            :incident-author-id-options="getIncidentAuthorOptions"
                            :incident-assignee-id-options="getIncidentAssigneeOptions"
                    />
                </template>
                <template slot="profile-content">
                    <ProfileBlock icon="file-alt" header="Operation Info">
                        <template slot="profile-block-content">
                            <el-table :data="getIncident.operation.id > -1 ? [getIncident.operation] : []"
                                      style="width: 100%" border>
                                <el-table-column type="expand">
                                    <template slot-scope="scope">
                                        <el-table :data="scope.row.logs" style="width: 100%" border>
                                            <el-table-column label="Date / Time" min-width="350">
                                                <template slot-scope="logScope">
                                                    <div style="font-size: 16px">{{ logScope.row.dateTime }}</div>
                                                </template>
                                            </el-table-column>
                                            <el-table-column label="Level" min-width="150">
                                                <template slot-scope="logScope">
                                                    <strong style="font-size: 18px">{{ logScope.row.level }}</strong>
                                                </template>
                                            </el-table-column>
                                            <el-table-column label="Logger" min-width="800">
                                                <template slot-scope="logScope">
                                                    <strong style="font-size: 16px">({{ logScope.row.logger }})</strong>
                                                </template>
                                            </el-table-column>
                                            <el-table-column label="Message" min-width="500">
                                                <template slot-scope="logScope">
                                                    <div style="font-size: 16px">{{ logScope.row.message }}</div>
                                                </template>
                                            </el-table-column>
                                            <el-table-column label="Additional Info" min-width="1000">
                                                <template slot-scope="logScope">
                                                    <div style="font-size: 16px">{{ logScope.row.value }}</div>
                                                </template>
                                            </el-table-column>
                                        </el-table>
                                    </template>
                                </el-table-column>
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
                        </template>
                    </ProfileBlock>
                    <ProfileBlock icon="exclamation-circle" header="Incident Description">
                        <template slot="profile-block-content">
                            <Row name="Initial Time" :value="getIncident.initialTime" half/>
                            <Row name="Status" :value="getStatusName(getIncident.status)" half/>
                            <DoubleRow>
                                <template slot="first">
                                    <Row name="Author" half>
                                        <template slot="description">
                                            <UserAvatarAndFullNameWithLink
                                                    :first-name="getIncident.authorFirstName"
                                                    :last-name="getIncident.authorLastName"
                                                    :color="getIncident.authorColor"
                                                    :user-id="getIncident.authorId"
                                            />
                                        </template>
                                    </Row>
                                </template>
                                <template slot="second">
                                    <Row name="Assignee" half>
                                        <template slot="description">
                                            <UserAvatarAndFullNameWithLink
                                                    :first-name="getIncident.assigneeFirstName"
                                                    :last-name="getIncident.assigneeLastName"
                                                    :color="getIncident.assigneeColor"
                                                    :user-id="getIncident.assigneeId"
                                            />
                                        </template>
                                    </Row>
                                </template>
                            </DoubleRow>
                        </template>
                    </ProfileBlock>
                    <ProfileBlock icon="history" header="Incident History">
                        <template slot="profile-block-content">
                            <Row name="Comment">
                                <template slot="description">
                                    <div class="field-content">
                                        <el-input v-model="comment" clearable type="textarea" maxlength="255"
                                                  show-word-limit/>
                                        <el-button type="primary" class="inline-field-element" ref="updateCommentButton"
                                                   @click="() => saveComment(comment, null, {buttonName: 'updateCommentButton'})">
                                            <strong>Save</strong>
                                        </el-button>
                                    </div>
                                </template>
                            </Row>
                            <el-timeline>
                                <el-timeline-item v-for="item in getIncident.history" :key="item"
                                                  :timestamp="item.changeTime" placement="top">
                                    <el-card shadow="hover">
                                        <ActionToolbar>
                                            <template slot="left">
                                                <div class="history-event history-event-header">
                                                    <UserAvatarAndFullNameWithLink
                                                            :first-name="item.changedByFirstName"
                                                            :last-name="item.changedByLastName"
                                                            :color="item.changedByColor"
                                                            :user-id="item.changedBy"
                                                    />
                                                    <div class="history-event-item-alt">made update:</div>
                                                </div>
                                                <div v-if="item.authorId"
                                                     class="history-event history-event-update history-event-with-user">
                                                    <div class="history-event-item">
                                                        • Incident author was changed to
                                                    </div>
                                                    <UserAvatarAndFullNameWithLink
                                                            :first-name="item.authorFirstName"
                                                            :last-name="item.authorLastName"
                                                            :color="item.authorColor"
                                                            :user-id="item.authorId"
                                                    />
                                                </div>
                                                <div v-if="item.assigneeId"
                                                     class="history-event history-event-update history-event-with-user">
                                                    <div class="history-event-item">
                                                        • Incident assignee was changed to
                                                    </div>
                                                    <UserAvatarAndFullNameWithLink
                                                            :first-name="item.assigneeFirstName"
                                                            :last-name="item.assigneeLastName"
                                                            :color="item.assigneeColor"
                                                            :user-id="item.assigneeId"
                                                    />
                                                </div>
                                                <div v-else-if="item.isUnassigned"
                                                     class="history-event history-event-update">
                                                    <div>• Incident was <strong>unassigned</strong></div>
                                                </div>
                                                <div v-if="item.status" class="history-event history-event-update">
                                                    <div>
                                                        • Incident status was changed to <strong>
                                                        "{{ getStatusName(item.status) }}"</strong>
                                                    </div>
                                                </div>
                                                <div v-if="item.comment" class="history-event history-event-update">
                                                    <div>
                                                        • Comment was added: <strong>{{ item.comment }}</strong>
                                                    </div>
                                                </div>
                                            </template>
                                            <template slot="right">
                                                <el-tooltip effect="dark" placement="top">
                                                    <div slot="content">
                                                        Edit This Comment
                                                    </div>
                                                    <el-button type="primary" plain circle
                                                               v-if="item.changedBy === getUserId || hasPermissionToManageIncidentComments"
                                                               @click="editComment(item.comment, item.id)"
                                                               class="rounded-button">
                                                        <span><fa icon="edit"/></span>
                                                    </el-button>
                                                </el-tooltip>

                                                <el-tooltip effect="dark" placement="top">
                                                    <div slot="content">
                                                        Delete This Comment
                                                    </div>
                                                    <el-button type="danger" plain circle
                                                               v-if="(item.changedBy === getUserId || hasPermissionToManageIncidentComments) && item.comment"
                                                               @click="deleteComment(item.comment, item.id)"
                                                               class="rounded-button">
                                                        <span><fa icon="trash-alt"/></span>
                                                    </el-button>
                                                </el-tooltip>
                                            </template>
                                        </ActionToolbar>
                                    </el-card>
                                </el-timeline-item>
                            </el-timeline>

                            <el-dialog
                                    :visible.sync="commentEditVisible.visible"
                                    style="text-align: center"
                                    width="75%">
                                <span slot="title" class="modal-title">
                                    Edit comment
                                </span>
                                <el-row class="auth-field-element" type="flex" justify="center"
                                        style="margin-bottom: 20px">
                                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                                        <el-input v-model="commentEditVisible.comment" clearable type="textarea"
                                                  maxlength="255" show-word-limit/>
                                    </el-col>
                                </el-row>
                                <el-row class="auth-field-element" type="flex" justify="center">
                                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                                        <el-row type="flex" justify="center" align="middle" :gutter="20">
                                            <el-col :span="12">
                                                <el-button @click="cancelEditComment" style="width: 100%">
                                                    <strong>Cancel</strong>
                                                </el-button>
                                            </el-col>
                                            <el-col :span="12">
                                                <el-button
                                                        @click="applyEditComment"
                                                        type="primary" style="width: 100%" ref="editCommentButton">
                                                    <strong>Submit</strong>
                                                </el-button>
                                            </el-col>
                                        </el-row>
                                    </el-col>
                                </el-row>
                            </el-dialog>

                            <ConfirmationDialog
                                    :dialog-status="commentDeleteVisible"
                                    confirmation-text="Are you sure that you want to delete the comment?"
                                    :additional-text="`Comment: ${commentDeleteVisible.comment}`"
                                    :cancel-click-action="cancelDeleteComment"
                                    :submit-click-action="applyDeleteComment"
                            />
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

<style scoped src="@/styles/modal.css">
</style>

<style scoped src="@/styles/status.css">
</style>

<style scoped>
    .history-event {
        display: flex;
        align-items: center;
        padding: 5px 0;
        font-size: 16px;
    }

    .history-event-with-user {
        margin-bottom: -5px;
        margin-top: -5px;
    }

    .history-event-update {
        margin-left: 15px;
    }

    .history-event-item {
        padding-right: 7px;
        padding-top: 5px;
        padding-bottom: 5px;
    }

    .history-event-item-alt {
        padding-left: 7px;
        padding-top: 5px;
        padding-bottom: 5px;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { UNAUTHORIZED } from "@/constants/formatPattern";
    import {
        DELETE_COMMENT_FOR_INCIDENT_ENDPOINT,
        GET_INCIDENT_PROFILE_ENDPOINT,
        SAVE_COMMENT_FOR_INCIDENT_ENDPOINT
    } from "@/constants/endpoints";
    import {
        DELETE_HTTP_REQUEST,
        GET_HTTP_REQUEST,
        PREPARE_INCIDENT_FORM,
        PUT_HTTP_REQUEST,
        RESET_INCIDENT_STORE_STATE,
        STORE_INCIDENT_DATA_REQUEST
    } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";
    import DoubleRow from "@/components/page/DoubleRow";
    import Row from "@/components/page/Row";
    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";
    import IncidentEditForm from "@/components/operations/IncidentEditForm";
    import ActionToolbar from "@/components/page/ActionToolbar";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";

    export default {
        name: "Incident",
        components: {
            LoadingContainer,
            Profile,
            ProfileBlock,
            DoubleRow,
            Row,
            UserAvatarAndFullNameWithLink,
            IncidentEditForm,
            ActionToolbar,
            ConfirmationDialog
        },
        data() {
            return {
                loadingIsActive: true,
                operationId: null,
                dialogIncidentFormStatus: {
                    visible: false
                },
                comment: "",
                commentEditVisible: {
                    visible: false,
                    historyId: 0,
                    comment: ""
                },
                commentDeleteVisible: {
                    visible: false,
                    historyId: 0,
                    comment: ""
                }
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getLookupValues",
                "getIncident",
                "getIncidentForm",
                "getIncidentAuthorOptions",
                "getIncidentAssigneeOptions",
                "getUserId"
            ]),
            hasPermissionToManageIncidentComments() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_INCIDENT_COMMENTS);
            }
        },
        methods: {
            loadIncident() {
                this.loadingIsActive = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_INCIDENT_PROFILE_ENDPOINT, {
                        id: this.operationId
                    }),
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.$store.commit(STORE_INCIDENT_DATA_REQUEST, {
                        operation: {
                            id: response.data.operationId,
                            scope: response.data.scope,
                            contextName: response.data.contextName,
                            isSuccessful: response.data.isSuccessful,
                            firstName: response.data.firstName,
                            lastName: response.data.lastName,
                            color: response.data.color,
                            userId: response.data.userId,
                            startTime: response.data.startTime,
                            stopTime: response.data.stopTime,
                            logs: response.data.logs
                        },
                        exists: true,
                        authorId: response.data.authorId,
                        authorColor: response.data.authorColor,
                        authorFirstName: response.data.authorFirstName,
                        authorLastName: response.data.authorLastName,
                        assigneeId: response.data.assigneeId,
                        assigneeColor: response.data.assigneeColor,
                        assigneeFirstName: response.data.assigneeFirstName,
                        assigneeLastName: response.data.assigneeLastName,
                        initialTime: response.data.initialTime,
                        status: `${response.data.status}`,
                        history: response.data.history
                    });
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load operation incident",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            openDialogToUpdate() {
                this.$store.commit(PREPARE_INCIDENT_FORM);
                this.dialogIncidentFormStatus.visible = true;
            },
            submitIncidentAction() {
                this.$store.commit(RESET_INCIDENT_STORE_STATE);
                this.loadIncident();
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
            },
            getStatusName(statusId) {
                let statuses = this.getLookupValues("incidentStatuses").filter(status => parseInt(status.value) === parseInt(statusId));
                return statuses && statuses.length > 0 ? statuses[0].name : "—";
            },
            editComment(comment, historyId) {
                this.commentEditVisible.comment = comment ? `${comment}` : "";
                this.commentEditVisible.historyId = historyId;
                this.commentEditVisible.visible = true;
            },
            cancelEditComment() {
                this.commentEditVisible.visible = false;
                this.commentEditVisible.comment = "";
                this.commentEditVisible.historyId = 0;
            },
            applyEditComment() {
                this.saveComment(this.commentEditVisible.comment, this.commentEditVisible.historyId, {
                    buttonName: "editCommentButton",
                    apply: () => this.commentEditVisible.visible = false
                });
            },
            deleteComment(comment, historyId) {
                this.commentDeleteVisible.comment = `${comment}`;
                this.commentDeleteVisible.historyId = historyId;
                this.commentDeleteVisible.visible = true;
            },
            cancelDeleteComment() {
                this.commentDeleteVisible.visible = false;
                this.commentDeleteVisible.comment = "";
                this.commentDeleteVisible.historyId = 0;
            },
            applyDeleteComment(button) {
                button.loading = true;

                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_COMMENT_FOR_INCIDENT_ENDPOINT, {
                        id: this.getIncident.operation.id,
                        historyId: this.commentDeleteVisible.historyId
                    }),
                    data: {},
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.comment = "";
                    this.commentDeleteVisible.visible = false;
                    this.loadIncident();

                    this.$notify.success({
                        title: "Comment was removed",
                        message: "History changes took effect"
                    });
                }).catch(error => {
                    button.loading = false;

                    this.$notify.error({
                        title: "Failed to remove comment",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            saveComment(comment, historyId, {buttonName, button, apply}) {
                let btn = button ? button : this.$refs[buttonName];
                btn.loading = true;

                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(SAVE_COMMENT_FOR_INCIDENT_ENDPOINT, {
                        id: this.getIncident.operation.id
                    }),
                    data: {
                        historyId: historyId,
                        comment: comment
                    },
                    config: getConfiguration()
                }).then(() => {
                    btn.loading = false;
                    this.comment = "";
                    if (apply) apply();
                    this.loadIncident();

                    this.$notify.success({
                        title: "Comment was saved",
                        message: "History changes took effect"
                    });
                }).catch(error => {
                    btn.loading = false;

                    this.$notify.error({
                        title: "Failed to save comment",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        },
        mounted() {
            this.operationId = this.$route.params.operationId;
            this.loadIncident();
        },
        beforeRouteUpdate(to, from, next) {
            this.operationId = to.params.operationId;
            this.loadIncident();
            next();
        }
    };
</script>