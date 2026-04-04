# Projektanalyse: PixelCompetitionServer

Erstellt am: 2026-04-04
Repository: https://github.com/PixelCompetition/PixelCompetitionServer

---

## 2.1 Projektzweck

PixelCompetitionServer ist ein TCP-basierter Spieleserver für **Pixel-Wettbewerbe** (angelehnt an das bekannte PixelFlut-Konzept). Teilnehmer verbinden sich per TCP-Client mit dem Server und senden textbasierte Befehle, um Pixel auf einer gemeinsamen Leinwand (Canvas) zu setzen. Die Leinwand wird als Raw-Video-Stream (RGB24) an FFmpeg weitergeleitet, das den Stream z. B. als H.264/FLV für Livestreaming encodiert.

Das Projekt besteht aus drei Hauptkomponenten:

1. **C#-Server** (`src/dotnet/`): Der eigentliche Competition-Server mit verschiedenen Wettbewerbsmodi (direktes Zeichnen, Turtle-Grafik, Polar-Koordinaten, Rätsel-basierte Wettbewerbe).
2. **Python-Vorbereitungsskripte** (`src/python/`): Werkzeuge zur Generierung von Rätsel-Daten (Primfaktorzerlegung, OCR-Bilder, Wortlisten) und Zeitplan-Konvertierung, sowie Test-Clients.
3. **PHP-Webseite** (`web/`): Informationsseite mit Regeln, Livestream-Einbettung, Impressum und Anleitungen.

### Abweichungen zwischen Dokumentation und Code

Die vorhandene Dokumentation ist minimal:
- **README.md**: Enthält eine kurze Beschreibung des PixelCompetition-Konzepts (12 Zeilen).
- **src/python/preparation/ReadMe.md**: Beschreibt den Vorbereitungs-Workflow auf Deutsch, ist inhaltlich korrekt aber nicht vollständig.
- Es fehlt jegliche Dokumentation zur Server-Architektur, zum Protokoll, zur Deployment-Anleitung und zu den einzelnen Wettbewerbsmodi.

---

## 2.2 Einstiegspunkte

### C#-Einstiegspunkte

| Einstiegspunkt | Dateipfad | Beschreibung |
|---|---|---|
| Server-Hauptprogramm | `src/dotnet/PixelCompetitionServer/PixelCompetitionServerApp/Program.cs` | Konsolenanwendung: Liest XML-Konfiguration, erstellt `PixelCompetitionServerApp`-Instanz und startet den Server (`app.run()`). |
| WinForms-Client | `src/dotnet/MyPixelCompetition/MyPixelCompetition/MyPixelCompetition/Program.cs` | GUI-Anwendung: Startet ein WinForms-Fenster, das `ICanvas` implementiert und einen eingebetteten PixelCompetitionServer betreibt. |

### Python-Einstiegspunkte (Standalone-Skripte)

| Skript | Dateipfad | Beschreibung |
|---|---|---|
| Zeitplan-Konvertierung | `src/python/preparation/prepareSchedule.py` | Konvertiert CSV-Zeitpläne in XML- und Wiki-Tabellen-Format. |
| Wortlisten-Erstellung | `src/python/preparation/prepareWords.py` | Erstellt `words.txt` aus Leipzig-Korpus-Dateien. |
| Font-Erkennung | `src/python/preparation/findFonts.py` | Sucht TTF-Schriftarten und generiert Vorschaubilder. |
| Primfaktor-Rätsel | `src/python/preparation/prepareC500.py` | Generiert Primfaktorzerlegungs-Rätsel mit NumPy. |
| OCR-Bild-Rätsel | `src/python/preparation/prepareC600.py` | Generiert OCR-Bildrätsel mit CAPTCHA-Bibliothek. |
| Test-Client (Force) | `src/python/testclients/B001forceClient.py` | Zeichnet Streifen und Zufallspixel mit 128 parallelen Sockets. |
| Test-Client (Multi) | `src/python/testclients/B001multiClient.py` | Lädt ein Logo-Bild und zeichnet es wiederholt mit 12 Sockets auf die Canvas. |

### Web-Einstiegspunkt

