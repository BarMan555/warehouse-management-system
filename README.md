# 📦 AsyncWarehouse API

**AsyncWarehouse** — это современный REST API сервис для управления логистикой и складом, построенный на строгих принципах **Clean Architecture**. Приложение позволяет управлять палетами, различными типами товаров (электроника, мебель, химикаты) и асинхронно маршрутизировать задачи на доставку с помощью брокера сообщений RabbitMQ.

## 🚀 Ключевые особенности
* **Чистая архитектура (Clean Architecture):** Строгое разделение ответственности на слои Domain, Application, Infrastructure и API. Бизнес-логика полностью изолирована от внешних фреймворков.
* **Асинхронная обработка (Event-Driven):** Использование RabbitMQ для постановки задач на доставку (Дроны, Грузовики, Корабли) и фоновые воркеры (`BackgroundService`) для их параллельной обработки.
* **Полиморфизм данных:** Использование паттерна TPH (Table-Per-Hierarchy) в Entity Framework Core для удобного хранения различных типов инвентаря (`Electronics`, `Furniture`, `Chemicals`) в единой таблице.
* **Контейнеризация:** Полная поддержка Docker и Docker Compose для развертывания БД, брокера сообщений и самого приложения одной командой.

## 🏗 Структура проекта

Проект разделен на 4 основных слоя:

* 📁 **[AsyncWarehouse.Domain](./AsyncWarehouse.Domain)** — Ядро системы. Содержит бизнес-сущности (`Pallet`, `InventoryItem`), строгую доменную логику (проверка веса, вместимости) и Value Objects/Enums. Не имеет внешних зависимостей.
* 📁 **[AsyncWarehouse.Application](./AsyncWarehouse.Application)** — Слой сценариев использования (Use Cases). Содержит сервисы (`WarehouseService`), DTO, профили `AutoMapper` и абстракции (интерфейсы) репозиториев и брокеров сообщений.
* 📁 **[AsyncWarehouse.Infrastructure](./AsyncWarehouse.Infrastructure)** — Слой реализации. Содержит `ApplicationContext` (EF Core), репозитории (`PalletRepository`), реализацию `RabbitMqProducer`, а также Consumers (фоновые воркеры) для обработки очередей доставки.
* 📁 **[AsyncWarehouse.Api](./AsyncWarehouse.Api)** — Точка входа (Presentation Layer). Содержит REST-контроллеры, настройки Dependency Injection и Swagger-документацию.

## 🛠 Технологический стек
* **Язык/Платформа:** C# / .NET 10.0
* **База данных:** PostgreSQL 16
* **ORM:** Entity Framework Core 10
* **Брокер сообщений:** RabbitMQ 7.x
* **Маппинг:** AutoMapper
* **Тестирование:** xUnit, Moq
* **Инфраструктура:** Docker, Docker Compose

---

## ⚙️ Установка и запуск (Локальная разработка)

Для запуска проекта на вашей машине потребуется **Docker** и **.NET 10 SDK**.

### 1. Поднятие инфраструктуры
В корневой папке проекта выполните команду для запуска базы данных и RabbitMQ:
```bash
docker compose up -d postgres_db rabbitmq
```
* PostgreSQL будет доступен на порту 5432
* RabbitMQ (внутренний) на порту 5670, Management UI — на 15672 (guest/guest)

### 2. Применение миграций
Так как файлы миграций не отслеживаются в системе контроля версий, перед запуском приложения необходимо сгенерировать первичную миграцию и применить её к базе данных.

Сгенерируйте миграцию:
```bash
dotnet ef migrations add InitialCreate --project AsyncWarehouse.Infrastructure --startup-project AsyncWarehouse.Api
```
Примените структуру к базе данных:
```bash
dotnet ef database update --project AsyncWarehouse.Infrastructure --startup-project AsyncWarehouse.Api
```

### 3. Запуск API
Запустите API проект через .NET CLI:
```bash
dotnet run --project AsyncWarehouse.Api
```
После успешного запуска перейдите в браузере по адресу: http://localhost:5000/swagger для тестирования эндпоинтов.

## 🧪 Тестирование
В проекте реализованы модульные тесты для доменной логики и сервисов слоя Application с использованием xUnit и Moq. Для запуска тестов выполните:
```bash
dotnet test
```
