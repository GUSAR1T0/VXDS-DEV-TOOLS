<template>
    <div class="home">
        <img alt="VXDESIGN.STORE: DEVELOPMENT TOOLS logo" src="@/assets/logo.png">
        <HelloWorld msg="Portal"/>
    </div>
</template>

<style scoped>
    .home {
        text-align: center;
    }
</style>

<script>
    import HelloWorld from "@/components/HelloWorld.vue";

    export default {
        name: "Home",
        components: {
            HelloWorld
        }
    };
</script>
