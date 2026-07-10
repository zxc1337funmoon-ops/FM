<div align="center">

# FMTool

### Minecraft Anti-Cheat Toolkit · Mod Checker · File Forensics

**FMTool** — мощный инструмент для проверки Minecraft модов на наличие чита, анализа подозрительных файлов и форензики. Разработан для серверов **FunMoon**.

[![Release](https://img.shields.io/github/v/release/zxc1337funmoon-ops/FM?style=flat-square&logo=github&color=3178C6)](https://github.com/zxc1337funmoon-ops/FM/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/zxc1337funmoon-ops/FM/total?style=flat-square&logo=docusign&color=3178C6)](https://github.com/zxc1337funmoon-ops/FM/releases)
[![License](https://img.shields.io/badge/License-MIT-3178C6?style=flat-square)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-7B1FA2?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Windows](https://img.shields.io/badge/Windows-10-11-0078D6?style=flat-square&logo=windows)](https://github.com/zxc1337funmoon-ops/FM/releases)

</div>

---

## О FMTool

FMTool — швейцарский нож для модератора Minecraft. Анализирует моды, находит читы, собирает доказательства.

| Для кого | Зачем |
|----------|-------|
| Модераторы Minecraft | Проверить подозрительный мод |
| Администраторы | Собрать доказательства для бана |
| Специалисты безопасности | Автоматизировать проверки |

---

## Возможности

### Mod Checker
- **300+ сигнатур** — база известных читов
- **SHA-256 хэш** — уникальный отпечаток файла
- **Байт-код анализ** — поиск подозрительных паттернов
- **Оценка угрозы** — от 0 до 100+ с деталями
- **4 потока** — мгновенное сканирование пачек файлов

### File Forensics
- PE-анализатор — проверка exe/dll
- Энтропия — выявление упаковщиков
- YARA-правила — кастомные сигнатуры
- Извлечение строк из бинарников
- 5 уровней угрозы с группировкой

### Report Window
- Два окна: навигация и просмотр
- Экспорт в TXT, HTML (с подсветкой), JSON

### Удобства
- Автообновление
- Drag-and-drop
- Плавные темы с анимацией
- Адаптивный интерфейс
- Горячие клавиши
- Автозапуск

---

## Инструменты

14 утилит для расследования:

| # | Инструмент | Назначение |
|---|-----------|-----------|
| 1 | HxD | Hex-редактор |
| 2 | Everything | Поиск файлов |
| 3 | SystemInformer | Мониторинг процессов |
| 4 | Luyten | Декомпилятор Java |
| 5 | PrefetchView++ | Анализ Prefetch |
| 6 | USBDriveLog | Журнал USB |
| 7 | JournalTrace | Анализ USN Journal |
| 8 | ShellBag Analyzer | Разбор ShellBag |
| 9 | ExecutedProgramsList | Список запусков |
| 10 | CleaningDetector | Детект чисток |
| 11 | PathsParser | Парсинг путей |
| 12 | InjGen | Обнаружение инъекций |
| 13 | Java JRE 8 | Установщик Java |
| 14 | И другие | По запросу |

---

## Темы оформления

- **Dark** — классическая тёмная
- **Arctic** — холодная сине-голубая
- **Emerald** — глубокая изумрудная
- **Rose** — мягкая розовая
- **Cherry** — насыщенная красная
- **Gold** — роскошная золотистая
- **Violet** — фиолетовая мистическая

Эффект стекла, плавные переходы, сглаженные углы.

---

## Языки

Русский и английский — интерфейс и отчёты. Переключение на лету.

---

## Установка

### Быстрый старт
1. Скачай последний релиз
2. Запусти .exe
3. Готово!

### Portable
- Без установки, без следов
- Работает с флешки
- Не требует прав админа

### Setup
- Ярлык в меню Пуск
- Ассоциации файлов
- Нужны права админа

---

## Системные требования

| Параметр | Требование |
|----------|-----------|
| ОС | Windows 10 / 11 (x64) |
| RAM | от 2 GB |
| Диск | ~250 MB (setup) / ~50 MB (portable) |
| .NET | Встроен (self-contained) |

---

## FAQ

**Антивирус ругается?** Добавь папку в исключения.

**Не открываются утилиты?** Скачай их отдельно.

**Не работает Java?** Нажми Java JRE 8 в панели.

**macOS / Linux?** Пока нет, только Windows.

---

## Roadmap

- [x] Mod Checker (300+ сигнатур)
- [x] File Forensics
- [x] 7 тем оформления
- [x] 14 утилит
- [x] Два языка
- [ ] Скриншоты
- [ ] Демо GIF
- [ ] CI/CD сборка
- [ ] Веб-отчёты
- [ ] API для сервера
- [ ] Sandbox
- [ ] Telegram Bot

---

## Список изменений

### v1.0.2
- Установщик Java JRE 8
- Поиск по классам
- Исправлен парсинг сигнатур
- 70+ файлов в белый список
- Улучшен UAC

### v1.0.1
- Чистка мусора
- Удалены дубликаты

### v1.0.0
- Первый релиз
- Mod Checker, сканнер, 7 тем, 12 утилит

---

## Лицензия

MIT License.

---

<div align="center">

Сделано с любовью для **FunMoon**

Автор: **zxc1337**

[Скачать](https://github.com/zxc1337funmoon-ops/FM/releases/latest) · [Баг](https://github.com/zxc1337funmoon-ops/FM/issues) · [Идея](https://github.com/zxc1337funmoon-ops/FM/issues)

</div>