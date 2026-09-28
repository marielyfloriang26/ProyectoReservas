const API_URL = 'http://localhost:5189/api/Property';

async function loadProperties() {
    const grid = document.getElementById('propertiesGrid');
    try {
        const response = await fetch(API_URL);
        const properties = await response.json();

        grid.innerHTML = properties.map(p => `
            <div class="property-card"
            style="cursor: pointer;" 
                 onclick="window.location.href='detalle.html?id=${p.id}'">
                <img src="assets/fondo-registro.jpg" alt="${p.titulo}">
                <div class="card-info">
                    <h3>${p.titulo}</h3>
                    <p>${p.ubicacion || 'Punta Cana, RD'}</p>
                    <p class="price"><strong>$${p.precioNoche} USD</strong> / noche</p>
                    <small style="color: #717171;">Ver detalles</small>
                </div>
            </div>
        `).join('');
    } catch (e) {
        grid.innerHTML = "<p>Verifica que el Backend esté encendido.</p>";
    }
}

// Abrir y cerrar el modal
function mostrarFormularioPublicar() {
    document.getElementById('modalPublicar').style.display = 'block';
}

function cerrarModal() {
    document.getElementById('modalPublicar').style.display = 'none';
}

// Manejar el envío del formulario
document.getElementById('formPublicar').addEventListener('submit', async (e) => {
    e.preventDefault();

    const nuevaPropiedad = {
        Titulo: document.getElementById('pubTitulo').value,
        Ubicacion: document.getElementById('pubUbicacion').value,
        PrecioNoche: parseFloat(document.getElementById('pubPrecio').value),
        Descripcion: document.getElementById('pubDescripcion').value,
        OwnerId: 1 // Tu ID de usuario
    };

    try {
        const response = await fetch('http://localhost:5189/api/Property', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(nuevaPropiedad)
        });

        if (response.ok) {
            alert("¡Propiedad publicada con éxito!");
            cerrarModal();
            loadProperties(); // Refresca la lista de una vez
        } else {
            const error = await response.text();
            alert("Error del servidor: " + error);
        }
    } catch (err) {
        alert("No se pudo conectar con el servidor.");
    }

    // 1. Función para mostrar/ocultar el menú al hacer clic
function toggleMenu() {
    document.getElementById("userDropdown").classList.toggle("show");
}

// 2. Función para Cerrar Sesión
function logout() {
    // Borramos el Token y los datos del usuario del navegador
    localStorage.removeItem('token');
    localStorage.removeItem('user'); // Si guardaste el nombre o ID

    alert("Sesión cerrada correctamente");
    
    // Redirigir al inicio o al login
    window.location.href = 'index.html'; 
}

// Cerrar el menú si el usuario hace clic fuera de él
window.onclick = function(event) {
    if (!event.target.matches('.user-icon') && !event.target.matches('.fa-user')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        for (var i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}


});

document.addEventListener('DOMContentLoaded', loadProperties);