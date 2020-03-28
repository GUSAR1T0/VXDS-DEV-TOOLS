<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <FilterableTableView
                    table="Notifications"
                    :reset-filters="resetFilters"
                    :reload="loadNotifications"
                    :settings="settings"
            >
                <template slot="buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Create New Notification
                        </div>
                        <el-button type="primary" circle v-if="hasPermissionToManageNotifications"
                                   @click="openDialogToCreate" class="rounded-button">
                            <span><fa icon="plus-circle"/></span>
                        </el-button>
                    </el-tooltip>

                    <NotificationEditForm v-if="hasPermissionToManageNotifications"
                                          :dialog-status="dialogNotificationFormStatus"
                                          :notification="getNotification"
                                          :notification-form="getNotificationForm"
                                          :closed="submitNotificationAction"/>
                </template>
                <template slot="filters">
                    <NotificationsTableFilters :filter="filter"/>
                </template>
                <template slot="table">
                    <NotificationsTable :notifications="items" :reload="loadNotifications"/>
                </template>
            </FilterableTableView>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/button.css">
</style>

<script>
    import { mapGetters } from "vuex";
    import { POST_HTTP_REQUEST, RESET_NOTIFICATION_STORE_STATE } from "@/constants/actions";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_NOTIFICATION_LIST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, getDate, getOnlyNumbers, renderErrorNotificationMessage } from "@/extensions/utils";

    import { PORTAL_PERMISSION } from "@/constants/permissions";
    import LoadingContainer from "@/components/page/LoadingContainer";
    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import NotificationsTableFilters from "@/components/notifications/NotificationsTableFilters";
    import NotificationsTable from "@/components/notifications/NotificationsTable";
    import NotificationEditForm from "@/components/notifications/NotificationEditForm";

    export default {
        name: "Notifications",
        components: {
            LoadingContainer,
            FilterableTableView,
            NotificationsTableFilters,
            NotificationsTable,
            NotificationEditForm
        },
        data() {
            return {
                loadingIsActive: true,
                settings: {
                    pageNo: 1,
                    pageSize: 10,
                    total: 0
                },
                filter: {
                    ids: [],
                    levels: [],
                    startTimeRange: [],
                    stopTimeRange: [],
                    isActive: true
                },
                items: [],
                dialogNotificationFormStatus: {
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "hasPortalPermission",
                "getNotification",
                "getNotificationForm"
            ]),
            hasPermissionToManageNotifications() {
                return this.hasPortalPermission(PORTAL_PERMISSION.MANAGE_NOTIFICATIONS);
            }
        },
        methods: {
            loadNotifications() {
                this.loadingIsActive = true;
                let request = {
                    ids: getOnlyNumbers(this.filter.ids),
                    levels: getOnlyNumbers(this.filter.levels),
                    startTimeRange: this.filter.startTimeRange && this.filter.startTimeRange.length > 1 ? {
                        min: getDate(this.filter.startTimeRange[0]),
                        max: getDate(this.filter.startTimeRange[1])
                    } : null,
                    stopTimeRange: this.filter.stopTimeRange && this.filter.stopTimeRange.length > 1 ? {
                        min: getDate(this.filter.stopTimeRange[0]),
                        max: getDate(this.filter.stopTimeRange[1])
                    } : null,
                    isActive: this.filter.isActive
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_NOTIFICATION_LIST_ENDPOINT,
                    data: {
                        pageNo: this.settings.pageNo - 1,
                        pageSize: this.settings.pageSize,
                        filter: request
                    },
                    config: getConfiguration()
                }).then(response => {
                    this.loadingIsActive = false;
                    this.settings.total = response.data.total;
                    this.items = response.data.items;
                }).catch(error => {
                    this.loadingIsActive = false;
                    this.$notify.error({
                        title: "Failed to load list of notifications",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, error.response)
                    });
                });
            },
            resetFilters() {
                this.filter.ids = [];
                this.filter.levels = [];
                this.filter.startTimeRange = [];
                this.filter.stopTimeRange = [];
                this.filter.isActive = true;
            },
            openDialogToCreate() {
                this.$store.commit(RESET_NOTIFICATION_STORE_STATE);
                this.dialogNotificationFormStatus.visible = true;
            },
            submitNotificationAction() {
                this.loadNotifications();
                this.$store.commit(RESET_NOTIFICATION_STORE_STATE);
            }
        },
        mounted() {
            this.loadNotifications();
        },
        beforeRouteUpdate(to, from, next) {
            this.loadNotifications();
            next();
        }
    };
</script>