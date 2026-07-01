function mostrarToast(mensaje, tipo) {
    var contenedor = document.querySelector('.toast-container');
    if (!contenedor) {
        contenedor = document.createElement('div');
        contenedor.className = 'toast-container';
        document.body.appendChild(contenedor);
    }

    var toast = document.createElement('div');
    toast.className = 'toast toast--' + tipo;
    toast.textContent = mensaje;
    contenedor.appendChild(toast);

    setTimeout(function () {
        toast.classList.add('toast--out');
        setTimeout(function () {
            if (toast.parentNode) {
                toast.parentNode.removeChild(toast);
            }
        }, 300);
    }, 4000);
}
