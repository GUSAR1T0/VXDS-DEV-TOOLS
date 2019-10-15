<template>
    <el-container class="user"
                  v-loading="loadingIsActive"
                  element-loading-text="Loading"
                  element-loading-spinner="el-icon-loading"
                  element-loading-background="rgba(255, 255, 255, 0.75)"
                  element-loading-custom-class="main-loading-spinner-custom">
        <el-main>
            <el-card shadow="hover">
                <div slot="header">
                    <h3>General Info</h3>
                </div>
                <UserCard :user="user"/>
                <HorizontalDivider v-if="noDetails" name="Details"/>
                <div v-if="!noDetails && isAboutMe" style="margin-top: 20px"></div>
                <UserInfoRow v-if="user.location" name="Location" :value-input="user.location"/>
                <UserInfoRow v-if="user.bio" name="Bio" :value-input="user.bio"/>
                <el-button v-if="isAboutMe" style="width: 100%" type="danger" plain @click="openUserGeneralInfoUpdateForm">
                    <fa icon="edit"></fa>
                </el-button>
                <UserGeneralInfoUpdateForm :user="user" :user-general-info-update-form="userGeneralInfoUpdateForm" :page-status="pageStatus"
                                           :closed="submitUserGeneralInfoUpdateForm"/>
            </el-card>
            <el-card shadow="hover" style="margin-top: 25px">
                <div slot="header">
                    <h3>System</h3>
                </div>
                <div style="text-align: left;">
                    <el-row>Content</el-row>
                </div>
            </el-card>
        </el-main>
    </el-container>
</template>

<style scoped>
    .user {
        margin-top: -17px;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import HttpClient from "@/extensions/httpClient";
    import { GET_PROFILE_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration } from "@/extensions/utils";
    import { LOCALHOST } from "@/constants/servers";

    import UserCard from "@/components/user/UserCard.vue";
    import HorizontalDivider from "@/components/page/HorizontalDivider.vue";
    import UserInfoRow from "@/components/user/UserInfoRow.vue";
    import UserGeneralInfoUpdateForm from "@/components/user/UserGeneralInfoUpdateForm.vue";

    let user = {
        id: "",
        email: "",
        firstName: "",
        lastName: "",
        color: "",
        location: "",
        bio: ""
    };

    let pageStatus = {
        userUpdateFormDialogVisible: false
    };

    export default {
        name: "User",
        components: {
            UserCard,
            HorizontalDivider,
            UserInfoRow,
            UserGeneralInfoUpdateForm
        },
        data() {
            return {
                user,
                userGeneralInfoUpdateForm: {},
                loadingIsActive: true,
                pageStatus
            };
        },
        computed: {
            ...mapGetters([
                "getEmail"
            ]),
            isAboutMe() {
                return this.getEmail === this.user.email;
            },
            noDetails() {
                return this.user.location || this.user.bio;
            }
        },
        methods: {
            fillForm(email) {
                this.loadingIsActive = true;
                let emailQueried = email === undefined ? this.getEmail : email;
                let query = `${GET_PROFILE_ENDPOINT}?email=${emailQueried}`;
                HttpClient.init().then(client => client.get(LOCALHOST, query, getConfiguration()).then(response => {
                    this.loadingIsActive = false;

                    this.user.id = response.data.id;
                    this.user.email = response.data.email;
                    this.user.firstName = response.data.firstName;
                    this.user.lastName = response.data.lastName;
                    this.user.color = response.data.color;
                    this.user.location = response.data.location;
                    this.user.bio = response.data.bio;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$router.back();
                    this.$notify.error({
                        title: "Error",
                        message: `Failed to load user profile: ${error.response.data.message}`
                    });
                }));
            },
            openUserGeneralInfoUpdateForm() {
                this.userGeneralInfoUpdateForm = JSON.parse(JSON.stringify(this.user));
                this.pageStatus.userUpdateFormDialogVisible = true;
            },
            submitUserGeneralInfoUpdateForm() {
                this.fillForm(this.$route.params.email);
            }
        },
        mounted() {
            this.fillForm(this.$route.params.email);
        },
        beforeRouteUpdate(to, from, next) {
            this.fillForm(to.params.email);
            next();
        }
    };
</script>