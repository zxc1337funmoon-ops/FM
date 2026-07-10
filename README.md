<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="130"></a>
</p>

<h1 align="center">🛡️ FMTool</h1>

<p align="center">
  <i>Криминалистический античит-инструментарий для модераторов Minecraft-сервера <b>FunMoon</b></i>
  <br><br>
  Сканирует ПК подозреваемого, находит читы по 500+ сигнатурам, SHA-256 хэшам и эвристикам,
  <br>
  анализирует .jar/.exe/.dll, запускает 12 форензик-утилит.
  <br><br>
  <b>Бесплатно</b> · <b>Открытый код</b> · <b>MIT</b>
</p>

<br>

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

## 📌 О проекте

**FMTool** — специализированное десктопное приложение для модераторов Minecraft-сервера **FunMoon**. Позволяет проверить ПК подозреваемого на наличие читов: от классических хак-клиентов до сложных обфусцированных модов.

**Основные задачи:**
- 🎯 Обнаружение любых модификаций, дающих преимущество
- 🔬 Глубокий анализ .jar (декомпиляция, сигнатуры, обфускация)
- 🗂 Форензика: история запусков, USB-накопители, системные журналы
- 📊 Автоматическая скоринговая система с вердиктом

> ⚠️ **Важно:** FMTool — это **НЕ мод** для Minecraft. Это отдельное Windows-приложение для модераторов.

---

## ⬇️ Скачать

| Файл | Размер | Описание |
|------|:------:|----------|
| **FMTool-Installer.exe** | ~15 MB | Установщик (Inno Setup) — скачает .NET 8 Runtime при необходимости |
| **FMTool.exe** | ~25 MB | Portable версия — работает без установки, с любого носителя |

