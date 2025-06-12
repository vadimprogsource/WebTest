class FaultView extends HtmlTemplate
{
    constructor(table,number)
    {
        super(table, "script");
        this.number = document.getElementById(number);
        this.owner = null; 
    }


    elementAt(model, index)
    {
        const item = model[index];
        item.index = 1 + index;
        return item;
    }


    async load(x)
    {

        try
        {
            this.owner = x;

            if (!x)
            {
                this.number.innerHTML = "";
                this.renderBody({});
                return;
            }

            const data = await fetchRestApi('POST', `fork/lift/fault/${x.id}`, { maxCount: 100 });
            this.renderBody(data);
            this.number.innerHTML = x.number; 
        }
        catch (err)
        {
            console.error('Ошибка при загрузке:', err); // ← логируем ошибку для отладки
        }
    }

    async refresh()
    {
        return await load(this.owner);
    }

};
