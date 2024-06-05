﻿const url = "https://qbank.websitos256.com/turno";
const urlLocal = "https://localhost:5002/turno";
const turnos = document.querySelectorAll(".container__turnos-item");
const currentTurno = document.querySelector("#turnoActual");
const $siguentesTurnos = document.querySelectorAll(".container__turnos-item-siguientes-turnos")

const connection = new signalR.HubConnectionBuilder()
    .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();

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
    const turnosContainer = document.querySelector(".container__turnos");

    const $div = document.createElement("div");
    $div.classList.add("container__turnos-item");
    const pSiguientesTurnos = document.createElement("p");
    pSiguientesTurnos.classList.add("container__turnos-item-siguientes-turnos");
    $div.appendChild(pSiguientesTurnos);
    const p = document.createElement("p");
    p.textContent = `${turno.codigoTurno}`;
    $div.appendChild(p);

    turnosContainer.appendChild($div);


});


connection.on("SetCurrentTurn", (turno, caja) => {

    if (turno && caja) {
        const $cajaDeTurnoActual = document.querySelector(".container__turnos-actual-caja");
        const turnos = document.querySelectorAll(".container__turnos-item");
        const p = document.createElement("p");
        currentTurno.innerHTML = "";
        p.textContent = `${turno}`;
        currentTurno.appendChild(p);
        $cajaDeTurnoActual.textContent = `CAJA ${caja}`;

        turnos.forEach((item) => {
            if (item.textContent.includes(turno)) {
                item.remove();
            }
        });

        localStorage.setItem("turnoActual", turno);
        localStorage.setItem("cajaActual", caja);
    }
});


connection.on("SkipTurn", (turno, siguienteTurno) => {
    if (turno, siguienteTurno) {
        const $cajaDeTurnoActual = document.querySelector(".container__turnos-actual-caja");
        const turnos = document.querySelectorAll(".container__turnos-item");
        currentTurno.textContent = `${siguienteTurno.codigoTurno}`;
        $cajaDeTurnoActual.textContent = `CAJA ${siguienteTurno.caja}`;

        turnos.forEach((item) => {
            if (item.textContent.includes(siguienteTurno.codigoTurno)) {
                item.remove();
            }
        });

        const $turnoSaltado = document.createElement("div");
        $turnoSaltado.classList.add("container__turnos-item");
        const pSaltado = document.createElement("p");
        const pSiguientesTurnos = document.createElement("p");
        pSiguientesTurnos.classList.add("container__turnos-item-siguientes-turnos");
        $turnoSaltado.appendChild(pSiguientesTurnos);
        pSaltado.textContent = `${turno.codigoTurno}`;
        $turnoSaltado.appendChild(pSaltado);
        document.querySelector(".container__turnos").appendChild($turnoSaltado);


        localStorage.setItem("turnoActual", siguienteTurno.codigoTurno);
        localStorage.setItem("cajaActual", siguienteTurno.caja);
    }

})


function init() {
    const turnoActual = localStorage.getItem("turnoActual");
    const cajaActual = localStorage.getItem("cajaActual");

    $siguentesTurnos.forEach((item, index) => {
        if (index == 0) {
            item.textContent = "Proximos turnos";
            return;
        }
    });

    if (turnoActual && cajaActual) {
        const $caja = document.querySelector(".container__turnos-actual-caja");
        const $turno = document.querySelector("#turnoActual");

        $caja.textContent = `CAJA ${cajaActual}`;
        $turno.textContent = turnoActual;


    }




}


// Start the connection.
init();
start();