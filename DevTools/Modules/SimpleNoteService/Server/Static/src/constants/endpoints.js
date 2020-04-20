// Account endpoints
export const SIGN_IN_ENDPOINT = "account/sign-in";
export const SIGN_UP_ENDPOINT = "account/sign-up";
export const REFRESH_ENDPOINT = "account/refresh";
export const LOGOUT_ENDPOINT = "account/logout";
export const GET_USER_DATA_ENDPOINT = "account";

// Lookup endpoints
export const GET_LOOKUP_VALUES_ENDPOINT = "lookup/values";

// User endpoints
export const GET_PROFILE_ENDPOINT = "user/{id}";
export const SEARCH_USERS_ENDPOINT = "user/search?p={pattern}&z={zeroUserName}";

// Project endpoints
export const SEARCH_PROJECTS_ENDPOINT = "project/search?p={pattern}";

// Folder & Note endpoints
export const GET_FOLDER_LIST_ENDPOINT = "folder/list";
export const UPDATE_FOLDER_POSITIONS_ENDPOINT = "folder/positions";
export const GET_FOLDER_NAME_ENDPOINT = "folder/{folderId}";
export const GET_AFFECTED_NOTE_FOLDERS_COUNTS_BY_FOLDER_ENDPOINT = "folder/{folderId}/affected/count";
export const CREATE_NEW_FOLDER_ENDPOINT = "folder/{folderId}/new";
export const UPDATE_FOLDER_ENDPOINT = "folder/{folderId}?n={name}";
export const DELETE_FOLDER_ENDPOINT = "folder/{folderId}";
export const GET_NOTE_LIST_ENDPOINT = "folder/{folderId}/note/list";
export const GET_NOTE_ENDPOINT = "folder/{folderId}/note/{noteId}";
export const CHANGE_NOTE_FOLDER_ENDPOINT = "folder/{folderId}/note/{noteId}/replace?f={newFolderId}";
export const CREATE_NOTE_ENDPOINT = "folder/{folderId}/note";
export const UPDATE_NOTE_ENDPOINT = "folder/{folderId}/note/{noteId}";
export const DELETE_NOTE_ENDPOINT = "folder/{folderId}/note/{noteId}";
