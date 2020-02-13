<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="User IDs">
                    <template slot="field">
                        <el-select v-model="filter.ids" multiple filterable remote reserve-keyword
                                   @change="filter.idGenerator += 1" :remote-method="filterByIds"
                                   style="width: 100%">
                            <el-option v-for="item in filter.idOptions" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="User Names">
                    <template slot="field">
                        <el-select v-model="filter.userNames" multiple filterable remote reserve-keyword
                                   @change="filter.userNameGenerator += 1" :remote-method="filterByUserNames"
                                   style="width: 100%">
                            <el-option v-for="item in filter.userNameOptions" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
                <TableFilterItem name="Emails">
                    <template slot="field">
                        <el-select v-model="filter.emails" multiple filterable remote reserve-keyword
                                   @change="filter.emailGenerator += 1" :remote-method="filterByEmails"
                                   style="width: 100%">
                            <el-option v-for="item in filter.emailOptions" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="User Roles">
                    <template slot="field">
                        <el-select v-model="filter.userRoleIds" multiple filterable remote reserve-keyword
                                   :remote-method="filterByUserRoleIds" style="width: 100%">
                            <el-option v-for="item in filter.userRoleIdOptions" :key="item.id" :label="item.name"
                                       :value="item.id"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
                <TableFilterItem name="Is Activated?">
                    <template slot="field">
                        <el-radio-group v-model="filter.isActivated" style="width: 100%">
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
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import { SEARCH_USER_ROLES_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import Blocks from "@/components/page/Blocks";
    import TableFilterItem from "@/components/table-filter/TableFilterItem";

    export default {
        name: "UsersTableFilters",
        props: {
            filter: Object
        },
        components: {
            Blocks,
            TableFilterItem
        },
        methods: {
            filterByIds(query) {
                this.filter.idOptions = query !== "" ? [ {
                    id: this.filter.idGenerator,
                    query: parseInt(query.replace(/\D/g, ""))
                } ] : [];
            },
            filterByUserNames(query) {
                this.filter.userNameOptions = query !== "" ? [ {
                    id: this.filter.userNameGenerator,
                    query: query
                } ] : [];
            },
            filterByEmails(query) {
                this.filter.emailOptions = query !== "" ? [ {
                    id: this.filter.emailGenerator,
                    query: query
                } ] : [];
            },
            filterByUserRoleIds(query) {
                if (query !== "") {
                    this.filter.userRoleIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_USER_ROLES_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.filter.userRoleIdsSearchLoading = false;
                        this.filter.userRoleIdOptions = response.data;
                    }).catch(error => {
                        this.filter.userRoleIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of user roles",
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