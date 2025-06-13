        class ForkView extends HtmlTemplate
        {
            constructor(table)
            {
                super(table, "script");
                this.maxCount = 100;
                this.selected = null; // ← добавил, чтобы явно инициализировать
                this.detailView = null;

        
            }

            elementAt(model, i)
            {
                const obj = model[i];
                obj.index = 1 + i;

                if (this.selected && this.selected.id === obj.id)
                {
                    obj.selected = true;
                }
                return obj; 
            }

            async refreshDetails()
            {
                const detailView = this.detailView;
                const selected = this.selected;
                if (detailView && this.selected)
                {
                    await detailView.load(selected);
                }
            }


            async select(tr)
            {
                const body = tr.parentNode.children;
                for (let i = 0; i < body.length; i++)
                {
                    body[i].classList.remove("active");
                }

                tr.classList.add("active");
                this.selected = this.getSelected(tr.dataset);
                await this.refreshDetails();
            }


            getSelected(x,data)
            {
                if (x)
                {
                    return { id: x.id, number: x.number }; 
                }
                if (data && data.length)
                {
                    x = data[0];
                    return { id: x.id, number: x.number }; 
                }
                return this.selected;
            } 
            
            async search(text)
            {
                try
                {
                    this.search_data =
                    {
                        searchPattern: text,
                        maxCount: this.maxCount
                    }; 

                    const data = await fetchRestApi('POST', '/fork/lift', this.search_data);

                    this.selected = this.getSelected(null, data);
                  
                    this.renderBody(data);
                    await this.refreshDetails();
       

                }
                catch (err)
                {
                    console.error('Ошибка при поиске:', err); // ← логируем ошибку для отладки
                }
            }

            async refresh(x)
            {
                try
                {
                    const data = await fetchRestApi('POST', '/fork/lift', this.search_data);
                    this.selected = this.getSelected(x, data);
                    this.renderBody(data);
                    await this.refreshDetails();
                }
                catch (err)
                {
                    alert('Ошибка при поиске:', err);
                    console.error('Ошибка при поиске:', err); // ← логируем ошибку для отладки

                }
                   
            }





  async setActive(el)
  {
       try
       {
        //event.preventDefault();
        event.stopPropagation();
        const data = await fetchRestApi("PATCH", "fork/lift/active/" + el.value + "/" + el.checked, {});
        el.checked = data.isActive;
        this.refresh(data);
    }
    catch (error)
    {
        console.error("Ошибка  данных:", error);
        alert("Ошибка данных:" + error.message);
    }
   
}


        };
