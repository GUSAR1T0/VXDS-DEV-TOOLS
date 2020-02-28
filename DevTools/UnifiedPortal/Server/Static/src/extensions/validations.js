export default class SignUpValidations {
    constructor(ruleForm) {
        this.ruleForm = ruleForm;
    }

    validatePassword = (rule, value, callback) => {
        if (value === "") {
            callback(new Error("Please, input the password"));
        } else if (value.length < 8) {
            callback(new Error("Please, input the password that has more than 8 symbols"));
        } else {
            callback();
        }
    };

    validatePasswordConfirmation = (rule, value, callback) => {
        if (value === "") {
            callback(new Error("Please, confirm the password"));
        } else if (value !== this.ruleForm.password) {
            callback(new Error("Password inputs don't match"));
        } else {
            callback();
        }
    };
}
