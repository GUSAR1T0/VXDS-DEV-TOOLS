<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile>
                <template slot="header">
                    <UserAvatarAndFullName
                            :first-name="getUserProfile.firstName"
                            :last-name="getUserProfile.lastName"
                            :color="getUserProfile.color"
                            big
                    />
                </template>
                <template slot="profile-buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Reload This Page
                        </div>
                        <el-button type="info" plain circle @click="$router.go(0)"
                                   class="rounded-button">
                            <span><fa icon="sync-alt"/></span>
                        </el-button>
                    </el-tooltip>

                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            To User On Unified Portal Page
                        </div>
                        <el-button type="info" plain circle class="rounded-button"
                                   @click="$router.push(`/unifiedPortal?host=${getUnifiedPortalHost}&link=user/${userId}`)">
                            <span><fa icon="user-alt"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
                <template slot="profile-content">
                    <ProfileBlock icon="user-alt" header="General Info"
                                  v-if="getUserProfile.location || getUserProfile.bio">
                        <template slot="profile-block-content">
                            <Row v-if="getUserProfile.location" name="Location" :value="getUserProfile.location"/>
                            <Row v-if="getUserProfile.bio" name="Bio" :value="getUserProfile.bio"/>
                        </template>
                    </ProfileBlock>
                    <ProfileBlock icon="address-card" header="Account Specific Info">
                        <template slot="profile-block-content">
                            <Row v-if="getUserProfile.isActivated !== null || getUserProfile.isActivated !== undefined"
                                 name="User Status" :value="getUserProfile.isActivated ? 'Activated' : 'Deactivated'"/>
                            <Row v-if="getUserProfile.userRole.id" name="User Role"
                                 :value="getUserProfile.userRole.name">
                            </Row>
                        </template>
                    </ProfileBlock>
                </template>
            </Profile>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { GET_PROFILE_ENDPOINT } from "@/constants/endpoints";
    import { renderErrorNotificationMessage } from "@/extensions/utils";
    import { RESET_USER_PROFILE_STORE_STATE, STORE_USER_PROFILE_DATA_REQUEST } from "@/constants/actions";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer.vue";
    import UserAvatarAndFullName from "@/components/user/UserAvatarAndFullName";
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";
    import Row from "@/components/page/Row";

    let dialogStatuses = {
        userUpdateFormDialog: {
            visible: false
        }
    };

    export default {
        name: "UserProfile",
        components: {
            LoadingContainer,
            UserAvatarAndFullName,
            Profile,
            ProfileBlock,
            Row
        },
        data() {
            return {
                loadingIsActive: true,
                userId: null,
                dialogStatuses,
                userRolePermissions: []
            };
        },
        computed: {
            ...mapGetters([
                "getUserId",
                "getUserProfile",
                "getUnifiedPortalHost"
            ]),
            noUserGeneralInfoDetails() {
                let userProfile = this.getUserProfile;
                return userProfile.location || userProfile.bio;
            }
        },
        methods: {
            fillForms() {
                this.loadingIsActive = true;
                this.$store.dispatch(STORE_USER_PROFILE_DATA_REQUEST,
                    format(GET_PROFILE_ENDPOINT, {
                        id: this.userId
                    })
                ).then(() => {
                    this.loadingIsActive = false;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load user profile",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            }
        },
        mounted() {
            this.userId = this.$route.params.id ? this.$route.params.id : this.getUserId;
            this.fillForms();
        },
        beforeRouteUpdate(to, from, next) {
            this.userId = to.params.id ? to.params.id : this.getUserId;
            this.fillForms();
            next();
        },
        beforeRouteLeave(to, from, next) {
            if (to.path !== "/unifiedPortal") {
                this.$store.commit(RESET_USER_PROFILE_STORE_STATE);
            }
            next();
        }
    };
</script>