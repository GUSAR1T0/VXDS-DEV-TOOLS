<template>
    <div>
        <el-table :data="items" style="width: 100%" border>
            <el-table-column type="expand">
                <template slot-scope="scope">
                    <Profile>
                        <template slot="profile-content">
                            <ProfileBlock icon="exclamation-circle" header="Incident" v-if="scope.row.incident">
                                <template slot="profile-block-content">
                                    <el-table :data="[scope.row.incident]" style="width: 100%" border
                                              highlight-current-row default-expand-all>
                                        <el-table-column label="Initial Date / Time" min-width="350">
                                            <template slot-scope="incidentScope">
                                                <div style="font-size: 16px">
                                                    {{ incidentScope.row.initialTime }}
                                                </div>
                                            </template>
                                        </el-table-column>
                                        <el-table-column label="Status" min-width="350">
                                            <template slot-scope="incidentScope">
                                                <strong style="font-size: 18px">
                                                    {{ getStatusName(incidentScope.row.status) }}
                                                </strong>
                                            </template>
                                        </el-table-column>
                                        <el-table-column label="Author" min-width="300">
                                            <template slot-scope="incidentScope">
                                                <UserAvatarAndFullNameWithLink
                                                        :first-name="incidentScope.row.authorFirstName"
                                                        :last-name="incidentScope.row.authorLastName"
                                                        :color="incidentScope.row.authorColor"
                                                        :user-id="incidentScope.row.authorId"
                                                />
                                            </template>
                                        </el-table-column>
                                        <el-table-column label="Assignee" min-width="300">
                                            <template slot-scope="incidentScope">
                                                <UserAvatarAndFullNameWithLink
                                                        :first-name="getAssigneeFirstName(incidentScope.row.assigneeFirstName)"
                                                        :last-name="incidentScope.row.assigneeLastName"
                                                        :color="incidentScope.row.assigneeColor"
                                                        :user-id="incidentScope.row.assigneeId"
                                                />
                                            </template>
                                        </el-table-column>
                                    </el-table>
                                </template>
                            </ProfileBlock>
                            <ProfileBlock icon="file-alt" header="Logs">
                                <template slot="profile-block-content">
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
                            </ProfileBlock>
                        </template>
                    </Profile>
                </template>
            </el-table-column>
            <el-table-column label="Incident" min-width="100" align="center">
                <template slot-scope="scope">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            To Incident Page
                        </div>
                        <el-button type="info" plain circle
                                   @click="$router.push(`/system/operation/${scope.row.id}/incident`)"
                                   class="rounded-button" v-if="scope.row.incident">
                            <span><fa icon="exclamation-triangle"/></span>
                        </el-button>
                    </el-tooltip>
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Initialize Incident
                        </div>
                        <el-button type="primary" plain circle @click="openDialogToInitialize(scope.row)"
                                   class="rounded-button" v-if="!scope.row.incident">
                            <span><fa icon="fire-alt"/></span>
                        </el-button>
                    </el-tooltip>
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
            <el-table-column label="Operation ID / Scope / Context" min-width="900" align="center">
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
            <el-table-column label="Performed by User" min-width="300" align="center">
                <template slot-scope="scope">
                    <UserAvatarAndFullNameWithLink
                            style="text-align: left"
                            :first-name="getOperationFirstName(scope.row.firstName)"
                            :last-name="scope.row.lastName"
                            :color="scope.row.color"
                            :user-id="scope.row.userId"
                    />
                </template>
            </el-table-column>
            <el-table-column label="Operation Date / Time" min-width="400" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 16px">
                        <strong>Start Time</strong>: {{ scope.row.startTime }}
                    </div>
                    <div style="font-size: 16px">
                        <strong>Stop Time</strong>: {{ scope.row.stopTime ? scope.row.stopTime : "—" }}
                    </div>
                </template>
            </el-table-column>
        </el-table>

        <IncidentEditForm
                :dialog-status="dialogIncidentFormStatus"
                :incident="getIncident"
                :incident-form="getIncidentForm"
                :closed="submitIncidentAction"
                :incident-author-id-options="getIncidentAuthorOptions"
                :incident-assignee-id-options="getIncidentAssigneeOptions"
        />
    </div>
</template>

<style scoped src="@/styles/button.css">
</style>

<style scoped>
    .is-successful {
        color: #0C7C59;
    }

    .is-unsuccessful {
        color: #DB2B3D;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { UNASSIGNED, UNAUTHORIZED } from "@/constants/formatPattern";
    import {
        PREPARE_INCIDENT_FORM,
        RESET_INCIDENT_STORE_STATE,
        STORE_INCIDENT_DATA_REQUEST
    } from "@/constants/actions";

    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";
    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";
    import IncidentEditForm from "@/components/operations/IncidentEditForm";

    export default {
        name: "OperationsTable",
        props: {
            items: Array,
            reload: Function
        },
        components: {
            Profile,
            ProfileBlock,
            UserAvatarAndFullNameWithLink,
            IncidentEditForm
        },
        data() {
            return {
                dialogIncidentFormStatus: {
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues",
                "getIncident",
                "getIncidentForm",
                "getIncidentAuthorOptions",
                "getIncidentAssigneeOptions",
                "getUserId",
                "getFullName"
            ])
        },
        methods: {
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
                    return "is-successful";
                } else if (row.isSuccessful === false) {
                    return "is-unsuccessful";
                } else {
                    return "";
                }
            },
            getOperationFirstName(firstName) {
                return firstName ? firstName : UNAUTHORIZED;
            },
            getAssigneeFirstName(firstName) {
                return firstName ? firstName : UNASSIGNED;
            },
            getStatusName(statusId) {
                let statuses = this.getLookupValues("incidentStatuses").filter(status => parseInt(status.value) === statusId);
                return statuses && statuses.length > 0 ? statuses[0].name : "—";
            },
            openDialogToInitialize(operation) {
                this.$store.commit(STORE_INCIDENT_DATA_REQUEST, {
                    operation: operation,
                    exists: !!operation.incident,
                    authorId: this.getUserId,
                    authorFirstName: this.getFullName,
                    status: "1"
                });
                this.$store.commit(PREPARE_INCIDENT_FORM);
                this.dialogIncidentFormStatus.visible = true;
            },
            submitIncidentAction() {
                this.$store.commit(RESET_INCIDENT_STORE_STATE);
            }
        }
    };
</script>