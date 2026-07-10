<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="180"></a>
</p>

<h1 align="center">🛡️ FMTool</h1>

<p align="center">
  <i>Криминалистический античит-инструментарий для модераторов Minecraft-сервера <b>FunMoon</b></i>
  <br>
  Сканирует ПК подозреваемого, находит читы по имени/размеру/SHA-256/структуре классов,
  <br>
  анализирует .jar/память/журналы через встроенные форензик-утилиты.
  <br><br>
  <b>Полностью бесплатно ✨ Открытый исходный код (MIT)</b>
</p>

<br>

<!-- ==================== BADGES ==================== -->
<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/github/v/release/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&logoColor=white&label=%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F&color=3178C6" alt="Version"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/github/downloads/zxc1337funmoon-ops/FM/total?style=for-the-badge&logo=github&logoColor=white&label=%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D0%BD%D0%BE&color=28A745" alt="Downloads"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/issues"><img src="https://img.shields.io/github/issues/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&logoColor=white&label=%D0%98%D1%81%D1%81%D1%8E%D1%81%D1%8B&color=D73A49" alt="Issues"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/github/stars/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&logoColor=white&label=%D0%97%D0%B2ё%D0%B7%D0%B4%D1%8B&color=E6B91E" alt="Stars"></a>
  <br>
  <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE"><img src="https://img.shields.io/badge/%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F-MIT-7B1FA2?style=for-the-badge&logo=open-source-initiative&logoColor=white" alt="License"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/github/repo-size/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&logoColor=white&label=%D0%A0%D0%B0%D0%B7%D0%BC%D0%B5%D1%80&color=FF6600" alt="Repo size"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/github/last-commit/zxc1337funmoon-ops/FM?style=for-the-badge&logo=github&logoColor=white&label=%D0%9E%D0%B1%D0%BD%D0%BE%D0%B2%D0%BB%D0%B5%D0%BD%D0%BE&color=1DA1F2" alt="Last commit"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new"><img src="https://img.shields.io/badge/%D0%A1%D0%BE%D0%BE%D0%B1%D1%89%D0%B8%D1%82%D1%8C%20%D0%BE%20%D0%B1%D0%B0%D0%B3%D0%B5-red?style=for-the-badge&logo=github&logoColor=white" alt="Report bug"></a>
</p>

---

<!-- ==================== SCREENSHOT ==================== -->
<h2 align="center">📷 Интерфейс</h2>

<p align="center">
  <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Square.jpg" alt="FMTool Screenshot" width="820">
  <br>
  <i>Главное окно — навигация, результаты сканирования, встроенные утилиты. 7 тем оформления.</i>
</p>

---

<!-- ==================== FEATURES ==================== -->
<h2 align="center">✨ Возможности</h2>

<table>
  <tr>
    <td width="50%" valign="top">

      <h3>🔍 Сканер дисков</h3>
      Сканирует C:\, Рабочий стол, Загрузки и любые папки. Ищет .jar, .exe, .dll по сигнатурам читов, классифицирует по уровню подозрения. Глубокий анализ содержимого.
      <br><br>
      <b>Метрики:</b>
      <br>📝 Имя файла — 150+ названий читов
      <br>🔢 Размер — 130+ известных размеров JAR
      <br>🔑 SHA-256 — точное совпадение с базой
      <br>📂 Подозрительные папки — %TEMP%, %APPDATA%, Downloads

    </td>
    <td width="50%" valign="top">

      <h3>🧰 Mod Checker</h3>
      Загрузка .jar через Drag&Drop или кнопку. Эвристический анализ по 500+ сигнатурам, проверка SHA-256, декомпиляция классов через Luyten.
      <br><br>
      <b>Анализирует:</b>
      <br>📂 Структуру ZIP-архива
      <br>📄 Манифест (MANIFEST.MF) и метаданные
      <br>🔍 Пути пакетов (70+ префиксов читов)
      <br>🧎 Декомпиляцию .class файлов

    </td>
  </tr>
  <tr>
    <td width="50%" valign="top">

      <h3>📊 Журналы и логи</h3>
      Просмотр истории PowerShell, списка автозапускающихся программ. Вся информация в одном окне — не нужно лазить по системным папкам.

    </td>
    <td width="50%" valign="top">

      <h3>📋 Паттерны поиска</h3>
      6 готовых поисковых запросов для Everything. Чит-паттерны, размеры JAR, проверка vec.dll, CSRSS и другие. Копирование в буфер одним кликом.

    </td>
  </tr>
  <tr>
    <td width="50%" valign="top">

      <h3>🎨 Персонализация</h3>
      7 тем оформления с плавными анимациями:
      <br><b>Dark</b> · <b>Arctic</b> · <b>Emerald</b> · <b>Rose</b> · <b>Cherry</b> · <b>Gold</b> · <b>Violet</b>
      <br>2 языка (Русский / English). Переключение в один клик.

    </td>
    <td width="50%" valign="top">

      <h3>⚙ Системная интеграция</h3>
      Трей (область уведомлений), автозапуск с Windows, контекстное меню для .jar в Проводнике, ассоциация файлов, горячие клавиши: Ctrl+S/E/F.

    </td>
  </tr>
