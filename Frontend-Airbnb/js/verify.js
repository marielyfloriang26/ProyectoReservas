document.getElementById('verifyForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const codigo = document.getElementById('codigo').value;
    const email = localStorage.getItem('email_verificar');

    if (!email) {
        alert("No se encontró el correo. Por favor, regístrate de nuevo.");
        window.location.href = 'register.html';
        return;
    }

    try {
        const response = await fetch('http://localhost:5189/api/Auth/confirmar', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, codigo })
        });

        if (response.ok) {
            alert('¡Cuenta activada con éxito! Ahora puedes iniciar sesión.');
            window.location.href = 'login.html';
        } else {
            const error = await response.text();
            alert('Error al verificar: ' + error);
        }
    } catch (err) {
        console.error(err);
        alert('Hubo un problema al conectar con el servidor.');
    }
});