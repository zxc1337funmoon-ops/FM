<!-- ============================================================ -->
<!--  FMTool                                                -->
<!--  Copyright (c) 2026 zxc1337 / FunMoon — MIT License        -->
<!-- ============================================================ -->

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FMTool" width="150"></a>
</p>

<h1 align="center">🛡️ FMTool</h1>

<p align="center">
  <b>Криминалистический античит-инструментарий</b>
  <br>
  для модераторов Minecraft-сервера <b>FunMoon</b>
  <br><br>
  Сканирует компьютер, находит читы по сотням сигнатур,
  <br>
  анализирует .jar / .exe / .dll, запускает 12 форензик-утилит.
  <br><br>
  <b>Бесплатно</b> · <b>Открытый код</b> · <b>MIT</b>
</p>

<br>

<!-- ==================== BADGES ====================
     Row 1: Действия       Row 2: Информация    Row 3: Возможности
-->
<p align="center">
  <!-- ════ ДЕЙСТВИЯ ════ -->
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases/latest"><img src="https://img.shields.io/badge/%F0%9F%93%A6%20%D0%92%D0%B5%D1%80%D1%81%D0%B8%D1%8F-v1.0.3-3178C6?style=for-the-badge&logo=github" alt="Версия v1.0.3"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/releases"><img src="https://img.shields.io/badge/%E2%AC%87%20%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C-Installer-28A745?style=for-the-badge&logo=github" alt="Скачать"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new"><img src="https://img.shields.io/badge/%F0%9F%90%9B%20%D0%A1%D0%BE%D0%BE%D0%B1%D1%89%D0%B8%D1%82%D1%8C-%D0%BE%20%D0%B1%D0%B0%D0%B3%D0%B5-D73A49?style=for-the-badge&logo=github" alt="Баг"></a>
  <!-- ════ ИНФОРМАЦИЯ ════ -->
  <br>
  <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE"><img src="https://img.shields.io/badge/%F0%9F%93%9C%20%D0%9B%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F-MIT-7B1FA2?style=for-the-badge&logo=open-source-initiative" alt="MIT"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%92%BB%20%D0%9F%D0%BB%D0%B0%D1%82%D1%84%D0%BE%D1%80%D0%BC%D0%B0-Windows%2010%2F11-0078D6?style=for-the-badge&logo=windows" alt="Windows 10/11"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%94%AE%20.NET-8.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET 8"></a>
  <!-- ════ ВОЗМОЖНОСТИ ════ -->
  <br>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%9B%A1%20%D0%A2%D0%B8%D0%BF-%D0%90%D0%BD%D1%82%D0%B8%D1%87%D0%B8%D1%82%20%D1%84%D0%BE%D1%80%D0%B5%D0%BD%D0%B7%D0%B8%D0%BA%D0%B0-6B7280?style=for-the-badge&logo=github" alt="Античит форензика"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%94%8D%20%D0%A1%D0%B8%D0%B3%D0%BD%D0%B0%D1%82%D1%83%D1%80-500%2B-E6B91E?style=for-the-badge&logo=github" alt="500+ сигнатур"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%E2%9A%99%20%D0%A3%D1%82%D0%B8%D0%BB%D0%B8%D1%82-12%20%D1%88%D1%82.-FF6600?style=for-the-badge&logo=github" alt="12 утилит"></a>
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://img.shields.io/badge/%F0%9F%8E%A8%20%D0%A2%D0%B5%D0%BC-7%20%D1%88%D1%82.-8B5CF6?style=for-the-badge&logo=github" alt="7 тем"></a>
</p>

---

## 📷 Интерфейс

<p align="center">
  <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Square.jpg" alt="FMTool Screenshot" width="820" style="border-radius: 8px;">
  <br>
  <i>Главное окно: сканирование, анализ, 12 утилит и 7 тем оформления</i>
</p>

---

## ❓ Что это такое?

