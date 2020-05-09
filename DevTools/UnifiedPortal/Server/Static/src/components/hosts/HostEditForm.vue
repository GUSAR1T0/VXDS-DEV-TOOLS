<template>
    <el-dialog :visible.sync="pageStatus.visible" width="75%" style="text-align: center" @closed="closed">
        <span slot="title" class="modal-title">
            {{ pageStatus.form && pageStatus.form.id > 0 ? "Update" : "Add" }} Host
        </span>
        <el-form :model="pageStatus.form" :rules="rules" ref="hostForm"
                 label-width="200px" @submit.native.prevent="submitForm('hostForm')">
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="name" label="Host Name">
                        <el-input v-model="pageStatus.form.name" clearable/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="domain" label="Host Domain">
                        <el-input v-model="pageStatus.form.domain" clearable/>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item prop="operatingSystem" label="Operating System">
                        <el-select v-model="pageStatus.form.operatingSystem" filterable reserve-keyword
                                   default-first-option :disabled="pageStatus.form && pageStatus.form.id > 0"
                                   style="width: 100%">
                            <el-option v-for="item in getLookupValues('hostOperatingSystems')" :key="item.value"
                                       :label="item.name" :value="parseInt(item.value)">
                                <div style="display: flex;">
                                    <div style="font-size: 14px; margin-right: 5px">
                                        <fa :icon="['fab', getOperatingSystemIcon(item.value)]"/>
                                    </div>
                                    {{ getOperatingSystemName(item.value) }}
                                </div>
                            </el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <div v-for="(credentials, index) in pageStatus.form.credentials" :key="credentials">
                <el-divider/>
                <el-row class="auth-field-element" type="flex" justify="center">
                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                        <el-form-item>
                            <strong>Credentials #{{ index + 1 }}</strong>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row class="auth-field-element" type="flex" justify="center">
                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                        <el-form-item label="Connection Type" :prop="`credentials.${index}.type`">
                            <el-select v-model="credentials.type" filterable reserve-keyword
                                       default-first-option style="width: 100%">
                                <el-option v-for="item in getConnectionTypes()" :key="item.value"
                                           :label="item.name" :value="parseInt(item.value)"/>
                            </el-select>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row class="auth-field-element" type="flex" justify="center">
                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                        <el-form-item label="Port" :prop="`credentials.${index}.port`">
                            <el-input-number v-model="credentials.port" :min="0" :max="65535" style="width: 100%;"/>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row class="auth-field-element" type="flex" justify="center">
                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                        <el-form-item label="Username" :prop="`credentials.${index}.username`">
                            <el-input v-model="credentials.username" clearable/>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row class="auth-field-element" type="flex" justify="center">
                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                        <el-form-item label="Password" :prop="`credentials.${index}.password`">
                            <el-input v-model="credentials.password" clearable/>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row class="auth-field-element" type="flex" justify="center">
                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                        <el-form-item>
                            <el-button type="info" plain style="width: 100%" @click="checkConnection(credentials)">
                                <strong>Check these credentials</strong>
                            </el-button>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row class="auth-field-element" type="flex" justify="center">
                    <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                        <el-form-item>
                            <el-button type="danger" plain style="width: 100%"
                                       @click="pageStatus.form.credentials.splice(parseInt(index), 1)">
                                <strong>Remove these credentials</strong>
                            </el-button>
                        </el-form-item>
                    </el-col>
                </el-row>
            </div>
            <el-divider style="margin-top: 0;"/>
            <el-row class="auth-field-element" type="flex" justify="center">
                <el-col :xs="24" :sm="20" :md="16" :lg="12" :xl="8">
                    <el-form-item>
                        <el-button type="primary" plain
                                   style="width: 100%" @click="pageStatus.form.credentials.push({})">
                            <strong>Add new credentials</strong>
                        </el-button>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-divider/>
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
                                <el-button type="primary" ref="hostFormButton" native-type="submit"
                                           style="width: 100%">
                                    <strong>Submit</strong>
                                </el-button>
                            </el-col>
                        </el-row>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>

        <CheckConnectionsDialog :page-status="checkConnectionsDialog" :closed="reload"/>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<style scoped>
    .el-divider--horizontal {
        margin-top: 0;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { POST_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { ADD_HOST_ENDPOINT, UPDATE_HOST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    function validateDomain(rule, value, callback) {
        if (value.toLowerCase().trim() === "localhost" ||
            /^((\*)[^*]|((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)|((\*\.)?([a-zA-Z0-9-]+\.){0,5}[a-zA-Z0-9-]+\.[a-zA-Z]{2,63}?))$/gm.test(value)) {
            callback();
        } else {
            callback(new Error("Please, input correct domain"));
        }
    }

    import CheckConnectionsDialog from "@/components/hosts/CheckConnectionsDialog";

    export default {
        name: "HostEditForm",
        props: {
            pageStatus: Object,
            closed: Function
        },
        components: {
            CheckConnectionsDialog
        },
        data() {
            return {
                rules: {
                    name: [
                        {required: true, message: "Please, input host name", trigger: "change"},
                        {min: 1, max: 50, message: "Length should be from 1 to 50", trigger: "change"}
                    ],
                    domain: [
                        {required: true, message: "Please, input host domain", trigger: "change"},
                        {validator: validateDomain, trigger: "change"}
                    ],
                    operatingSystem: [
                        {required: true, message: "Please, input host operating system", trigger: "change"}
                    ]
                },
                checkConnectionsDialog: {
                    visible: false,
                    hostName: null,
                    hostDomain: null,
                    hostOperatingSystem: null,
                    hostCredentials: {}
                }
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            getOperatingSystemIcon(osId) {
                let osList = this.getLookupValues("hostOperatingSystems").filter(os => os.value === osId);
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
            getOperatingSystemName(osId) {
                let osList = this.getLookupValues("hostOperatingSystems").filter(os => os.value === osId);
                return osList && osList.length > 0 ? osList[0].name : "—";
            },
            getConnectionType(typeId) {
                let types = this.getLookupValues("hostConnectionTypes").filter(type => parseInt(type.value) === typeId);
                return types && types.length > 0 ? types[0].name : "—";
            },
            getConnectionTypes() {
                return this.getLookupValues("hostConnectionTypes");
            },
            cancel() {
                this.pageStatus.visible = false;
            },
            submitForm(formName) {
                this.$refs.hostFormButton.loading = true;
                this.$refs[formName].validate((valid) => {
                    if (!valid) {
                        this.$refs.hostFormButton.loading = false;
                        return false;
                    }

                    if (this.pageStatus.host.id) {
                        this.$store.dispatch(PUT_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: format(UPDATE_HOST_ENDPOINT, {
                                id: this.pageStatus.host.id
                            }),
                            data: {
                                name: this.pageStatus.form.name,
                                domain: this.pageStatus.form.domain,
                                operatingSystem: this.pageStatus.form.operatingSystem,
                                credentials: this.pageStatus.form.credentials
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.$refs.hostFormButton.loading = false;
                            this.pageStatus.visible = false;

                            this.$notify.success({
                                title: "Host was updated",
                                message: "Host changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.hostFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to update host",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    } else {
                        this.$store.dispatch(POST_HTTP_REQUEST, {
                            server: LOCALHOST,
                            endpoint: ADD_HOST_ENDPOINT,
                            data: {
                                name: this.pageStatus.form.name,
                                domain: this.pageStatus.form.domain,
                                operatingSystem: this.pageStatus.form.operatingSystem,
                                credentials: this.pageStatus.form.credentials
                            },
                            config: getConfiguration()
                        }).then(() => {
                            this.$refs.hostFormButton.loading = false;
                            this.pageStatus.visible = false;

                            this.$notify.success({
                                title: "Host was created",
                                message: "Host changes took effect"
                            });
                        }).catch(error => {
                            this.$refs.hostFormButton.loading = false;

                            this.$notify.error({
                                title: "Failed to create host",
                                duration: 10000,
                                message: renderErrorNotificationMessage(this.$createElement, error.response)
                            });
                        });
                    }
                });
            },
            checkConnection(credentials) {
                this.checkConnectionsDialog.visible = true;
                this.checkConnectionsDialog.hostName = this.pageStatus.form.name;
                this.checkConnectionsDialog.hostDomain = this.pageStatus.form.domain;
                this.checkConnectionsDialog.hostOperatingSystem = this.pageStatus.form.operatingSystem;
                this.checkConnectionsDialog.hostCredentials = credentials;
            }
        }
    };
</script>