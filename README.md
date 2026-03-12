# DP2: opdrachtomschrijving
Je krijgt een risicoanalysedocument (template) met daarin als bijlage de requirements (zie ELO), en een (imperfecte) codebase met een contactpagina (daar zit je nu, hoi!👋). Je voert een risicoanalyse uit op de gegeven codebase en formuleert maatregelen om de kans en impact van de risico’s te minimaliseren. Je houdt in je analyse rekening met de toegankelijkheids- en beveiligingsaspecten die van belang zijn bij gebruikersinvoer via formulieren en maakt daarvoor gebruik van de WCAG-richtlijnen. Je legt je risicoanalyse voor aan de opdrachtgever (de docent) en implementeert na goedkeuring de maatregelen in de codebase, waarbij je bewaakt dat de requirements voor de contactpagina nageleefd worden.

Naast de aanpassingen die nodig zijn om de toegankelijkheid en veiligheid te garanderen, verwacht de opdrachtgever dat je je best doet om de codebase netjes en leesbaar te houden (of eigenlijk maken😉, want dat is momenteel niet overal zo). Je hoeft niet al deze wijzigingen op te nemen als maatregelen, maar brengt de verbeteringen aan terwijl je de maatregelen implementeert. Pas hiervoor de kennis toe die je in de afgelopen weken hebt opgedaan.

---

## Repository-analyse: wat doet elk bestand?

Dit is een ASP.NET Core MVC-webapplicatie (targeting .NET 8) met een contactpagina die via e-mail berichten verstuurt.

### Projectstructuur

```
Portfoliowebsite/
├── Controllers/
│   ├── ContactController.cs
│   └── HomeController.cs
├── Models/
│   └── ErrorViewModel.cs
├── Services/
│   ├── IEmailSender.cs
│   └── SmtpEmailSender.cs
├── Views/
│   ├── Contact/
│   │   ├── Index.cshtml
│   │   └── Thanks.cshtml
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   ├── Shared/
│   │   ├── Error.cshtml
│   │   ├── _Layout.cshtml
│   │   ├── _Layout.cshtml.css
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
│   ├── css/
│   │   └── site.css
│   └── js/
│       └── site.js
├── appsettings.json
├── appsettings.Development.json
├── Portfoliowebsite.csproj
├── Portfoliowebsite.sln
└── Program.cs
```

---

### Controllers

#### `Controllers/ContactController.cs`
Beheert de contactpagina. Bevat drie acties:
- `Index()` (GET) – toont het contactformulier.
- `Index(...)` (POST) – ontvangt de formulierdata (naam, e-mail, onderwerp, bericht), stuurt via `IEmailSender` een e-mail, slaat de gegevens op in `TempData` en stuurt de gebruiker door naar de bedankpagina.
- `Thanks()` – toont de bedankpagina na het versturen van het formulier.

#### `Controllers/HomeController.cs`
Beheert de algemene pagina's van de website:
- `Index()` – toont de homepage.
- `Privacy()` – toont de privacypagina.
- `Error()` – toont een foutpagina bij een niet-afgehandelde uitzondering (gebruikt `ErrorViewModel`).

---

### Models

#### `Models/ErrorViewModel.cs`
Eenvoudig model voor de foutpagina. Bevat:
- `RequestId` – de unieke ID van het HTTP-verzoek, handig bij het debuggen.
- `ShowRequestId` – een berekende property die `true` geeft als `RequestId` een waarde heeft.

---

### Services

#### `Services/IEmailSender.cs`
Een interface die de methode `SendAsync(name, email, subject, message)` definieert. Door gebruik van een interface kan de implementatie eenvoudig worden uitgewisseld (bijv. voor testdoeleinden).

#### `Services/SmtpEmailSender.cs`
De concrete implementatie van `IEmailSender`. Verstuurt e-mails via SMTP (geconfigureerd voor Mailtrap, een e-mailtestdienst). Let op: de SMTP-credentials staan momenteel leeg en moeten worden ingevuld.

---

### Views

#### `Views/Contact/Index.cshtml`
Het HTML-contactformulier. Bevat velden voor naam, e-mail, onderwerp en bericht. Heeft een verborgen "honeypot"-veld (`website`) om bots te detecteren. Bevat client-side validatie-elementen (`nameErr`, `emailErr`, `msgErr`) en een `liveStatus`-div voor schermlezer-meldingen.