</table>

---

<!-- ==================== TOOLS ==================== -->
<h2 align="center">🔧 Встроенные инструменты</h2>

<p align="center"><i>Все утилиты хранятся внутри одного .exe файла и извлекаются автоматически.</i></p>

<table>
  <tr>
    <th align="center">#</th>
    <th align="center">Иконка</th>
    <th align="left">Инструмент</th>
    <th align="left">Категория</th>
    <th align="left">Описание</th>
  </tr>
  <tr><td align="center">1</td><td align="center">🔍</td><td><b>Everything</b></td><td>Поиск</td><td>Мгновенный поиск файлов по имени/размеру/типу на всём диске</td></tr>
  <tr><td align="center">2</td><td align="center">🔬</td><td><b>HxD</b></td><td>Редактирование</td><td>Hex-редактор для анализа бинарных данных</td></tr>
  <tr><td align="center">3</td><td align="center">☕</td><td><b>Luyten</b></td><td>Декомпиляция</td><td>Java-декомпилятор — просмотр исходного кода .jar/.class</td></tr>
  <tr><td align="center">4</td><td align="center">💻</td><td><b>SystemInformer</b></td><td>Мониторинг</td><td>Монитор процессов и DLL-инъекций</td></tr>
  <tr><td align="center">5</td><td align="center">📥</td><td><b>InjGen</b></td><td>Детекция</td><td>Детект DLL-инъекций в процессы</td></tr>
  <tr><td align="center">6</td><td align="center">🔊</td><td><b>CleaningDetector</b></td><td>Детекция</td><td>Обнаруживает чистку цифровых следов</td></tr>
  <tr><td align="center">7</td><td align="center">📃</td><td><b>JournalTrace</b></td><td>Форензика</td><td>USN Journal — восстановление удалённых файлов</td></tr>
  <tr><td align="center">8</td><td align="center">📁</td><td><b>ShellBag Analyzer</b></td><td>Форензика</td><td>ShellBags — история посещённых папок</td></tr>
  <tr><td align="center">9</td><td align="center">⚡</td><td><b>USBDriveLog</b></td><td>Форензика</td><td>История подключённых USB-накопителей</td></tr>
  <tr><td align="center">10</td><td align="center">📊</td><td><b>ExecutedProgramsList</b></td><td>Форензика</td><td>Список всех запускавшихся программ</td></tr>
  <tr><td align="center">11</td><td align="center">🔏</td><td><b>PrefetchView++</b></td><td>Форензика</td><td>Хронология запуска приложений (Prefetch)</td></tr>
  <tr><td align="center">12</td><td align="center">📍</td><td><b>PathsParser</b></td><td>Форензика</td><td>Парсинг путей из системных артефактов</td></tr>
</table>

---

<!-- ==================== DETECTION ENGINE ==================== -->
<h2 align="center">🧬 Движок детекции</h2>

