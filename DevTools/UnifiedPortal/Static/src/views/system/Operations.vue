<template>
    <div class="operations">
        <LoadingContainer :loading-state="loadingIsActive">
            <template slot="content">
                <Blocks style="padding-bottom: 20px">
                    <template slot="first">
                        <TableFilterItem name="Operation IDs">
                            <template slot="field">
                                <el-select v-model="filter.ids" multiple filterable remote reserve-keyword
                                           @change="idGenerator += 1" :remote-method="filterByIds"
                                           style="width: 100%">
                                    <el-option v-for="item in idOptions" :key="item.id" :label="item.query"
                                               :value="item.id"/>
                                </el-select>
                            </template>
                        </TableFilterItem>
                    </template>
                    <template slot="second">
                        <TableFilterItem name="Scopes">
                            <template slot="field">
                                <el-select v-model="filter.scopes" multiple filterable remote reserve-keyword
                                           @change="scopeGenerator += 1" :remote-method="filterByScopes"
                                           style="width: 100%">
                                    <el-option v-for="item in scopeOptions" :key="item.id" :label="item.query"
                                               :value="item.query"/>
                                </el-select>
                            </template>
                        </TableFilterItem>
                    </template>
                    <template slot="third">
                        <TableFilterItem name="Contexts">
                            <template slot="field">
                                <el-select v-model="filter.contextNames" multiple filterable remote reserve-keyword
                                           @change="contextNameGenerator += 1" :remote-method="filterByContextNames"
                                           style="width: 100%">
                                    <el-option v-for="item in contextNameOptions" :key="item.id" :label="item.query"
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
                                           :remote-method="filterByUserIds" style="width: 100%">
                                    <el-option v-for="item in userIdOptions" :key="item.id" :label="item.fullName"
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
                <Blocks style="padding-bottom: 10px">
                    <template slot="first">
                        <TableFilterItem name="Start Time Range">
                            <template slot="field">
                                <el-date-picker style="width: 100%"
                                                v-model="filter.startTimeRange"
                                                type="datetimerange"
                                                range-separator="—">
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
                                                range-separator="—">
                                </el-date-picker>
                            </template>
                        </TableFilterItem>
                    </template>
                </Blocks>
                <el-row type="flex" justify="center" align="middle" :gutter="20" style="margin: 20px 0">
                    <el-col :span="12">
                        <el-button type="primary" style="width: 100%" @click="applyFilters">
                            <strong>Apply</strong>
                        </el-button>
                    </el-col>
                    <el-col :span="12">
                        <el-button style="width: 100%" @click="resetFilters">
                            <strong>Reset</strong>
                        </el-button>
                    </el-col>
                </el-row>
                <el-table :data="model.items" style="width: 100%" :row-class-name="defineRowStyle" border>
                    <el-table-column type="expand">
                        <template slot-scope="scope">
                            <el-table :data="scope.row.logs" style="width: 100%" border>
                                <el-table-column label="Date / Time" min-width="300">
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
                    <el-table-column label="ID / Scope / Context" min-width="900">
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
                    <el-table-column label="User" min-width="300">
                        <template slot-scope="scope">
                            <el-link :href="`/user/${scope.row.userId}`" type="primary" :underline="false"
                                     v-if="scope.row.userId">
                                <div class="user-avatar-and-name">
                                    <el-avatar :size="32" :style="getAvatarStyle(scope.row)">
                                        <div style="font-size: 16px">
                                            {{ getInitials(scope.row) }}
                                        </div>
                                    </el-avatar>
                                    <strong style="font-size: 16px">
                                        {{ getFullName(scope.row) }}
                                    </strong>
                                </div>
                            </el-link>
                            <div v-else>
                                <div class="user-avatar-and-name">
                                    <el-avatar :size="32" :style="getAvatarStyle(scope.row)">
                                        <div style="font-size: 16px">
                                            {{ getInitials(scope.row) }}
                                        </div>
                                    </el-avatar>
                                    <strong style="font-size: 16px">
                                        {{ getFullName(scope.row) }}
                                    </strong>
                                </div>
                            </div>
                        </template>
                    </el-table-column>
                    <el-table-column label="Date" min-width="400">
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
                <el-pagination style="width: 100%; margin-top: 20px"
                               @size-change="changePageSize"
                               @current-change="changePageNo"
                               :current-page.sync="pageNo"
                               :page-sizes="[10, 25, 50, 100, 250, 500]"
                               :page-size="pageSize"
                               :total="model.total"
                               layout="total, sizes, prev, pager, next"
                />
            </template>
        </LoadingContainer>
    </div>
</template>

<style scoped src="@/styles/user.css">
</style>

<style>
    .el-table .is-successful {
        background: rgba(12, 124, 89, 0.1);
    }

    .el-table .is-failed {
        background: rgba(219, 43, 61, 0.1);
    }
</style>

