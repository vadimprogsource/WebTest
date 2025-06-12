function clearValidationErrors(form)
{
    const elements = form.querySelectorAll('input[name], select[name], textarea[name]');

    for (let i = 0; i < elements.length; i++) {
        const el = elements[i];
        el.classList.remove('is-invalid');
        el.classList.remove('is-valid');
        el.is_error = undefined;

        const next = el.nextElementSibling;
        if (next && next.classList.contains('invalid-feedback')) {
            next.remove();
        }

        const label = form.querySelector(`label[for="${el.id}"], label[for="${el.name}"]`);
        if (label) {
            label.classList.remove('text-danger');
        }
    }
}

function applyValidationErrors(form, errors)
{
    const elements = form.querySelectorAll('input[name], select[name], textarea[name]');

    for (let i = 0; i < elements.length; i++) {
        const el = elements[i];
        const name = el.name;
        if (!name) continue;



        const label = form.querySelector(`label[for="${el.id}"], label[for="${name}"]`);
        const error = errors[name];


        const next = el.nextElementSibling;

        if (next && next.classList.contains('invalid-feedback')) {
            next.remove();
        }

        if (error)
        {
            el.classList.remove('is-valid');
            el.classList.add('is-invalid');
            el.is_error = true;

            if (label)
            {
                label.classList.add('text-danger');
            }

            const reason = extractReason(error);
            if (!reason) continue;

            const feedback = document.createElement('div');
            feedback.className = 'invalid-feedback';
            feedback.textContent = reason;
            el.insertAdjacentElement('afterend', feedback);
            continue;
        }

        if (label)
        {
            label.classList.remove('text-danger');
        }

        el.classList.remove('is-invalid');
        if (el.is_error)
        {
            el.is_error = false;
            el.classList.add('is-valid');

        }

    }
}


function extractReason(errorObj)
{
    while (errorObj) {
        if (errorObj.reason && errorObj.reason.trim()) return errorObj.reason;
        errorObj = errorObj.inner;
    }
    return '';
}
