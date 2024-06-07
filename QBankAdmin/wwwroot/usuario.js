const url = "https://qbank.websitos256.com/turno";
const urlLocal = "https://localhost:5002/turno";
const turnos = document.querySelectorAll(".container__turnos-item");
const currentTurno = document.querySelector("#turnoActual");
const $siguentesTurnos = document.querySelectorAll(".container__turnos-item-siguientes-turnos")
const currentCaja = document.querySelector(".container__turnos-actual-caja");
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
        await AddToGroup();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

async function AddToGroup() {
    try {
        await connection.invoke("AddToGroup", "usuario");
        console.log("Unido al grupo")
    } catch (e) {
        console.error("No se pudo unir al grupo")
    }
}


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

    setSiguientesTurnosText();


});


connection.on("SetCurrentTurn", (turno, caja) => {
    console.log("turno", turno, caja)
    console.log("solo los usuarios deberian ver esto", turno, caja)
    if (!turno && !caja) {
        currentCaja.textContent = "Esperando...";
        currentTurno.textContent = "Esperando...";
    }

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

        setSiguientesTurnosText();
    }
});


connection.on("SkipTurn", (turno, siguienteTurno) => {


    if (!siguienteTurno) {
        const $turnoSaltado = document.createElement("div");
        $turnoSaltado.classList.add("container__turnos-item");
        const pSaltado = document.createElement("p");
        const pSiguientesTurnos = document.createElement("p");
        pSiguientesTurnos.classList.add("container__turnos-item-siguientes-turnos");
        $turnoSaltado.appendChild(pSiguientesTurnos);
        pSaltado.textContent = `${turno.codigoTurno}`;
        $turnoSaltado.appendChild(pSaltado);
        document.querySelector(".container__turnos").appendChild($turnoSaltado);


        currentTurno.textContent = `Esperando...`;
        currentCaja.textContent = `Esperando...`;


        return;
    }

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


    }

})

function setSiguientesTurnosText() {
    $siguentesTurnos.forEach((item, index) => {
        if (index == 0) {
            item.textContent = "Proximos turnos";
            return;
        }
    });
}


function init() {
  

    setSiguientesTurnosText();

        const $caja = document.querySelector(".container__turnos-actual-caja");
        const $turno = document.querySelector("#turnoActual");

        $caja.textContent = "Esperando...";
        $turno.textContent = "Esperando...";



}


// Start the connection.
init();
start();