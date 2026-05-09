window.router = {
    navigeer(scherm, extraData = {}) {
        const container = document.getElementById('app-container') || document.getElementById('account-app-container');
        if (!container) return;

        const render = (templateId, data = {}) => {
            const element = document.getElementById(templateId);
            if (!element) return;
            const source = element.innerHTML;
            const template = Handlebars.compile(source);
            container.innerHTML = template(data);
        };

        // Algemene schermen
        if (scherm === 'oefeningen') {
            fetch('/api/Oefening')
                .then(response => response.json())
                .then(data => {
                    render("oefeningen-menu-template", { oefeningen: data })
                }); 
        }
        else if (scherm === 'speel-treble') render("treble-clef-template", extraData);
        else if (scherm === 'home') render("home-template");


        // Account gerelateerde schermen
        else if (scherm === 'account-start') render("account-template");
        else if (scherm === 'login') render("login-template");
        else if (scherm === 'register') render("register-template");
    }
};

window.showLogin = () => router.navigeer('login');
window.showRegister = () => router.navigeer('register');
window.initApp = () => router.navigeer('account-start');

window.verstuurScoreNaarServer = async function(behaaldeScore, oefeningId) {
    const url = `/api/Oefening?score=${behaaldeScore}&oefeningId=${oefeningId}`;

    try {
        const reponse = await fetch(url, {
             method: 'POST' 
        });

        if (reponse.ok) {
            alert("Score succesvol opgeslagen!");
            router.navigeer('oefeningen');
        } else {
            const errorData = await reponse.text();
            console.error("Fout bij opslaan score:", errorData);
            alert("Fout bij opslaan score. ");
        }
    } catch (error) {
        console.error("Netwerkfout bij opslaan score:", error);
        alert("Netwerkfout bij opslaan score. Probeer het later opnieuw.");
    }
}


function checkServerErrors() {
    const errorElement = document.getElementById('server-error');
    if (errorElement) {
        const message = errorElement.getAttribute('data-message');
        
        alert("Fout bij registratie: " + message);
        
        showRegister(); 
    }
}


// Clef Oefening
let goedeAntwoorden = 0;
let totaalVragen = 0;
let huidigeNoot = "";
let huidigeOefeningId = null;

const alleNoten = ['C', 'D', 'E', 'F', 'G', 'A', 'B'];
window.startNieuweVraag = function() {
    huidigeNoot = alleNoten[Math.floor(Math.random() * alleNoten.length)];
    
    const nootSvgElement = document.getElementById("noot-bolletje-svg");
    const debugLabel = document.getElementById("debug-noot-label");

    // Exacte mapping voor lijnen op 60,70,80,90,100
    const coordinaten = {
        'C': 110, // Hulplijn onder de balk
        'D': 105, // Hangt onder de onderste lijn
        'E': 100, // OP de onderste lijn (100)
        'F': 95,  // Tussen onderste en 2e lijn
        'G': 90,  // OP de 2e lijn (90)
        'A': 85,  // Tussen 2e en 3e lijn
        'B': 80   // OP de 3e lijn (80)
    };

    if (nootSvgElement) {
        nootSvgElement.setAttribute("cy", coordinaten[huidigeNoot]);
    }

    if (debugLabel) {
        debugLabel.textContent = "Debug: " + huidigeNoot;
    }
}

window.checkAntwoord = function(antwoord) {
    const feedbackEl = document.getElementById("feedback-bericht");
    if (!feedbackEl) return;

    feedbackEl.textContent = "Controleren..."; 
    feedbackEl.style.color = "orange";
    totaalVragen++;
    
    setTimeout(() => {
        const feedbackEl = document.getElementById("feedback-bericht");

        if (antwoord === huidigeNoot) {
            goedeAntwoorden++;
            feedbackEl.textContent = "Correct! Goed gedaan.";
            feedbackEl.style.color = "green";
        } else {
            feedbackEl.textContent = "Helaas! Het juiste antwoord was: " + huidigeNoot;
            feedbackEl.style.color = "red";
        }

        const tellerEl = document.getElementById("vraag-teller");
        if (tellerEl) tellerEl.textContent = `Beantwoord: ${totaalVragen}`;

        setTimeout(() => {
            feedbackEl.textContent = "Welke noot is dit?";
            feedbackEl.style.color = "black";
            startNieuweVraag();
        }, 1000);
    }, 150);
}

window.stopEnOpslaan = function() {
    if (totaalVragen === 0) {
        alert("Je hebt nog geen vragen beantwoord!");
        return;
    }

    const eindScore = Math.round((goedeAntwoorden / totaalVragen) * 100);
    window.verstuurScoreNaarServer(eindScore, huidigeOefeningId);
    goedeAntwoorden = 0;
    totaalVragen = 0;
}

window.speelOefening = function(id, naam) {
    huidigeOefeningId = id;
    router.navigeer('speel-treble', { oefeningId: id, oefeningNaam: naam });
    setTimeout(() => {
        startNieuweVraag();
    }, 50);
};



document.addEventListener("DOMContentLoaded", () => {
    const url = window.location.href.toLowerCase();

    if (document.getElementById('app-container')) {
        router.navigeer('home');
    } 
    else if (document.getElementById('account-app-container')) {

        const errorElement = document.getElementById('server-error');

        if (errorElement) {
            const message = errorElement.getAttribute('data-message');
            const isLoginFout = message.toLowerCase().includes("inloggegevens");

            if (isLoginFout) {
                router.navigeer('login');
                setTimeout(() => {
                    const loginErrorMsg = document.getElementById('login-error-msg');
                    if (loginErrorMsg) loginErrorMsg.textContent = message;
                }, 50);
            } else {
                router.navigeer('register');
                setTimeout(() => {
                    const registerErrorMsg = document.getElementById('register-error-msg');
                    if (registerErrorMsg) registerErrorMsg.textContent = message;
                }, 50);
            }
            return;
        }

        if (url.includes('register')) {
            router.navigeer('register');
        } else if (url.includes('login')) {
            router.navigeer('login');
        } else {
            router.navigeer('account-start');
        }
    }
});