<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="140"></a>
</p>

<h1 align="center">🛡️ FMTool</h1>

<p align="center">
  <b>Криминалистический античит-инструментарий для модераторов Minecraft-сервера <b>FunMoon</b></b>
  <br><br>
  Сканирует компьютер подозреваемого, находит читы по 500+ сигнатурам,
  <br>
  анализирует .jar / .exe / .dll, запускает 12 форензик-утилит.
  <br><br>
  <b>Бесплатно</b> · <b>Открытый исходный код</b> · <b>Лицензия MIT</b>
</p>

<br>

<p align="center">
  <!-- ════ ДЕЙСТВИЯ ════ -->
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/github/v/release/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&label=%F0%9F%93%A6%20%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F&color=3178C6" alt="Версия"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/badge/%E2%AC%87%20%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C-%D0%A0%D0%B5%D0%BB%D0%B8%D0%B7-28A745?style=for-the-badge&logo=github" alt="Скачать"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new"><img src="https://img.shields.io/badge/%F0%9F%90%9B%20%D0%91%D0%B0%D0%B3-%D0%A1%D0%BE%D0%BE%D0%B1%D1%89%D0%B8%D1%82%D1%8C-D73A49?style=for-the-badge&logo=github" alt="Баг"></a>
  <br>
  <!-- ════ ПЛАТФОРМА ════ -->
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%92%BB%20%D0%9F%D0%BB%D0%B0%D1%82%D1%84%D0%BE%D1%80%D0%BC%D0%B0-Windows%2010%20%7C%2011-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows 10/11"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%94%AE%20.NET-8.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET 8"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE"><img src="https://img.shields.io/badge/%F0%9F%93%9C%20%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F-MIT-6B7280?style=for-the-badge&logo=open-source-initiative" alt="MIT"></a>
</p>

---

## 📌 О проекте

**FMTool** — это программа для Windows, созданная для модераторов Minecraft-сервера **FunMoon**. Она устанавливается на компьютер подозреваемого, сканирует его и помогает быстро найти читы, запрещённые моды и следы нечестной игры.

> ⚠️ **Это не мод для Майнкрафта.** Это десктопное приложение на C# / WPF. Оно не требует установки на сервер или в игру.

---

## ⬇️ Скачать

