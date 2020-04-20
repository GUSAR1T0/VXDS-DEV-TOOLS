<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Note IDs">
                    <template slot="field">
                        <el-select v-model="filter.ids" multiple filterable reserve-keyword allow-create
                                   default-first-option style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Note Titles">
                    <template slot="field">
                        <el-select v-model="filter.titles" multiple filterable reserve-keyword allow-create
                                   default-first-option style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
                <TableFilterItem name="Note Projects">
                    <template slot="field">
                        <el-select v-model="filter.projectIds" multiple filterable remote reserve-keyword
                                   :remote-method="filterByProjectIds" style="width: 100%">
                            <el-option v-for="item in filter.projectIdOptions" :key="item.id"
                                       :label="`${item.name} (${item.alias})`" :value="item.id"/>
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
                            <el-option v-for="item in filter.userIdOptions" :key="item.id" :label="item.fullName"
                                       :value="item.id"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Edit Time Range">
                    <template slot="field">
                        <el-date-picker style="width: 100%"
                                        v-model="filter.editTimeRange"
                                        type="datetimerange"
                                        range-separator="—">
                        </el-date-picker>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
    </div>
</template>

<script>
    import { mapGetters } from "vuex";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { UNIFIED_PORTAL } from "@/constants/servers";
    import { SEARCH_USERS_ENDPOINT, SEARCH_PROJECTS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import Blocks from "@/components/page/Blocks";
    import TableFilterItem from "@/components/table-filter/TableFilterItem";

    export default {
        name: "NotesTableFilters",
        props: {
            filter: Object
        },
        components: {
            Blocks,
            TableFilterItem
        },
        computed: {
            ...mapGetters([
                "getUnifiedPortalHost"
            ])
        },
        methods: {
            filterByUserIds(query) {
                if (query !== "") {
                    this.filter.userIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: UNIFIED_PORTAL,
                        endpoint: format(SEARCH_USERS_ENDPOINT, {
                            pattern: query
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
                            message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                        });
                    });
                } else {
                    this.filter.userIdOptions = [];
                }
            },
            filterByProjectIds(query) {
                if (query !== "") {
                    this.filter.projectIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: UNIFIED_PORTAL,
                        endpoint: format(SEARCH_PROJECTS_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.filter.projectIdsSearchLoading = false;
                        this.filter.projectIdOptions = response.data;
                    }).catch(error => {
                        this.filter.projectIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of projects",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                        });
                    });
                } else {
                    this.filter.projectIdOptions = [];
                }
            }
        }
    };
</script>