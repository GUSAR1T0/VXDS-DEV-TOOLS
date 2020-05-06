<template>
    <el-table :data="modules" style="width: 100%" border highlight-current-row>
        <el-table-column label="Module ID" min-width="100" align="center">
            <template slot-scope="scope">
                <strong style="font-size: 16px">{{ scope.row.id }}</strong>
            </template>
        </el-table-column>
        <el-table-column label="Module" min-width="500" align="center">
            <template slot-scope="scope">
                <div style="text-align: left">
                    <el-link :href="`/components/module/${scope.row.id}`" type="primary" :underline="false">
                        <div style="font-size: 22px">
                            <strong>{{ scope.row.name }} ({{ scope.row.alias }})</strong>
                        </div>
                        <div style="font-size: 16px">
                            ver. <strong>{{ scope.row.version }}</strong>
                        </div>
                    </el-link>
                </div>
            </template>
        </el-table-column>
        <el-table-column label="Responsible User" min-width="300" align="center">
            <template slot-scope="scope">
                <UserAvatarAndFullNameWithLink
                        style="text-align: left"
                        :first-name="scope.row.firstName"
                        :last-name="scope.row.lastName"
                        :color="scope.row.color"
                        :user-id="scope.row.userId"
                />
            </template>
        </el-table-column>
        <el-table-column label="Host" min-width="300" align="center">
            <template slot-scope="scope">
                <HostFullNameWithLink
                        :host-id="scope.row.hostId"
                        :name="scope.row.hostName"
                        :domain="scope.row.hostDomain"
                        :operating-system="scope.row.hostOperatingSystem"
                />
            </template>
        </el-table-column>
        <el-table-column label="Module Status" min-width="200" align="center">
            <template slot-scope="scope">
                <div style="font-size: 32px; padding-bottom: 10px">
                    <fa :icon="getModuleStatusIcon(scope.row.status)"
                        :class="getModuleStatusColor(scope.row.status)"/>
                </div>
                    <strong style="font-size: 16px">
                        {{ getModuleStatusName(scope.row.status) }}
                    </strong>
            </template>
        </el-table-column>
    </el-table>
</template>

<style scoped src="@/styles/status.css">
</style>

<script>
    import { mapGetters } from "vuex";

    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";
    import HostFullNameWithLink from "@/components/hosts/HostFullNameWithLink";

    export default {
        name: "ModulesTable",
        props: {
            modules: Array
        },
        components: {
            HostFullNameWithLink,
            UserAvatarAndFullNameWithLink
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            getModuleStatusName(statusId) {
                let statuses = this.getLookupValues("moduleStatuses").filter(status => parseInt(status.value) === statusId);
                return statuses && statuses.length > 0 ? statuses[0].name : "—";
            },
            getModuleStatusIcon(statusId) {
                if (statusId % 4 === 1) {
                    return "info-circle";
                } else if (statusId % 4 === 2) {
                    return "spinner";
                } else if (statusId % 4 === 3) {
                    return "check-circle";
                } else if (statusId % 4 === 0) {
                    return "times-circle";
                } else {
                    return "question-circle";
                }
            },
            getModuleStatusColor(statusId) {
                if (statusId % 4 === 1) {
                    return "info";
                } else if (statusId % 4 === 2) {
                    return "warning";
                } else if (statusId % 4 === 3) {
                    return "success";
                } else if (statusId % 4 === 0) {
                    return "error";
                } else {
                    return "unknown";
                }
            }
        }
    };
</script>