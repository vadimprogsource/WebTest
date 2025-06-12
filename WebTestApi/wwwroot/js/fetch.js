

async function fetchResponse(method, url, body)
{
    const token = localStorage.getItem('jwt');

    const response = await fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(body)
    });

    return response;

}

async function fetchRest(method, url, body)
{
    const response = await fetchResponse(method, url, body);

    if (!response.ok)
    {
        throw new Error(`HTTP error ${response.status}`);
    }

    return response;

}

async function fetchRestApi(method, url, body)
{
    const response = await fetchRest(method, url, body);
    const data = await response.json();
    return data;

}

async function del(commandURI, callBack)
{
    try {
        if (!confirm("Удалить ?")) return;

        await fetchRest('DELETE', commandURI);
        if (callBack) callBack();
    }
    catch (error) {
        console.error("Ошибка при удалении данных:", error);
        alert("Ошибка при удалении данных:" + error.message);
    }
}