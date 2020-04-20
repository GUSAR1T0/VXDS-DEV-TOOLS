<template>
    <div>
        <div style="margin: 10px">
            <strong>Folders</strong>
        </div>
        <el-tree
                class="custom-tree"
                :data="folders"
                :props="folderProps"
                draggable
                default-expand-all
                :expand-on-click-node="false"
                highlight-current
                node-key="id"
                :current-node-key="activeFolder.id"
                @current-change="handleCurrentNodeChange"
                :allow-drop="allowDrop"
                :allow-drag="allowDrag"
                @node-drag-end="handleDrag"
                ref="custom-tree"
        >
            <span class="custom-tree-node" slot-scope="{ node, data }">
                <span style="margin: 0 5px 0 0">
                    <span v-if="folderEditing.id !== data.id">{{ node.label }}</span>
                    <span v-else>
                        <el-input class="folder-edit" v-model="folderEditing.value"/>
                    </span>
                </span>
                <span style="margin: 0 0 0 5px">
                    <span v-if="folderEditing.id !== data.id">
                        <el-button type="text" size="mini" @click="() => append(data)"
                                   :disabled="!!folderEditing.id"
                                   :ref="`add-${data.id}`"
                        >
                            <fa icon="plus-circle"/>
                        </el-button>
                        <el-button type="text" size="mini" @click="() => startEdit(data)"
                                   :disabled="!!folderEditing.id">
                            <fa icon="edit"/>
                        </el-button>
                        <el-button type="text" size="mini" @click="() => openDeleteNoteDialog(data)" v-if="data.id !== 0"
                                   :disabled="!!folderEditing.id"
                                   :ref="`delete-${data.id}`"
                        >
                            <fa icon="trash-alt"/>
                        </el-button>
                    </span>
                    <span v-else>
                        <el-button type="text" size="mini" @click="() => endEdit(data)"
                                   :ref="`edit-${data.id}`"
                        >
                            <fa icon="check-circle"/>
                        </el-button>
                        <el-button type="text" size="mini" @click="cancelEdit">
                            <fa icon="times-circle"/>
                        </el-button>
                    </span>
                </span>
            </span>
        </el-tree>

        <ConfirmationDialog
                :dialog-status="dialogFolderDeleteStatus"
                :confirmation-text="dialogFolderDeleteStatus.confirmationText"
                :additional-text="dialogFolderDeleteStatus.additionalText"
                :cancel-click-action="() => dialogFolderDeleteStatus.visible = false"
                :submit-click-action="remove"/>
    </div>
</template>