<details>
<summary><b>🔍 Что ищет Сканер</b></summary>
<br>
<table>
  <tr><th>Метод</th><th>Описание</th><th>Баллы</th></tr>
  <tr><td>📝 Имя файла</td><td>150+ известных названий читов</td><td align="center">+30</td></tr>
  <tr><td>🔢 Размер файла</td><td>130+ известных размеров JAR-читов</td><td align="center">+25</td></tr>
  <tr><td>🔑 SHA-256 хэш</td><td>Точное совпадение с базой известных читов</td><td align="center">+99</td></tr>
  <tr><td>🎯 Ключевые слова</td><td>260+ ключевых слов (чит-модули, пакеты, термины)</td><td align="center">+12</td></tr>
  <tr><td>📂 Подозрительные папки</td><td>%TEMP%, %APPDATA%, \Downloads, \Public</td><td align="center">+10</td></tr>
  <tr><td>💡 Хеш-имена</td><td>32+ hex-символов, GUID, случайные имена</td><td align="center">+6–12</td></tr>
</table>
</details>

<details>
<summary><b>🧰 Как работает Mod Checker</b></summary>
<br>
<ol>
  <li><b>Распаковка ZIP-архива</b> и анализ структуры</li>
  <li><b>Проверка MANIFEST.MF</b> на main-class</li>
  <li><b>Анализ mcmod.info / fabric.mod.json / mods.toml</b> — поиск запрещённых модов</li>
  <li><b>Поиск путей пакетов</b> — 70+ префиксов читов</li>
  <li><b>Декомпиляция .class файлов</b> и поиск строк</li>
  <li><b>Подсчёт совпадений</b> с 260+ ключевыми словами</li>
  <li><b>Обнаружение обфускации</b> (короткие имена, энтропия)</li>
  <li><b>Проверка на вложенные JAR-архивы</b></li>
  <li><b>Сравнение SHA-256</b> с базой известных читов</li>
</ol>
Результат: подробный отчёт с баллами, вердиктом и деталями.
</details>

<details>
<summary><b>🏴 PE-анализ (для .exe/.dll)</b></summary>
<br>
<ul>
  <li>📦 Секции PE (подозрительные: .upack, .themida, .vmp, .mpress)</li>
  <li>🏴 Import-таблицы (WriteProcessMemory, CreateRemoteThread)</li>
  <li>🕰 Время компиляции (нулевое / будущее)</li>
  <li>💠 Overlay-данные (прикреплённые данные после секций)</li>
  <li>🔐 Цифровая подпись (отсутствие = подозрительно)</li>
  <li>🎮 TLS-колбэки (редко в легитимных приложениях)</li>
  <li>🎯 .NET обфускация (Confuser, Obfuscar, .NET Reactor)</li>
</ul>
</details>

<details>
<summary><b>🛡 Белый список</b></summary>
<br>
Никогда не считаются читом:
<ul>
  <li>🎮 Ядра Minecraft: minecraft, netty, lwjgl, authlib</li>
  <li>⚙ Модификации: optifine, sodium, iris, lithium, create, jei, rei</li>
  <li>📡 Библиотеки: gson, fastutil, jackson, guava, log4j</li>
  <li>🖥 Системные файлы Windows: ntdll, kernel32, shell32</li>
  <li>📖 Модификации FunMoon: fmutils, fm-utils</li>
</ul>
</details>

---

<!-- ==================== ROADMAP ==================== -->
<h2 align="center">🗺 Дорожная карта</h2>

