<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed" @open="prepareModal">
        <span slot="title" class="modal-title">{{ getProcessName }} Module</span>
        <el-table :data="getTable" style="width: 100%" border>
            <el-table-column label="Parameter" align="center" width="300">
                <template slot-scope="scope">
                    <strong style="font-size: 18px">{{ scope.row.parameter }}</strong>
                </template>
            </el-table-column>
            <el-table-column label="Value" align="center">
                <template slot-scope="scope">
                    <div v-if="scope.row.value.showOperatingSystems && getConfiguration">
                        <div style="font-size: 16px; padding: 5px 0" v-for="os in getConfiguration.operatingSystems"
                             :key="os">
                            <fa :icon="['fab', getOperatingSystemIcon(os)]"/>
                            {{ getOperatingSystemName(os) }}
                        </div>
                    </div>
                    <el-select v-model="moduleForm.userId" filterable remote reserve-keyword
                               :remote-method="filterByUserIds" :loading="userIdsSearchLoading"
                               style="width: 100%" v-else-if="scope.row.value.selectByUser"
                               clearable>
                        <el-option v-for="item in userIdOptions" :key="item.id"
                                   :label="item.fullName" :value="item.id"/>
                    </el-select>
                    <el-select v-model="moduleForm.hostId" filterable remote reserve-keyword
                               :remote-method="filterByHostIds" :loading="hostIdsSearchLoading"
                               style="width: 100%" v-else-if="scope.row.value.selectByHost"
                               disabled clearable>
                        <el-option v-for="item in hostIdOptions" :key="item.id"
                                   :label="`${item.name} (${item.domain})`" :value="item.id">
                            <div style="display: flex; font-size: 14px">
                                <div style="margin-right: 5px">
                                    <fa :icon="['fab', getOperatingSystemIcon(item.operatingSystem)]"/>
                                </div>
                                {{ item.name }} ({{ item.domain }})
                            </div>
                        </el-option>
                    </el-select>
                    <div v-else style="font-size: 16px">{{ scope.row.value }}</div>
                </template>
            </el-table-column>
        </el-table>
        <el-row type="flex" justify="center" align="middle" :gutter="20" style="padding-top: 25px">
            <el-col :span="12">
                <el-button style="width: 100%" @click="cancel">
                    <strong>Cancel</strong>
                </el-button>
            </el-col>
            <el-col :span="12">
                <el-button type="primary" style="width: 100%"
                           :disabled="!moduleForm.userId"
                           @click="submit" ref="submitButton"
                >
                    <strong>Submit</strong>
                </el-button>
            </el-col>
        </el-row>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { GET_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import {
        DOWNGRADE_MODULE_CONFIGURATION_ENDPOINT, SEARCH_HOSTS_ENDPOINT, SEARCH_USERS_ENDPOINT,
        UPDATE_MODULE_ENDPOINT,
        UPGRADE_MODULE_CONFIGURATION_ENDPOINT
    } from "@/constants/endpoints";
    import { getConfiguration, getUserFullName, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    export default {
        name: "ModuleUpdateForm",
        props: {
            dialogStatus: Object,
            module: Object,
            closed: Function
        },
        data() {
            return {
                moduleForm: {},

                userIdsSearchLoading: false,
                userIdOptions: [],
                hostIdsSearchLoading: false,
                hostIdOptions: []
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ]),
            getProcessName() {
                if (this.dialogStatus.process === 1) {
                    return "Update";
                } else if (this.dialogStatus.process === 2) {
                    return "Upgrade";
                } else if (this.dialogStatus.process === 3) {
                    return "Downgrade";
                }

                return "";
            },
            getEndpointForModifying() {
                if (this.dialogStatus.process === 1) {
                    return UPDATE_MODULE_ENDPOINT;
                } else if (this.dialogStatus.process === 2) {
                    return UPGRADE_MODULE_CONFIGURATION_ENDPOINT;
                } else if (this.dialogStatus.process === 3) {
                    return DOWNGRADE_MODULE_CONFIGURATION_ENDPOINT;
                }

                return "";
            },
            getSuccessResponseMessage() {
                if (this.dialogStatus.process === 1) {
                    return "Profile changes took effect";
                } else if (this.dialogStatus.process === 2) {
                    return "The status of module was triggered for upgrading";
                } else if (this.dialogStatus.process === 3) {
                    return "The status of module was triggered for downgrading";
                }

                return "";
            },
            getErrorResponseTitle() {
                if (this.dialogStatus.process === 1) {
                    return "Failed to update module";
                } else if (this.dialogStatus.process === 2) {
                    return "Failed to upgrade module";
                } else if (this.dialogStatus.process === 3) {
                    return "Failed to downgrade module";
                }

                return "";
            },
            getNextConfiguration() {
                let configurations = this.module.configurations.filter(item => item.id === this.module.nextConfigurationId);
                return configurations && configurations.length > 0 ? configurations[0] : null;
            },
            getPreviousConfiguration() {
                let configurations = this.module.configurations.filter(item => item.id === this.module.previousConfigurationId);
                return configurations && configurations.length > 0 ? configurations[0] : null;
            },
            getCurrentConfiguration() {
                let configurations = this.module.configurations.filter(item => item.version === this.module.version);
                return configurations && configurations.length > 0 ? configurations[0] : null;
            },
            getConfiguration() {
                if (this.dialogStatus.process === 1) {
                    return this.getCurrentConfiguration;
                } else if (this.dialogStatus.process === 2) {
                    return this.getNextConfiguration;
                } else if (this.dialogStatus.process === 3) {
                    return this.getPreviousConfiguration;
                }

                return null;
            },
            getTable() {
                return [
                    {
                        parameter: "Alias",
                        value: this.module ? this.module.alias : "—"
                    },
                    {
                        parameter: "Name",
                        value: this.module ? `${this.module.name}${this.dialogStatus.process > 1 ? ` → ${this.getConfiguration.name}` : ""}` : "—"
                    },
                    {
                        parameter: "Version",
                        value: this.module ? `${this.module.version}${this.dialogStatus.process > 1 ? ` → ${this.getConfiguration.version}` : ""}` : "—"
                    },
                    {
                        parameter: "Operating Systems",
                        value: {
                            showOperatingSystems: true
                        }
                    },
                    {
                        parameter: "Responsible User",
                        value: {
                            selectByUser: true
                        }
                    },
                    {
                        parameter: "Host",
                        value: {
                            selectByHost: true
                        }
                    }
                ];
            }
        },
        methods: {
            prepareModal() {
                this.moduleForm = JSON.parse(JSON.stringify(this.module));

                this.userIdOptions = this.module.userId ? [ {
                    id: this.module.userId,
                    fullName: getUserFullName(this.module.firstName, this.module.lastName)
                } ] : [];

                this.hostIdOptions = this.module.hostId ? [ {
                    id: this.module.hostId,
                    name: this.module.hostName,
                    domain: this.module.hostDomain,
                    operatingSystem: this.module.hostOperatingSystem
                } ] : [];
            },
            filterByUserIds(query) {
                if (query !== "") {
                    this.userIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_USERS_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.userIdsSearchLoading = false;
                        this.userIdOptions = response.data;
                    }).catch(error => {
                        this.userIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of users",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.userIdOptions = [];
                }
            },
            filterByHostIds(query) {
                if (query !== "" && this.getConfiguration.operatingSystems && this.getConfiguration.operatingSystems.length > 0) {
                    this.hostIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_HOSTS_ENDPOINT, {
                            pattern: query,
                            operatingSystems: "&" + this.getConfiguration.operatingSystems.map(item => `os=${item}`).join("&")
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.hostIdsSearchLoading = false;
                        this.hostIdOptions = response.data;
                    }).catch(error => {
                        this.hostIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of hosts",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.hostIdOptions = [];
                }
            },
            getOperatingSystemName(osId) {
                let osList = this.getLookupValues("hostOperatingSystems").filter(os => parseInt(os.value) === osId);
                return osList && osList.length > 0 ? osList[0].name : "—";
            },
            getOperatingSystemIcon(osId) {
                let osList = this.getLookupValues("hostOperatingSystems").filter(os => parseInt(os.value) === parseInt(osId));
                if (osList && osList.length > 0) {
                    let os = osList[0];
                    if (os.value === "1") {
                        return "windows";
                    } else if (os.value === "2") {
                        return "linux";
                    } else if (os.value === "3") {
                        return "apple";
                    }
                } else {
                    return "question-circle";
                }
            },
            cancel() {
                this.dialogStatus.visible = false;
            },
            submit() {
                this.$refs.submitButton.loading = true;
                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(this.getEndpointForModifying, {
                        id: this.moduleForm.id
                    }),
                    data: {
                        userId: this.moduleForm.userId
                    },
                    config: getConfiguration()
                }).then(() => {
                    this.dialogStatus.visible = false;
                    this.$refs.submitButton.loading = false;

                    this.$notify.success({
                        title: "This module was updated",
                        message: this.getSuccessResponseMessage
                    });
                }).catch(error => {
                    this.$refs.submitButton.loading = false;

                    this.$notify.error({
                        title: this.getErrorResponseTitle,
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>