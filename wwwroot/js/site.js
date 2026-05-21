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
        else if (scherm === 'speel-bass') render("bass-clef-template", extraData);
        else if (scherm === 'speel-beide') render("beide-clef-template", extraData);
        else if (scherm === 'home') render("home-template");
        else if (scherm === 'klas') { laadKlassen(); }
        else if (scherm === 'klas-details') { bekijkKlasDetails(extraData.klasId); }
        else if (scherm === 'leerling-dashboard') { laadLeerlingResultaten(); }
        else if (scherm === 'docent-dashboard') laadDocentDashboard();
        else if (scherm === 'docent-leerling-voortgang') {
            // Wordt aangeroepen via functie
        }


        // Account gerelateerde schermen
        else if (scherm === 'account-start') render("account-template");
        else if (scherm === 'login') render("login-template");
        else if (scherm === 'register') render("register-template");
    }
};

Handlebars.registerHelper('formatDate', function(dateString) {
    if (!dateString) return 'Onbekend';
    const date = new Date(dateString);
    return date.toLocaleDateString('nl-NL', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
});

window.showLogin = () => router.navigeer('login');
window.showRegister = () => router.navigeer('register');
window.initApp = () => router.navigeer('account-start');

window.verstuurScoreNaarServer = async function(goedeAntwoorden, aantalVragen, oefeningId) {
    const url = `/api/Oefening?goedeAntwoorden=${goedeAntwoorden}&aantalVragen=${aantalVragen}&oefeningId=${oefeningId}`;

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

window.toonKlasAanmakenForm = function() {
    document.getElementById('klas-aanmaken-form').style.display = 'block';
}

window.annuleerKlasAanmaken = function() {
    document.getElementById('klas-aanmaken-form').style.display = 'none';
    document.getElementById('klas-naam').value = '';
}

window.maakKlasAan = async function() {
    const naam = document.getElementById('klas-naam').value;

    if (!naam) {
        alert("Vul alstublieft een naam in voor de klas.");
        return;
    }

    try {
        const response = await fetch('/api/Klas', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ naam: naam })
        });

        if (response.ok) {
            alert("Klas succesvol aangemaakt!");
            annuleerKlasAanmaken();
            laadKlassen();
        } else {
            alert("Fout bij aanmaken klas");
        }
    } catch (error) {
        console.error("Fout", error);
        alert("Netwerkfout bij aanmaken klas.");
    }
}

window.laadKlassen = async function() {
    try {
        const response = await fetch('/api/Klas/');
        if (!response.ok) throw new Error('Fout bij laden klassen');
        const klassen = await response.json();
        const template = Handlebars.compile(document.getElementById('klas-template').innerHTML);
        document.getElementById('app-container').innerHTML = template({ klassen: klassen });
    } catch (error) {
            console.error("Fout bij laden klassen:", error);
            alert("Fout bij laden klassen. Probeer het later opnieuw.");
    }
}

window.bekijkKlasDetails = async function(id) {
    try {
        const response = await fetch(`/api/Klas/${id}`);
        if (!response.ok) throw new Error('Fout bij laden klas details');
        const klas = await response.json();
        const template = Handlebars.compile(document.getElementById('klas-details-template').innerHTML);
        document.getElementById('app-container').innerHTML = template({ klas: klas });
    } catch (error) {
        console.error("Fout bij laden klas details:", error);
        alert("Fout bij laden klas details. Probeer het later opnieuw.");
    }
}

