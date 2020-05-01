<template>
    <div>
        <el-table :data="hosts" style="width: 100%" border>
            <el-table-column type="expand">
                <template slot-scope="scope">
                    <el-table :data="scope.row.credentials" style="width: 100%" border>
                        <el-table-column label="Connection Type" align="center">
                            <template slot-scope="credentials">
                                <strong style="font-size: 16px">
                                    {{ getConnectionType(credentials.row.type) }}
                                </strong>
                            </template>
                        </el-table-column>
                        <el-table-column label="Port" align="center">
                            <template slot-scope="credentials">
                                <div style="font-size: 16px">
                                    <strong v-if="credentials.row.port">
                                        {{ credentials.row.port }}
                                    </strong>
                                    <strong v-else>—</strong>
                                </div>
                            </template>
                        </el-table-column>
                        <el-table-column label="Username" align="center">
                            <template slot-scope="credentials">
                                <div style="font-size: 16px">
                                    <strong v-if="credentials.row.username">
                                        {{ credentials.row.username }}
                                    </strong>
                                    <strong v-else>—</strong>
                                </div>
                            </template>
                        </el-table-column>
                        <el-table-column label="Password" align="center">
                            <template slot-scope="credentials">
                                <div style="font-size: 16px">
                                    <strong v-if="credentials.row.password">
                                        {{ credentials.row.password }}
                                    </strong>
                                    <strong v-else>—</strong>
                                </div>
                            </template>
                        </el-table-column>
                    </el-table>
                </template>
            </el-table-column>
            <el-table-column label="Host ID" min-width="100" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">{{ scope.row.id }}</strong>
                </template>
            </el-table-column>
            <el-table-column label="Manage Host" min-width="300" align="center">
                <template slot-scope="scope">
                    <el-tooltip effect="dark" placement="top" v-if="scope.row.credentials.length > 0">
                        <div slot="content">
                            Check Connection{{ getCredentialsCount(scope.row) ? "s" : "" }} To This Host
                        </div>
                        <el-button type="info" plain circle class="rounded-button">
                            <span><fa icon="plug"/></span>
                        </el-button>
                    </el-tooltip>
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Edit This Host
                        </div>
                        <el-button type="primary" plain circle class="rounded-button"
                                   @click="openEditHostDialog(scope.row)">
                            <span><fa icon="edit"/></span>
                        </el-button>
                    </el-tooltip>
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Delete This Host
                        </div>
                        <el-button type="danger" plain circle class="rounded-button"
                                   :ref="`deleteHostButton-${scope.row.id}`"
                                   @click="openDeleteHostDialog(scope.row)">
                            <span><fa icon="trash-alt"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
            </el-table-column>
            <el-table-column label="Host" min-width="600" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 20px">
                        <strong>{{ scope.row.name }}</strong>
                    </div>
                    <div style="font-size: 16px">
                        <strong>Domain:</strong> {{ scope.row.domain }}
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="Operation System" min-width="400" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 18px">
                        <fa :icon="['fab', getOperationSystemIcon(scope.row.operationSystem)]"/>
                        {{ getOperationSystemName(scope.row.operationSystem) }}
                    </div>
                </template>
            </el-table-column>
        </el-table>

        <HostEditForm :page-status="hostEditDialog" :closed="reload"/>

        <ConfirmationDialog
                :dialog-status="hostDeleteDialog"
                :confirmation-text="hostDeleteDialog.confirmationText"
                :additional-text="hostDeleteDialog.additionalText"
                :cancel-click-action="() => hostDeleteDialog.visible = false"
                :submit-click-action="deleteHost"
                :closed="reload"/>
    </div>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { DELETE_HTTP_REQUEST, GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { DELETE_HOST_ENDPOINT, GET_AFFECTED_MODULES_COUNT_BY_HOST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import HostEditForm from "@/components/settings/HostEditForm";
    import ConfirmationDialog from "@/components/page/ConfirmationDialog";

    export default {
        name: "HostsTable",
        props: {
            hosts: Array,
            reload: Function
        },
        components: {
            HostEditForm,
            ConfirmationDialog
        },
        data() {
            return {
                hostEditDialog: {
                    visible: false,
                    host: null,
                    form: {}
                },
                hostDeleteDialog: {
                    visible: false,
                    confirmationText: null,
                    additionalText: null,
                    host: null
                }
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            getOperationSystemIcon(osId) {
                let osList = this.getLookupValues("hostOperationSystems").filter(os => parseInt(os.value) === osId);
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
            getOperationSystemName(osId) {
                let osList = this.getLookupValues("hostOperationSystems").filter(os => parseInt(os.value) === osId);
                return osList && osList.length > 0 ? osList[0].name : "—";
            },
            getConnectionType(typeId) {
                let types = this.getLookupValues("hostConnectionTypes").filter(type => parseInt(type.value) === typeId);
                return types && types.length > 0 ? types[0].name : "—";
            },
            getCredentialsCount(row) {
                return row.credentials && row.credentials.length > 1;
            },
            openEditHostDialog(host) {
                this.hostEditDialog.host = host;
                this.hostEditDialog.form = host ? JSON.parse(JSON.stringify(host)) : {};
                this.hostEditDialog.visible = true;
            },
            openDeleteHostDialog(host) {
                let button = this.$refs[`deleteHostButton-${host.id}`];
                button.loading = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_AFFECTED_MODULES_COUNT_BY_HOST_ENDPOINT, {
                        id: host.id
                    }),
                    config: getConfiguration()
                }).then(response => {
                    button.loading = false;

                    let count = response.data;
                    this.hostDeleteDialog.host = host;
                    this.hostDeleteDialog.confirmationText = `Are you sure that you want to delete host "${host.name}"`;
                    this.hostDeleteDialog.additionalText = count > 0 ? `Count of affected modules: ${count}` : "";
                    this.hostDeleteDialog.visible = true;
                }).catch(error => {
                    button.loading = false;
                    this.$notify.error({
                        title: "Failed to get affected modules count",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            deleteHost(button) {
                button.loading = true;
                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_HOST_ENDPOINT, {
                        id: this.hostDeleteDialog.host.id
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.hostDeleteDialog.visible = false;

                    this.$notify.success({
                        title: "Host was deleted",
                        message: `Host "${this.hostDeleteDialog.host.name}" was removed`
                    });
                }).catch(error => {
                    button.loading = false;
                    this.hostDeleteDialog.visible = false;

                    this.$notify.error({
                        title: "Failed to delete host",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        }
    };
</script>