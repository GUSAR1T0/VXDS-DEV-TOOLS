<template>
    <div class="profile">
        <ActionToolbar style="margin-bottom: 40px">
            <template slot="left">
                <h1 class="profile-header" v-if="!$slots['header']">{{ header }}</h1>
                <h1 class="profile-header" v-else>
                    <slot name="header"/>
                </h1>
            </template>
            <template slot="right" v-if="!!$slots['profile-buttons']">
                <div class="profile-buttons">
                    <slot name="profile-buttons"/>
                </div>
            </template>
        </ActionToolbar>
        <div class="profile-tab">
            <slot name="profile-content"/>
        </div>
    </div>
</template>

<style scoped src="@/styles/profile.css">
</style>

<script>
    import ActionToolbar from "@/components/page/ActionToolbar";

    export default {
        name: "Profile",
        props: {
            header: String
        },
        components: {
            ActionToolbar
        }
    };
</script>