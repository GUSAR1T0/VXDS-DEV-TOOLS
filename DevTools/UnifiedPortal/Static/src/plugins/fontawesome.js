import Vue from "vue";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
    faUserCircle,
    faUserAlt,
    faEllipsisH,
    faInfoCircle,
    faSignOutAlt,
    faSignInAlt,
    faUserPlus,
    faQuestionCircle,
    faAlignJustify,
    faEdit,
    faTools
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(faUserCircle, faUserAlt, faEllipsisH, faInfoCircle, faSignOutAlt, faSignInAlt, faUserPlus, faQuestionCircle,
    faAlignJustify, faEdit, faTools);

Vue.component("fa", FontAwesomeIcon);