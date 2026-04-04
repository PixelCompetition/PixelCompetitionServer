# PixelCompetitionServer — Projektstatus

**Status:** Aktiv
**Entscheidungsdatum:** 2026-04-04
**Repository:** https://github.com/PixelCompetition/PixelCompetitionServer

---

## Zusammenfassung der Analyseergebnisse

TCP-basierter Spieleserver fuer Pixel-Wettbewerbe (PixelFlut-Konzept). Teilnehmer verbinden sich per TCP und setzen Pixel auf einer gemeinsamen Leinwand, die als Livestream ausgegeben wird.

**Technologie-Stack:**
- C# / .NET 5.0 (Server-Kern)
- Python 3 (Datenvorbereitung, Test-Clients)
- PHP + Bootstrap 4 (Info-Webseite)

**Architektur:** Factory Pattern + Strategy Pattern mit Reflection-basierter Wettbewerbs-Entdeckung. Thread-per-Client-Modell, shared Canvas ohne Locking (gewollt).

**Staerken:**
- Saubere Factory/Strategy-Hierarchie, einfach erweiterbar
- Selbstheilendes Netzwerk-Design (Retry-Loops)
- Client-Isolation (ein fehlerhafter Client crasht nicht den Server)

**Identifizierte Probleme:**
- .NET 5.0 ist End-of-Life (Upgrade auf .NET 8+ noetig)
- Kritischer Bug: `2 ^ 14` ist XOR in C# (ergibt 12 statt 16384) in `RiddleCompetitionTextFactory`
- Bug: `FillRiddlesTask` Race Condition in `RiddleCompetitionFactory`
- Bug: TurtleBot-Hilfetext vertauscht links/rechts
- Kein Test-Framework, keine Tests vorhanden
- Kein strukturiertes Logging (nur Console.WriteLine)
- Kein `requirements.txt` fuer Python-Abhaengigkeiten
- Stub-Code: `MoveCompetition` (NotImplementedException), `StephanFactory` (Platzhalter)
- Veraltete Pillow-API (`textsize()` deprecated)
- Python verwendet unsicheres `exec()` fuer Imports

**Vollstaendige Analyse:** siehe `docs/ANALYSIS.md`

---

## Naechste geplante Schritte

1. **Test-Framework einrichten** — xUnit/NUnit/MSTest fuer C#, ggf. pytest fuer Python (Entscheidung steht noch aus)
2. **.NET-Version upgraden** — von .NET 5.0 (EOL) auf .NET 8 oder 9
3. **Kritische Bugs beheben** — `2^14` XOR-Bug, FillRiddlesTask Race Condition, TurtleBot-Hilfetext
4. **Strukturiertes Logging einfuehren** — Console.WriteLine durch ILogger ersetzen
5. **Python-Abhaengigkeiten deklarieren** — `requirements.txt` erstellen
6. **Stub-Code aufräumen** — MoveCompetition/StephanFactory entscheiden: implementieren oder entfernen
7. **docs/Test-Setup.md erstellen** — nach Test-Framework-Entscheidung (Phase 3 der Konvention)
