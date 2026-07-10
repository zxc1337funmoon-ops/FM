# 🛡️ FMTool

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM">
    <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="256">
  </a>
  <br>
  <b><i>Anti-cheat forensic toolkit for Minecraft server moderators</i></b>
</p>

<br>

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/github/v/release/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&logoColor=white&label=%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F&color=%233178C6"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/github/downloads/zxc1337funmoon-ops/FM/total?style=for-the-badge&logo=github&logoColor=white&label=%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D0%BD%D0%BE&color=%2328A745"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/issues"><img src="https://img.shields.io/github/issues/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&logoColor=white&label=%D0%98%D1%81%D1%81%D1%8E%D1%81&color=%23D73A49"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/github/stars/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&logoColor=white&label=%D0%97%D0%B2ё%D0%B7%D0%B4%D1%8B&color=%23E6B91E"></a>
</p>

<br>

---

<br>

<p align="center">
  <b>FMTool</b> — это криминалистический инструмент для модераторов Minecraft-сервера <b>FunMoon</b>.
  <br>
  Сканирует ПК подозреваемого, находит читы по имени/размеру/SHA-256/структуре классов,
  <br>
  анализирует .jar/память/журналы через встроенные форензик-утилиты.
  <br><br>
  <b>Полностью бесплатен. Открытый исходный код (MIT).</b>
</p>

<br>

---

<br>

<h2 align="center">
  📷 Интерфейс
</h2>

<p align="center">
  <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Square.jpg" alt="FMTool Interface" width="820">
<br>
  <i>Главное окно приложения — навигация, результаты сканирования, встроенные утилиты</i>
</p>

<br>

---

<br>

<h2 align="center">
  ✨ Возможности
</h2>

<table>
  <tr>
    <td width="50%" valign="top">

      🔍   <b>Модуль сканирования</b>
      <br>
      Сканирует C:\, Рабочий стол, Загрузки и любые папки.
      Ищет .jar, .exe, .dll по сигнатурам читов,
      классифицирует по уровню подозрения.
      Поддерживает глубокий анализ содержимого.

    </td>
    <td width="50%" valign="top">

      🧰   <b>Mod Checker</b>
      <br>
      Загрузка .jar-файлов через Drag&Drop.
      Эвристический анализ по 500+ сигнатурам,
      проверка SHA-256, декомпиляция классов через Luyten.

    </td>
  </tr>
  <tr>
    <td width="50%" valign="top">

      📊   <b>Системные журналы</b>
      <br>
      Просмотр PowerShell истории,
      списка автозапускающихся программ.
      Вся информация в одном окне.

    </td>
    <td width="50%" valign="top">

      📋   <b>Паттерны поиска</b>
      <br>
      6 готовых поисковых запросов для Everything.
      Чит-паттерны, размеры JAR, проверка vec.dll, CSRSS и др.

    </td>
  </tr>
  <tr>
    <td width="50%" valign="top">

      🎨   <b>Персонализация</b>
      <br>
      7 тем оформления с плавными анимациями.
      2 языка (Русский / English).
      Переключение в один клик.

    </td>
    <td width="50%" valign="top">

      ⚙️   <b>Системная интеграция</b>
      <br>
      Трей, автозапуск с Windows,
      контекстное меню для .jar,
      горячие клавиши (Ctrl+S/E/F).

    </td>
  </tr>
</table>

<br>

---

<br>

<h2 align="center">
  🛠️ Встроенные инструменты
</h2>

<p align="center">
  <sup>Все утилиты хранятся внутри одного .exe файла и извлекаются автоматически при запуске.</sup>
</p>

