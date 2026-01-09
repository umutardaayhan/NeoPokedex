### Neo Pokedex v1.2 ⚡

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Platform](https://img.shields.io/badge/Platform-Cross--Platform-lightgrey?style=flat&logo=linux)](https://dotnet.microsoft.com/apps/aspnet)
[![Database](https://img.shields.io/badge/Database-SQLite-003B57?style=flat&logo=sqlite)](https://www.sqlite.org/)


<div align="center">
  <h3>
    <a href="#-english">🇬🇧 English</a> | 
    <a href="#-türkçe">🇹🇷 Türkçe</a>
  </h3>
</div>

--------------------------------------------------------------------------------

<a name="-english"></a>
#### 🇬🇧 English
**Neo Pokedex** is a modern, high-performance Pokedex application built with **ASP.NET Core 9.0**. Designed for Pokemon enthusiasts and developers to demonstrate clean MVC architecture, dynamic data fetching, and efficient caching mechanisms with a "Neo" aesthetic interface.

##### 🌟 Features
* **Modern Tech Stack:** Built on .NET 9.0, Entity Framework Core, and MVC Architecture.
* **Auto-Seeder System:** Automatically fetches, parses, and stores data/images from **PokeAPI** into a local SQLite database upon first run.
* **Performance:** Implements **IMemoryCache** to serve Pokemon lists instantly without repeated database hits.
* **Advanced Filtering:** Filter by Type, Search by Name/ID, and Sort by Stats (HP, Attack, Speed, XP).
* **SEO Optimized:** Dynamic `sitemap.xml` generation and Open Graph (OG) meta tags for social sharing.
* **Responsive Design:** Mobile-friendly dark UI using Bootstrap and custom CSS.
* **Detailed Analytics:** View detailed combat stats, physical attributes, and type analysis for every Pokemon.

##### 🛠️ Installation
1.  **Clone the repository:**
    ```bash
    git clone https://github.com/umutardaayhan/NeoPokedex.git
    cd NeoPokedex
    ```
2.  **Prerequisites:**
    * Ensure you have **.NET SDK 9.0** installed.

3.  **Restore Packages:**
    ```bash
    dotnet restore
    ```

##### ⚠️ IMPORTANT: First Run & Database
This application uses a local **SQLite** database. You do not need to download a database file manually.
1.  **Launch:** When you run the app for the first time, the `DbSeeder` will activate.
2.  **Wait:** It will connect to PokeAPI, download Pokemon data, and save images locally to `wwwroot/images/pokemons`.
3.  **Note:** This process might take a few minutes depending on your internet connection. Check the console for progress logs (e.g., `--> [UPDATING] 50 / 2000`).

##### 🚀 Usage
Run the application via terminal:
```bash
dotnet run
```
Then open your browser and navigate to: `http://localhost:5259` or `https://localhost:7106`

--------------------------------------------------------------------------------

<a name="-türkçe"></a>
#### 🇹🇷 Türkçe

**Neo Pokedex**, **ASP.NET Core 9.0** ile geliştirilmiş modern ve yüksek performanslı bir Pokedex uygulamasıdır. Temiz MVC mimarisi, dinamik veri çekme ve "Neo" estetiğine sahip arayüzü ile hem Pokemon tutkunları hem de geliştiriciler için tasarlanmıştır.

##### 🌟 Özellikler
* **Modern Teknoloji:** .NET 9.0, Entity Framework Core ve MVC Mimarisi üzerine inşa edilmiştir.
* **Otomatik Veri Sistemi (Seeder):** İlk çalıştırıldığında PokeAPI üzerinden verileri ve resimleri otomatik olarak çeker ve yerel SQLite veritabanına kaydeder.
* **Performans:** **IMemoryCache** kullanarak Pokemon listelerini veritabanını yormadan anında listeler.
* **Gelişmiş Filtreleme:** Türe göre filtreleme, İsim/ID ile arama ve İstatistiklere (Can, Saldırı, Hız, XP) göre sıralama özellikleri.
* **SEO Uyumlu:** Dinamik `sitemap.xml` oluşturma ve sosyal medya paylaşımları için Open Graph (OG) etiketleri.
* **Duyarlı Tasarım:** Bootstrap ve özel CSS ile hazırlanmış mobil uyumlu karanlık arayüz.
* **Detaylı Analiz:** Her Pokemon için detaylı savaş istatistikleri, fiziksel özellikler ve tür analizleri.

##### 🛠️ Kurulum
1.  **Projeyi bilgisayarınıza klonlayın:**
    ```bash
    git clone https://github.com/umutardaayhan/NeoPokedex.git
    cd NeoPokedex
    ```
2.  **Gereksinimler:**
    * Bilgisayarınızda **.NET SDK 9.0** yüklü olduğundan emin olun.

3.  **Paketleri Yükleyin:**
    ```bash
    dotnet restore
    ```

##### ⚠️ Önemli: İlk Çalıştırma ve Veritabanı
Bu uygulama yerel bir **SQLite** veritabanı kullanır. Herhangi bir SQL sunucusu kurmanıza gerek yoktur.
1.  **Başlat:** Uygulamayı ilk kez çalıştırdığınızda `DbSeeder` devreye girer.
2.  **Bekle:** Uygulama PokeAPI'ye bağlanır, verileri çeker ve resimleri `wwwroot/images/pokemons` klasörüne kaydeder.
3.  **Not:** Bu işlem internet hızınıza bağlı olarak birkaç dakika sürebilir. İlerlemeyi terminalden takip edebilirsiniz (Örn: `--> [GÜNCELLENİYOR] 50 / 2000`).

##### 🚀 Kullanım
Uygulamayı başlatmak için terminalde şu komutu çalıştırın:
```bash
dotnet run
```
Ardından tarayıcınızda şu adrese gidin: `http://localhost:5259` veya `https://localhost:7106`

--------------------------------------------------------------------------------

### 📄 License / Lisans
This project is licensed under the MIT License. Data provided by PokeAPI. Pokémon and Pokémon character names are trademarks of Nintendo.