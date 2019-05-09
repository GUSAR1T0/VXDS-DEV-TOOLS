<template>
    <div class="home">
        <Navigation/>
        <img alt="Vue logo" src="../assets/logo.png">
        <HelloWorld msg="Welcome to Your Vue.js App"/>
    </div>
</template>

<script>
    // @ is an alias to /src
    import Navigation from "@/components/Navigation.vue";
    import HelloWorld from "@/components/HelloWorld.vue";

    export default {
        name: "home",
        components: {
            Navigation,
            HelloWorld
        }
    };
</script>
