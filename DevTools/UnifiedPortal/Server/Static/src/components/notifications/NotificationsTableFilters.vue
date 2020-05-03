<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Notification IDs">
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
                <TableFilterItem name="Notification Levels">
                    <template slot="field">
                        <el-select v-model="filter.levels" multiple filterable reserve-keyword
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in getLookupValues('notificationLevels')" :key="item.value" :label="item.name"
                                       :value="item.value"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
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
        </Blocks>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Is Active?">
                    <template slot="field">
                        <el-radio-group v-model="filter.isActive" style="width: 100%">
                            <el-radio :label="null">Not Stated</el-radio>
                            <el-radio :label="true">Yes</el-radio>
                            <el-radio :label="false">No</el-radio>
                        </el-radio-group>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
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
            <template slot="third">
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
    </div>
</template>

<script>
    import { mapGetters } from "vuex";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import { SEARCH_USERS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import Blocks from "@/components/page/Blocks";
    import TableFilterItem from "@/components/table-filter/TableFilterItem";

    export default {
        name: "NotificationsTableFilters",
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
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.filter.userIdOptions = [];
                }
            }
        }
    };
</script>