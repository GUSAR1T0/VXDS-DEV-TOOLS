<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <div class="hosts">
                <ProfileBlock icon="database" header="Hosts">
                    <template slot="profile-block-buttons">
                        <el-tooltip effect="dark" placement="top">
                            <div slot="content">
                                Add New Host
                            </div>
                            <el-button type="primary" circle class="rounded-button"
                                       @click="hostEditDialog.visible = true">
                                <span><fa icon="plus-circle"/></span>
                            </el-button>
                        </el-tooltip>
                    </template>
                    <template slot="profile-block-content">
                        <ProfileBlock icon="filter" header="Filters">
                            <template slot="profile-block-content">
                                <HostsTableFilters :filter="filter"/>
                                <el-row type="flex" justify="center" align="middle" :gutter="20" style="margin: 20px 0">
                                    <el-col :span="12">
                                        <el-button type="primary" style="width: 100%" @click="applyFiltersComplex">
                                            <strong>Apply</strong>
                                        </el-button>
                                    </el-col>
                                    <el-col :span="12">
                                        <el-button style="width: 100%" @click="resetFiltersComplex">
                                            <strong>Reset</strong>
                                        </el-button>
                                    </el-col>
                                </el-row>
                            </template>
                        </ProfileBlock>
                        <ProfileBlock icon="table" header="Table">
                            <template slot="profile-block-content">
                                <HostsTable :hosts="hosts" :reload="loadSettings"/>
                                <el-pagination style="width: 100%; margin-top: 20px; text-align: center"
                                               @size-change="changePageSize"
                                               @current-change="changePageNo"
                                               :current-page.sync="settings.pageNo"
                                               :page-sizes="[10, 25, 50, 100, 250, 500]"
                                               :page-size="settings.pageSize"
                                               :total="settings.total"
                                               layout="total, sizes, prev, pager, next"
                                />
                            </template>
                        </ProfileBlock>
                    </template>
                </ProfileBlock>

                <HostEditForm :page-status="hostEditDialog" :closed="closeDialog"/>
            </div>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/profile.css">
</style>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { LOAD_HOST_SETTINGS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, getOnlyNumbers, renderErrorNotificationMessage } from "@/extensions/utils";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import ProfileBlock from "@/components/page/ProfileBlock";
    import HostsTable from "@/components/hosts/HostsTable";
    import HostsTableFilters from "@/components/hosts/HostsTableFilters";
    import HostEditForm from "@/components/hosts/HostEditForm";

    export default {
        name: "Environment",
        components: {
            LoadingContainer,
            ProfileBlock,
            HostsTable,
            HostsTableFilters,
            HostEditForm
        },
        data() {
            return {
                loadingIsActive: false,
                settings: {
                    pageNo: 1,
                    pageSize: 10,
                    total: 0
                },
                filter: {
                    ids: [],
                    names: [],
                    domains: [],
                    operatingSystems: []
                },
                hosts: [],
                hostEditDialog: {
                    visible: false,
                    host: {},
                    form: {
                        credentials: []
                    }
                }
            };
        },
        methods: {
            loadSettings() {
                this.loadingIsActive = true;
                let request = {
                    ids: getOnlyNumbers(this.filter.ids),
                    names: this.filter.names,
                    domains: this.filter.domains,
                    operatingSystems: getOnlyNumbers(this.filter.operatingSystems)
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: LOAD_HOST_SETTINGS_ENDPOINT,
                    data: {
                        pageNo: this.settings.pageNo - 1,
                        pageSize: this.settings.pageSize,
                        filter: request
                    },
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.settings.total = response.data.total;
                    this.hosts = response.data.items;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load hosts settings",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            resetFilters() {
                this.filter.ids = [];
                this.filter.names = [];
                this.filter.domains = [];
                this.filter.operatingSystems = [];
            },
            applyFiltersComplex() {
                this.settings.pageNo = 1;
                this.loadSettings();
            },
            resetFiltersComplex() {
                this.settings.pageNo = 1;
                this.resetFilters();
                this.loadSettings();
            },
            changePageSize(value) {
                this.settings.pageSize = value;
                this.settings.pageNo = 1;
                this.loadSettings();
            },
            changePageNo(value) {
                this.settings.pageNo = value;
                this.loadSettings();
            },
            closeDialog() {
                this.hostEditDialog.host = {};
                this.hostEditDialog.form = {
                    credentials: []
                };
                this.loadSettings();
            },
            searchHostsById(id) {
                if (id && !isNaN(id)) {
                    id = parseInt(id);
                    this.filter.ids = [ id ];
                }
            }
        },
        mounted() {
            this.searchHostsById(this.$route.query.hostId);
            this.loadSettings();
        },
        beforeRouteUpdate(to, from, next) {
            this.searchHostsById(to.query.hostId);
            this.loadSettings();
            next();
        }
    };
</script>