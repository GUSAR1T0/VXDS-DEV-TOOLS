<template>
    <div class="about">
        <Welcome msg="About"/>
        <h1>This is an about page</h1>
    </div>
</template>

<style scoped>
    .about {
        text-align: center;
    }
</style>

<script>
    import Welcome from "@/components/Welcome.vue";

    export default {
        name: "About",
        components: {
            Welcome: Welcome,
        }
    };
</script>