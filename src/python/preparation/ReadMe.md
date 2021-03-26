# Vorbereitung

In dieser Übersicht sind die notwendigen Schritte zur Vorbereitung einer Competition und die dazugehörigen
Skripte in der Reihenfolge beschrieben, in der Ihr die Skripte verwenden solltet.

Zu Vorbereitung einer Competition müsst Ihr einen Server aufsetzen. Der Server führt dann nach einem
Fahrplan, den Ihr in seiner Konfigurationsdatei hinterlegt die verschiedenen Competitions aus. Für einige Competitions 
werden die zu lösenden Aufgaben nicht zur Laufzeit vom Server erstellt, sondern aus Text-Dateien ausgelesen.

In diesem Ordner findet Ihr Python-Skripte um aus einer Fahrplan-Tabelle (csv, z.B aus Calc oder Excel) eine
Serverkonfiguration und einen Wiki-Eintrag für den Fahrplan zu erzeugen und eine Reihe von Skripten um die 
Aufgabenstellungen zu erzeugen.

## generalVars.py
Im Verzeichnis oberhalb dieses Verzeichnisses findet Ihr ein Python-Skript in dem die Konfiguration hinterlegt ist.
Alle Skripte nutzen diese Konfiguration, diese geht z.B. davon aus, dass es relativ zum git-Repo einen Ordner zur
Ablage der Daten gibt oder, dass die TTF-Fonts unter "C:\Windows\Fonts" abgelegt sind.

**An den Stellen, die Ihr wahrscheinlich anpassen müsste, steht jeweils TODO in einem Kommentar.**

## prepareSchedule.py
Erzeugt eine WikiTabelle des Fahrplans und die Schedule-XML-Einträge aus schedule.csv
* Header: Tage in 2021-12-01  
* Spalte 1: Start (00:00)
* Spalte 2: Ende (Wird inoriert)
* Spalte 3: Typ des Eintrage
  * s: simple --> grün
  * m: medium --> gelb
  * h: hard --> rot
  * w: workshop
* WikiTabelle --> schedule.txt
* XmlFragment --> schedule.xml

Das XmlFragment könnt Ihr dann in die Config-Datei des Servers übernehmen.

## findFonts.py
Für die Challenges, bei denen Bilder mit Text erzeugt werden, benötigt man Fonts. Dieses Skript liest alle TTF-Fonts
aus einem Verzeichnis ein (s. Config) und erzeugt Probebilder im Datenverzeichnis unter "allFonts". Um später alle
gültigen Schriften zu finden, wird dieses Verzeichnis ausgewertet. Löscht alle Bilder aus diesem Verzeichnis, die mit
Schriftarten erzeugt wurden, die Ihr nicht verwenden wollt (z.B. Symbolschriften etc.)

## prepareWords.py
Für die Challenges in denen Wörter benötigt werden, werden diese aus einer Datei im Datenordner ausgelesen. 
Dieses Skript erlaubt es eine solche Wortdatei zu erstellen. Als Default liest es die Wörter aus einer 
Korpora-Wortdatei der Deutschen Spache ein. (Eine Korpora wird auch Textkorpurs genannt. Die Linguisten nennen so 
eine Sammlung von Wörtern und Sätzen einer Sprache.) Ihr bekommt solche Daten für verschiedene Sprachen z.B. hier:
https://wortschatz.uni-leipzig.de/en/download/German

## prepareCxxx.py
Erstellt Daten für die Cxxx, da es nicht sinnvoll ist, wenn die Competitions vor einem Event schon bekannt sind, 
ist das public repo hier immer etwas hinter dem internen. Die Skripte sind in der Regel so aufgebaut, dass sie im
Datenordner (s.o. generalVars.py) zunächst einen kompletten Satz Rätsel anlegen:

* Cxxx.answers.txt: Antworten für die einzelnen Rätsel, also das, was der Client als Lösung senden soll.
* Cxxx.questions.txt: Fragen für die einzelnen Rätsel, also das, was der Server dem Client als Aufgabe schickt.
* Cxxx.sources.txt: Die Information aus der das Rätsel generiert wird.

Im zweiten Schritt werden dann die notwendigen Dateien für den Pixelserver und einen Webserver zum Download 
zusätzlicher Rätselteile anlegen. (Diese Ordner heißen pixelserver und webserver). 
Wird das Skript abgebrochen und erneut gestartet, wird auf Basis des erzeugten Rätsel-Satzes weitergearbeitet,
d.h. evtl. schon hochgeladene Teile bleiben gültig. (Wenn Ihr die Anzahl der Rätsel erhöht, wird der Rätsel-Datensatz
erst erweitert und dann werden die abhängigen Daten erzeugt, es bleibt also auch dann alles gültig.)
Wenn Ihr einen wirklich neuen Datensatz erzeugen wollt, dann löscht die drei Text-Dateien des Rätsel-Datensatzes und
die Competition-Daten aus dem Folder webserver.


 
      