<table>
  <tr>
    <th align="center">Иконка</th>
    <th align="left">Инструмент</th>
    <th align="left">Категория</th>
    <th align="left">Описание</th>
  </tr>
  <tr>
    <td align="center">🔍</td>
    <td><b>Everything</b></td>
    <td>Поиск</td>
    <td>Мгновенный поиск файлов по имени/размеру/типу</td>
  </tr>
  <tr>
    <td align="center">🔬</td>
    <td><b>HxD</b></td>
    <td>Редактирование</td>
    <td>Hex-редактор для анализа бинарных данных</td>
  </tr>
  <tr>
    <td align="center">☕</td>
    <td><b>Luyten</b></td>
    <td>Декомпиляция</td>
    <td>Java-декомпилятор (просмотр .jar/.class)</td>
  </tr>
  <tr>
    <td align="center">💻</td>
    <td><b>SystemInformer</b></td>
    <td>Мониторинг</td>
    <td>Монитор процессов и DLL-инъекций</td>
  </tr>
  <tr>
    <td align="center">📥</td>
    <td><b>InjGen</b></td>
    <td>Детекция</td>
    <td>Детект DLL-инъекций в процессы</td>
  </tr>
  <tr>
    <td align="center">🔊</td>
    <td><b>CleaningDetector</b></td>
    <td>Детекция</td>
    <td>Обнаруживает чистку цифровых следов</td>
  </tr>
  <tr>
    <td align="center">📃</td>
    <td><b>JournalTrace</b></td>
    <td>Форензика</td>
    <td>USN Journal — восстановление удалённых файлов</td>
  </tr>
  <tr>
    <td align="center">📁</td>
    <td><b>ShellBag Analyzer</b></td>
    <td>Форензика</td>
    <td>ShellBags — история посещённых папок</td>
  </tr>
  <tr>
    <td align="center">⚡</td>
    <td><b>USBDriveLog</b></td>
    <td>Форензика</td>
    <td>История подключенных USB-накопителей</td>
  </tr>
  <tr>
    <td align="center">📊</td>
    <td><b>ExecutedProgramsList</b></td>
    <td>Форензика</td>
    <td>Список всех запускавшихся программ</td>
  </tr>
  <tr>
    <td align="center">🔏</td>
    <td><b>PrefetchView++</b></td>
    <td>Форензика</td>
    <td>Хронология запуска приложений (Prefetch)</td>
  </tr>
  <tr>
    <td align="center">📍</td>
    <td><b>PathsParser</b></td>
    <td>Форензика</td>
    <td>Парсинг путей из системных артефактов</td>
  </tr>
</table>

<br>

---

<br>

<h2 align="center">
  🧬 Движок детекции
</h2>

<details>
<summary><b>🔍 Что ищет Сканер</b></summary>
<br>

<table>
  <tr>
    <th>Метод</th>
    <th>Описание</th>
    <th>Вес</th>
  </tr>
  <tr>
    <td>📝 Имя файла</td>
    <td>Совпадение с 150+ известными названиями читов</td>
    <td align="center">+30</td>
  </tr>
  <tr>
    <td>🔢 Размер файла</td>
    <td>Совпадение с 130+ известными размерами JAR-читов</td>
    <td align="center">+25</td>
  </tr>
  <tr>
    <td>🔑 SHA-256 хэш</td>
    <td>Точное совпадение с базой известных читов</td>
    <td align="center">+99</td>
  </tr>
  <tr>
    <td>🎯 Ключевые слова</td>
    <td>260+ ключевых слов (чит-модули, пакеты, термины)</td>
    <td align="center">+12</td>
  </tr>
  <tr>
    <td>📍 Подозрительные папки</td>
    <td>Файлы в %TEMP%, %APPDATA%, \Downloads, \Public</td>
    <td align="center">+10</td>
  </tr>
  <tr>
    <td>💡 Хеш-подобные имена</td>
    <td>32+ hex-символов, GUID, случайные имена</td>
    <td align="center">+6–12</td>
  </tr>
  <tr>
    <td>🌐 Локация</td>
    <td>Файлы в нестандартных директориях</td>
    <td align="center">+4–8</td>
  </tr>
</table>

</details>

<details>
<summary><b>🧰 Как работает Mod Checker</b></summary>
<br>

При анализе .jar-файла происходят следующие проверки:

1. 📂 Распаковка ZIP-архива и анализ структуры
2. 📄 Проверка MANIFEST.MF на main-class
3. 📋 Анализ mcmod.info / fabric.mod.json / mods.toml
4. 🔍 Поиск известных путей пакетов (более 70 префиксов)
5. 🧮 Декомпиляция .class файлов и поиск строк
6. 🎯 Подсчёт совпадений с 260+ ключевыми словами
7. 🧠 Обнаружение обфускации (короткие имена, энтропия)
8. 📏 Проверка на вложенные JAR-архивы
9. 🔢 Сравнение SHA-256 с базой известных читов

