class forkEdit extends HtmlForm
{
    constructor(id,view)
    {
        super(id, "script");
        this.rootView = view;
    }

    async createNew()
    {
        await this.get("fork/lift/new");
        this.show("PUT", "fork/lift");
    }

    async edit(id)
    {
        await this.get("fork/lift/"+id);
        this.show("PATCH", "fork/lift");
    }

    async delete(id)
    {

    }
}
