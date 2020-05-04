<template>
    <div>
        <el-link :href="`/system/settings?tab=environment&hostId=${hostId}`" type="primary"
                 :underline="false" v-if="hasPermissionToOpenSettingsPage">
            <HostFullName
                    :name="name"
                    :domain="domain"
                    :operating-system="operatingSystem"
            />
        </el-link>
        <div v-else>
            <HostFullName
                    :name="name"
                    :domain="domain"
                    :operating-system="operatingSystem"
            />
        </div>
    </div>
</template>

<script>
    import { mapGetters } from "vuex";
    import { PORTAL_PERMISSION } from "@/constants/permissions";

    import HostFullName from "@/components/hosts/HostFullName";

    export default {
        name: "HostFullNameWithLink",
        props: {
            hostId: Number,
            name: String,
            domain: String,
            operatingSystem: [ Number, String ]
        },
        components: {
            HostFullName
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