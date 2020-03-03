<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile :header="`${project.name} (${project.alias})`">
                <template slot="profile-buttons">
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Edit This Project
                        </div>
                        <el-button type="primary" plain circle
                                   class="rounded-button">
                            <span><fa icon="edit"/></span>
                        </el-button>
                    </el-tooltip>
                    <el-tooltip effect="dark" placement="top">
                        <div slot="content">
                            Delete This Project
                        </div>
                        <el-button type="danger" plain circle
                                   class="rounded-button">
                            <span><fa icon="trash-alt"/></span>
                        </el-button>
                    </el-tooltip>
                </template>
                <template slot="profile-content">
                    <ProfileBlock icon="code" header="Project Info">
                        <template slot="profile-block-content">
                            <Row v-if="project.description" name="Description" :value="project.description"/>
                            <Row name="Project Status" :value="project.isActive ? 'Active' : 'Inactive'"/>
                        </template>
                    </ProfileBlock>
                    <ProfileBlock :icon="['fab', 'github']" header="GitHub Repository">
                        <template slot="profile-block-content">
                            <Blocks>
                                <template slot="first">
                                    <Row name="Owner" value="GUSAR1T0" half/>
                                </template>
                                <template slot="second">
                                    <Row name="Repository Name" value="GUSAR1T0/VXDS-DEV-TOOLS" half/>
                                </template>
                            </Blocks>
                        </template>
                    </ProfileBlock>
                </template>
            </Profile>
        </template>
    </LoadingContainer>
</template>

<style scoped src="@/styles/profile.css">
</style>

<style scoped src="@/styles/button.css">
</style>

<script>
    import LoadingContainer from "@/components/page/LoadingContainer";
    import Row from "@/components/page/Row";
    import Blocks from "@/components/page/Blocks";
    import Profile from "@/components/page/Profile";
    import ProfileBlock from "@/components/page/ProfileBlock";

    export default {
        name: "Project",
        components: {
            LoadingContainer,
            Row,
            Blocks,
            Profile,
            ProfileBlock
        },
        data() {
            return {
                loadingIsActive: true,
                projectId: null,
                project: {}
            };
        },
        methods: {
            loadProject() {
                this.project.name = "VXDESIGN.STORE: Development Tools";
                this.project.alias = "VXDS-DT";
                this.project.description = "The power is in development";
                this.project.isActive = true;
                this.loadingIsActive = false;
            }
        },
        mounted() {
            this.projectId = this.$route.params.id;
            this.loadProject();
        },
        beforeRouteUpdate(to, from, next) {
            this.projectId = to.params.id;
            this.loadProject();
            next();
        }
    };
</script>