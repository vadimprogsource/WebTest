class HtmlForm extends HtmlTemplate
{
    constructor(id,rootView)
    {
        super(id, "script");
        this.rootView = rootView;
    }


    static getFormData(form)
    {

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

    async edit(saveMETHOD, saveURI)
    {
        const form = this.rootElement.querySelector("form");
        const caller = this.rootView;

        if (form)
        {

            if (!form.context)
            {
                form.context = { action: saveURI, method: saveMETHOD, view: this.rootView };
            }


            form.action = saveURI;
            form.dataset.method = saveMETHOD;

            form.addEventListener("submit", async function ()
            {
                event.preventDefault();

                try
                {
                    const response = await fetchResponse(this.dataset.formMethod, this.action, getFormData(form));

                    if (response.ok)
                    {
                        const data = await response.json();

                        const view = this.context.view; 

                        if (view)
                        {
                            view.refresh(data);
                        }

                        const modal = this.closest(".modal");

                        if (modal)
                        {
                            bootstrap.Modal.getInstance(modal).hide();
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


        new bootstrap.Modal(this.rootElement).show();
    }

}