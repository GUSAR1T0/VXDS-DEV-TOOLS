<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Host IDs">
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
                <TableFilterItem name="Host Names">
                    <template slot="field">
                        <el-select v-model="filter.names" multiple filterable reserve-keyword allow-create
                                   default-first-option clearable  style="width: 100%">
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
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Operating Systems">
                    <template slot="field">
                        <el-select v-model="filter.operatingSystems" multiple filterable reserve-keyword
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in getLookupValues('hostOperatingSystems')" :key="item.value"
                                       :label="item.name" :value="item.value">
                                <div style="display: flex; font-size: 14px">
                                    <div style="margin-right: 5px">
                                        <fa :icon="['fab', getOperatingSystemIcon(item.value)]"/>
                                    </div>
                                    {{ item.name }}
                                </div>
                            </el-option>
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
            getOperatingSystemIcon(osId) {
                let osList = this.getLookupValues("hostOperatingSystems").filter(os => parseInt(os.value) === parseInt(osId));
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
            }
        }
    };
</script>