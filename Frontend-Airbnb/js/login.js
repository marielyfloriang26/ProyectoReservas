document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const email = document.getElementById('correo').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('http://localhost:5189/api/Auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });

        if (response.ok) {
            const data = await response.json();
            
            // GUARDAR EL TOKEN: Esto es lo más importante
            localStorage.setItem('token', data.token);
            localStorage.setItem('usuario', JSON.stringify(data.usuario));

            alert('¡Bienvenido de nuevo!');
            window.location.href = 'index.html'; // Nos vamos al inicio
        } else {
            const errorText = await response.text();
            alert('Error al iniciar sesión: ' + errorText);
        }
    } catch (err) {
        console.error(err);
        alert('No se pudo conectar con el servidor.');
    }
});