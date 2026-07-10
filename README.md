<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="140"></a>
</p>

<h1 align="center">🛡️ FMTool</h1>

<p align="center">
  <b>Десктопный античит-инструментарий для модераторов Minecraft-сервера <b>FunMoon</b></b>
  <br><br>
  Сканирует компьютер подозреваемого, находит читы и запрещённые моды
  <br>
  по сигнатурам, SHA-256, структуре классов, анализирует .jar/.exe/.dll,
  <br>
  запускает 12 форензик-утилит одним кликом.
  <br><br>
  <b>Бесплатно</b> · <b>Открытый исходный код</b> · <b>Лицензия MIT</b>
</p>

<br>

<!-- ═══════ БЕЙДЖИ: ТОЛЬКО ПОЛЕЗНОЕ ═══════ -->
<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/badge/%E2%AC%87%20%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C-%D0%9F%D0%BE%D1%81%D0%BB%D0%B5%D0%B4%D0%BD%D1%8F%D1%8F%20%D0%B2%D0%B5%D1%80%D1%81%D0%B8%D1%8F-28A745?style=for-the-badge&logo=github" alt="Скачать"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/badge/%F0%9F%93%A6-v1.0.3-3178C6?style=for-the-badge&logo=github" alt="v1.0.3"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/badge/%E2%AC%86%20%D0%92%D1%81%D0%B5%20%D1%80%D0%B5%D0%BB%D0%B8%D0%B7%D1%8B-5865F2?style=for-the-badge&logo=github" alt="Все релизы"></a>
  <br>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%92%BB%20Windows%2010%20%7C%2011-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows 10/11"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%94%AE%20.NET%208.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET 8"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE"><img src="https://img.shields.io/badge/%F0%9F%93%9C%20%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F-MIT-6B7280?style=for-the-badge&logo=open-source-initiative" alt="MIT"></a>
</p>

---

## 📌 О проекте

**FMTool** — это программа для Windows, которая помогает модераторам проверять игроков на читы.

> ⚠ **Это не мод для Майнкрафта.** Это десктопное приложение на C#, которое запускается на компьютере подозреваемого. Не требует установки на сервер или в игру.

---

## 🚀 Быстрый старт