| Einstiegspunkt | Dateipfad | Beschreibung |
|---|---|---|
| Webseite | `web/index.php` | PHP-Einstiegspunkt: Lädt Template-Includes fuer Navigation, Content-Sektionen und Footer. |

---

## 2.3 Technologie-Stack

### Programmiersprachen und Frameworks

| Sprache | Einsatzbereich | Framework/Runtime |
|---|---|---|
| C# | Server-Kern, Competition-Logik, Client-Handling | .NET 5.0 |
| Python 3 | Datenaufbereitung, Raetsel-Generierung, Test-Clients | Standalone-Skripte |
| PHP | Webseite | Serverseitiges Rendering |
| JavaScript | Web-Frontend | jQuery 3.2.1 + Bootstrap 4 |
| HTML/CSS | Web-Frontend | Bootstrap 4 |

### Technologie-Mischung

Das Projekt ist ein klassisches Mehrsprachenprojekt:
- **C#-Backend** bildet den Kern (Server + Competition-Logik).
- **Python** dient der Offline-Datenvorbereitung und als Test-Client (kein Laufzeit-Bezug zum Server außer TCP-Verbindung).
- **PHP/JS-Web** ist eine eigenständige Informationsseite ohne direkte Kopplung an den Server.

### Externe Bibliotheken und Packages

**C# (.NET 5.0)** — laut `.csproj`-Dateien:
- Keine externen NuGet-Pakete. Das Projekt nutzt ausschließlich .NET-Standardbibliotheken (`System.Net.Sockets`, `System.Drawing`, `System.Numerics`, `System.Reflection`, `System.Xml`).

**Python** — implizit verwendet (kein `requirements.txt` vorhanden):

| Package | Verwendung |
|---|---|
| NumPy | Sieb des Eratosthenes fuer Primzahl-Generierung (`prepareC500.py`), Bild-Array-Verarbeitung (`B001multiClient.py`) |
| Pillow (PIL) | Bildgenerierung fuer OCR-Raetsel (`PostProcessorImage.py`), Bild laden im Test-Client |
| captcha | CAPTCHA-Bildgenerierung (`PostProcessorCaptcha.py`) |

**JavaScript/Web** — eingebunden als Dateien (kein Package-Manager):

| Bibliothek | Version | Verwendung |
|---|---|---|
| Bootstrap | 4.x | CSS-Framework, Responsive Layout, Komponenten |
| jQuery | 3.2.1 | DOM-Manipulation, AJAX, Event-Handling |
| jQuery Migrate | 3.0.0 | Abwaertskompatibilitaet |
| Animate.css | - | CSS-Animationen |
| WOW.js | - | Scroll-getriggerte Animationen |
| Waypoints | - | Scroll-Position-Tracking |
| Retina.js | 1.1.0 | HiDPI-Bilder |
| Popper.js | - | Tooltip-Positionierung (Bootstrap-Abhaengigkeit) |
| jQuery BackStretch | - | Hintergrundbilder |

---

## 2.4 Architektur

### Verzeichnisstruktur und Zustaendigkeiten

```
src/
  dotnet/
    PixelCompetitionServer/
      PixelCompetitionServerApp/     -> Server-Anwendung (Entry Point, Konfiguration, FFmpeg-Configs)
      PixelCompetitionServerLib/     -> Kernbibliothek
        A/A001/                      -> Kategorie A: Basis-Wettbewerbe (NullCompetition)
        B/B001/, B/B003/             -> Kategorie B: Mittlere Schwierigkeit (PixelFlut, TurtleBot)
        C/C050/, C100/, C180/,       -> Kategorie C: Fortgeschritten/Raetsel-basiert
          C310/, C500/, C600/           (SnailBot, PolarCoordinate, SquareRoot, Stephan, PrimeFactors, OcrImage)
        Common/                      -> Meta-Wettbewerb (Help, Lang, Competition-Info)
        Move/                        -> Stub (nicht implementiert)
        Riddle/                      -> Raetsel-Subsystem (Riddle, RiddleCompetition, Factories)
    MyPixelCompetition/              -> WinForms-GUI-Client (Entwicklungswerkzeug)
  python/
    preparation/                     -> Offline-Datenvorbereitung
    testclients/                     -> Python-Test-Clients (B001forceClient, B001multiClient)
    notebooks/                       -> Jupyter Notebook (PixelFlut-Client-Beispiel)
web/
  inc/                               -> PHP-Includes (Template-Teile)
    content/                         -> Inhalts-Sektionen (Start, Stream, Rules, etc.)
  assets/                            -> Custom CSS/JS/Bilder
  css/, js/                          -> Bootstrap-Dateien
doc/
  img/                               -> Screenshots, Logos, Flux-Bilder
  use_logo/                          -> Logo in verschiedenen Groessen
```

