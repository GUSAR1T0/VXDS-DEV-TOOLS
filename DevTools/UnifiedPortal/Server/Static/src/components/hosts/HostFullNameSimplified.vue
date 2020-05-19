<template>
    <div>
        <div>
            <fa style="font-size: 20px" :icon="['fab', getOperatingSystemIcon(operatingSystem)]"/>
            <strong style="font-size: 22px; padding-left: 5px">{{ name }}</strong>
        </div>
        <div style="font-size: 16px; padding-top: 5px">
            <strong>Domain:</strong> {{ domain }}
        </div>
    </div>
</template>

<script>
    import { mapGetters } from "vuex";

    export default {
        name: "HostFullNameSimplified",
        props: {
            name: String,
            domain: String,
            operatingSystem: [ Number, String ]
        },
        computed: {
            ...mapGetters([
                "getLookupValues"
            ])
        },
        methods: {
            getOperatingSystemIcon(osId) {
                let osList = this.getLookupValues("hostOperatingSystems").filter(os => parseInt(os.value) === parseInt(osId));
                if (osList && osList.length > 0) {
                    let os = osList[0];
                    if (os.value === "1") {
                        return "windows";
                    } else if (os.value === "2") {
                        return "linux";
                    } else if (os.value === "3") {
                        return "apple";
                    }
                } else {
                    return "question-circle";
                }
            }
        }
    };
</script>