| | |
|---|---|
| **1.** | Скачайте **FMTool-Installer.exe** из [последнего релиза](https://github.com/zxc1337funmoon-ops/FM/releases/latest) |
| **2.** | Запустите установщик, следуйте инструкциям |
| **3.** | Откройте FMTool — все инструменты уже внутри |

> 📌 Требуется Windows 10/11 (64-bit) и .NET 8 Runtime. Установщик скачает .NET автоматически, если его нет.

---

## ✨ Возможности

<table>
  <tr>
    <td width="50%" valign="top">

      ### 🔍 Сканер дисков
      Сканирует диск, Рабочий стол, Загрузки и любые папки. Ищет .jar, .exe, .dll по 500+ сигнатурам:
      - 📝 **Имя файла** — 150+ названий читов
      - 🔢 **Размер** — 130+ характерных JAR-размеров
      - 🔑 **SHA-256** — точный хэш из базы
      - 📂 **Папки** — %TEMP%, %APPDATA%, Downloads, Public
      - 💡 **Хэш-имена** — 32+ hex-символа, GUID

      Каждый файл получает **баллы подозрения**. Чем выше сумма — тем вероятнее чит.

    </td>
    <td width="50%" valign="top">

      ### 🧰 Mod Checker
      Перетащите .jar в окно или нажмите «Обзор». Программа:
      1. Распакует ZIP, проверит структуру
      2. Найдёт 70+ префиксов читов в путях пакетов
      3. Проанализирует MANIFEST.MF и mcmod.info
      4. Декомпилирует .class файлы (Luyten)
      5. Обнаружит обфускацию
      6. Сверит SHA-256 с базой

      Результат — вердикт с баллами и доказательствами.

    </td>
  </tr>
  <tr>
    <td valign="top">

      ### 📊 Журналы и логи
      История PowerShell, автозагрузка, запущенные процессы — без лазанья по системным папкам.

    </td>
    <td valign="top">

      ### 📋 Паттерны поиска
      6 готовых запросов для Everything: чит-паттерны, JAR-размеры, vec.dll, CSRSS. Копирование в буфер.

    </td>
  </tr>
  <tr>
    <td valign="top">

      ### 🎨 Персонализация
      7 тем с плавными анимациями: **Dark**, **Arctic**, **Emerald**, **Rose**, **Cherry**, **Gold**, **Violet**.  
      🌐 Русский и английский интерфейс — переключение в один клик.

    </td>
    <td valign="top">

      ### ⚙ Интеграция
      Трей, автозапуск, контекстное меню .jar в Проводнике, ассоциация форматов, горячие клавиши: <kbd>Ctrl+S</kbd> / <kbd>Ctrl+E</kbd> / <kbd>Ctrl+F</kbd>.

    </td>
  </tr>
</table>

---

## 🔧 Встроенные инструменты

<table>
  <tr>
    <th>#</th>
    <th>Инструмент</th>
    <th>Категория</th>
    <th>Назначение</th>
  </tr>
  <tr><td>1</td><td><b>Everything</b></td><td>🔍 Поиск</td><td>Мгновенный поиск файлов на всём диске</td></tr>
  <tr><td>2</td><td><b>HxD</b></td><td>🔬 Редактор</td><td>Просмотр и правка бинарных данных</td></tr>
  <tr><td>3</td><td><b>Luyten</b></td><td>☕ Декомпиляция</td><td>Исходный код из .jar / .class</td></tr>
  <tr><td>4</td><td><b>SystemInformer</b></td><td>💻 Мониторинг</td><td>Процессы, DLL-инъекции, сеть</td></tr>
  <tr><td>5</td><td><b>InjGen</b></td><td>📥 Детекция</td><td>Обнаружение DLL-инъекций</td></tr>
  <tr><td>6</td><td><b>CleaningDetector</b></td><td>🔊 Детекция</td><td>Поиск чистки цифровых следов</td></tr>
  <tr><td>7</td><td><b>JournalTrace</b></td><td>📃 Форензика</td><td>USN Journal — восстановление удалённых файлов</td></tr>
  <tr><td>8</td><td><b>ShellBag Analyzer</b></td><td>📁 Форензика</td><td>История посещённых папок</td></tr>
  <tr><td>9</td><td><b>USBDriveLog</b></td><td>⚡ Форензика</td><td>История подключения USB</td></tr>
  <tr><td>10</td><td><b>ExecutedProgramsList</b></td><td>📊 Форензика</td><td>Список запускавшихся программ</td></tr>
  <tr><td>11</td><td><b>PrefetchView++</b></td><td>🔏 Форензика</td><td>Хронология запуска (.pf)</td></tr>
  <tr><td>12</td><td><b>PathsParser</b></td><td>📍 Форензика</td><td>Извлечение путей из артефактов</td></tr>
</table>

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 Сканер — методология проверки</b></summary>
<br>

| Метод | Описание | Баллы |
|-------|----------|:-----:|
| 📝 Имя файла | 150+ известных названий читов | +30 |
| 🔢 Размер | 130+ характерных размеров JAR | +25 |
| 🔑 SHA-256 | Точное совпадение с базой | +99 |
| 🎯 Ключевые слова | 260+ слов в содержимом | +12 |
| 📂 Подозрительные папки | %TEMP%, %APPDATA%, Downloads, Public | +10 |
| 💡 Хэш-имена | 32+ hex-символа, GUID, случайные | +6–12 |

Каждый метод добавляет баллы. Файлы с суммой выше порога помечаются как подозрительные.
</details>

<details>
<summary><b>🛡 Белый список</b></summary>
<br>
Никогда не считаются читами:

- 🎮 **Ядра Minecraft**: minecraft, netty, lwjgl, authlib, lwjgl3
- ⚙ **Популярные моды**: optifine, sodium, iris, lithium, create, jei, rei, oculus
- 📡 **Библиотеки**: gson, fastutil, jackson, guava, log4j, slf4j, apache
- 🖥 **Системные файлы Windows**: ntdll, kernel32, shell32, user32, gdi32
- 📖 **Моды FunMoon**: fmutils, fm-utils, funmoon
</details>

<details>
<summary><b>🏴 PE-анализ (.exe / .dll)</b></summary>
<br>
Анализирует исполняемые файлы по 7 критериям:
- 📦 **Секции PE**: .upack, .themida, .vmp, .mpress — маркеры упаковщиков
- 🏴 **Импорты**: WriteProcessMemory, CreateRemoteThread — признаки инжекта
- 🕰 **Время компиляции**: нулевое или из будущего
- 💠 **Overlay**: данные, прикреплённые после PE-секций
- 🔐 **Цифровая подпись**: отсутствие = повод для проверки
- 🎮 **TLS-колбэки**: редкость в легитимных приложениях
- 🎯 **.NET обфускация**: Confuser, Obfuscar, .NET Reactor
</details>

---

## 💬 Поддержка и обратная связь

| | |
|---|---|
| 🐛 **Нашли баг?** | Создайте [новое обращение](https://github.com/zxc1337funmoon-ops/FM/issues/new) — подробно опишите проблему |
| 💡 **Есть идея?** | Предложите в [Issues](https://github.com/zxc1337funmoon-ops/FM/issues/new) — мы рассматриваем все идеи |
| ⭐ **Нравится проект?** | Поставьте звезду на GitHub — это помогает развитию |
| 📦 **Хотите собрать из исходников?** | Код открыт под [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) — форкайте и используйте |

---

## 📋 Версии

**v1.0.3** (текущая)
- 🖼 Иконки приложений извлекаются из самих .exe
- 🧰 Кнопки Save/Load в Mod Checker выровнены
- 💾 Пустой список модов больше не вызывает ошибок

**v1.0.2**
- 🐛 Удалены ссылки на несуществующий recaf-cli
- 📑 Удалены 17 дублирующихся ключей локализации
- 🗑 Очищены неиспользуемые темы

**v1.0.1** — Исправлены мелкие ошибки, улучшена стабильность

**v1.0.0**
- 🔍 Сканер · 🧰 Mod Checker · 🎨 7 тем · 🛡️ 12 утилит
- ⚙ Автообновление · ⌨ Горячие клавиши · 🌐 EN/RU

---

## 🛠 Технические детали

| | |
|---|---|
| 💻 Платформа | Windows 10 / 11, 64-bit |
| 🔮 Фреймворк | .NET 8.0, WPF, C# |
| 📦 Упаковка | Single-file self-contained, сжатие LZMA2 |
| 📄 Установщик | Inno Setup 6 |
| 📜 Лицензия | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) — можно свободно использовать и изменять |

---

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="48"></a>
  <br><br>
  <sub>Сделано с ❤️ для сообщества <b>FunMoon</b></sub>
  <br>
  <sub>© 2026 <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT License</a></sub>
  <br><br>
  <a href="https://github.com/zxc1337funmoon-ops/FM">GitHub</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/releases">Releases</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">Сообщить о баге</a>
</p>