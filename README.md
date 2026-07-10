<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="130"></a>
</p>

<h1 align="center">🛡️ FMTool</h1>

<p align="center">
  <b>Десктопный античит-инструментарий для модераторов Minecraft-сервера <b>FunMoon</b></b>
  <br><br>
  Сканирует компьютер подозреваемого, находит читы, анализирует .jar/.exe/.dll,
  <br>
  запускает 12 форензик-утилит. Бесплатно, с открытым кодом (MIT).
</p>

<br>

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/github/v/release/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&label=%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F&color=3178C6" alt="Версия"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/badge/%E2%AC%87%20%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C-%D0%A0%D0%B5%D0%BB%D0%B8%D0%B7-28A745?style=for-the-badge&logo=github" alt="Скачать"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new"><img src="https://img.shields.io/badge/%F0%9F%90%9B%20%D0%91%D0%B0%D0%B3-%D0%A1%D0%BE%D0%BE%D0%B1%D1%89%D0%B8%D1%82%D1%8C-D73A49?style=for-the-badge&logo=github" alt="Баг"></a>
  <br>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%92%BB%20%D0%9F%D0%BB%D0%B0%D1%82%D1%84%D0%BE%D1%80%D0%BC%D0%B0-Windows%2010%2F11-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows 10/11"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%94%AE%20.NET%208.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET 8"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE"><img src="https://img.shields.io/badge/%F0%9F%93%9C%20%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F-MIT-6B7280?style=for-the-badge&logo=open-source-initiative" alt="MIT"></a>
</p>

---

## ⬇ Скачать

Последнюю версию можно скачать на [странице релизов](https://github.com/zxc1337funmoon-ops/FM/releases).  
Установщик сам скачает .NET 8, если его нет.

---

## ✨ Возможности

### 🔍 Сканер дисков
Сканирует системный диск, Рабочий стол, Загрузки. Ищет .jar, .exe, .dll по 500+ сигнатурам: имя файла, размер, SHA-256, подозрительные папки, хэш-имена. Каждому файлу присваиваются баллы подозрения.

### 🧰 Mod Checker
Загрузите .jar через Drag&Drop. Программа распакует ZIP, проверит манифест, найдёт 70+ префиксов читов, декомпилирует классы и вынесет вердикт с баллами.

### 📊 Журналы и логи
История PowerShell, автозагрузка, запущенные процессы — всё в одном окне.

### 📋 Паттерны поиска
6 готовых запросов для Everything. Копирование в буфер одним кликом.

### 🎨 7 тем оформления
Dark, Arctic, Emerald, Rose, Cherry, Gold, Violet. Плавные анимации. Русский / English.

### ⚙ Интеграция
Трей, автозапуск, контекстное меню .jar, Ctrl+S / Ctrl+E / Ctrl+F.

---

## 🔧 Встроенные инструменты

Все утилиты внутри одного .exe — ничего качать не нужно.

| # | Инструмент | Категория | Назначение |
|---|------------|-----------|-----------|
| 1 | **Everything** | 🔍 Поиск | Мгновенный поиск файлов |
| 2 | **HxD** | 🔬 Редактор | Hex-редактор |
| 3 | **Luyten** | ☕ Декомпиляция | Java декомпилятор |
| 4 | **SystemInformer** | 💻 Мониторинг | Процессы, DLL-инъекции |
| 5 | **InjGen** | 📥 Детекция | DLL-инъекции |
| 6 | **CleaningDetector** | 🔊 Детекция | Чистка цифровых следов |
| 7 | **JournalTrace** | 📃 Форензика | USN Journal |
| 8 | **ShellBag Analyzer** | 📁 Форензика | История папок |
| 9 | **USBDriveLog** | ⚡ Форензика | USB-история |
| 10 | **ExecutedProgramsList** | 📊 Форензика | Запускавшиеся программы |
| 11 | **PrefetchView++** | 🔏 Форензика | .pf хронология |
| 12 | **PathsParser** | 📍 Форензика | Пути из артефактов |

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 Методология Сканера</b></summary>
<br>

| Метод | Описание | Баллы |
|-------|----------|:-----:|
| 📝 Имя файла | 150+ названий читов | +30 |
| 🔢 Размер | 130+ размеров JAR-читов | +25 |
| 🔑 SHA-256 | Точный хэш из базы | +99 |
| 🎯 Ключевые слова | 260+ слов в файлах | +12 |
| 📂 Папки | %TEMP%, %APPDATA%, Downloads | +10 |
| 💡 Хэш-имена | 32+ hex, GUID | +6–12 |

Чем выше сумма — тем вероятнее чит.
</details>

<details>
<summary><b>🛡 Белый список</b></summary>
<br>
Никогда не считаются читами:

- 🎮 **Ядра Minecraft**: minecraft, netty, lwjgl, authlib
- ⚙ **Моды**: optifine, sodium, iris, lithium, create, jei, rei
- 📡 **Библиотеки**: gson, fastutil, jackson, guava, log4j
- 🖥 **Системные**: ntdll, kernel32, shell32, user32
- 📖 **FunMoon**: fmutils, fm-utils
</details>

<details>
<summary><b>🏴 PE-анализ (.exe / .dll)</b></summary>
<br>
- 📦 Секции: .upack, .themida, .vmp, .mpress
- 🏴 Импорты: WriteProcessMemory, CreateRemoteThread
- 🕰 Время компиляции: нулевое или будущее
- 💠 Overlay-данные после секций
- 🔐 Отсутствие цифровой подписи
- 🎮 TLS-колбэки
- 🎯 .NET обфускация: Confuser, Obfuscar, .NET Reactor
</details>

---

## 🛠 Технические детали

| Параметр | Значение |
|----------|----------|
| 💻 **Платформа** | .NET 8 WPF, Windows 10/11 x64 |
| 📦 **Упаковка** | Single-file self-contained, LZMA2 |
| 📄 **Установщик** | Inno Setup 6 |
| 📜 **Лицензия** | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) |
| 🌐 **Языки** | Русский, Английский |

---

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="40"></a>
  <br><br>
  <sub>Сделано <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a> для сервера <b>FunMoon</b></sub>
  <br>
  <sub>© 2026 · <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT License</a></sub>
  <br><br>
  <a href="https://github.com/zxc1337funmoon-ops/FM">GitHub</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/releases">Releases</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">Баг</a>
</p>