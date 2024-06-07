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
    await getCurrentTurn();


}

async function start() {
    try {
        await connection.start();
        await getCurrentTurn();
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
 
});

connection.on("SetCurrentTurn", (turno, cajaId) => {
       console.log(turno, cajaId, "setcurrenturno")
    if (turno && cajaId) {
        const turnos = document.querySelectorAll(".container__operador-turnos-item");
        $turnoActual.textContent = `Turno: ${turno}`;
        $turnoActual.dataset.turno = turno;
        turnos.forEach((item) => {
            if (item.textContent.includes(turno)) {
                item.remove();
            }
        });
    } else {
        $turnoActual.textContent = "No hay turno por atender";
    }


});

connection.on("SkipTurn", (turno, siguienteTurno) => {

    if (!siguienteTurno) {
        $turnoActual.textContent = "No hay turno por atender";
        return;
    }

    if (turno, siguienteTurno) {
        $turnoActual.textContent = `Turno: ${siguienteTurno.codigoTurno}`;
        $turnoActual.dataset.turno = siguienteTurno.codigoTurno;
    }
});

connection.on("GetCurrentTurn", (turno) => {
    if (turno) {
        $turnoActual.textContent = `Turno: ${turno.codigoTurno}`;
        $turnoActual.dataset.turno = turno.codigoTurno;
    } else {
        $turnoActual.textContent = "No hay turno por atender";
    }

})


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


async function getCurrentTurn() {
    try {
        $turnoActual.textContent = "Esperando turno...";
        await connection.invoke("GetCurrentTurn", +localStorage.getItem("cajaId"));
    } catch (e) {
          $turnoActual.textContent = "No hay turno por atender";
    }
}


async function endTurno() {

    const cajaId = localStorage.getItem("cajaId");
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

}



// Start the connection.
start();
init();