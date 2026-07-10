
<p align="center">
  <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="200">
</p>

<p align="center">
  <img src="https://img.shields.io/badge/%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F-1.0.3-3178C6?style=for-the-badge&logo=github"/>
  <img src="https://img.shields.io/badge/.NET-8.0-7B1FA2?style=for-the-badge&logo=dotnet"/>
  <img src="https://img.shields.io/badge/Platform-Windows%20x64-28A745?style=for-the-badge&logo=windows"/>
  <img src="https://img.shields.io/badge/%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F-MIT-D73A49?style=for-the-badge"/>
</p>

<h1 align="center">🛡️ FMTool</h1>

<p align="center">
  <b>Криминалистический инструмент для модераторов Minecraft-сервера <b>FunMoon</b></b>
  <br>
  Сканирует ПК подозреваемого, находит читы по имени/размеру/SHA-256/структуре классов,
  <br>
  анализирует .jar/память/журналы через встроенные форензик-утилиты.
  <br><br>
  <b>Полностью бесплатно ✨ Открытый исходный код (MIT)</b>
</p>

<br>

---

<br>

<h2 align="center">📷 Интерфейс</h2>

<p align="center">
  <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Square.jpg" alt="FMTool главное окно" width="820">
  <br>
  <i>Главное окно приложения — навигация, результаты сканирования, встроенные утилиты</i>
</p>

<br>

---

<br>

<h2 align="center">✨ Возможности</h2>

<table>
  <tr>
    <td width="50%" valign="top">
      <h3>🔍 Сканер дисков</h3>
      Сканирует C:\, Рабочий стол, Загрузки и любые папки. Ищет .jar, .exe, .dll по сигнатурам читов, классифицирует по уровню подозрения. Поддерживает глубокий анализ содержимого.
      <br><br>
      <b>Ключевые метрики:</b>
      <br>
      📝 Имя файла — проверка по 150+ названиям читов
      <br>
      🔢 Размер — сравнение с 130+ известными размерами JAR
      <br>
      🔑 SHA-256 — точное совпадение с базой
      <br>
      📍 Подозрительные папки — %TEMP%, %APPDATA%, Downloads
    </td>
    <td width="50%" valign="top">
      <h3>🧰 Mod Checker</h3>
      Загрузка .jar-файлов через Drag&Drop или кнопку. Эвристический анализ по 500+ сигнатурам, проверка SHA-256, декомпиляция классов через Luyten.
      <br><br>
      <b>Что анализируется:</b>
      <br>
      📂 Структура ZIP-архива
      <br>
      📄 Манифест (MANIFEST.MF) и метаданные
      <br>
      🔍 Пути пакетов (70+ префиксов читов)
      <br>
      🧮 Декомпиляция .class файлов
    </td>
  </tr>
  <tr>
    <td width="50%" valign="top">
      <h3>📊 Журналы и логи</h3>
      Просмотр истории PowerShell, списка автозапускающихся программ. Вся информация в одном окне — не нужно лазить по системным папкам.
    </td>
    <td width="50%" valign="top">
      <h3>📋 Паттерны поиска</h3>
      6 готовых поисковых запросов для Everything. Чит-паттерны, размеры JAR, проверка vec.dll, CSRSS и другие. Копируйте в буфер одним кликом.
    </td>
  </tr>
  <tr>
    <td width="50%" valign="top">
      <h3>🎨 Персонализация</h3>
      7 тем оформления с плавными анимациями: Dark, Arctic, Emerald, Rose, Cherry, Gold, Violet. 2 языка (Русский / English). Переключение в один клик.
    </td>
    <td width="50%" valign="top">
      <h3>⚙️ Системная интеграция</h3>
      Трей (сворачивание в область уведомлений), автозапуск с Windows, контекстное меню для .jar, горячие клавиши: Ctrl+S (скан), Ctrl+E (экспорт), Ctrl+F (поиск).
    </td>
  </tr>
</table>

<br>

---

<br>

<h2 align="center">🛠️ Встроенные инструменты</h2>

<p align="center">
  <i>Все утилиты хранятся внутри одного .exe файла и извлекаются автоматически при запуске.</i>
</p>

<table>
  <tr>
    <th align="center">Иконка</th>
    <th align="left">Инструмент</th>
    <th align="left">Категория</th>
    <th align="left">Описание</th>
  </tr>
  <tr><td align="center">🔍</td><td><b>Everything</b></td><td>Поиск</td><td>Мгновенный поиск файлов по имени/размеру/типу</td></tr>
  <tr><td align="center">🔬</td><td><b>HxD</b></td><td>Редактирование</td><td>Hex-редактор для анализа бинарных данных</td></tr>
  <tr><td align="center">☕</td><td><b>Luyten</b></td><td>Декомпиляция</td><td>Java-декомпилятор (просмотр .jar/.class)</td></tr>
  <tr><td align="center">💻</td><td><b>SystemInformer</b></td><td>Мониторинг</td><td>Монитор процессов и DLL-инъекций</td></tr>
  <tr><td align="center">📥</td><td><b>InjGen</b></td><td>Детекция</td><td>Детект DLL-инъекций в процессы</td></tr>
  <tr><td align="center">🔊</td><td><b>CleaningDetector</b></td><td>Детекция</td><td>Обнаруживает чистку цифровых следов</td></tr>
  <tr><td align="center">📃</td><td><b>JournalTrace</b></td><td>Форензика</td><td>USN Journal — восстановление удалённых файлов</td></tr>
  <tr><td align="center">📁</td><td><b>ShellBag Analyzer</b></td><td>Форензика</td><td>ShellBags — история посещённых папок</td></tr>
  <tr><td align="center">⚡</td><td><b>USBDriveLog</b></td><td>Форензика</td><td>История подключённых USB-накопителей</td></tr>
  <tr><td align="center">📊</td><td><b>ExecutedProgramsList</b></td><td>Форензика</td><td>Список всех запускавшихся программ</td></tr>
  <tr><td align="center">🔏</td><td><b>PrefetchView++</b></td><td>Форензика</td><td>Хронология запуска приложений (Prefetch)</td></tr>
  <tr><td align="center">📍</td><td><b>PathsParser</b></td><td>Форензика</td><td>Парсинг путей из системных артефактов</td></tr>
