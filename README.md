# TAB_Projekt_Warsztat_Samochodowy

Co zrobić żeby to działało:

1. SPRAWDŹ PLIK appsettings.json
W tym pliku jest taki długi string "Server=(local)....". Odpal SQL Server Management Studio. Jeśli w tym małym okienku na początku w polu Server name masz nazwę swojego komputera a po ukośniku 'SQLEXPRESS' to nie musisz nic robić w pliku appsettings.json. Jeśli jednak w polu Server name jest tylko nazwa twojego komputera (prawdopodobnie będzie tam nazwa komputera, no w każdym razie chodzi o to, że NIE ma 'SQLEXPRESS') to wtedy zmien ten długi string w appsettings.json na: "Server=(local);Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true"       CHYBA
Ten sam string trzeba wstawić w pliki PersonelRepository.cs i ClientRepository.cs. Na razie tak trzeba, nie wiem jeszcze jak to sparametryzować.

2. Po pierwszym uruchomieniu projektu, baza danych powinna się sama zrobić. Tylko nie wiem czy tak bez problemów to pójdzie, bo nie mam jak tego sprawdzić. Możliwe, że trzeba będzie jakieś migracje porobić. Jakby co to proszę pytać ;)

3. Mechanizm logowania działa (jest to raczej prymitywny mechanizm, ale myślę, że taki styknie na potrzeby tego projektu). Żeby go przetestować należy do swojej bazy danych dodać jakichś userów. Czyli w SQL management studio klikamy prawym najpierw na tabeli Address => edit top 200 rows, wpisujemy sobie jakieś adresy, bo muszą być adresy, żeby stworzyć użytkowników,  użytkownik musi mieć adres. Potem prawym na tabeli Personel => Edit top 200 rows i dodajemy sobie kilku pracowników.
4. Jak już coś jest w bazie danych to można przetestować mechanizm logowania. Po wpisaniu niepoprawnego loginu lub hasła (lub jednego i drugiego źle) wyskoczy błąd 400 bad request i to DOBRZE, bo na razie po prostu strona "niepoprawne dane logowania" nie jest jeszcze zaimplementowana. Jak się wpisze poprawne login i hasło to przeskakujemy do takiej na razie całej białej strony.
