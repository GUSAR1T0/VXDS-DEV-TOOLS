<template>
    <DashboardCard
            entityName="User Profile"
            entityIcon="user-alt"
            linkToEntity="/user"
            :load="load"
    >
        <template slot>
            <UserVerticalCard :user="getUserCardData" style="width: 100%"/>
        </template>
    </DashboardCard>
</template>

<style scoped src="@/styles/dashboard.css">
</style>

<script>
    import { mapGetters } from "vuex";

    import DashboardCard from "@/components/dashboard/DashboardCard";
    import UserVerticalCard from "@/components/user/UserVerticalCard";

    export default {
        name: "UserProfileCard",
        components: {
            DashboardCard,
            UserVerticalCard
        },
        computed: {
            ...mapGetters([
                "getUserCardData"
            ])
        },
        methods: {
            load(state) {
                state.loadingIsActive = false;
            }
        }
    };
</script>