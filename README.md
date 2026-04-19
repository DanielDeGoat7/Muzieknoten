# 🎹 Muzieknoten Project
Een interactieve webapplicatie gebouwd met **ASP.NET Core MVC** en **Tone.js** om gebruikers te helpen muziektheorie te leren door middel van een virtuele piano en oefeningen.

## 🚀 Technologie Stack
* **Backend:** ASP.NET Core v10 MVC (C#)
* **Frontend:** HTML5, CSS3, JavaScript
* **Audio Engine:** Tone.js (Web Audio API)
* **Database:** SQLite (Entity Framework Core)

## 🔄 Gitflow Workflow
We gebruiken het Gitflow-model om de stabiliteit van de code te waarborgen.

| Branch | Doel |
| :--- | :--- |
| **main** | Productie-klare code. Merges alleen vanuit `release/*` en `hotfix/*`. |
| **development** | Lopende ontwikkeling. Verzamelpunt voor alle features vóór een release. |
| **feature/naam** | Nieuwe functies of taken (bijv. de piano-interface). Tak af van `development`. |
| **fix/naam** | Voor reguliere bugfixes tijdens de ontwikkeling. Tak af van `development`. |
| **hotfix/naam** | Voor urgente bugfixes in de live (`main`) omgeving. |
| **release/versie** | Voorbereiding van een nieuwe productie-release (bijv. einde sprint). |

## 🏷️ Branch Naamgeving
Houd je aan de volgende prefixen voor een overzichtelijke historie:
* **Features:** `feature/naam` (bijv. `feature/notedle`)
* **Bugfixes:** `fix/bug-naam` (bijv. `fix/audio-vertraging`)
* **Documentatie:** `docs/naam` (bijv. `docs/c4-modellen`)

## 💬 Commit Message Conventies
Gebruik de volgende types in je commit berichten:
* `feat` — Een nieuwe functie.
* `fix` — Een bugfix.
* `docs` — Wijzigingen in de documentatie.
* `style` — Opmaak, witruimte, missende puntkomma's; geen logische codewijziging.
* `chore` — Aanpassingen aan tools of libraries (bijv. update Tone.js).

## 🔐 Beveiliging & Mitigatie
In dit project zijn specifieke beveiligingsmaatregelen genomen op basis van de uitgevoerde STRIDE-analyse. Hieronder staan mitigaties beschreven:

1. Verbindingsbeveiliging (Dreiging ID 4)
Dreiging: Denial of Service/Tampering op de dataflow door onbeveiligde verbindingen.
Implementatie:
* In Program.cs is app.UseHttpsRedirection() geactiveerd om een beveiligde verbinding af te dwingen.
* In launchSettings.json is het onbeveiligde HTTP-profiel verwijderd.
* Alle communicatie verloopt via een versleutelde HTTPS-verbinding (TLS 1.3), wat interceptie voorkomt.

2. Data-integriteit & Validatie (Dreiging ID 6)
Dreiging: Manipulatie van score-data (Tampering) tijdens het verzenden naar de server.
Implementatie:
* Server-side check: In ResultaatController.cs is een validatie ingebouwd die controleert of de score binnen de toegestane range valt.
* SQL-injectie preventie: Door gebruik te maken van Entity Framework Core worden queries geparametriseerd uitgevoerd, wat SQL-injectie onmogelijk maakt.
* Frontend Feedback: In site.js is foutafhandeling toegevoegd (else-statement) om gebruikers te informeren wanneer data niet correct verwerkt kan worden.

### 📄 Zie ook: Technisch Ontwerp – Hoofdstuk 4.3 Mitigaties.

## 📦 Release Proces
1. **Samenvoegen:** Zodra een sprint klaar is, maken we een `release/<versie>` aan vanaf `development`.
2. **Testen:** De laatste tests worden uitgevoerd op de release branch.
3. **Productie:** Na goedkeuring mergen we de branch naar `main` en wordt er een tag geplaatst.