### Erkannte Architekturmuster

1. **Factory Pattern (zentral)**: Jeder Wettbewerb hat eine eigene Factory-Klasse (`ICompetitionFactory` -> `BaseCompetitionFactory`). Die Factories werden per **Reflection** aus der Assembly geladen (`CompetitionFactoryLoader`), basierend auf Namespace-Konventionen (z. B. `PixelCompetitionServerLib.B.B001` -> Wettbewerb "B001").

2. **Strategy Pattern**: Verschiedene Wettbewerbsimplementierungen (`ICompetition`) sind austauschbar. Der `PixelCompetitionClientHandler` routet Befehle an die jeweils aktive Competition-Instanz.

3. **Template Method Pattern**: `RiddleCompetitionFactory` definiert den Ablauf fuer Raetsel-Wettbewerbe. Subklassen (`SquareRootFactory`, `PrimeFactorsFactory`, `StephanFactory`, `RiddleCompetitionTextFactory`) implementieren spezifische Raetsel-Generierung.

4. **Abstract Factory**: `BaseCompetitionFactory` als abstrakte Factory, die Wettbewerbs-Instanzen (`generateCompetition()`) und Metadaten (`CompetitionMetaData`) erzeugt.

5. **Client-Server-Architektur**: TCP-basierter Server mit Thread-per-Client-Modell. Jeder Client erhaelt eigene Competition-Instanzen, teilt sich aber die globale Canvas.

6. **Schichtenarchitektur (implizit)**:
   - **Netzwerk-Schicht**: `PixelCompetitionServer`, `PixelCompetitionClientHandler`, `ResponseWriter`
   - **Geschaeftslogik**: Competition-Klassen, Scheduler
   - **Rendering**: `ICanvas`, `RawVideoCanvas`, `ImageSender*`
   - **Konfiguration**: `PixelCompetitionServerConfig`

---

## 2.5 Stilanalyse

### C# — Editorconfig-Konformitaet

Das Projekt definiert eine umfangreiche `.editorconfig`. Allgemein wird der C#-Stil gut eingehalten, es gibt aber folgende Verstoesse:

| Datei | Zeile | Verstoss |
|---|---|---|
| `PixelCompetitionServerApp.cs` | 17 | Privates Feld `Starter` beginnt mit Grossbuchstabe — sollte laut Konvention `_starter` oder `starter` heissen. |
| `BaseCompetition.cs` | 68 | String-Konkatenation `redToken+""+blueToken` — fehlender Leerzeichenseparator, vermutlich Tippfehler. |
| `PixelCompetitionClientHandler.cs` | 220-223 | Leere Methode `releaseUnmanagedResources()` mit TODO — toter Code. |
| `FFMpegStarter.cs` | 52 | Ungenutzte String-Konkatenation von Dictionary-Keys. |
| `FFMpegStarter.cs` | 101 | Kommentar deutet auf unerreichbaren Code-Pfad hin. |
| `RawVideoCanvas.cs` | 25 | TODO-Kommentar "double check calculation" — Unsicherheit in der Implementierung. |
| `TurtleBot.cs` | 19 | Hilfetext-Bug: "left" steht sowohl bei L als auch bei R (sollte "right" fuer R sein). |

### Python — PEP 8

