<template>
    <el-container class="error">
        <el-header class="header-error" height="auto">
            <h1 class="header-text-error">
                <el-badge value="oops..." type="danger">
                    {{ errorNumber }}
                </el-badge>
            </h1>
        </el-header>
        <el-main class="main-error">
            <div>
                <h3>{{ problemTitle }}</h3>
                <el-row class="description">
                    <el-col :span="12" :offset="6">
                        {{ description }}
                    </el-col>
                </el-row>
                <el-button type="primary" @click="$router.push('/')">
                    Go to Home Page
                </el-button>
            </div>
        </el-main>
    </el-container>
</template>

<style scoped>
    .error {
        height: 100%;
    }

    .header-error {
        padding: 20px;
    }

    .header-text-error {
        font-size: 128px !important;
        margin: 100px 0;
        height: 50%;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .main-error {
        height: 50%;
    }

    .description {
        padding-bottom: 20px;
    }
</style>

<script>
    export default {
        name: "ErrorPage",
        props: {
            errorNumber: Number,
            problemTitle: String,
            description: String
        }
    };
</script>