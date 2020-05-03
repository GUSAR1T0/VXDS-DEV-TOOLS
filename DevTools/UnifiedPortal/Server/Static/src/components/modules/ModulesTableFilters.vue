<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Module IDs">
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
                <TableFilterItem name="Module Names">
                    <template slot="field">
                        <el-select v-model="filter.names" multiple filterable reserve-keyword allow-create
                                   default-first-option clearable style="width: 100%">
                            <el-option v-for="item in []" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
                <TableFilterItem name="Module Aliases">
                    <template slot="field">
                        <el-select v-model="filter.aliases" multiple filterable reserve-keyword allow-create
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
                <TableFilterItem name="Hosts">
                    <template slot="field">
                        <el-select v-model="filter.hostIds" multiple filterable remote reserve-keyword
                                   :remote-method="filterByHostIds" :loading="filter.hostIdsSearchLoading"
                                   clearable style="width: 100%">
                            <el-option v-for="item in filter.hostIdOptions" :key="item.id"
                                       :label="`${item.name} (${item.domain})`" :value="item.id">
                                <div style="display: flex; font-size: 14px">
                                    <div style="margin-right: 5px">
                                        <fa :icon="['fab', getOperatingSystemIcon(item.operatingSystem)]"/>
                                    </div>
                                    {{ item.name }} ({{ item.domain }})
                                </div>
                            </el-option>
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
    </div>
</template>

<script>
    import { mapGetters } from "vuex";
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import { SEARCH_HOSTS_ENDPOINT, SEARCH_USERS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import Blocks from "@/components/page/Blocks";
    import TableFilterItem from "@/components/table-filter/TableFilterItem";

    export default {
        name: "ModulesTableFilters",
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
            },
            filterByHostIds(query) {
                if (query !== "") {
                    this.filter.hostIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_HOSTS_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.filter.hostIdsSearchLoading = false;
                        this.filter.hostIdOptions = response.data;
                    }).catch(error => {
                        this.filter.hostIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of hosts",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.filter.hostIdOptions = [];
                }
            },
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