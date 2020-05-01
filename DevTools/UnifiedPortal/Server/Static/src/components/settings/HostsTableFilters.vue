<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Host IDs">
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
                <TableFilterItem name="Host Names">
                    <template slot="field">
                        <el-select v-model="filter.names" multiple filterable reserve-keyword allow-create
                                   default-first-option style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Host Domains">
                    <template slot="field">
                        <el-select v-model="filter.domains" multiple filterable reserve-keyword allow-create
                                   default-first-option style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Operation Systems">
                    <template slot="field">
                        <el-select v-model="filter.operationSystems" multiple filterable reserve-keyword
                                   default-first-option style="width: 100%">
                            <el-option v-for="item in getLookupValues('hostOperationSystems')" :key="item.value" :label="item.name"
                                       :value="item.value"/>
                        </el-select>
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
        name: "HostsTableFilters",
        props: {
            filter: Object
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        components: {
            Blocks,
            TableFilterItem
        },
    };
</script>