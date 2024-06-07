const url = "https://qbank.websitos256.com/turno";
const urlLocal = "https://localhost:5002/turno";

const $btnCerrarBanco = document.querySelector("#btnCerrarBanco");
const $btnAbrirBanco = document.querySelector("#btnAbrirBanco");
const $bankStatusText = document.querySelector(".bankStatusText")
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
        await connection.invoke("GetBankStatus");
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.on("GetBankStatus", (status) => {
    console.log("El estatus del banco es: ", status);
    if (status === 0) {
        $bankStatusText.textContent = "Banco cerrado"
        $btnCerrarBanco.disabled = true;
        $btnAbrirBanco.disabled = false;
    } else {
        $bankStatusText.textContent = "Banco abierto"
        $btnCerrarBanco.disabled = false;
        $btnAbrirBanco.disabled = true;
    }
})


$btnAbrirBanco.addEventListener("click", async function(){
    
   
        try {
            this.textContent = "Abriendo banco..."
            await connection.invoke("SetBankStatus", 1);
            $bankStatusText.textContent = "Banco abierto"
            this.textContent = "Abrir baco";
            this.disabled = true;
            $btnCerrarBanco.disabled = false;
        } catch (e) {
            this.textContent = "Abrir banco"

        }
})

$btnCerrarBanco.addEventListener("click", async function () {
        try {
            this.textContent = "Cerrando banco..."
            await connection.invoke("SetBankStatus", 0);
            $bankStatusText.textContent = "Banco cerrado"
            this.textContent = "Cerrar banco";
            this.disabled = true;
            $btnAbrirBanco.disabled = false;
        } catch (e) {
            this.textContent = "Cerrar banco"
        }
})


start();




