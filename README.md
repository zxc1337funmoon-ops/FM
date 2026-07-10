<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="130"></a>
</p>

<h1 align="center">🛡️ FMTool</h1>

<p align="center">
  <i>Криминалистический античит-инструментарий для модераторов Minecraft-сервера <b>FunMoon</b></i>
  <br><br>
  Сканирует ПК подозреваемого, находит читы по 500+ сигнатурам,
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

## ⬇️ Скачать

| Файл | Описание |
|------|----------|
| **FMTool-Installer.exe** | Установщик (Inno Setup) — скачает .NET 8, если нужно |
| **FMTool.exe** | Portable версия — запускается без установки |

Скачать: [последний релиз](https://github.com/zxc1337funmoon-ops/FM/releases/latest) · [все релизы](https://github.com/zxc1337funmoon-ops/FM/releases)

**Требования:** Windows 10 или 11 (64-bit) · .NET 8 Runtime

---

## ✨ Возможности

### 🔍 Mod Checker
Загрузите .jar-файл через Drag&Drop или кнопку «Обзор». Программа распакует ZIP, проанализирует структуру, манифест и мета-файлы модов, найдёт 70+ префиксов читов в путях пакетов, декомпилирует .class файлы и вынесет вердикт с баллами.

**Что проверяет:** 500+ сигнатур, SHA-256, обфускацию, вложенные JAR, 260+ ключевых слов.

### 📂 Сканер дисков
Сканирует системный диск, Рабочий стол, Загрузки. Ищет .jar, .exe, .dll по 150+ известным названиям читов, 130+ характерным размерам, SHA-256 хэшам, подозрительным папкам (%TEMP%, %APPDATA%).

**Результат:** каждый файл получает баллы подозрения. Чем выше — тем вероятнее чит.

### 📄 Отчёты
Детальный отчёт с цветовой маркировкой. Четыре вкладки: Детект, Классы, Структура, Метаданные. Экспорт в TXT и JSON.

### 🚀 12 встроенных инструментов
| Инструмент | Назначение |
|------------|-----------|
| **Everything** | Мгновенный поиск файлов |
| **HxD** | Hex-редактор |
| **Luyten** | Java-декомпилятор |
| **SystemInformer** | Мониторинг процессов |
| **InjGen** | Детектор DLL-инъекций |
| **CleaningDetector** | Поиск чистки следов |
| **JournalTrace** | USN Journal |
| **ShellBag Analyzer** | История папок |
| **USBDriveLog** | USB-история |
| **ExecutedProgramsList** | Запускавшиеся программы |
| **PrefetchView++** | Хронология запуска |
| **PathsParser** | Пути из артефактов |

### 🎨 Интерфейс
7 тем: **Dark** · **Arctic** · **Emerald** · **Rose** · **Cherry** · **Gold** · **Violet**  
Плавные анимации, закруглённые углы. Русский и английский язык.

### ⚙️ Интеграция
Трей, автозапуск, контекстное меню .jar в Проводнике, ассоциация файлов.  
⌨ **Ctrl+S** / **Ctrl+E** / **Ctrl+F**

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 Сканер — методология</b></summary>
<br>

| Метод | Описание | Баллы |
|-------|----------|:-----:|
| 📝 Имя файла | 150+ названий читов | +30 |
| 🔢 Размер файла | 130+ размеров JAR-читов | +25 |
| 🔑 SHA-256 | Точный хэш из базы | +99 |
| 🎯 Ключевые слова | 260+ слов в файлах | +12 |
| 📂 Папки | %TEMP%, %APPDATA%, Downloads | +10 |
| 💡 Хэш-имена | 32+ hex, GUID | +6–12 |

</details>

<details>
<summary><b>🛡️ Белый список</b></summary>
<br>
Никогда не считаются читами:
- 🎮 **Ядра Minecraft:** minecraft, netty, lwjgl, authlib
- ⚙ **Моды:** optifine, sodium, iris, lithium, create, jei, rei
- 📡 **Библиотеки:** gson, fastutil, jackson, guava, log4j
- 🖥 **Системные:** ntdll, kernel32, shell32, user32
- 📖 **FunMoon:** fmutils, fm-utils
</details>

<details>
<summary><b>🏴 PE-анализ (.exe / .dll)</b></summary>
<br>
- 📦 Секции: .upack, .themida, .vmp, .mpress
- 🏴 Импорты: WriteProcessMemory, CreateRemoteThread
- 🕰 Время компиляции: нулевое или из будущего
- 💠 Overlay-данные после секций
- 🔐 Отсутствие цифровой подписи
- 🎮 TLS-колбэки
- 🎯 .NET обфускация: Confuser, Obfuscar, .NET Reactor
</details>

---

## 🛠 Технические детали

| Параметр | Значение |
|----------|----------|
| 💻 Платформа | .NET 8 WPF, Windows 10/11 x64 |
| 📦 Упаковка | Single-file self-contained, LZMA2 |
| 📄 Установщик | Inno Setup 6 |
| 📜 Лицензия | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) |
| 🌐 Языки | Русский, Английский |

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
  <a href="https://github.com/zxc1337funmoon-ops/FM">GitHub</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/releases">Releases</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">Баг</a>
</p>