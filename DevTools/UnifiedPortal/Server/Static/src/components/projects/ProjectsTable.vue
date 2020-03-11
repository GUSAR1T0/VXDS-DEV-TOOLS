<template>
    <el-table :data="projects" style="width: 100%" border highlight-current-row>
        <el-table-column label="Project ID" min-width="100" align="center">
            <template slot-scope="scope">
                <strong style="font-size: 16px">{{ scope.row.id }}</strong>
            </template>
        </el-table-column>
        <el-table-column label="Project" min-width="750" align="center">
            <template slot-scope="scope">
                <div style="text-align: left">
                    <el-link :href="`/pages/project/${scope.row.id}`" type="primary" :underline="false">
                        <div style="font-size: 22px; padding-bottom: 5px">
                            <strong>{{ scope.row.name }} ({{ scope.row.alias }})</strong>
                        </div>
                        <div style="font-size: 16px">
                            <strong>{{ scope.row.description }}</strong>
                        </div>
                    </el-link>
                </div>
            </template>
        </el-table-column>
        <el-table-column label="Code Services" min-width="450" align="center">
            <template slot-scope="scope">
                <div v-if="scope.row.gitHubRepository">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            <div style="display: flex; align-items: center; font-size: 14px">
                                <div>Owner:</div>
                                <el-link type="primary" :href="scope.row.gitHubRepository.owner.profileUrl"
                                         target="_blank" :underline="false" class="github-owner-link">
                                    <div class="user-avatar-and-name">
                                        <img :src="scope.row.gitHubRepository.owner.avatarUrl" alt="avatar"
                                             class="avatar"/>
                                        <div style="margin-left: 10px">
                                            <strong>{{ scope.row.gitHubRepository.owner.login }}</strong>
                                        </div>
                                    </div>
                                </el-link>
                            </div>
                        </div>
                        <div style="display: flex; align-items: center; justify-content: center">
                            <div style="font-size: 16px">
                                <strong>GitHub:</strong>
                            </div>
                            <div>
                                <el-link :href="scope.row.gitHubRepository.repositoryUrl" target="_blank"
                                         type="primary" :underline="false" style="margin-left: 5px">
                                    <strong style="font-size: 16px">
                                        {{ scope.row.gitHubRepository.fullName }}
                                    </strong>
                                </el-link>
                            </div>
                            <div style="margin-left: 5px; margin-bottom: 15px">
                                    <span :class="getPrivateStatusIconColor(scope.row)" style="font-size: 8px">
                                        <fa :icon="getPrivateStatusIcon(scope.row)"/>
                                    </span>
                            </div>
                        </div>
                    </el-tooltip>
                </div>
                <div v-else>—</div>
            </template>
        </el-table-column>
        <el-table-column label="Project Status" min-width="200" align="center">
            <template slot-scope="scope">
                <strong style="font-size: 16px">{{ scope.row.isActive ? "Active" : "Inactive" }}</strong>
            </template>
        </el-table-column>
    </el-table>
</template>

<style scoped src="@/styles/user.css">
</style>

<style scoped>
    .avatar {
        width: 32px;
        height: 32px;
        border-radius: 5px;
    }

    .is-private {
        color: #F56C6C;
    }

    .is-public {
        color: #0C7C59;
    }

    .github-owner-link {
        margin-left: 10px;
    }

    .github-owner-link:not(:hover) {
        color: #FFF;
    }
</style>

<script>
    export default {
        name: "ProjectsTable",
        props: {
            projects: Array
        },
        methods: {
            getPrivateStatusIconColor(row) {
                return row.gitHubRepository.private ? "is-private" : "is-public";
            },
            getPrivateStatusIcon(row) {
                return row.gitHubRepository.private ? "lock" : "lock-open";
            }
        }
    };
</script>