Результат: подробный отчёт с баллами, вердиктом и деталями.

</details>

<details>
<summary><b>🏴 PE-анализ (для .exe/.dll)</b></summary>
<br>

При глубоком сканировании анализируются:

- 📦 Секции PE (подозрительные имена: .upack, .themida, .vmp, .mpress и др.)
- 🏴 Import-таблицы (WriteProcessMemory, CreateRemoteThread, NtQueryInformationProcess)
- 🕰️ Время компиляции (нулевое / будущее)
- 💠 Overlay-данные (прикреплённые данные после последней секции)
- 🔐 Цифровая подпись (отсутствие подписи = подозрительно)
- 🎮 TLS-колбэки (редко в легитимных приложениях)
- 🎯 .NET обфускация (Confuser, Obfuscar, .NET Reactor и др.)

</details>

<details>
<summary><b>🛡️ Что исключается из проверки</b></summary>
<br>

Белый список (никогда не считается читом):
- 🎮 Ядра Minecraft: minecraft, netty, lwjgl, authlib, fabric-api, forge
- ⚙️ Модификации: optifine, sodium, iris, lithium, create, jei, rei, emi
- 📡 Библиотеки: gson, fastutil, jackson, guava, log4j, slf4j
- 📽️ Системные файлы Windows: ntdll, kernel32, user32, shell32, ole32 и т.д.
- 📖 Модификации FunMoon: fmutils, fm-utils, fm_utils

</details>

<br>

---

<br>

<h2 align="center">
  🎉 Дорожная карта
</h2>

<table>
  <tr>
    <th align="center">Статус</th>
    <th align="left">Что сделано / планируется</th>
  </tr>
  <tr>
    <td align="center">✅</td>
    <td>Сканер дисков с классификацией по уровню подозрения</td>
  </tr>
  <tr>
    <td align="center">✅</td>
    <td>Mod Checker с эвристическим анализом</td>
  </tr>
  <tr>
    <td align="center">✅</td>
    <td>12 встроенных форензик-утилит</td>
  </tr>
  <tr>
    <td align="center">✅</td>
    <td>7 тем оформления, 2 языка</td>
  </tr>
  <tr>
    <td align="center">✅</td>
    <td>Tray-режим, автозапуск, горячие клавиши</td>
  </tr>
  <tr>
    <td align="center">✅</td>
    <td>Контекстное меню для .jar в Проводнике</td>
  </tr>
  <tr>
    <td align="center">🎯</td>
    <td>Онлайн-проверка по базе сигнатур через API</td>
  </tr>
  <tr>
    <td align="center">🎯</td>
    <td>Drag-and-drop папок в Mod Checker</td>
  </tr>
  <tr>
    <td align="center">🎯</td>
    <td>Экспорт логов в формате HTML/PDF</td>
  </tr>
  <tr>
    <td align="center">🎯</td>
    <td>Плагины и кастомные правила сканирования</td>
  </tr>
</table>

<br>

---

<br>

<h2 align="center">
  📱 Установка
</h2>

<table align="center">
  <tr>
    <td align="center" width="60">🛡️</td>
    <td><b>Шаг 1</b></td>
    <td>Скачайте <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><code>FMTool-Installer.exe</code></a> из раздела Releases</td>
  </tr>
  <tr>
    <td align="center" width="60">⚡</td>
    <td><b>Шаг 2</b></td>
    <td>Запустите установщик и следуйте инструкциям</td>
  </tr>
  <tr>
    <td align="center" width="60">🚀</td>
    <td><b>Шаг 3</b></td>
    <td>Запустите ярлык FMTool на рабочем столе</td>
  </tr>
  <tr>
    <td align="center" width="60">🤖</td>
    <td><b>Шаг 4</b></td>
    <td>Пользуйтесь! Все инструменты уже внутри</td>
  </tr>
</table>

<br>