<table>
  <tr><th align="center">Статус</th><th align="left">Что сделано / планируется</th></tr>
  <tr><td align="center">✅</td><td>Сканер дисков с классификацией по уровню подозрения</td></tr>
  <tr><td align="center">✅</td><td>Mod Checker с эвристическим анализом</td></tr>
  <tr><td align="center">✅</td><td>12 встроенных форензик-утилит</td></tr>
  <tr><td align="center">✅</td><td>7 тем оформления, 2 языка</td></tr>
  <tr><td align="center">✅</td><td>Tray-режим, автозапуск, горячие клавиши</td></tr>
  <tr><td align="center">✅</td><td>Контекстное меню для .jar в Проводнике</td></tr>
  <tr><td align="center">🎯</td><td>Онлайн-проверка по базе сигнатур через API</td></tr>
  <tr><td align="center">🎯</td><td>Drag-and-drop папок в Mod Checker</td></tr>
  <tr><td align="center">🎯</td><td>Экспорт логов в HTML/PDF</td></tr>
  <tr><td align="center">🎯</td><td>Плагины и кастомные правила сканирования</td></tr>
</table>

---

<!-- ==================== INSTALLATION ==================== -->
<h2 align="center">📱 Установка</h2>

<table align="center">
  <tr><td align="center" width="60">🛡️</td><td><b>Шаг 1</b></td><td>Скачайте <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><code>FMTool-Installer.exe</code></a> из раздела Releases</td></tr>
  <tr><td align="center" width="60">⚡</td><td><b>Шаг 2</b></td><td>Запустите установщик и следуйте инструкциям</td></tr>
  <tr><td align="center" width="60">🚀</td><td><b>Шаг 3</b></td><td>Запустите ярлык FMTool на рабочем столе</td></tr>
  <tr><td align="center" width="60">🤖</td><td><b>Шаг 4</b></td><td>Пользуйтесь! Все инструменты уже внутри</td></tr>
</table>

<blockquote>🆇 <b>Требования:</b> Windows 10/11 64-bit, .NET 8.0 Runtime (установщик предложит скачать при необходимости)</blockquote>

---

<!-- ==================== FAQ ==================== -->
<h2 align="center">❓ FAQ</h2>

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
<summary><b>🐛 Нашёл баг? Есть идея?</b></summary>
<br>
Создайте <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">новую заметку</a> в разделе Issues. Подробно опишите проблему или предложение.
</details>

---

<!-- ==================== CHANGELOG ==================== -->
<h2 align="center">📝 История версий</h2>

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
  <li>🎨 7 тем оформления</li>
  <li>🛡️ 12 встроенных утилит для форензики</li>
  <li>⚙️ Автообновление, автозапуск, интеграция</li>
  <li>⌨ Горячие клавиши: Ctrl+S/E/F</li>
</ul>

---

<!-- ==================== TECH DETAILS ==================== -->
<h2 align="center">📰 Технические детали</h2>

<table align="center">
  <tr><th>Параметр</th><th>Значение</th></tr>
  <tr><td>💻 Платформа</td><td>.NET 8 WPF, Windows 10/11 x64</td></tr>
  <tr><td>📦 Формат</td><td>Single-file self-contained</td></tr>
  <tr><td>🧰 Сжатие</td><td>LZMA2</td></tr>
  <tr><td>📄 Установщик</td><td>Inno Setup 6</td></tr>
  <tr><td>📜 Лицензия</td><td><a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT</a></td></tr>
</table>

---

<!-- ==================== AUDIENCE ==================== -->
<h2 align="center">❤️ Для кого это</h2>

<table align="center">
  <tr>
    <td align="center" width="80">🛡️</td>
    <td><b>Модераторов</b> FunMoon</td>
    <td>Проверка игроков на читы и запрещённые моды</td>
  </tr>
  <tr>
    <td align="center" width="80">💻</td>
    <td><b>Администраторов</b> серверов</td>
    <td>Форензика, журналы, анализ активности</td>
  </tr>
  <tr>
    <td align="center" width="80">🔍</td>
    <td><b>Исследователей</b></td>
    <td>Анализ .jar, декомпиляция, HEX-редактирование</td>
  </tr>
</table>

---

<!-- ==================== FOOTER ==================== -->
<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="80"></a>
  <br><br>
  <sub>Сделано с ❤️ для сервера <b>FunMoon</b> ✨</sub>
  <br>
  <sub>© 2026 <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a> / FunMoon — <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT License</a></sub>
</p>