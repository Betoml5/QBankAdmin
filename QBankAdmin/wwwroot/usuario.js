const url = "https://qbank.websitos256.com/turno";
const urlLocal = "https://localhost:5002/turno";
const turnos = document.querySelectorAll(".container__turnos-item");
const currentTurno = document.querySelector("#turnoActual");
const $siguentesTurnos = document.querySelectorAll(".container__turnos-item-siguientes-turnos")
const currentCaja = document.querySelector(".container__turnos-actual-caja");
const $myAudio = document.querySelector("#myAudio");

const connection = new signalR.HubConnectionBuilder()
    .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();




function playAudio() {
    $myAudio.play();
}

function pauseAudio() {
    $myAudio.pause();
}
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
        await connection.invoke("GetBankStatus");
        console.log("Unido al grupo")
    } catch (e) {
        console.error("No se pudo unir al grupo")
    }
}

connection.on("SetBankStatus", (status, turnosCancelados) => {
    console.log("El nuevo estatus del banco es: ", status)
    console.log("Turnos nuevo cancelados: ", turnosCancelados)

    if (status === 0) {

        //Eliminar turnos cancelados si existen
        const turnos = document.querySelectorAll(".container__turnos-item");

        if (turnosCancelados.length > 0) {
            turnosCancelados.forEach((turno) => {
                turnos.forEach((item) => {
                    if (item.textContent.includes(turno.codigoTurno)) {
                        item.remove();
                    }
                });
            })

        }

        document.body.style.opacity = "0.5";
        const dialog = document.createElement("dialog");
        dialog.textContent = "El banco ha sido cerrado, por favor espere a que se abra nuevamente";
        dialog.classList.add("dialog");
        dialog.style.background = "transparent"
        dialog.style.border = "none"
        document.body.appendChild(dialog);
        dialog.showModal();
    } else {
        const dialog = document.querySelector("dialog");
        dialog.textContent = "";
        dialog.classList.remove("dialog");
        document.body.style.opacity = "1";
        dialog.close();
        document.removeChild(dialog)
    }
});

connection.on("GetBankStatus", (status) => {
    console.log("El estatus del banco es: ", status)
    if (status === 0) {
        document.body.style.opacity = "0.5";
        const dialog = document.createElement("dialog");
        dialog.textContent = "El banco ha sido cerrado, por favor espere a que se abra nuevamente";
        dialog.classList.add("dialog");
        dialog.style.background = "transparent"
        dialog.style.border = "none"
        document.body.appendChild(dialog);
        dialog.showModal();
    };
})


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


let queue = [];
connection.on("SetCurrentTurn", (turno, caja) => {
    console.log("turno", turno, caja)
    console.log("solo los usuarios deberian ver esto", turno, caja)


    if (!turno && !caja) {
        currentCaja.textContent = "Esperando...";
        currentTurno.textContent = "Esperando...";
    }

    if (turno && caja) {
        queue.push(turno);
        // check if its already a turn
        if (currentCaja && currentCaja) {
            setTimeout(() => {
                playAudio();

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

                queue.pop();

                setSiguientesTurnosText();
            }, queue.length * 4000)

        } else {
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

       
    }
});

connection.on("RemoveFromQueue", (turno, siguienteTurno) => {
    console.log("turno", turno, "siguiente turno", siguienteTurno)
    if (!siguienteTurno) {
        currentCaja.textContent = "Esperando...";
        currentTurno.textContent = "Esperando...";
        return;
    }

    if (turno && siguienteTurno) {
        const $cajaDeTurnoActual = document.querySelector(".container__turnos-actual-caja");
        const turnos = document.querySelectorAll(".container__turnos-item");
        const p = document.createElement("p");
        currentTurno.innerHTML = "";
        p.textContent = `${siguienteTurno.codigoTurno}`;
        currentTurno.appendChild(p);
        $cajaDeTurnoActual.textContent = `CAJA ${siguienteTurno.caja}`;

        turnos.forEach((item) => {
            if (item.textContent.includes(siguienteTurno.codigoTurno)) {
                item.remove();
            }
        });

        setSiguientesTurnosText();
    }

        
})


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
    document.querySelectorAll(".container__turnos-item-siguientes-turnos")
        .forEach((item, index) => {
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