</table>

<br>

---

<br>

<h2 align="center">🧬 Движок детекции</h2>

<details>
<summary><b>🔍 Что ищет Сканер</b></summary>
<br>
<table>
  <tr><th>Метод</th><th>Описание</th><th>Баллы</th></tr>
  <tr><td>📝 Имя файла</td><td>Совпадение с 150+ известными названиями читов</td><td align="center">+30</td></tr>
  <tr><td>🔢 Размер файла</td><td>Совпадение с 130+ известными размерами JAR-читов</td><td align="center">+25</td></tr>
  <tr><td>🔑 SHA-256 хэш</td><td>Точное совпадение с базой известных читов</td><td align="center">+99</td></tr>
  <tr><td>🎯 Ключевые слова</td><td>260+ ключевых слов (чит-модули, пакеты, термины)</td><td align="center">+12</td></tr>
  <tr><td>📍 Подозрительные папки</td><td>Файлы в %TEMP%, %APPDATA%, \Downloads, \Public</td><td align="center">+10</td></tr>
  <tr><td>💡 Хеш-подобные имена</td><td>32+ hex-символов, GUID, случайные имена</td><td align="center">+6–12</td></tr>
</table>
</details>

<details>
<summary><b>🧰 Как работает Mod Checker</b></summary>
<br>
При анализе .jar-файла происходят следующие проверки:
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
При глубоком сканировании анализируются:
<ul>
  <li>📦 Секции PE (подозрительные имена: .upack, .themida, .vmp, .mpress)</li>
  <li>🏴 Import-таблицы (WriteProcessMemory, CreateRemoteThread, NtQueryInformationProcess)</li>
  <li>🕰️ Время компиляции (нулевое / будущее)</li>
  <li>💠 Overlay-данные (прикреплённые данные)</li>
  <li>🔐 Цифровая подпись (отсутствие = подозрительно)</li>
  <li>🎮 TLS-колбэки (редко в легитимных приложениях)</li>
  <li>🎯 .NET обфускация (Confuser, Obfuscar, .NET Reactor)</li>
</ul>
</details>

<details>
<summary><b>🛡️ Что исключается из проверки</b></summary>
<br>
Белый список (никогда не считается читом):
<ul>
  <li>🎮 Ядра Minecraft: minecraft, netty, lwjgl, authlib</li>
  <li>⚙️ Модификации: optifine, sodium, iris, lithium, create, jei, rei</li>
  <li>📡 Библиотеки: gson, fastutil, jackson, guava, log4j</li>
  <li>📽️ Системные файлы Windows: ntdll, kernel32, shell32</li>
  <li>📖 Модификации FunMoon: fmutils, fm-utils</li>
</ul>
</details>

<br>

---

<br>

<h2 align="center">🎉 Дорожная карта</h2>

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

<br>

---

<br>

<h2 align="center">📱 Установка</h2>

<table align="center">
  <tr><td align="center" width="60">🛡️</td><td><b>Шаг 1</b></td><td>Скачайте <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><code>FMTool-Installer.exe</code></a> из раздела Releases</td></tr>
  <tr><td align="center">⚡</td><td><b>Шаг 2</b></td><td>Запустите установщик и следуйте инструкциям</td></tr>
  <tr><td align="center">🚀</td><td><b>Шаг 3</b></td><td>Запустите ярлык FMTool на рабочем столе</td></tr>
  <tr><td align="center">🤖</td><td><b>Шаг 4</b></td><td>Пользуйтесь! Все инструменты уже внутри</td></tr>
</table>

<blockquote>
🚧 <b>Требования:</b> Windows 10/11 64-bit, .NET 8.0 Runtime (установщик предложит скачать при необходимости)
</blockquote>

<br>

---

<br>

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

<br>

---

<br>

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
  <li>⌨️ Горячие клавиши: Ctrl+S/E/F</li>
</ul>

<br>

---

<br>

<h2 align="center">📰 Технические детали</h2>

<table align="center">
  <tr><th>Параметр</th><th>Значение</th></tr>
  <tr><td>💻 Платформа</td><td>.NET 8 WPF, Windows 10/11 x64</td></tr>
  <tr><td>📦 Формат</td><td>Single-file self-contained</td></tr>
  <tr><td>🧰 Сжатие</td><td>LZMA2</td></tr>
  <tr><td>📄 Установщик</td><td>Inno Setup 6</td></tr>
  <tr><td>📜 Лицензия</td><td><a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT</a></td></tr>
</table>

<br>

---

<br>

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
    <td>Анализ .jar, декомпиляция, HEX</td>
  </tr>
</table>

<br>

---

<br>

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM">
    <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="80">
  </a>
  <br><br>
  <sub>Сделано с любовью для сервера <b>FunMoon</b> ✨</sub>
  <br>
  <sub>© 2026 <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a> / FunMoon — Распространяется под лицензией <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT</a></sub>
</p>
