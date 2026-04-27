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
        if (scherm === 'oefeningen') render("oefeningen-menu-template");
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


window.speelOefening = function(id, naam) {
    router.navigeer('speel-treble', { oefeningId: id, oefeningNaam: naam });
};

window.verstuurScore = async function(oefeningId) {
    const resultaat = {
        score: 95,
        leerlingId: 1,
        oefeningId: oefeningId,
        datetime: new Date().toISOString()
    };

    const response = await fetch('/api/resultaat', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(resultaat)
    });

    if (response.ok) {
        alert("Top! Je score is opgeslagen.");
        router.navigeer('oefeningen');
    } else {
        alert("Oeps, er ging iets mis bij het opslaan van je score.");
    }
};

document.addEventListener("DOMContentLoaded", () => {
    const url = window.location.href.toLowerCase();

    // Check of we op de Piano pagina zijn
    if (document.getElementById('app-container')) {
        router.navigeer('home');
    } 
    // Check of we op de Account pagina zijn
    else if (document.getElementById('account-app-container')) {
        if (url.includes('register')) {
            router.navigeer('register');
        } else if (url.includes('login')) {
            router.navigeer('login');
        } else {
            router.navigeer('account-start');
        }
    }
});