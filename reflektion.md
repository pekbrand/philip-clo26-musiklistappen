# Reflektion — Musiklistappen

**Namn:** Philip Ekbrand
**Datum:** 2026-11-01

---

## Vad var svårast?

det svårast är att bygga kopplingen till databasen,
egentligen inte supersvårt men lite annorlunda, gillar dock
hur man anänder parametrar för att skicka med variable.
har formaterat strängar i python innan men ska se om det finns bättre metoder för det i python också! 

---

## Vad lärde du dig?

Jag lärde mig använda paramerar, jag har knapp användt update,delete, insert innan så fick känna på det!(inte update dock)
Även hur man skapar en koppling i c# för att ansluta till en databas!

---

## Hur använde du ADO.NET i praktiken — vad kändes naturligt och vad var förvirrande?

Tänk på de fem stegen: öppna anslutning, skapa kommando, skriv SQL, skicka in värden, kör.

jag använde ADO för att ansluta till databasen skicka sql till databasen. inget känndes jätteförrvirande men är inte helt bekväm i c# ännu så blir mkt ai för att hitta komandon.
Men det som känndes väldigt naturligt var att anävnda parametrar! gillade det väldigt mycket!

---

## Varför ska man använda parameteriserade queries?

Vad kan hända om man klistrar ihop SQL med strängar i stället? (Tänk på Bobby Tables.)

det blir för att man inte skall kunna göra injections så man kan använda det för att manupilera sql koden på ett skadligt sätt.

---

## Om du hade mer tid — vad hade du lagt till eller gjort annorlunda?

Jag hade kikat in lite mer på transaktioner, dvs att eftersom databasen har flera tabeller hade jag velat kika på album och gener, skall jag lägga till en låt om man kan tvinga att lägga albumID och gener i en anna tabell.

---

## (VG) Varför delade du upp koden som du gjorde?

Resonera kring ditt designval: varför ligger databaskoden i `DbHandler` och inte i `Program.cs`? Vad hade blivit svårare om allt låg i samma fil?

mycket var ju redan gjort från forken så använde den strukturen men det hade blivit väldigt otydligt om all låg i program.cs. Jag gillade också det du visade med klassen på lektionen frågade ai för om vi kunde göra något likande för att inte öppna anslutningen varje gång och då tog den fram
""public class DbHandler : IDisposable"" just att klassen har en method för att stänga anslutningen, i mitt fall blir det när programet stängs med case 5! 

valde subquery eftersom jag redan hade mer eller mindre all kod för att skriv ut alla låtarna det vara bara att ända sql:en till att få fram alla låtarna som var över medelsnittet av längden
