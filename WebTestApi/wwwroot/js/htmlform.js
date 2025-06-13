class HtmlForm extends HtmlTemplate
{
    constructor(id,rootView)
    {
        super(id, "script");
        this.rootView = rootView;
    }


    static get_data(form)
    {
        const data = {};
        const elements = form.elements;

        for (let i = 0; i < elements.length; i++)
        {
            const el = elements[i];
            if (!el.name || el.disabled) continue;

            if (el.dataset.type === "datetime")
            {
                data[el.name] = parseToISO(el.value);
                continue;
            }


            switch (el.type)
            {
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

    static def_form(rootElement)
    {
        const form = rootElement.querySelector("form");


        if (form && !form.context)
        {
            form.context = { view: this.rootView, modal: new bootstrap.Modal(rootElement) };
            form.addEventListener("submit", async function ()
            {
                event.preventDefault();

                try
                {
                    const response = await fetchResponse(this.dataset.method || this.method, this.action, HtmlTemplate.get_data(form));

                    if (response.ok)
                    {
                        const data = await response.json();

                        const view = this.context.view; 

                        if (view)
                        {
                            view.refresh(data);
                        }

                        const modal = this.context.modal;

                        if (modal)
                        {
                            modal.hide();
                            return;
                        }
                    }

                    const err = await response.json();

                    if (err)
                    {
                        applyValidationErrors(this, err);
                    }

                }
                catch (error)
                {
                    console.error("Ошибка при загрузке данных:", error);
                    alert("Ошибка при получении данных: " + error.message);
                }
            });

        }

        return form;

    }


    async get(getURI)
    {
       
        try
        {
            const data = await fetchRestApi('GET', getURI);
            this.renderBody(data);
        }
        catch (err)
        {

        }

    }

    async show(saveMETHOD, saveURI)
    {
        const form = HtmlTemplate.def_form(this.rootElement);

        if (form)
        {
            form.action = saveURI;
            form.method = formMETHOD;
            form.context.modal.show();
        }

       
    }

}