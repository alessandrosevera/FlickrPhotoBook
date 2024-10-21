# FlickrPhotoBook

**Descrizione:**  
FlickrPhotoBook è un'applicazione sviluppata in .NET MAUI (.NET 8) con target iOS e Android, che permette agli utenti di cercare foto su Flickr utilizzando le Flickr API. Gli utenti possono inserire parole chiave per trovare immagini, visualizzare i risultati in un'infinite-scroll, e visualizzare una foto a schermo intero con i dettagli associati cliccando su di essa.

---

## Requisiti di sistema

- .NET SDK 8.0
- JetBrains Rider 2024.1.2 (versione aggiornata per lo sviluppo MAUI)
- Connessione a internet per accedere alle API di Flickr

---

## Come eseguire il progetto

1. **Clonare il repository:**
   ```bash 
    git clone https://github.com/username/FlickrPhotoBook.git
2. **Aprire il progetto in Visual Studio 2022:**
Aprire il file `FlickrPhotoBook.sln`.

3. **Configurare la chiave API di Flickr:**
L'ApiRoot e l'ApiKey per le chiamate alle API di Flickr risiedono nel file `configuration.json` (**EmbeddedResource**) del progetto.
Il file viene letto in fase di startup dell’app per inizializzare le chiamate alle API REST.

4. **Compilare ed eseguire:**
Selezionare il target desiderato (iOS o Android).
Compilare ed eseguire l’app direttamente su un dispositivo o emulatore.

---

## Librerie utilizzate

1. **FFImageLoading.Maui**
Utilizzata per ottimizzare il caricamento e la gestione delle immagini all'interno dell'applicazione.

2. **Flurl, Flurl.Http, Flurl.Http.Newtonsoft**
Libreria con **sintassi fluent**, utilizzata per gestire le **chiamate REST** alle Flickr API in modo semplice e con supporto nativo alla serializzazione/deserializzazione JSON.

3. **HtmlAgilityPack**
Usata per la realizzazione del controllo custom HtmlFormsLabel, a sua volta utilizzato nelle schermate di dettaglio delle foto per processare le descrizioni HTML, laddove necessario.

4. **Newtonsoft.Json**
Utilizzata per la gestione della **serializzazione JSON** all'interno dell'applicazione.

5. **PropertyChanged.Fody**
Aggiunge automaticamente il supporto per la gestione delle notifiche di proprietà (`INotifyPropertyChanged`) nel pattern MVVM.

6. **VisualStateLayout.Maui (proprietaria)**
Usata per gestire lo stato visivo dei layout (`Success` | `Error` | `Empty` | `Loading`), migliorando l'esperienza d'uso.

---

## Note aggiuntive
L’app è stata sviluppata seguendo il design pattern **MVVM**, per garantire una separazione pulita tra logica di business e interfaccia utente.
È stato creato un `handler` per un controllo personalizzato chiamato `BorderlessEntry`, utilizzato nella realizzazione del controllo custom `MaterialEntry` per una migliore resa grafica nella UI.

---

## Licenza
Questo progetto è distribuito con licenza MIT.