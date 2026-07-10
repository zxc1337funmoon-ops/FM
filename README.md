![FMTool](https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png)

# 🛡️ FMTool

**FMTool** — криминалистический античит-инструментарий для модераторов Minecraft-сервера **FunMoon**. Сканирует ПК подозреваемого, находит читы по 500+ сигнатурам, SHA-256 хэшам и эвристикам, анализирует .jar/.exe/.dll, запускает 12 форензик-утилит.

**Бесплатно** · **Открытый код** · **MIT**

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/github/v/release/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&label=%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F&color=3178C6" alt="Версия"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/badge/%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C-%D0%A0%D0%B5%D0%BB%D0%B8%D0%B7-28A745?style=for-the-badge&logo=github" alt="Скачать"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new"><img src="https://img.shields.io/badge/%D0%91%D0%B0%D0%B3-%D0%A1%D0%BE%D0%BE%D0%B1%D1%89%D0%B8%D1%82%D1%8C-D73A49?style=for-the-badge&logo=github" alt="Баг"></a>
  <br>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/Windows-10%20%7C%2011-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE"><img src="https://img.shields.io/badge/%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F-MIT-6B7280?style=for-the-badge&logo=open-source-initiative" alt="MIT"></a>
</p>

---

> ⚠️ **Важно:** FMTool — это **НЕ мод** для Minecraft. Это отдельное Windows-приложение для модераторов, не требующее установки в игру.

---

## 📌 О проекте

FMTool создан для модерации Minecraft-сервера **FunMoon**. Программа позволяет проверить ПК подозреваемого на наличие читов — от простых хак-клиентов до сложных обфусцированных модов.

**Что умеет:**
- 🎯 Обнаруживать любые модификации, дающие преимущество
- 🔬 Глубокий анализ .jar (декомпиляция, сигнатуры, обфускация)
- 🗂 Форензика: история запусков, USB-накопители, системные журналы
- 📊 Автоматическая скоринговая система с вердиктом

---

## ⬇️ Скачать

| Файл | Размер | Описание |
|------|:------:|----------|
| **FMTool-Installer.exe** | ~15 MB | Установщик (Inno Setup), при необходимости скачает .NET 8 |
| **FMTool.exe** | ~25 MB | Portable — без установки, запуск с любого носителя |

