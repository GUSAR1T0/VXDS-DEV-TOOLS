export default function() {
    let firstYear = 2019;
    let currentYear = new Date().getFullYear();
    return `${firstYear + (currentYear !== firstYear ? `-${currentYear}` : "")}`;
}