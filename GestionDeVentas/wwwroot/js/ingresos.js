async function cargarIngresos() {
    const container = document.getElementById("ingresos-container");
    container.innerHTML = "<p class='text-center'>Cargando ingresos...</p>";

    try {
        const response = await fetch("/Ingresos/lista");
        
        if (!response.ok) {
            throw new Error("Error al obtener los ingresos");
        }

        const ingresos = await response.json(); 
        console.log(ingresos)
        renderizarTablaIngresos(ingresos);
    } catch (error) {
        console.error("Error en cargarIngresos:", error);
        container.innerHTML = `<div class="alert alert-danger">Error: ${error.message}</div>`;
    }
}

function renderizarTablaIngresos(lista) {
    const container = document.getElementById("ingresos-container");

    if (lista.length === 0) {
        container.innerHTML = `<div class="alert alert-info">No hay ingresos disponibles.</div>`;
        return;
    }

    let html = `
        <table id="tablaIngresos" class="table table-bordered table-hover">
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

    for (const ingreso of lista) {
        
        html += `
        <tr 
            data-usuario="${ingreso.nombreUsu.toLowerCase()}" 
            data-proveedor="${ingreso.nombreProv.toLowerCase()}" 
            data-serie="${ingreso.serieComprobante.toLowerCase()}">
            <td class="text-center">${ingreso.idIngreso}</td>
            <td>${ingreso.nombreUsu}</td>
            <td>${ingreso.nombreProv}</td>
            <td>${ingreso.tipoComprobante}</td>
            <td>${ingreso.serieComprobante}</td>
            <td>${ingreso.numeroComprabante}</td>
            <td class="text-center">${new Date(ingreso.fechaHoraIngreso).toLocaleDateString()}</td>
             <td class="text-end">$${ingreso.impuesto.toFixed(2)}</td>
            <td class="text-end">$${ingreso.totalIngreso.toFixed(2)}</td>
            <td class="text-center">
                <button
                    class="btn btn-sm ${ingreso.estado ? 'btn-success' : 'btn-danger'}"
                    onclick="confirmarCambioEstado(${ingreso.idIngreso}, ${ingreso.estado})">
                    <i class="bi ${ingreso.estado ? 'bi-check-circle' : 'bi-x-circle'}"></i>
                </button>
                <a href="/Ingresos/Editar/${ingreso.idIngreso}" 
                   class="btn btn-warning btn-sm"
                   onclick='guardarIngresoLocal(${JSON.stringify(ingreso)})'>
                   <i class="bi bi-pencil-square"></i> Editar
                </a>
            </td>
        </tr>
    `;
    }

    html += "</tbody></table>";
    container.innerHTML = html;
}

let ingresoSeleccionado = 0;
let estadoActual = true;
function confirmarCambioEstado(id, estado) {
    ingresoSeleccionado = id;
    estadoActual = estado;

    const mensaje = estado
        ? "¿Está seguro que desea <strong>desactivar</strong> este ingreso?"
        : "¿Está seguro que desea <strong>activar</strong> este ingreso?";

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
        const response = await fetch(`/Ingresos/CambiarEstado/${id}`, {
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


function guardarIngresoLocal(ingreso) {
    localStorage.setItem("ingresoEditar", JSON.stringify(ingreso));
}

function filtrarTabla() {
    const texto = document.getElementById("busquedaInput").value.toLowerCase();
    const filas = document.querySelectorAll("#tablaIngresos tbody tr");

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
    cargarIngresos();
});


