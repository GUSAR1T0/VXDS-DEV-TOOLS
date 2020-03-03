<template>
    <div class="profile-block">
        <h2 class="profile-block-header"><span><fa :icon="icon"/> {{ header }}</span></h2>
        <el-divider class="profile-block-divider"/>
        <div class="profile-block-content">
            <slot name="profile-block-content"/>
        </div>
    </div>
</template>

<style scoped src="@/styles/profile.css">
</style>

<script>
    export default {
        name: "ProfileBlock",
        props: {
            icon: [ String, Array ],
            header: String
        }
    };
</script>