#### `Views/Contact/Thanks.cshtml`
De bedankpagina die wordt getoond nadat het formulier is verstuurd. Toont de naam, het e-mailadres en het bericht van de gebruiker via `TempData`. **Let op:** gebruikt momenteel `Html.Raw()`, wat een XSS-risico vormt.

#### `Views/Home/Index.cshtml`
De homepage van de website. Toont een welkomsttekst met een link naar de ASP.NET Core documentatie.

#### `Views/Home/Privacy.cshtml`
De privacypagina. Momenteel leeg (geen inhoud aanwezig).

#### `Views/Shared/_Layout.cshtml`
De gedeelde lay-out die door alle pagina's wordt gebruikt. Bevat de HTML-structuur (head, navigatiebalk, footer), laadt Bootstrap en de projectspecifieke CSS en JavaScript.

#### `Views/Shared/_Layout.cshtml.css`
Scoped CSS specifiek voor de `_Layout.cshtml`. Momenteel leeg.

#### `Views/Shared/Error.cshtml`
De foutpagina. Toont een generieke foutmelding en optioneel de `RequestId` als die beschikbaar is.

#### `Views/Shared/_ValidationScriptsPartial.cshtml`
Een partial view die de jQuery Unobtrusive Validation-scripts laadt voor client-side formuliervalidatie op basis van data-annotaties.

#### `Views/_ViewImports.cshtml`
Importeert namespaces (`Portfoliowebsite`, `Portfoliowebsite.Models`) en voegt Tag Helpers toe voor alle views.

#### `Views/_ViewStart.cshtml`
Stelt voor alle views de standaard lay-out in op `_Layout`.

---

### wwwroot (statische bestanden)

#### `wwwroot/css/site.css`
De projectspecifieke CSS. Stelt de basisstijl in (lettertype, kleuren, formulierelementen, knopstijl, honeypot-verberging, foutstijlen). **Let op:** de regel `*:focus { outline: none; box-shadow: none; }` verwijdert de zichtbare focusindicator voor alle elementen. Dit is een **kritieke toegankelijkheidsschending** van WCAG 2.4.7 (Focus Visible): gebruikers die enkel het toetsenbord gebruiken kunnen niet meer zien welk element actief is, waardoor de website voor hen onbruikbaar wordt.

#### `wwwroot/js/site.js`
De projectspecifieke JavaScript. Bevat:
- Onderdrukking van het contextmenu (rechtermuisknop) – een toegankelijkheidsprobleem.
- Onderdrukking van de Tab-toets – een **kritieke schending van WCAG 2.1.1 (Keyboard)**: toetsenbordgebruikers kunnen niet meer door interactieve elementen navigeren met Tab, waardoor de website voor hen volledig ontoegankelijk wordt.
- Een eenvoudige e-mailvalidatiefunctie (`naiveEmailCheck`) die enkel controleert op aanwezigheid van `@`.
- Client-side validatie van naam, e-mail en bericht met inline foutmeldingen.
- Honeypot-detectie: als het verborgen veld (`website`) is ingevuld, wordt het formulier niet verstuurd.

---

### Configuratiebestanden

#### `appsettings.json`
De hoofdconfiguratie van de applicatie. Bevat logniveaus en `AllowedHosts`.

#### `appsettings.Development.json`
Overschrijft instellingen in de ontwikkelomgeving (bijv. hogere loginformatie voor debugging).

#### `Portfoliowebsite.csproj`
Het projectbestand voor MSBuild/.NET. Definieert het doelframework (net8.0), en schakelt nullable reference types en impliciete usings in.

#### `Portfoliowebsite.sln`
Het Visual Studio Solution-bestand. Groepeert het project zodat het vanuit Visual Studio geopend en gebouwd kan worden.

#### `Program.cs`
Het opstartpunt van de applicatie. Configureert:
- MVC met views.
- CORS (momenteel te permissief: alle origins, headers en methoden zijn toegestaan).
- `IEmailSender` als singleton service (gebruikt `SmtpEmailSender`).
- De middleware-pipeline (foutafhandeling, HSTS, statische bestanden, routing, autorisatie).
- De standaard route: `{controller=Home}/{action=Index}/{id?}`.

**Let op:** HTTPS-omleiding (`app.UseHttpsRedirection()`) is uitgecommentarieerd.

---

### `.gitignore`
Sluit bestanden uit van versiebeheer die niet ingecheckt hoeven te worden, zoals build-artefacten (`bin/`, `obj/`), IDE-bestanden en tijdelijke bestanden.
