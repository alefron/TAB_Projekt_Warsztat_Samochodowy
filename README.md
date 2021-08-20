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

3. Mechanizm logowania działa (jest to raczej prymitywny mechanizm, ale myślę, że taki styknie na potrzeby tego projektu). Żeby go przetestować należy do swojej bazy danych dodać jakichś pracowników. Czyli w SQL management studio klikamy prawym najpierw na tabeli Address => edit top 200 rows, wpisujemy sobie jakieś adresy, bo muszą być adresy, żeby stworzyć użytkowników,  użytkownik musi mieć adres. Potem prawym na tabeli Role i dodajemy role: MAN (Manager), WOR (Worker), ADM (Admin). Potem prawym na tabeli Personel => Edit top 200 rows i dodajemy sobie kilku pracowników.
4. Jak już coś jest w bazie danych to można przetestować mechanizm logowania. Po wpisaniu niepoprawnego loginu lub hasła (lub jednego i drugiego źle) wyskoczy błąd. Jak się wpisze poprawne login i hasło to przechodzimy do strony głównej.
