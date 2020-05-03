<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Operation IDs">
                    <template slot="field">
                        <el-select v-model="filter.ids" multiple filterable reserve-keyword allow-create
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Scopes">
                    <template slot="field">
                        <el-select v-model="filter.scopes" multiple filterable reserve-keyword allow-create
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
                <TableFilterItem name="Contexts">
                    <template slot="field">
                        <el-select v-model="filter.contextNames" multiple filterable reserve-keyword allow-create
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Users">
                    <template slot="field">
                        <el-select v-model="filter.userIds" multiple filterable remote reserve-keyword
                                   :remote-method="filterByUserIds" :loading="filter.userIdsSearchLoading"
                                   clearable style="width: 100%">
                            <el-option v-for="item in filter.userIdOptions" :key="item.id" :label="item.fullName"
                                       :value="item.id"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Is System Action?">
                    <template slot="field">
                        <el-radio-group v-model="filter.isSystemAction" style="width: 100%">
                            <el-radio :label="null">Not Stated</el-radio>
                            <el-radio :label="true">Yes</el-radio>
                            <el-radio :label="false">No</el-radio>
                        </el-radio-group>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
                <TableFilterItem name="Is Successful?">
                    <template slot="field">
                        <el-radio-group v-model="filter.isSuccessful" style="width: 100%">
                            <el-radio :label="null">Not Stated</el-radio>
                            <el-radio :label="true">Yes</el-radio>
                            <el-radio :label="false">No</el-radio>
                        </el-radio-group>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Start Time Range">
                    <template slot="field">
                        <el-date-picker style="width: 100%"
                                        v-model="filter.startTimeRange"
                                        type="datetimerange"
                                        range-separator="—"
                                        clearable>
                        </el-date-picker>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Stop Time Range">
                    <template slot="field">
                        <el-date-picker style="width: 100%"
                                        v-model="filter.stopTimeRange"
                                        type="datetimerange"
                                        range-separator="—"
                                        clearable>
                        </el-date-picker>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Incident Initial Time Range">
                    <template slot="field">
                        <el-date-picker style="width: 100%"
                                        v-model="filter.incidentInitialTimeRange"
                                        type="datetimerange"
                                        range-separator="—"
                                        clearable>
                        </el-date-picker>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Has Incident?">
                    <template slot="field">
                        <el-radio-group v-model="filter.hasIncident" style="width: 100%">
                            <el-radio :label="null">Not Stated</el-radio>
                            <el-radio :label="true">Yes</el-radio>
                            <el-radio :label="false">No</el-radio>
                        </el-radio-group>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
                <TableFilterItem name="Incident Statuses">
                    <template slot="field">
                        <el-select v-model="filter.incidentStatuses" multiple filterable reserve-keyword
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in getLookupValues('incidentStatuses')" :key="item.value" :label="item.name"
                                       :value="item.value"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Incident Authors">
                    <template slot="field">
                        <el-select v-model="filter.incidentAuthorIds" multiple filterable remote reserve-keyword
                                   :remote-method="filterByAuthorIds" :loading="filter.incidentAuthorIdsSearchLoading"
                                   clearable style="width: 100%">
                            <el-option v-for="item in filter.incidentAuthorIdOptions" :key="item.id"
                                       :label="item.fullName"
                                       :value="item.id"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Incident Assignees">
                    <template slot="field">
                        <el-select v-model="filter.incidentAssigneeIds" multiple filterable remote reserve-keyword
                                   :remote-method="filterByAssigneeIds" :loading="filter.incidentAssigneeIdsSearchLoading"
                                   clearable style="width: 100%">
                            <el-option v-for="item in filter.incidentAssigneeIdOptions" :key="item.id"
                                       :label="item.fullName"
                                       :value="item.id"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
    </div>
</template>

<script>
    import { mapGetters } from "vuex";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import { SEARCH_USERS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import { UNAUTHORIZED, UNASSIGNED } from "@/constants/formatPattern";

    import Blocks from "@/components/page/Blocks";
    import TableFilterItem from "@/components/table-filter/TableFilterItem";

    export default {
        name: "OperationsTableFilters",
        props: {
            filter: Object
        },
        components: {
            Blocks,
            TableFilterItem
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            filterByUserIds(query) {
                if (query !== "") {
                    this.filter.userIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_USERS_ENDPOINT, {
                            pattern: query,
                            zeroUserName: UNAUTHORIZED
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.filter.userIdsSearchLoading = false;
                        this.filter.userIdOptions = response.data;
                    }).catch(error => {
                        this.filter.userIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of users",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.filter.userIdOptions = [];
                }
            },
            filterByAuthorIds(query) {
                if (query !== "") {
                    this.filter.incidentAuthorIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_USERS_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.filter.incidentAuthorIdsSearchLoading = false;
                        this.filter.incidentAuthorIdOptions = response.data;
                    }).catch(error => {
                        this.filter.incidentAuthorIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of authors",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.filter.incidentAuthorIdOptions = [];
                }
            },
            filterByAssigneeIds(query) {
                if (query !== "") {
                    this.filter.incidentAssigneeIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_USERS_ENDPOINT, {
                            pattern: query,
                            zeroUserName: UNASSIGNED
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.filter.incidentAssigneeIdsSearchLoading = false;
                        this.filter.incidentAssigneeIdOptions = response.data;
                    }).catch(error => {
                        this.filter.incidentAssigneeIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of assignees",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.filter.incidentAssigneeIdOptions = [];
                }
            }
        }
    };
</script>