🔗 [Последний релиз](https://github.com/zxc1337funmoon-ops/FM/releases/latest) · [Все релизы](https://github.com/zxc1337funmoon-ops/FM/releases)

**Требования:**
- Windows 10 или 11 (64-bit)
- .NET 8 Runtime (установщик скачает автоматически, если не установлен)
- 4 ГБ RAM · 200 МБ свободного места на диске

---

## ✨ Возможности

### 🔍 Mod Checker — проверка .jar модов

Загрузите .jar-файл через Drag&Drop или кнопку «Обзор». Программа:
1. 📦 Распакует ZIP-архив, извлечёт все .class файлы
2. 📋 Проанализирует манифест, meta-inf и структуру пакетов
3. ☕ Декомпилирует классы (встроенный Luyten)
4. 🔎 Найдёт 70+ префиксов читов в путях пакетов
5. 🧪 Проверит по 500+ сигнатурам и SHA-256 базе
6. 🏷 Выявит обфускацию (ProGuard, Allatori, Zelix, Stringer)
7. 📊 Вынесет вердикт с баллами: от 0 (чисто) до 100+ (чит)

**Поддержка мод-loaders:** Minecraft Forge, Fabric, Quilt, LiteLoader, Rift.

### 📂 Сканер дисков — поиск по всему ПК

Сканирует три ключевые зоны: **Системный диск** (C:\) · **Рабочий стол** · **Папка Загрузок**.

| Метод | Описание | Баллы |
|-------|----------|:-----:|
| 📝 Имя файла | 150+ известных названий читов | +30 |
| 🔢 Размер файла | 130+ характерных размеров JAR-читов | +25 |
| 🔑 SHA-256 | Точный хэш из базы FMTool | +99 |
| 📂 Подозрительные папки | %TEMP%, %APPDATA%, %LOCALAPPDATA% | +10 |
| 🎯 Ключевые слова | 260+ слов внутри классов и конфигов | +12 |
| 💡 Хэш-имена | 32+ hex, GUID — признак маскировки | +6–12 |
| 🌀 Энтропия | Высокая → обфускация/шифрование | +15 |
| 🏷 Сигнатуры | Известные классы и пакеты читов | +25 |

### 📄 Отчёты — детализация каждого файла

После проверки открывается окно с подробным отчётом:
- **4 вкладки:** Детект, Классы, Структура, Метаданные
- 🎨 **Цветовая маркировка:** зелёный (чисто) → жёлтый (подозрительно) → красный (чит)
- 📤 **Экспорт:** TXT (читаемый), JSON (машинная обработка)

### 📊 Система оценки

| Уровень | Баллы | Значение |
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
| **HxD** | 🔬 Hex | Просмотр и редактирование шестнадцатеричных данных |
| **Luyten** | ☕ Java | Декомпиляция .class файлов из модов |
| **SystemInformer** | 📊 Мониторинг | Детальный мониторинг процессов и сети |
| **InjGen** | 🛡 Безопасность | Обнаружение DLL-инъекций и внедрённых модулей |
| **CleaningDetector** | 🧹 Форензика | Поиск следов чистки улик (журналы, tmp, Prefetch) |
| **JournalTrace** | 🗂 Форензика | Анализ USN Journal — история изменений файлов |
| **ShellBag Analyzer** | 📁 Форензика | Извлечение истории открытия папок из реестра |
| **USBDriveLog** | 💾 Форензика | Журнал подключения USB-накопителей |
| **ExecutedProgramsList** | 🕐 Форензика | Список запускавшихся программ (UserAssist, Prefetch) |
| **PrefetchView++** | ⏳ Форензика | Хронология запуска приложений (.pf файлы) |
| **PathsParser** | 🧩 Артефакты | Извлечение путей из ShellBags, JumpLists, реестра |

### 🎨 Интерфейс
- **7 тем оформления:** Dark · Arctic · Emerald · Rose · Cherry · Gold · Violet
- Плавные анимации, стеклянный эффект (Acrylic), закруглённые углы
- Русский и английский язык — переключение на лету без перезапуска

### ⚙️ Интеграция с системой
- 📌 **Системный трей** — работает в фоне
- 🔄 **Автозапуск** при старте Windows
- 📂 **Контекстное меню** .jar в Проводнике → «Проверить в FMTool»
- 🔗 **Ассоциация файлов** .jar
- ⌨ **Горячие клавиши:** Ctrl+S (скан), Ctrl+E (экспорт), Ctrl+F (поиск)

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 Сканер — методология и баллы</b></summary>
<br>

Каждому найденному файлу присваиваются баллы подозрения по нескольким методам одновременно:

| Метод | Что проверяет | Макс. баллов |
|-------|---------------|:------------:|
| 📝 Имя файла | Совпадение со 150+ названиями читов | +30 |
| 🔢 Размер | Совпадение со 130+ размерами JAR-читов | +25 |
| 🔑 SHA-256 | Точное совпадение с базой | +99 |
| 🎯 Ключевые слова | 260+ маркерных слов внутри классов | +12 |
| 📂 Расположение | %TEMP%, %APPDATA%, Downloads | +10 |
| 💡 Хэш-имя | Имя файла как hex-строка / GUID | +6–12 |
| 🌀 Энтропия | Высокая → упаковка / обфускация | +15 |
| 🏷 Сигнатуры | Известные классы и пакеты читов | +25 |

**Баллы суммируются.** Если общая сумма ≥ 100 — файл маркируется как «Чит».

</details>

<details>
<summary><b>🛡️ Белый список — никогда не считается читом</b></summary>
<br>

| Категория | Примеры |
|-----------|---------|
| 🎮 **Ядра Minecraft** | minecraft, netty, lwjgl, authlib, paulscode, jinput |
| ⚙ **Популярные моды** | optifine, sodium, iris, lithium, create, jei, rei, twilight, bhc |
| 📡 **Библиотеки** | gson, fastutil, jackson, guava, log4j, apache, cglib, asm, slf4j |
| 🖥 **Системные** | ntdll, kernel32, shell32, user32, comctl32, advapi32 |
| 📖 **FunMoon** | fmutils, fm-utils, funmoon-core, fmcore |

</details>

<details>
<summary><b>🏴 PE-анализ — проверка .exe / .dll</b></summary>
<br>

| Признак | Что обнаруживает |
|---------|------------------|
| 📦 Упаковщики | Секции: .upack, .themida, .vmp, .mpress, .packer |
| 🏴 Опасные импорты | WriteProcessMemory, CreateRemoteThread, VirtualAllocEx |
| 🕰 Время компиляции | Нулевое или из будущего — подделка |
| 💠 Overlay-данные | Данные за последней PE-секцией |
| 🔐 Отсутствие подписи | Нет цифровой подписи или она невалидна |
| 🎮 TLS-колбэки | Нестандартные callback'и — подозрение |
| 🎯 .NET обфускация | ConfuserEx, Obfuscar, .NET Reactor, SmartAssembly, Xenocode |

</details>

<details>
<summary><b>🔑 SHA-256 база данных</b></summary>
<br>

FMTool содержит встроенную локальную базу SHA-256 хэшей известных читов:
- ✅ Каждый найденный .jar / .exe / .dll хэшируется (SHA-256)
- ✅ Хэш сверяется с базой
- ✅ При совпадении → **+99 баллов**, вердикт «Чит»
- ✅ База обновляется с каждым новым релизом

> 🔄 При обнаружении нового чита на сервере администраторы добавляют хэш — он появляется в следующей версии FMTool.

</details>

---

## 🛠 Технические детали

| Параметр | Значение |
|----------|----------|
| 💻 **Платформа** | .NET 8 WPF, Windows 10/11 x64 |
| 🏗 **Архитектура** | MVVM (Model-View-ViewModel) |
| 📦 **Упаковка** | Single-file self-contained, LZMA2 сжатие |
| 📄 **Установщик** | Inno Setup 6 |
| ☕ **Встроенная JRE** | Java 8 (jre-8u491) — для декомпиляции .class |
| 🔗 **Нативные вызовы** | Win32 API (kernel32, ntdll, user32) |
| 📚 **Основные библиотеки** | .NET Community Toolkit, Newtonsoft.Json |
| 🌐 **Языки** | Русский · Английский |
| 📜 **Лицензия** | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) |

---

## 🔒 Конфиденциальность

- 🛡 FMTool **не собирает** и **не передаёт** данные пользователей
- 🚫 **Не отправляет** хэши, файлы или логи на внешние серверы
- 💾 Вся база сигнатур — **локальная**, встроена непосредственно в .exe
- 🧹 При удалении программы **не остаётся следов** в системе
- 🔍 Единственная цель — найти читы на проверяемом ПК

---

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="40"></a>
  <br><br>
  <i>Сделано с ❤️ для сообщества <b>FunMoon</b></i>
  <br>
  <b>Автор:</b> <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a>
  <br>
  © 2026 · <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT License</a>
  <br><br>
  <a href="https://github.com/zxc1337funmoon-ops/FM">GitHub</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/releases">Releases</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">Баги и предложения</a>
</p>