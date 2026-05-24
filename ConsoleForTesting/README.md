# Семинар 7. Основы тестирования в .NET

## Описание проекта

Данный проект выполнен по учебным материалам семинара №7 по дисциплине «Архитектура автоматизированных систем обработки информации и управления».

Тема семинара: **основы автоматизированного тестирования в .NET**.

Цель работы — освоить практику написания автоматизированных тестов для программ на C#, познакомиться с основными тестовыми фреймворками .NET, реализовать модульные тесты, параметризованные тесты, а также BDD-сценарии.

В проекте демонстрируются:

- создание отдельного тестового проекта для основного консольного приложения;
- модульное тестирование с использованием MSTest;
- модульное тестирование с использованием NUnit;
- модульное тестирование с использованием xUnit;
- параметризованные тесты;
- разработка логики через тестирование на примере `FamilyFineRule`;
- BDD-тестирование с использованием Reqnroll;
- запуск тестов через `dotnet test` в VS Code.

## Структура проекта

```text
ConsoleForTesting/
├── ConsoleApp1/
│   ├── ConsoleApp1.csproj
│   ├── Program.cs
│   └── FamilyFineRule.cs
│
├── MSTest_Console/
│   ├── MSTest_Console.csproj
│   └── Test1.cs
│
├── NUnit_Test_Console/
│   ├── NUnit_Test_Console.csproj
│   └── UnitTest1.cs
│
├── XUnit_Test_Console/
│   ├── XUnit_Test_Console.csproj
│   └── UnitTest1.cs
│
├── XUnit_BDD/
│   ├── XUnit_BDD.csproj
│   ├── features/
│   │   └── FamilyFineRule.feature
│   └── StepDefinitions/
│       └── FamilyFineRuleSteps.cs
│
└── README.md
```

## Основной проект

Основной проект находится в папке:

```text
ConsoleApp1
```

В нём реализованы классы, которые проверяются автоматизированными тестами.

## Класс RegularFineRule

Класс `RegularFineRule` реализует простой расчёт штрафа за просрочку.

```csharp
public class RegularFineRule
{
    public decimal Calculate(int daysOverdue) => daysOverdue * 5m;
}
```

Правило расчёта:

```text
штраф = количество дней просрочки × 5 рублей
```

Примеры работы:

```text
0 дней  -> 0 рублей
1 день  -> 5 рублей
3 дня   -> 15 рублей
10 дней -> 50 рублей
```

Именно этот класс используется для демонстрации модульного тестирования в трёх фреймворках: MSTest, NUnit и xUnit.

## Класс FamilyFineRule

Класс `FamilyFineRule` реализует расчёт штрафа для читателей с семейным абонементом.

Правила расчёта:

```text
Первые 5 дней — без штрафа.
После 5 дней — 2 рубля за каждый день просрочки.
Максимальный штраф — 100 рублей.
```

Примеры работы:

```text
3 дня    -> 0 рублей
5 дней   -> 0 рублей
6 дней   -> 2 рубля
8 дней   -> 6 рублей
100 дней -> 100 рублей
```

Пример реализации:

```csharp
public class FamilyFineRule
{
    private const int GraceDays = 5;
    private const decimal RatePerDay = 2m;
    private const decimal MaxFine = 100m;

    public decimal Calculate(int daysOverdue)
    {
        if (daysOverdue < 0)
        {
            throw new ArgumentException(
                "Количество дней просрочки не может быть отрицательным",
                nameof(daysOverdue));
        }

        if (daysOverdue <= GraceDays)
        {
            return 0m;
        }

        decimal fine = (daysOverdue - GraceDays) * RatePerDay;
        return Math.Min(fine, MaxFine);
    }
}
```

## Используемые технологии

В проекте используются:

```text
C#
.NET 8
VS Code
WSL Ubuntu
MSTest
NUnit
xUnit
Reqnroll
Gherkin
```

