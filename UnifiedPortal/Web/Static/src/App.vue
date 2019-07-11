<template>
    <div id="app">
        <el-container class="app-container">
            <NavigationBar/>
            <el-main>
                <router-view/>
            </el-main>
        </el-container>
    </div>
</template>

<style>
    * {
        font-family: 'Didact Gothic', 'Avenir', Helvetica, Arial, sans-serif;
    }

    #app {
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        color: #2c3e50;
        min-height: 100%;
        width: 100%
    }

    .app-container {
        min-height: 100%;
    }
</style>

<script>
    import NavigationBar from "@/components/navigation-bar/NavigationBar.vue";

    export default {
        components: {
            NavigationBar
        }
    };
</script>
