const setDefaultDate = fieldQuerySelector => {
    $(fieldQuerySelector).val(moment().format("DD/MM/YYYY"));
};

const setReadOnly = fieldQuerySelector => {
    $(fieldQuerySelector).prop('readonly', true);
};

const validateDateFormField = fieldQuerySelector => {
    return $(fieldQuerySelector).change(() => {
        const informedDate = $(fieldQuerySelector).val();

        if (!moment(informedDate, "DD/MM/YYYY").isValid()) {
            setInvalidField(fieldQuerySelector);
        } else {
            cleanInvalidField(fieldQuerySelector);
        }
    });
};

const validateNumericRequiredFormField = fieldQuerySelector => {
    return $(fieldQuerySelector).change(() => {
        const value = $(fieldQuerySelector).val();

        if (!/^\d+$/g.test(value.trim())) {
            setInvalidField(fieldQuerySelector);
        } else {
            cleanInvalidField(fieldQuerySelector);
        }
    });
};

const validateRequiredFormField = fieldQuerySelector => {
    return $(fieldQuerySelector).change(() => {
        const value = $(fieldQuerySelector).val();

        if (!value) {
            setInvalidField(fieldQuerySelector);
        } else {
            cleanInvalidField(fieldQuerySelector);
        }
    });
};

const setInvalidField = fieldQuerySelector => {
    $(fieldQuerySelector).css("border-color", "red");
};

const cleanInvalidField = fieldQuerySelector => {
    $(fieldQuerySelector).css("border-color", "");
};

