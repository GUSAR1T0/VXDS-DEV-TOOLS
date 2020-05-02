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
                <el-link :href="`/system/settings?tab=environment&hostId=${scope.row.hostId}`" type="primary" :underline="false"
                         v-if="hasPermissionToOpenSettingsPage">
                    <HostFullName
                            :name="scope.row.hostName"
                            :domain="scope.row.hostDomain"
                            :operating-system="scope.row.hostOperatingSystem"
                    />
                </el-link>
                <div v-else>
                    <HostFullName
                            :name="scope.row.hostName"
                            :domain="scope.row.hostDomain"
                            :operating-system="scope.row.hostOperatingSystem"
                    />
                </div>
            </template>
        </el-table-column>
        <el-table-column label="Module Status" min-width="200" align="center">
            <template slot-scope="scope">
                <strong style="font-size: 16px">{{ scope.row.isActive ? "Active" : "Inactive" }}</strong>
            </template>
        </el-table-column>
    </el-table>
</template>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";

    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";
    import HostFullName from "@/components/hosts/HostFullName";

    export default {
        name: "ModulesTable",
        props: {
            modules: Array
        },
        components: {
            HostFullName,
            UserAvatarAndFullNameWithLink
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission"
            ]),
            hasPermissionToOpenSettingsPage() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_SETTINGS);
            }
        }
    };
</script>