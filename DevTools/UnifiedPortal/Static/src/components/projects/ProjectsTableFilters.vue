<template>
    <div>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="Project IDs">
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
                <TableFilterItem name="Project Names">
                    <template slot="field">
                        <el-select v-model="filter.names" multiple filterable remote reserve-keyword
                                   @change="filter.nameGenerator += 1" :remote-method="filterByProjectNames"
                                   style="width: 100%">
                            <el-option v-for="item in filter.nameOptions" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="third">
                <TableFilterItem name="Project Aliases">
                    <template slot="field">
                        <el-select v-model="filter.aliases" multiple filterable remote reserve-keyword
                                   @change="filter.aliasGenerator += 1" :remote-method="filterByProjectAliases"
                                   style="width: 100%">
                            <el-option v-for="item in filter.aliasOptions" :key="item.id" :label="item.query"
                                       :value="item.query"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
        </Blocks>
        <Blocks style="padding-bottom: 20px">
            <template slot="first">
                <TableFilterItem name="GitHub Repositories">
                    <template slot="field">
                        <el-select v-model="filter.gitHubRepoIds" multiple filterable remote reserve-keyword
                                   :remote-method="filterByGitHubRepoIds" style="width: 100%">
                            <el-option v-for="item in filter.gitHubRepoIdOptions" :key="item.id" :label="item.fullName"
                                       :value="item.id"/>
                        </el-select>
                    </template>
                </TableFilterItem>
            </template>
            <template slot="second">
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
    import { GET_HTTP_REQUEST } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";
    import { SEARCH_GITHUB_REPOSITORIES_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";

    import Blocks from "@/components/page/Blocks";
    import TableFilterItem from "@/components/table-filter/TableFilterItem";

    export default {
        name: "ProjectsTableFilters",
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
            filterByProjectNames(query) {
                this.filter.nameOptions = query !== "" ? [ {
                    id: this.filter.nameGenerator,
                    query: query
                } ] : [];
            },
            filterByProjectAliases(query) {
                this.filter.aliasOptions = query !== "" ? [ {
                    id: this.filter.aliasGenerator,
                    query: query
                } ] : [];
            },
            filterByGitHubRepoIds(query) {
                if (query !== "") {
                    this.filter.gitHubRepoIdsSearchLoading = true;
                    this.$store.dispatch(GET_HTTP_REQUEST, {
                        server: LOCALHOST,
                        endpoint: format(SEARCH_GITHUB_REPOSITORIES_ENDPOINT, {
                            pattern: query
                        }),
                        config: getConfiguration()
                    }).then(response => {
                        this.filter.gitHubRepoIdsSearchLoading = false;
                        this.filter.gitHubRepoIdOptions = response.data;
                    }).catch(error => {
                        this.filter.gitHubRepoIdsSearchLoading = false;
                        this.$notify.error({
                            title: "Failed to load list of GitHub repositories",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                } else {
                    this.filter.gitHubRepoIdOptions = [];
                }
            }
        }
    };
</script>