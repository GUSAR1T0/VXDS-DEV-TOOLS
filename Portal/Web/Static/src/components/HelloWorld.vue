<template>
    <div class="hello">
        <h1>VXDESIGN.STORE: DEVELOPMENT TOOLS</h1>
        <h3 style="text-transform: uppercase;">{{ msg }}</h3>
        <h5>Hello World!</h5>
    </div>
</template>

<script>
    export default {
        name: "HelloWorld",
        props: {
            msg: String
        }
    };
</script>