window.laadLeerlingResultaten = async function() {
    try {
        const response = await fetch(`/api/Resultaat/mijnresultaten`);
        if (!response.ok) throw new Error('Fout bij laden leerling resultaten');
        const resultaten = await response.json();
        const template = Handlebars.compile(document.getElementById('leerling-dashboard-template').innerHTML);
        document.getElementById('app-container').innerHTML = template({ resultaten: resultaten });
    } catch (error) {
        console.error("Fout bij laden leerling resultaten:", error);
        alert("Fout bij laden leerling resultaten. Probeer het later opnieuw.");
    }
}
window.verwijderKlas = async function(id) {
    if (confirm("Weet je zeker dat je deze klas wilt verwijderen?")) {
        try {
            const response = await fetch(`/api/Klas/${id}`, { method: 'DELETE' });
            if (response.ok) {
                alert("Klas succesvol verwijderd!");
                laadKlassen();
            } else {
                alert("Fout bij verwijderen klas");
            }
        } catch (error) {
            console.error("Fout bij verwijderen klas:", error);
            alert("Netwerkfout bij verwijderen klas.");
        }
    }
}

window.toonLeerlingToevoegenForm = function(klasId) {
    const form = document.getElementById('leerling-toevoegen-form');
    if (form) {
        form.style.display = 'block';
        form.dataset.klasId = klasId;
        document.getElementById('leerling-email').value = '';
    }
}

window.annuleerLeerlingToevoegen = function() {
    const form = document.getElementById('leerling-toevoegen-form');
    if (form) {
        form.style.display = 'none';
        document.getElementById('leerling-email').value = '';
    }
}

window.voegLeerlingToe = async function() {
    const form = document.getElementById('leerling-toevoegen-form');
    const klasId = form?.dataset.klasId;
    const email = document.getElementById('leerling-email').value;

    if (!email) {
        alert("Vul alstublieft een e-mailadres in voor de leerling.");
        return;
    }

    try {
        const response = await fetch(`/api/Klas/${klasId}/leerling`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ 
                email: email,
                naam: email,
                niveau: 1
            })
        });

        if (response.ok) {
            alert("Leerling succesvol toegevoegd!");
            annuleerLeerlingToevoegen();
            router.navigeer('klas-details', { klasId: klasId });
        } else {
            const error = await response.text();
            console.error("Fout bij toevoegen leerling:", error);
            alert("Fout bij toevoegen leerling: " + error);
        }
    } catch (error) {
        console.error("Netwerkfout bij toevoegen leerling:", error);
        alert("Netwerkfout bij toevoegen leerling.");
    }
}

window.verwijderLeerling = async function(klasId, leerlingId) {
    if (confirm("Weet je zeker dat je deze leerling wilt verwijderen?")) {
        try {
            const response = await fetch(`/api/Klas/${klasId}/leerling/${leerlingId}`, { 
                method: 'DELETE' 
            });

            if (response.ok) {
                alert("Leerling succesvol verwijderd!");
                router.navigeer('klas-details', { klasId: klasId });
            } else {
                alert("Fout bij verwijderen leerling");
            }
        } catch (error) {
            console.error("Fout bij verwijderen leerling:", error);
            alert("Netwerkfout bij verwijderen leerling.");
        }
    }
}

window.laadDocentDashboard = async function() {
    try {
        const response = await fetch('/api/Resultaat/mijnleerlingen');
        if (!response.ok) throw new Error('Fout bij laden leerlingen');
        
        const leerlingen = await response.json();
        
        const template = Handlebars.compile(document.getElementById('docent-dashboard-template').innerHTML);
        document.getElementById('app-container').innerHTML = template({ leerlingen: leerlingen });
    } catch (error) {
        console.error("Fout bij laden docent dashboard:", error);
        alert("Fout bij laden van leerlingenoverzicht.");
    }
}

window.bekijkLeerlingVoortgang = async function(leerlingId) {
    try {
        const response = await fetch(`/api/Resultaat/leerlingvoortgang/${leerlingId}`);
        if (!response.ok) {
            if (response.status === 403) {
                alert("Je hebt geen toegang tot deze leerling.");
                return;
            }
            throw new Error('Fout bij laden voortgang');
        }
        
        const data = await response.json();
        
        const template = Handlebars.compile(document.getElementById('docent-leerling-voortgang-template').innerHTML);
        document.getElementById('app-container').innerHTML = template({ 
            leerlingNaam: data.leerlingNaam,
            leerlingEmail: data.leerlingEmail,
            klasNaam: data.klasNaam,
            resultaten: data.resultaten
        });
    } catch (error) {
        console.error("Fout bij laden leerling voortgang:", error);
        alert("Fout bij laden van leerling voortgang.");
    }
}

