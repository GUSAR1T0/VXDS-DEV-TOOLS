import { getTokens } from "@/extensions/tokens";
import randomColor from "randomcolor";

export function getYearsForFooter() {
    let firstYear = 2019;
    let currentYear = new Date().getFullYear();
    return `${firstYear + (currentYear !== firstYear ? `-${currentYear}` : "")}`;
}

export function getConfiguration(notSavedAccessToken = null) {
    let headers = {};

    const setToken = (token) => {
        headers["Authorization"] = `Bearer ${token}`;
    };
    if (notSavedAccessToken) {
        setToken(notSavedAccessToken);
    } else {
        let {accessToken} = getTokens();
        if (accessToken) {
            setToken(accessToken);
        }
    }

    return {headers};
}

export function generateColor() {
    return randomColor({
        luminosity: "dark",
        format: "rgba",
        alpha: 0.75
    });
}

export function getUserFullName(firstName, lastName) {
    return `${firstName} ${lastName}`;
}

export function getUserInitials(firstName, lastName) {
    return (firstName.substr(0, 1) + lastName.substr(0, 1)).toUpperCase();
}
