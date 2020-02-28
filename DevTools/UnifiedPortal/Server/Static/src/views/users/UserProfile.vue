<template>
    <LoadingContainer class="user-container" :loading-state="loadingIsActive">
        <template slot="content">
            <el-card shadow="hover">
                <div slot="header">
                    <h3>General Info</h3>
                </div>
                <UserCard :user="getUserProfile"/>
                <HorizontalDivider v-if="noUserGeneralInfoDetails" name="Details"/>
                <div v-if="!noUserGeneralInfoDetails && hasPermissionToUpdateUserProfile"
                     style="margin-top: 20px"></div>
                <UserInfoRow v-if="getUserProfile.location" name="Location" :value="getUserProfile.location"/>
                <UserInfoRow v-if="getUserProfile.bio" name="Bio" :value="getUserProfile.bio"/>
                <el-button v-if="hasPermissionToUpdateUserProfile" class="user-container-card-button" type="primary"
                           plain @click="openUserGeneralInfoUpdateForm">
                    <span><fa icon="edit"/><strong> | Edit General Info</strong></span>
                </el-button>
                <UserGeneralInfoUpdateForm v-if="hasPermissionToUpdateUserProfile"
                                           :user="getUserProfile"
                                           :user-general-info-update-form="getUserGeneralInfoUpdateForm"
                                           :page-status="dialogStatuses.userGeneralInfoUpdateFormDialog"
                                           :closed="submitUserGeneralInfoUpdateForm"/>
            </el-card>
            <el-card shadow="hover" style="margin-top: 25px">
                <div slot="header">
                    <h3>Account Specific Info</h3>
                </div>
                <UserInfoRow v-if="getUserProfile.userRole.id" name="User Role" :value="getUserProfile.userRole.name">
                    <template slot="description">
                        <UserRolePermissionsTable
                                :user-role-permissions="userRolePermissions"
                                :user-role="getUserProfile.userRole"
                        />
                    </template>
                </UserInfoRow>
                <UserInfoRow v-if="getUserProfile.isActivated !== undefined" name="User Status"
                             :value="getUserProfile.isActivated ? 'Activated' : 'Deactivated'"/>
                <el-button v-if="hasPermissionToUpdateUserProfile" class="user-container-card-button" type="primary"
                           plain @click="openAccountSpecificInfoUpdateForm">
                    <span><fa icon="edit"/><strong> | Edit Account Specific Info</strong></span>
                </el-button>
                <AccountSpecificInfoUpdateForm v-if="hasPermissionToUpdateUserProfile"
                                               :user="getUserProfile"
                                               :account-specific-info-update-form="getAccountSpecificInfoUpdateForm"
                                               :page-status="dialogStatuses.accountSpecificInfoUpdateFormDialog"
                                               :closed="submitAccountSpecificInfoUpdateForm"/>
            </el-card>
        </template>
    </LoadingContainer>
</template>

<style scoped>
    .user-container {
        margin-top: -17px;
    }

    .user-container-card-button {
        width: 100%;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { GET_PROFILE_ENDPOINT, GET_USER_ROLE_PERMISSIONS_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import {
        GET_HTTP_REQUEST,
        PREPARE_ACCOUNT_SPECIFIC_INFO_UPDATE_FORM,
        PREPARE_USER_GENERAL_INFO_UPDATE_FORM,
        RESET_USER_PROFILE_STORE_STATE,
        SIGN_IN_REQUEST,
        STORE_USER_PROFILE_DATA_REQUEST,
        STORE_USER_PROFILE_ID_REQUEST
    } from "@/constants/actions";
    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import { LOCALHOST } from "@/constants/servers";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer.vue";
    import UserCard from "@/components/user/UserCard.vue";
    import HorizontalDivider from "@/components/page/HorizontalDivider.vue";
    import UserInfoRow from "@/components/user/UserInfoRow.vue";
    import UserRolePermissionsTable from "@/components/user-role/UserRolePermissionsTable.vue";
    import UserGeneralInfoUpdateForm from "@/components/user/UserGeneralInfoUpdateForm.vue";
    import AccountSpecificInfoUpdateForm from "@/components/user/AccountSpecificInfoUpdateForm.vue";

    let dialogStatuses = {
        userGeneralInfoUpdateFormDialog: {
            visible: false
        },
        accountSpecificInfoUpdateFormDialog: {
            visible: false
        }
    };

    export default {
        name: "UserProfile",
        components: {
            LoadingContainer,
            UserCard,
            HorizontalDivider,
            UserInfoRow,
            UserRolePermissionsTable,
            UserGeneralInfoUpdateForm,
            AccountSpecificInfoUpdateForm
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
                "getUserGeneralInfoUpdateForm",
                "getAccountSpecificInfoUpdateForm"
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
            openUserGeneralInfoUpdateForm() {
                this.$store.commit(PREPARE_USER_GENERAL_INFO_UPDATE_FORM);
                this.dialogStatuses.userGeneralInfoUpdateFormDialog.visible = true;
            },
            submitUserGeneralInfoUpdateForm() {
                this.fillForms(this.isAboutMe(this.getUserId));
            },
            openAccountSpecificInfoUpdateForm() {
                this.$store.commit(PREPARE_ACCOUNT_SPECIFIC_INFO_UPDATE_FORM);
                this.dialogStatuses.accountSpecificInfoUpdateFormDialog.visible = true;
            },
            submitAccountSpecificInfoUpdateForm() {
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