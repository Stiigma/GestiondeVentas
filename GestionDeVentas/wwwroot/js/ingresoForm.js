
// Lista donde se almacenan los artículos seleccionados para el ingreso
 const articulosSeleccionados = [];
console.log(articulosSeleccionados)

async function cargarArticulos(filtro = "") {
    try {
        const response = await fetch(`/Ingresos/lista-articulos`);
        if (!response.ok) throw new Error("Error al obtener artículos");

        const articulos = await response.json();
        console.log("Artículos recibidos:", articulos);

        const tabla = document.getElementById("tablaArticulos");
        tabla.innerHTML = "";

        articulos
            .filter(a => a.nombreArticulo && a.nombreArticulo.toLowerCase().includes(filtro.toLowerCase()))
            .forEach((a, index) => {
                const fila = document.createElement("tr");

                fila.innerHTML = `
            <td>${a.nombreArticulo}</td>
            <td>${a.nombreCategoria}</td>
            <td>${a.stock}</td>
            <td>${a.precioVenta}</td>
            <td>
                <button class="btn btn-success btn-sm seleccionar-btn" data-index="${index}">+</button>
            </td>
        `;

                tabla.appendChild(fila);
            });

        // Asignar evento a cada botón luego de renderizar las filas
        document.querySelectorAll(".seleccionar-btn").forEach(btn => {
            btn.addEventListener("click", (e) => {
                const index = e.target.getAttribute("data-index");
                seleccionarArticulo(articulos[index]);
            });
        });

    } catch (error) {
        console.error("Error cargando artículos:", error);
        document.getElementById("tablaArticulos").innerHTML = `
            <tr><td colspan="5" class="text-center text-danger">Error al cargar artículos</td></tr>
        `;
    }
}

async function cargarProveedores() {
    const select = document.getElementById("idUsuario");

    try {
        const response = await fetch("/Ingresos/lista-provedores");

        if (!response.ok) throw new Error("Error al obtener proveedores");

        const proveedores = await response.json();
        console.log("Proveedores:", proveedores);

        select.innerHTML = '<option disabled selected>Seleccione un proveedor</option>';

        proveedores.forEach(prov => {
            const option = document.createElement("option");
            option.value = prov.idUsuario; // asegúrate que este sea el campo correcto
            option.textContent = prov.nombreUsuario;
            select.appendChild(option);
        });

    } catch (error) {
        console.error("Error cargando proveedores:", error);
        select.innerHTML = '<option disabled selected>Error al cargar</option>';
    }
}

function seleccionarArticulo(articulo) {
    const yaExiste = articulosSeleccionados.some(a => a.idArticulo === articulo.idArticulo);

    if (yaExiste) {
        alert("Este artículo ya fue agregado.");
        return;
    }

    articulo.cantidad = 1; // default
    articulosSeleccionados.push(articulo);

    console.log("Artículos seleccionados:", articulosSeleccionados);

    
    renderizarDetalles();
}


function eliminarArticulo(index) {
    articulosSeleccionados.splice(index, 1);
    renderizarDetalles();
}



function actualizarCantidad(index, nuevaCantidad) {
    nuevaCantidad = parseInt(nuevaCantidad);
    
    if (isNaN(nuevaCantidad) || nuevaCantidad < 1) {
        nuevaCantidad = 1;
    }

    articulosSeleccionados[index].cantidad = nuevaCantidad;
    renderizarDetalles();
}


function renderizarDetalles() {
    const tbody = document.querySelector("#tablaDetalles tbody");
    const contenedor = document.getElementById("detalleIngresoContainer");
    const sinDetalles = document.getElementById("sinDetalles");

    tbody.innerHTML = "";

    if (articulosSeleccionados.length === 0) {
        contenedor.style.display = "none";
        sinDetalles.style.display = "block";
        return;
    }

    contenedor.style.display = "block";
    sinDetalles.style.display = "none";

    let totalParcial = 0;

    articulosSeleccionados.forEach((articulo, index) => {
        const cantidad = articulo.cantidad || 1;
        const precio = parseFloat(articulo.precioVenta);
        const subtotal = cantidad * precio;

        totalParcial += subtotal;

        const fila = document.createElement("tr");
        fila.innerHTML = `
            <td>${articulo.nombreArticulo}</td>
            <td class="text-center">
                <input type="number" min="1" value="${cantidad}" class="form-control form-control-sm text-center"
                    onchange="actualizarCantidad(${index}, this.value)" />
            </td>
            <td class="text-end">$${precio.toFixed(2)}</td>
            <td class="text-end">$${subtotal.toFixed(2)}</td>
            <td class="text-center">
                <button class="btn btn-danger btn-sm" onclick="eliminarArticulo(${index})">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        `;
        tbody.appendChild(fila);
    });

    const impuesto = totalParcial * (parseFloat(document.getElementById("impuesto").value || 0) / 100);
    const totalNeto = totalParcial + impuesto;

    document.getElementById("totalParcial").textContent = totalParcial.toFixed(2);
    document.getElementById("totalImpuesto").textContent = impuesto.toFixed(2);
    document.getElementById("totalNeto").textContent = totalNeto.toFixed(2);
    document.getElementById("totalIngreso").value = totalNeto.toFixed(2);
}



