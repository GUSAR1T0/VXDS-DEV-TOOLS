import Vue from "vue";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
    faEllipsisH,
    faServer,
    faSignInAlt,
    faSignOutAlt,
    faUserAlt,
    faUserCircle,
    faUserPlus,
    faAddressCard,
    faTrashAlt,
    faPlusCircle,
    faFilter,
    faTable,
    faStickyNote
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(
    faUserCircle, faUserAlt, faEllipsisH, faSignOutAlt, faSignInAlt, faUserPlus, faServer, faAddressCard, faTrashAlt,
    faPlusCircle, faFilter, faTable, faStickyNote
);

Vue.component("fa", FontAwesomeIcon);