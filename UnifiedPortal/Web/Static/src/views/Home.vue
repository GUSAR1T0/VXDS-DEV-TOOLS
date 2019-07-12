<template>
    <div class="home">
        <Welcome msg="Home Page"/>
    </div>
</template>

<style scoped>
    .home {
        text-align: center;
    }
</style>

<script>
    import Welcome from "@/components/Welcome.vue";

    export default {
        name: "Home",
        components: {
            Welcome
        }
    };
</script>
