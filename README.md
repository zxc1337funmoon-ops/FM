<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="130"></a>
</p>

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/github/v/release/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&label=%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F&color=3178C6" alt="Версия"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/badge/%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C-%D0%A0%D0%B5%D0%BB%D0%B8%D0%B7-28A745?style=for-the-badge&logo=github" alt="Скачать"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new"><img src="https://img.shields.io/badge/%D0%91%D0%B0%D0%B3-%D0%A1%D0%BE%D0%BE%D0%B1%D1%89%D0%B8%D1%82%D1%8C-D73A49?style=for-the-badge&logo=github" alt="Баг"></a>
  <br>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/Windows-10%20%7C%2011-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE"><img src="https://img.shields.io/badge/%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F-MIT-6B7280?style=for-the-badge&logo=open-source-initiative" alt="MIT"></a>
</p>

# 🛡️ FMTool

**FMTool** — криминалистический античит-инструментарий для модераторов Minecraft-сервера **FunMoon**. Сканирует ПК подозреваемого, анализирует .jar/.exe/.dll по базе сигнатур, SHA-256 хэшам, размерам и эвристикам. Запускает 12 встроенных форензик-утилит.

**Бесплатно** · **Открытый код** · **MIT**

---

> ⚠️ **Важно:** FMTool — это **НЕ мод** для Minecraft. Это отдельное Windows-приложение для модераторов.

---

## 📦 Файлы для скачивания

| Файл | Размер | Описание |
|------|:------:|----------|
| **FMTool-1.0.3.exe** | ~179 MB | Портативная версия — без установки, запуск с любого носителя |
| **FMTool-Setup-1.0.3.exe** | ~151 MB | Установщик (Inno Setup, LZMA2), скачает .NET 8 при необходимости |

