const auth =
{

    async signIn(login, password)
    {
        try {
            const response = await fetchRest('POST', '/auth/signin', { login, password });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('jwt', data.token);
                return true;
            }
        } catch (err) {
            console.error('Ошибка при авторизации:', err);
        }
    },

    async signOut()
    {
        try {
            await fetchRest("POST", "/auth/signout");
            localStorage.removeItem('jwt');
        } catch (err) {
            console.error('Ошибка при выходе:', err);
        }
        window.location.reload();
    }

};
