<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center">
        <span slot="title" class="modal-title">Choose new folder</span>
        <h2 v-if="dialogStatus.text">
            {{ dialogStatus.text }}
        </h2>
        <el-container
                v-loading="dialogStatus.foldersLoading"
                element-loading-text="Loading"
                element-loading-spinner="el-icon-loading"
                element-loading-background="rgba(255, 255, 255, 0.75)"
                element-loading-custom-class="main-loading-spinner-custom">
            <el-tree
                    class="custom-tree"
                    :data="dialogStatus.folders"
                    :props="folderProps"
                    draggable
                    default-expand-all
                    :expand-on-click-node="false"
                    highlight-current
                    @current-change="handleCurrentNodeChange"
                    node-key="id"
                    :current-node-key="folderId"
            >
                <span class="custom-tree-node" slot-scope="{ node }">
                    <span>{{ node.label }}</span>
                </span>
            </el-tree>
        </el-container>
        <el-row type="flex" justify="center" align="middle" :gutter="20">
            <el-col :span="12">
                <el-button @click="dialogStatus.visible = false" style="width: 100%">
                    <strong>Cancel</strong>
                </el-button>
            </el-col>
            <el-col :span="12">
                <el-button type="primary" ref="dialogNoteChangeFolderButton"
                           @click="submit($refs.dialogNoteChangeFolderButton)" style="width: 100%">
                    <strong>Submit</strong>
                </el-button>
            </el-col>
        </el-row>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<style scoped>
    .custom-tree {
        padding-bottom: 20px;
    }

    .custom-tree-node {
        font-size: 14px;
    }
</style>

<script>
    export default {
        name: "ChangeFolderDialog",
        props: {
            folderId: Number,
            dialogStatus: Object,
            submit: Function
        },
        data() {
            return {
                folderProps: {
                    label: "name",
                    children: "nodes"
                }
            };
        },
        methods: {
            handleCurrentNodeChange(node) {
                this.dialogStatus.folderId = node.id;
            }
        }
    };
</script>