function construirJsonIngreso() {
    const ingreso = {
        idUsuario: parseInt(document.getElementById("idUsuario")?.value || "0"),
        idPersona: parseInt(document.getElementById("idPersona")?.value || "0"),
        tipoComprobante: document.getElementById("tipoComprobante").value,
        serieComprobante: document.getElementById("serie").value,
        numeroComprabante: document.getElementById("numero").value,
        impuesto: parseFloat(document.getElementById("impuesto").value || "0"),
        totalIngreso: parseFloat(document.getElementById("totalNeto").textContent || "0"),
        estado: true, // puedes cambiarlo según lógica
        articulos: articulosSeleccionados.map(a => ({
            idArticulo: a.idArticulo,
            cantidad: a.cantidad,
            precioVenta: a.precioVenta
        }))
    };

    return ingreso;
}

document.addEventListener("DOMContentLoaded", () => {
    cargarProveedores();
    cargarArticulos();

    const buscador = document.getElementById("buscarArticulo");
    buscador.addEventListener("input", () => {
        cargarArticulos(buscador.value);
    });
});



document.getElementById("form-ingreso").addEventListener("submit", function (e) {
    // Limpiar inputs ocultos anteriores
    const contenedor = document.getElementById("inputsOcultosArticulos");
    contenedor.innerHTML = "";

    // Agregar cada artículo como grupo de inputs
    articulosSeleccionados.forEach((articulo, index) => {
        const inputId = crearInput(`Articulos[${index}].IdArticulo`, articulo.idArticulo);
        const inputCantidad = crearInput(`Articulos[${index}].Cantidad`, articulo.cantidad);
        const inputPrecio = crearInput(`Articulos[${index}].Precio`, articulo.precioVenta);

        contenedor.appendChild(inputId);
        contenedor.appendChild(inputCantidad);
        contenedor.appendChild(inputPrecio);
    });
});

// Utilidad para crear inputs hidden
function crearInput(name, value) {
    const input = document.createElement("input");
    input.type = "hidden";
    input.name = name;
    input.value = value;
    return input;
}



document.getElementById("form-ingreso").addEventListener("submit", async function (e) {
    e.preventDefault(); // Detenemos envío clásico

    const jsonFinal = construirJsonIngreso();
    console.log("🟢 Enviando JSON al backend:", jsonFinal);

    try {
        const response = await fetch('/Ingresos/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(jsonFinal)
        });

        if (response.ok) {
            // Redirigir o mostrar éxito
            alert("Ingreso guardado correctamente.");
            window.location.href = "/Ingresos"; // o Index, o a donde quieras
        } else {
            const error = await response.text();
            console.error("Error al guardar:", error);
            alert("Error al guardar el ingreso.");
        }
    } catch (err) {
        console.error("Excepción:", err);
        alert("Ocurrió un error inesperado.");
    }
});


if (modoEdicion === true || modoEdicion === "true") {
    document.querySelector("h2").innerHTML = '<i class="bi bi-pencil-square"></i> Editar Ingreso';
    document.querySelector("#form-ingreso button[type='submit']").innerHTML = '<i class="bi bi-save"></i> Guardar Cambios';
}


async function precargarIngreso(id) {
    try {
        // 1. Obtener detalles del ingreso por ID
        const response = await fetch(`/Ingresos/obtener-dI/${id}`);
        if (!response.ok) throw new Error("Error al obtener ingreso");
        const detalles = await response.json();

        // 2. Obtener todos los artículos para mapear sus nombres
        const articulosResponse = await fetch("/Ingresos/lista-articulos");
        if (!articulosResponse.ok) throw new Error("Error al obtener artículos");
        const articulos = await articulosResponse.json();

        // 3. Cargar proveedores antes de rellenar el formulario
        await cargarProveedores();

        // 4. Agregar artículos al array global con nombre completo
        detalles.forEach(det => {
            const articuloEncontrado = articulos.find(a => a.idArticulo === det.idArticulo);

            articulosSeleccionados.push({
                idArticulo: det.idArticulo,
                cantidad: det.cantidad,
                precioVenta: det.precioVenta,
                nombreArticulo: articuloEncontrado ? articuloEncontrado.nombreArticulo : `Artículo ${det.idArticulo}`
            });
        });

        renderizarDetalles();

    } catch (error) {
        console.error("Error precargando ingreso:", error);
        alert("❌ No se pudo cargar la información del ingreso.");
    }
}

