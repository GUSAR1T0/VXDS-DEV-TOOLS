import Vue from "vue";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
    faUserCircle,
    faUserAlt,
    faEllipsisH,
    faHome,
    faInfoCircle,
    faAngleRight,
    faAngleLeft,
    faSignOutAlt,
    faSignInAlt,
    faUserPlus
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(faUserCircle, faUserAlt, faEllipsisH, faHome, faInfoCircle, faAngleRight, faAngleLeft, faSignOutAlt, faSignInAlt, faUserPlus);

Vue.component("fa", FontAwesomeIcon);