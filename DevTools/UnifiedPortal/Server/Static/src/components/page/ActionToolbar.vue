<template>
    <div class="action-toolbar">
        <div class="left-action-toolbar">
            <slot name="left"/>
        </div>
        <div class="right-action-toolbar">
            <slot name="right"/>
        </div>
    </div>
</template>

<style scoped>
    .action-toolbar {
        display: flex;
    }

    .left-action-toolbar {
        margin-right: auto;
    }

    .right-action-toolbar {
        margin-left: auto;
    }
</style>

<script>
    export default {
        name: "ActionToolbar"
    };
</script>