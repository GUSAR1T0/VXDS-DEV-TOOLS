<template>
    <el-table :data="items" style="width: 100%" :row-class-name="defineRowStyle" border>
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
    import { getUserFullName, getUserInitials } from "@/extensions/utils";

    export default {
        name: "OperationsTable",
        props: {
            items: Array
        },
        methods: {
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
            }
        }
    };
</script>