| Datei | Zeile | Verstoss |
|---|---|---|
| `generalVars.py` | 4-23 | Verwendet `exec()` statt regulaerer Imports — unsicher und unueblich. |
| `findFonts.py` | 5 | Ebenfalls `exec()` fuer Imports. |
| `B001multiClient.py` | 6 | Verwendet `exec(open(...).read())` fuer Import von generalVars — gleiche Problematik. |
| `PostProcessorImage.py` | 25 | Verwendet `ImageDraw.textsize()` — deprecated seit Pillow 10.0, entfernt in neueren Versionen. |
| `PostProcessorCaptcha.py` | 20 | Direkter Zugriff auf privates Attribut `_truefonts`. |
| `PreparerWords.py` | 19 | `createRiddle()` hat `pass` statt `raise NotImplementedError`. |
| `prepareSchedule.py` | 3-6 | Globale Variablen ohne Modul-Organisation. |
| Allgemein | - | Kein `requirements.txt` vorhanden — Abhaengigkeiten sind nicht deklariert. |

### JavaScript

| Datei | Zeile | Verstoss |
|---|---|---|
| `scripts.js` | 10 | `$.getJSON()` ohne Fehlerbehandlung. |
| Allgemein | - | jQuery 3.2.1 ist veraltet (aktuell: 3.7+). |

---

## 2.6 Pattern-Analyse

### Verwendete Patterns

| Pattern | Wo verwendet | Bewertung |
|---|---|---|
| **Factory Pattern** | `ICompetitionFactory`, `BaseCompetitionFactory`, alle `*Factory`-Klassen | Konsequent umgesetzt. Reflection-basiertes Laden ueber Namespace-Konventionen ist elegant und erweiterbar. |
| **Strategy Pattern** | `ICompetition`-Implementierungen | Sauber implementiert. Jeder Wettbewerb kapselt seine eigene Befehlsverarbeitung. |
| **Template Method** | `RiddleCompetitionFactory` -> `RiddleCompetitionTextFactory`, `SquareRootFactory`, `StephanFactory` | Gute Abstraktion fuer Raetsel-Wettbewerbe mit gemeinsamer Infrastruktur (Caching, Locking). |
| **Observer (implizit)** | Server-Client-Broadcast bei Wettbewerbswechsel | Funktional vorhanden, aber nicht als explizites Observer-Pattern implementiert. |
| **Singleton (implizit)** | `CompetitionFactoryLoader` cacht Factory-Instanzen | Kein echtes Singleton, aber Factory-Instanzen werden einmalig erzeugt und wiederverwendet. |

### Pattern-Verletzungen

| Verletzung | Wo | Moegliche Ursache |
|---|---|---|
| **MoveCompetition/MoveCompetitionFactory**: Enthaelt nur `throw new NotImplementedException()` | `Move/` | Geplantes Feature, das nie fertiggestellt wurde. Vermutlich Zeitdruck oder Prioritaetswechsel. |
| **CommonCompetition** bricht das Single-Responsibility-Prinzip: Kombiniert Help, Lang-Auswahl und Competition-Info in einer Klasse (129 Zeilen) | `Common/CommonCompetition.cs` | Gewachsener Code — Meta-Befehle wurden sukzessive ergaenzt, ohne sie in eigene Handler aufzuteilen. |
| **PixelCompetitionClientHandler** ist mit 257 Zeilen zu gross und uebernimmt zu viele Aufgaben (Protokoll-Parsing, Competition-Routing, Lifecycle-Management, Dispose-Pattern) | `PixelCompetitionClientHandler.cs` | Typisches Anzeichen fuer gewachsenen Code ohne regelmaessiges Refactoring. |
| **Inkonsistente Fehlerbehandlung**: Manche Methoden fangen Exceptions, andere lassen sie durchreichen — kein einheitliches Muster erkennbar | Diverse | Fehlende Konvention / gewachsener Code. |
| **Python `exec()`-Pattern** fuer Imports widerspricht dem Explicit-is-better-than-implicit-Prinzip | `generalVars.py`, `findFonts.py`, `B001multiClient.py` | Vermutlich ein Versuch, Import-Pfade dynamisch zu gestalten. Deutet auf fehlendes Python-Packaging-Wissen hin. |
| **Fehlende Dependency Injection**: Abhaengigkeiten (Canvas, Config, Factories) werden direkt in Konstruktoren erzeugt oder durchgereicht | Diverse | Das Projekt nutzt kein DI-Framework — bei der aktuellen Groesse noch vertretbar, erschwert aber Testbarkeit. |
| **StephanFactory (C310)**: Platzhalter-Wettbewerb ohne eigene Logik — erbt von `RiddleCompetitionTextFactory` und definiert nur den Ordnernamen | `C/C310/StephanFactory.cs` | Offenbar reserviert fuer eine zukuenftige Implementierung durch "Stephan". |