🔗 [Последний релиз](https://github.com/zxc1337funmoon-ops/FM/releases/latest) · [Все релизы](https://github.com/zxc1337funmoon-ops/FM/releases)

**Требования:** Windows 10/11 64-bit · .NET 8 Runtime · 4 ГБ RAM · 200 МБ на диске

---

## 🛠 Что умеет программа

### 🔍 Mod Checker

Загрузите .jar через Drag&Drop или кнопку «Обзор». Mod Checker анализирует файл в 12 слоёв: имя файла (190 названий читов + 319 banned-модов), размер (142 известных размера), SHA-256 (17 хэшей), ZIP-структуру, MANIFEST.MF, мета-файлы модов (mcmod.info, fabric.mod.json, mods.toml), class-файлы (декoмпиляция, 365 ключевых слов, энтропия, версия Java, обфускация), mixin JSON, текстовое содержимое, вложенные архивы, ресурсы (PNG, lang), версию Minecraft. Дополнительно — интеллектуальный профайлер с 20+ метриками и корреляционными бонусами.

**Поддержка мод-loaders:** Forge · Fabric · Quilt · LiteLoader · Rift

### 📂 Scanner

Сканирует три зоны: системный диск, Рабочий стол, Загрузки. Ищет .jar/.exe/.dll. Первый проход (Classify) оценивает имя, размер, расположение, подозрительные ключевые слова, хэш-имена. Второй проход (DeepClassify) — глубокий анализ: PE-структура (.exe/.dll), секции, импорты, обфускация .NET, содержимое JAR/ZIP (пакеты, классы, ключевые слова, подписи обфускаторов). Каждый файл получает баллы, от 5+ — «Подозрительный».

### 📄 Report Window

Детальный отчёт с цветовой маркировкой. Четыре вкладки: Детект, Классы, Структура, Метаданные. Экспорт в TXT и JSON.

### 📊 Система оценки

Три индикатора подозрения:
- 🟢 **Normal** — чисто, ничего не найдено
- 🟡 **Suspicious** — требует проверки модератором
- 🔴 **Critical** — обнаружен чит (SHA-256 совпадение или множественные сигнатуры)

### 🚀 12 встроенных утилит

| Инструмент | Назначение |
|------------|------------|
| **HxD** | Шестнадцатеричный редактор для анализа файлов |
| **Everything** | Мгновенный поиск файлов по паттернам |
| **SystemInformer** | Мониторинг процессов и инъекций |
| **Luyten** | Декомпилятор Java (.jar/.class) |
| **InjGen** | Детектирование DLL-инъекций |
| **CleaningDetector** | Поиск следов чистки улик |
| **JournalTrace** | Анализ USN Journal (удалённые файлы) |
| **ShellBag Analyzer** | История посещения папок (ShellBags) |
| **USBDriveLog** | Журнал подключения USB-накопителей |
| **ExecutedProgramsList** | Список запускавшихся программ |
| **PrefetchView++** | Хронология запуска (.pf файлы) |
| **PathsParser** | Извлечение путей из артефактов Windows |

### 🎨 Интерфейс

7 тем: **Dark** · **Arctic** · **Emerald** · **Rose** · **Cherry** · **Gold** · **Violet**. Русский и английский язык. 7 навигационных вкладок. Плавные анимации.

### ⚙️ Интеграция

Системный трей — работает в фоне. Автозапуск при старте Windows. Горячие клавиши: Ctrl+S (скан) · Ctrl+E (экспорт) · Ctrl+F (поиск).

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 База данных — точные цифры из кода</b></summary>
<br>

| Категория | Количество |
|-----------|:----------:|
| Названия читов | 190 |
| Имена banned-модов | 319 |
| Ключевые слова (поиск по содержимому) | 365 |
| Известные размеры JAR-читов | 142 |
| SHA-256 хэши читов | 17 |
| Пути пакетов читов (Mod Checker) | 248 |
| Термины читерских модулей | 38 |
| Подозрительные ключевые слова имён | 78 |
| Сигнатуры обфускаторов (Mod Checker) | 59 |
| Байт-сигнатуры (YARA-подобные) | 7 |
| Известные cheat-пути (глубокий поиск) | 126 |
| Белый список (не считаются читами) | 204 |
| Легитимные термины (FP-редукция) | 129 |

</details>

<details>
<summary><b>📊 Сканер — баллы подозрения</b></summary>
<br>

**Проход 1 — Classify (быстрая оценка):**

| Проверка | Баллы |
|----------|:-----:|
| Имя совпадает со списком читов | +30 |
| Размер совпадает с базой читов | +25 |
| Имя как хэш (32+ hex) | +12 |
| Подозрительное ключевое слово в имени | +12 |
| .exe/.dll в %TEMP% или Downloads | +10 |
| Имя как GUID | +10 |
| 5+ цифр подряд в имени | +6 |
| Нет гласных, 12+ символов | +5 |
| Скрытый файл (.префикс) | +5 |
| Двойное расширение | +4 |

**Проход 2 — DeepClassify (глубокий анализ):**

| Проверка | Баллы |
|----------|:-----:|
| SHA-256 совпадает с базой | +60 |
| 3+ читерских модуля в содержимом | +25 |
| 2+ известных cheat-пути | +20 |
| PE: плохие секции (.upack, .themida, .vmp) | +18 |
| PE: подозрительные импорты | +15 |
| .NET обфускатор | +15 |
| JAR: manifest main-class = чит | +20 |
| JAR: banned-mod в метаданных | +30 |
| JAR: 2+ известных корня пакетов | +20 |
| JAR: 2+ подписи обфускаторов | +10 |
| Энтропия >7.5 (упаковка/шифрование) | +10 |
| Вложенный архив с ключевым словом | +10 |

> Баллы суммируются. При ≥15 — «Подозрительный». При ≥80 — «Критично».

</details>

<details>
<summary><b>🛡️ Белый список</b></summary>
<br>

Никогда не считаются читами (204 записи):

| Категория | Примеры |
|-----------|---------|
| 🎮 **Ядра Minecraft** | minecraft, netty, lwjgl, authlib, paulscode, jinput, joml |
| ⚙ **Моды и инструменты** | optifine, sodium, iris, lithium, fabric-api, forge, recaf-cli, create, jei, rei |
| 📡 **Библиотеки** | gson, fastutil, jackson, guava, log4j, apache, cglib, asm, slf4j, snakeyaml |
| 🖥 **Системные** | ntdll, kernel32, shell32, user32, gdi32, comctl32, advapi32 |
| 📖 **FunMoon** | fmutils, fm-utils, funmoon-core, fmcore |

</details>

<details>
<summary><b>🏴 PE-анализ (.exe / .dll)</b></summary>
<br>

| Признак | Что обнаруживает |
|---------|------------------|
| 📦 Плохие секции | .upack, .themida, .vmp, .enigma, .mpress, .upx1, .armadillo |
| 🏴 Опасные импорты | WriteProcessMemory, CreateRemoteThread, NtQueryInformationProcess |
| 🕰 Время компиляции | Нулевое или из будущего |
| 💠 Overlay-данные | Данные за последней секцией |
| 🔐 Цифровая подпись | Отсутствует |
| 🎮 TLS-колбэки | Нестандартные callback'и |
| 🎯 .NET обфускация | ConfuserEx, Obfuscar, .NET Reactor, SmartAssembly, CryptoObfuscator |
| 📦 Упаковщики | UPX, MPRESS (поиск байт-сигнатур) |

</details>

<details>
<summary><b>🔑 SHA-256 база</b></summary>
<br>

Встроенная локальная база хэшей: Wurst, Meteor, Aristois, Impact, LiquidBounce, Future, Sigma, BleachHack, Inertia, RusherHack, Konas, DoomsDay, Nursultan, Celestial, Wexside, FuzeClient, WildClient. При совпадении +60 баллов, вердикт «Critical». База обновляется с каждым релизом.

</details>

---

## 🛠 Технические детали

| Параметр | Значение |
|----------|----------|
| **Платформа** | .NET 8 WPF, Windows 10/11 x64 |
| **Архитектура** | 7 страниц, MVVM, Parallel.ForEach |
| **Упаковка** | Single-file self-contained |
| **Сжатие** | LZMA2 (максимальное) |
| **Установщик** | Inno Setup 6 |
| **JRE** | Встроенная Java 8 (jre-8u491) |
| **Native API** | Win32 (kernel32, ntdll, user32) |
| **Библиотеки** | .NET Community Toolkit, Newtonsoft.Json |
| **Размер окна** | 1260×770 (мин. 1040×640) |
| **Языки** | Русский, Английский (133 ключа) |
| **Лицензия** | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) |

---

## 🔒 Конфиденциальность

- 🛡 FMTool **не собирает** и **не передаёт** данные пользователей
- 🚫 **Не отправляет** хэши, файлы или логи на серверы
- 💾 База сигнатур — **локальная**, вшита в .exe
- 🧹 Не требует интернета для работы
- 🔍 Единственная цель — найти читы на проверяемом ПК

---

<p align="center">
  <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="40">
  <br><br>
  <i>Сделано с ❤️ для сообщества <b>FunMoon</b></i>
  <br>
  <b>Автор:</b> <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a>
  <br>
  © 2026 · <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT License</a>
  <br><br>
  <a href="https://github.com/zxc1337funmoon-ops/FM">GitHub</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/releases">Releases</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">Баги и предложения</a>
</p>