<script>
    import { GET_HTTP_REQUEST, POST_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_OPERATION_LIST_ENDPOINT, SEARCH_USERS_ENDPOINT } from "@/constants/endpoints";
    import {
        getConfiguration,
        getUserFullName,
        getUserInitials,
        renderErrorNotificationMessage
    } from "@/extensions/utils";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer";
    import Blocks from "@/components/page/Blocks";
    import TableFilterItem from "@/components/table-filter/TableFilterItem";

    export default {
        name: "Operations",
        components: {
            LoadingContainer,
            Blocks,
            TableFilterItem
        },
        data() {
            return {
                loadingIsActive: true,
                pageNo: 1,
                pageSize: 10,
                filter: {
                    ids: [],
                    scopes: [],
                    contextNames: [],
                    userIds: [],
                    isSystemAction: null,
                    isSuccessful: null,
                    startTimeRange: [],
                    stopTimeRange: []
                },
                idOptions: [],
                idGenerator: 0,
                scopeOptions: [],
                scopeGenerator: 0,
                contextNameOptions: [],
                contextNameGenerator: 0,
                userIdOptions: [],
                userIdsSearchLoading: false,
                model: {
                    total: 0,
                    items: []
                }
            };
        },
        methods: {
            getDate(date) {
                let transformer = (value) => ("0" + value).slice(-2);
                if (date) {
                    let year = date.getFullYear();
                    let month = date.getMonth() + 1;
                    let day = date.getDate();
                    let hour = date.getHours();
                    let minute = date.getMinutes();
                    let second = date.getSeconds();
                    return `${year}-${transformer(month)}-${transformer(day)}T${transformer(hour)}:${transformer(minute)}:${transformer(second)}`;
                } else {
                    return null;
                }
            },
            loadOperations() {
                this.loadingIsActive = true;
                let request = {
                    ids: this.filter.ids,
                    scopes: this.filter.scopes,
                    contextNames: this.filter.contextNames,
                    userIds: this.filter.userIds,
                    isSystemAction: this.filter.isSystemAction,
                    isSuccessful: this.filter.isSuccessful,
                    startTimeRange: this.filter.startTimeRange && this.filter.startTimeRange.length > 1 ? {
                        min: this.getDate(this.filter.startTimeRange[0]),
                        max: this.getDate(this.filter.startTimeRange[1])
                    } : null,
                    stopTimeRange: this.filter.stopTimeRange && this.filter.stopTimeRange.length > 1 ? {
                        min: this.getDate(this.filter.stopTimeRange[0]),
                        max: this.getDate(this.filter.stopTimeRange[1])
                    } : null
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_OPERATION_LIST_ENDPOINT,
                    data: {
                        pageNo: this.pageNo - 1,
                        pageSize: this.pageSize,
                        filter: request
                    },
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.model.total = response.data.total;
                    this.model.items = response.data.items;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load list of operations",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            defineRowStyle({row}) {
                if (row.isSuccessful === true) {
                    return "is-successful";
                } else if (row.isSuccessful === false) {
                    return "is-failed";
                } else {
                    return "";
                }
            },
            getInitials(row) {
                return getUserInitials(row.firstName, row.lastName);
            },
            getFullName(row) {
                return getUserFullName(row.firstName, row.lastName);
            },
            getAvatarStyle(row) {
                return {
                    backgroundColor: row.color ? row.color : "#909399",
                    marginRight: "10px"
                };
            },
            changePageSize(value) {
                this.pageSize = value;
                this.pageNo = 1;
                this.loadOperations();
            },
            changePageNo(value) {
                this.pageNo = value;
                this.loadOperations();
            },
            filterByIds(query) {
                this.idOptions = query !== "" ? [ {
                    id: this.idGenerator,
                    query: query.replace(/\D/g, "")
                } ] : [];
            },
            filterByScopes(query) {
                this.scopeOptions = query !== "" ? [ {
                    id: this.scopeGenerator,
                    query: query
                } ] : [];
            },
            filterByContextNames(query) {
                this.contextNameOptions = query !== "" ? [ {
                    id: this.contextNameGenerator,
                    query: query
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
            applyFilters() {
                this.pageNo = 1;
                this.pageSize = 10;
                this.loadOperations();
            },
            resetFilters() {
                this.pageNo = 1;
                this.pageSize = 10;
                this.filter = {
                    ids: [],
                    scopes: [],
                    contextNames: [],
                    userIds: [],
                    isSystemAction: null,
                    isSuccessful: null,
                    startTimeRange: [],
                    stopTimeRange: []
                };
                this.idOptions = [];
                this.idGenerator = 0;
                this.scopeOptions = [];
                this.scopeGenerator = 0;
                this.contextNameOptions = [];
                this.contextNameGenerator = 0;
                this.userIdOptions = [];
                this.loadOperations();
            }
        },
        mounted() {
            // this.filters <= [this.$route.query.id];
            this.loadOperations();
        },
        beforeRouteUpdate(to, from, next) {
            // this.filters <= [to.query.id];
            this.loadOperations();
            next();
        }
    };
</script>