MrCrusher
=========

yet another cool 2D tank game ;-)

Changelog: Version 0.268
- Farbliche Markierung der eigenen Einheit und die der anderen Mitspieler
- Username steht nun oben (in eigener Farbe)
![FarblicheMarkierung](https://github.com/vgoetz/MrCrusher/blob/master/MrCrusher/Screenshots/FarblicheMarkierungUndUsernamenEinblenden.png "Who is who ...")<br>

Changelog: Version 0.252
- Bugfixing: AI Zielauswahl und Bewegung, Collisiondetection, Game Reset, ...<br>
- Granaten für Soldaten hinzugefügt<br>
- Start-Parameter hinzugefügt<br>

Changelog: Version 0.228
- Die Keyboard und Mouse-Daten eines Clients werden zum Server geschickt und dort verarbeitet (wie auch die des am Server lokalen Spielers auch).<br>
- Server verschickt nur Bilddaten (incl. Spielstatus)<br>

Changelog: Version 0.225
- Server und Client-Architektur von WCF auf XSockets geändert<br>
- Ein oder mehrere Clients können nun gestartet werden. Diese verbinden sich automatisch zum Server.<br>
- Die Clients fungieren momentan noch als reine Spectators<br>


Spielstart: 
-----------
- Sever: MrCrusher.XSocketsServer.exe starten  (Adminrechte für Verbindungsaufbau ggf. erforderlich)
- Client: MrCrusher.XSocketsClient.exe [IpDesServers] [PlayerName]

[IpDesServers] ist optional, alle Parameter sind ohne [ bzw. ] anzugeben


Steuerung:
----------
Bewegung und Drehung mit Tastatur (W,A,S,D)<br>
W - vorwärts laufen / fahren<br>
S - rückwärts laufen / fahren<br>
A - nach links drehen / wenden<br>
D - nach rechts drehen / wenden<br>

Drehung des Soldaten / Drehung des Panzerturms mit der Maus. Diese drehen sich in die Richtung des Mauszeigers.

Linke Maustaste - Primäre Waffe<br>
Rechte Maustaste - Sekundäre Waffe<br>

E - Ein- bzw. aussteigen in einen Panzer oder Bunker

Sonstiges:
----------
STRG+D - Sofortiger Tod<br>
R - Restart (funktioniert nur am Server)<br>
ESC - Spiel beenden<br>
Mausrad drücken - Teleportation an diese Stelle (für Testzwecke)<br>