Изначально проекты были настроены на `net10.0`, но так как локально установлен .NET SDK 8, целевая платформа была заменена на:

```xml
<TargetFramework>net8.0</TargetFramework>
```

Это позволяет корректно собирать и запускать проект в установленной среде.

## Тестовые проекты

В проекте созданы отдельные тестовые проекты:

```text
MSTest_Console
NUnit_Test_Console
XUnit_Test_Console
XUnit_BDD
```

Отдельные тестовые проекты нужны для того, чтобы тестовые зависимости не попадали в основное консольное приложение.

## MSTest_Console

Проект `MSTest_Console` содержит модульные тесты на фреймворке MSTest.

Пример обычного теста:

```csharp
[TestMethod]
public void Calculate_ThreeDaysOverdue_Returns15()
{
    // Arrange
    var rule = new RegularFineRule();

    // Act
    decimal fine = rule.Calculate(daysOverdue: 3);

    // Assert
    Assert.AreEqual(15m, fine);
}
```

В тесте используется паттерн AAA:

```text
Arrange — подготовка объекта;
Act — выполнение действия;
Assert — проверка результата.
```

Также в проекте реализованы параметризованные тесты через атрибуты `DataRow`.

```csharp
[TestMethod]
[DataRow(0, "0")]
[DataRow(1, "5")]
[DataRow(3, "15")]
[DataRow(10, "50")]
public void Calculate_VariousDays_ReturnsCorrectFine(int days, string expectedStr)
{
    decimal expected = decimal.Parse(
        expectedStr,
        System.Globalization.CultureInfo.InvariantCulture);

    var rule = new RegularFineRule();

    Assert.AreEqual(expected, rule.Calculate(days));
}
```

## NUnit_Test_Console

Проект `NUnit_Test_Console` содержит модульные тесты на фреймворке NUnit.

Пример обычного теста:

```csharp
[Test]
public void Calculate_ThreeDaysOverdue_Returns15()
{
    // Arrange
    var rule = new RegularFineRule();

    // Act
    decimal fine = rule.Calculate(daysOverdue: 3);

    // Assert
    Assert.That(fine, Is.EqualTo(15m));
}
```

Параметризованные тесты реализованы через атрибуты `TestCase`.

```csharp
[TestCase(0, 0)]
[TestCase(1, 5)]
[TestCase(3, 15)]
[TestCase(10, 50)]
public void Calculate_VariousDays_ReturnsCorrectFine(int days, decimal expected)
{
    var rule = new RegularFineRule();

    Assert.That(rule.Calculate(days), Is.EqualTo(expected));
}
```

## XUnit_Test_Console

Проект `XUnit_Test_Console` содержит модульные тесты на фреймворке xUnit.

Пример обычного теста:

```csharp
[Fact]
public void Calculate_ThreeDaysOverdue_Returns15()
{
    // Arrange
    var rule = new RegularFineRule();

    // Act
    decimal fine = rule.Calculate(daysOverdue: 3);

    // Assert
    Assert.Equal(15m, fine);
}
```

Параметризованные тесты реализованы через `Theory` и `InlineData`.

```csharp
[Theory]
[InlineData(0, 0)]
[InlineData(1, 5)]
[InlineData(3, 15)]
[InlineData(10, 50)]
public void Calculate_VariousDays_ReturnsCorrectFine(int days, decimal expected)
{
    var rule = new RegularFineRule();

    Assert.Equal(expected, rule.Calculate(days));
}
```

## Разработка через тестирование

В проекте реализован пример разработки через тестирование на классе `FamilyFineRule`.

Требования к классу:

```text
Первые 5 дней — без штрафа.
Далее 2 рубля в день.
Не более 100 рублей всего.
```

Сначала были определены тестовые сценарии:

```text
3 дня просрочки -> 0 рублей
8 дней просрочки -> 6 рублей
100 дней просрочки -> 100 рублей
```

