# TAB_Projekt_Warsztat_Samochodowy

Co zrobić żeby to działało:

1. SPRAWDŹ PLIK appsettings.json
W tym pliku jest taki długi string "Server=(local)....". Odpal SQL Server Management Studio. Jeśli w tym małym okienku na początku w polu Server name masz nazwę swojego komputera a po ukośniku 'SQLEXPRESS' to nie musisz nic robić w pliku appsettings.json. Jeśli jednak w polu Server name jest tylko nazwa twojego komputera (prawdopodobnie będzie tam nazwa komputera, no w każdym razie chodzi o to, że NIE ma 'SQLEXPRESS') to wtedy zmien ten długi string w appsettings.json na: "Server=(local);Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true"       CHYBA

2. 
