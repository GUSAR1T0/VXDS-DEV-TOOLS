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
    faTools,
    faUsers,
    faExternalLinkAlt,
    faEnvelope,
    faMinusCircle,
    faPlusCircle,
    faCogs,
    faUsersCog,
    faFileAlt,
    faServer,
    faCodeBranch,
    faCode,
    faLock,
    faLockOpen
} from "@fortawesome/free-solid-svg-icons";
import { faGithub } from "@fortawesome/free-brands-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(
    faUserCircle, faUserAlt, faEllipsisH, faInfoCircle, faSignOutAlt, faSignInAlt, faUserPlus, faQuestionCircle,
    faAlignJustify, faEdit, faTools, faUsers, faExternalLinkAlt, faEnvelope, faMinusCircle, faPlusCircle, faCogs,
    faUsersCog, faFileAlt, faServer, faGithub, faCodeBranch, faCode, faLock, faLockOpen
);

Vue.component("fa", FontAwesomeIcon);