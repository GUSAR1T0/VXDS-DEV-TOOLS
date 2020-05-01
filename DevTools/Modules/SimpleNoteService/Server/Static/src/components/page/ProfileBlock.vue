<template>
    <div class="profile-block">
        <ActionToolbar>
            <template slot="left">
                <h2 class="profile-block-header" v-if="!$slots['header']">
                    <span><fa :icon="icon"/> {{ header }}</span>
                </h2>
                <h2 class="profile-block-header" v-else>
                    <slot name="header"/>
                </h2>
            </template>
            <template slot="right" v-if="!!$slots['profile-block-buttons']">
                <div class="profile-block-buttons">
                    <slot name="profile-block-buttons"/>
                </div>
            </template>
        </ActionToolbar>
        <el-divider class="profile-block-divider"/>
        <div class="profile-block-content">
            <slot name="profile-block-content"/>
        </div>
    </div>
</template>

<style scoped src="@/styles/profile.css">
</style>

<script>
    import ActionToolbar from "@/components/page/ActionToolbar";

    export default {
        name: "ProfileBlock",
        props: {
            icon: [ String, Array ],
            header: String
        },
        components: {
            ActionToolbar
        }
    };
</script>