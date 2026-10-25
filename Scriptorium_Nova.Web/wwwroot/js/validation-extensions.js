$.validator.addMethod("mustcontainletter", function (value, element) {
    return this.optional(element) || /\p{L}/u.test(value);
});

$.validator.unobtrusive.adapters.addBool("mustcontainletter");