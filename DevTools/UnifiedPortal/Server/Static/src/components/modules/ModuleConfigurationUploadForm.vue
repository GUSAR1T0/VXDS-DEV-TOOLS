<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">Upload Module Configuration</span>
        <el-upload drag
                   action=""
                   :file-list="files"
                   :show-file-list="false"
                   :before-upload="beforeUpload"
                   :http-request="uploadFile"
                   :on-success="onSuccess"
                   :on-error="onError"
        >
            <i v-if="!loadingIsActive" class="el-icon-upload"/>
            <i v-else class="el-icon-loading upload"/>
            <div class="el-upload__text">Drop file here or <em>click to upload</em></div>
            <div class="el-upload__tip" slot="tip">Availvable formats: <strong>YAML/YML, JSON</strong></div>
        </el-upload>
        <el-divider v-if="module"/>
        <div v-if="module">
            <el-table :data="getTable" style="width: 100%" border>
                <el-table-column label="Parameter" align="center" width="300">
                    <template slot-scope="scope">
                        <strong style="font-size: 18px">{{ scope.row.parameter }}</strong>
                    </template>
                </el-table-column>
                <el-table-column label="Value" align="center">
                    <template slot-scope="scope">
                        <div v-if="scope.row.value.showOperatingSystems">
                            <div style="font-size: 16px; padding: 5px 0" v-for="os in module.operatingSystems"
                                 :key="os">
                                <fa :icon="['fab', getOperatingSystemIcon(os)]"/>
                                {{ getOperatingSystemName(os) }}
                            </div>
                        </div>
                        <el-select v-model="module.userId" filterable remote reserve-keyword
                                   :remote-method="filterByUserIds" :loading="userIdsSearchLoading"
                                   style="width: 100%" v-else-if="scope.row.value.selectByUser"
                                   :disabled="module.verdict !== 1 && module.verdict !== 3"
                                   clearable>
                            <el-option v-for="item in userIdOptions" :key="item.id"
                                       :label="item.fullName" :value="item.id"/>
                        </el-select>
                        <el-select v-model="module.hostId" filterable remote reserve-keyword
                                   :remote-method="filterByHostIds" :loading="hostIdsSearchLoading"
                                   style="width: 100%" v-else-if="scope.row.value.selectByHost"
                                   :disabled="module.verdict !== 1"
                                   clearable>
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
                    <el-button style="width: 100%" @click="close">
                        <strong>Cancel</strong>
                    </el-button>
                </el-col>
                <el-col :span="12">
                    <el-button type="primary" style="width: 100%"
                               :disabled="(module.verdict !== 1 && module.verdict !== 3) || !module.userId || !module.hostId"
                               @click="submit" ref="submitButton"
                    >
                        <strong>Submit</strong>
                    </el-button>
                </el-col>
            </el-row>
        </div>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<style scoped>
    .el-upload__tip {
        font-size: 14px;
        padding-top: 15px;
    }

    .upload {
        font-size: 67px;
        color: #c0c4cc;
        margin: 40px 0 16px;
        line-height: 50px;
    }
</style>

<script>
    import { getConfiguration, getUserFullName, renderErrorNotificationMessage } from "@/extensions/utils";
    import { GET_HTTP_REQUEST, POST_HTTP_REQUEST, UPLOAD_FILE_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import {
        SEARCH_HOSTS_ENDPOINT,
        SEARCH_USERS_ENDPOINT,
        SUBMIT_MODULE_CONFIGURATION_ENDPOINT,
        UPLOAD_MODULE_CONFIGURATIONS_ENDPOINT
    } from "@/constants/endpoints";
    import { mapGetters } from "vuex";
    import format from "string-format";

    export default {
        name: "ModuleConfigurationUploadForm",
        props: {
            dialogStatus: Object,
            closed: Function
        },
        data() {
            return {
                loadingIsActive: false,
                files: [],
                module: null,
                userIdsSearchLoading: false,
                userIdOptions: [],
                hostIdsSearchLoading: false,
                hostIdOptions: []
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues",
                "getUserId",
                "getFullName"
            ]),
            getTable() {
                return [
                    {
                        parameter: "Alias",
                        value: this.module.alias
                    },
                    {
                        parameter: "Name",
                        value: `${this.module.oldName ? `${this.module.oldName} → ` : ""}${this.module.newName}`
                    },
                    {
                        parameter: "Version",
                        value: `${this.module.oldVersion ? `${this.module.oldVersion} → ` : ""}${this.module.newVersion}`
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
                    },
                    {
                        parameter: "Verdict",
                        value: this.getVerdictName(this.module.verdict)
                    }
                ];
            }
        },
        methods: {
            beforeUpload(file) {
                this.files = [];
                this.files.push(file);

                this.module = null;
                this.userIdOptions = [];
                this.loadingIsActive = true;
            },
            uploadFile() {
                let formData = new FormData();
                formData.append("files", this.files[0], this.files[0].name);

                return this.$store.dispatch(UPLOAD_FILE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: UPLOAD_MODULE_CONFIGURATIONS_ENDPOINT,
                    data: formData,
                    config: getConfiguration()
                });
            },
            onSuccess(response) {
                this.module = response.data && response.data.length > 0 ? response.data[0] : null;
                if (this.module) {
                    if (this.module.verdict === 1) {
                        this.module.userId = this.getUserId;
                        this.userIdOptions = [ {
                            id: this.getUserId,
                            fullName: this.getFullName
                        } ];
                    } else {
                        this.userIdOptions = this.module.userId ? [ {
                            id: this.module.userId,
                            fullName: getUserFullName(this.module.firstName, this.module.lastName)
                        } ] : [];
                    }

                    this.hostIdOptions = this.module.hostId ? [ {
                        id: this.module.hostId,
                        name: this.module.hostName,
                        domain: this.module.hostDomain,
                        operatingSystem: this.module.hostOperatingSystem
                    } ] : [];
                }
                this.loadingIsActive = false;
            },
            onError(error) {
                this.$notify.error({
                    title: "Failed to upload configuration",
                    duration: 10000,
                    message: renderErrorNotificationMessage(this.$createElement, error.response)
                });
                this.loadingIsActive = false;
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
                if (query !== "" && this.module.operatingSystems && this.module.operatingSystems.length > 0) {
                    this.hostIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_HOSTS_ENDPOINT, {
                            pattern: query,
                            operatingSystems: "&" + this.module.operatingSystems.map(item => `os=${item}`).join("&")
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
            getVerdictName(verdictId) {
                let verdicts = this.getLookupValues("moduleConfigurationVerdicts").filter(item => parseInt(item.value) === verdictId);
                return verdicts && verdicts.length > 0 ? verdicts[0].name : "—";
            },
            close() {
                this.dialogStatus.visible = false;
                this.files = [];
                this.module = null;
            },
            submit() {
                let button = this.$refs.submitButton;
                button.loading = true;

                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: SUBMIT_MODULE_CONFIGURATION_ENDPOINT,
                    data: {
                        moduleId: this.module.moduleId,
                        fileId: this.module.fileId,
                        userId: this.module.userId,
                        hostId: this.module.hostId
                    },
                    config: getConfiguration()
                }).then(response => {
                    button.loading = false;
                    this.$notify.success({
                        title: "Configuration submission is success",
                        message: "Configuration was saved and the system started to configure this module"
                    });
                    this.$router.push(`/components/module/${response.data}`);
                }).catch(error => {
                    button.loading = false;
                    this.$notify.error({
                        title: "Failed to submit configuration",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>