---

## 2.7 Tiefergehende Architekturanalyse (Datei-/Klassenebene)

### 2.7.1 Klassenhierarchie

```
ICompetition (interface)
  +-- BaseCompetition (abstract)
       +-- PixelFlut (B.B001)          — Direktes Pixel-Setzen
       +-- TurtleBot (B.B003)          — Turtle-Grafik (F/L/R)
       +-- SnailBot (C.C050)           — Fibonacci-Spirale
       +-- PolarCoordinate (C.C100)    — Polar-Koordinaten
       +-- NullCompetition (A.A001)    — Leer-Implementierung
       +-- RiddleCompetition (Riddle)  — Raetsel-Protokoll (ASK/PX)
       +-- MoveCompetition (Move)      — Stub (NotImplementedException)

ICompetitionFactory (interface)
  +-- BaseCompetitionFactory (abstract)
       +-- PixelFlutFactory
       +-- TurtleBotFactory
       +-- SnailBotFactory
       +-- PolarCoordinateFactory
       +-- NullCompetitionFactory
       +-- CommonCompetitionFactory    — Meta-Wrapper (help/lang/competition)
       +-- RiddleCompetitionFactory (abstract)
            +-- RiddleCompetitionTextFactory (abstract)
                 +-- SquareRootFactory (C180)
                 +-- StephanFactory (C310)    — Platzhalter
                 +-- PrimeFactorsFactory (C500)
                 +-- OcrImageFactory (C600)
       +-- MoveCompetitionFactory      — Stub

ICanvas (interface)
  +-- RawVideoCanvas                   — RGB24-Byte-Array im Speicher

ImageSender (abstract)
  +-- ImageSenderTcp                   — TCP-Stream an FFmpeg
  +-- ImageSenderSocket                — Named Pipe an FFmpeg (Windows)
```

### 2.7.2 Abhaengigkeiten zwischen Klassen

```
PixelCompetitionServerApp (Orchestrator)
  ├── PixelCompetitionServerConfig     — Konfiguration
  ├── CompetitionFactoryLoader         — Factory-Registry (Reflection)
  ├── PixelCompetitionServer           — TCP-Server
  ├── RawVideoCanvas (ICanvas)         — Pixel-Buffer
  ├── ImageSender (Tcp/Socket)         — Frame-Uebertragung
  ├── FfMpegStarter                    — FFmpeg-Prozess
  └── Scheduler                        — Zeitplan-Steuerung

PixelCompetitionServer
  ├── PixelCompetitionServerConfig     — Config-Referenz (readonly)
  ├── ICanvas                          — Shared Canvas
  ├── List<ICompetitionFactory>        — Aktive Factories
  └── ConcurrentBag<ClientHandler>     — Verbundene Clients

PixelCompetitionClientHandler
  ├── TcpClient                        — TCP-Verbindung
  ├── PixelCompetitionServer           — Server-Referenz
  ├── ResponseWriter                   — Gepufferter Antwort-Stream
  └── ICompetition[]                   — Pro Client eine Instanz je Factory

BaseCompetitionFactory
  ├── PixelCompetitionServer           — Server-Referenz (via assignServer)
  └── Dictionary<string, MetaData>     — Sprach-Metadaten (aus XML-Ressourcen)

RiddleCompetitionFactory
  ├── Riddle[,]                        — 2D-Raetsel-Array (x,y -> Riddle)
  ├── ConcurrentQueue<Riddle>          — Raetsel-Cache (Hintergrund-Befuellung)
  └── Task FillRiddlesTask             — Async Raetsel-Generierung
```

### 2.7.3 Threading-Modell

Das Projekt nutzt 4+ Hintergrund-Threads:

| Thread | Klasse | Aufgabe |
|---|---|---|
| TCP-Listener | `PixelCompetitionServer` | Akzeptiert neue Client-Verbindungen (async) |
| Client-Handler (je Client) | `PixelCompetitionClientHandler` | Liest/verarbeitet Befehle per Task (async/await) |
| Image-Sender | `ImageSender` | Sendet Canvas-Frames an FFmpeg (Thread + Sleep-Loop) |
| FFmpeg-Starter | `FfMpegStarter` | Startet/ueberwacht FFmpeg-Prozess (Thread) |
| Raetsel-Generator | `RiddleCompetitionFactory` | Fuellt Raetsel-Cache im Hintergrund (Task.Run) |
| Hauptschleife | `PixelCompetitionServerApp` | Scheduler-Pruefung alle 60s, Client-Cleanup |

**Synchronisationsprimitive:**
- `ConcurrentBag<T>` fuer Client-Liste
- `ConcurrentQueue<T>` fuer Raetsel-Cache
- `ConcurrentStack<T>` fuer Dateinamen-Verwaltung
- `lock(this)` in ResponseWriter, ImageSenderTcp
- `lock(riddle)` in RiddleCompetition (Antwort-Pruefung)
- `lock(_currentRiddles)` in RiddleCompetitionFactory
- `CancellationTokenSource` fuer Client-Timeouts

**Bewusste Designentscheidung:** `RawVideoCanvas.Buffer` wird **ohne Synchronisation** von allen Client-Threads gleichzeitig beschrieben. Das ist gewollt — visuelle Artefakte (Tearing) sind bei einem Pixel-Wettbewerb akzeptabel und der Performance-Gewinn ist erheblich.

### 2.7.4 Datenfluss: Client-Befehl bis Pixel im Video

```
TCP-Client sendet "PX 100 200 255 0 0\n"
  │
  ▼
PixelCompetitionClientHandler.handleAsync()
  └─ stream.ReadAsync() → Bytes empfangen
     └─ Zeilen-Puffer (_preFix) → vollstaendige Zeile extrahieren
        └─ processLine("PX 100 200 255 0 0")
           └─ Split auf Whitespace → ["PX","100","200","255","0","0"]
              └─ Fuer jede Competition in _competitions[]:
                 └─ competition.processInput(tokens, state)
                    └─ PixelFlut: Erkennt "PX", parst x/y/r/g/b
                       └─ Canvas.drawPixel(100, 200, 255, 0, 0)
                          └─ RawVideoCanvas: offset = 3*(200*1920+100)
                             Buffer[offset]=255, [+1]=0, [+2]=0
                             (kein Lock, direkt ins Byte-Array)
  │
  ▼ (parallel, unabhaengig)
ImageSender-Thread (alle 200ms bei 5fps)
  └─ writeToPipe(): stream.Write(Canvas.Buffer)
     └─ 6.220.800 Bytes (1920*1080*3) an FFmpeg senden
        └─ FFmpeg: rawvideo → H.264 → FLV-Stream
```

### 2.7.5 Konfigurations-Fluss

```
Program.Main(args)
  └─ PixelCompetitionServerConfig.readConfig(["config.xml"] + args)
     └─ Fuer jede Datei: XmlSerializer.Deserialize()
        └─ MergeHelper.copyValues() → Reflection-basiertes Property-Merging
           └─ Schedule-Listen werden kumuliert (nicht ueberschrieben)
  │
  ▼
Config verteilt sich an:
  ├─ PixelCompetitionServer  → LocalPort, ClientTimeOut, InstanceWelcome
  ├─ RawVideoCanvas          → CanvasWidth, CanvasHeight
  ├─ ImageSender             → Framerate, RawVideoTcpHost/Port
  ├─ FfMpegStarter           → FFMpegCommand, FFMpegParams (mit Template-Substitution)
  ├─ Scheduler               → Schedule-Liste
  └─ RiddleCompetitionFactory → DataFolder (fuer Raetsel-Dateien)
```

### 2.7.6 Competition-Lebenszyklus

