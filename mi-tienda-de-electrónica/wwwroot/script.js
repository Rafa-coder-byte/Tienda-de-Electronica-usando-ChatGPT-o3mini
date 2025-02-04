let productos = [];
let carrito = [];
let productoSeleccionado = null;

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

// Abrir modal de producto
function abrirModalProducto(id) {
    productoSeleccionado = productos.find(p => p.id === id);
    if (!productoSeleccionado) return;

    document.getElementById('modal-nombre').textContent = productoSeleccionado.name;
    document.getElementById('modal-descripcion').textContent = productoSeleccionado.description;
    document.getElementById('modal-precio').textContent = productoSeleccionado.price;
    document.getElementById('modal-imagen').src = productoSeleccionado.imageUrl;

    document.getElementById('modal-producto').style.display = 'block';
}

// Cerrar modal
function cerrarModal(modalId) {
    document.getElementById(modalId).style.display = 'none';
}

// Actualizar contador del carrito
function actualizarContadorCarrito() {
    document.getElementById('carrito-count').textContent = carrito.length;
}

// Renderizar carrito
function renderizarCarrito() {
    const contenedor = document.getElementById('carrito-items');
    contenedor.innerHTML = '';

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
    document.getElementById('carrito-total').textContent = total.toFixed(2);
}

// Agregar producto al carrito
function agregarAlCarrito() {
    if (productoSeleccionado) {
        carrito.push(productoSeleccionado);
        actualizarContadorCarrito();
        cerrarModal('modal-producto');
        alert(`${productoSeleccionado.name} se agregó al carrito.`);
    }
}

// Eliminar producto del carrito
function eliminarDelCarrito(id) {
    carrito = carrito.filter(item => item.id !== id);
    actualizarContadorCarrito();
    renderizarCarrito();
}

// Buscar productos
function buscarProductos(termino) {
    const filtrados = productos.filter(prod =>
        prod.name.toLowerCase().includes(termino.toLowerCase()) ||
        prod.description.toLowerCase().includes(termino.toLowerCase())
    );
    renderizarProductos(filtrados);
}

// Configurar Event Listeners
document.addEventListener('DOMContentLoaded', () => {
    fetchProducts();

    // Abrir modal de producto al hacer clic en "Ver detalles"
    document.getElementById('productos-container').addEventListener('click', (e) => {
        if (e.target.tagName === 'BUTTON') {
            const id = parseInt(e.target.getAttribute('data-id'));
            abrirModalProducto(id);
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
