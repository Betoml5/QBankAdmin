const urlLocal = "https://localhost:5002/turno";
const url = "https://qbank.websitos256.com/turno";
const btnNext = document.getElementById("btnNext");
const $turnoActual = document.querySelector(".container__operador-turnos-actual p");
const $btnsAtender = document.querySelectorAll(".container__operador-cajas-item button");
const $containerTurnosControl = document.querySelector(".container__operador-turnos-control");
const $containerCajasControl = document.querySelector(".container__operador-cajas");
const $caja = document.querySelector(".container__operador-caja");
const $btnSkipTurno = document.querySelector("#btnSkip");
const $btnEndTurno = document.getElementById("btnEndTurno");

const cajaId = localStorage.getItem("cajaId")
const cajaNumero = localStorage.getItem("cajaNumero");


const connection = new signalR.HubConnectionBuilder()
    .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();


$btnEndTurno.addEventListener("click", endTurno);

$btnsAtender.forEach((btn) => {
    btn.addEventListener("click", atender);
});

async function atender(e) {

    const dataset = e.target.parentElement.querySelector("p").dataset;
    localStorage.setItem("cajaId", dataset.cajaId);
    localStorage.setItem("cajaNumero", dataset.cajaNumero);

    $caja.style.display = "block";
    $caja.textContent = `Caja: ${dataset.cajaNumero}`;
    $containerCajasControl.style.display = "none";
    $containerTurnosControl.style.display = "block";

    await connection.invoke("SetActiveCaja", +dataset.cajaId);


}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

connection.on("AddToQueue", (turno) => {
    const turnosContainer = document.querySelector(".container__operador-turnos");
    const $div = document.createElement("div");
    $div.classList.add("container__operador-turnos-item");
    const pTurno = document.createElement("p");
    const pEstado = document.createElement("p");
    pTurno.textContent = `Turno: ${turno.codigoTurno}`;
    pEstado.textContent = `${turno.estado}`;
    $div.appendChild(pTurno);
    $div.appendChild(pEstado);
    turnosContainer.appendChild($div);
});

connection.on("SetCurrentTurn", (turno, cajaId) => {

    if (turno, cajaId) {
        const turnos = document.querySelectorAll(".container__operador-turnos-item");
        $turnoActual.textContent = `Turno: ${turno}`;
        $turnoActual.dataset.turno = turno;
        turnos.forEach((item) => {
            if (item.textContent.includes(turno)) {
                item.remove();
            }
        });
    }


});

connection.on("SkipTurn", (turno, siguienteTurno) => {
    if (turno, siguienteTurno) {
        $turnoActual.textContent = `Turno: ${siguienteTurno.codigoTurno}`;
        $turnoActual.dataset.turno = siguienteTurno.codigoTurno;
    }
});

btnNext.addEventListener("click", async () => {
    const cajaId = localStorage.getItem("cajaId");
    const cajaNumero = localStorage.getItem("cajaNumero");
    if (cajaId) {
        btnNext.disabled = true;
        btnNext.style.cursor = "not-allowed";
        btnNext.style.opacity = 0.5;
        $btnSkipTurno.disabled = true;
        $btnSkipTurno.style.cursor = "not-allowed";
        $btnSkipTurno.style.opacity = 0.5;
        await connection.invoke("SetCurrentTurn", (+cajaId));
    }

    setTimeout(() => {
        btnNext.disabled = false;
        btnNext.style.opacity = 1;
        btnNext.style.cursor = "inherit"

        $btnSkipTurno.disabled = false;
        $btnSkipTurno.style.opacity = 1;
        $btnSkipTurno.style.cursor = "inherit"

    }, 10000)
});


$btnSkipTurno.addEventListener("click", async () => {
    if ($turnoActual.dataset.turno) {
        await connection.invoke("SkipTurn", $turnoActual.dataset.turno);
    }
});





async function endTurno() {

    const cajaId = localStorage.getItem("cajaId");
    const cajaNumero = localStorage.getItem("cajaNumero");
    $btnEndTurno.disabled = true;
    await connection.invoke("SetInactiveCaja", +cajaId)
    localStorage.removeItem("cajaId");
    localStorage.removeItem("cajaNumero");
    window.location.reload();
    $btnEndTurno.disabled = false;
}

function init() {
    if (cajaId != null && cajaNumero != null) {
        $containerTurnosControl.style.display = "block";
        $containerCajasControl.style.display = "none";
        $caja.style.display = "block";
        $caja.textContent = `Caja: ${cajaNumero}`;
    }

    const turnoActual = localStorage.getItem("turnoActual");

    if (turnoActual) {
        $turnoActual.textContent = `Turno: ${turnoActual}`;
        $turnoActual.dataset.turno = turnoActual;
    }


}



// Start the connection.
init();
start();