<template>
    <div class="welcome">
        <img alt="VXDESIGN.STORE: DEVELOPMENT TOOLS logo" src="@/assets/logo.png">
        <h1>VXDESIGN.STORE: DEVELOPMENT TOOLS</h1>
        <h3 style="text-transform: uppercase;">{{ msg }}</h3>
    </div>
</template>

<script>
    export default {
        name: "Welcome",
        props: {
            msg: String
        }
    };
</script>
