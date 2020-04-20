<template>
    <el-container>
        <el-aside width="300px" class="folders-container hidden-md-and-down"
                  v-loading="foldersLoading"
                  element-loading-text="Loading"
                  element-loading-spinner="el-icon-loading"
                  element-loading-background="rgba(255, 255, 255, 0.75)"
                  element-loading-custom-class="main-loading-spinner-custom"
        >
            <FolderTree
                    :folders="folders"
                    :active-folder="activeFolder"
                    :load-notes="loadNotes"
                    :load-folders="loadFolders"
                    :find-node="findNode"
            />
        </el-aside>
        <el-container
                v-loading="notesLoading"
                element-loading-text="Loading"
                element-loading-spinner="el-icon-loading"
                element-loading-background="rgba(255, 255, 255, 0.75)"
                element-loading-custom-class="main-loading-spinner-custom"
        >
            <el-main>
                <el-container class="hidden-lg-and-up">
                    <FolderTree
                            :folders="folders"
                            :active-folder="activeFolder"
                            :load-notes="loadNotes"
                            :load-folders="loadFolders"
                            :find-node="findNode"
                            style="width: -webkit-fill-available; padding-bottom: 30px"
                    />
                </el-container>
                <FilterableTableView
                        :table="getNoteFolderName"
                        :reset-filters="resetFilters"
                        :reload="loadNotes"
                        :settings="settings"
                >
                    <template slot="buttons">
                        <el-tooltip effect="dark" placement="top">
                            <div slot="content">
                                Create New Note
                            </div>
                            <el-button type="primary" circle class="rounded-button"
                                       @click="$router.push(`folder/${activeFolder.id}/note`)">
                                <span><fa icon="plus-circle"/></span>
                            </el-button>
                        </el-tooltip>
                    </template>
                    <template slot="filters">
                        <NotesTableFilters :filter="filter"/>
                    </template>
                    <template slot="table">
                        <NotesTable
                                :notes="items"
                                :active-folder="activeFolder"
                                :reload="loadNotes"
                        />
                    </template>
                </FilterableTableView>
            </el-main>
        </el-container>
    </el-container>
</template>

<style scoped src="@/styles/button.css">
</style>

<style scoped>
    .folders-container {
        border-right: 1px solid #dcdfe6;
        border-left: 1px solid #dcdfe6;
    }
</style>

<script>
    import { mapGetters } from "vuex";
    import { LOCALHOST } from "@/constants/servers";
    import { GET_HTTP_REQUEST, POST_HTTP_REQUEST } from "@/constants/actions";
    import { GET_FOLDER_LIST_ENDPOINT, GET_NOTE_LIST_ENDPOINT } from "@/constants/endpoints";
    import { getConfiguration, getDate, getOnlyNumbers, renderErrorNotificationMessage } from "@/extensions/utils";
    import format from "string-format";

    import FilterableTableView from "@/components/table-filter/FilterableTableView";
    import NotesTableFilters from "@/components/notes/NotesTableFilters";
    import NotesTable from "@/components/notes/NotesTable";
    import FolderTree from "@/components/notes/FolderTree";

    export default {
        name: "Home",
        components: {
            FilterableTableView,
            NotesTableFilters,
            NotesTable,
            FolderTree
        },
        data() {
            return {
                foldersLoading: true,
                notesLoading: true,
                folders: [],
                activeFolder: {
                    id: null,
                    name: ""
                },
                settings: {
                    pageNo: 1,
                    pageSize: 10,
                    total: 0
                },
                filter: {
                    ids: [],
                    userIds: [],
                    titles: [],
                    projectIds: [],
                    editTimeRange: [],

                    userIdOptions: [],
                    userIdsSearchLoading: false,
                    projectIdOptions: [],
                    projectIdsSearchLoading: false
                },
                items: []
            };
        },
        computed: {
            ...mapGetters([
                "getUnifiedPortalHost"
            ]),
            getNoteFolderName() {
                return this.activeFolder.name ? `Folder: "${this.activeFolder.name}"` : "...";
            }
        },
        methods: {
            loadFolders(callback = null) {
                this.foldersLoading = true;
                this.$store.dispatch(GET_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: GET_FOLDER_LIST_ENDPOINT,
                    config: getConfiguration()
                }).then(response => {
                    this.foldersLoading = false;
                    this.folders = [ response.data ];

                    if (callback) {
                        callback(this.folders);
                    }
                }).catch(error => {
                    this.foldersLoading = false;
                    this.$notify.error({
                        title: "Failed to load list of folders",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            loadNotes() {
                this.notesLoading = true;
                let request = {
                    ids: getOnlyNumbers(this.filter.ids),
                    userIds: this.filter.userIds,
                    titles: this.filter.titles,
                    projectIds: this.filter.projectIds,
                    editTimeRange: this.filter.editTimeRange && this.filter.editTimeRange.length > 1 ? {
                        min: getDate(this.filter.editTimeRange[0]),
                        max: getDate(this.filter.editTimeRange[1])
                    } : null
                };
                this.$store.dispatch(POST_HTTP_REQUEST, {
                    server: LOCALHOST,
                    endpoint: format(GET_NOTE_LIST_ENDPOINT, {
                        folderId: this.activeFolder.id
                    }),
                    data: {
                        pageNo: this.settings.pageNo - 1,
                        pageSize: this.settings.pageSize,
                        filter: request
                    },
                    config: getConfiguration()
                }).then(response => {
                    this.notesLoading = false;
                    this.settings.total = response.data.total;
                    this.items = response.data.items;
                }).catch(error => {
                    this.notesLoading = false;
                    this.$notify.error({
                        title: "Failed to load list of notes",
                        duration: 10000,
                        message: renderErrorNotificationMessage(this.$createElement, this.getUnifiedPortalHost, error.response)
                    });
                });
            },
            resetFilters() {
                this.filter.ids = [];
                this.filter.userIds = [];
                this.filter.titles = [];
                this.filter.projectIds = [];
                this.filter.editTimeRange = [];

                this.filter.userIdOptions = [];
                this.filter.userIdsSearchLoading = false;
                this.filter.projectIdOptions = [];
                this.filter.projectIdsSearchLoading = false;
            },
            findNode(folders, folderId) {
                if (folders) {
                    for (let i = 0; i < folders.length; i++) {
                        if (folders[i].id === folderId) {
                            return folders[i];
                        } else {
                            let result = this.findNode(folders[i].nodes, folderId);
                            if (result) {
                                return result;
                            }
                        }
                    }
                }

                return null;
            }
        },
        mounted() {
            this.activeFolder.id = this.$route.query.folderId ? parseInt(this.$route.query.folderId) : 0;
            this.loadFolders(folders => {
                let folder = this.findNode(folders, this.activeFolder.id);
                this.activeFolder.name = folder ? folder.name : this.folders[0].name;
            });
            this.loadNotes();
        },
        beforeRouteUpdate(to, from, next) {
            this.activeFolder.id = to.query.folderId ? parseInt(to.query.folderId) : 0;
            this.loadFolders(folders => {
                let folder = this.findNode(folders, this.activeFolder.id);
                this.activeFolder.name = folder ? folder.name : this.folders[0].name;
            });
            this.loadNotes();
            next();
        }
    };
</script>