| Шаг | Действие |
|-----|----------|
| **1** | Скачайте **[FMTool-Installer.exe](https://github.com/zxc1337funmoon-ops/FM/releases/latest)** из последнего релиза |
| **2** | Запустите установщик — он сам проверит и скачает .NET 8, если нужно |
| **3** | Готово! Ярлык FMTool на рабочем столе — все инструменты уже внутри |

**Требования:** Windows 10 / 11, 64-bit · .NET 8.0 Runtime (будет установлен автоматически)

---

## ✨ Возможности

<table>
  <tr>
    <td width="50%" valign="top">
      <h3>🔍 Сканер дисков</h3>
      Сканирует системный диск, Рабочий стол, Загрузки и любые папки. Ищет .jar, .exe, .dll по 500+ сигнатурам:
      <br><br>
      📝 <b>Имя файла</b> — 150+ названий читов<br>
      🔢 <b>Размер</b> — 130+ характерных JAR-размеров<br>
      🔑 <b>SHA-256</b> — точный хэш из базы<br>
      📂 <b>Папки</b> — %TEMP%, %APPDATA%, Downloads<br>
      💡 <b>Хэш-имена</b> — 32+ hex-символа, GUID
    </td>
    <td width="50%" valign="top">
      <h3>🧰 Mod Checker</h3>
      Загрузите .jar через Drag&Drop или кнопку «Обзор»:
      <br><br>
      1. Распаковка ZIP и анализ структуры<br>
      2. Проверка MANIFEST.MF, fabric.mod.json<br>
      3. 70+ префиксов читов в путях пакетов<br>
      4. Декомпиляция .class файлов (Luyten)<br>
      5. Обнаружение обфускации<br>
      6. Сверка SHA-256 с базой
    </td>
  </tr>
  <tr>
    <td valign="top">
      <h3>📊 Журналы и логи</h3>
      История PowerShell, автозагрузка, запущенные процессы — вся системная активность в одном окне. Без лазанья по дебрям ОС.
    </td>
    <td valign="top">
      <h3>📋 Паттерны поиска</h3>
      6 готовых запросов для Everything: чит-паттерны, размеры JAR, vec.dll, CSRSS. Копирование в буфер одним кликом.
    </td>
  </tr>
  <tr>
    <td valign="top">
      <h3>🎨 Персонализация</h3>
      7 тем с плавными анимациями: <b>Dark</b> · <b>Arctic</b> · <b>Emerald</b> · <b>Rose</b> · <b>Cherry</b> · <b>Gold</b> · <b>Violet</b>
      <br><br>
      🌐 Два языка: <b>Русский</b> и <b>English</b> — переключение в один клик.
    </td>
    <td valign="top">
      <h3>⚙️ Интеграция</h3>
      Трей (область уведомлений), автозапуск с Windows, контекстное меню для .jar в Проводнике, ассоциация форматов.
      <br><br>
      ⌨ Горячие клавиши: <kbd>Ctrl+S</kbd> · <kbd>Ctrl+E</kbd> · <kbd>Ctrl+F</kbd>
    </td>
  </tr>
</table>

---

## 🔧 12 встроенных инструментов

<table>
  <tr>
    <th>#</th>
    <th>Инструмент</th>
    <th>Категория</th>
    <th>Назначение</th>
  </tr>
  <tr><td>1</td><td><b>Everything</b></td><td>🔍 Поиск</td><td>Мгновенный поиск файлов на всём диске</td></tr>
  <tr><td>2</td><td><b>HxD</b></td><td>🔬 Редактор</td><td>Hex-редактор для бинарных данных</td></tr>
  <tr><td>3</td><td><b>Luyten</b></td><td>☕ Декомпиляция</td><td>Java-декомпилятор (.jar / .class)</td></tr>
  <tr><td>4</td><td><b>SystemInformer</b></td><td>💻 Мониторинг</td><td>Процессы, DLL-инъекции, сетевая активность</td></tr>
  <tr><td>5</td><td><b>InjGen</b></td><td>📥 Детекция</td><td>Обнаружение DLL-инъекций</td></tr>
  <tr><td>6</td><td><b>CleaningDetector</b></td><td>🔊 Детекция</td><td>Поиск попыток чистки цифровых следов</td></tr>
  <tr><td>7</td><td><b>JournalTrace</b></td><td>📃 Форензика</td><td>USN Journal — восстановление удалённых файлов</td></tr>
  <tr><td>8</td><td><b>ShellBag Analyzer</b></td><td>📁 Форензика</td><td>История посещённых папок (ShellBags)</td></tr>
  <tr><td>9</td><td><b>USBDriveLog</b></td><td>⚡ Форензика</td><td>История подключения USB-накопителей</td></tr>
  <tr><td>10</td><td><b>ExecutedProgramsList</b></td><td>📊 Форензика</td><td>Список всех запускавшихся программ</td></tr>
  <tr><td>11</td><td><b>PrefetchView++</b></td><td>🔏 Форензика</td><td>Хронология запуска (.pf файлы)</td></tr>
  <tr><td>12</td><td><b>PathsParser</b></td><td>📍 Форензика</td><td>Извлечение путей из системных артефактов</td></tr>
</table>

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 Сканер — методология проверки</b></summary>
<br>
Каждому файлу присваиваются баллы. Чем выше сумма — тем выше вероятность чита.

| Метод | Описание | Баллы |
|-------|----------|:-----:|
| 📝 Имя файла | 150+ известных названий читов | +30 |
| 🔢 Размер файла | 130+ характерных размеров JAR | +25 |
| 🔑 SHA-256 | Точное совпадение с базой | +99 |
| 🎯 Ключевые слова | 260+ слов в содержимом | +12 |
| 📂 Подозрительные папки | %TEMP%, %APPDATA%, Downloads | +10 |
| 💡 Хэш-имена | 32+ hex-символа, GUID, случайные | +6–12 |
</details>

<details>
<summary><b>🛡️ Белый список</b></summary>
<br>
Никогда не считаются читами:
- 🎮 **Ядра Minecraft:** minecraft, netty, lwjgl, authlib, lwjgl3
- ⚙ **Моды:** optifine, sodium, iris, lithium, create, jei, rei, oculus
- 📡 **Библиотеки:** gson, fastutil, jackson, guava, log4j, slf4j
- 🖥 **Системные:** ntdll, kernel32, shell32, user32, gdi32
- 📖 **FunMoon:** fmutils, fm-utils, funmoon
</details>

<details>
<summary><b>🏴 PE-анализ (.exe / .dll)</b></summary>
<br>
- 📦 **Секции PE:** .upack, .themida, .vmp, .mpress — признаки упаковщиков
- 🏴 **Импорты:** WriteProcessMemory, CreateRemoteThread — признаки инжекта
- 🕰 **Время компиляции:** нулевое или из будущего
- 💠 **Overlay:** данные, прикреплённые после PE-секций
- 🔐 **Цифровая подпись:** отсутствие = повод для проверки
- 🎮 **TLS-колбэки:** редко встречаются в легитимных приложениях
- 🎯 **.NET обфускация:** Confuser, Obfuscar, .NET Reactor
</details>

---

## 🛠 Технические детали

| Параметр | Значение |
|----------|----------|
| 💻 Платформа | .NET 8 WPF, Windows 10 / 11 x64 |
| 📦 Упаковка | Single-file self-contained, LZMA2 |
| 📄 Установщик | Inno Setup 6 |
| 📜 Лицензия | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) |
| 🌐 Языки | Русский, Английский |

---

## 💬 Поддержка

| | |
|---|---|
| 🐛 **Нашли баг?** | Создайте [обращение](https://github.com/zxc1337funmoon-ops/FM/issues/new) — подробно опишите проблему |
| 💡 **Есть идея?** | Предложите в [Issues](https://github.com/zxc1337funmoon-ops/FM/issues/new) — мы рассматриваем всё |
| ⭐ **Нравится проект?** | Поставьте звезду на GitHub — это поможет развитию |
| 📦 **Хотите собрать сами?** | Код открыт под [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) — форкайте и используйте |

---

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="48"></a>
  <br><br>
  <sub>Сделано <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a> для сообщества <b>FunMoon</b> ❤️</sub>
  <br>
  <sub>© 2026 · <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT License</a></sub>
  <br><br>
  <a href="https://github.com/zxc1337funmoon-ops/FM">GitHub</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/releases">Releases</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">Сообщить о баге</a>
</p>