🔗 [Последний релиз](https://github.com/zxc1337funmoon-ops/FM/releases/latest) · [Все релизы](https://github.com/zxc1337funmoon-ops/FM/releases)

**Требования:** Windows 10/11 (64-bit) · .NET 8 Runtime · 4 ГБ RAM · 200 МБ на диске

---

## ✨ Возможности

### 🔍 Mod Checker

Загрузите .jar через Drag&Drop или кнопку «Обзор». Программа распакует ZIP, извлечёт .class, проанализирует манифест и структуру пакетов, декомпилирует классы, найдёт 70+ префиксов читов, проверит по SHA-256, выявит обфускацию (ProGuard, Allatori, Zelix, Stringer) и вынесет вердикт от 0 (чисто) до 100+ (чит).

**Поддержка мод-loaders:** Forge · Fabric · Quilt · LiteLoader · Rift

### 📂 Сканер дисков

Сканирует три зоны: **Системный диск (C:\)** · **Рабочий стол** · **Загрузки**.

| Метод | Что проверяет | Баллы |
|-------|---------------|:-----:|
| 📝 Имя файла | 150+ названий читов | +30 |
| 🔢 Размер | 130+ характерных размеров JAR-читов | +25 |
| 🔑 SHA-256 | Точное совпадение с базой | +99 |
| 📂 Расположение | %TEMP%, %APPDATA%, %LOCALAPPDATA% | +10 |
| 🎯 Ключевые слова | 260+ маркерных слов | +12 |
| 💡 Хэш-имена | hex-строка / GUID | +6–12 |
| 🌀 Энтропия | Обфускация / шифрование | +15 |
| 🏷 Сигнатуры классов | Известные классы читов | +25 |

### 📄 Отчёты

Детальный отчёт с цветовой маркировкой: 🟢 чисто → 🟡 подозрительно → 🔴 чит. Четыре вкладки: Детект, Классы, Структура, Метаданные. Экспорт в TXT и JSON.

### 📊 Система оценки

| Уровень | Баллы | Описание |
|:-------:|:-----:|----------|
| 🟢 **Чисто** | 0 | Ничего не найдено |
| 🔵 **Низкий** | 1–29 | Незначительные совпадения |
| 🟡 **Средний** | 30–59 | Требуется проверка модератором |
| 🟠 **Высокий** | 60–99 | Высокая вероятность чита |
| 🔴 **Чит** | 100+ | Точное совпадение SHA-256 или множественные сигнатуры |

### 🚀 12 встроенных инструментов

| Инструмент | Категория | Назначение |
|------------|:---------:|------------|
| **Everything** | 🔎 Поиск | Мгновенный поиск любых файлов на диске |
| **HxD** | 🔬 Hex | Редактирование шестнадцатеричных данных |
| **Luyten** | ☕ Java | Декомпиляция .class файлов |
| **SystemInformer** | 📊 Мониторинг | Детальный мониторинг процессов и сети |
| **InjGen** | 🛡 Безопасность | Обнаружение DLL-инъекций и внедрённых модулей |
| **CleaningDetector** | 🧹 Форензика | Поиск следов чистки улик |
| **JournalTrace** | 🗂 Форензика | Анализ USN Journal |
| **ShellBag Analyzer** | 📁 Форензика | История открытия папок из реестра |
| **USBDriveLog** | 💾 Форензика | Журнал подключения USB-накопителей |
| **ExecutedProgramsList** | 🕐 Форензика | Список запускавшихся программ |
| **PrefetchView++** | ⏳ Форензика | Хронология запуска (.pf файлы) |
| **PathsParser** | 🧩 Артефакты | Извлечение путей из ShellBags, JumpLists, реестра |

### 🎨 Интерфейс

7 тем: **Dark** · **Arctic** · **Emerald** · **Rose** · **Cherry** · **Gold** · **Violet**. Плавные анимации, стеклянный эффект (Acrylic), закруглённые углы. Русский и английский языки — переключение на лету.

### ⚙️ Интеграция

- 📌 Системный трей — работает в фоне
- 🔄 Автозапуск при старте Windows
- ⌨ Горячие клавиши: Ctrl+S (скан) · Ctrl+E (экспорт) · Ctrl+F (поиск)

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 Сканер — методология</b></summary>
<br>

Каждому файлу присваиваются баллы по нескольким методам. Баллы суммируются. При сумме ≥ 100 — вердикт «Чит».

| Метод | Что проверяет | Макс. баллов |
|-------|---------------|:------------:|
| 📝 Имя файла | Совпадение со 150+ названиями читов | +30 |
| 🔢 Размер | Совпадение со 130+ размерами | +25 |
| 🔑 SHA-256 | Точное совпадение с базой | +99 |
| 🎯 Ключевые слова | 260+ маркерных слов в классах | +12 |
| 📂 Расположение | %TEMP%, %APPDATA%, Downloads | +10 |
| 💡 Хэш-имя | hex-строка / GUID | +6–12 |
| 🌀 Энтропия | Высокая → упаковка / обфускация | +15 |
| 🏷 Сигнатуры | Известные классы и пакеты читов | +25 |

</details>

<details>
<summary><b>🛡️ Белый список</b></summary>
<br>

Файлы из белого списка **никогда** не считаются читами:

| Категория | Примеры |
|-----------|---------|
| 🎮 **Ядра Minecraft** | minecraft, netty, lwjgl, authlib, paulscode, jinput |
| ⚙ **Популярные моды** | optifine, sodium, iris, lithium, create, jei, rei, twilight |
| 📡 **Библиотеки** | gson, fastutil, jackson, guava, log4j, apache, cglib, asm |
| 🖥 **Системные** | ntdll, kernel32, shell32, user32, comctl32, advapi32 |
| 📖 **FunMoon** | fmutils, fm-utils, funmoon-core, fmcore |

</details>

<details>
<summary><b>🏴 PE-анализ (.exe / .dll)</b></summary>
<br>

| Признак | Что обнаруживает |
|---------|------------------|
| 📦 Упаковщики | Секции .upack, .themida, .vmp, .mpress |
| 🏴 Импорты | WriteProcessMemory, CreateRemoteThread, VirtualAllocEx |
| 🕰 Время компиляции | Нулевое или из будущего |
| 💠 Overlay | Данные за последней секцией |
| 🔐 Цифровая подпись | Отсутствует или невалидна |
| 🎮 TLS-колбэки | Нестандартные callback'и |
| 🎯 .NET обфускация | ConfuserEx, Obfuscar, .NET Reactor, SmartAssembly |

</details>

<details>
<summary><b>🔑 SHA-256 база данных</b></summary>
<br>

Встроенная локальная база хэшей известных читов. Каждый найденный .jar / .exe / .dll хэшируется, сверяется с базой. При совпадении → **+99 баллов**, вердикт «Чит». База обновляется с каждым релизом — администраторы FunMoon добавляют хэши новых читов.

</details>

---

## 🛠 Технические детали

| Параметр | Значение |
|----------|----------|
| 💻 **Платформа** | .NET 8 WPF, Windows 10/11 x64 |
| 🏗 **Архитектура** | MVVM (Model-View-ViewModel) |
| 📦 **Упаковка** | Single-file self-contained, LZMA2 |
| 📄 **Установщик** | Inno Setup 6 |
| ☕ **JRE** | Встроенная Java 8 (jre-8u491) для декомпиляции .class |
| 🔗 **Нативные вызовы** | Win32 API (kernel32, ntdll, user32) |
| 📚 **Библиотеки** | .NET Community Toolkit, Newtonsoft.Json |
| 🌐 **Языки** | Русский · Английский |
| 📜 **Лицензия** | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) |

---

## 🔒 Конфиденциальность

- 🛡 FMTool **не собирает** и **не передаёт** данные пользователей
- 🚫 **Не отправляет** хэши, файлы или логи на внешние серверы
- 💾 База сигнатур **локальная**, встроена в .exe
- 🧹 При удалении **не оставляет** следов в системе
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