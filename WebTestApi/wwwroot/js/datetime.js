
function formatDateTime(dateString)
{
    const date = new Date(dateString);

    const pad = n => n.toString().padStart(2, '0');

    const formatted =
        pad(date.getDate()) + '.' +
        pad(date.getMonth() + 1) + '.' +
        date.getFullYear() + ' ' +
        pad(date.getHours()) + ':' +
        pad(date.getMinutes());

    return formatted;
}

function parseToISO(dateStr)
{
    try {

        const [datePart, timePart] = dateStr.split(' ');
        const [day, month, year] = datePart.split('.');
        const [hours, minutes] = timePart.split(':');
        const isoDate = new Date(
            Number(year),
            Number(month) - 1,
            Number(day),
            Number(hours),
            Number(minutes)
        );
        return isoDate.toISOString();

    }
    catch
    {
        return null;
    }


}

Handlebars.registerHelper("dateTime", function (value) {
    if (value) {
        return formatDateTime(value);
    }

    return "-";
});