<blockquote align="center">
  🚧 <b>Требования:</b> Windows 10/11 64-bit, .NET 8.0 Runtime
  <br>
  Установщик предложит скачать .NET при необходимости.
</blockquote>

<br>

---

<br>

<h2 align="center">
  ❓ FAQ
</h2>

<details>
<summary><b>💡 Что такое FMTool? Это мод для Майнкрафта?</b></summary>
<br>
Нет. FMTool — это <b>десктопное Windows-приложение</b> (C#, .NET 8.0, WPF). Оно запускается на ПК подозреваемого и помогает модераторам найти читы. Это не мод и не плагин для игры.
</details>

<details>
<summary><b>🔍 Как проверить .jar файл?</b></summary>
<br>
Перетащите .jar в окно Mod Checker или нажмите «Обзор». Можно также нажать правой кнопкой мыши на .jar в Проводнике и выбрать «Scan with FMTool».
</details>

<details>
<summary><b>🔄 Что делать с результатами скана?</b></summary>
<br>
Каждый результат можно: открыть в Проводнике, скопировать путь или SHA-256, отправить в Mod Checker, открыть подробный отчёт. Результаты можно экспортировать в файл.
</details>

<details>
<summary><b>📦 Як пополнить базу сигнатур?</b></summary>
<br>
База сигнатур встроена напрямую в код приложения. Она обновляется с новыми релизами. В планах — онлайн-проверка через API.
</details>

<details>
<summary><b>🐛 Нашёл баг? Есть идея?</b></summary>
<br>
Создайте <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">новую заметку</a> в разделе Issues. Подробно опишите проблему. Мы читаем все обращения.
</details>

<br>

---

<br>

<h2 align="center">
  📝 История версий
</h2>

<h3>🗑️ v1.0.3</h3>
<ul>
  <li>🖼 Иконки программ теперь извлекаются из самих .exe файлов</li>
  <li>🧰 Кнопки Save/Load в Mod Checker выровнены</li>
  <li>💾 Пустой список модов корректно сохраняется и загружается</li>
</ul>

<h3>🗑️ v1.0.2</h3>
<ul>
  <li>🐛 Удалены ссылки на отсутствующий recaf-cli-0.8.8.jar</li>
  <li>📑 Удалены 17 дублирующихся ключей локализации</li>
  <li>🗑 Удалены неиспользуемые файлы тем</li>
</ul>

<h3>🗑️ v1.0.0 <sup>— первый релиз</sup></h3>
<ul>
  <li>🔍 Сканер дисков с параллельным сканированием в 4 потока</li>
  <li>🧰 Mod Checker с группировкой по уровню подозрения</li>
  <li>🎨 7 тем оформления (Dark, Arctic, Emerald, Rose, Cherry, Gold, Violet)</li>
  <li>🛡️ 12 встроенных утилит для форензики</li>
  <li>⚙️ Автообновление, автозапуск, системная интеграция</li>
  <li>⌨️ Ctrl+S (скан), Ctrl+E (экспорт), Ctrl+F (поиск)</li>
</ul>

<br>

---

<br>

<h2 align="center">
  ❤️ Для кого это
</h2>

<table align="center">
  <tr>
    <td align="center" width="100">🛡️</td>
    <td><b>Модераторов</b> сервера FunMoon</td>
    <td>Проверка игроков на наличие читов и запрещённых модов</td>
  </tr>
  <tr>
    <td align="center" width="100">💻</td>
    <td><b>Администраторов</b> Minecraft-серверов</td>
    <td>Форензика системы, анализ журналов, поиск подозрительной активности</td>
  </tr>
  <tr>
    <td align="center" width="100">🔍</td>
    <td><b>Исследователей</b> и безопасников</td>
    <td>Анализ .jar-файлов, декомпиляция, HEX-редактирование</td>
  </tr>
</table>

<br>

---

<br>

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM">
    <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="96">
  </a>
  <br><br>
  <sub>Сделано с любовью для сервера <b>FunMoon</b> ✨</sub>
  <br>
  <sub>© 2026 <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a> / FunMoon &mdash; Распространяется под лицензией <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT</a></sub>
</p>
