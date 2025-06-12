
function getFormData(form)
{
    const data = {};
    const elements = form.elements;

    for (let i = 0; i < elements.length; i++) {
        const el = elements[i];
        if (!el.name || el.disabled) continue;

        if (el.dataset.type === "datetime") {
            data[el.name] = parseToISO(el.value);
            continue;
        }



        switch (el.type) {
            case 'checkbox':
                data[el.name] = el.checked;
                break;
            case 'radio':
                if (el.checked) data[el.name] = el.value;
                break;
            case 'number':
                data[el.name] = el.value ? parseFloat(el.value) : null;
                break;
            case 'datetime':
                data[el.name] = el.value || null;
                break;
            default:
                data[el.name] = el.value;

        }
    }

    return data;
}



function setFormData(form, data)
{
    for (const [key, value] of Object.entries(data)) {
        const input = form.querySelector(`[name="${key}"]`);

        if (!input) continue;

        const type = input.type;

        if (type === 'datetime' || input.dataset.type === 'datetime') {
            if (value && value !== 'null') input.value = formatDateTime(value); else input.value = '';
            continue;
        }

        if (type === 'checkbox') {
            input.checked = Boolean(value);
        } else if (type === 'radio') {
            const radioToCheck = form.querySelector(`[name="${key}"][value="${value}"]`);
            if (radioToCheck) radioToCheck.checked = true;
        } else {
            input.value = value ?? '';
        }
    }
}

async function postSubmit(form)
{


    event.preventDefault();

    try {
        const response = await fetchResponse(form.dataset.formMethod, form.action, getFormData(form));

        if (response.ok)
        {

            const data = await response.json();

            if (form.callBack)
            {
                form.callBack(data);
            }

            return modal.close(form);
      
        }

        const err = await response.json();
        if (err)
        {
            applyValidationErrors(form, err);
        }

    }
    catch (error)
    {
        console.error("Ошибка при загрузке неисправностей:", error);
        alert("Ошибка при получении данных: " + error.message);
    }
}

