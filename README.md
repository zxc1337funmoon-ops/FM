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

**FMTool** — криминалистический античит-инструментарий для модераторов Minecraft-сервера **FunMoon**. Сканирует ПК подозреваемого, находит читы по 500+ сигнатурам, SHA-256 хэшам и эвристикам, анализирует .jar/.exe/.dll, запускает 12 форензик-утилит.

**Бесплатно** · **Открытый код** · **MIT**

---

> ⚠️ **Важно:** FMTool — это **НЕ мод** для Minecraft. Это отдельное Windows-приложение для модераторов, не требующее установки в игру.

---

## 📦 Файлы для скачивания

| Файл | Размер | Описание |
|------|:------:|----------|
| **FMTool-1.0.3.exe** | ~179 MB | 📦 Портативная версия — без установки, с любого носителя |
| **FMTool-Setup-1.0.3.exe** | ~151 MB | 🚀 Установщик (Inno Setup) — скачает .NET 8 при необходимости |

🔗 [Последний релиз](https://github.com/zxc1337funmoon-ops/FM/releases/latest) · [Все релизы](https://github.com/zxc1337funmoon-ops/FM/releases)

**Требования:** Windows 10/11 (64-bit) · .NET 8 Runtime · 4 ГБ RAM · 200 МБ на диске

---

## 🛠 Что умеет программа

### 🔍 Mod Checker

Загрузите .jar-файл через Drag&Drop или кнопку «Обзор». Программа распакует ZIP, проанализирует манифест и структуру, декомпилирует .class, найдёт 70+ префиксов читов, проверит по SHA-256, выявит обфускацию (ProGuard, Allatori, Zelix, Stringer) и вынесет вердикт от 0 (чисто) до 100+ (чит).

**Поддержка:** Forge · Fabric · Quilt · LiteLoader · Rift · 500+ сигнатур

### 📂 Scanner

Сканирует три зоны: системный диск, Рабочий стол, Загрузки. Ищет .jar, .exe, .dll по 150+ названиям читов, 130+ размерам, SHA-256 хэшам, 260+ ключевым словам, энтропии и сигнатурам классов. Каждый файл получает баллы подозрения — чем выше, тем вероятнее чит.

### 📄 Report Window

Детальный отчёт с цветовой маркировкой (🟢 → 🟡 → 🔴). Четыре вкладки: Детект, Классы, Структура, Метаданные. Экспорт в TXT и JSON.

### 📊 Система оценки

| Уровень | Баллы | Значение |
|:-------:|:-----:|----------|
| 🟢 **Чисто** | 0 | Ничего не найдено |
| 🔵 **Низкий** | 1–29 | Незначительные совпадения |
| 🟡 **Средний** | 30–59 | Требуется проверка |
| 🟠 **Высокий** | 60–99 | Высокая вероятность чита |
| 🔴 **Чит** | 100+ | SHA-256 совпадение или множественные сигнатуры |

### 🚀 Встроенные утилиты (12 шт)

- **HxD** — шестнадцатеричный редактор
- **Everything** — мгновенный поиск файлов
- **SystemInformer** — мониторинг процессов (аналог Process Explorer)
- **Luyten** — декомпилятор Java
- **Остальные:** PrefetchView++, USBDriveLog, JournalTrace, ShellBag Analyzer, ExecutedProgramsList, CleaningDetector, PathsParser, InjGen

### 🎨 Интерфейс

7 тем: **Dark** · **Arctic** · **Emerald** · **Rose** · **Cherry** · **Gold** · **Violet**. Плавные анимации, стеклянный эффект (Acrylic), закруглённые углы. Русский и английский язык.

### ⚙️ Интеграция

Системный трей, автозапуск при старте Windows. Горячие клавиши: Ctrl+S (скан) · Ctrl+E (экспорт) · Ctrl+F (поиск).

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 Сканер — методология и баллы</b></summary>
<br>

| Метод | Что проверяет | Баллы |
|-------|---------------|:-----:|
| Имя файла | 150+ названий читов | +30 |
| Размер файла | 130+ размеров JAR-читов | +25 |
| SHA-256 | Точное совпадение с базой | +99 |
| Ключевые слова | 260+ маркерных слов | +12 |
| Расположение | %TEMP%, %APPDATA%, Downloads | +10 |
| Хэш-имена | hex / GUID | +6–12 |
| Энтропия | Обфускация / шифрование | +15 |
| Сигнатуры | Известные классы читов | +25 |

> Баллы суммируются. При сумме ≥ 100 — вердикт «Чит».

</details>

<details>
<summary><b>🛡️ Белый список</b></summary>
<br>

Никогда не считаются читами:

| Категория | Примеры |
|-----------|---------|
| 🎮 **Ядра Minecraft** | minecraft, netty, lwjgl, authlib, paulscode, jinput |
| ⚙ **Популярные моды** | optifine, sodium, iris, lithium, create, jei, rei, twilight |
| 📡 **Библиотеки** | gson, fastutil, jackson, guava, log4j, apache |
| 🖥 **Системные** | ntdll, kernel32, shell32, user32 |
| 📖 **FunMoon** | fmutils, fm-utils, funmoon-core |

</details>

<details>
<summary><b>🏴 PE-анализ (.exe / .dll)</b></summary>
<br>

| Признак | Что обнаруживает |
|---------|------------------|
| 📦 Упаковщики | Секции .upack, .themida, .vmp, .mpress |
| 🏴 Импорты | WriteProcessMemory, CreateRemoteThread |
| 🕰 Время компиляции | Нулевое или из будущего |
| 💠 Overlay | Данные за последней секцией |
| 🔐 Подпись | Отсутствует или невалидна |
| 🎯 .NET обфускация | ConfuserEx, Obfuscar, .NET Reactor |

</details>

<details>
<summary><b>🔑 SHA-256 база данных</b></summary>
<br>

Встроенная локальная база хэшей известных читов. Каждый .jar/.exe/.dll хэшируется и сверяется. При совпадении → **+99 баллов**, вердикт «Чит». База обновляется с каждым релизом.

</details>

---

## 🛠 Технические детали

| Параметр | Значение |
|----------|----------|
| 💻 **Платформа** | .NET 8 WPF, Windows 10/11 x64 |
| 🏗 **Архитектура** | MVVM (Model-View-ViewModel) |
| 📦 **Упаковка** | Single-file self-contained |
| 🔐 **Сжатие** | LZMA2 |
| 📄 **Установщик** | Inno Setup 6 |
| ☕ **JRE** | Встроенная Java 8 (jre-8u491) |
| 🔗 **Native API** | Win32 (kernel32, ntdll, user32) |
| 🌐 **Языки** | Русский · Английский |
| 📜 **Лицензия** | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) |

---

## 🔒 Конфиденциальность

- 🛡 FMTool **не собирает** и **не передаёт** данные пользователей
- 🚫 **Не отправляет** хэши, файлы или логи на внешние серверы
- 💾 База сигнатур **локальная**, встроена в .exe
- 🧹 При удалении **не оставляет** следов

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