<style>
    .el-tree-node {
        flex-grow: 1;
    }

    .custom-tree {
        margin: 10px;
        display: flex;
        flex-grow: 1;
        flex-wrap: nowrap;
        overflow-x: auto;
    }

    .custom-tree-node {
        display: flex;
        align-items: center;
        justify-content: space-between;
        font-size: 14px;
    }

    .folder-edit > input {
        height: 22px;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { LOCALHOST } from "@/constants/servers";
    import { DELETE_HTTP_REQUEST, GET_HTTP_REQUEST, POST_HTTP_REQUEST, PUT_HTTP_REQUEST } from "@/constants/actions";
    import {
        CREATE_NEW_FOLDER_ENDPOINT,
        DELETE_FOLDER_ENDPOINT,
        GET_AFFECTED_NOTE_FOLDERS_COUNTS_BY_FOLDER_ENDPOINT,
        UPDATE_FOLDER_ENDPOINT,
        UPDATE_FOLDER_POSITIONS_ENDPOINT
    } from "@/constants/endpoints";
    import { getConfiguration, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import ConfirmationDialog from "@/components/page/ConfirmationDialog";

    export default {
        name: "FolderTree",
        props: {
            folders: Array,
            activeFolder: Object,
            loadNotes: Function,
            loadFolders: Function,
            findNode: Function
        },
        components: {
            ConfirmationDialog
        },
        data() {
            return {
                folderProps: {
                    label: "name",
                    children: "nodes"
                },
                folderEditing: {
                    id: null,
                    value: ""
                },
                dialogFolderDeleteStatus: {
                    data: null,
                    confirmationText: "",
                    additionalText: "",
                    visible: false
                }
            };
        },
        computed: {
            ...mapGetters([
                "getUnifiedPortalHost"
            ])
        },
        methods: {
            handleCurrentNodeChange(node) {
                if (this.activeFolder.id !== node.id) {
                    this.activeFolder.id = node.id;
                    this.activeFolder.name = node.name;
                    this.$router.replace({
                        name: "home", query: {
                            folderId: this.activeFolder.id
                        }
                    });
                    this.loadNotes();
                }
            },
            allowDrop(draggingNode, dropNode, type) {
                return !(dropNode.data.id === 0 && type !== "inner");
            },
            allowDrag(draggingNode) {
                return draggingNode.data.id !== 0 && !this.folderEditing.id;
            },
            handleDrag() {
                this.foldersLoading = true;
                let updatedFolderPositions = this.prepareTreeAfterDragging(this.folders[0]);

                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: UPDATE_FOLDER_POSITIONS_ENDPOINT,
                    data: updatedFolderPositions,
                    config: getConfiguration()
                }).then(() => {
                    this.loadFolders();
                }).catch(error => {
                    this.foldersLoading = false;
                    this.$notify.error({
                        title: "Failed to update folder positions",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            prepareTreeAfterDragging(folder) {
                let items = [];
                if (folder.nodes) {
                    for (let i = 0; i < folder.nodes.length; i++) {
                        items.push(this.prepareTreeAfterDragging(folder.nodes[i]));
                    }
                }

                return {
                    id: folder.id,
                    nodes: items
                };
            },
            append(data) {
                let button = this.$refs[`add-${data.id}`];
                button.loading = true;

                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(CREATE_NEW_FOLDER_ENDPOINT, {
                        folderId: data.id
                    }),
                    config: getConfiguration()
                }).then(response => {
                    button.loading = false;
                    this.$notify.success({
                        title: "Folder was created"
                    });

                    this.activeFolder.id = null;
                    this.activeFolder.name = "";
                    this.loadFolders(folders => {
                        this.activeFolder.id = response.data;
                        let folder = this.findNode(folders, this.activeFolder.id);
                        this.activeFolder.name = folder ? folder.name : this.folders[0].name;
                        this.loadNotes();
                    });
                }).catch(error => {
                    button.loading = false;
                    this.$notify.error({
                        title: "Failed to create new folder",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            startEdit(data) {
                this.folderEditing.id = data.id;
                this.folderEditing.value = data.name;
            },
            endEdit(data) {
                let button = this.$refs[`edit-${data.id}`];
                button.loading = true;

                this.$store.dispatch(PUT_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(UPDATE_FOLDER_ENDPOINT, {
                        folderId: data.id,
                        name: this.folderEditing.value
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.folderEditing.id = null;
                    this.folderEditing.value = "";
                    this.$notify.success({
                        title: "Folder was updated"
                    });

                    this.activeFolder.id = null;
                    this.activeFolder.name = "";
                    this.loadFolders(folders => {
                        this.activeFolder.id = data.id;
                        let folder = this.findNode(folders, this.activeFolder.id);
                        this.activeFolder.name = folder ? folder.name : this.folders[0].name;
                        this.loadNotes();
                    });
                }).catch(error => {
                    button.loading = false;
                    this.$notify.error({
                        title: "Failed to update folder",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            cancelEdit() {
                this.folderEditing.id = null;
                this.folderEditing.value = "";
            },
            remove(button) {
                button.loading = true;

                this.$store.dispatch(DELETE_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(DELETE_FOLDER_ENDPOINT, {
                        folderId: this.dialogFolderDeleteStatus.data.id
                    }),
                    config: getConfiguration()
                }).then(() => {
                    button.loading = false;
                    this.dialogFolderDeleteStatus.visible = false;

                    this.$notify.success({
                        title: "Folder was deleted"
                    });

                    this.activeFolder.id = null;
                    this.activeFolder.name = "";
                    this.loadFolders(folders => {
                        this.activeFolder.id = 0;
                        let folder = this.findNode(folders, this.activeFolder.id);
                        this.activeFolder.name = folder ? folder.name : this.folders[0].name;
                        this.loadNotes();
                    });
                }).catch(error => {
                    button.loading = false;
                    this.dialogFolderDeleteStatus.visible = false;

                    this.$notify.error({
                        title: "Failed to delete folder",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            openDeleteNoteDialog(data) {
                let button = this.$refs[`delete-${data.id}`];
                button.loading = true;

                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_AFFECTED_NOTE_FOLDERS_COUNTS_BY_FOLDER_ENDPOINT, {
                        folderId: data.id
                    }),
                    config: getConfiguration()
                }).then(response => {
                    button.loading = false;

                    let counts = response.data;
                    this.dialogFolderDeleteStatus.confirmationText = `Are you sure that you want to delete folder "${data.name}"?`;
                    if (counts.foldersCount > 0 && counts.notesCount > 0) {
                        this.dialogFolderDeleteStatus.additionalText = `Count of affected folders: ${counts.foldersCount}, count of affected notes: ${counts.notesCount}`;
                    } else if (counts.foldersCount > 0) {
                        this.dialogFolderDeleteStatus.additionalText = `Count of affected folders: ${counts.foldersCount}`;
                    } else if (counts.notesCount > 0) {
                        this.dialogFolderDeleteStatus.additionalText = `Count of affected notes: ${counts.notesCount}`;
                    } else {
                        this.dialogFolderDeleteStatus.additionalText = "";
                    }
                    this.dialogFolderDeleteStatus.data = data;
                    this.dialogFolderDeleteStatus.visible = true;
                }).catch(error => {
                    button.loading = false;

                    this.$notify.error({
                        title: "Failed to get affected items counts",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
        }
    };
</script>