class MuziekOefening {
    constructor(config) {
        this.naam = config.naam;
        this.suffix = config.suffix;
        this.notenPosities = config.notenPosities;
        this.svgElementId = config.svgElementId;
        this.feedbackElementId = config.feedbackElementId;
        this.tellerElementId = config.tellerElementId;

        this.goedeAntwoorden = 0;
        this.totaalVragen = 0;
        this.huidigeNootPositie = null;
        this.huidigeOefeningId = null;
        this.isBezig = false;
    }

    startNieuweVraag() {
        this.isBezig = false;
        this.huidigeNootPositie = this.notenPosities[Math.floor(Math.random() * this.notenPosities.length)];

        const trebleNote = document.getElementById("treble-note");
        const bassNote = document.getElementById("bass-note");
    
        if (trebleNote) trebleNote.style.opacity = "0";
        if (bassNote) bassNote.style.opacity = "0";

        if (this.huidigeNootPositie.balk === "treble") {
            const nootSvgElement = document.getElementById("treble-note");
            if (nootSvgElement) {
            nootSvgElement.setAttribute("cy", this.huidigeNootPositie.y);
            nootSvgElement.style.opacity = "1";
            }
        } else {
            const nootSvgElement = document.getElementById("bass-note");
            if (nootSvgElement) {
            nootSvgElement.setAttribute("cy", this.huidigeNootPositie.y);
            nootSvgElement.style.opacity = "1";
            }
        }
    }

    checkAntwoord(antwoord) {
        if (this.isBezig) return;

        const feedbackEl = document.getElementById(this.feedbackElementId);
        if (!feedbackEl) return;

        this.isBezig = true;
        feedbackEl.textContent = "Controleren..."; 
        feedbackEl.style.color = "orange";
        this.totaalVragen++;

        setTimeout(() => {
            const feedbackEl = document.getElementById(this.feedbackElementId);

            if (antwoord === this.huidigeNootPositie.letter) {
                this.goedeAntwoorden++;
                feedbackEl.textContent = "Correct! Goed gedaan.";
                feedbackEl.style.color = "green";
            } else {
                feedbackEl.textContent = "Helaas! Het juiste antwoord was: " + this.huidigeNootPositie.letter;
                feedbackEl.style.color = "red";
            }

            const tellerEl = document.getElementById(this.tellerElementId);
            if (tellerEl) tellerEl.textContent = `Beantwoord: ${this.totaalVragen} | Goed: ${this.goedeAntwoorden}`;

            setTimeout(() => {
                feedbackEl.textContent = "Welke noot is dit?";
                feedbackEl.style.color = "black";
                this.startNieuweVraag();
            }, 1000);
        }, 150);
    }

    stopEnOpslaan() {
        if (this.totaalVragen === 0) {
            alert("Je hebt nog geen vragen beantwoord!");
            return;
        }

        window.verstuurScoreNaarServer(this.goedeAntwoorden, this.totaalVragen, this.huidigeOefeningId);
        this.goedeAntwoorden = 0;
        this.totaalVragen = 0;
        this.isBezig = false;
    }

    speelOefening(id, naam) {
        this.huidigeOefeningId = id;
        router.navigeer(`speel-${this.suffix}`, { oefeningId: id, oefeningNaam: naam });
        setTimeout(() => {
            this.startNieuweVraag();
        }, 50);
    }

}



