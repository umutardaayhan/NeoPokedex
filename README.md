### Neo Pokedex v1.6 ⚡

--------------------------------------------------------------------------------

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Platform](https://img.shields.io/badge/Platform-Cross--Platform-lightgrey?style=flat&logo=linux)](https://dotnet.microsoft.com/apps/aspnet)
[![Database](https://img.shields.io/badge/Database-SQLite-003B57?style=flat&logo=sqlite)](https://www.sqlite.org/)

> 🌐 **Live Demo / Canlı Site:** : [https://neopokedex.runasp.net]

<a name="-english"></a>
#### 🇬🇧 English
**Neo Pokedex** is a modern, high-performance Pokedex application built with **ASP.NET Core 9.0**. Designed for Pokemon enthusiasts and developers to demonstrate clean MVC architecture, dynamic data fetching, and efficient caching mechanisms with a "Neo" aesthetic interface.

##### 🚀 v1.6 Update (Performance Edition)
* **Local Asset Caching:** Reverted to local storage for images (`wwwroot/images`) to ensure lightning-fast load times and offline compatibility.
* **Gen 9 Support:** Database limit extended to cover **1025 Pokemons** (up to Generation 9).
* **Aggressive Caching:** Implemented 1-year static file caching headers for maximum client-side performance.

##### ⚔️ v1.5 Features (Tactical Edition)
* **Type Matchup Engine:** Automatically calculates **Weaknesses** (2x/4x) and **Resistances** (0.5x/0x) based on types.
* **Combat Moves:** Displays the top 6 combat moves for each Pokemon.
* **Smart Navigation:** "Previous" ❮ and "Next" ❯ buttons for seamless browsing.
* **Favorites System:** Mark favorites with ❤️ and filter them on the home page.

##### 🌟 Core Features
* **Modern Tech Stack:** .NET 9.0, Entity Framework Core, MVC.
* **Interactive Elements:** Toggle **Shiny Mode** ✨, play **Cries/Sounds** 🔊, and view Ability details via popovers.
* **Advanced Filtering:** Filter by Type, Search by Name/ID, Sort by Stats, and Filter by Favorites.
* **SEO Optimized:** Dynamic `sitemap.xml` and Open Graph tags.

##### 🛠️ Installation & First Run
1.  **Clone the repository:**
    ```bash
    git clone https://github.com/umutardaayhan/NeoPokedex.git
    cd NeoPokedex
    ```
2.  **Restore Packages:**
    ```bash
    dotnet restore
    ```
3.  **Run the App:**
    ```bash
    dotnet run
    ```

##### ⚠️ IMPORTANT: Database Seeding (v1.6 Change)
Since v1.6 downloads images locally for performance:
1.  **First Launch:** The app will download **1025 images** and data from PokeAPI.
2.  **Duration:** This process takes **5-10 minutes** depending on your internet speed.
3.  **Console Logs:** Watch the terminal for progress (e.g., `✅ 50/1025 Downloaded`).
4.  **Note:** Do not close the terminal until you see `🎉 ALL OPERATIONS COMPLETED!`. Subsequent runs will be instant.

--------------------------------------------------------------------------------

<a name="-türkçe"></a>
#### 🇹🇷 Türkçe
**Neo Pokedex**, **ASP.NET Core 9.0** ile geliştirilmiş modern ve yüksek performanslı bir Pokedex uygulamasıdır. Temiz MVC mimarisi ve "Neo" estetiğine sahip arayüzü ile hem Pokemon tutkunları hem de geliştiriciler için tasarlanmıştır.

##### 🚀 v1.6 Güncellemesi (Performans Sürümü)
* **Yerel Varlık Yönetimi:** Sayfa yüklenme hızlarını maksimize etmek için resimler tekrar yerel diske (`wwwroot`) indirilmektedir. İnternet yavaşlasa bile uygulama "ışık hızında" çalışır.
* **9. Nesil Desteği:** Veritabanı limiti **1025 Pokemon**'a çıkarıldı (Gen 9 dahil).
* **Statik Önbellek:** Tarayıcı tarafında 1 yıllık önbellek (cache) başlıkları eklendi.

##### ⚔️ v1.5 Özellikleri (Taktiksel Sürüm)
* **Tür Analiz Motoru:** Türlere göre **Zayıflıklar** (Weakness) ve **Dirençler** (Resistance) otomatik hesaplanır.
* **Savaş Hamleleri:** Her Pokemon için en önemli 6 saldırı hamlesi (Moves) listelenir.
* **Akıllı Navigasyon:** Detay sayfasında "Önceki" ❮ ve "Sonraki" ❯ butonları.
* **Favori Sistemi:** Beğendiğiniz Pokemonları ❤️ ile işaretleyip filtreleyebilirsiniz.

##### 🌟 Temel Özellikler
* **Modern Teknoloji:** .NET 9.0, Entity Framework Core ve MVC.
* **Etkileşim:** **Shiny Mod** ✨ geçişi, **Ses (Cry)** 🔊 çalma ve Yetenek açıklamaları.
* **Gelişmiş Filtreleme:** Tür, İsim, ID, İstatistik Sıralaması ve Favori filtreleme.
* **SEO Uyumlu:** Dinamik `sitemap.xml` ve sosyal medya etiketleri.

##### 🛠️ Kurulum ve İlk Çalıştırma
1.  **Projeyi klonlayın:**
    ```bash
    git clone https://github.com/umutardaayhan/NeoPokedex.git
    cd NeoPokedex
    ```
2.  **Paketleri Yükleyin:**
    ```bash
    dotnet restore
    ```
3.  **Başlatın:**
    ```bash
    dotnet run
    ```

##### ⚠️ Önemli: Veritabanı Kurulumu (v1.6 Değişikliği)
v1.6 sürümü resimleri yerel diske indirdiği için:
1.  **İlk Açılış:** Uygulama PokeAPI'den **1025 Pokemon** verisini ve resmini indirir.
2.  **Süre:** Bu işlem internet hızınıza bağlı olarak **5-10 dakika** sürebilir.
3.  **Takip:** İlerlemeyi terminalden izleyin (Örn: `✅ 50/1025 Downloaded`).
4.  **Not:** `🎉 ALL OPERATIONS COMPLETED!` yazısını görene kadar bekleyin. Sonraki açılışlar anında gerçekleşecektir.

--------------------------------------------------------------------------------

#### 📄 License
This project is licensed under the MIT License.
Data provided by [PokeAPI](https://pokeapi.co/). Pokémon characters are trademarks of Nintendo.