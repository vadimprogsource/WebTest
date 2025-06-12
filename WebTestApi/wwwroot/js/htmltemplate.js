class HtmlTemplate
{
    constructor(id, tagName)
    {
        const root = document.getElementById(id);
        if (!root) throw new Error(`Element with id "${id}" not found`);
        this.rootElement = root;

        const templateElement = root.querySelector(tagName);
        if (!templateElement) throw new Error(`Element with id "${id}" must have a ${tagName}`);

        this.templateElement  = Handlebars.compile(templateElement.innerHTML);
        this.containerElement = templateElement.parentNode;

        templateElement.remove();
    }

    elementAt(model,index)
    {
        return model[index];
    }


    renderBody(model)
    {
        const template = this.templateElement;
        let html = "";


        if (Array.isArray(model))
        {
            for (let i = 0; i < model.length; i++)
            {
                html += template(this.elementAt(model,i)); 
            }
        }
        else
        {
            html = template(model);
        }

        this.containerElement.innerHTML = html;
    }
}
