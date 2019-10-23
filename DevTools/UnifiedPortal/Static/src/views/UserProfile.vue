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
                <el-button v-if="hasPermissionToUpdateUserProfile" class="user-container-card-button" type="danger"
                           plain @click="openUserGeneralInfoUpdateForm">
                    <span><fa icon="edit"/> | Edit General Info</span>
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
                <UserInfoRow v-if="getUserProfile.userRole.id" name="User Role" :value="getUserProfile.userRole.name"
                             :has-description="hasUserRoleDescription">
                    <template slot="description">
                        <UserRolePermissionsTable :user-role="getUserProfile.userRole"/>
                    </template>
                </UserInfoRow>
                <el-button v-if="hasPermissionToUpdateUserProfile" class="user-container-card-button" type="danger"
                           plain @click="openAccountSpecificInfoUpdateForm">
                    <span><fa icon="edit"/> | Edit Account Specific Info</span>
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
    import { GET_PROFILE_ENDPOINT } from "@/constants/endpoints";
    import { renderErrorNotificationMessage } from "@/extensions/utils";
    import {
        PREPARE_ACCOUNT_SPECIFIC_INFO_UPDATE_FORM,
        PREPARE_USER_GENERAL_INFO_UPDATE_FORM, RESET_USER_PROFILE_STORE_STATE,
        SIGN_IN_REQUEST,
        STORE_USER_PROFILE_DATA_REQUEST,
        STORE_USER_PROFILE_ID_REQUEST
    } from "@/constants/actions";
    import { USER_PERMISSION } from "@/constants/permissions";
    import format from "string-format";

    import LoadingContainer from "@/components/page/LoadingContainer.vue";
    import UserCard from "@/components/user/UserCard.vue";
    import HorizontalDivider from "@/components/page/HorizontalDivider.vue";
    import UserInfoRow from "@/components/user/UserInfoRow.vue";
    import UserRolePermissionsTable from "@/components/user/UserRolePermissionsTable.vue";
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
        name: "User",
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
                dialogStatuses
            };
        },
        computed: {
            ...mapGetters([
                "isAboutMe",
                "getUserId",
                "hasUserPermission",
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
            hasUserRoleDescription() {
                return this.getLookupValues("userPermissions") || this.getLookupValues("userRolePermissions");
            },
            hasPermissionToUpdateUserProfile() {
                return this.isAboutMe(this.getUserId) || this.hasUserPermission(USER_PERMISSION.UPDATE_USER);
            }
        },
        methods: {
            fillForms(reloadAuthenticationData = false) {
                this.loadingIsActive = true;
                this.$store.dispatch(STORE_USER_PROFILE_DATA_REQUEST, format(GET_PROFILE_ENDPOINT, {
                        id: this.getUserProfileId ? this.getUserProfileId : this.getUserId
                    }
                )).then(response => {
                    this.loadingIsActive = false;
                    if (reloadAuthenticationData) {
                        this.$store.commit(SIGN_IN_REQUEST, response.data);
                    }
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$router.back();
                    this.$notify.error({
                        title: "Failed to load user profile",
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