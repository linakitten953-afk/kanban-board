Kanban Board (аналог Jira)

Веб-приложение для управления задачами по методологии Kanban.

Функционал

    Аутентификация (JWT) и роли (Admin, Manager, Developer).

    Создание проектов, досок и колонок с WIP-лимитами.

    Карточки с приоритетами, дедлайнами и исполнителями.

    Drag-and-drop перемещение карточек.

    Редактирование карточек двойным кликом.

    Поиск по карточкам.

    Панель прогресса выполнения задач.

Технологический стек

    Backend: ASP.NET Core Web API (.NET 8), C#

    Frontend: Blazor WebAssembly

    База данных: SQLite + Entity Framework Core (Code First)

    Тестирование: xUnit, EF Core InMemory

    CI/CD: GitHub Actions

Как запустить

Требования

    .NET 8 SDK

    Visual Studio 2022

Запуск

    Клонировать репозиторий:
    bash

    git clone https://github.com/linakitten953-afk/kanban-board.git

    Открыть решение KanbanBoard.sln в Visual Studio.

    Настроить запуск нескольких проектов:

        Правой кнопкой на Решении -> Свойства -> Запускаемые проекты.

        Выбрать KanbanBoard.Api и KanbanBoard.Client.

    Нажать F5.

    Открыть в браузере http://localhost:5234.

Тестовые данные

    Логин: admin / Пароль: admin (создается автоматически)

Тестирование

Проект содержит 14 модульных тестов.

Запуск через Test Explorer в Visual Studio (Тест -> Запустить все тесты) или командой:
bash

dotnet test

CI/CD

Настроен автоматический запуск тестов через GitHub Actions при каждом пуше в ветку main.
