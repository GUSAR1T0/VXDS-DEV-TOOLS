<template>
    <el-dialog :visible.sync="dialogStatus.visible" width="75%" style="text-align: center">
        <span slot="title" class="modal-title">Note description</span>
        <div style="margin: 20px">
            <Row name="Title" :value="note.title"/>
            <Row name="Folder" :value="note.folderName"/>
            <Row name="Creator">
                <template slot="description">
                    <UserAvatarAndFullNameWithLink
                            :first-name="note.firstName"
                            :last-name="note.lastName"
                            :color="note.color"
                            :user-id="note.userId"
                    />
                </template>
            </Row>
            <Row name="Edit Date / Time" :value="note.editTime"/>
        </div>
    </el-dialog>
</template>

<style scoped src="@/styles/modal.css">
</style>

<script>
    import Row from "@/components/page/Row";
    import UserAvatarAndFullNameWithLink from "@/components/user/UserAvatarAndFullNameWithLink";

    export default {
        name: "NoteInfoDialog",
        props: {
            dialogStatus: Object,
            note: Object
        },
        components: {
            Row,
            UserAvatarAndFullNameWithLink
        }
    };
</script>