**FMTool** — это Windows-программа для модераторов сервера **FunMoon**.  
Она устанавливается на компьютер подозреваемого, находит читы, запрещённые моды и следы нечестной игры.

> ⚠️ **Это не мод для Майнкрафта.** Это десктопное приложение на C# / WPF под Windows 10/11.

---

## ✨ Возможности

| | |
|---|---|
| **🔍 Сканер дисков** — ищет .jar, .exe, .dll по 500+ сигнатурам. Проверяет имя, размер, SHA-256, папки, хэш-имена. Каждому файлу — баллы подозрения. | **🧰 Mod Checker** — загрузите .jar через Drag&Drop. Распакует ZIP, проверит манифест, найдёт 70+ префиксов читов, декомпилирует классы и вынесет вердикт. |
| **📊 Журналы и логи** — история PowerShell, автозагрузка, процессы. Всё в одном окне. | **📋 Паттерны поиска** — 6 готовых запросов для Everything. Копирование в буфер одним кликом. |
| **🎨 7 тем** — Dark, Arctic, Emerald, Rose, Cherry, Gold, Violet. Плавные анимации. 🌐 Русский / English. | **⚙ Интеграция** — трей, автозапуск, контекстное меню .jar, Ctrl+S / Ctrl+E / Ctrl+F. |

---

## 🔧 12 встроенных инструментов

Все утилиты внутри одного .exe — ничего качать не нужно.

| # | Инструмент | Назначение |
|---|------------|-----------|
| 1 | **Everything** | Мгновенный поиск файлов на всём диске |
| 2 | **HxD** | Hex-редактор для бинарных данных |
| 3 | **Luyten** | Декомпилятор Java (.jar / .class) |
| 4 | **SystemInformer** | Мониторинг процессов и DLL-инъекций |
| 5 | **InjGen** | Детектор DLL-инъекций |
| 6 | **CleaningDetector** | Поиск чистки цифровых следов |
| 7 | **JournalTrace** | USN Journal — восстановление удалённого |
| 8 | **ShellBag Analyzer** | История посещённых папок |
| 9 | **USBDriveLog** | История USB-накопителей |
| 10 | **ExecutedProgramsList** | Все запускавшиеся программы |
| 11 | **PrefetchView++** | Хронология запуска (.pf файлы) |
| 12 | **PathsParser** | Извлечение путей из артефактов |

---

## 🧬 Движок детекции

<details>
<summary><b>🔍 Как работает Сканер</b></summary>

| Метод | Описание | Баллы |
|-------|----------|:-----:|
| 📝 Имя файла | 150+ известных названий читов | +30 |
| 🔢 Размер | 130+ характерных размеров JAR | +25 |
| 🔑 SHA-256 | Точное совпадение с базой | +99 |
| 🎯 Ключевые слова | 260+ слов в содержимом | +12 |
| 📂 Папки | %TEMP%, %APPDATA%, Downloads, Public | +10 |
| 💡 Хэш-имена | 32+ hex, GUID, случайные имена | +6–12 |

Чем выше сумма баллов — тем выше вероятность чита.
</details>

<details>
<summary><b>🧰 Как работает Mod Checker</b></summary>

1. Распаковка ZIP и анализ структуры
2. Проверка MANIFEST.MF — поиск main-class
3. Анализ mcmod.info / fabric.mod.json / mods.toml
4. Поиск путей пакетов — 70+ префиксов читов
5. Декомпиляция .class файлов и поиск строк
6. Подсчёт совпадений с 260+ ключевыми словами
7. Обнаружение обфускации (короткие имена, энтропия)
8. Проверка вложенных JAR
9. Сверка SHA-256 с базой

Результат — отчёт с баллами, вердиктом и доказательствами.
</details>

<details>
<summary><b>🏴 PE-анализ (.exe / .dll)</b></summary>

