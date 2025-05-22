async function cargarVentas() {
    const container = document.getElementById("ventas-container");
    container.innerHTML = "<p class='text-center'>Cargando Ventas...</p>";

    try {
        const response = await fetch("/Ventas/lista");

        if (!response.ok) {
            throw new Error("Error al obtener los ventas");
        }

        const ventas = await response.json();
        console.log(ventas)
        renderizarTablaIngresos(ventas);
    } catch (error) {
        console.error("Error en cargarVentas:", error);
        container.innerHTML = `<div class="alert alert-danger">Error: ${error.message}</div>`;
    }
}

function renderizarTablaIngresos(lista) {
    const container = document.getElementById("ventas-container");

    if (lista.length === 0) {
        container.innerHTML = `<div class="alert alert-info">No hay ingresos disponibles.</div>`;
        return;
    }

    let html = `
        <table id="tablaVentas" class="table table-bordered table-hover">
            <thead class="table-dark text-center">
                <tr>
                    <th>ID</th>
                    <th>Nombre</th>
                    <th>Proveedor</th>
                    <th>Comprobrante</th>
                    <th>Serie</th>
                    <th>Número</th>
                    <th>Fecha</th>
                    <th>Impuesto</th>
                    <th>Total</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
    `;

    for (const venta of lista) {
        console.log("Procesando venta:", venta);
        html += `
        <tr 
            data-usuario="${venta.nombreUsu.toLowerCase()}" 
            data-proveedor="${venta.nombreProv.toLowerCase()}" 
            data-serie="${venta.serieComprobante.toLowerCase()}">
            <td class="text-center">${venta.idVenta}</td>
            <td>${venta.nombreUsu}</td>
            <td>${venta.nombreProv}</td>
            <td>${venta.tipoComprobante}</td>
            <td>${venta.serieComprobante}</td>
            <td>${venta.numeroComprobante}</td>
            <td class="text-center">${new Date(venta.fechaHora).toLocaleDateString()}</td>
             <td class="text-end">$${Number(venta.impuestoVenta || 0).toFixed(2)}</td>
            <td class="text-end">$${Number(venta.totalVenta || 0).toFixed(2)}</td>
            <td class="text-center">
                <button
                    class="btn btn-sm ${venta.estado ? 'btn-success' : 'btn-danger'}"
                    onclick="confirmarCambioEstado(${venta.idVenta}, ${venta.estado})">
                    <i class="bi ${venta.estado ? 'bi-check-circle' : 'bi-x-circle'}"></i>
                </button>
                <a href="/Ventas/Editar/${venta.idVenta}" 
                   class="btn btn-warning btn-sm"
                   onclick='guardarVentaLocal(${JSON.stringify(venta)})'>
                   <i class="bi bi-pencil-square"></i> Editar
                </a>
            </td>
        </tr>
    `;
    }

    html += "</tbody></table>";
    container.innerHTML = html;
}

function guardarVentaLocal(venta) {
    localStorage.setItem("VentaEditar", JSON.stringify(venta));
}

function filtrarTabla() {
    const texto = document.getElementById("busquedaInput").value.toLowerCase();
    const filas = document.querySelectorAll("#tablaVentas tbody tr");

    filas.forEach(fila => {
        const usuario = fila.dataset.usuario || "";
        const proveedor = fila.dataset.proveedor || "";
        const serie = fila.dataset.serie || "";

        const coincide =
            usuario.includes(texto) ||
            proveedor.includes(texto) ||
            serie.includes(texto);

        fila.style.display = coincide ? "" : "none";
    });
}

document.addEventListener("DOMContentLoaded", () => {
    cargarVentas();
});


let ingresoSeleccionado = 0;
let estadoActual = true;
function confirmarCambioEstado(id, estado) {
    ingresoSeleccionado = id;
    estadoActual = estado;

    const mensaje = estado
        ? "¿Está seguro que desea <strong>desactivar</strong> este Venta?"
        : "¿Está seguro que desea <strong>activar</strong> este Venta?";

    document.getElementById("modalEstadoMensaje").innerHTML = mensaje;
    const modal = new bootstrap.Modal(document.getElementById('modalConfirmarEstado'));
    modal.show();
}

document.getElementById("btnConfirmarCambioEstado").addEventListener("click", async function () {
    console.log("Entrando..")
    try {
        const id = parseInt(ingresoSeleccionado);
        console.log(id);
        if (isNaN(id)) {
            alert("ID no válido.");
            return;
        }
        console.log(id);
        const response = await fetch(`/Ventas/CambiarEstado/${id}`, {
            method: "POST"
        });

        if (response.ok) {
            window.location.reload();
        } else {
            alert("Error al cambiar el estado.");
        }
    } catch (err) {
        console.error("❌ Error:", err);
        alert("Error inesperado.");
    }
});

