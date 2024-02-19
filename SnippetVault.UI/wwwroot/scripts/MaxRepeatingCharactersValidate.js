jQuery.validator.addMethod("max-repeating-characters",
    function (value, element, params) {
        console.log(params.targetValue);

        let repeatedCount = 1;

        for (let i = 0; i < value.length; i++) {
            if (i == 0) {
                continue;
            }

            if (value[i] == value[i - 1]) {
                repeatedCount++;
                if (repeatedCount > params.targetValue) {
                    return false;
                }
            }
            else {
                repeatedCount = 1;
            }
        }

        return true;
    });
jQuery.validator.unobtrusive.adapters.add("max-repeating-characters", ["value"], function (options) {
    options.rules['max-repeating-characters'] = { targetValue: options.params.value };
    options.messages['max-repeating-characters'] = options.message;
});