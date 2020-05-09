<template>
    <el-dialog :visible.sync="pageStatus.visible" width="75%" style="text-align: center" @closed="close"
               @open="prepareDialog" :append-to-body="!pageStatus.hostId">
        <span slot="title" class="modal-title">
            Check Connections to Host "{{ pageStatus.hostName }}" 
        </span>
        <el-table :data="checks" style="width: 100%" border v-loading="loadingIsActive" default-expand-all>
            <el-table-column type="expand">
                <template slot-scope="scope">
                    <ProfileBlock :header="scope.row.result.hasErrors ? 'Unsuccessful result' : 'Successful result'"
                                  :icon="scope.row.result.hasErrors ? 'times-circle' : 'check-circle'"
                    >
                        <template slot="profile-block-content">
                            <Row name="Command" :value="scope.row.result.command"/>
                            <Row name="Exit Status" :value="scope.row.result.exitStatus"/>
                            <Row name="Output">
                                <template slot="description">
                                    <code v-if="scope.row.result.output" class="result">{{
                                        scope.row.result.output
                                    }}</code>
                                    <div v-else style="font-size: 20px">—</div>
                                </template>
                            </Row>
                            <Row name="Error">
                                <template slot="description">
                                    <code v-if="scope.row.result.error" class="result">
                                        {{ scope.row.result.error }}
                                    </code>
                                    <div v-else style="font-size: 20px">—</div>
                                </template>
                            </Row>
                        </template>
                    </ProfileBlock>
                </template>
            </el-table-column>
            <el-table-column label="Connection Type" align="center">
                <template slot-scope="scope">
                    <strong style="font-size: 16px">
                        {{ getConnectionType(scope.row.item.type) }}
                    </strong>
                </template>
            </el-table-column>
            <el-table-column label="Port" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 16px">
                        <strong v-if="scope.row.item.port">
                            {{ scope.row.item.port }}
                        </strong>
                        <strong v-else>—</strong>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="Username" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 16px">
                        <strong v-if="scope.row.item.username">
                            {{ scope.row.item.username }}
                        </strong>
                        <strong v-else>—</strong>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="Password" align="center">
                <template slot-scope="scope">
                    <div style="font-size: 16px">
                        <strong v-if="scope.row.item.password">
                            {{ scope.row.item.password }}
                        </strong>
                        <strong v-else>—</strong>
                    </div>
                </template>
            </el-table-column>
        </el-table>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<style scoped>
    .result {
        font-family: monospace;
        white-space: pre-wrap;
        font-size: 16px;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { GET_HTTP_REQUEST, POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { CHECK_CONNECTION_TO_HOST_ENDPOINT, CHECK_CONNECTIONS_TO_HOST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import Row from "@/components/page/Row";
    import ProfileBlock from "@/components/page/ProfileBlock";

    export default {
        name: "CheckConnectionsDialog",
        props: {
            pageStatus: Object,
            closed: Function
        },
        components: {
            Row,
            ProfileBlock
        },
        data() {
            return {
                loadingIsActive: false,
                checks: []
            };
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            prepareDialog() {
                this.loadingIsActive = true;

                if (!this.pageStatus.hostId) {
                    this.$store.dispatch(POST_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: CHECK_CONNECTION_TO_HOST_ENDPOINT,
                        data: {
                            operatingSystem: this.pageStatus.hostOperatingSystem,
                            host: this.pageStatus.hostDomain,
                            type: this.pageStatus.hostCredentials.type,
                            port: this.pageStatus.hostCredentials.port,
                            username: this.pageStatus.hostCredentials.username,
                            password: this.pageStatus.hostCredentials.password
                        },
                        config: getConfiguration()
                    }).then(response => {
                        this.loadingIsActive = false;
                        this.checks = [ response.data ];
                    }).catch(error => {
                        this.loadingIsActive = false;
                        this.$notify.error({
                            title: "Failed to get data after check connection",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(CHECK_CONNECTIONS_TO_HOST_ENDPOINT, {
                            id: this.pageStatus.hostId
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.loadingIsActive = false;
                        this.checks = response.data;
                    }).catch(error => {
                        this.loadingIsActive = false;
                        this.$notify.error({
                            title: "Failed to get data after check connections",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                }
            },
            getConnectionType(typeId) {
                let types = this.getLookupValues("hostConnectionTypes").filter(type => parseInt(type.value) === typeId);
                return types && types.length > 0 ? types[0].name : "—";
            },
            close() {
                this.checks = [];
                if (this.closed) {
                    this.closed();
                }
            }
        }
    };
</script>