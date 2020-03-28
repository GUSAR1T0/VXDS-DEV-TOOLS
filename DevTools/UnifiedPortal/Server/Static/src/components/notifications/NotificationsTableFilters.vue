<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Notification IDs">
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
                <TableFilterItem name="Notification Levels">
                    <template slot="field">
                        <el-select v-model="filter.levels" multiple filterable reserve-keyword
                                   default-first-option style="width: 100%">
                            <el-option v-for="item in getLookupValues('notificationLevels')" :key="item.value" :label="item.name"
                                       :value="item.value"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
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
        </Blocks>
        <Blocks style="padding-bottom: 20px">
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
    </div>
</template>

<script>
    import { mapGetters } from "vuex";

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
        }
    };
</script>