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
        align-items: center;
    }

    .left-action-toolbar {
        margin-right: auto;
        padding-right: 5px;
        text-align: left;
    }

    .right-action-toolbar {
        margin-left: auto;
        padding-left: 5px;
        text-align: right;
    }
</style>

<script>
    export default {
        name: "ActionToolbar"
    };
</script>