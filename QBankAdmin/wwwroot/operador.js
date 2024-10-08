﻿const urlLocal = "https://localhost:5002/turno";
const url = "https://qbank.websitos256.com/turno";
const btnNext = document.getElementById("btnNext");
const $turnoActual = document.querySelector(".container__operador-turnos-actual p");
const $btnsAtender = document.querySelectorAll(".container__operador-cajas-item button");
const $containerTurnosControl = document.querySelector(".container__operador-turnos-control");
const $containerCajasControl = document.querySelector(".container__operador-cajas");
const $caja = document.querySelector(".container__operador-caja");
const $btnSkipTurno = document.querySelector("#btnSkip");
const $btnEndTurno = document.getElementById("btnEndTurno");
const $bankStatusText = document.querySelector("#bankStatus");
const $btnCancel = document.querySelector("#btnCancel");

const cajaId = localStorage.getItem("cajaId")
const cajaNumero = localStorage.getItem("cajaNumero");

var nextTimeOut;
var skipTimeOut;


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

$btnCancel.addEventListener("click", async function () {
    if ($turnoActual.dataset.turno) {
        disableButtons();
        await connection.invoke("RemoveFromQueue", $turnoActual.dataset.turno);
    }
})


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
        await connection.invoke("GetBankStatus");
        await getCurrentTurn();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.on("RemoveFromQueue", (turnoCancelado, siguienteTurno) => {
    if (!siguienteTurno) {
        $turnoActual.textContent = "No hay turno por atender";
        return;
    }

    if (turnoCancelado && siguienteTurno) {
        $turnoActual.textContent = `Turno: ${siguienteTurno.codigoTurno}`;
        $turnoActual.dataset.turno = siguienteTurno.codigoTurno;
    }
})

connection.onclose(async () => {
    await start();
});

connection.on("SetBankStatus", (status) => {
    console.log("El estado del banco es: ", status)
    if (status === 0) {
        alert("El banco a cerrado, apurate en atender a los clientes")
        $bankStatusText.textContent = "Banco cerrado";
        $bankStatusText.style.color = "red";

        $btnsAtender.forEach((btn) => {
            btn.disabled = true;
            btn.style.cursor = "not-allowed";
            btn.style.opacity = 0.5;
        });

    } else {
        $btnSkipTurno.disabled = false;
        $btnSkipTurno.style.cursor = "inherit";
        $btnSkipTurno.style.opacity = 1;
        btnNext.disabled = false;
        btnNext.style.cursor = "inherit";
        btnNext.style.opacity = 1;
        $bankStatusText.textContent = "Banco abierto";
        $bankStatusText.style.color = "green";
        $btnCancel.disabled = false;
        $btnCancel.style.cursor = "inherit";
        $btnCancel.style.opacity = 1;

        $btnsAtender.forEach((btn) => {
            btn.disabled = false;
            btn.style.cursor = "inherit";
            btn.style.opacity = 1;
        });

    }
})

connection.on("GetBankStatus", (status) => {
    console.log("El estado del banco es: ", status);
    if (status === 0) {
        $btnSkipTurno.disabled = true;
        $btnSkipTurno.style.cursor = "not-allowed";
        $btnSkipTurno.style.opacity = 0.5;
        btnNext.disabled = true;
        btnNext.style.cursor = "not-allowed";
        btnNext.style.opacity = 0.5;
        $btnCancel.disabled = true;
        $btnCancel.style.cursor = "not-allowed";
        $btnCancel.style.opacity = 0.5;

        $bankStatusText.textContent = "Banco cerrado";
        $bankStatusText.style.color = "red";

        $btnsAtender.forEach((btn) => {
            btn.disabled = true;
            btn.style.cursor = "not-allowed";
            btn.style.opacity = 0.5;
        });
        

    } else {
        $btnSkipTurno.disabled = false;
        $btnSkipTurno.style.cursor = "inherit";
        $btnSkipTurno.style.opacity = 1;
        btnNext.disabled = false;
        btnNext.style.cursor = "inherit";
        btnNext.style.opacity = 1;
        $btnCancel.disabled = false;
        $btnCancel.style.cursor = "inherit";
        $btnCancel.style.opacity = 1;
        $bankStatusText.textContent = "Banco abierto";
        $bankStatusText.style.color = "green";


        $btnsAtender.forEach((btn) => {
            btn.disabled = false;
            btn.style.cursor = "inherit";
            btn.style.opacity = 1;
        });

    }
});

connection.on("AddToQueue", (turno) => {
    if (turno) {
        const $pAvisos = document.querySelector(".container__operador-turnos-control-avisos");
        $pAvisos.textContent = "Nuevo turno en espera";
        $pAvisos.style.textAlign = "center";
        $pAvisos.style.margin = "20px 0px";


        setTimeout(() => {
            $pAvisos.textContent = "";
        }, 10000)
    }

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

    if (turno && siguienteTurno) {
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
        disableButtons();
        await connection.invoke("SetCurrentTurn", (+cajaId));
    }

   
});


$btnSkipTurno.addEventListener("click", async () => {
    if ($turnoActual.dataset.turno) {
        disableButtons();
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

function disableButtons() {
    btnNext.disabled = true;
    btnNext.style.cursor = "not-allowed";
    btnNext.style.opacity = 0.5;
    $btnSkipTurno.disabled = true;
    $btnSkipTurno.style.cursor = "not-allowed";
    $btnSkipTurno.style.opacity = 0.5;
    $btnCancel.disabled = true;
    $btnCancel.style.cursor = "not-allowed";
    $btnCancel.style.opacity = 0.5;

    nextTimeOut = setTimeout(() => {
        btnNext.disabled = false;
        btnNext.style.opacity = 1;
        btnNext.style.cursor = "inherit"

        $btnSkipTurno.disabled = false;
        $btnSkipTurno.style.opacity = 1;
        $btnSkipTurno.style.cursor = "inherit"

        $btnCancel.disabled = false;
        $btnCancel.style.opacity = 1;
        $btnCancel.style.cursor = "inherit"


    }, 10000)
}



// Start the connection.
start();
init();