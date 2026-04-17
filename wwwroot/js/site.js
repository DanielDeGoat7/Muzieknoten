window.router = {
    navigeer(scherm, extraData = {}) {
        const container = document.getElementById('app-container');
        if (!container) return;

        const render = (templateId, data = {}) => {
            const element = document.getElementById(templateId);
            if (!element) {
                console.error("Template niet gevonden:", templateId);
                return;
            }
            const source = element.innerHTML;
            const template = Handlebars.compile(source);
            container.innerHTML = template(data);
        };

        if (scherm === 'oefeningen') render("oefeningen-menu-template");
        else if (scherm === 'speel-treble') render("treble-clef-template", extraData);
        else if (scherm === 'home') render("home-template");
    }
};

// Functies voor knoppen ook globaal maken
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

// Start de app zodra de pagina klaar is
document.addEventListener("DOMContentLoaded", () => {
    if (document.getElementById('app-container')) {
        router.navigeer('home');
    }
});