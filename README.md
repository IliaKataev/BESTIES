# WebLibrary

WebLibrary — приложение для библиотекаря. В нём можно найти книгу, открыть карточку читателя и посмотреть, какие книги он брал. Ещё можно собрать подборку для выставки или скачать список просроченных выдач в CSV.

Через API можно оформить выдачу, продлить её или отметить возврат. В интерфейсе можно добавлять и редактировать карточки читателей и выставки.

## Как устроен проект

У проекта две части. [LibraryWeb](LibraryApp/LibraryWeb) показывает страницы в браузере, а [LibraryApi](LibraryApp/LibraryApi) принимает запросы от сайта и работает с PostgreSQL через Entity Framework Core и Npgsql. Например, когда библиотекарь открывает карточку читателя, сайт запрашивает у API список его текущих выдач и историю возвратов.

Обе части написаны на C# и работают на .NET 9. Сайт использует ASP.NET Core MVC и Razor, а API можно изучить через Swagger.

## Локальный запуск

Для запуска нужны .NET 9 SDK, PostgreSQL со схемой библиотеки и доверенный сертификат разработки ASP.NET Core. Таблицы и связи можно посмотреть в [LibraryContext](LibraryApp/LibraryApi/Data/Data.cs). Перед запуском API задайте строку подключения в переменной окружения `ConnectionStrings__LibraryDb`.

Из корня репозитория запустите API и веб-приложение в отдельных терминалах.

```sh
dotnet run --project LibraryApp/LibraryApi/LibraryApi.csproj --launch-profile https
```

```sh
dotnet run --project LibraryApp/LibraryWeb/LibraryWeb.csproj --launch-profile https
```

В профиле `https` интерфейс открывается по адресу `https://localhost:7287`, Swagger UI — по адресу `https://localhost:7055/swagger`. Для первого знакомства с API откройте список книг или авторов в Swagger.