- 📦 Секции: .upack, .themida, .vmp, .mpress
- 🏴 Импорты: WriteProcessMemory, CreateRemoteThread
- 🕰 Время компиляции: нулевое или из будущего
- 💠 Overlay-данные после секций
- 🔐 Отсутствие цифровой подписи
- 🎮 TLS-колбэки (редкость в легитимном ПО)
- 🎯 .NET обфускация: Confuser, Obfuscar, .NET Reactor
</details>

<details>
<summary><b>🛡 Белый список</b></summary>

Никогда не считаются читами:
- 🎮 Ядра Minecraft: minecraft, netty, lwjgl, authlib
- ⚙ Моды: optifine, sodium, iris, lithium, create, jei, rei
- 📡 Библиотеки: gson, fastutil, jackson, guava, log4j
- 🖥 Системные: ntdll, kernel32, shell32, user32
- 📖 FunMoon: fmutils, fm-utils, funmoon
</details>

---

## 📥 Установка

| Шаг | Действие |
|-----|----------|
| 1 | Скачайте [**FMTool-Installer.exe**](https://github.com/zxc1337funmoon-ops/FM/releases/latest) из Releases |
| 2 | Запустите и следуйте инструкциям |
| 3 | Готово — все инструменты уже внутри |

> 📌 Windows 10/11 x64, .NET 8.0. Установщик предложит скачать .NET, если его нет.

---

## ❓ Частые вопросы

<details>
<summary><b>💡 Это мод для Майнкрафта?</b></summary>
Нет. Это Windows-программа (C#, .NET 8, WPF). Она не ставится в игру.
</details>

<details>
<summary><b>🔍 Как проверить .jar?</b></summary>
Перетащите в Mod Checker или нажмите «Обзор». Можно также через контекстное меню Проводника → «Scan with FMTool».
</details>

<details>
<summary><b>🐛 Нашли баг?</b></summary>
Создайте [обращение на GitHub](https://github.com/zxc1337funmoon-ops/FM/issues/new). Подробно опишите проблему.
</details>

<details>
<summary><b>🔒 Нужен интернет?</b></summary>
Нет. Все проверки локальные. Интернет нужен только для обновлений.
</details>

---

## 📋 История версий

**v1.0.3**
- 🖼 Иконки извлекаются из самих .exe
- 🧰 Кнопки Save/Load выровнены
- 💾 Пустой список модов не вызывает ошибок

**v1.0.2**
- 🐛 Удалены ссылки на отсутствующий recaf-cli
- 📑 Удалены 17 дублирующихся ключей локализации
- 🗑 Очищены неиспользуемые темы

**v1.0.1** — Исправлены мелкие ошибки

**v1.0.0**
- 🔍 Сканер (4 потока) · 🧰 Mod Checker · 🎨 7 тем
- 🛡️ 12 утилит · ⚙ Автообновление · ⌨ Ctrl+S/E/F · 🌐 EN/RU

---

## 🛠 Технические детали

| | |
|---|---|
| 💻 Платформа | .NET 8 WPF, Windows 10/11 x64 |
| 📦 Упаковка | Single-file self-contained, LZMA2 |
| 📄 Установщик | Inno Setup 6 |
| 📜 Лицензия | [MIT](https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE) |
| ⭐ | Пока 0 😅 — поставьте, если полезно! |

---

<p align="center">
  <a href="https://github.com/zxc1337funmoon-ops/FM"><img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/assets/Circle.png" alt="FunMoon" width="48"></a>
  <br><br>
  Сделано с ❤️ для сообщества <b>FunMoon</b>
  <br>
  © 2026 <a href="https://github.com/zxc1337funmoon-ops">zxc1337</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/blob/main/LICENSE">MIT License</a>
  <br><br>
  <a href="https://github.com/zxc1337funmoon-ops/FM">GitHub</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/releases">Releases</a> · <a href="https://github.com/zxc1337funmoon-ops/FM/issues/new">Сообщить о баге</a>
</p>