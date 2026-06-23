var accionConfirmadaSistema = null;

function mostrarMensajeSistema(mensaje, tipo, contenedor) {
    tipo = tipo || "exito";
    contenedor = contenedor || "#mensajeOperacion";

    var $contenedor = $(contenedor);

    if ($contenedor.length === 0) {
        crearMensajeFlotanteSistema(mensaje, tipo);
        return;
    }

    $contenedor
        .removeClass("exito error advertencia alert-success alert-danger alert-warning")
        .addClass(tipo)
        .text(mensaje)
        .fadeIn(150);

    setTimeout(function () {
        $contenedor.fadeOut(250);
    }, 4500);
}

function crearMensajeFlotanteSistema(mensaje, tipo) {
    var clase = "alert-info";

    if (tipo === "exito") clase = "alert-success";
    if (tipo === "error") clase = "alert-danger";
    if (tipo === "advertencia") clase = "alert-warning";

    var $mensaje = $("<div>", {
        class: "alert " + clase + " shadow-sm mensaje-flotante-sistema",
        text: mensaje
    });

    $("body").append($mensaje);

    setTimeout(function () {
        $mensaje.fadeOut(250, function () {
            $(this).remove();
        });
    }, 4500);
}

function mostrarConfirmacionSistema(titulo, mensaje, callback, textoBoton, claseBoton) {
    accionConfirmadaSistema = callback;

    textoBoton = textoBoton || "Aceptar";
    claseBoton = claseBoton || "btn-danger";

    $("#tituloConfirmacionSistema").text(titulo);
    $("#textoConfirmacionSistema").text(mensaje);

    $("#btnAceptarConfirmacionSistema")
        .removeClass("btn-danger btn-primary btn-success btn-warning btn-secondary")
        .addClass(claseBoton)
        .text(textoBoton);

    $("#modalConfirmacionSistema").modal("show");
}

$(document).on("click", "#btnAceptarConfirmacionSistema", function () {
    $("#modalConfirmacionSistema").modal("hide");

    if (typeof accionConfirmadaSistema === "function") {
        accionConfirmadaSistema();
    }

    accionConfirmadaSistema = null;
});

function obtenerTextoAccionEstado(estaActivo) {
    if (window.esAdministradorSistema === true) {
        return estaActivo ? "Inactivar" : "Activar";
    }

    return "Eliminar";
}

function obtenerTituloAccionEstado(estaActivo) {
    if (window.esAdministradorSistema === true) {
        return estaActivo ? "Inactivar registro" : "Activar registro";
    }

    return "Eliminar registro";
}

function obtenerMensajeAccionEstado(estaActivo, nombreRegistro) {
    nombreRegistro = nombreRegistro || "este registro";

    if (window.esAdministradorSistema === true) {
        return estaActivo
            ? "¿Desea inactivar " + nombreRegistro + "?"
            : "¿Desea activar " + nombreRegistro + "?";
    }

    return "¿Desea eliminar " + nombreRegistro + "?";
}

function obtenerMensajeExitoEstado(estaActivo, nombreRegistro) {
    nombreRegistro = nombreRegistro || "Registro";

    if (window.esAdministradorSistema === true) {
        return estaActivo
            ? nombreRegistro + " inactivado correctamente."
            : nombreRegistro + " activado correctamente.";
    }

    return nombreRegistro + " eliminado correctamente.";
}

function crearBotonEstado(item, callbackInactivar, callbackActivar, nombreRegistro) {
    var estaActivo = item.Activo === true;

    if (!estaActivo && window.esAdministradorSistema !== true) {
        return null;
    }

    var texto = obtenerTextoAccionEstado(estaActivo);

    var clase = estaActivo
        ? "btn btn-sm btn-outline-danger"
        : "btn btn-sm btn-outline-success";

    var boton = $("<button>", {
        type: "button",
        class: clase,
        text: texto
    });

    boton.on("click", function () {
        mostrarConfirmacionSistema(
            obtenerTituloAccionEstado(estaActivo),
            obtenerMensajeAccionEstado(estaActivo, nombreRegistro),
            function () {
                if (estaActivo) {
                    callbackInactivar();
                } else if (typeof callbackActivar === "function") {
                    callbackActivar();
                }
            },
            texto,
            estaActivo ? "btn-danger" : "btn-success"
        );
    });

    return boton;
}