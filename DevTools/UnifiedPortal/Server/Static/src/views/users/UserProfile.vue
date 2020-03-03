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
                            Edit This User Profile
                        </div>
                        <el-button v-if="hasPermissionToUpdateUserProfile" type="primary" plain circle
                                   @click="openUserUpdateForm" class="rounded-button">
                            <span><fa icon="edit"/></span>
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
                                <template slot="description">
                                    <UserRolePermissionsTable
                                            :user-role-permissions="userRolePermissions"
                                            :user-role="getUserProfile.userRole"
                                    />
                                </template>
                            </Row>
                        </template>
                    </ProfileBlock>
                </template>
            </Profile>

            <UserUpdateForm v-if="hasPermissionToUpdateUserProfile"
                            :user="getUserProfile"
                            :user-update-form="getUserUpdateForm"
                            :page-status="dialogStatuses.userUpdateFormDialog"
                            :closed="submitUserUpdateForm"/>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { GET_PROFILE_ENDPOINT, GET_USER_ROLE_PERMISSIONS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import {
        GET_HTTP_REQUEST,
        PREPARE_USER_UPDATE_FORM,
        RESET_USER_PROFILE_STORE_STATE,
        SIGN_IN_REQUEST,
        STORE_USER_PROFILE_DATA_REQUEST,
        STORE_USER_PROFILE_ID_REQUEST
    } from "@/constants/actions";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer.vue";
    import UserAvatarAndFullName from "@/components/user/UserAvatarAndFullName";
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";
    import Row from "@/components/page/Row";
    import UserRolePermissionsTable from "@/components/user-role/UserRolePermissionsTable.vue";
    import UserUpdateForm from "@/components/user/UserUpdateForm.vue";

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
            Row,
            UserRolePermissionsTable,
            UserUpdateForm
        },
        data() {
            return {
                loadingIsActive: true,
                dialogStatuses,
                userRolePermissions: []
            };
        },
        computed: {
            ...mapGetters([
                "isAboutMe",
                "getUserId",
                "hasPortalPermission",
                "getUserProfileId",
                "getUserProfile",
                "getLookupValues",
                "getUserUpdateForm"
            ]),
            noUserGeneralInfoDetails() {
                let userProfile = this.getUserProfile;
                return userProfile.location || userProfile.bio;
            },
            hasPermissionToUpdateUserProfile() {
                return this.isAboutMe(this.getUserId) || this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_USER_PROFILES);
            }
        },
        methods: {
            fillForms(reloadAuthenticationData = false) {
                this.loadingIsActive = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_USER_ROLE_PERMISSIONS_ENDPOINT,
                    config: getConfiguration()
                }).then(firstResponse => {
                    this.userRolePermissions = firstResponse.data;
                    this.$store.dispatch(STORE_USER_PROFILE_DATA_REQUEST, format(GET_PROFILE_ENDPOINT, {
                            id: this.getUserProfileId ? this.getUserProfileId : this.getUserId
                        }
                    )).then(secondResponse => {
                        this.loadingIsActive = false;
                        if (reloadAuthenticationData) {
                            this.$store.commit(SIGN_IN_REQUEST, secondResponse.data);
                        }
                    }).catch(error => {
                        this.loadingIsActive = false;
                        this.$notify.error({
                            title: "Failed to load user profile",
                            duration: 10000,
                            message: renderErrorNotificationMessage(this.$createElement, error.response)
                        });
                    });
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load list of user role permissions",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            openUserUpdateForm() {
                this.$store.commit(PREPARE_USER_UPDATE_FORM);
                this.dialogStatuses.userUpdateFormDialog.visible = true;
            },
            submitUserUpdateForm() {
                this.fillForms(this.isAboutMe(this.getUserId));
            }
        },
        mounted() {
            this.$store.commit(STORE_USER_PROFILE_ID_REQUEST, this.$route.params.id);
            this.fillForms();
        },
        beforeRouteUpdate(to, from, next) {
            this.$store.commit(STORE_USER_PROFILE_ID_REQUEST, to.params.id);
            this.fillForms();
            next();
        },
        beforeRouteLeave(to, from, next) {
            this.$store.commit(RESET_USER_PROFILE_STORE_STATE);
            next();
        }
    };
</script>