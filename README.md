# TAB_Projekt_Warsztat_Samochodowy

RÓŻNE WAŻNE INFO

Co zrobić żeby to działało:

1. Pobierz i zainstaluj SQL server - https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads - najlepiej wersję EXPRESS

2. Pobierz i zainstaluj SQL management studio - https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15

3. Sklonuj repozytorium i włącz projekt w visualu ale zanim odpalisz to...

4. SPRAWDŹ PLIK appsettings.json
W tym pliku jest taki długi string "Server=(local)....". Odpal SQL Server Management Studio. Jeśli w tym małym okienku na początku w polu Server name masz nazwę swojego komputera a po ukośniku 'SQLEXPRESS' to nie musisz nic robić w pliku appsettings.json. Jeśli jednak w polu Server name jest tylko nazwa twojego komputera (prawdopodobnie będzie tam nazwa komputera, no w każdym razie chodzi o to, że NIE ma 'SQLEXPRESS') to wtedy zmien ten długi string w appsettings.json na: "Server=(local);Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true"       CHYBA
Ten sam string trzeba wstawić w pliki PersonelRepository.cs i ClientRepository.cs. Na razie tak trzeba, nie wiem jeszcze jak to sparametryzować.

5. wejdź w visualu w widok => inne okna => konsola menadżera pakietów

6. w tej konsoli wpisz Update-Database

7. w SQL management studio podłącz się do bazy danych - w folderze database powinna być nowo utworzona baza danych - TAB_proj_DB (czy jakoś tak)

3. UPDATE: Odpal appke (włączy się przeglądarka) do wejdź w url localhst:port/Debug i kliknij guzik żeby wypełnić Baze Danych po tym będzie się dało zalogować przy pomcy passów. 

adm0@tab.pl - admin0 (Admin)

wor0@tab.pl - worker0 (Worker)

man0@tab.pl - manager0 (Manager)

7. Jak już coś jest w bazie danych to można przetestować mechanizm logowania. Po wpisaniu niepoprawnego loginu lub hasła (lub jednego i drugiego źle) wyskoczy błąd. Jak się wpisze poprawne login i hasło to przechodzimy do strony głównej.