const nootPositiesTreble = [
    { naam: "C (midden)", letter: "C", y: 110, balk: "treble" },  // hulplijn onder
    { naam: "D",           letter: "D", y: 105, balk: "treble" },  // onder onderste lijn
    { naam: "E",           letter: "E", y: 100, balk: "treble" },  // op onderste lijn
    { naam: "F",           letter: "F", y: 95, balk: "treble" },   // tussen 1e en 2e lijn
    { naam: "G",           letter: "G", y: 90, balk: "treble" },   // op 2e lijn
    { naam: "A",           letter: "A", y: 85, balk: "treble" },   // tussen 2e en 3e lijn
    { naam: "B",           letter: "B", y: 80, balk: "treble" },   // op 3e lijn
    { naam: "C (hoger)",   letter: "C", y: 75, balk: "treble" },   // tussen 3e en 4e lijn
    { naam: "D (hoger)",   letter: "D", y: 70, balk: "treble" },   // op 4e lijn
    { naam: "E (hoger)",   letter: "E", y: 65, balk: "treble" },   // tussen 4e en 5e lijn
    { naam: "F (hoger)",   letter: "F", y: 60, balk: "treble" },   // op 5e lijn
    { naam: "G (hoger)",   letter: "G", y: 55, balk: "treble" },   // hulplijn boven
];

const nootPositiesBass = [
    { naam: "E (laag)",    letter: "E", y: 110, balk: "bass" },  // hulplijn onder
    { naam: "F",           letter: "F", y: 105, balk: "bass" },  // onder onderste lijn
    { naam: "G",           letter: "G", y: 100, balk: "bass" },  // op onderste lijn
    { naam: "A",           letter: "A", y: 95, balk: "bass" },   // tussen 1e en 2e lijn
    { naam: "B",           letter: "B", y: 90, balk: "bass" },   // op 2e lijn
    { naam: "C (midden)",  letter: "C", y: 85, balk: "bass" },   // tussen 2e en 3e lijn
    { naam: "D",           letter: "D", y: 80, balk: "bass" },   // op 3e lijn
    { naam: "E",           letter: "E", y: 75, balk: "bass" },   // tussen 3e en 4e lijn
    { naam: "F",           letter: "F", y: 70, balk: "bass" },   // op 4e lijn
    { naam: "G",           letter: "G", y: 65, balk: "bass" },   // tussen 4e en 5e lijn
    { naam: "A",           letter: "A", y: 60, balk: "bass" },   // op 5e lijn
    { naam: "B (hoog)",    letter: "B", y: 55, balk: "bass" },   // hulplijn boven
]

const nootPositiesBeide = [...nootPositiesTreble, ...nootPositiesBass];



const trebleOefening = new MuziekOefening({
    naam : "Treble Clef Oefening",
    suffix : "treble",
    notenPosities : nootPositiesTreble,
    svgElementId : "treble-note",
    feedbackElementId : "treble-feedback",
    tellerElementId : "treble-teller"
});

const bassOefening = new MuziekOefening({
    naam : "Bass Clef Oefening",
    suffix : "bass",
    notenPosities : nootPositiesBass,
    svgElementId : "bass-note",
    feedbackElementId : "bass-feedback",
    tellerElementId : "bass-teller"
});

const beideOefening = new MuziekOefening({
    naam : "Beide Clefs Oefening",
    suffix : "beide",
    notenPosities : nootPositiesBeide,
    svgElementId : "beide-note",
    feedbackElementId : "beide-feedback",
    tellerElementId : "beide-teller"
});


let actieveOefening = null;

window.checkAntwoord = function(antwoord) {
    if (actieveOefening) {
        actieveOefening.checkAntwoord(antwoord);
    }
}

window.stopEnOpslaan = function() {
    if (actieveOefening) {
        actieveOefening.stopEnOpslaan();
    }
}

window.speelOefening = function(id, naam, oefeningType) {
    if (oefeningType.toLowerCase().includes('treble')) {
        actieveOefening = trebleOefening;
    } else if (oefeningType.toLowerCase().includes('bass')) {
        actieveOefening = bassOefening;
    } else if (oefeningType.toLowerCase().includes('beide')) {
        actieveOefening = beideOefening;
    } else {
        console.error("Onbekend oefeningtype:", oefeningType);
        return;
    }
    
    if (actieveOefening) {
        actieveOefening.speelOefening(id, naam);
    }
}



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