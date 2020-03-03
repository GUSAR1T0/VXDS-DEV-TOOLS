<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" @closed="closed">
        <span slot="title" class="modal-title">{{ projectId ? "Update" : "Create" }} Project</span>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<script>
    export default {
        name: "ProjectEditForm",
        props: {
            projectId: Number,
            dialogStatus: Object
        }
    };
</script>