После этого была реализована логика класса.

Такой подход соответствует циклу TDD:

```text
Red — сначала пишется тест на требуемое поведение.
Green — затем пишется минимальный код, чтобы тест прошёл.
Refactor — после прохождения тестов код можно улучшать.
```

## BDD-тестирование

BDD-тесты находятся в проекте:

```text
XUnit_BDD
```

Для описания сценариев используется язык Gherkin.

Файл сценариев:

```text
XUnit_BDD/features/FamilyFineRule.feature
```

Пример BDD-сценария:

```gherkin
# language: ru

Функция: Расчёт штрафа для читателей с семейным абонементом

  Сценарий: Просрочка в льготный период не штрафуется
    Дано читатель с семейным абонементом
    Когда книга просрочена на 3 дня
    Тогда штраф равен 0 рублей
```

Также реализована структура сценария с таблицей примеров:

```gherkin
Структура сценария: Расчёт штрафа для разных периодов
  Дано читатель с семейным абонементом
  Когда книга просрочена на <дни> дней
  Тогда штраф равен <штраф> рублей

Примеры:
  | дни | штраф |
  | 0   | 0     |
  | 5   | 0     |
  | 6   | 2     |
  | 100 | 100   |
```

Шаги BDD-сценариев связаны с C#-кодом через класс `FamilyFineRuleSteps`.

Пример привязки шагов:

```csharp
[Binding]
public class FamilyFineRuleSteps
{
    private FamilyFineRule? _rule;
    private decimal _actualFine;

    [Given("читатель с семейным абонементом")]
    public void GivenFamilyReader()
    {
        _rule = new FamilyFineRule();
    }

    [When(@"книга просрочена на (\d+) дней")]
    [When(@"книга просрочена на (\d+) дня")]
    public void WhenBookOverdueByDays(int days)
    {
        _actualFine = _rule!.Calculate(days);
    }

    [Then(@"штраф равен (\d+) рублей")]
    public void ThenFineEquals(decimal expected)
    {
        Assert.Equal(expected, _actualFine);
    }
}
```

BDD-тесты позволяют описывать поведение системы на языке, понятном не только программисту, но и аналитику или заказчику.

## Паттерн AAA

В тестах используется паттерн AAA.

```text
Arrange — подготовка данных и объектов.
Act — выполнение тестируемого действия.
Assert — проверка результата.
```

Пример:

```csharp
[Fact]
public void Calculate_ThreeDaysOverdue_Returns15()
{
    // Arrange
    var rule = new RegularFineRule();

    // Act
    decimal fine = rule.Calculate(daysOverdue: 3);

    // Assert
    Assert.Equal(15m, fine);
}
```

Такой подход делает тесты понятными, читаемыми и удобными для сопровождения.

## Именование тестов

В проекте используется понятное именование тестов по схеме:

```text
Метод_Сценарий_ОжидаемыйРезультат
```

Примеры:

```text
Calculate_ThreeDaysOverdue_Returns15
Calculate_VariousDays_ReturnsCorrectFine
Calculate_WithinGracePeriod_ReturnsZero
Calculate_AfterGracePeriod_ChargesTwoRublesPerDay
```

Такие имена позволяют понять назначение теста без дополнительного изучения его тела.

## Принципы хорошего теста FIRST

В проекте соблюдаются основные принципы хороших модульных тестов FIRST.

```text
F — Fast.
Тесты выполняются быстро.

I — Isolated.
Тесты изолированы друг от друга.

R — Repeatable.
Повторный запуск тестов даёт одинаковый результат.

S — Self-validating.
Результат теста однозначный: прошёл или не прошёл.

T — Timely.
Тесты написаны вместе с реализацией проверяемой логики.
```

Так как тесты не зависят от базы данных, файловой системы, сети или текущего времени, они выполняются быстро и стабильно.

## Запуск основного приложения

