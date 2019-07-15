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

    h1 {
        font-size: 30px;
    }

    h2 {
        font-size: 24px;
    }

    h3 {
        font-size: 20px;
    }

    h4 {
        font-size: 16px;
    }

    h5 {
        font-size: 12px;
    }

    h6 {
        font-size: 8px;
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
