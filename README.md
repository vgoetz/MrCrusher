MrCrusher
=========

yet another cool 2D tank game


Changelog: Version 0.251
- Bugfixing: AI Zielauswahl und Bewegung, Collisiondetection, Game Reset, ...
- Granaten für Soldaten hinzugefügt
- Start-Parameter hinzugefügt

Changelog: Version 0.228
- Die Keyboard und Mouse-Daten eines Clients werden zum Server geschickt und dort verarbeitet (wie auch die des am Server lokalen Spielers auch).
- Server verschickt nur Bilddaten (incl. Spielstatus)

Changelog: Version 0.225
- Server und Client-Architektur von WCF auf XSockets geändert
- Ein oder mehrere Clients können nun gestartet werden. Diese verbinden sich automatisch zum Server.
- Die Clients fungieren momentan noch als reine Spectators


Spielstart: 
-----------
- Sever: MrCrusher.XSocketsServer.exe starten  (Adminrechte für Verbindungsaufbau ggf. erforderlich)
- Client: MrCrusher.XSocketsClient.exe [IpDesServers] [PlayerName]

[IpDesServers] ist optional, alle Parameter sind ohne [ bzw. ] anzugeben


Steuerung:
----------
Bewegung und Drehung mit Tastatur (W,A,S,D)
W - vorwärts laufen / fahren
S - rückwärts laufen / fahren
A - nach links drehen / wenden
D - nach rechts drehen / wenden

Drehung des Soldaten / Drehung des Panzerturms mit der Maus. Diese drehen sich in die Richtung des Mauszeigers.

Linke Maustaste - Primäre Waffe
Rechte Maustaste - Sekundäre Waffe

E - Ein- bzw. aussteigen in einen Panzer oder Bunker

Sonstiges:
----------
STRG+D - Sofortiger Tod
R - Restart (funktioniert nur am Server)
ESC - Spiel beenden
Mausrad drücken - Teleportation an diese Stelle (für Testzwecke)

