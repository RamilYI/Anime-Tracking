<h3 align="center">
  <div align="center">
    <h1>Anime Tracking Bot</h1>
    <h6>Бот, уведомляющий о выходе новых серий ваших любимых онгоингов аниме.</h6>
  </div>
  <a href="https://github.com/RamilYI/Anime-Tracking">
    <img src="https://i.imgur.com/apIsefH.jpeg" alt="MiniApp"/>
  </a>
</h3>

Телеграм-бот, который предоставляет данные об аниме-сериалах текущего сезона, в т.ч. и даёт возможность уведомляет о выходе новых серий выбранных пользователем тайтлов.

## Текущие фичи: 

* Grid-список аниме-сериалов текущего сезона с обложкой и названием;
* Возможность подписываться на тайтлы путём нажатия на галочки;
* Возможность отписываться от тайтлов путём отжатия галочки;
* Поиск аниме-сериалов;

## Архитектура приложения

* Приложение состоит из двух сборок: **AnimeTrackingWeb** и **AnimeTrackingApi**
  
* **AnimeTrackingWeb** — веб-приложение ASP.NET Core, предоставляющий webhook для телеграм-бота. Он обрабатывает HTTP запросы, взаимодействует с [мини-приложением](https://github.com/RamilYI/Anime-Tracking-Bot-MiniApp) и взаимодействует с базами данных;
* Приложение спроектировано по шаблону MVC;
* Всего имеется два контроллера:
  * GetSeasonController: обрабатывает GET-запрос получения коллекция тайтлов за весь текущий сезон;
  * BotController: обрабатывает POST-запросы пользователя (открытие мини-приложения, закрытие мини-приложения, выбор тайтлов в мини-приложении);
* Всего имеется две БД:
  * БД Hangfire, хранилище добавленных в очередь задач;
  * БД пользователей, хранящий данные о пользователях, выбранных ими тайтлов и т.п.
    
* **AnimeTrackingApi** — библиотека классов, взаимодействующая с AniList API GraphQL, который предоставляет данные об аниме-сериалах;
* Библиотека представляет собой набор скриптов для API AniList и выходных ДТОшек.
* Взаимодействие с **AnimeTrackingWeb** осуществляется через интерфейс IAnimeTracking;

## Стек технологий

* [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet) — платформа разработки веб-приложений;
* [EF Core](https://learn.microsoft.com/ru-ru/ef/core/) — взаимодействие с БД;
* [Hangfire](https://www.hangfire.io/) — планировщик задач;
* [PostgreSQL](https://www.postgresql.org/) — СУБД;
* [AniList API GraphQL](https://github.com/AniList/ApiV2-GraphQL-Docs) (для получениях данных об аниме-сериалах, расписание новых серий и т.п.)
* [.NET Client for Telegram Bot API](https://github.com/TelegramBots/Telegram.Bot) — взаимодействие с телеграмом;

## Скриншоты

<img src="https://i.imgur.com/gRt0w17.jpeg" width="500"/>
<img src="https://i.imgur.com/2TbYQYJ.jpeg" width="500"/>
<img src="https://i.imgur.com/6LboleQ.jpeg" width="500"/>

[Ссылка на тг-бот](https://t.me/animtetrackingdemobot_fst_bot)
