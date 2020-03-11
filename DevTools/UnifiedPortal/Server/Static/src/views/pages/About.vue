<template>
    <LoadingContainer :loading-state="loadingIsActive">
        <template slot="content">
            <Profile header="About">
                <template slot="profile-content">
                    <div style="text-align: center">
                        <h1>This is an about page</h1>
                    </div>
                </template>
            </Profile>
        </template>
    </LoadingContainer>
</template>

<script>
    import LoadingContainer from "@/components/page/LoadingContainer";
    import Profile from "@/components/page/Profile";

    export default {
        name: "About",
        components: {
            LoadingContainer,
            Profile
        },
        data() {
            return {
                loadingIsActive: false
            };
        }
    };
</script>