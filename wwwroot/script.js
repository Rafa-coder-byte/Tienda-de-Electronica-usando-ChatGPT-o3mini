let productos = [];
let productoSeleccionado = null;
let carrito = [];


// Función para actualizar el contador del carrito en el DOM


// Configurar Event Listeners
document.addEventListener('DOMContentLoaded', () => {

    // Cargar el carrito desde localStorage
    const carritoGuardado = localStorage.getItem('carrito');
    if (carritoGuardado) {
        carrito = JSON.parse(carritoGuardado);
        actualizarContadorCarrito();
        renderizarCarrito();
    }
    function actualizarContadorCarrito() {
        const contador = document.getElementById('carrito-count');
        if (contador) {
            contador.textContent = carrito.length;
        }
    }

    // Función para renderizar el contenido del carrito
    function renderizarCarrito() {
        const contenedor = document.getElementById('carrito-items');
        if (!contenedor) return;

        contenedor.innerHTML = ''; // Limpiar contenido anterior
        let total = 0;

        carrito.forEach(item => {
            total += parseFloat(item.price);
            const div = document.createElement('div');
            div.className = 'carrito-item';
            div.innerHTML = `
            <span>${item.name} - $${item.price}</span>
            <button data-id="${item.id}" class="btn-eliminar">Eliminar</button>
        `;
            contenedor.appendChild(div);
        });

        const totalElemento = document.getElementById('carrito-total');
        if (totalElemento) {
            totalElemento.textContent = total.toFixed(2);
        }
    }

    // Función para agregar un producto al carrito
    function agregarAlCarrito() {
        // Obtener el ID del producto desde la vista Details
        const productId = parseInt(window.location.pathname.split('/').pop()); // Extrae el ID de la URL
        productoSeleccionado = productos.find(p => p.id === productId);

        if (productoSeleccionado) {
            carrito.push(productoSeleccionado);
            localStorage.setItem('carrito', JSON.stringify(carrito)); // Guardar en localStorage
            actualizarContadorCarrito();
            renderizarCarrito();
            alert(`${productoSeleccionado.name} se agregó al carrito.`);
        }
        else {
            console.error("No se pudo obtener el producto");
        }
    }

    // Función para eliminar un producto del carrito
    function eliminarDelCarrito(id) {
        carrito = carrito.filter(item => item.id !== id);
        actualizarContadorCarrito();
        renderizarCarrito();
    }

    // Obtener productos desde el API y renderizarlos
    function fetchProducts() {
        fetch('/api/products')
            .then(response => response.json())
            .then(data => {
                productos = data;
                renderizarProductos(productos);
            });
    }

    function renderizarProductos(lista) {
        const contenedor = document.getElementById('productos-container');
        contenedor.innerHTML = ''; // Limpiar productos previos

        if (lista.length === 0) {
            contenedor.innerHTML = '<p>No se encontraron productos.</p>';
            return;
        }

        lista.forEach(prod => {
            const div = document.createElement('div');
            div.className = 'producto';
            div.innerHTML = `
      <img src="${prod.imageUrl}" alt="${prod.name}">
      <h3>${prod.name}</h3>
      <p>${prod.description}</p>
      <p><strong>Precio: </strong>$${prod.price}</p>
      <button data-id="${prod.id}">Ver detalles</button>
    `;
            contenedor.appendChild(div);
        });
    }

    // Cerrar modal
    function cerrarModal(modalId) {
        document.getElementById(modalId).style.display = 'none';
    }



    // Buscar productos
    function buscarProductos(termino) {
        const filtrados = productos.filter(prod =>
            prod.name.toLowerCase().includes(termino.toLowerCase()) ||
            prod.description.toLowerCase().includes(termino.toLowerCase())
        );
        renderizarProductos(filtrados);
    }

    fetchProducts();

    // Agregar al carrito desde la página de detalles
    const btnAgregarCarrito = document.getElementById('btn-agregar-carrito');
    if (btnAgregarCarrito) {
        btnAgregarCarrito.addEventListener('click', agregarAlCarrito);
    }


    // Abrir modal de producto al hacer clic en "Ver detalles"
    document.getElementById('productos-container').addEventListener('click', (e) => {
        if (e.target.tagName === 'BUTTON') {
            console.log("Botón clickeado, redirigiendo...");
            const id = parseInt(e.target.getAttribute('data-id'));
            // Redirecciona a la vista de detalles del producto
            window.location.href = `/ProductsDetails/Details/${id}`;
        }
    });


    // Cerrar modal de producto
    document.querySelector('#modal-producto .cerrar').addEventListener('click', () => {
        cerrarModal('modal-producto');
    });

    // Agregar al carrito desde el modal
    document.getElementById('btn-agregar-carrito').addEventListener('click', agregarAlCarrito);

    // Abrir modal del carrito
    document.getElementById('ver-carrito').addEventListener('click', (e) => {
        e.preventDefault();
        console.log("Se hizo clic en el enlace del carrito");  // Verifica que el evento se dispara
        renderizarCarrito();
        document.getElementById('modal-carrito').style.display = 'block';
    });


    // Cerrar modal del carrito
    document.getElementById('cerrar-carrito').addEventListener('click', () => {
        cerrarModal('modal-carrito');
    });

    // Eliminar producto del carrito
    document.getElementById('carrito-items').addEventListener('click', (e) => {
        if (e.target.classList.contains('btn-eliminar')) {
            const id = parseInt(e.target.getAttribute('data-id'));
            eliminarDelCarrito(id);
        }
    });

    // Buscar productos al hacer clic en "Buscar"
    document.getElementById('search-btn').addEventListener('click', () => {
        const termino = document.getElementById('search-input').value;
        buscarProductos(termino);
    });

    // Buscar al presionar Enter en el input de búsqueda
    document.getElementById('search-input').addEventListener('keyup', (e) => {
        if (e.key === 'Enter') {
            buscarProductos(e.target.value);
        }
    });
});

// Cerrar modales al hacer clic fuera del contenido
window.addEventListener('click', (e) => {
    const modalProducto = document.getElementById('modal-producto');
    const modalCarrito = document.getElementById('modal-carrito');
    if (e.target === modalProducto) {
        cerrarModal('modal-producto');
    }
    if (e.target === modalCarrito) {
        cerrarModal('modal-carrito');
    }
});