Для запуска основного консольного приложения используется команда:

```bash
dotnet run --project ConsoleApp1/ConsoleApp1.csproj
```

Текущий вывод приложения:

```text
Hello, World!
```

При необходимости стандартный вывод можно заменить на более информативный:

```csharp
Console.WriteLine("Семинар 7. Основы тестирования в .NET");
Console.WriteLine("Основная логика проекта проверяется автоматизированными тестами:");
Console.WriteLine("- MSTest");
Console.WriteLine("- NUnit");
Console.WriteLine("- xUnit");
Console.WriteLine("- BDD / Reqnroll");
```

## Запуск тестов

Так как в проекте используется файл решения `.slnx`, в текущей версии .NET удобнее запускать тесты по отдельным проектам.

## Запуск MSTest

```bash
dotnet test MSTest_Console/MSTest_Console.csproj
```

Результат выполнения:

```text
Passed!  - Failed: 0, Passed: 5, Skipped: 0, Total: 5
```

## Запуск NUnit

```bash
dotnet test NUnit_Test_Console/NUnit_Test_Console.csproj
```

Результат выполнения:

```text
Passed!  - Failed: 0, Passed: 5, Skipped: 0, Total: 5
```

## Запуск xUnit

```bash
dotnet test XUnit_Test_Console/XUnit_Test_Console.csproj
```

Результат выполнения:

```text
Passed!  - Failed: 0, Passed: 5, Skipped: 0, Total: 5
```

## Запуск BDD-тестов

```bash
dotnet test XUnit_BDD/XUnit_BDD.csproj
```

Результат выполнения:

```text
Passed!  - Failed: 0, Passed: 7, Skipped: 0, Total: 7
```

## Запуск всех тестов подряд

```bash
dotnet test MSTest_Console/MSTest_Console.csproj && \
dotnet test NUnit_Test_Console/NUnit_Test_Console.csproj && \
dotnet test XUnit_Test_Console/XUnit_Test_Console.csproj && \
dotnet test XUnit_BDD/XUnit_BDD.csproj
```

## Результаты выполнения

В результате запуска всех тестовых проектов были получены следующие результаты:

```text
MSTest_Console      Passed: 5
NUnit_Test_Console  Passed: 5
XUnit_Test_Console  Passed: 5
XUnit_BDD           Passed: 7

Failed: 0
Skipped: 0
```

Все тесты успешно проходят.

## Соответствие методическим материалам

Проект соответствует методическим материалам семинара №7, так как в нём выполнены следующие пункты:

```text
1. Создан основной консольный проект на C#.
2. Созданы отдельные тестовые проекты.
3. Реализованы тесты с использованием MSTest.
4. Реализованы тесты с использованием NUnit.
5. Реализованы тесты с использованием xUnit.
6. Реализованы параметризованные тесты.
7. Использован паттерн AAA.
8. Реализован пример разработки через тестирование.
9. Реализован класс FamilyFineRule.
10. Реализованы BDD-сценарии на языке Gherkin.
11. Использован Reqnroll для связи feature-файлов с C#-кодом.
12. Все тесты успешно выполняются через dotnet test.
```

## Вывод

В ходе выполнения работы были изучены основы автоматизированного тестирования в .NET.

Были созданы отдельные тестовые проекты для трёх основных фреймворков модульного тестирования:

```text
MSTest
NUnit
xUnit
```

Также была реализована проверка поведения класса `FamilyFineRule` с использованием BDD-подхода, Reqnroll и Gherkin-сценариев.

Проект показывает, что автоматизированные тесты позволяют:

```text
быстро проверять корректность работы программы;
безопасно изменять и рефакторить код;
фиксировать ожидаемое поведение системы;
использовать тесты как исполняемую документацию;
поддерживать качество проекта при дальнейшей разработке.
```

Все реализованные тесты проходят успешно, следовательно, проект работает корректно и соответствует требованиям семинара.
