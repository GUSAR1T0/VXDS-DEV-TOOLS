<template>
    <el-table :data="getPermissionsTable" style="width: 100%" border>
        <el-table-column label="Permissions types" align="center">
            <template slot-scope="scope">
                <strong class="permissions-type">{{ scope.row.type }}</strong>
            </template>
        </el-table-column>
        <el-table-column label="Lists of permissions" align="center">
            <template slot-scope="scope">
                <div v-if="scope.row.list && scope.row.list.length > 0">
                    <div v-for="permission in scope.row.list" v-bind:key="permission"
                         style="display: inline-block; padding: 5px">
                        <el-tag type="info" :effect="permission.type" hit>
                            <strong>{{ permission.name }}</strong>
                        </el-tag>
                    </div>
                </div>
                <div v-else>—</div>
            </template>
        </el-table-column>
    </el-table>
</template>

<style scoped>
    .permissions-type {
        font-size: 16px;
        word-break: break-word;
    }
</style>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "UserRolePermissionsTable",
        props: {
            userRolePermissions: Array,
            userRole: Object
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ]),
            getPermissionsTable() {
                let getPermissionTagType = (permissionsValue, permissionValue) => {
                    return !permissionsValue || permissionsValue.length === 0 || !permissionsValue.includes(permissionValue) ? "plain" : "dark";
                };
                return this.userRolePermissions.map(userRolePermission => {
                    let userPermissions = this.userRole.permissions.filter(item => item.permissionGroupId === userRolePermission.id);
                    return {
                        type: userRolePermission.name,
                        list: userRolePermission.permissions.map(permission => {
                            return {
                                type: getPermissionTagType(userPermissions && userPermissions.length > 0 ? userPermissions[0].permissions : 0, permission.id),
                                name: permission.name
                            };
                        })
                    };
                });
            }
        }
    };
</script>