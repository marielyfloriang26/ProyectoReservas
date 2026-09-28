document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const nombre = document.getElementById('nombre').value;
    const email = document.getElementById('correo').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('http://localhost:5189/api/Auth/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nombre, email, password })
        });

        if (response.ok) {
            alert('¡Registro exitoso! Revisa tu correo para el código de verificación.');
            // Guardamos el correo para usarlo en la pantalla de verificación
            localStorage.setItem('email_verificar', email);
            window.location.href = 'verify.html';
        } else {
            const error = await response.text();
            alert('Error: ' + error);
        }
    } catch (err) {
        console.error(err);
        alert('No se pudo conectar con el servidor.');
    }
});
/*document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const nombre = document.getElementById('nombre').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const btn = document.getElementById('btnRegister');
    const messageDiv = document.getElementById('message');

    btn.innerText = "Cargando...";
    btn.disabled = true;

    try {
        const response = await fetch('http://localhost:5189/api/Auth/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nombre, email, password })
        });

        const data = await response.json();

        if (response.ok) {
            // Guardamos el email en localStorage para usarlo en la siguiente pantalla
            localStorage.setItem('userEmail', email);
            messageDiv.innerHTML = `<p style="color: green;">${data.message}</p>`;
            
            // Redirigir a verificación después de 2 segundos
            setTimeout(() => {
                window.location.href = 'verify.html';
            }, 2000);
        } else {
            messageDiv.innerHTML = `<p style="color: red;">${data}</p>`;
        }
    } catch (error) {
        messageDiv.innerHTML = `<p style="color: red;">Error de conexión con la API</p>`;
    } finally {
        btn.innerText = "Registrarse";
        btn.disabled = false;
    }
});*/