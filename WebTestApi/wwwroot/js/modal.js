const modal =
{
  async  show(getURI, windowID, saveMETHOD, saveURI, callBack)
  {
    try {
        const data = await fetchRestApi('GET', getURI);
        const win = document.getElementById(windowID);


        if (win)
        {
            var form = win.getElementsByTagName("FORM");
            if (form && form.length && (form = form[0])) {
                form.method = saveMETHOD;
                form.dataset.formMethod = saveMETHOD;
                form.action = saveURI;
                form.dataset.formUri = saveURI;
                form.callBack = callBack;
                setFormData(form, data);
                clearValidationErrors(form);
            }
            new bootstrap.Modal(win).show();
        }
        

    } catch (error) {
        console.error("Ошибка при загрузке неисправностей:", error);
        alert("Ошибка при получении данных: " + error.message);
    }

},

close(el)
{
    const modal = el.closest(".modal");

    if (modal)
    {
        bootstrap.Modal.getInstance(modal).hide();
        return true;
    }
    }
};