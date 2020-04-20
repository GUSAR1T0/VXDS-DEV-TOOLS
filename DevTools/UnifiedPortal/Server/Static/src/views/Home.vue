<template>
    <el-collapse v-model="activeNames" style="margin-top: 10px">
        <el-collapse-item title="User Profile & System Status" name="1">
            <Blocks>
                <template slot="first">
                    <UserProfileCard/>
                </template>
                <template slot="second">
                    <NotificationsCard class="card-top"/>
                    <IncidentsCard class="card-bottom"/>
                </template>
                <template slot="third">
                    <ServerTimeCard class="card-top"/>
                    <HealthCheckCard class="card-bottom"/>
                </template>
            </Blocks>
        </el-collapse-item>
        <el-collapse-item title="Users & Components" name="2">
            <el-carousel trigger="click" indicator-position="outside" height="400px"
                         :type="windowWidth >= 1200 ? 'card' : ''">
                <el-carousel-item>
                    <UsersCard/>
                </el-carousel-item>
                <el-carousel-item>
                    <UserRolesCard/>
                </el-carousel-item>
                <el-carousel-item>
                    <ProjectsCard/>
                </el-carousel-item>
                <el-carousel-item>
                    <ModulesCard/>
                </el-carousel-item>
            </el-carousel>
        </el-collapse-item>
        <el-collapse-item title="System Operations" name="3">
            <SystemCard/>
        </el-collapse-item>
    </el-collapse>
</template>

<style scoped>
    .card-top {
        margin: 10px 10px 40px;
        height: 275px;
    }

    .card-bottom {
        margin: 40px 10px 10px;
        height: 275px;
    }
</style>

<script>
    import Blocks from "@/components/page/Blocks";
    import UserProfileCard from "@/components/dashboard/UserProfileCard";
    import UsersCard from "@/components/dashboard/UsersCard";
    import UserRolesCard from "@/components/dashboard/UserRolesCard";
    import ProjectsCard from "@/components/dashboard/ProjectsCard";
    import ModulesCard from "@/components/dashboard/ModulesCard";
    import SystemCard from "@/components/dashboard/SystemCard";
    import NotificationsCard from "@/components/dashboard/NotificationsCard";
    import IncidentsCard from "@/components/dashboard/IncidentsCard";
    import ServerTimeCard from "@/components/dashboard/ServerTimeCard";
    import HealthCheckCard from "@/components/dashboard/HealthCheckCard";

    export default {
        name: "Home",
        components: {
            Blocks,
            UserProfileCard,
            UsersCard,
            UserRolesCard,
            ProjectsCard,
            ModulesCard,
            SystemCard,
            NotificationsCard,
            IncidentsCard,
            ServerTimeCard,
            HealthCheckCard
        },
        data() {
            return {
                windowWidth: window.innerWidth,
                activeNames: [ "1", "2", "3" ]
            };
        },
        methods: {
            handleResize() {
                this.windowWidth = window.innerWidth;
            }
        },
        created() {
            window.addEventListener("resize", this.handleResize);
            this.handleResize();
        },
        destroyed() {
            window.removeEventListener("resize", this.handleResize);
        }
    };
</script>
