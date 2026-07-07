<div align="center">
  <img src="https://raw.githubusercontent.com/zxc1337funmoon-ops/FM/main/Resources/Circle.png" alt="FMTool" width="96" height="96" style="border-radius: 50%;">
  
  # 🛡️ FMTool
  
  ### Minecraft Cheat Detection & File Analysis Tool
  
  <p align="center">
    <strong>Mod Checker</strong> • <strong>Scanner</strong> • <strong>Forensic Toolkit</strong>
  </p>
  
  <p align="center">
    <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8"/>
    <img src="https://img.shields.io/badge/Windows-10%2F11-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows"/>
    <img src="https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#"/>
    <img src="https://img.shields.io/badge/UI-WPF-0C54C2?style=for-the-badge&logo=windows&logoColor=white" alt="WPF"/>
    <img src="https://img.shields.io/github/v/release/zxc1337funmoon-ops/FM?style=for-the-badge&label=Version&color=FF8A3D" alt="Version"/>
  </p>
  
  <br>
</div>

---

## ✨ Возможности

### 🔍 Mod Checker

Глубокая проверка Minecraft модов на наличие читов.

- **Параллельное сканирование** — 4 потока с реальным прогресс-баром
- **SHA-256 кэш** — не пересчитывает хеш, если файл не менялся
- **Drag & Drop** — кинь папку, FMTool сам найдёт все `.jar`
- **История проверок** — автосохранение, загрузка (JSON)
- **Фильтр по имени** — быстрый поиск нужного мода
- **Оценка 0–100+** на основе 40+ категорий анализа

<details>
<summary>🕵️ Что анализирует Mod Checker</summary>
<br>

- **150+ названий читов** — Impact, Wurst, Meteor, LiquidBounce, Doomsday и др.
- **200+ ключевых слов** — поиск читерских классов внутри `.class` файлов
- **10+ обфускаторов** — Allatori, ProGuard, Zelix, Radon, Stringer и др.
- **SHA-256 сигнатуры** — сравнение с базой известных модов
- **Размеры с допуском 10%** — сопоставление с базой размеров
- **Вложенные JAR/ZIP** — детект архивов внутри архивов
- **Mixin JSON** — анализ конфигов модов
- **MANIFEST.MF** — парсинг метаданных

</details>

### 📂 Scanner

Сканирование файловой системы для обнаружения подозрительных файлов.

- **Группировка** — результаты по уровням: Suspicious (High) / Suspicious / Normal
- **Панель деталей** — имя, путь, размер, тип, статус по клику
- **Умный анализ** — PE-заголовки, энтропия, импорты, YARA-сигнатуры
- **Без лимитов** — JAR/EXE/DLL любого размера
- **100+ в белом списке** — netty, lwjgl, gson, log4j — без ложных срабатываний
- **Горячие клавиши** — `Ctrl+S` (скан), `Ctrl+E` (экспорт), `Ctrl+F` (поиск)

<details>
<summary>📋 Что проверяет Scanner</summary>
<br>

- Системные диски (C:\), Рабочий стол, Загрузки
- Папку `.minecraft` на всех дисках (автоопределение)
- Любую кастомную папку через Drag & Drop
- **40+ системных путей исключены** (Windows, Program Files, Steam, .git и т.д.)
- 5 уровней: Normal → Low Suspicion → Suspicious → High Suspicion → Critical

</details>

### 🚀 Programs — Встроенные утилиты

| Утилита | Назначение |
|---------|-----------|
| **HxD** | Hex-редактор |
| **Everything** | Мгновенный поиск файлов |
| **SystemInformer** | Монитор процессов и сети |
| **Luyten** | Java-декомпилятор |
| **PrefetchView++** | Анализ Prefetch |
| **USBDriveLog** | История USB |
| **JournalTrace** | USN Journal |
| **ShellBag Analyzer** | История папок |
| **ExecutedProgramsList** | Список запусков |
| **CleaningDetector** | Детект чистки |
| **PathsParser** | Парсинг путей |
| **InjGen** | Анализ инъекций |
| **Recaf** | Java байткод-редактор |

### ⚙️ Интеграция

- **Контекстное меню** — правый клик на `.jar` → «Сканировать FMTool»
- **Ассоциация файлов** — открытие `.jar` через FMTool по умолчанию
- **Авто-обновление** — проверка новой версии на GitHub
- **Системный трей** — закрытие → в трей, двойной клик → восстановить
- **Автозапуск** — старт вместе с Windows

### 🎨 Интерфейс

<div align="center">
  <table>
    <tr>
      <td align="center"><b>7 тем</b></td>
      <td>Dark, Arctic, Emerald, Rose, Cherry, Gold, Violet</td>
    </tr>
    <tr>
      <td align="center"><b>Локализация</b></td>
      <td>Русский + English (переключение на лету)</td>
    </tr>
    <tr>
      <td align="center"><b>Скруглённые углы</b></td>
      <td>DWM (Windows 11)</td>
    </tr>
    <tr>
      <td align="center"><b>Анимации</b></td>
      <td>300ms BackEase, Glass-морфизм, тени</td>
    </tr>
  </table>
</div>

---

## 🛠 Технические детали

| Параметр | Значение |
|----------|----------|
| **Платформа** | .NET 8 WPF, Windows 10/11 x64 |
| **Сборка** | Single-file self-contained (118 MB) |
| **Установщик** | Inno Setup LZMA2 |
| **Архитектура** | x64 |
| **Автор** | zxc1337 |

---

## 📦 Установка

1. Скачай `FMTool-Setup-x.x.x.exe` из [релизов](https://github.com/zxc1337funmoon-ops/FM/releases)
2. Запусти установщик
3. Готово! Можно сразу сканировать.

Или скачай `FMTool-x.x.x.exe` — портативная версия без установки.

---

## 💻 Требования

- **ОС:** Windows 10 / 11 (x64)
- **RAM:** от 2 GB
- **Место:** ~150 MB (портативная) / ~300 MB (с утилитами)
- **Java (опционально):** для запуска Luyten и Recaf

---

<div align="center">
  <br>
  <p><i>Сделано zxc1337 для сервера FunMoon.</i></p>
  <p>
    <a href="https://github.com/zxc1337funmoon-ops/FM/issues">Сообщить о проблеме</a>
    ·
    <a href="https://github.com/zxc1337funmoon-ops/FM/releases">Скачать</a>
  </p>
  <br>
</div>
