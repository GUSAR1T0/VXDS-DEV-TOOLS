import { RESET_PROJECT_STORE_STATE, STORE_PROJECT_DATA_REQUEST } from "@/constants/actions";

export default {
    state: {
        project: {
            id: 0,
            name: "",
            alias: "",
            description: "",
            isActive: null,
            gitHubRepoId: null,
            gitHubRepository: {
                id: 0,
                fullName: "",
                private: true,
                owner: {
                    login: "",
                    avatarUrl: "",
                    profileUrl: ""
                },
                repositoryUrl: "",
                description: "",
                stargazersCount: 0,
                forksCount: 0,
                openIssuesCount: 0,
                license: "",
                languages: {}
            }
        }
    },
    getters: {
        getProject: state => {
            return state.project;
        }
    },
    mutations: {
        [STORE_PROJECT_DATA_REQUEST]: (state, project) => {
            state.project.id = project ? project.id : 0;
            state.project.name = project ? project.name : "";
            state.project.alias = project ? project.alias : "";
            state.project.description = project ? project.description : "";
            state.project.isActive = project ? project.isActive : null;
            state.project.gitHubRepoId = project ? project.gitHubRepoId : null;
            state.project.gitHubRepository.id = project && project.gitHubRepository ? project.gitHubRepository.id : 0;
            state.project.gitHubRepository.fullName = project && project.gitHubRepository ? project.gitHubRepository.fullName : "";
            state.project.gitHubRepository.private = project && project.gitHubRepository ? project.gitHubRepository.private : false;
            state.project.gitHubRepository.owner.login = project && project.gitHubRepository && project.gitHubRepository.owner ? project.gitHubRepository.owner.login : "";
            state.project.gitHubRepository.owner.avatarUrl = project && project.gitHubRepository && project.gitHubRepository.owner ? project.gitHubRepository.owner.avatarUrl : "";
            state.project.gitHubRepository.owner.profileUrl = project && project.gitHubRepository && project.gitHubRepository.owner ? project.gitHubRepository.owner.profileUrl : "";
            state.project.gitHubRepository.repositoryUrl = project && project.gitHubRepository ? project.gitHubRepository.repositoryUrl : "";
            state.project.gitHubRepository.description = project && project.gitHubRepository ? project.gitHubRepository.description : "";
            state.project.gitHubRepository.stargazersCount = project && project.gitHubRepository ? project.gitHubRepository.stargazersCount : 0;
            state.project.gitHubRepository.forksCount = project && project.gitHubRepository ? project.gitHubRepository.forksCount : 0;
            state.project.gitHubRepository.openIssuesCount = project && project.gitHubRepository ? project.gitHubRepository.openIssuesCount : 0;
            state.project.gitHubRepository.license = project && project.gitHubRepository ? project.gitHubRepository.license : "";
            state.project.gitHubRepository.languages = project && project.gitHubRepository ? project.gitHubRepository.languages : {};
        },
        [RESET_PROJECT_STORE_STATE]: state => {
            state.project = {
                id: 0,
                name: "",
                alias: "",
                description: "",
                isActive: null,
                gitHubRepoId: null,
                gitHubRepository: {
                    id: 0,
                    fullName: "",
                    private: true,
                    owner: {
                        login: "",
                        avatarUrl: "",
                        profileUrl: ""
                    },
                    repositoryUrl: "",
                    description: "",
                    stargazersCount: 0,
                    forksCount: 0,
                    openIssuesCount: 0,
                    license: "",
                    languages: {}
                }
            };
        }
    }
};