1. **Factory-Entdeckung**: `CompetitionFactoryLoader` scannt per Reflection die Assembly nach `ICompetitionFactory`-Implementierungen. Namespace-Konvention: `PixelCompetitionServerLib.{Buchstabe}.{Nummer}` (z.B. `B.B001`).

2. **Factory-Instanziierung**: `Activator.CreateInstance()` mit parameterlosem Konstruktor. Instanzen werden gecacht.

3. **Wettbewerb starten** (`Server.startCompetition()`):
   - Alle bestehenden Clients werden geschlossen ("We are starting a new Competition.")
   - Neue Factory-Liste wird gesetzt (immer mit `CommonCompetitionFactory` als Wrapper).
   - `assignServer()` auf allen Factories → laedt Sprach-Metadaten, initialisiert Raetsel-Arrays.

4. **Client verbindet sich**: Fuer jede Factory wird `generateCompetition(canvas, writer)` aufgerufen → erzeugt eine `ICompetition`-Instanz pro Client.

5. **Befehlsverarbeitung**: `processLine()` iteriert ueber alle Competition-Instanzen. Die erste, die `true` zurueckgibt, hat den Befehl behandelt.

6. **Wettbewerbswechsel**: Scheduler prueft alle 60s ob ein neuer Zeitplan-Eintrag faellig ist → ruft `startCompetition()` mit neuen Factories auf.

### 2.7.7 Identifizierte Fehler und Risiken

| Kategorie | Beschreibung | Datei | Schwere |
|---|---|---|---|
| **Bug** | `FillRiddlesTask` wird im `finally`-Block sofort auf `null` gesetzt, bevor der Task abgeschlossen ist. Erlaubt versehentlich parallele Aufrufe. | `RiddleCompetitionFactory.cs` | Mittel |
| **Bug** | TurtleBot-Hilfetext: R-Befehl wird als "left" beschrieben statt "right". | `TurtleBot.cs:19` | Niedrig |
| **Bug** | `BaseCompetition.parseColor()`: `redToken+""+blueToken` — leerer String statt Leerzeichen als Separator. | `BaseCompetition.cs:68` | Niedrig |
| **Risiko** | `RiddleCompetitionTextFactory.createRiddles()` hat keinen try-catch um File-I/O. Fehlender DataFolder fuehrt zu leerem Raetsel-Cache. | `RiddleCompetitionTextFactory.cs` | Mittel |
| **Risiko** | `FfMpegStarter.start()` hat keinen try-catch. Fehlende FFmpeg-Installation fuehrt zu unbehandelter Exception im Hintergrund-Thread. | `FfMpegStarter.cs` | Mittel |
| **Risiko** | `ResponseWriter` setzt `_stream`/`_writer` auf `null` in `Dispose()`, aber die Stream-Objekte werden nicht explizit disposed. | `ResponseWriter.cs` | Niedrig |
| **Design** | `PixelCompetitionClientHandler.processLine()` faengt alle Exceptions pauschal — Fehler werden nur auf Console geloggt, kein strukturiertes Logging. | `PixelCompetitionClientHandler.cs` | Niedrig |
| **Design** | `RiddlePerFile = 2 ^ 14` in `RiddleCompetitionTextFactory` — C# interpretiert `^` als XOR, nicht als Potenz! Ergebnis ist 12 statt 16384. | `RiddleCompetitionTextFactory.cs` | Hoch |

### 2.7.8 Fehlerbehandlungsstrategie (Zusammenfassung)

Die Fehlerbehandlung folgt einem konsistenten aber simplen Muster:

- **Netzwerk-Fehler**: Catch + Console.WriteLine + Retry-Loop (selbstheilend)
- **Client-Fehler**: Catch + Console.WriteLine + Verbindung schliessen (Isolation)
- **Protokoll-Fehler**: Ueber `ProcessState` gesammelt, als Textnachricht an Client
- **Konfigurations-Fehler**: Catch + Console.WriteLine + Default-Werte beibehalten
- **Datei-Fehler**: Teilweise ungefangen (Risiko bei fehlendem DataFolder)
- **Kein strukturiertes Logging** — alles geht an Console.WriteLine
- **Keine Metriken oder Health-Checks**
