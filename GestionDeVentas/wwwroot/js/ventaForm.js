
const articulosSeleccionados = [];
var idVenta = 0;
console.log(articulosSeleccionados)


async function cargarProveedores() {
    const select = document.getElementById("idUsuario");

    try {
        const response = await fetch("/Ventas/lista-clientes");

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

async function cargarPersonas() {
    const select = document.getElementById("idPersona");

    try {
        const response = await fetch("/Ingresos/lista-personas");

        if (!response.ok) throw new Error("Error al obtener personas");

        const personas = await response.json();
        console.log("Personas recibidas:", personas);

        select.innerHTML = '<option disabled selected>Seleccione una persona</option>';

        personas.forEach(persona => {
            const option = document.createElement("option");
            option.value = persona.idPersona;
            option.textContent = persona.nombrePersona;
            select.appendChild(option);
        });

    } catch (error) {
        console.error("Error cargando personas:", error);
        select.innerHTML = '<option disabled selected>Error al cargar personas</option>';
    }
}



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

document.addEventListener("DOMContentLoaded", () => {
    setTimeout(async () => {
        if (modoEdicion !== true && modoEdicion !== "true") {
            localStorage.removeItem("VentaEditar");
            await Promise.all([
                cargarPersonas(), // <-- asegúrate de que esta función sea async también
                cargarProveedores(),
                cargarArticulos(),
                //cargarDetallesPorId(ingreso.idIngreso)
            ]);
            limpiarFormulario();
        } else {
            await cargarRe();
            console.log("✅ Se ejecutó cargarRe()");
        }

        const buscador = document.getElementById("buscarArticulo");
        buscador?.addEventListener("input", () => {
            cargarArticulos(buscador.value);
        });
    }, 50); // Espera 50ms para asegurar que el DOM esté completamente renderizado
});


function renderizarDetalles() {
    const tbody = document.querySelector("#tablaDetalles tbody");
    const contenedor = document.getElementById("detalleVentaContainer");
    const sinDetalles = document.getElementById("sinDetalles");

    // Validar que los elementos existen
    if (!tbody || !contenedor || !sinDetalles) {
        console.warn("⛔ Uno o más elementos del DOM no están disponibles aún.");
        return;
    }

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

    let impuesto = totalParcial * (parseFloat(document.getElementById("impuesto")?.value || 0) / 100);
    let _descuento = totalParcial * (parseFloat(document.getElementById("descuento")?.value || 0) / 100);
    let totalNeto = totalParcial + impuesto - _descuento;
    
    document.getElementById("totalParcial").textContent = totalParcial.toFixed(2);
    document.getElementById("totalImpuesto").textContent = impuesto.toFixed(2);
    document.getElementById("totalDescuento").textContent = _descuento.toFixed(2);
    document.getElementById("totalNeto").textContent = totalNeto.toFixed(2);

    if (articulosSeleccionados.length == 0) {
        document.getElementById("totalParcial").textContent = 0;
        document.getElementById("totalImpuesto").textContent = 0;
        document.getElementById("totalDescuento").textContent = 0;
        document.getElementById("totalNeto").textContent = 0;

    }


}


function construirJsonVenta() {
    const _descuento = parseFloat(document.getElementById("descuento").value || "0");
    const venta = {
        idVenta: idVenta,
        idUsuario: parseInt(document.getElementById("idUsuario")?.value || "0"),
        idPersona: parseInt(document.getElementById("idPersona")?.value || "0"),
        tipoComprobante: document.getElementById("tipoComprobante").value,
        serieComprobante: document.getElementById("serie").value,
        numeroComprabante: document.getElementById("numero").value,
        impuestoVenta: parseFloat(document.getElementById("impuesto").value || "0"),
        TotalVenta: parseFloat(document.getElementById("totalNeto").textContent || "0"),
        estado: true, // puedes cambiarlo según lógica
        articulos: articulosSeleccionados.map(a => ({
            idArticulo: a.idArticulo,
            cantidad: a.cantidad,
            precioVenta: a.precioVenta,
            descuento: _descuento
        }))
    };

    return venta;
}


document.getElementById("form-venta").addEventListener("submit", function (e) {
    // Limpiar inputs ocultos anteriores
    const contenedor = document.getElementById("inputsOcultosArticulos");
    contenedor.innerHTML = "";
    const _descuento = parseFloat(document.getElementById("descuento").value || "0");
    // Agregar cada artículo como grupo de inputs
    articulosSeleccionados.forEach((articulo, index) => {
        const inputId = crearInput(`Articulos[${index}].IdArticulo`, articulo.idArticulo);
        const inputCantidad = crearInput(`Articulos[${index}].Cantidad`, articulo.cantidad);
        const inputPrecio = crearInput(`Articulos[${index}].Precio`, articulo.precioVenta);
        const inputDescuento = crearInput(`Articulos[${index}].Descuento`, _descuento);

        contenedor.appendChild(inputId);
        contenedor.appendChild(inputCantidad);
        contenedor.appendChild(inputPrecio);
    });
});


function crearInput(name, value) {
    const input = document.createElement("input");
    input.type = "hidden";
    input.name = name;
    input.value = value;
    return input;
}

document.getElementById("form-venta").addEventListener("submit", async function (e) {
    e.preventDefault(); // Detenemos envío clásico

    const jsonFinal = construirJsonVenta();
    console.log("🟢 Enviando JSON al backend:", jsonFinal);

    try {
        const endpoint = (modoEdicion === true || modoEdicion === "true")
            ? '/Ventas/Editar'   // o el nombre de tu acción editar en el controlador
            : '/Ventas/Create';
        console.log(endpoint);
        const response = await fetch(endpoint, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(jsonFinal)
        });


        if (response.ok) {
            // Redirigir o mostrar éxito
            alert("Venta guardado correctamente.");
            window.location.href = "/Ventas"; // o Index, o a donde quieras
        } else {
            const error = await response.text();
            console.error("Error al guardar:", error);
            alert("Error al guardar el Venta.");
        }
    } catch (err) {
        console.error("Excepción:", err);
        alert("Ocurrió un error inesperado.");
    }
});


async function cargarRe() {
    const VentaRaw = localStorage.getItem("VentaEditar");

    if (!VentaRaw) return;

    const venta = JSON.parse(VentaRaw);
    idVenta = venta.idVenta;
    console.log("Ingreso iD:", idVenta);
    console.log("Ingreso cargado:", venta);
    await Promise.all([
        cargarPersonas(), // <-- asegúrate de que esta función sea async también
        cargarProveedores(),
        cargarArticulos(),
        cargarDetallesPorId(idVenta)
    ]);


    async function cargarDetallesPorId(idVenta) {
        try {
            const response = await fetch(`/Ventas/obtener-dI/${idVenta}`);
            console.log(response);
            if (!response.ok) {
                throw new Error("No se pudo obtener los detalles del ingreso.");
            }

            const detalles = await response.json();
            console.log(detalles);

            // Opcional: puedes cargar la lista completa si necesitas nombre del artículo
            const articulosResponse = await fetch("/Ingresos/lista-articulos");
            if (!articulosResponse.ok) throw new Error("Error al obtener artículos");
            const articulos = await articulosResponse.json();
            console.log(articulos);
            // Limpiar el array actual
            articulosSeleccionados.length = 0;
            
            // Insertar los nuevos detalles
            detalles.forEach(det => {
                const articuloEncontrado = articulos.find(a => a.idArticulo === det.idArticulo);
                articulosSeleccionados.push({
                    idArticulo: det.idArticulo,
                    cantidad: det.cantidad,
                    precioVenta: det.precioVenta,
                    nombreArticulo: articuloEncontrado ? articuloEncontrado.nombreArticulo : `Artículo ${det.idArticulo}`
                });

                document.getElementById("descuento").value = det.descuento;
            });

            renderizarDetalles();
            console.log("logrado");
            console.log("✅ Detalles cargados en articulosSeleccionados");

        } catch (error) {
            console.error("❌ Error en cargarDetallesPorId:", error);
            alert("No se pudieron cargar los detalles del ingreso.");
        }
    }


    // Esperar a que se hayan cargado las opciones del select

    
    // Asignar valores a los inputs
    document.getElementById("idPersona").value = venta.idPersona;
    document.getElementById("idUsuario").value = venta.idUsuario;
    document.getElementById("tipoComprobante").value = venta.tipoComprobante;
    document.getElementById("serie").value = venta.serieComprobante;
    document.getElementById("numero").value = venta.numeroComprobante;
    document.getElementById("impuesto").value = venta.impuestoVenta;
}


if (modoEdicion === true || modoEdicion === "true") {
    document.querySelector("h2").innerHTML = '<i class="bi bi-pencil-square"></i> Editar Venta';
    document.querySelector("#form-venta button[type='submit']").innerHTML = '<i class="bi bi-save"></i> Guardar Cambios';
}



function limpiarFormulario() {
    document.getElementById("idPersona").value = "";
    document.getElementById("idUsuario").value = "";
    document.getElementById("tipoComprobante").value = "Factura"; // o ""
    document.getElementById("serie").value = "";
    document.getElementById("numero").value = "";
    document.getElementById("impuesto").value = 16;
    articulosSeleccionados.length = 0;
    idVenta = 0;
    renderizarDetalles();
}