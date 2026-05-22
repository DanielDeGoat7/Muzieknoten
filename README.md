# 🎹 Muzieknoten Project
Een interactieve webapplicatie gebouwd met **ASP.NET Core MVC** en **Tone.js** om gebruikers te helpen muziektheorie te leren door middel van een virtuele piano en oefeningen.

## Aan de slag
Volg onderstaande stappen om de Muzieknoten applicatie lokaal te draaien

### Systeem Vereisten
| Vereiste | Versie |
| :--- | :--- |
| **.NET** | 10 |

### Visual Studio
### Stap 1 - Repository clonen
Klik op "Clone a repository" (of ga naar File → Clone Repository)
Vul bij repository locatie in: 
`https://github.com/DanielDeGoat7/Muzieknoten.git`
Kies een lokale map en klik op "Clone"

### Stap 2 - Database aanmaken
Ga naar Tools → NuGet Package Manager → Package Manager Console
Selecteer het juiste project
Voer het volgende commando uit:
`Update-Database`

### Stap 3 - Applicatie starten
Zorg dat Piano als startup project is geselecteerd
Druk op F5 of klik op de groene "Play" knop met "https" in de werkbalk


### Visual Studio Code
### Stap 1 - Repository clonen
Open een terminal en voer uit:
`git clone https://github.com/DanielDeGoat7/Muzieknoten.git`
`cd Muzieknoten`

### Stap 2 - Database aanmaken
Voer het volgende commando uit in de projectmap:
`dotnet ef database update`

Mocht dotnet ef niet worden herkend, installeer dan de EF Core tools met:
`dotnet tool install --global dotnet-ef`

### Stap 3 - Applicatie starten
Voer uit
`dotnet run`


### Account aanmaken
Wanneer de applicatie is gestart, druk op "Registreren" en maak een account aan.




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

## 📦 Release Proces
1. **Samenvoegen:** Zodra een sprint klaar is, maken we een `release/<versie>` aan vanaf `development`.
2. **Testen:** De laatste tests worden uitgevoerd op de release branch.
3. **Productie:** Na goedkeuring mergen we de branch naar `main` en wordt er een tag geplaatst.


