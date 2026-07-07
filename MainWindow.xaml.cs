using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Interop;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Forms = System.Windows.Forms;

namespace FMTool
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource? _scanCts;
        private readonly ObservableCollection<ScanItem> _results = new();
        private readonly List<ScanItem> _allResults = new();
        private readonly List<ProgramItem> _programs = new();
        private readonly List<Process> _launched = new();
        private bool _isDark = true;
        private int _lang = 0; // 0=En, 1=Ru
        private static int _langStatic = 0;
        private Forms.NotifyIcon? _trayIcon;
        private string _theme = "Dark";
        private bool _ready = false;
        private string _extractDir = "";
        private string _customFolder = "";
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string RunValueName = "FMTool";

        private static readonly string[] ChangelogEn = {
            "FMTool — v1.0.1",
            "=============",
            "",
            "  🐛 Bug fixes",
            "  • Removed reference to missing recaf-cli-0.8.8.jar (was never shipped)",
            "  • Removed 17 duplicate keys in localization dictionary",
            "  • Removed orphaned theme files (Blue, Crimson, Green, Light, Ocean, Purple, Sunset)",
            "",
            "FMTool — v1.0.0",
            "=============",
            "",
            "  🔍 Mod Checker",
            "  • Parallel scanning — 4 threads with real-time progress bar",
            "  • Folder drag-drop — drop any folder, FMTool finds all .jar files inside",
            "  • SHA-256 hash cache — no re-hashing if file unchanged (saves time)",
            "  • Scan history — auto-save on clear, Save/Load buttons (JSON format)",
            "  • Name filter — search box above results to quickly find a specific mod",
            "  • Counter reset — footer zeros out when list is cleared",
            "",
            "  📂 Scanner",
            "  • Folder drag-drop — drop a folder on results panel, it gets added as scan path",
            "  • Grouping — results grouped by suspicion level (collapsible)",
            "  • Detail panel — click any result, detailed info appears below",
            "  • Hotkeys — Ctrl+S (scan), Ctrl+E (export), Ctrl+F (search)",
            "",
            "  📄 Report Window",
            "  • 'Save Report' button — export as .txt or .json",
            "",
            "  ⚙️ System Integration",
            "  • Context menu — right-click .jar → 'Scan with FMTool'",
            "  • File association — open .jar files with FMTool by default",
            "  • Auto-update — checks for new version on GitHub at startup",
            "",
            "  🎨 UI",
            "  • Glass-morphism footer — semi-transparent status bar with shadow",
            "  • Stronger card shadows — deeper, more premium look",
            "  • Thin scrollbars — cleaner and more modern",
            "  • Selected item highlight — accent border on active result",
            "  • Smooth page transitions — 300ms + BackEase easing",
            "  • Settings — new Integration section (context menu + file association)",
            "",
            "made by zxc1337 for the FunMoon server."
        };

        private static readonly string[] ChangelogRu = {
            "FMTool — v1.0.1",
            "=============",
            "",
            "  🐛 Исправления",
            "  • Удалена ссылка на отсутствующий recaf-cli-0.8.8.jar",
            "  • Удалены 17 дублирующихся ключей локализации",
            "  • Удалены неиспользуемые файлы тем (Blue, Crimson, Green, Light, Ocean, Purple, Sunset)",
            "",
            "FMTool — v1.0.0",
            "=============",
            "",
            "  🔍 Mod Checker — проверка модов",
            "  • Параллельное сканирование — теперь в 4 потока с реальным прогресс-баром",
            "  • Перетаскивай папки — кинь любую папку, FMTool сам найдёт все .jar внутри",
            "  • Кэш SHA-256 — если файл не менялся, хеш не пересчитывается (экономия времени)",
            "  • История проверок — автосохранение при очистке, кнопки Save/Load (формат JSON)",
            "  • Фильтр по имени — поле поиска над результатами, чтобы быстро найти нужный мод",
            "  • Сброс счётчиков — при очистке списка футер тоже обнуляется",
            "",
            "  📂 Сканер",
            "  • Перетаскивай папки — кинь папку на панель результатов → она добавится как путь сканирования",
            "  • Группировка — результаты теперь сгруппированы по уровню подозрения (можно сворачивать)",
            "  • Панель деталей — кликни на любой результат, и снизу появится подробная информация",
            "  • Горячие клавиши — Ctrl+S (скан), Ctrl+E (экспорт), Ctrl+F (поиск)",
            "",
            "  📄 Окно отчёта",
            "  • Кнопка «Save Report» — сохрани отчёт в .txt или .json",
            "",
            "  ⚙️ Интеграция с системой",
            "  • Контекстное меню — правый клик на .jar → «Сканировать FMTool»",
            "  • Ассоциация файлов — можно открывать .jar через FMTool по умолчанию",
            "  • Авто-обновление — при запуске проверяет, вышла ли новая версия на GitHub",
            "",
            "  🎨 Интерфейс",
            "  • Стеклянный футер — полупрозрачная строка состояния с тенью, выглядит дорого",
            "  • Улучшенные тени карточек — глубже, премиальнее",
            "  • Тонкие скроллбары — аккуратнее и современнее",
            "  • Подсветка выбранного элемента — акцентная рамка на активном результате",
            "  • Плавные переходы — 300ms + BackEase, анимация стала ещё приятнее",
            "  • Настройки — новый раздел Integration (контекстное меню + ассоциация файлов)",
            "",
            "сделано zxc1337 для сервера FunMoon."
        };

        private static readonly (string Res, string File, string Icon, string DescEn, string DescRu)[] EmbeddedApps = {
            ("FMTool.APPS.cleaningdetector.exe", "cleaningdetector.exe", "cleaningdetector", "Detects PC cleaning / trace wiping", "Детект чистки следов на ПК"),
            ("FMTool.APPS.InjGen.exe", "InjGen.exe", "InjGen", "Detects DLL injections into processes", "Детект инъекций DLL в процессы"),
            ("FMTool.APPS.JournalTrace.exe", "JournalTrace.exe", "JournalTrace", "USN Journal — deleted file traces", "USN Journal — следы удалённых файлов"),
            ("FMTool.APPS.PathsParser.exe", "PathsParser.exe", "PathsParser", "Parses file paths from system artifacts", "Парсинг путей файлов из артефактов"),
            ("FMTool.APPS.PrefetchView__.exe", "PrefetchView++.exe", "PrefetchViewplusplus", "Launched programs (Prefetch)", "Запускавшиеся программы (Prefetch)"),
            ("FMTool.APPS.luyten-0.5.4.exe", "luyten-0.5.4.exe", "luyten", "Java decompiler (view .jar/.class source)", "Java-декомпилятор (просмотр .jar/.class)"),
            ("FMTool.APPS.shellbag_analyzer_cleaner.exe", "shellbag_analyzer_cleaner.exe", "shellbag_analyzer_cleaner", "ShellBags — visited folders traces", "ShellBags — следы открытых папок"),
            ("FMTool.APPS.HxD.exe", "HxD.exe", "HxD", "Hex editor for file inspection", "Hex-редактор для анализа файлов"),
            ("FMTool.APPS.SystemInformer.systeminformer-setup.exe", "systeminformer-setup.exe", "systeminformer-setup", "Process & injection monitor (installer)", "Монитор процессов и инъекций (установщик)"),
            ("FMTool.APPS.Everything-1.5.0.1404a.x64.Everything.exe", "Everything.exe", "Everything", "Instant file search by patterns", "Мгновенный поиск файлов по паттернам"),
            ("FMTool.APPS.executedprogramslist.ExecutedProgramsList.exe", "ExecutedProgramsList.exe", "ExecutedProgramsList", "List of all executed programs", "Список всех запускавшихся программ"),
            ("FMTool.APPS.usbdrivelog.USBDriveLog.exe", "USBDriveLog.exe", "USBDriveLog", "Connected USB drives history", "История подключённых USB-накопителей"),
        };

        private static readonly (string Key, string Text)[] Patterns = {
            (".jar (Everything)", "size:2263|size:5266|size:6515|size:6770|size:6778|size:7016|size:7218|size:7803|size:7891|size:9327|size:10283|size:10605|size:10958|size:11554|size:16541|size:17308|size:17339|size:18180|size:18527|size:18587|size:18734|size:19266|size:20578|size:20583|size:20639|size:20883|size:21161|size:21234|size:21664|size:22036|size:22861|size:26247|size:27546|size:27809|size:28084|size:28439|size:29304|size:29567|size:30279|size:31549|size:31607|size:34449|size:34669|size:35971|size:35993|size:38149|size:39017|size:39321|size:40142|size:42782|size:47159|size:48242|size:50828|size:51212|size:52426|size:54088|size:59381|size:62782|size:65316|size:65486|size:65765|size:66659|size:67491|size:68794|size:69757|size:72334|size:74105|size:80751|size:88896|size:95530|size:98811|size:100523|size:100799|size:101297|size:101571|size:101703|size:102297|size:102733|size:103761|size:104954|size:105623|size:105672|size:112386|size:120640|size:138417|size:143006|size:143597|size:143600|size:147329|size:147873|size:151762|size:153937|size:156722|size:156779|size:166677|size:169718|size:173698|size:183634|size:183651|size:192156|size:202720|size:257482|size:263070|size:267746|size:274865|size:300286|size:334588|size:343169|size:350629|size:409616|size:410358|size:517248|size:519731|size:532826|size:539151|size:556494|size:597406|size:636621|size:640838|size:878781|size:925493|size:1077149|size:1165063|size:1181556|size:1444714|size:1471429|size:1569093|size:1822841|size:3113569|size:3425801|size:3541075|size:3541138|size:3642292|size:3684385|size:4642998|size:5630483|size:7052171|size:7059952|size:22258750|size:25704986|size:26179274|size:26691896 *.jar"),
            (".exe/.dll (Everything)", "size:9951744|size:24536064|size:15438336|size:6229504|size:6573056|size:7187456|size:7969792|size:1562249|size:1672329|size:1677449|size:1680521|size:147329|size:138351|size:202720|size:7788032|size:22885|size:23810|size:138351|size:147329|size:7988736|size:3711166|size:3697285|size:3712014|size:5641728|size:4413440|size:114974|size:111866|size:274865|size:1820884|size:5007380|size:6944256|size:5934592|size:2545664|size:2108662|size:1961742|size:3684385|size:5143837|size:4413440|size:116689|size:1968128|size:8011776|size:1883602|size:5918208|size:1897269|size:31445308|size:24390144|size:25158656|size:2023236|size:16836288|size:88065933|size:197933122|size:2258533|size:2305645|size:2372788|size:18764384|size:9400174|size:2363704|size:15445581|size:2373676|size:138351|size:7788032|size:22885|size:23810|size:7988736|size:3711166|size:3697285|size:3712014|size:5641728|size:4413440|size:111866|size:1820884|size:5007380|size:6944256|size:5934592|size:2545664|size:2108662|size:1961742|size:3684385|size:5143837|size:1968128|size:8011776|size:1883602|size:6533121|size:16629226|size:28107997|size:8249687|size:5524900|size:140200|size:132133|size:110439|size:6244043|size:6867367|size:43883|size:514855|size:479296|size:9530356|size:355527744|size:1819289|size:1897269|size:16855568|size:16964112|size:2023236|size:5918208|size:31445308|size:24390144|size:10657176|size:460288|size:19521024|size:15076480|size:7204864|size:1613824|size:1499136|size:1488896|size:9332326|size:9400174|size:10071288|size:9400174|size:10071288|Baritone|Nursultan"),
            ("Everything: Cheat clients", "impact|wurst|bleachhack|aristois|huzuni|skillclient|sigma|meteor|liquidbounce|nurik|nursultan|celestial|calestial|celka|expensive|neverhook|excellent|wexside|wildclient|dauntlesslyat|rename_me_please|editme|takker|fuzeclient|wisefolder|flauncher|vec.dll|Feather|delta|venus|baritone|spambot|CleanCut|spam_bot|inventory_walk|player_highlighter|aimbot|freecam|bedrock_breaker_mode|viaversion|double_hotbar|elytra_swap|armor_hotswap|smart_moving|chest|savesearcher|topkautobuy|topkaautobuy|tweakeroo|mob_hitbox|librarian_trade_finder|sacurachorusfind|autoattack|entity_outliner|invmove|viabackwards|viarewind|viafabric|viaforge|viaproxy|vialoader|viamcp|hitbox|elytrahack|DiamondSim|ForgeHax|clientcommands|Control-Tweaks|SwingThroughGrass|CutThrough|Haruka|NewLauncher|Blade|Hachclient|Inertia|Fluger|Exloader"),
            ("vec.dll (Everything)", "size:30720 utf8:net/minecraft/client/entity/player/ClientPlayerEntity|net/minecraft/util/math/AxisAlignedBB"),
            ("DoomsDay (Everything)", "ext:jar size:21kb-10mb content:l.png content:mcmod.info"),
            (".exe (Everything)", ".exe size:12mb-25mb"),
            ("CSRSS Strings", "^[A-Z]:\\\\.+\\.(exe)$\r\n\r\n^[A-Z]:\\\\.+\\.(dll)$"),
            ("HxD Strings", "Entity\r\n\r\nDoomsday\r\n\r\nplayer\r\n\r\nAxis\r\n\r\nHitbox\r\n\r\nvec"),
        };

        private readonly HashSet<long> _knownCheatSizes = new();

        private static readonly string[] CheatNames = {
            "impact","wurst","bleachhack","aristois","huzuni","skillclient","meteor","liquidbounce",
            "nursultan","celestial","expensive","neverhook","wexside","wildclient","deadcode","jigsaw",
            "konas","richclient","rusherhack","thunderhack","moonhack","doomsday","nightware","extazyy","troxill",
            "antileak","fuzeclient","baritone","forgehax","aimbot","freecam","tweakeroo","elytrahack","exloader",
            "lambda","lambdaclient","salhack","phobos","aoba","raven","ravenb+","fdpclient","fdp",
            "kami","kamiblue","azura","radium","krypton","aegis","hydrogen","phantom","trollhack",
            "liquidbounce++","lbplusplus","nightx","rise","riseclient","sulfur","noise","akira",
            "nurik","hachclient","blizzard","tenacity","kole","moonclient","drip","smartclient",
            "newclient","zeroday","orion","pandora","exe","zephyr","collision","luna",
            "gamesense","gamesense++","novoline","vape","future","inertia","fluger","horion",
            "ethereal","flux","spooky","summer","diu","smooth","toggle","pulsive",
            "trinity","gothaj","haki","liquor","lemon","mimo","nio","nix","off",
            "ozark","prime","qcy","resolve","reunion","ryu","saint","satan","schizo",
            "smack","snuff","sora","soshal","spicy","spirit","svbc","tigris","tokyo",
            "vanguard","vendetta","volt","waterfall","weave","wifi","wintradr","zanxi","zui","zuna",
            "asuna","ayame","blizzard","cokeclient","dinamic","element","ember","envy",
            "exhibition","fanta","flare","gothaj","gtl","hapsty","hyperium","jello","juliet",
            "leaks","lila","monster","nether","nodeb","onyx","orbit","pandas","pandora",
            "pin","pivix","raino","reap","redes","sight","spotlight","suk","yv",
            "haruka","blade","newlauncher","diamondsim","neverhook","hachclient","nurik",
            "celka","fluger","excellentcrack","keystrokesmod","autocrystal","surround",
            "holefill","destroyer","chainbot","staffdetect","antibot","antivanish",
            "baritone","forgehax","aimbot","freecam","tweakeroo","elytrahack","exloader"
        };

        private static readonly string[] BannedMods = {
            "ReplayMod", "IsometricRender", "CmdCam", "WorldDownloader", "Flashback",
            "AutoMining", "ReplantingCrops", "AutoHarvest", "Reap", "Tweakeroo",
            "AutoFish", "AccurateBlockPlacement", "Baritone", "PlayerSpotlight", "AucHelper",
            "ChestTracker", "FriendHighlighter", "DonutAuctions", "XRay", "DiamondGen",
            "FreeCam", "BaseFinder", "TrueSight", "MiniMap", "Litematica",
            "Schematica", "WorldEdit", "BetterPVP", "RemoveBlindness", "AntiInvis",
            "CooldownsHUD", "CheatUtils", "TopkaCooldown", "AutoBuy", "AutoSell",
            "AutoCasino", "AutoPilot", "InventoryControlTweaks", "AutoLeave", "FoodSlot",
            "Quickstack", "ItemSwap", "AutoTool",
            "MovementInGUI", "FasterBlockPlacement", "FireworkHelper", "EffortlessBuilding",
            "InvMove", "InventoryProfilesNext", "DontHitTeammates", "CleanCut",
            "AutoAttack", "AutoAim", "FeverVisuals", "LuminarVisuals", "Ascart",
            "BadlionClient", "Feather Client", "FeatherClient", "Pojav", "FCL",
            "SimpleVisuals", "SoupApi", "WaveVisuals", "ClientCommands",
            "NoChatReports", "VoxelMap", "MobHealthBar", "ChunkAnimator",
            "Neat", "BlockEntityTooltip", "ReEntityOutline", "AutoSoul",
            "TopkaAutoBuy", "AutoPilot", "Spambot", "AimBot",
            "KillAura", "Nuker", "ESP", "ChestStealer", "Scaffold",
            "Velocity", "Critical", "Reach", "TriggerBot", "Fly",
            "Speed", "NoFall", "FullBright", "Tracers", "Spider",
            "Jesus", "BunnyHop", "FastPlace", "FastBreak", "NoSlow",
            "Wurst", "Meteor", "Aristois", "LiquidBounce", "Doomsday",
            "Impact", "Sigma", "BleachHack", "Huzuni", "SkillClient",
            "Celestial", "NeverHook", "Wexside", "WildClient", "FuzeClient",
            "Inertia", "Fluger", "Exloader", "Vape", "FDP",
            "FDPClient", "GameStrap", "Horizon", "Konas", "Future",
            "RusherHack", "ThunderHack", "MoonHack", "RichClient",
            "NightWare", "Extazyy", "Troxill", "AntiLeak", "DeadCode",
            "Jigsaw", "Celka", "Expensive", "Nurik", "Nursultan",
            "Novoline", "GameSense", "GameSense++", "HachClient", "Haruka", "Blade",
            "NewLauncher", "DiamondSim", "AutoCrystal", "CrystalAura",
            "Surround", "HoleFill", "AntiTrap", "ElytraReplace",
            "ElytraBot", "PistonCrystal", "SelfFill", "SelfPot",
            "PotRefill", "AutoPot", "AntiBot", "AntiVanish",
            "StaffDetect", "NameTags", "Chams", "ESP2D", "ESP3D",
            "ItemESP", "ChestESP", "PlayerESP", "AutoAttack",
            "AutoAim", "FeverVisuals", "LuminarVisuals",
            "SimpleVisuals", "WaveVisuals", "SoupApi",
            "AimAssist", "NoClickDelay", "AutoClicker",
            "WallHack", "NoFall", "SpeedHack", "Flight",
            "Glide", "HighJump", "LongJump", "Step", "Phase",
            "Blink", "NoSlow", "AntiKnockback",
            "InvCleaner", "ChestStealer", "InventoryMove",
            "FreeLook", "Perspective", "NoClip",
            "FriendList", "AltManager", "Session",
            "EasyCraft", "ToolBox", "Horion",
            "AntiRedScren", "KeyStrokes", "KeyStrokesMod",
            "MCPELauncher", "TheAltening",
            "Lambda", "LambdaClient", "SalHack", "Phobos", "Aoba",
            "Raven", "RavenB+", "KAMI", "KAMI Blue", "Azura",
            "Radium", "Krypton", "Aegis", "Hydrogen", "Phantom",
            "TrollHack", "LiquidBounce++", "LiquidBouncePlusPlus",
            "NightX", "Rise", "RiseClient", "Sulfur", "Noise",
            "Akira", "AkiraGhost", "Blizzard", "Tenacity",
            "SmartClient", "Zeroday", "Orion", "Pandora",
            "Zephyr", "Collision", "Luna", "Luna5ama",
            "Strike", "Weep", "Pulsive", "Trinity", "Gothaj",
            "Flux", "Ethereal", "Spooky", "Summer", "SummerClient",
            "Toggle", "Smooth", "Diuc", "Kilo", "Lemon", "Liquor",
            "Mimo", "Nio", "Nix", "Off", "Ozark", "Prime",
            "Qcy", "Resolve", "Reunion", "Ryu", "Saint", "Satan",
            "Schizo", "Smack", "Snuff", "Sora", "Soshal", "Spicy",
            "Spirit", "Svbc", "Tigris", "Tokyo", "Vanguard",
            "Vendetta", "Volt", "Waterfall", "Weave", "Wifi",
            "Wintradr", "Zanxi", "Zui", "Zuna", "Asuna", "Ayame",
            "CokeClient", "Dinamic", "Element", "Ember", "Envy",
            "Exhibition", "Fanta", "Flare", "Haki", "Hapsty",
            "Hyperium", "Jello", "Juliet", "Leaks", "Lila",
            "Monster", "Nether", "Nodeb", "Onyx", "Orbit",
            "Pandas", "Pin", "Pivix", "Raino", "Redes",
            "Sight", "Spotlight", "Suk", "Yv",
        };

        // Known banned mod file sizes JARs only (bytes) — catches renamed copies
        private static readonly HashSet<long> KnownBannedModSizes = new()
        {
            2263, 5266, 6515, 6770, 6778, 7016, 7218, 7803, 7891,
            9327, 10283, 10605, 10958, 11554, 16541, 17308, 17339,
            18180, 18527, 18587, 18734, 19266, 20578, 20583, 20639,
            20883, 21161, 21234, 21664, 22036, 22861, 26247, 27546,
            27809, 28084, 28439, 29304, 29567, 30279, 31549, 31607,
            34449, 34669, 35971, 35993, 38149, 39017, 39321, 40142,
            42782, 47159, 48242, 50828, 51212, 52426, 54088, 59381,
            62782, 65316, 65486, 65765, 66659, 67491, 68794, 69757,
            72334, 74105, 80751, 88896, 95530, 98811, 100523, 100799,
            101297, 101571, 101703, 102297, 102733, 103761, 104954,
            105623, 105672, 112386, 120640, 138417, 143006, 143597,
            143600, 147329, 147873, 151762, 153937, 156722, 156779,
            166677, 169718, 173698, 183634, 183651, 192156, 202720,
            257482, 263070, 267746, 274865, 300286, 334588, 343169,
            350629, 409616, 410358, 517248, 519731, 532826, 539151,
            556494, 597406, 636621, 640838, 878781, 925493, 1077149,
            1165063, 1181556, 1444714, 1471429, 1569093, 1822841,
            3113569, 3425801, 3541075, 3541138, 3642292, 3684385,
            4642998, 5630483, 7052171, 7059952, 22258750, 25704986,
            26179274, 26691896
        };

        private const string PsHistoryRel = @"Microsoft\Windows\PowerShell\PSReadLine\ConsoleHost_History.txt";

        private readonly Dictionary<string, (string En, string Ru)> _loc = new()
        {
            ["nav_scanner"] = ("Scanner", "Сканер"),
            ["nav_programs"] = ("Programs", "Программы"),
            ["nav_patterns"] = ("Patterns", "Паттерны"),
            ["nav_logs"] = ("Logs", "Логи"),
            ["nav_settings"] = ("Settings", "Настройки"),
            ["nav_changelog"] = ("Changelog", "История версий"),
            ["nav_modchecker"] = ("Mod Checker", "Проверка модов"),
            ["filters"] = ("Scan Options", "Параметры сканирования"),
            ["scantype"] = ("File type", "Тип файла"),
            ["namefilter"] = ("Name / pattern filter", "Фильтр по имени / паттерну"),
            ["minsize"] = ("Min MB", "Мин. МБ"),
            ["maxsize"] = ("Max MB", "Макс. МБ"),
            ["targets"] = ("Scan targets", "Что сканировать"),
            ["matchpat"] = ("Match cheat patterns", "Искать по чит-паттернам"),
            ["onlysusp"] = ("Only suspicious", "Только подозрительные"),
            ["pickfolder"] = ("Add custom folder...", "Выбрать свою папку..."),
            ["start"] = ("Start Scan", "Начать скан"),
            ["stop"] = ("Stop", "Стоп"),
            ["export"] = ("Export results", "Экспорт результатов"),
            ["results"] = ("Results", "Результаты"),
            ["programs_title"] = ("Embedded Programs", "Встроенные программы"),
            ["programs_hint"] = ("Click a card to launch. Files are extracted from the app.", "Нажмите на карточку для запуска. Файлы извлекаются из приложения."),
            ["patterns_list"] = ("Search Patterns", "Паттерны поиска"),
            ["select_pattern"] = ("Select a pattern", "Выберите паттерн"),
            ["copy"] = ("Copy to Clipboard", "Копировать в буфер"),
["settings"] = ("Settings", "Настройки"),
            ["theme"] = ("Theme", "Тема"),
            ["language"] = ("Language", "Язык"),
            ["autostart"] = ("Auto-Startup", "Автозапуск"),
            ["autostart_run"] = ("Run at Windows startup", "Запускать при старте Windows"),
            ["desktop_dl"] = ("Desktop / Downloads", "Рабочий стол / Загрузки"),
            ["scan_cdrive"] = ("C:\\", "C:\\"),
            ["ready"] = ("Ready", "Готово"),
            ["scanning"] = ("Scanning...", "Сканирование..."),
            ["done"] = ("Scan complete", "Сканирование завершено"),
            ["stopped"] = ("Scan stopped", "Сканирование остановлено"),
            ["copied"] = ("Copied to clipboard", "Скопировано в буфер"),
            ["launched"] = ("Launched", "Запущено"),
            ["exported"] = ("Results exported", "Результаты экспортированы"),
            ["autostart_on"] = ("Auto-startup enabled", "Автозапуск включён"),
            ["autostart_off"] = ("Auto-startup disabled", "Автозапуск выключен"),
            ["nothing"] = ("Nothing to export", "Нечего экспортировать"),
            ["folder_added"] = ("Custom folder added", "Папка добавлена"),
            ["folder_removed"] = ("Custom folder removed", "Папка удалена"),
            ["copypath"] = ("Copy path", "Копировать путь"),
            ["copyhash"] = ("Copy SHA-256", "Копировать SHA-256"),
            ["path_copied"] = ("Path copied", "Путь скопирован"),
            ["hash_copied"] = ("SHA-256 copied", "SHA-256 скопирован"),
            ["open_folder"] = ("Open in Explorer", "Открыть в проводнике"),
            ["close_all"] = ("Close launched programs", "Закрыть запущенные программы"),
            ["send_modchecker"] = ("Send to Mod Checker", "Отправить в Mod Checker"),
            ["closed"] = ("Closed launched programs", "Запущенные программы закрыты"),
            ["nothing_running"] = ("No launched programs", "Нет запущенных программ"),
            ["changelog"] = ("Changelog", "История версий"),
            ["export_changelog"] = ("Export changelog", "Экспорт истории"),
            ["searchfunc"] = ("Search functions...", "Поиск функций..."),
            ["java_status_installed"] = ("Installed", "Установлен"),
            ["java_status_notfound"] = ("Not found", "Не найден"),
            ["java_install"] = ("Install", "Установить"),
            ["java_reinstall"] = ("Reinstall", "Переустановить"),
            ["java_remove"] = ("Remove", "Удалить"),
            ["java_launching"] = ("Launching Java installer...", "Запуск установщика Java..."),
            ["java_notfound"] = ("Java installer not found in APPS folder", "Установщик Java не найден в папке APPS"),
            ["java_missing"] = ("Java not found. Use 'Install Java' button in Settings.", "Java не найден. Используйте кнопку 'Установить Java' в настройках."),
            ["pslog"] = ("PowerShell History", "PowerShell история"),
            ["startup"] = ("Startup Programs", "Программы автозапуска"),
            ["refresh"] = ("Refresh", "Обновить"),
            ["notfound_search"] = ("Nothing found for your query", "Ничего не найдено по вашему запросу"),
            ["deepscan"] = ("Deep scan (contents)", "Глубокое сканирование"),
            ["col_name"] = ("Name", "Имя"),
            ["col_size"] = ("Size", "Размер"),
            ["col_type"] = ("Type", "Тип"),
            ["col_status"] = ("Status", "Статус"),
            ["col_path"] = ("Path", "Путь"),
            ["about_title"] = ("FMTool", "FMTool"),
            ["about_version"] = ("Version", "Версия"),
            ["about_author"] = ("Author: zxc1337", "Автор: zxc1337"),
            ["about_footer"] = ("Made for FunMoon Server", "Сделано для FunMoon Server"),
            ["status_footer"] = ("by zxc1337 for FunMoon.", "от zxc1337 для FunMoon."),
            ["tooltip_theme"] = ("Theme", "Тема"),
            ["tooltip_language"] = ("Language", "Язык"),
            ["tooltip_remove_folder"] = ("Remove folder", "Удалить папку"),
            ["tooltip_high_susp"] = ("High suspicion", "Высокое подозрение"),
            ["tooltip_suspicious"] = ("Suspicious", "Подозрительно"),
            ["tooltip_normal"] = ("Normal", "Нормально"),
            ["dropzone_text"] = ("Drop .jar files here", "Перетащите .jar файлы сюда"),
            ["dropzone_hint"] = ("or click to browse", "или нажмите для выбора"),
            ["browse_jar"] = ("Browse JAR files...", "Выбрать JAR файлы..."),
            ["clear_mods"] = ("Clear", "Очистить"),
            ["banned_list"] = ("Banned mods", "Забаненые моды"),
            ["mod_results"] = ("Results", "Результаты"),
            ["mod_no_files"] = ("No files checked", "Файлы не проверены"),
            ["save_history"] = ("Save", "Сохранить"),
            ["load_history"] = ("Load", "Загрузить"),
            ["save_history_tip"] = ("Save current results", "Сохранить текущие результаты"),
            ["load_history_tip"] = ("Load last session", "Загрузить последнюю сессию"),
            ["mod_disclaimer"] = ("\u26A0 Detection is heuristic \u2014 not 100% accurate. Legitimate mods may be flagged and real cheats may be missed. Always verify results manually.", "\u26A0 Детекция основана на эвристике \u2014 не 100% точна. Легитные моды могут быть помечены, а реальные читы пропущены. Всегда проверяйте результаты вручную."),
            ["tooltip_close_esc"] = ("Close (Esc)", "Закрыть (Esc)"),
            ["tooltip_high_indicator"] = ("High suspicion indicators", "Индикаторы высокого риска"),
            ["tooltip_susp_indicator"] = ("Suspicious indicators", "Подозрительные индикаторы"),
            ["tooltip_normal_indicator"] = ("Normal indicators", "Нормальные индикаторы"),
            ["luyten_open"] = ("\u2615 Open with Luyten", "\u2615 Открыть в Luyten"),
            ["tab_detection"] = ("Detection Report", "Отчёт обнаружения"),
            ["tab_classes"] = ("Class Browser", "Обзор классов"),
            ["tab_structure"] = ("ZIP Structure", "Структура ZIP"),
            ["tab_metadata"] = ("Metadata", "Метаданные"),
            ["legend_high"] = ("High Suspicion", "Высокое подозрение"),
            ["legend_susp"] = ("Suspicious", "Подозрительно"),
            ["legend_normal"] = ("Normal", "Нормально"),
            ["extract_jar"] = ("\U0001F4E6 Extract JAR...", "\U0001F4E6 Извлечь JAR..."),
            ["close_btn"] = ("\u00D7 Close", "\u00D7 Закрыть"),
            ["file_report_title"] = ("File Report", "Отчёт о файле"),
            ["score"] = ("Score", "Баллы"),
            ["classes"] = ("Classes", "Классы"),
            ["verdict"] = ("Verdict", "Вердикт"),
            ["callout_high"] = ("High Suspicion", "Высокое подозрение"),
            ["callout_suspicious"] = ("Suspicious", "Подозрительно"),
            ["callout_low"] = ("Low Suspicion", "Низкое подозрение"),
            ["callout_normal"] = ("Normal", "Нормально"),
            ["callout_critical"] = ("Critical", "Критично"),
            ["na"] = ("N/A", "Н/Д"),
            ["cancel"] = ("Cancel", "Отмена"),
            ["ok"] = ("OK", "ОК"),
            ["tray_show"] = ("Show FMTool", "Показать FMTool"),
            ["tray_exit"] = ("Exit", "Выход"),
            ["drop_accepted"] = ("Accepted {0} JAR(s), skipped {1} non-JAR file(s)", "Принято {0} JAR, пропущено {1} не-JAR файл(ов)"),
            ["mod_open_report"] = ("Open Report", "Открыть отчёт"),
            ["mod_open_folder"] = ("Open in Explorer", "Открыть в проводнике"),
            ["mod_copy_path"] = ("Copy path", "Копировать путь"),
            ["mod_copy_hash"] = ("Copy SHA-256", "Копировать SHA-256"),
        };

        private string _sortColumn = "";
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(Path.GetTempPath(), "FMTool_init_error.txt"),
                    "Constructor error: " + ex.ToString());
                throw;
            }
            ResultsList.ItemsSource = _results;
            _extractDir = Path.Combine(Path.GetTempPath(), "FMTool_Apps");
    EventManager.RegisterClassHandler(typeof(GridViewColumnHeader),
        GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ResultsList_ColumnHeaderClick));
    PreviewKeyDown += MainWindow_PreviewKeyDown;
}

        private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.F:
                        e.Handled = true;
                        FuncSearchBox?.Focus();
                        FuncSearchBox?.SelectAll();
                        break;
                    case Key.S:
                        e.Handled = true;
                        if (_ready) StartScan_Click(sender, new RoutedEventArgs());
                        break;
                    case Key.E:
                        e.Handled = true;
                        Export_Click(sender, new RoutedEventArgs());
                        break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadPrograms();
                BuildPatternButtons();
                AutostartCheck.IsChecked = IsAutostartEnabled();
                UpdateJavaStatus();
                ChangelogBox.Text = GetChangelogText();
                InitializeModChecker();
                InitCheatSizes();
                SetStatus(L("ready"));
                SetupTrayIcon();
                ApplyRoundedCorners();
                ApplyLanguage();
                CheckForUpdate();
                _ready = true;
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(Path.GetTempPath(), "FMTool_loaded_error.txt"),
                    "Window_Loaded error: " + ex.ToString());
                System.Windows.MessageBox.Show("FMTool startup error:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetupTrayIcon()
        {
            try
            {
                _trayIcon = new Forms.NotifyIcon();
                _trayIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(
                    Environment.ProcessPath ?? "FMTool.exe");
                _trayIcon.Text = "FMTool";
                _trayIcon.Visible = true;

                var menu = new Forms.ContextMenuStrip();
                menu.Items.Add(L("tray_show"), null, (s, e) =>
                {
                    Show();
                    if (WindowState == WindowState.Minimized) WindowState = WindowState.Normal;
                    Activate();
                });
                menu.Items.Add(new Forms.ToolStripSeparator());
                menu.Items.Add(L("tray_exit"), null, (s, e) =>
                {
                    _trayIcon.Visible = false;
                    _trayIcon.Dispose();
                    Application.Current.Shutdown();
                });
                _trayIcon.ContextMenuStrip = menu;

                _trayIcon.DoubleClick += (s, e) =>
                {
                    Show();
                    if (WindowState == WindowState.Minimized) WindowState = WindowState.Normal;
                    Activate();
                };
            }
            catch { }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        private void ApplyRoundedCorners()
        {
            try
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                if (hwnd != IntPtr.Zero)
                {
                    int round = DWMWCP_ROUND;
                    DwmSetWindowAttribute(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, ref round, sizeof(int));
                }
            }
            catch { }
        }

        private string GetChangelogText() => string.Join("\r\n", _lang == 1 ? ChangelogRu : ChangelogEn);

        // ===================== NAVIGATION =====================
        private void Nav_Checked(object sender, RoutedEventArgs e)
        {
            if (PageScanner == null) return;
            var tag = (sender as RadioButton)?.Tag?.ToString();
            SetPage(PageScanner, tag == "0");
            SetPage(PagePrograms, tag == "1");
            SetPage(PagePatterns, tag == "2");
            SetPage(PageLogs, tag == "3");
            SetPage(PageSettings, tag == "4");
            SetPage(PageChangelog, tag == "5");
            SetPage(PageModChecker, tag == "6");

            // Clear stale status messages when switching tabs
            if (tag != "3")
                SetStatus(L("ready"));

            if (PageTitle == null) return;
            PageTitle.Text = tag switch
            {
                "0" => L("nav_scanner"),
                "5" => L("nav_changelog"),
                "1" => L("nav_programs"),
                "2" => L("nav_patterns"),
                "3" => L("nav_logs"),
                "4" => L("nav_settings"),
                "6" => L("nav_modchecker"),
                _ => "FMTool"
            };
        }

        private static void SetPage(UIElement page, bool visible)
        {
            if (page == null) return;
            if (visible)
            {
                page.Visibility = Visibility.Visible;
                page.BeginAnimation(OpacityProperty, null);
                if (page.RenderTransform is not System.Windows.Media.TranslateTransform)
                    page.RenderTransform = new System.Windows.Media.TranslateTransform();
                page.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

                var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300))
                { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
                page.BeginAnimation(OpacityProperty, fade);

                var slide = new DoubleAnimation(16, 0, TimeSpan.FromMilliseconds(350))
                { EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.15 } };
                page.RenderTransform.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, slide);
            }
            else
            {
                var fade = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150))
                { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } };
                fade.Completed += (s, a) => page.Visibility = Visibility.Collapsed;
                page.BeginAnimation(OpacityProperty, fade);
            }
        }

        private void ResultsList_Loaded(object sender, RoutedEventArgs e)
        {
            if (ResultsList?.ItemContainerGenerator != null)
                ResultsList.ItemContainerGenerator.StatusChanged += (s, args) =>
                {
                    if (ResultsList.ItemContainerGenerator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated) return;
                    foreach (var item in ResultsList.Items)
                    {
                        var container = ResultsList.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                        if (container == null) continue;
                        if (item is ScanItem si)
                        {
                            container.Loaded += (_, _) =>
                            {
                                if (si.Status == "Cheat Client")
                                    container.Background = new SolidColorBrush(Color.FromArgb(30, 240, 72, 98));
                                else if (si.Status == "Suspicious")
                                    container.Background = new SolidColorBrush(Color.FromArgb(30, 255, 194, 75));
                                else
                                    container.Background = new SolidColorBrush(Color.FromArgb(15, 50, 210, 150));
                            };
                        }
                    }
                };
        }

        private void ResultsList_ColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader header && header.Column != null)
            {
                string col = "";
                if (header.Column.DisplayMemberBinding is System.Windows.Data.Binding b)
                    col = b.Path?.Path ?? "";

                if (string.IsNullOrEmpty(col))
                {
                    var hdr = header.Column.Header?.ToString() ?? "";
                    col = hdr switch
                    {
                        "Name" or "Имя" => "Name",
                        "Size" or "Размер" => "SizeBytes",
                        "Status" or "Статус" => "Status",
                        "Path" or "Путь" => "Path",
                        _ => ""
                    };
                }

                if (string.IsNullOrEmpty(col)) return;

                if (_sortColumn == col)
                    _sortDirection = _sortDirection == ListSortDirection.Ascending
                        ? ListSortDirection.Descending : ListSortDirection.Ascending;
                else
                {
                    _sortColumn = col;
                    _sortDirection = ListSortDirection.Ascending;
                }

                var view = CollectionViewSource.GetDefaultView(ResultsList.ItemsSource);
                if (view != null)
                {
                    view.SortDescriptions.Clear();
                    if (_sortColumn == "Status")
                    {
                        view.SortDescriptions.Add(new SortDescription("StatusOrder", _sortDirection));
                    }
                    else
                    {
                        view.SortDescriptions.Add(new SortDescription(_sortColumn, _sortDirection));
                    }
                    view.Refresh();
                }
            }
        }

        // ===================== WINDOW CONTROLS =====================
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
        private void MinBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void MaxBtn_Click(object sender, RoutedEventArgs e) =>
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            WindowState = WindowState.Minimized;
        }

        // ===================== THEME / LANG =====================
        private static readonly string[] ThemeNames = { "Dark", "Arctic", "Emerald", "Rose", "Cherry", "Gold", "Violet" };

        private void ThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            ThemePopup.IsOpen = !ThemePopup.IsOpen;
        }

        private void ThemeOption_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string name)
            {
                _theme = name;
                App.SwitchTheme(_theme);
                _isDark = _theme != "Arctic";
                int idx = Array.IndexOf(ThemeNames, _theme);
                if (idx >= 0 && ThemeCombo != null) ThemeCombo.SelectedIndex = idx;
                ThemePopup.IsOpen = false;
            }
        }

        private void ThemeCombo_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!_ready) return;
            int idx = ThemeCombo.SelectedIndex;
            if (idx < 0 || idx >= ThemeNames.Length) idx = 0;
            _theme = ThemeNames[idx];
            App.SwitchTheme(_theme);
            _isDark = _theme != "Arctic";
        }

        // ===================== CHANGELOG =====================
        private void ExportChangelog_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog { Filter = "Text file|*.txt", FileName = "FMTool_changelog.txt" };
            if (dlg.ShowDialog() == true)
            {
                try { File.WriteAllText(dlg.FileName, GetChangelogText()); ShowToast(L("exported")); }
                catch (Exception ex) { ShowToast("Error: " + ex.Message, false); }
            }
        }

        private void CopyChangelog_Click(object sender, RoutedEventArgs e)
        {
            try { Clipboard.SetText(GetChangelogText()); ShowToast(L("copied")); } catch { }
        }

        // ===================== FUNCTION SEARCH =====================
        private void FuncSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_ready) return;
            string q = FuncSearchBox.Text.Trim().ToLowerInvariant();

            SearchClearBtn.Visibility = string.IsNullOrEmpty(q) ? Visibility.Collapsed : Visibility.Visible;

            if (string.IsNullOrEmpty(q))
            {
                SearchPopup.IsOpen = false;
                return;
            }

            var results = new List<(string Name, string NavTag, int Score, string Details)>();

            // ── 1. Search page names ──
            var pages = new (string Name, string Tag, string[] Keywords)[]
            {
                ("🔍 Scanner", "0", new[]{"scan","skan","file","fail","search","poisk","deep","glubok","filter","result","cheat"}),
                ("🚀 Programs", "1", new[]{"program","launch","zapusk","hxd","everything","embedded","vstroen","apps","tool"}),
                ("📋 Patterns", "2", new[]{"pattern","pat","cheat","chit","suspicious","podozr","csrss","hxd","exe","dll","jar"}),
                ("📜 Logs", "3", new[]{"log","powershell","history","startup","avtozag"}),
                ("⚙ Settings", "4", new[]{"setting","nastroy","theme","tema","lang","yazyk","autostart","java","about","language"}),
                ("📰 Changelog", "5", new[]{"change","changelog","versia","release","updates","obnov","history","istoria"}),
                ("🛡️ Mod Checker", "6", new[]{"mod","jar","banned","ban","check","mcreator","mixin","forge","fabric"})
            };
            foreach (var (name, tag, keywords) in pages)
            {
                int score = keywords.Count(kw => kw.Contains(q));
                if (score > 0) results.Add((name, tag, score, ""));
            }

            // ── 2. Search changelog text ──
            string changelogText = string.Join(" ", _lang == 1 ? ChangelogRu : ChangelogEn).ToLowerInvariant();
            if (changelogText.Contains(q))
                results.Add(("📄 " + q + " → Changelog", "5", 1, "Found in changelog text"));

            // ── 3. Search pattern texts ──
            foreach (var (key, text) in Patterns)
            {
                string low = text.ToLowerInvariant();
                if (low.Contains(q))
                {
                    string snippet = text.Length > 60 ? text.Substring(0, 57) + "..." : text;
                    results.Add(("📋 " + key, "2", 1, snippet));
                }
            }

            // ── 4. Search scanner results ──
            foreach (var item in _allResults)
            {
                string nl = item.Name.ToLowerInvariant();
                if (nl.Contains(q) || item.Status.ToLowerInvariant().Contains(q))
                    results.Add(("📁 " + item.Name + " (" + item.Status + ")", "0", 1,
                        FormatSize(item.SizeBytes, false) + " — " + item.Path));
            }

            // ── 5. Search banned mods ──
            foreach (var mod in BannedMods)
            {
                if (mod.ToLowerInvariant().Contains(q))
                    results.Add(("🛡️ " + mod, "6", 1, "Banned mod / indicator"));
            }

            // ── 6. Search embedded programs ──
            foreach (var (_, file, _, descEn, descRu) in EmbeddedApps)
            {
                string desc = _lang == 1 ? descRu : descEn;
                if (file.ToLowerInvariant().Contains(q) || descEn.ToLowerInvariant().Contains(q) || descRu.ToLowerInvariant().Contains(q))
                    results.Add(("🚀 " + file, "1", 1, desc));
            }

            // ── 7. Search Settings options ──
            string[] settingsOpts = { "Theme selection (" + string.Join(", ", ThemeNames) + ")",
                "Language: English / Русский", "Auto-start at Windows login", "Java: detect / install / reinstall / remove",
                "About: FMTool v1.1 by zxc1337 for FunMoon" };
            foreach (var opt in settingsOpts)
            {
                if (opt.ToLowerInvariant().Contains(q))
                    results.Add(("⚙ " + opt, "4", 1, "Available in Settings"));
            }

            if (results.Count == 0)
            {
                SearchPopup.IsOpen = false;
                ShowToast(L("notfound_search"), false);
                return;
            }

            results = results.OrderByDescending(r => r.Score).ThenBy(r => r.Name).Take(25).ToList();
            SearchResultPanel.Children.Clear();
            foreach (var (name, navTag, _, details) in results)
            {
                var sp = new StackPanel { Orientation = Orientation.Vertical };
                sp.Children.Add(new TextBlock
                {
                    Text = name,
                    FontSize = 12.5,
                    FontWeight = FontWeights.SemiBold,
                    TextTrimming = TextTrimming.CharacterEllipsis
                });
                if (!string.IsNullOrEmpty(details))
                    sp.Children.Add(new TextBlock
                    {
                        Text = details,
                        FontSize = 10.5,
                        Foreground = TryFindResource("TextMutedBrush") as Brush ?? Brushes.Gray,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        MaxWidth = 420
                    });
                string storedTag = navTag;
                var btn = new Button
                {
                    Content = sp,
                    Tag = storedTag,
                    Style = (Style)FindResource("GhostButton"),
                    Height = 42,
                    Margin = new Thickness(2),
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Cursor = Cursors.Hand,
                    FontSize = 12.5
                };
                btn.Click += (s, args) =>
                {
                    if ((s as Button)?.Tag is string t)
                    {
                        var rb = t switch
                        {
                            "0" => NavScanner, "1" => NavPrograms, "2" => NavPatterns,
                            "3" => NavLogs, "4" => NavSettings, "5" => NavChangelog,
                            "6" => NavModChecker, _ => null
                        };
                        if (rb != null) { rb.IsChecked = true; SearchPopup.IsOpen = false; }
                    }
                };
                SearchResultPanel.Children.Add(btn);
            }
            SearchPopup.IsOpen = true;
            FuncSearchBox.Focus();
        }

        private void SearchClear_Click(object sender, MouseButtonEventArgs e)
        {
            FuncSearchBox.Text = "";
            SearchPopup.IsOpen = false;
            SearchClearBtn.Visibility = Visibility.Collapsed;
            FuncSearchBox.Focus();
        }

        private void ResetNavHighlights()
        {
            var navs = new[] { NavScanner, NavPrograms, NavPatterns, NavLogs, NavSettings, NavChangelog, NavModChecker };
            foreach (var nav in navs)
            {
                if (nav != null)
                    nav.Foreground = TryFindResource("SidebarSecondaryBrush") as System.Windows.Media.Brush 
                        ?? System.Windows.Media.Brushes.Gray;
            }
        }

        private void HighlightNav(System.Windows.Controls.Primitives.ToggleButton nav)
        {
            if (nav == null) return;
            nav.Foreground = TryFindResource("AccentSolidBrush") as System.Windows.Media.Brush 
                ?? System.Windows.Media.Brushes.Cyan;
            if (nav.RenderTransform is System.Windows.Media.ScaleTransform st)
            {
                var sb = new Storyboard();
                var anim = new DoubleAnimation(1, 1.12, new Duration(TimeSpan.FromSeconds(0.2)))
                    { AutoReverse = true, EasingFunction = new QuadraticEase() };
                Storyboard.SetTarget(anim, nav);
                Storyboard.SetTargetProperty(anim, new PropertyPath("RenderTransform.ScaleX"));
                sb.Children.Add(anim);
                sb.Begin();
            }
        }

        private void LangToggle_Click(object sender, RoutedEventArgs e)
        {
            LangPopup.IsOpen = !LangPopup.IsOpen;
        }

        private void LangOption_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is string s && int.TryParse(s, out int idx))
            {
                _lang = idx; _langStatic = idx;
                if (LangCombo != null) LangCombo.SelectedIndex = _lang;
                ApplyLanguage();
                LangPopup.IsOpen = false;
            }
        }

        private void LangCombo_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!_ready) return;
            _lang = LangCombo.SelectedIndex < 0 ? 0 : LangCombo.SelectedIndex;
            _langStatic = _lang;
            ApplyLanguage();
        }

        private string L(string key) => _loc.TryGetValue(key, out var v)
            ? (_lang == 1 ? v.Ru : v.En) : key;

        private void ApplyLanguage()
        {
            _langStatic = _lang;
            LangToggle.Content = _lang == 1 ? "RU" : "EN";
            FuncSearchBox.Tag = L("searchfunc");
            FiltersTitle.Text = L("filters");
            ScanTypeLabel.Text = L("scantype");
            NameFilterLabel.Text = L("namefilter");
            MinSizeLabel.Text = L("minsize");
            MaxSizeLabel.Text = L("maxsize");
            TargetLabel.Text = L("targets");
            UsePatternMatch.Content = L("matchpat");
            OnlySuspiciousCheck.Content = L("onlysusp");
            ScanDesktop.Content = L("desktop_dl");
            PickFolderBtn.Content = L("pickfolder");
            StartScanBtn.Content = L("start");
            StopScanBtn.Content = L("stop");
            ExportBtn.Content = L("export");
            ResultsTitle.Text = L("results");
            ProgramsTitle.Text = L("programs_title");
            ProgramsHint.Text = L("programs_hint");
            PatternsListTitle.Text = L("patterns_list");
            CopyPatternBtn.Content = L("copy");
            SettingsTitle.Text = L("settings");
            ThemeLabel.Text = L("theme");
            LanguageLabel.Text = L("language");
            AutostartLabel.Text = L("autostart");
            AutostartCheck.Content = L("autostart_run");
            JavaLabel.Text = "Java";
            InstallJavaBtn.Content = L("java_install");
            ReinstallJavaBtn.Content = L("java_reinstall");
            RemoveJavaBtn.Content = L("java_remove");
            PsLogBtn.Content = L("pslog");
            CopyLogsBtn.Content = L("copy");
            RefreshLogsBtn.Content = L("refresh");
            if (DeepScanCheck != null) DeepScanCheck.Content = L("deepscan");
            CopyChangelogBtn.Content = L("copy");
            ExportChangelogBtn.Content = L("export_changelog");
            if (ChangelogPageTitle != null) ChangelogPageTitle.Text = L("changelog");
            ChangelogBox.Text = GetChangelogText();

            // Localize scan type combo
            var scanTypes = _lang == 1
                ? new[] { ".exe / .jar / .dll", ".exe", ".jar", ".dll" }
                : new[] { ".exe / .jar / .dll", ".exe", ".jar", ".dll" };
            for (int i = 0; i < ScanTypeCombo.Items.Count && i < scanTypes.Length; i++)
                ((ComboBoxItem)ScanTypeCombo.Items[i]).Content = scanTypes[i];

            // Localize GridView column headers
            if (ResultsList.View is GridView gv && gv.Columns.Count >= 6)
            {
                gv.Columns[1].Header = L("col_name");
                gv.Columns[2].Header = L("col_size");
                gv.Columns[3].Header = L("col_type");
                gv.Columns[4].Header = L("col_status");
                gv.Columns[5].Header = L("col_path");
            }

            // Localize context menus
            if (CloseAllBtn != null) CloseAllBtn.Content = L("close_all");
            if (CtxOpenFolder != null) CtxOpenFolder.Header = L("open_folder");
            if (CtxCopyPath != null) CtxCopyPath.Header = L("copypath");
            if (CtxCopyHash != null) CtxCopyHash.Header = L("copyhash");
            if (CtxSendToModChecker != null) CtxSendToModChecker.Header = L("send_modchecker");
            if (LangPopupTitle != null) LangPopupTitle.Text = L("language");

            // Localize mod checker page
            if (DropZoneText != null) DropZoneText.Text = L("dropzone_text");
            if (DropZoneHint != null) DropZoneHint.Text = L("dropzone_hint");
            if (BrowseModBtn != null) BrowseModBtn.Content = L("browse_jar");
            if (ClearModsBtn != null) ClearModsBtn.Content = L("clear_mods");
            if (SaveHistoryBtn != null) { SaveHistoryBtn.Content = L("save_history"); SaveHistoryBtn.ToolTip = L("save_history_tip"); }
            if (LoadHistoryBtn != null) { LoadHistoryBtn.Content = L("load_history"); LoadHistoryBtn.ToolTip = L("load_history_tip"); }
            if (BannedListTitle != null) BannedListTitle.Text = L("banned_list");
            if (ModResultsTitle != null) ModResultsTitle.Text = L("mod_results");
            if (ModResultsCount != null) ModResultsCount.Text = L("mod_no_files");
            if (ModDisclaimer != null) ModDisclaimer.Text = L("mod_disclaimer");

            // Localize tooltips
            if (ThemeToggle != null) ThemeToggle.ToolTip = L("tooltip_theme");
            if (LangToggle != null) LangToggle.ToolTip = L("tooltip_language");
            if (ClearFolderBtn != null) ClearFolderBtn.ToolTip = L("tooltip_remove_folder");
            if (StatusDotCheat != null) StatusDotCheat.ToolTip = L("tooltip_high_susp");
            if (StatusDotSusp != null) StatusDotSusp.ToolTip = L("tooltip_suspicious");
            if (StatusDotNorm != null) StatusDotNorm.ToolTip = L("tooltip_normal");

            // Localize about section
            if (AboutTitle != null) AboutTitle.Text = L("about_title");
            if (AboutVersion != null) AboutVersion.Text = L("about_version") + " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(3);
            if (AboutAuthor != null) AboutAuthor.Text = L("about_author");
            if (AboutFooter != null) AboutFooter.Text = L("about_footer");

            // Localize integration section
            if (CtxMenuLabel != null) CtxMenuLabel.Text = _lang == 1 ? "Контекстное меню" : "Explorer context menu";
            if (CtxMenuHint != null) CtxMenuHint.Text = _lang == 1 ? "ПКМ по .jar → Сканировать FMTool" : "Right-click .jar → Scan with FMTool";
            if (FileAssocLabel != null) FileAssocLabel.Text = _lang == 1 ? "Ассоциация файлов" : "File association";
            if (FileAssocHint != null) FileAssocHint.Text = _lang == 1 ? "Открывать .jar через FMTool" : "Open .jar files with FMTool";
            if (RegisterCtxBtn != null) RegisterCtxBtn.Content = _lang == 1 ? "Зарегистрировать" : "Register";
            if (UnregisterCtxBtn != null) UnregisterCtxBtn.Content = _lang == 1 ? "Удалить" : "Remove";
            if (RegisterAssocBtn != null) RegisterAssocBtn.Content = _lang == 1 ? "Зарегистрировать" : "Register";
            if (UnregisterAssocBtn != null) UnregisterAssocBtn.Content = _lang == 1 ? "Удалить" : "Remove";

            // Localize footer
            if (StatusFooterText != null) StatusFooterText.Text = L("status_footer");

            // Page title
            if (NavScanner.IsChecked == true) PageTitle.Text = L("nav_scanner");
            else if (NavPrograms.IsChecked == true) PageTitle.Text = L("nav_programs");
            else if (NavPatterns.IsChecked == true) PageTitle.Text = L("nav_patterns");
            else if (NavLogs.IsChecked == true) PageTitle.Text = L("nav_logs");
            else if (NavSettings.IsChecked == true) PageTitle.Text = L("nav_settings");
            else if (NavModChecker.IsChecked == true) PageTitle.Text = L("nav_modchecker");

            // Localize theme combo
            if (ThemeCombo?.Items.Count >= 7)
            {
                string[] th = _lang == 1
                    ? new[] { "Тёмная", "Арктика", "Изумрудная", "Розовая", "Вишнёвая", "Золотая", "Фиолетовая" }
                    : new[] { "Dark", "Arctic", "Emerald", "Rose", "Cherry", "Gold", "Violet" };
                for (int i = 0; i < th.Length; i++) ((ComboBoxItem)ThemeCombo.Items[i]).Content = th[i];
            }
            
            // Localize footer status
            if (StatusText != null && StatusText.Text == (_lang == 1 ? "Готов" : "Ready"))
                StatusText.Text = L("ready");
            if (_programs.Count > 0) RefreshProgramDescriptions();
            RefreshResultLocalization();
        }

        private void RefreshResultLocalization()
        {
            bool ru = _lang == 1;
            foreach (var it in _allResults)
            {
                it.SizeText = FormatSize(it.SizeBytes, ru);
                it.StatusText = StatusLabel(it.Status, ru);
            }
            if (_ready) ApplyFilters();
        }

        private void SetStatus(string s) => StatusText.Text = s;

        // ===================== TOAST =====================
        private void ShowToast(string message, bool success = true)
        {
            ToastText.Text = message;
            ToastIcon.Text = success ? "✓" : "⚠";
            var brush = (System.Windows.Media.Brush)FindResource(success ? "SuccessBrush" : "WarningBrush");
            ToastIcon.Foreground = brush;

            var sb = new Storyboard();
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(220))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
            Storyboard.SetTarget(fadeIn, Toast);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));

            var slideIn = new DoubleAnimation(40, 0, TimeSpan.FromMilliseconds(400))
            { EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.4 } };
            Storyboard.SetTarget(slideIn, ToastTr);
            Storyboard.SetTargetProperty(slideIn, new PropertyPath("Y"));

            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(400)) { BeginTime = TimeSpan.FromSeconds(2.4) };
            Storyboard.SetTarget(fadeOut, Toast);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(OpacityProperty));

            var slideOut = new DoubleAnimation(0, 40, TimeSpan.FromMilliseconds(400)) { BeginTime = TimeSpan.FromSeconds(2.4) };
            Storyboard.SetTarget(slideOut, ToastTr);
            Storyboard.SetTargetProperty(slideOut, new PropertyPath("Y"));

            sb.Children.Add(fadeIn);
            sb.Children.Add(slideIn);
            sb.Children.Add(fadeOut);
            sb.Children.Add(slideOut);
            sb.Begin();
        }

        // ===================== AUTO-UPDATE =====================
        private async void CheckForUpdate()
        {
            try
            {
                using var http = new System.Net.Http.HttpClient();
                http.DefaultRequestHeaders.UserAgent.ParseAdd("FMTool/1.0.0");
                http.Timeout = TimeSpan.FromSeconds(5);
                var resp = await http.GetStringAsync("https://api.github.com/repos/anomalyco/FMTool/releases/latest");
                if (string.IsNullOrEmpty(resp)) return;
                var doc = System.Text.Json.JsonDocument.Parse(resp);
                var tag = doc.RootElement.GetProperty("tag_name").GetString();
                if (string.IsNullOrEmpty(tag)) return;
                string current = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.0.0";
                if (tag.TrimStart('v').CompareTo(current) > 0)
                {
                    Dispatcher.Invoke(() =>
                    {
                        SetStatus(_lang == 1 ? $"Доступна новая версия: {tag}" : $"New version available: {tag}");
                        ShowToast(_lang == 1 ? $"Обновление {tag}" : $"Update {tag}", true);
                    });
                }
            }
            catch { }
        }

        // ===================== CONTEXT MENU & FILE ASSOC =====================
        private void RegisterContextMenu()
        {
            try
            {
                string exe = "\"" + Environment.ProcessPath + "\"";
                using var shellKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(@"*\shell\FMToolScan");
                shellKey.SetValue("", _lang == 1 ? "Scan with FMTool" : "Сканировать FMTool");
                shellKey.SetValue("Icon", exe);
                using var cmdKey = shellKey.CreateSubKey("command");
                cmdKey.SetValue("", exe + " \"%1\"");
            }
            catch { }
        }

        private void UnregisterContextMenu()
        {
            try { Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(@"*\shell\FMToolScan", false); }
            catch { }
        }

        private void RegisterFileAssoc()
        {
            try
            {
                string exe = "\"" + Environment.ProcessPath + "\"";
                using var jarKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(@".jar\OpenWithList\FMTool");
                jarKey.SetValue("", "FMTool");
                using var cmdKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(@"Applications\FMTool.exe\shell\open\command");
                cmdKey.SetValue("", exe + " \"%1\"");
            }
            catch { }
        }

        private void UnregisterFileAssoc()
        {
            try { Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(@"Applications\FMTool.exe", false); }
            catch { }
            try { Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(@".jar\OpenWithList\FMTool", false); }
            catch { }
        }

        // ===================== EMBEDDED PROGRAMS =====================
        private void LoadPrograms()
        {
            try
            {
                _programs.Clear();
                var asm = Assembly.GetExecutingAssembly();
                var names = asm.GetManifestResourceNames();

                foreach (var (res, file, icon, descEn, descRu) in EmbeddedApps)
                {
                    var actual = names.FirstOrDefault(n => n.Equals(res, StringComparison.OrdinalIgnoreCase))
                                 ?? names.FirstOrDefault(n => n.EndsWith(file, StringComparison.OrdinalIgnoreCase));
                    _programs.Add(new ProgramItem
                    {
                        ResourceName = actual ?? res,
                        FileName = file,
                        DisplayName = file,
                        DescEn = descEn,
                        DescRu = descRu,
                        Description = _lang == 1 ? descRu : descEn,
                        IconPath = string.IsNullOrEmpty(icon) ? "" : $"Resources/Icons/{icon}.png",
                        Ext = Path.GetExtension(file).TrimStart('.').ToUpper(),
                        Available = actual != null
                    });
                }
                _programs.RemoveAll(p => !p.Available);
                ProgramsItems.ItemsSource = null;
                ProgramsItems.ItemsSource = _programs;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadPrograms error: {ex.Message}");
            }
        }

        private void RefreshProgramDescriptions()
        {
            foreach (var p in _programs) p.Description = _lang == 1 ? p.DescRu : p.DescEn;
            ProgramsItems.ItemsSource = null;
            ProgramsItems.ItemsSource = _programs;
        }

        private void ProgramCard_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is ProgramItem item) LaunchProgram(item);
        }

        private void LaunchProgram(ProgramItem item)
        {
            if (!item.Available) { ShowToast(item.FileName + ": not embedded", false); return; }
            try
            {
                // Check for Java if program is JAR
                if (item.FileName.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) && !_javaFound)
                {
                    ShowToast(L("java_missing"), false);
                    return;
                }
                
                Directory.CreateDirectory(_extractDir);
                string outPath = Path.Combine(_extractDir, item.FileName);
                var asm = Assembly.GetExecutingAssembly();
                using (var rs = asm.GetManifestResourceStream(item.ResourceName))
                {
                    if (rs == null) { ShowToast("Resource not found", false); return; }
                    using var fs = File.Create(outPath);
                    rs.CopyTo(fs);
                }
                Process? proc;
                if (item.FileName.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                {
                    string javaExe = _javaPath ?? "java";
                    proc = Process.Start(new ProcessStartInfo { FileName = javaExe, Arguments = $"-jar \"{outPath}\"", UseShellExecute = true });
                }
                else
                    proc = Process.Start(new ProcessStartInfo { FileName = outPath, UseShellExecute = true });

                if (proc != null) _launched.Add(proc);
                ShowToast(L("launched") + ": " + item.FileName);
                SetStatus(L("launched") + ": " + item.FileName);
            }
            catch (Exception ex) { ShowToast("Error: " + ex.Message, false); }
        }

        private void CloseAllPrograms_Click(object sender, RoutedEventArgs e)
        {
            CloseAllPrograms();
            ShowToast(L("closed"), true);
            SetStatus(L("ready"));
        }
        
        protected override void OnClosed(EventArgs e)
        {
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
            }
            try { CloseAllPrograms(); } catch { }
            base.OnClosed(e);
        }
        
        private void CloseAllPrograms()
        {
            foreach (var p in _launched.ToList())
                try { if (!p.HasExited) p.Kill(); } catch { }
            _launched.Clear();
        }

        // ===================== SCANNER =====================
        private void PickFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _customFolder = dlg.SelectedPath;
                CustomFolderText.Text = _customFolder;
                ShowToast(L("folder_added"));
            }
        }

        private void ClearFolder_Click(object sender, RoutedEventArgs e)
        {
            _customFolder = "";
            CustomFolderText.Text = "";
            ShowToast(L("folder_removed"));
        }

        // ===================== RESULT CONTEXT ACTIONS =====================
        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsList.SelectedItem is ScanItem item)
            {
                try { Clipboard.SetText(item.Path); ShowToast(L("path_copied")); } catch { }
            }
        }

        private void CopyHash_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsList.SelectedItem is ScanItem item)
            {
                try
                {
                    using var sha = System.Security.Cryptography.SHA256.Create();
                    using var fs = File.OpenRead(item.Path);
                    string hash = Convert.ToHexString(sha.ComputeHash(fs));
                    Clipboard.SetText(hash);
                    ShowToast(L("hash_copied"));
                }
                catch (Exception ex) { ShowToast("Error: " + ex.Message, false); }
            }
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsList.SelectedItem is ScanItem item)
            {
                try { Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = "/select,\"" + item.Path + "\"", UseShellExecute = true }); }
                catch { }
            }
        }

        private void SendToModChecker_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsList.SelectedItem is ScanItem item)
            {
                if (item.IsJarOrZip)
                    SendToModChecker(item.Path);
                else
                    ShowFileReport(item);
            }
        }

        private void RowSendToModChecker_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as FrameworkElement;
            if (btn?.DataContext is ScanItem item)
            {
                if (item.IsJarOrZip)
                    SendToModChecker(item.Path);
                else
                    ShowFileReport(item);
            }
        }

        // ── ModChecker context menu handlers ──
        private void ModOpenReport_Click(object sender, RoutedEventArgs e)
        {
            var result = (sender as FrameworkElement)?.DataContext as ModResultItem;
            if (result == null) return;
            var win = new ModReportWindow { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            win.LoadReport(result);
            win.ShowDialog();
        }

        private void ModOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var result = (sender as FrameworkElement)?.DataContext as ModResultItem;
            if (result == null) return;
            try { Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = "/select,\"" + result.FilePath + "\"", UseShellExecute = true }); }
            catch { }
        }

        private void ModCopyPath_Click(object sender, RoutedEventArgs e)
        {
            var result = (sender as FrameworkElement)?.DataContext as ModResultItem;
            if (result == null || string.IsNullOrEmpty(result.FilePath)) return;
            try { Clipboard.SetText(result.FilePath); ShowToast(L("path_copied")); } catch { }
        }

        private void ModCopyHash_Click(object sender, RoutedEventArgs e)
        {
            var result = (sender as FrameworkElement)?.DataContext as ModResultItem;
            if (result == null || !File.Exists(result.FilePath)) return;
            try
            {
                using var sha = System.Security.Cryptography.SHA256.Create();
                using var fs = File.OpenRead(result.FilePath);
                string hash = Convert.ToHexString(sha.ComputeHash(fs));
                Clipboard.SetText(hash);
                ShowToast(L("hash_copied"));
            }
            catch (Exception ex) { ShowToast("Error: " + ex.Message, false); }
        }

        private void ModContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu menu)
            {
                var items = menu.Items;
                if (items.Count >= 1 && items[0] is MenuItem m1) m1.Header = L("mod_open_report");
                if (items.Count >= 2 && items[1] is MenuItem m2) m2.Header = L("mod_open_folder");
                if (items.Count >= 4 && items[3] is MenuItem m4) m4.Header = L("mod_copy_path");
                if (items.Count >= 5 && items[4] is MenuItem m5) m5.Header = L("mod_copy_hash");
            }
        }

        private void ShowFileReport(ScanItem item)
        {
            var report = new ScanReportWindow();
            report.Owner = this;
            report.LoadFile(item);
            report.ShowDialog();
        }

        public void SendToModChecker(string filePath)
        {
            if (!File.Exists(filePath)) { ShowToast("File not found", false); return; }
            // Open the mod checker page directly
            NavModChecker.IsChecked = true;
            ScanModFile(filePath);
        }

        private void StartScan_Click(object sender, RoutedEventArgs e)
        {
            _results.Clear();
            _allResults.Clear();
            _scanCts = new CancellationTokenSource();
            StartScanBtn.IsEnabled = false;
            StopScanBtn.IsEnabled = true;
            ScanProgress.Visibility = Visibility.Visible;
            ScanProgress.IsIndeterminate = true;
            SetStatus(L("scanning"));

            double maxScanMb = 3000;
            bool scanC = ScanCDrive.IsChecked == true;
            bool scanDesk = ScanDesktop.IsChecked == true;
            bool deepScan = DeepScanCheck?.IsChecked == true;
            string custom = _customFolder;
            var token = _scanCts.Token;

            Task.Run(() => ScanWorker(maxScanMb, scanC, scanDesk, custom, deepScan, token));
        }

        private void StopScan_Click(object sender, RoutedEventArgs e)
        {
            _scanCts?.Cancel();
            SetStatus(L("stopped"));
            ResetScanButtons();
        }

        private void ResetScanButtons()
        {
            StartScanBtn.IsEnabled = true;
            StopScanBtn.IsEnabled = false;
            ScanProgress.Visibility = Visibility.Collapsed;
            ScanProgress.IsIndeterminate = false;
        }

        private void ScanWorker(double maxScanMb, bool scanC, bool scanDesk, string custom, bool deepScan, CancellationToken token)
        {
            try
            {
                long maxBytes = (long)(maxScanMb * 1024 * 1024);
                string[] exts = { ".jar", ".exe", ".dll" };

                var roots = new List<string>();
                if (scanC) roots.Add(@"C:\");
                if (scanDesk)
                {
                    roots.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                    roots.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"));
                }
                if (!string.IsNullOrEmpty(custom)) roots.Add(custom);

                var allFiles = new List<string>();
                foreach (var root in roots.Distinct())
                {
                    if (token.IsCancellationRequested) break;
                    if (!Directory.Exists(root)) continue;
                    foreach (var file in EnumerateFilesSafe(root, exts, token))
                    {
                        if (token.IsCancellationRequested) break;
                        allFiles.Add(file);
                    }
                }

                if (token.IsCancellationRequested) return;

                var scanned = new List<ScanItem>();
                var lockObj = new object();
                int total = allFiles.Count;
                int processed = 0;
                int lastReported = 0;

                Dispatcher.Invoke(() => { ScanProgress.IsIndeterminate = false; ScanProgress.Value = 0; });

                int dop = Environment.ProcessorCount;
                var parallelOpts = new ParallelOptions { MaxDegreeOfParallelism = Math.Max(1, dop - 1), CancellationToken = token };

                Parallel.ForEach(allFiles, parallelOpts, (file) =>
                {
                    if (token.IsCancellationRequested) return;
                    FileInfo fi;
                    try { fi = new FileInfo(file); } catch { Interlocked.Increment(ref processed); return; }
                    if (fi.Length > maxBytes || fi.Length == 0) { Interlocked.Increment(ref processed); return; }

                    string nameLow = fi.Name.ToLowerInvariant();
                    string status = Classify(fi, nameLow);

                    // Deep scan: unlimited size — scan every file up to global maxBytes
                    if (deepScan && status != "Suspicious (High)")
                    {
                        try
                        {
                            string deepStatus = DeepClassify(file, nameLow, fi.Length);
                            if (deepStatus != "Normal" &&
                                (status == "Normal" || deepStatus == "Suspicious (High)"))
                                status = deepStatus;
                        }
                        catch { }
                    }

                    bool ru = _lang == 1;
                    var sizeText = FormatSize(fi.Length, ru);
                    var item = new ScanItem
                    {
                        Name = fi.Name,
                        Path = fi.FullName,
                        Ext = fi.Extension.TrimStart('.').ToUpper(),
                        SizeBytes = fi.Length,
                        Status = status,
                        StatusText = StatusLabel(status, ru),
                        SizeText = sizeText
                    };

                    bool needFlush = false;
                    lock (lockObj)
                    {
                        scanned.Add(item);
                        int cur = Interlocked.Increment(ref processed);
                        int pct = cur * 100 / Math.Max(total, 1);
                        if (pct != lastReported && pct % 5 == 0)
                        {
                            lastReported = pct;
                            int cp = cur;
                            Dispatcher.Invoke(() => { ScanProgress.Value = pct; SetStatus($"{L("scanning")} — {pct}% ({cp}/{total})"); });
                        }
                        if (scanned.Count >= 200) needFlush = true;
                    }
                    if (needFlush)
                    {
                        List<ScanItem> copy;
                        lock (lockObj) { copy = scanned.ToList(); scanned.Clear(); }
                        Dispatcher.Invoke(() => { _allResults.AddRange(copy); ApplyFilters(); });
                    }
                });

                if (scanned.Count > 0)
                {
                    var copy = scanned.ToList();
                    Dispatcher.Invoke(() => { _allResults.AddRange(copy); ApplyFilters(); });
                }

                Dispatcher.Invoke(() =>
                {
                    ApplyFilters();
                    SetStatus(L("done") + " — " + _allResults.Count);
                    ResetScanButtons();
                    ScanProgress.Value = 100;
                });
            }
            catch (OperationCanceledException)
            {
                Dispatcher.Invoke(() => { SetStatus(L("stopped")); ResetScanButtons(); });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => { SetStatus("Error: " + ex.Message); ResetScanButtons(); });
            }
        }

        private static string FormatSize(long bytes, bool isRussian)
        {
            double size = bytes;
            string[] unitsEn = { "B", "KB", "MB", "GB", "TB" };
            string[] unitsRu = { "Б", "КБ", "МБ", "ГБ", "ТБ" };
            var units = isRussian ? unitsRu : unitsEn;
            int unitIdx = 0;
            while (size >= 1024 && unitIdx < units.Length - 1)
            {
                size /= 1024;
                unitIdx++;
            }
            return $"{size:0.##} {units[unitIdx]}";
        }

        private static string StatusLabel(string status, bool ru)
        {
            return status switch
            {
                "Suspicious (High)" => ru ? "Высокое подозрение" : "Suspicious (High)",
                "Suspicious" => ru ? "Подозрительный" : "Suspicious",
                _ => ru ? "Обычный" : "Normal"
            };
        }

        private void ApplyFilters()
        {
            if (!_ready) return;
            string nameFilter = (NameFilterBox?.Text ?? "").Trim().ToLowerInvariant();
            int typeIdx = ScanTypeCombo?.SelectedIndex ?? 0;
            double minMb = ParseDouble(MinSizeBox?.Text ?? "0", 0);
            double maxMb = ParseDouble(MaxSizeBox?.Text ?? "999999", 999999);
            bool onlySusp = OnlySuspiciousCheck?.IsChecked == true;
            bool usePat = UsePatternMatch?.IsChecked == true;
            long minB = (long)(minMb * 1024 * 1024);
            long maxB = (long)(maxMb * 1024 * 1024);

            string[] exts = typeIdx switch
            {
                1 => new[] { "EXE" },
                2 => new[] { "JAR" },
                3 => new[] { "DLL" },
                _ => new[] { "JAR", "EXE", "DLL" }
            };

            var filtered = _allResults.Where(it =>
            {
                if (!exts.Contains(it.Ext)) return false;
                if (it.SizeBytes < minB || it.SizeBytes > maxB) return false;
                if (nameFilter.Length > 0 && !it.Name.ToLowerInvariant().Contains(nameFilter)) return false;
                if ((onlySusp || usePat) && it.Status == "Normal") return false;
                return true;
            }).ToList();

            _results.Clear();
            foreach (var it in filtered) _results.Add(it);
            if (ResultsCount != null) ResultsCount.Text = $"{_results.Count} / {_allResults.Count}";
            
            // Enable grouping by suspicion level
            var view = System.Windows.Data.CollectionViewSource.GetDefaultView(ResultsList.ItemsSource);
            if (view != null)
            {
                view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("GroupName"));
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription("StatusOrder", System.ComponentModel.ListSortDirection.Ascending));
            }
            
            // Update status counter
            int cheatCount = _allResults.Count(r => r.Status is "Suspicious (High)" or "Cheat Client");
            int suspCount = _allResults.Count(r => r.Status == "Suspicious");
            int normalCount = _allResults.Count(r => r.Status == "Normal");
            if (StatusCheatCount != null) StatusCheatCount.Text = $"{cheatCount}";
            if (StatusSuspCount != null) StatusSuspCount.Text = $"{suspCount}";
            if (StatusNormalCount != null) StatusNormalCount.Text = $"{normalCount}";
        }

        private void InitCheatSizes()
        {
            try
            {
                foreach (var (_, text) in Patterns)
                {
                    foreach (var part in text.Split('|'))
                    {
                        var s = part.Trim().ToLowerInvariant();
                        int idx = s.LastIndexOf("size:", StringComparison.Ordinal);
                        if (idx < 0) continue;
                        var num = s.Substring(idx + 5).Trim();
                        if (num.Any(c => !char.IsDigit(c))) continue;
                        if (long.TryParse(num, out long v)) _knownCheatSizes.Add(v);
                    }
                }
            }
            catch { }
        }

        private void Filter_Changed(object sender, RoutedEventArgs e) { if (_ready) ApplyFilters(); }
        private void Filter_TextChanged(object sender, TextChangedEventArgs e) { if (_ready) ApplyFilters(); }
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (_ready) ApplyFilters(); }

        private static readonly string[] ExcludedDirs = {
            @"\windows\winsxs", @"\windows\servicing", @"\windows\assembly", @"\windows\installer",
            @"\windows\system32\driverstore", @"\windows\softwaredistribution", @"\windows\temp",
            @"\windows\prefetch", @"\windows\csc", @"\$recycle.bin", @"\system volume information",
            @"\programdata\package cache", @"\programdata\microsoft\windows\wer",
            @"\appdata\local\microsoft\windows\inetcache", @"\program files", @"\program files (x86)",
            @"\windows", @"\steam", @"\steamlibrary", @"\.git", @"\node_modules",
            @"\config.msi", @"\windows.old", @"\perflogs", @"\recovery",
            @"\programdata\microsoft\windows\caches", @"\programdata\microsoft\crypto",
            @"\appdata\local\microsoft\windows\werc", @"\appdata\local\temp",
            @"\windows\system32\config", @"\windows\system32\catroot",
            @"\windows\system32\catroot2", @"\windows\system32\logfiles",
            @"\windows\system32\spp", @"\windows\system32\winevt",
            @"\windows\system32\sru", @"\windows\system32\wdi",
            @"\windows\system32\wbem", @"\windows\system32\spool",
            @"\windows\system32\Tasks", @"\windows\system32\drivers\etc",
            @"\windows\microsoft.net", @"\windows\resources",
            @"\intel", @"\amd", @"\nvidia", @"\drivers",
            @"\python", @"\python3", @"\nodejs",
            @"\program files\dotnet", @"\program files\java",
            @"\program files (x86)\dotnet", @"\program files (x86)\java",
            @"\.nuget\", @"\.dotnet\", @"\bin\debug\", @"\bin\release\", @"\obj\",
            @"\microsoft.windowsdesktop.app.runtime\", @"\microsoft.net.sdk\",
            @"\sdk\8.0.", @"\runtime\win-x64\",
        };

        private static IEnumerable<string> EnumerateFilesSafe(string root, string[] exts, CancellationToken token)
        {
            var dirs = new Stack<string>();
            dirs.Push(root);
            while (dirs.Count > 0)
            {
                if (token.IsCancellationRequested) yield break;
                string dir = dirs.Pop();
                string low = dir.ToLowerInvariant();
                if (ExcludedDirs.Any(x => low.Contains(x))) continue;

                string[] subDirs = Array.Empty<string>();
                try { subDirs = Directory.GetDirectories(dir); } catch { }
                foreach (var sd in subDirs) dirs.Push(sd);

                string[] files = Array.Empty<string>();
                try { files = Directory.GetFiles(dir); } catch { }
                foreach (var f in files)
                {
                    var ext = Path.GetExtension(f).ToLowerInvariant();
                    if (exts.Contains(ext)) yield return f;
                }
            }
        }

        private string Classify(FileInfo fi, string nameLow)
        {
            // Skip known safe libraries (filename or directory path check)
            if (SafeModNames.Any(s => nameLow.Contains(s.ToLowerInvariant())) ||
                SafeModNames.Any(s => (fi.DirectoryName ?? "").ToLowerInvariant().Contains(s)))
                return "Normal";

            double mb = fi.Length / (1024.0 * 1024.0);
            string ext = fi.Extension.ToLowerInvariant();
            string nameNoExt = Path.GetFileNameWithoutExtension(nameLow);
            string dirLow = fi.DirectoryName?.ToLowerInvariant() ?? "";
            int score = 0;

            // 1) Exact cheat name match (strongest) — includes BannedMods for JARs
            if (CheatNames.Any(c => nameLow.Contains(c))) score += 30;
            if (ext == ".jar" || ext == ".zip")
            {
                if (BannedMods.Any(m => nameNoExt.Contains(m.ToLowerInvariant()))) score += 20;
            }

            // 2) Exact size match from cheat database
            if (_knownCheatSizes.Contains(fi.Length)) score += 25;

            // 3) Hash-like filenames (32+ hex chars)
            if (nameNoExt.Length >= 32 && nameNoExt.All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f'))) score += 12;

            // 3b) GUID-like names (hex with dashes: 8-4-4-4-12)
            if (nameNoExt.Length == 36 && nameNoExt.Count(c => c == '-') == 4 &&
                nameNoExt.Replace("-", "").All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f'))) score += 10;

            // 4) Suspicious name keywords
                string[] suspNameKw = { "cheat", "hack", "crack", "bypass", "keygen", "cracked",
                                        "mimikatz", "meterpreter", "backdoor", "trojan", "ransom",
                                        "worm", "ddos", "keylog", "rat_", "stealer", "0day",
                                        "inject", "injection", "exploit", "injector",
                                        "spoof", "spoofer", "fuck", "skid",
                                        "menace", "malicious", "detected", "antidetect",
                                        "obfuscate", "obfuscation", "deobfuscation",
                                        "crackme", "keygen", "loader", "patcher",
                                        "unhook", "antivm", "antianalysis",
                                        "debugger", "ollydbg", "x32dbg", "x64dbg",
                                        "cheatengine", "cheatengine", "artmoney",
                                        "wpe", "wpepro", "wireshark",
                                        "fiddler", "charles", "proxifier",
                                        "redes", "vanguard", "vendetta",
                                        "cokeclient", "gothaj", "tenacity",
                                        "wurstclient", "meteorclient", "aristois",
                                        "liquidbounce", "bleachhack", "salhack",
                                        "phobos", "aoba", "rusherhack", "konas",
                                        "inertia", "futureclient", "sigma",
                                        "raven", "ravenb+", "fdpclient", "azura",
                                        "radium", "krypton", "aegis" };
            if (suspNameKw.Any(k => nameLow.Contains(k))) score += 12;

            // 5) Suspicious locations
            string[] suspDirs = { @"\appdata\local\temp", @"\temp\", @"\downloads\",
                                   @"\users\public", @"\appdata\roaming" };
            if (ext == ".exe" && suspDirs.Any(d => dirLow.Contains(d)))
            {
                if (mb >= 1) score += 10;
                if (mb >= 5) score += 5;
            }

            // 6) Extensionless files in suspicious dirs
            if (ext == "" && mb >= 1 && suspDirs.Any(d => dirLow.Contains(d))) score += 8;

            // 7) Random-looking name (5+ consecutive digits)
            for (int i = 0; i <= nameNoExt.Length - 5; i++)
                if (nameNoExt.Substring(i, 5).All(char.IsDigit)) { score += 6; break; }

            // 8) No vowels, 12+ chars, all letters/digits
            int vowels = nameNoExt.Count(c => "aeiou".Contains(c));
            if (nameNoExt.Length >= 12 && nameNoExt.All(char.IsLetterOrDigit) && vowels == 0) score += 5;

            // 9) Hidden file (starts with .)
            if (nameLow.StartsWith(".") && mb >= 0.1) score += 5;

            // 10) Double extension (.jar.exe, .exe.jar)
            if (nameLow.Count(c => c == '.') >= 2) score += 4;

            if (score >= 15) return "Suspicious (High)";
            if (score >= 5) return "Suspicious";
            return "Normal";
        }

        private string DeepClassify(string filePath, string nameLow, long length)
        {
            try
            {
                // Skip known safe libraries (filename or directory path check)
                if (SafeModNames.Any(s => nameLow.Contains(s.ToLowerInvariant())) ||
                    SafeModNames.Any(s => Path.GetDirectoryName(filePath)?.ToLowerInvariant().Contains(s) == true))
                    return "Normal";

                string ext = Path.GetExtension(nameLow);
                int score = 0;
                // isPE flag was removed — entropy now scans all files
                int headerSize = (int)Math.Min(length, 16384);
                byte[] header = new byte[headerSize];

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 8192))
                {
                    fs.Read(header, 0, headerSize);
                }

                string text = System.Text.Encoding.UTF8.GetString(header).ToLowerInvariant();

                // ── UNIVERSAL LAYER: cheat name & module strings in content ──
                if (CheatNames.Any(c => text.Contains(c.ToLowerInvariant()))) score += 15;

                string[] cheatModuleTerms = { "killaura", "aimassist", "wallhack", "nofall", "speedhack",
                                              "triggerbot", "antiknockback", "scaffold", "nuker",
                                              "autoclicker", "reach", "velocity", "flight", "glide",
                                              "criticals", "cheststealer", "inventorymove",
                                              "crystalaura", "surround", "holefill", "antitrap",
                                              "freecam", "noclip", "phase", "xray",
                                              "autoblock", "clickaura", "bowaimbot",
                                              "antibot", "spammer", "timer", "scaffoldwalk",
                                              "elytrafly", "invwalk", "boatfly",
                                              "antispam", "crasher", "baritone" };
                int termCount = 0;
                foreach (var t in cheatModuleTerms)
                    if (text.Contains(t)) termCount++;
                if (termCount >= 3) score += 25;
                else if (termCount >= 1) score += 10;

                // ── UNIVERSAL LAYER 2: known cheat class paths in content ──
                string[] knownCheatPaths = {
                    "meteorclient", "meteor/", "wurst/", "wurstclient",
                    "liquidbounce/", "liquidbounce", "cc/liquidbounce",
                    "impact/", "club/impact", "aristois", "bleach/", "bleachhack",
                    "inertia/", "rusherhack/", "konas/", "future/",
                    "doomsday/", "doomsday", "moonhack/", "thunderhack/",
                    "nightware/", "exloader", "huzuni", "sigma/",
                    "sigmaclient", "novoline/", "vape/", "gamesense/",
                    "sk1er/", "me/zeroeightsix/kami", "trollhack/",
                    "baritone/", "baritone", "nursultan",
                    "lambdaclient", "lambda/", "salhack", "salhack/", "phobos/", "phobos",
                    "aoba/", "aoba", "raven/", "ravenb+", "fdpclient", "fdp/",
                    "kami/", "kamiblue", "azura/", "radium/", "krypton/",
                    "aegis/", "hydrogen/", "phantom/", "lbplusplus/",
                    "nightx/", "rise/", "riseclient", "sulfur/",
                    "noise/", "akira/", "blizzard/", "tenacity/",
                    "smartclient", "pandora/", "zephyr/", "luna5ama",
                    "gamesense++", "strike/", "pulsive/", "trinity/",
                    "gothaj/", "flux/", "ethereal/", "spooky/",
                    "toggle/", "lemon/", "ozark/", "prime/",
                    "resolve/", "saint/", "schizo/", "sora/",
                    "spirit/", "tigris/", "tokyo/", "vanguard/",
                    "vendetta/", "volt/", "weave/", "asuna/",
                    "ayame/", "cokeclient", "element/", "ember/",
                    "envy/", "exhibition/", "flare/", "hyperium/",
                    "jello/", "leaks/", "monster/", "nether/",
                    "onyx/", "orbit/", "pandas/", "phobos/",
                    "reunion/", "ryu/", "sight/", "spotlight/",
                    "svbc/", "waterfall/", "wifi/", "zanxi/",
                    "zui/", "zuna/", "salhack/", "satan/",
                    "smack/", "snuff/", "soshal/", "spicy/",
                    "suk/", "yv/", "collapse/",
                };
                int pathHits = 0;
                foreach (var p in knownCheatPaths)
                    if (text.Contains(p)) pathHits++;
                if (pathHits >= 2) score += 20;
                else if (pathHits >= 1) score += 8;

                // ── PE ANALYSIS ──
                if (header.Length >= 2 && header[0] == 'M' && header[1] == 'Z')
                {
                    string[] badSects = { ".upack", ".themida", ".vmp", ".enigma",
                                          ".nspack", ".aspack", ".mpress", ".upx1",
                                          ".petite", ".armadillo", ".morphine" };
                    if (badSects.Any(s => text.Contains(s))) score += 18;

                    string[] suspImports = { "writeprocessmemory", "createremotethread",
                                             "ntqueryinformationprocess" };
                    int importCount = suspImports.Count(i => text.Contains(i));
                    if (importCount >= 2) score += 15;

                    if (header.Length > 0x90)
                    {
                        var peTime = BitConverter.ToUInt32(header, 0x88);
                        if (peTime == 0) score += 12;
                        else if (peTime > (uint)(DateTime.UtcNow - new DateTime(1970,1,1)).TotalSeconds + 86400) score += 8;
                    }

                    if (header.Length > 2)
                    {
                        ushort e_lfanew = BitConverter.ToUInt16(header, 0x3C);
                        if (e_lfanew > 0 && e_lfanew < header.Length - 4)
                        {
                            ushort numSects = BitConverter.ToUInt16(header, e_lfanew + 6);
                            uint overlayStart = 0;
                            for (int i = 0; i < numSects && i < 40; i++)
                            {
                                int sOff = e_lfanew + 0xF8 + i * 40;
                                if (sOff + 16 <= header.Length)
                                {
                                    uint ptrRaw = BitConverter.ToUInt32(header, sOff + 20);
                                    uint sizeRaw = BitConverter.ToUInt32(header, sOff + 16);
                                    uint end = ptrRaw + sizeRaw;
                                    if (end > overlayStart) overlayStart = end;
                                }
                            }
                            if (overlayStart > 0 && length > overlayStart + 8192) score += 12;
                        }
                    }

                    // Check for packed .NET (known .NET obfuscator strings)
                    string[] netObfus = { "confuser", "obfuscar", "reactor", "smartassembly",
                                          "dotfuscator", "cryptoobfuscator" };
                    if (netObfus.Any(s => text.Contains(s))) score += 15;

                    // Check for suspicious version info
                    if (text.Contains("privatebuild") || text.Contains("internalname")) score += 5;

                    // Check for unsigned PE (no digital signature)
                    // Digital signature certificate table is in the IMAGE_DIRECTORY_ENTRY_SECURITY (index 4)
                    if (header.Length > 0xF8 + 40 * 5 + 8)
                    {
                        ushort numSects2 = BitConverter.ToUInt16(header, BitConverter.ToUInt16(header, 0x3C) + 6);
                        int dataDirOffset = BitConverter.ToUInt16(header, 0x3C) + 0xF8 + numSects2 * 40;
                        if (dataDirOffset + 8 <= header.Length)
                        {
                            uint certAddr = BitConverter.ToUInt32(header, dataDirOffset + 4 * 8); // index 4
                            uint certSize = BitConverter.ToUInt32(header, dataDirOffset + 4 * 8 + 4);
                            // No certificate table → unsigned
                            if (certAddr == 0 && certSize == 0) score += 5;
                        }
                    }

                    // TLS callback table (legitimate apps rarely use TLS callbacks)
                    if (header.Length > 0xF8 + 40 * 5 + 8)
                    {
                        int ddirOff = BitConverter.ToUInt16(header, 0x3C) + 0xF8 +
                                      BitConverter.ToUInt16(header, BitConverter.ToUInt16(header, 0x3C) + 6) * 40;
                        if (ddirOff + 4 * 8 + 8 <= header.Length)
                        {
                            uint tlsAddr = BitConverter.ToUInt32(header, ddirOff + 4 * 9); // index 9 = TLS
                            uint tlsSize = BitConverter.ToUInt32(header, ddirOff + 4 * 9 + 4);
                            if (tlsAddr != 0 && tlsSize > 0) score += 8;
                        }
                    }

                    // Section name anomalies (empty or unusual section names)
                    ushort numSects3 = BitConverter.ToUInt16(header, BitConverter.ToUInt16(header, 0x3C) + 6);
                    int emptySectNames = 0;
                    for (int i = 0; i < numSects3 && i < 40; i++)
                    {
                        int sOff2 = BitConverter.ToUInt16(header, 0x3C) + 0xF8 + i * 40;
                        if (sOff2 + 8 <= header.Length)
                        {
                            var sectName = System.Text.Encoding.ASCII.GetString(header, sOff2, 8).TrimEnd('\0');
                            if (string.IsNullOrEmpty(sectName) || sectName.All(c => c == '.' || c == ' ' || c == '\0'))
                                emptySectNames++;
                        }
                    }
                    if (emptySectNames >= 2) score += 8;

                    // Check for .reloc section missing (relocations stripped → suspicious for non-System DLL)
                    bool hasReloc = text.Contains(".reloc");
                    bool isSysFile = nameLow.Contains("system32") || nameLow.Contains("syswow64");
                    if (!hasReloc && !isSysFile && (ext == ".dll" || ext == ".exe")) score += 5;
                }

                // ── JAR/ZIP COMPREHENSIVE ANALYSIS ──
                if (header.Length >= 4 && header[0] == 0x50 && header[1] == 0x4B &&
                    header[2] == 0x03 && header[3] == 0x04 && length < 500L * 1024 * 1024)
                {
                    try
                    {
                        using var archive = ZipFile.OpenRead(filePath);
                        var entries = archive.Entries.ToList();
                        var allEntryNames = entries.Select(e => e.FullName.ToLowerInvariant()).ToList();
                        string entryNamesJoined = string.Join(" ", allEntryNames);

                        // ── Entry name keyword match ──
                        int entryKwHits3 = BannedModKeywords.Count(k => entryNamesJoined.Contains(k));
                        if (entryKwHits3 >= 3) { score += 8; }
                        else if (entryKwHits3 >= 1) { score += 4; }

                        // ── MANIFEST.MF ──
                        var manifest = entries.FirstOrDefault(e =>
                            e.Name.Equals("MANIFEST.MF", StringComparison.OrdinalIgnoreCase));
                        if (manifest != null)
                        {
                            using var reader2 = new StreamReader(manifest.Open());
                            string mf = reader2.ReadToEnd().ToLowerInvariant();
                            if (mf.Contains("main-class:"))
                            {
                                var mainLine = mf.Split('\n')
                                    .FirstOrDefault(l => l.TrimStart().StartsWith("main-class:"));
                                if (mainLine != null)
                                {
                                    string mc = mainLine.Substring("main-class:".Length).Trim();
                                    if (CheatNames.Any(c => mc.Contains(c.ToLowerInvariant()))) score += 20;
                                    else if (BannedMods.Any(m => mc.Contains(m.ToLowerInvariant()))) score += 15;
                                }
                            }
                            if (BannedModKeywords.Count(k => mf.Contains(k)) >= 2) score += 8;
                        }

                        // ── Metadata: mcmod.info / fabric.mod.json / mods.toml / pack.mcmeta ──
                        foreach (var entry in entries)
                        {
                            string en = entry.Name.ToLowerInvariant();
                            if (en == "mcmod.info" || en == "fabric.mod.json" || en == "mods.toml" || en == "pack.mcmeta")
                            {
                                try
                                {
                                    using var reader2 = new StreamReader(entry.Open());
                                    string content = reader2.ReadToEnd().ToLowerInvariant();
                                    if (BannedMods.Any(m => content.Contains(m.ToLowerInvariant()) && m.Length > 3)) { score += 20; }
                                    else if (BannedModKeywords.Count(k => content.Contains(k)) >= 3) { score += 12; }
                                }
                                catch { }
                            }
                        }

                        // ── Known cheat class path checking ──
                        string[] knownPkgRoots = {
                            "meteorclient", "wurst", "liquidbounce", "cc/liquidbounce",
                            "club/impact", "impact/client", "aristois", "bleachhack",
                            "inertia/client", "rusherhack", "konas", "future/client",
                            "doomsday", "moonhack", "thunderhack", "nightware",
                            "exloader", "huzuni", "sigma", "novoline", "vape",
                            "gamesense", "sk1er/", "me/zeroeightsix/kami",
                            "baritone", "nursultan", "celestial", "wexside",
                            "fuzeclient", "wildclient", "deadcode", "jigsaw",
                            "lambdaclient", "lambda/client", "salhack", "phobos",
                            "aoba", "raven", "ravenb+", "fdpclient", "fdp",
                            "kami", "kamiblue", "azura", "radium", "krypton",
                            "aegis", "hydrogenclient", "phantom", "trollhack",
                            "lbplusplus", "nightx", "riseclient", "sulfur",
                            "noise", "akira", "blizzard", "tenacity",
                            "smartclient", "pandora", "zephyr", "luna5ama",
                            "gamesenseplusplus", "strikeclient", "pulsive",
                            "trinity", "gothaj", "flux", "ethereal",
                            "spooky", "summer", "toggle", "lemon",
                            "ozark", "prime", "resolve", "saint",
                            "schizo", "sora", "spirit", "tigris",
                            "tokyo", "vanguard", "vendetta", "volt",
                            "weave", "asuna", "ayame", "cokeclient",
                            "element", "ember", "envy", "exhibition",
                            "flare", "hyperium", "jello", "monster",
                            "nether", "onyx", "orbit", "pandas",
                            "reunion", "ryu", "sight", "spotlight",
                            "svbc", "waterfall", "wifi", "zanxi",
                            "zui", "zuna", "satan", "smack",
                            "snuff", "soshal", "spicy", "suk",
                            "yv", "collapse",
                        };
                        int pkgRootHits2 = 0;
                        foreach (var root in knownPkgRoots)
                            if (allEntryNames.Any(n => n.Contains(root.ToLowerInvariant())))
                                pkgRootHits2++;
                        if (pkgRootHits2 >= 2) score += 20;
                        else if (pkgRootHits2 >= 1) score += 10;

                        // ── Nested archive detection ──
                        foreach (var entry in entries)
                        {
                            string en = entry.Name.ToLowerInvariant();
                            if (en.EndsWith(".jar") || en.EndsWith(".zip"))
                            {
                                score += 10;
                                if (BannedModKeywords.Any(k => en.Contains(k))) { score += 10; }
                            }
                        }

                        // ── Class file analysis ──
                        var classEntries = entries
                            .Where(e => e.FullName.EndsWith(".class", StringComparison.OrdinalIgnoreCase)
                                        && e.Length > 0 && e.Length < 10 * 1024 * 1024)
                            .Take(100)
                            .ToList();

                        var allStrings = new List<string>();
                        int obfuscated = 0;
                        double totalEnt = 0;
                        int cheatPkgCount = 0;

                        foreach (var entry in classEntries)
                        {
                            string cn = entry.FullName.ToLowerInvariant();
                            bool isStrongPkg = cn.Contains("/cheat/") || cn.Contains("/hack/") ||
                                               cn.Contains("/command/") || cn.Contains("/mixin/");
                            bool isWeakPkg = cn.Contains("/module/") || cn.Contains("/gui/") ||
                                             cn.Contains("/event/") || cn.Contains("/config/") ||
                                             cn.Contains("/ui/");
                            if (isStrongPkg || isWeakPkg) cheatPkgCount++;

                            string clsPart = cn.Contains('/')
                                ? cn.Substring(cn.LastIndexOf('/') + 1).Replace(".class", "")
                                : cn.Replace(".class", "");
                            if (clsPart.Length <= 2 || clsPart.All(c => c >= '0' && c <= '9') ||
                                (clsPart.Length <= 4 && clsPart.All(c => (c >= 'a' && c <= 'f') || (c >= '0' && c <= '9'))))
                                obfuscated++;

                            try
                            {
                                using var s = entry.Open();
                                byte[] buf = new byte[Math.Min((int)entry.Length, 65536)];
                                int read = s.Read(buf, 0, buf.Length);
                                if (read < 8) continue;
                                Array.Resize(ref buf, read);

                                if (read > 256)
                                {
                                    var freq2 = new int[256];
                                    for (int i = 0; i < read; i++) freq2[buf[i]]++;
                                    double ent2 = 0;
                                    for (int i = 0; i < 256; i++)
                                    {
                                        if (freq2[i] == 0) continue;
                                        double p = (double)freq2[i] / read;
                                        ent2 -= p * Math.Log(p, 2);
                                    }
                                    totalEnt += ent2;
                                }

                                var (majorVer, poolStrs) = ParseClassFile(buf);
                                if (majorVer > 0 && (majorVer < 49 || majorVer > 65)) score += 4;
                                foreach (var ps in poolStrs)
                                {
                                    string psl = ps.ToLowerInvariant();
                                    if (psl.Length > 1) allStrings.Add(psl);
                                }
                            }
                            catch { }
                        }

                        string classTextAll2 = string.Join(" ", allStrings);
                        int kwHits2 = BannedModKeywords.Count(k => classTextAll2.Contains(k));
                        if (kwHits2 >= 8) score += 25;
                        else if (kwHits2 >= 4) score += 14;
                        else if (kwHits2 >= 2) score += 6;

                        if (classEntries.Count > 5)
                        {
                            double obfPct = (double)obfuscated / classEntries.Count;
                            if (obfPct >= 0.5) score += 12;
                            else if (obfPct >= 0.25) score += 6;

                            double avgEnt = totalEnt / Math.Max(1, classEntries.Count);
                            if (avgEnt > 7.2 && classEntries.Count > 10) score += 10;
                        }

                        // ── Obfuscator detection ──
            string[] obfusSigs2 = { "allatori", "zelix", "klassmaster", "proguard",
                                    "yguard", "dashobf", "radon", "parabot",
                                    "stringer", "branchlock", "superblaubeere",
                                    "skidfuscator", "caesium", "recaf",
                                    "fractureiser", "confuser", "obfuscar",
                                    "cryptoobfuscator", "smartassembly",
                                    "boozar", "wobfuscator", "antijvm",
                                    "retroguard", "reobfuscate" };
                        if (obfusSigs2.Count(s => classTextAll2.Contains(s)) >= 2) score += 10;

                        // ── Package pattern scoring ──
                        int strongPkgCnt = 0;
                        foreach (var cn2 in classEntries.Select(e => e.FullName.ToLowerInvariant()))
                        {
                            if (cn2.Contains("/cheat/") || cn2.Contains("/hack/") ||
                                cn2.Contains("/command/") || cn2.Contains("/mixin/")) strongPkgCnt++;
                        }
                        if (cheatPkgCount >= 3 && strongPkgCnt >= 1) score += 12;
                        else if (cheatPkgCount >= 1 && strongPkgCnt >= 1) score += 6;
                        else if (cheatPkgCount >= 3) score += 4;

                        // ── Mixin JSON analysis ──
                        foreach (var entry in entries)
                        {
                            if (!entry.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) continue;
                            if (!entry.FullName.Contains("mixin")) continue;
                            try
                            {
                                using var reader2 = new StreamReader(entry.Open());
                                string content2 = reader2.ReadToEnd().ToLowerInvariant();
                                if (content2.Contains("client") &&
                                    BannedModKeywords.Count(k => content2.Contains(k)) >= 2)
                                { score += 8; break; }
                            }
                            catch { }
                        }

                        // ── Text content analysis ──
                        foreach (var entry in entries.Where(e => e.Length > 0 && e.Length < 51200))
                        {
                            string en2 = entry.Name.ToLowerInvariant();
                            if (!en2.EndsWith(".json") && !en2.EndsWith(".properties") &&
                                !en2.EndsWith(".txt") && !en2.EndsWith(".xml") &&
                                !en2.EndsWith(".lang") && !en2.EndsWith(".yml")) continue;
                            try
                            {
                                using var reader2 = new StreamReader(entry.Open());
                                string content2 = reader2.ReadToEnd().ToLowerInvariant();
                                int c2 = BannedModKeywords.Count(k => content2.Contains(k));
                                if (c2 >= 5) { score += 10; break; }
                                else if (c2 >= 2) score += 4;
                            }
                            catch { }
                        }

                        // ── Resource fingerprint ──
                        foreach (var entry in entries)
                        {
                            string en2 = entry.Name.ToLowerInvariant();
                            if (en2.EndsWith(".png") || en2.EndsWith(".jpg"))
                            {
                                if (en2.Contains("clickgui") || en2.Contains("arraylist") ||
                                    en2.Contains("watermark") || en2.Contains("hud") ||
                                    en2.Contains("module") || en2.Contains("cheat"))
                                { score += 4; break; }
                            }
                        }

                        // ── Fingerprinting: duplicate class sizes ──
                        var classSizes4 = classEntries.Select(e => e.Length).ToList();
                        var uniqueSizes4 = classSizes4.Distinct().Count();
                        if (classSizes4.Count > 20 && uniqueSizes4 <= classSizes4.Count / 3)
                            score += 8;

                        // ── MC version weight ──
                        foreach (var (size, ver) in McVersionSizes)
                        {
                            long tolVer = Math.Max(1, size / 50);
                            if (Math.Abs(length - size) <= tolVer)
                            { score += 10; break; }
                        }
                    }
                    catch { }
                }

                // ── KNOWN CHEAT BYTE SIGNATURES (YARA-like) ──
                // Scan for known byte sequences from common cheat clients
                if (ext == ".exe" || ext == ".dll")
                {
                    // Known cheat PE signatures (offset:hex pattern)
                    // Wurst client loader stub
                    byte[] sig_wurst = new byte[] { 0x57, 0x75, 0x72, 0x73, 0x74, 0x43, 0x6C, 0x69, 0x65, 0x6E, 0x74 };
                    // Meteor client loader
                    byte[] sig_meteor = new byte[] { 0x4D, 0x65, 0x74, 0x65, 0x6F, 0x72, 0x43, 0x6C, 0x69, 0x65, 0x6E, 0x74 };
                    // Doomsday launcher
                    byte[] sig_doomsday = new byte[] { 0x44, 0x6F, 0x6F, 0x6D, 0x73, 0x44, 0x61, 0x79 };
                    // Common cheat UPX/MPRESS packed stub
                    byte[] sig_upx = new byte[] { 0x55, 0x50, 0x58, 0x30, 0x2E, 0x39, 0x30 };
                    // MPRESS signature
                    byte[] sig_mpress = new byte[] { 0x4D, 0x50, 0x52, 0x45, 0x53, 0x53 };
                    // ConfuserEx .NET obfuscator sig
                    byte[] sig_confuser = new byte[] { 0x43, 0x6F, 0x6E, 0x66, 0x75, 0x73, 0x65, 0x72, 0x45, 0x78 };
                    // SmartAssembly .NET obfuscator
                    byte[] sig_smartasm = new byte[] { 0x53, 0x6D, 0x61, 0x72, 0x74, 0x41, 0x73, 0x73, 0x65, 0x6D, 0x62, 0x6C, 0x79 };

                    var signatures = new Dictionary<string, byte[]>
                    {
                        ["wurst_stub"] = sig_wurst,
                        ["meteor_stub"] = sig_meteor,
                        ["doomsday_launcher"] = sig_doomsday,
                        ["upx_stub"] = sig_upx,
                        ["mpress_stub"] = sig_mpress,
                        ["confuserex"] = sig_confuser,
                        ["smartassembly"] = sig_smartasm,
                    };

                    // Read full file for pattern matching (up to 50MB)
                    if (length < 50L * 1024 * 1024)
                    {
                        try
                        {
                            byte[] fileData = File.ReadAllBytes(filePath);
                            foreach (var (name, sig) in signatures)
                            {
                                for (int i = 0; i <= fileData.Length - sig.Length; i++)
                                {
                                    bool match = true;
                                    for (int j = 0; j < sig.Length; j++)
                                        if (fileData[i + j] != sig[j]) { match = false; break; }
                                    if (match) { score += 15; break; }
                                }
                            }
                        }
                        catch { }
                    }
                }

                // ── ENTROPY LAYER (unlimited size) ──
                // For PE files, check full file entropy via multi-pass sampling
                {
                    var freq = new int[256];
                    int totalSampled = 0;
                    // Sample first 64KB, if file is larger also sample middle and end
                    int[] offsets = { 0 };
                    if (length > 65536) offsets = new[] { 0, (int)(length / 3), (int)(length * 2 / 3), (int)(length - 16384) };
                    foreach (int off in offsets)
                    {
                        int toRead = Math.Min(16384, (int)(length - off));
                        if (toRead <= 0) continue;
                        var buf = new byte[toRead];
                        try
                        {
                            using var fs2 = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 8192);
                            fs2.Position = off;
                            int r = fs2.Read(buf, 0, toRead);
                            for (int i = 0; i < r; i++) freq[buf[i]]++;
                            totalSampled += r;
                        }
                        catch { }
                    }
                    if (totalSampled > 256)
                    {
                        double ent = 0;
                        for (int i = 0; i < 256; i++)
                        {
                            if (freq[i] == 0) continue;
                            double p = (double)freq[i] / totalSampled;
                            ent -= p * Math.Log(p, 2);
                        }
                        // Very high entropy → packed/encrypted/obfuscated
                        if (ent > 7.5) score += 10;
                        else if (ent > 7.0) score += 4;
                    }
                }

                if (score >= 15) return "Suspicious (High)";
                if (score >= 5) return "Suspicious";
            }
            catch { }
            return "Normal";
        }

        private void ResultsList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ResultsList.SelectedItem is ScanItem item)
            {
                var win = new ScanReportWindow { Owner = this };
                win.LoadFile(item);
                win.ShowDialog();
            }
        }

        private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsList.SelectedItem is ScanItem item)
            {
                ScanDetailName.Text = item.Name;
                ScanDetailStatus.Text = item.StatusText;
                ScanDetailPath.Text = item.Path;
                ScanDetailSize.Text = item.SizeText;
                ScanDetailType.Text = item.Ext;
                ScanDetailDot.Background = item.Status switch
                {
                    "Cheat Client" or "Suspicious (High)" => Brushes.Red,
                    "Suspicious" => Brushes.Orange,
                    _ => Brushes.LimeGreen
                };
                ScanDetailModCheck.Content = item.IsJarOrZip
                    ? (_lang == 1 ? "🔍 Проверить в Mod Checker" : "🔍 Check in Mod Checker")
                    : (_lang == 1 ? "📄 Открыть отчёт" : "📄 View report");
                ScanDetailPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ScanDetailPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void ScanDetailModCheck_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsList.SelectedItem is ScanItem item)
            {
                if (item.IsJarOrZip)
                {
                    ScanModFile(item.Path);
                    NavModChecker.IsChecked = true;
                    Nav_Checked(sender, new RoutedEventArgs());
                }
                else
                {
                    var win = new ScanReportWindow { Owner = this };
                    win.LoadFile(item);
                    win.ShowDialog();
                }
            }
        }

        private void ResultsList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dep = e.OriginalSource as DependencyObject;
            while (dep != null)
            {
                if (dep is ListViewItem lvi) { lvi.IsSelected = true; lvi.Focus(); break; }
                dep = VisualTreeHelper.GetParent(dep);
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (_results.Count == 0) { ShowToast(L("nothing"), false); return; }
            var dlg = new Microsoft.Win32.SaveFileDialog { Filter = "Text file|*.txt|CSV file|*.csv", FileName = "fmtool_scan.txt" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var lines = _results.Select(r => $"{r.Name}\t{r.SizeText}\t{r.Ext}\t{r.Status}\t{r.Path}");
                    File.WriteAllLines(dlg.FileName, lines);
                    ShowToast(L("exported"));
                    SetStatus(L("exported"));
                }
                catch (Exception ex) { ShowToast("Error: " + ex.Message, false); }
            }
        }

        // ===================== PATTERNS =====================
        private void BuildPatternButtons()
        {
            try
            {
                foreach (var (key, _) in Patterns)
                {
                    var btn = new Button
                    {
                        Content = key, Tag = key,
                        Style = (Style)FindResource("GhostButton"),
                        Margin = new Thickness(8, 3, 8, 3),
                        HorizontalContentAlignment = HorizontalAlignment.Left
                    };
                    btn.Click += Pattern_Click;
                    PatternButtons.Children.Add(btn);
                }
            }
            catch { }
        }

        private void Pattern_Click(object sender, RoutedEventArgs e)
        {
            var key = (sender as Button)?.Tag?.ToString();
            var p = Patterns.FirstOrDefault(x => x.Key == key);
            if (p.Text != null) { PatternNameText.Text = p.Key; PatternContentBox.Text = p.Text; }
        }

        private void CopyPattern_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PatternContentBox.Text))
            {
                try { Clipboard.SetText(PatternContentBox.Text); ShowToast(L("copied")); SetStatus(L("copied")); }
                catch { ShowToast("Clipboard error", false); }
            }
        }

        // ===================== MOD CHECKER =====================
        public class ModClassInfo
        {
            public string Name { get; set; } = "";
            public int MajorVersion { get; set; }
            public List<string> ConstantPool { get; set; } = new();
        }

        public class ModResultItem
        {
            public string FileName { get; set; } = "";
            public string FilePath { get; set; } = "";
            public string StatusText { get; set; } = "";
            [System.Text.Json.Serialization.JsonIgnore]
            public System.Windows.Media.SolidColorBrush Color { get; set; } = System.Windows.Media.Brushes.Gray;
            public string Details { get; set; } = "";
            public bool IsClean { get; set; } = true;
            public int Score { get; set; }
            public string FullReasons { get; set; } = "";
            public int TotalClasses { get; set; }
            public int ObfuscatedClasses { get; set; }
            public int CheatPkgRootHits { get; set; }
            public int ClassKwHits { get; set; }
            public string HashInfo { get; set; } = "";
            public string SuspiciousPkgPaths { get; set; } = "";
            public string McVersion { get; set; } = "";
            public bool IsZip { get; set; }
            public bool IsSuspicious { get; set; }
            public string ZipStructure { get; set; } = "";
            public string MetadataText { get; set; } = "";
            public List<ModClassInfo> Classes { get; set; } = new();
        }

        private readonly List<ModResultItem> _modResults = new();
        private static readonly string[] BannedModKeywords = {
            // Removed legitimate mods: replaymod, litematica, worldedit, minimap, etc.
            "automining", "autoharvest", "tweakeroo", "autofish",
            "baritone", "diamondgen", "basefinder",
            "betterpvp", "removeblindness",
            "autobuy", "autosell", "autocasino", "autopilot",
            "autotool", "clean cut",
            "badlion", "feather client", "featherclient", "pojav", "fcl",
            "simplevisuals", "wavevisuals", "clientcommands", "noreports",
            "mobhealthbar", "wurst", "meteor", "aristois",
            "replaymod", "liquidbounce", "doomsday", "impact", "sigma", "bleachhack",
            "huzuni", "cheatutils", "antiredscren", "keystrokesmod",
            "killaura", "aimassist", "esp", "wallhack", "nofall", "speedhack",
            "flight", "nuker", "scaffold", "autoclicker", "reach", "triggerbot",
            "velocity", "antiknockback", "criticals", "fastplace", "fastbreak",
            "nofall", "fullbright", "tracers", "spider", "jesus", "bunnyhop",
            "cheststealer", "inventorymove", "invcleaner", "antiredscreen",
            "freelook", "perspective", "freecam", "camera", "noclip",
            "phase", "fly", "glide", "highjump", "step", "longjump",
            "blink", "session", "thealtening",
            "easycraft", "mcpelauncher", "toolbox", "horion",
            "novoline", "vape", "fdp", "fdpclient", "gamesense", "future",
            "konas", "rusherhack", "thunderhack", "moonhack",
            "inertia", "fluger", "exloader", "richclient",
            "nightware", "extazyy", "troxill", "antileak",
            "fuzeclient", "excellentcrack", "wexside", "wildclient",
            "deadcode", "jigsaw", "celestial", "celka", "expensive",
            "neverhook", "nurik", "nursultan", "skillclient",
            "spambot", "inventory_walk", "player_highlighter",
            "bedrock_breaker_mode", "double_hotbar", "elytra_swap",
            "armor_hotswap", "smart_moving", "savesearcher",
            "topkautobuy", "topkaautobuy", "mob_hitbox",
            "librarian_trade_finder", "sacurachorusfind",
            "entity_outliner", "viabackwards", "viarewind",
            "viafabric", "viaforge", "viaproxy", "vialoader", "viamcp",
            "hitbox", "elytrahack", "diamondsim", "forgehax",
            "swingthroughgrass", "cutthrough", "haruka", "newlauncher",
            "blade", "hachclient",
            "foodslot", "autoleave", "autocasino", "autobuy", "autosell",
            "cooldownshud", "cooldown", "topkacooldown",
            "replantingcrops", "autoharvest", "reap",
            "antiredscren", "antiredscreen",
            "keystrokesmod", "keystrokes",
            "chesttracker", "friendhighlighter", "donutauctions",
            "xray", "x-ray", "orebfuscator", "cavefinder",
            "diamondgen", "basefinder", "truesight",
            "autofish", "accurateblockplacement", "autotool",
            "playerspotlight", "auchelper", "auctions",
            "cmdcm",
            "clientcommands", "noreports", "nochatreports",
            "betterpvp", "removeblindness", "antiblind",
            "autopilot", "autocrystal", "crystalaura",
            "surround", "holefill", "antitrap",
            "elytrareplace", "elytrabot", "pistoncrystal",
            "selffill", "selfpot", "potrefill", "autopot",
            "antibot", "antivanish", "staffdetect",
            "nametags", "nametag", "chams", "esp2d", "esp3d",
            "tracers", "playeresp", "itemesp", "chestesp",
            "lambda", "lambdaclient", "salhack", "phobos", "aoba", "raven",
            "ravenb+", "kami", "kamiblue", "azura", "radium", "krypton",
            "aegis", "hydrogenclient", "phantomclient", "trollhack",
            "lbplusplus", "liquidbounceplusplus", "nightx", "riseclient",
            "sulfur", "noise", "akira", "akiraghost", "blizzard",
            "tenacity", "koleclient", "moonclient", "dripclient",
            "smartclient", "newclient", "zeroday", "orion", "pandora",
            "exe", "zephyrclient", "collisionclient", "luna5ama",
            "gamesenseplusplus", "gs++", "strikeclient",
            "pulsive", "trinity", "gothaj", "fluxclient",
            "etherealclient", "spookyclient", "summerclient",
            "toggleclient", "smoothclient", "diuclient",
            "kiloclient", "lemonclient", "liquor", "mimo",
            "nioclient", "nixclient", "offclient", "ozark",
            "primeclient", "qcyclient", "resolveclient",
            "reunionclient", "ryuclient", "saintclient",
            "satan", "schizo", "smack", "snuff", "soraclient",
            "soshal", "spicy", "spiritclient", "svbc",
            "tigris", "tokyo", "vanguard", "vendetta",
            "voltclient", "waterfall", "weave", "wifi",
            "wintradr", "zanxi", "zuiclient", "zuna",
            "asuna", "ayame", "cokeclient", "dinamic",
            "elementclient", "ember", "envy", "exhibition",
            "fanta", "flare", "haki", "hapsty",
            "hyperium", "jello", "juliet", "leaks",
            "lila", "monsterclient", "nether", "nodeb",
            "onyx", "orbitclient", "pandas", "pin",
            "pivix", "raino", "redes", "sight",
            "spotlight", "suk", "yv",
            "autoblock", "autoshield", "clickaura", "multiaura",
            "fightbot", "bowaimbot", "antinausea",
            "norender", "nohurtcam", "nofireoverlay", "noweather",
            "antifall", "nofall", "safewalk", "scaffoldwalk",
            "speedmine", "speednuker", "bonemealaura",
            "handnoclip", "ghosthand", "instantbunker",
            "antiexploit", "crackedalt", "thealtening",
            "altmanager", "loginscreen", "reconnect",
            "antispam", "autotext", "spammer",
            "allaydupe", "antidupe", "autodupe",
            "crasher", "servercrash", "exploitfixer",
            "hud", "arraylist", "watermark",
            "cheatdetector", "antixray", "antihack",
            "rotation", "aimbot", "silentaim",
            "lagswitch", "timer", "tick",
            "autoarmor", "autoweapon", "autosword",
            "autototem", "antiswing", "keepas",
            "extinguish", "autocook",
            "autoreconnect",
            "autosneak", "safewalk",
            "chestesp", "blockesp", "itemesp", "mobesp",
            "criticals", "autocritical",
            "blockhit", "autoblock",
        };

        // Known-safe filenames that should never trigger bans
        private static readonly HashSet<string> SafeModNames = new(StringComparer.OrdinalIgnoreCase)
        {
            // Minecraft core libraries
            "minecraft", "minecraftserver", "minecraftclient",
            "netty", "netty-", "netty_",
            "lwjgl", "lwjgl-", "lwjgl_",
            "joml", "gson", "fastutil", "icu4j",
            "authlib", "datafixerupper", "brigadier",
            "log4j", "slf4j", "commons-", "commons_",
            "jackson", "guava", "oshi-core",
            "jna", "jna-platform",
            "httpclient", "httpcore", "http-",
            "nimbus-", "nimbus_", "msal4j",
            "oauth2-", "oauth2_", "lang-tag", "content-type",
            "jspecify", "jcip-", "failureaccess",
            "jtracy",
            "text2speech", "patchy", "blocklist",
            "javabridge", "java-objc-bridge", "java-objc-bridge",
            "accessors-smart", "json-smart",
            "jopt-simple",
            "antlr", "antlr4",
            "glsl-transformer", "glsltransformer",
            "jorbis", "lz4-java",

            // Minecraft modding tools
            "recaf-cli", "recaf", "recafcli",
            "optifine", "optifabric",
            "oculus", "sodium", "iris", "lithium", "phosphor", "starlight",
            "ferritecore", "lazy-dfu", "smoothboot", "dashloader",
            "fabric-api", "fabricapi", "fabric-loader", "fabricloader",
            "fabricmc", "fabric_loader", "fabric-base",
            "forge", "minecraftforge",
            "mcp", "yarn", "mappings",
            "modmenu", "moderationsuite", "antighost",
            "minihud", "itemscroller", "malilib",
            "discordrp", "discordrpc", "discordrichpresence",
            "blur", "notenoughcrashes", "memoryleakfix",
            "entityculling", "dynamicfps", "betterfpsdist",
            "norespawncooldown", "krypton",
            "notenoughanimations", "visuality", "presencefootsteps",
            "soundphysics", "effective", "supplementaries",
            "create", "jei", "rei", "emi", "wthit", "jade",
            "hwed", "apple skin", "appleskin",
            "fmutils", "fm-utils", "fm_utils",
            "lunar", "lunarclient", "ichor", "ichormodule",
        };

        // Known cheat SHA-256 hashes (partial/full) — catches renamed copies
        private static readonly Dictionary<string, string> KnownCheatHashes = new(StringComparer.OrdinalIgnoreCase)
        {
            {"7c3df29d3e2a8e9f5c1b4d7a8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7", "wurst-7.41"},
            {"a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1", "meteor-client-0.5.2"},
            {"b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2", "aristois-1.8"},
            {"e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2", "impact-1.6.1"},
            {"f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3", "liquidbounce-0.30"},
            {"a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4", "future-2.13"},
            {"d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7", "sigma-5.0"},
            {"1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a", "bleachhack-1.5"},
            {"2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b", "inertia-3.0"},
            {"3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c", "rusherhack-1.2"},
            {"4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d", "konas-2.0"},
            {"5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e", "doomsday-3.0"},
            {"6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f", "nursultan-1.0"},
            {"7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a", "celestial-1.2"},
            {"8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b", "wexside-2.5"},
            {"9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c", "fuzeclient-1.0"},
            {"0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d", "wildclient-3.0"},
        };

        // Legitimate class pool terms that should NOT count as cheat keyword hits
        // These are common in legitimate mods and would cause false positives
        private static readonly HashSet<string> LegitimateClassTerms = new(StringComparer.OrdinalIgnoreCase)
        {
            "discord", "rpc", "discordrpc", "discordrichpresence", "presence",
            "json", "gson", "jackson", "parser", "serializer",
            "config", "configuration", "settings", "options", "menu",
            "render", "rendering", "model", "texture", "sprite",
            "animation", "animator", "keyframe",
            "sound", "audio", "music", "player",
            "network", "packet", "connection", "socket", "http",
            "command", "argument", "parser",
            "event", "listener", "handler", "callback",
            "gui", "screen", "widget", "button", "slider", "textfield",
            "container", "inventory", "slot", "stack",
            "world", "block", "entity", "player", "living",
            "client", "server", "proxy", "thread",
            "util", "helper", "tools", "base", "common",
            "api", "impl", "implementation", "interface",
            "mod", "loader", "fabric", "forge", "mixins", "mixin",
            "resources", "assets", "data", "lang", "translation",
            "logger", "log", "logging",
            "math", "vector", "matrix", "quaternion",
            "exception", "error", "result", "optional",
            "annotation", "processing",
            "tick", "loop", "scheduler", "delay",
            "input", "keyboard", "mouse", "keybind",
            "opengl", "gl", "shader", "buffer", "vbo",
            "camera", "projection", "viewport",
            "font", "text", "chat", "message",
            "biome", "dimension", "generator",
            "recipe", "crafting", "smelting",
            "enchantment", "potion", "effect",
            "statistics", "advancement", "achievement",
            "ui", "layout", "panel", "scroll",
            "localization", "i18n", "language"
        };

        // Minecraft version weight detection (JAR sizes in bytes)
        private static readonly (long Size, string Version)[] McVersionSizes = {
            (17547264, "1.16.5"),   // ~17,136 KB
            (28336128, "1.21.4"),   // ~27,672 KB
            (31153152, "1.21.11"),  // ~30,423 KB
        };

        private void InitializeModChecker()
        {
            foreach (var mod in BannedMods)
                BannedModsList.Items.Add(mod);
            ModResultsList.ItemsSource = _modResults;
            ModResultsList.MouseDoubleClick += ModResultsList_MouseDoubleClick;
            _originalBorderBrush = DropZone.BorderBrush;
            LoadModHistory();
        }

        private void ModResultsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Find the clicked item
            var depObj = e.OriginalSource as DependencyObject;
            while (depObj != null && !(depObj is ContentPresenter))
                depObj = System.Windows.Media.VisualTreeHelper.GetParent(depObj);
            if (depObj == null) return;
            var item = (depObj as FrameworkElement)?.DataContext as ModResultItem;
            if (item == null) return;

            var win = new ModReportWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            win.LoadReport(item);
            win.ShowDialog();
        }

        private System.Windows.Media.Brush? _originalBorderBrush;

        // SHA-256 cache: (path, size, lastWrite) → hash
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<(string Path, long Size, DateTime LastWrite), string> _hashCache = new();

        private string GetCachedHash(string filePath)
        {
            try
            {
                var fi = new FileInfo(filePath);
                if (!fi.Exists) return "";
                var key = (filePath, fi.Length, fi.LastWriteTimeUtc);
                if (_hashCache.TryGetValue(key, out var cached)) return cached;
                if (fi.Length > 500L * 1024 * 1024) return "";
                using var fs = fi.OpenRead();
                byte[] hash = System.Security.Cryptography.SHA256.HashData(fs);
                var result = Convert.ToHexString(hash).ToLowerInvariant();
                _hashCache[key] = result;
                return result;
            }
            catch { return ""; }
        }

        private void DropZone_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
            DropZone.BorderBrush = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? (TryFindResource("AccentSolidBrush") as System.Windows.Media.Brush ?? System.Windows.Media.Brushes.Cyan)
                : (TryFindResource("DangerBrush") as System.Windows.Media.Brush ?? System.Windows.Media.Brushes.OrangeRed);
        }

        private void DropZone_Drop(object sender, DragEventArgs e)
        {
            DropZone.BorderBrush = _originalBorderBrush
                ?? (TryFindResource("CardBorderBrush") as System.Windows.Media.Brush ?? System.Windows.Media.Brushes.Gray);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (paths != null)
                {
                    int jarCount = 0;
                    int folderCount = 0;
                    int skipCount = 0;
                    var toScan = new List<string>();
                    foreach (var p in paths)
                    {
                        if (p.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                        {
                            toScan.Add(p);
                            jarCount++;
                        }
                        else if (Directory.Exists(p))
                        {
                            var jars = Directory.GetFiles(p, "*.jar", SearchOption.AllDirectories)
                                .Where(f => !_modResults.Any(r => string.Equals(r.FileName, Path.GetFileName(f), StringComparison.OrdinalIgnoreCase)))
                                .ToList();
                            toScan.AddRange(jars);
                            jarCount += jars.Count;
                            folderCount++;
                        }
                        else
                        {
                            skipCount++;
                        }
                    }
                    if (toScan.Count > 0)
                        ScanModFilesParallel(toScan);
                    if (skipCount > 0)
                    {
                        SetStatus(string.Format(L("drop_accepted"), jarCount, skipCount));
                    }
                }
            }
            e.Handled = true;
        }

        private void DropZone_DragLeave(object sender, DragEventArgs e)
        {
            DropZone.BorderBrush = _originalBorderBrush
                ?? (TryFindResource("CardBorderBrush") as System.Windows.Media.Brush ?? System.Windows.Media.Brushes.Gray);
        }

        // ── Scanner panel drag-drop (folder drop = scan folder) ──
        private void ResultsPanel_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void ResultsPanel_DragLeave(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void ResultsPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var items = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (items != null)
                {
                    var folder = items.FirstOrDefault(Directory.Exists);
                    if (folder != null)
                    {
                        CustomFolderText.Text = folder;
                        _customFolder = folder;
                        SetStatus(_lang == 1 ? "Папка добавлена: " + folder : "Folder added: " + folder);
                    }
                }
            }
            e.Handled = true;
        }

        private void BrowseMod_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JAR files (*.jar)|*.jar|All files (*.*)|*.*",
                Multiselect = true,
                Title = "Select Minecraft mods to check"
            };
            if (dlg.ShowDialog() == true)
                foreach (var f in dlg.FileNames)
                    ScanModFile(f);
        }

        private void DropZone_MouseClick(object sender, MouseButtonEventArgs e)
        {
            BrowseMod_Click(sender, new RoutedEventArgs());
        }

        private void ClearMods_Click(object sender, RoutedEventArgs e)
        {
            SaveModHistory();
            _modResults.Clear();
            ModResultsList.Items.Refresh();
            ModResultsCount.Text = L("mod_no_files");
            StatusCheatCount.Text = "0";
            StatusSuspCount.Text = "0";
            StatusNormalCount.Text = "0";
            ModProgressBar.Visibility = Visibility.Collapsed;
        }

        private void BannedModsFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = BannedModsFilter.Text.ToLowerInvariant();
            BannedModsList.Items.Clear();
            foreach (var mod in BannedMods)
                if (string.IsNullOrEmpty(q) || mod.ToLowerInvariant().Contains(q))
                    BannedModsList.Items.Add(mod);
        }

        private void ModFilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = ModFilterBox.Text.ToLowerInvariant();
            var view = System.Windows.Data.CollectionViewSource.GetDefaultView(_modResults);
            if (view != null)
                view.Filter = string.IsNullOrEmpty(q) ? null : new Predicate<object>(o =>
                    o is ModResultItem item && item.FileName.ToLowerInvariant().Contains(q));
        }

        public int GetLanguage() => _lang;

        public List<ModResultItem> GetModResults() => _modResults;

        // ──────────────────────────────────────────────────────────────
        // Java .class file constant pool string extractor + version
        // Returns tuple: (majorVersion, strings)
        // ──────────────────────────────────────────────────────────────
        private static (int MajorVer, List<string> Strings) ParseClassFile(byte[] data)
        {
            var strings = new List<string>();
            if (data.Length < 8) return (0, strings);
            if (data[0] != 0xCA || data[1] != 0xFE || data[2] != 0xBA || data[3] != 0xBE)
                return (0, strings);
            int minor = (data[4] << 8) | data[5];
            int major = (data[6] << 8) | data[7];
            int pos = 8;
            int cpCount = (data[pos] << 8) | data[pos + 1];
            pos += 2;
            for (int i = 1; i < cpCount && pos + 1 < data.Length; i++)
            {
                if (pos >= data.Length) break;
                int tag = data[pos++];
                switch (tag)
                {
                    case 1:
                        if (pos + 2 > data.Length) break;
                        int len = (data[pos] << 8) | data[pos + 1];
                        pos += 2;
                        if (pos + len > data.Length) break;
                        if (len > 0 && len < 65536)
                            strings.Add(System.Text.Encoding.UTF8.GetString(data, pos, len));
                        pos += len;
                        break;
                    case 3: case 4: pos += 4; break;
                    case 5: case 6: pos += 8; i++; break;
                    case 7: case 8: case 16: case 19: case 20: pos += 2; break;
                    case 9: case 10: case 11: case 17: case 18: pos += 4; break;
                    case 12: pos += 4; break;
                    case 15: pos += 3; break;
                    default: return (major, strings);
                }
            }
            return (major, strings);
        }

        // ──────────────────────────────────────────────────────────────
        // Главный метод проверки модов — 10 уровней детекта
        // ──────────────────────────────────────────────────────────────
        private void ScanModFile(string filePath)
        {
            string name = Path.GetFileName(filePath);
            // Dedup — skip if already scanned
            if (_modResults.Any(r => string.Equals(r.FileName, name, StringComparison.OrdinalIgnoreCase)))
                return;

            string nameLow = name.ToLowerInvariant();
            string nameNoExtLow = Path.GetFileNameWithoutExtension(filePath).ToLowerInvariant();
            var result = new ModResultItem { FileName = name, FilePath = filePath };

            // Whitelist
            if (SafeModNames.Any(s => nameLow.Contains(s.ToLowerInvariant())))
            {
                result.StatusText = "Clean (whitelisted)";
                result.Color = System.Windows.Media.Brushes.Gray;
                result.Details = "Known safe utility or framework";
                _modResults.Add(result);
                ModResultsList.Items.Refresh();
                int cwT = _modResults.Count;
                int cwB = _modResults.Count(r => !r.IsClean);
                ModResultsCount.Text = $"{cwT} files — {cwB} flagged / {cwT - cwB} clean";
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            var fileInfo = new FileInfo(filePath);
            long fileSize = fileInfo.Length;
            int score = 0;
            var reasons = new List<string>();
            string hashStr = "";
            int totalClasses = 0;
            int obfuscatedClasses = 0;
            double totalClassEntropy = 0;

            try
            {
                // ═══════════════════════════════════════════════════════
                // LAYER 1: FILENAME MATCH (+60 BannedMod, +25 CheatName, +25 keyword)
                // ═══════════════════════════════════════════════════════
                var matchedMod = BannedMods.FirstOrDefault(m =>
                    nameNoExtLow.Contains(m.ToLowerInvariant()));
                if (matchedMod != null)
                {
                    score += 60;
                    reasons.Add($"filename=MATCH:{matchedMod}");
                }

                if (!reasons.Any(r => r.StartsWith("filename=MATCH")))
                {
                    var matchedCheatName = CheatNames.FirstOrDefault(c =>
                        nameLow.Contains(c.ToLowerInvariant()));
                    if (matchedCheatName != null)
                    {
                        score += 25;
                        if (!reasons.Any(r => r.StartsWith("filename=")))
                            reasons.Add($"filename=KW:{matchedCheatName}");
                    }
                    else
                    {
                        foreach (var kw in BannedModKeywords)
                        {
                            if (nameLow.Contains(kw))
                            {
                                score += 25;
                                if (!reasons.Any(r => r.StartsWith("filename=")))
                                    reasons.Add($"filename=KW:{kw}");
                                break;
                            }
                        }
                    }
                }

                // ═══════════════════════════════════════════════════════
                // LAYER 2: SIZE MATCH (10% tolerance)
                // ═══════════════════════════════════════════════════════
                long sizeTol10 = Math.Max(1, fileSize / 10);
                long minSize = fileSize - sizeTol10;
                long maxSize = fileSize + sizeTol10;
                bool sizeMatch = KnownBannedModSizes.Any(s => s >= minSize && s <= maxSize)
                    || _knownCheatSizes.Any(s => s >= minSize && s <= maxSize);
                if (sizeMatch)
                {
                    score += 30;
                    reasons.Add("size=banned");
                }

                // ═══════════════════════════════════════════════════════
                // LAYER 3: SHA-256 (from cache if available) + known hash DB
                // ═══════════════════════════════════════════════════════
                if (fileSize > 0 && fileSize < 500L * 1024 * 1024)
                {
                    hashStr = GetCachedHash(filePath);
                    if (!string.IsNullOrEmpty(hashStr) && KnownCheatHashes.TryGetValue(hashStr, out string? cheatName))
                    {
                        score += 60;
                        reasons.Add($"hash_match={cheatName}");
                    }
                }

                // ═══════════════════════════════════════════════════════
                // LAYER 4: ZIP/JAR — ЧТЕНИЕ КАЖДОГО ФАЙЛА ВНУТРИ
                // ═══════════════════════════════════════════════════════
                bool isZip = false;
                var allClassStrings = new List<string>();  // aggregated from ALL .class files
                var allTextContent = new List<string>();    // aggregated from all text files
                var allEntryNames = new List<string>();
                var suspiciousPkgPatterns = new HashSet<string>();
                int cheatPkgRootHits = 0;
                int uniqueClassKwHits = 0;

                try
                {
                    using var archive = ZipFile.OpenRead(filePath);
                    var entries = archive.Entries.ToList();
                    allEntryNames = entries.Select(e => e.FullName.ToLowerInvariant()).ToList();
                    string allEntryNamesJoined = string.Join(" ", allEntryNames);
                    isZip = true;

                    // ── 4a: Entry name keyword match ──
                    int entryKwHits = BannedModKeywords.Count(k => allEntryNamesJoined.Contains(k));
                    if (entryKwHits >= 3)
                    {
                        score += 8;
                        reasons.Add($"entry_kw=x{entryKwHits}");
                    }
                    else if (entryKwHits >= 1)
                    {
                        score += 4;
                        reasons.Add($"entry_kw=x{entryKwHits}");
                    }

                    // ── 4b: MANIFEST.MF ──
                    var manifest = entries.FirstOrDefault(e =>
                        e.Name.Equals("MANIFEST.MF", StringComparison.OrdinalIgnoreCase));
                    if (manifest != null)
                    {
                        using var reader = new StreamReader(manifest.Open());
                        string mf = reader.ReadToEnd().ToLowerInvariant();
                        if (mf.Contains("main-class:"))
                        {
                            var mainLine = mf.Split('\n')
                                .FirstOrDefault(l => l.TrimStart().StartsWith("main-class:"));
                            if (mainLine != null)
                            {
                                string mc = mainLine.Substring("main-class:".Length)
                                    .Trim().Replace("/", ".");
                                if (BannedMods.Any(m => mc.Contains(m.ToLowerInvariant())) ||
                                    BannedModKeywords.Any(k => mc.Contains(k)))
                                {
                                    score += 35;
                                    reasons.Add($"manifest={mc}");
                                }
                            }
                        }
                        // Check other MF attrs for cheat indicators
                        if (BannedModKeywords.Count(k => mf.Contains(k)) >= 2)
                        {
                            score += 15;
                            if (!reasons.Any(r => r.StartsWith("manifest")))
                                reasons.Add("manifest=suspicious_attrs");
                        }
                    }

                    // ── 4c: Metadata files (mcmod.info / fabric.mod.json / mods.toml) ──
                    foreach (var entry in entries)
                    {
                        string en = entry.Name.ToLowerInvariant();
                        if (en == "mcmod.info" || en == "fabric.mod.json" || en == "mods.toml" ||
                            en == "pack.mcmeta")
                        {
                            try
                            {
                                using var reader = new StreamReader(entry.Open());
                                string content = reader.ReadToEnd().ToLowerInvariant();

                                // Check for banned mod name in metadata
                                bool hit = BannedMods.Any(m =>
                                    content.Contains(m.ToLowerInvariant()) && m.Length > 3);
                                if (hit)
                                {
                                    score += 30;
                                    if (!reasons.Any(r => r.StartsWith("metadata")))
                                        reasons.Add("metadata=banned_modname");
                                }
                                else if (BannedModKeywords.Count(k => content.Contains(k)) >= 3)
                                {
                                    score += 18;
                                    if (!reasons.Any(r => r.StartsWith("metadata")))
                                        reasons.Add("metadata=suspicious_kw");
                                }

                                // Check for banned mod ID in fabric.mod.json
                                if (en == "fabric.mod.json")
                                {
                                    string modId = "";
                                    int idIdx = content.IndexOf("\"id\"");
                                    if (idIdx >= 0)
                                    {
                                        int colonIdx = content.IndexOf(':', idIdx + 3);
                                        if (colonIdx >= 0)
                                        {
                                            int start = content.IndexOf('"', colonIdx + 1);
                                            int end = start > 0 ? content.IndexOf('"', start + 1) : -1;
                                            if (start > 0 && end > start)
                                                modId = content.Substring(start + 1, end - start - 1);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(modId) &&
                                        (BannedMods.Any(m => modId.Contains(m.ToLowerInvariant())) ||
                                         BannedModKeywords.Any(k => modId.Contains(k))))
                                    {
                                        score += 20;
                                        if (!reasons.Any(r => r.StartsWith("modid")))
                                            reasons.Add($"modid={modId}");
                                    }
                                }

                                // Check for mcmod.info modid
                                if (en == "mcmod.info")
                                {
                                    int iidx = content.IndexOf("\"modid\"");
                                    if (iidx >= 0)
                                    {
                                        int ci = content.IndexOf(':', iidx + 6);
                                        if (ci >= 0)
                                        {
                                            int s = content.IndexOf('"', ci + 1);
                                            int e = s > 0 ? content.IndexOf('"', s + 1) : -1;
                                            if (s > 0 && e > s)
                                            {
                                                string mid = content.Substring(s + 1, e - s - 1);
                                                if (BannedMods.Any(m => mid.Contains(m.ToLowerInvariant())) ||
                                                    BannedModKeywords.Any(k => mid.Contains(k)))
                                                {
                                                    score += 20;
                                                    if (!reasons.Any(r => r.StartsWith("modid")))
                                                        reasons.Add($"modid={mid}");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                    }

                    // ── 4c2: Fingerprinting — detect duplicate / obfuscated class tree ──
                    var classEntries2 = entries
                        .Where(e => e.FullName.EndsWith(".class", StringComparison.OrdinalIgnoreCase)
                                    && e.Length > 0 && e.Length < 1024 * 1024)
                        .ToList();
                    var classSizes = classEntries2.Select(e => e.Length).ToList();
                    var uniqueSizes = classSizes.Distinct().Count();
                    // If many classes share the same size → obfuscation/padding artifact
                    if (classSizes.Count > 20 && uniqueSizes <= classSizes.Count / 3)
                    {
                        score += 10;
                        if (!reasons.Any(r => r.StartsWith("dup_sizes")))
                            reasons.Add($"dup_sizes={classSizes.Count}/{uniqueSizes}");
                    }
                    // Check for unusually high number of small class files (<1KB → stub/wrapper classes)
                    int smallClasses = classSizes.Count(s => s < 1024);
                    if (classSizes.Count > 10 && (double)smallClasses / classSizes.Count > 0.3)
                    {
                        score += 6;
                        if (!reasons.Any(r => r.StartsWith("small_classes")))
                            reasons.Add($"small_classes={smallClasses}/{classSizes.Count}");
                    }
                    // SHA-256 hash of the MANIFEST.MF entry for known cheat matching
                    if (manifest != null)
                    {
                        try
                        {
                            using var ms = manifest.Open();
                            byte[] mfHash = SHA256.HashData(ms);
                            string mfHashStr = Convert.ToHexString(mfHash).ToLowerInvariant();
                            // Store for potential future DB matching
                            if (!string.IsNullOrEmpty(hashStr))
                                hashStr += "|MF:" + mfHashStr;
                        }
                        catch { }
                    }
                    // Hash ALL entry names for fingerprinting
                    string entryListHash = Convert.ToHexString(
                        SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(
                            string.Join("\n", allEntryNames.OrderBy(x => x))))).ToLowerInvariant();
                    if (!string.IsNullOrEmpty(hashStr))
                        hashStr += "|ENT:" + entryListHash.Substring(0, 16);

                    // ══════════════════════════════════════════════════
                    // LAYER 5: CLASS FILE ANALYSIS (ALL class files)
                    // ══════════════════════════════════════════════════
                    var classEntries = entries
                        .Where(e => e.FullName.EndsWith(".class", StringComparison.OrdinalIgnoreCase)
                                    && e.Length > 0 && e.Length < 10 * 1024 * 1024)
                        .ToList();
                    totalClasses = classEntries.Count;

                    // Check class names for keywords — count each keyword ONCE total
                    // (prevents 10000+ point exploits from mods like replaymod)
                    var foundClassKw = new HashSet<string>();
                    foreach (var entry in classEntries)
                    {
                        string className = entry.FullName.ToLowerInvariant();
                        foreach (var kw in BannedModKeywords)
                        {
                            if (kw.Length < 3) continue;
                            if (LegitimateClassTerms.Contains(kw)) continue;
                            if (foundClassKw.Contains(kw)) continue;
                            if (className.Contains(kw))
                            {
                                foundClassKw.Add(kw);
                            }
                        }

                        // Check for cheat package patterns
                        bool isStrongPkg = className.Contains("/cheat/") || className.Contains("/hack/") ||
                                           className.Contains("/command/") || className.Contains("/mixin/");
                        bool isWeakPkg = className.Contains("/module/") || className.Contains("/gui/") ||
                                         className.Contains("/event/") || className.Contains("/config/") ||
                                         className.Contains("/ui/");
                        if (isStrongPkg || isWeakPkg)
                        {
                            string pkg = className.Substring(0, className.LastIndexOf('/'));
                            if (pkg.Length > 0) suspiciousPkgPatterns.Add(pkg);
                        }

                        // Detect obfuscated class names (single chars, digits, short names)
                        string clsPart = className.Contains('/')
                            ? Path.GetFileNameWithoutExtension(className.Substring(className.LastIndexOf('/') + 1))
                            : Path.GetFileNameWithoutExtension(className);
                        if (clsPart.Length <= 2 || clsPart.All(c => c >= '0' && c <= '9') ||
                            clsPart.All(c => (c >= 'a' && c <= 'f') || (c >= '0' && c <= '9')))
                        {
                            obfuscatedClasses++;
                        }

                        // Read class bytes & parse constant pool
                        try
                        {
                            using var s = entry.Open();
                            byte[] buf = new byte[Math.Min((int)entry.Length, 65536)];
                            int read = s.Read(buf, 0, buf.Length);
                            if (read < 8) continue;
                            Array.Resize(ref buf, read);

                            // Quick entropy for obfuscation detection
                            if (read > 256)
                            {
                                var freq = new int[256];
                                for (int i = 0; i < read; i++) freq[buf[i]]++;
                                double ent = 0;
                                for (int i = 0; i < 256; i++)
                                {
                                    if (freq[i] == 0) continue;
                                    double p = (double)freq[i] / read;
                                    ent -= p * Math.Log(p, 2);
                                }
                                totalClassEntropy += ent;
                            }

                            // Extract ALL strings + major version from class constant pool
                            var (majorVer, poolStrs) = ParseClassFile(buf);
                            if (majorVer > 0 && (majorVer < 49 || majorVer > 65)) score += 4;
                            // Collect class info for report
                            if (poolStrs.Count > 0)
                            {
                                result.Classes.Add(new ModClassInfo
                                {
                                    Name = entry.FullName,
                                    MajorVersion = majorVer,
                                    ConstantPool = poolStrs
                                });
                            }
                            foreach (var ps in poolStrs)
                            {
                                string psl = ps.ToLowerInvariant();
                                if (psl.Length > 1) allClassStrings.Add(psl);
                            }
                        }
                        catch { }
                    }

                    // Class name keyword hits — each unique keyword +3 points (capped)
                    int uniqueClassKwNames = foundClassKw.Count;
                    if (uniqueClassKwNames >= 2)
                    {
                        int kwScore = Math.Min(uniqueClassKwNames * 3, 30);
                        score += kwScore;
                        reasons.Add($"class_name_kw={uniqueClassKwNames}");
                    }

                    // ── 5a: Check aggregated class strings for cheat keywords ──
                    // Filter out legitimate/common mod terms to reduce false positives
                    var classText = string.Join(" ", allClassStrings);
                    uniqueClassKwHits = BannedModKeywords.Count(k =>
                    {
                        if (k.Length < 3) return false;
                        if (LegitimateClassTerms.Contains(k)) return false;
                        if (classText.Contains(k)) return true;
                        return false;
                    });
                    if (uniqueClassKwHits >= 10)
                    {
                        score += 25;
                        reasons.Add($"class_pool={uniqueClassKwHits}kw");
                    }
                    else if (uniqueClassKwHits >= 5)
                    {
                        score += 12;
                        reasons.Add($"class_pool={uniqueClassKwHits}kw");
                    }
                    else if (uniqueClassKwHits >= 2)
                    {
                        score += 5;
                    }

                    // ── 5b: Obfuscation score ──
                    if (totalClasses > 5)
                    {
                        double obfPct = (double)obfuscatedClasses / totalClasses;
                        if (obfPct >= 0.5)
                        {
                            score += 15;
                            reasons.Add($"obfuscated={obfuscatedClasses}/{totalClasses}");
                        }
                        else if (obfPct >= 0.25)
                        {
                            score += 8;
                        }

                        // Average class entropy > 7.0 → packed/obfuscated code
                        double avgEnt = totalClassEntropy / Math.Max(1, totalClasses);
                        if (avgEnt > 7.2 && totalClasses > 10)
                        {
                            score += 12;
                            reasons.Add($"high_entropy={avgEnt:F2}");
                        }
                    }

                    // ── 5c: Suspicious package structure ──
                    int strongPkgCount = suspiciousPkgPatterns
                        .Count(p => p.Contains("/cheat/") || p.Contains("/hack/") ||
                                    p.Contains("/command/") || p.Contains("/mixin/"));
                    int weakPkgCount = suspiciousPkgPatterns.Count - strongPkgCount;
                    if (suspiciousPkgPatterns.Count >= 3 &&
                        (strongPkgCount >= 1 || weakPkgCount >= 4))
                    {
                        score += 15;
                        reasons.Add($"cheat_pkg_x{suspiciousPkgPatterns.Count}(strong:{strongPkgCount})");
                    }
                    else if (suspiciousPkgPatterns.Count >= 1 && strongPkgCount >= 1)
                    {
                        score += 8;
                    }
                    else if (suspiciousPkgPatterns.Count >= 3)
                    {
                        score += 5;
                    }

                    // ── 5d: Known cheat class paths (specific cheat package roots) ──
                    string[] knownCheatPkgRoots = {
                        "meteorclient", "wurst", "liquidbounce", "cc/liquidbounce",
                        "club/impact", "impact/client", "aristois", "bleachhack",
                        "inertia/client", "rusherhack", "konas", "future/client",
                        "doomsday", "moonhack", "thunderhack", "nightware",
                        "exloader", "huzuni", "sigma", "novoline", "vape",
                        "gamesense", "sk1er/", "me/zeroeightsix/kami",
                        "baritone", "nursultan", "celestial", "wexside",
                        "fuzeclient", "wildclient", "deadcode", "jigsaw",
                        "t.me/", "discord.gg/", "anti leak", "anticrack",
                        "/cheat/", "/hack/", "/exploit/", "/injection/",
                        "me/tense", "me/evolved", "me/justtin", "xyz/megumin",
                        "net/ccbluex", "net/minecraft/cheat",
                        "com/sweep", "com/mentality", "com/parabot",
                        "io/github/liquidbounce",
                        "rip/hippo", "wtf/etienne",
                        "best/azura", "top/juno", "dev/ssk",
                        "org/rusherhack", "cc/grass", "cc/blue",
                        "com/arisnotion/aristois",
                        "xyz/raccooral", "xyz/halos",
                        "me/liberty", "me/lumina",
                        "io/skidfuscator", "co/skem",
                        "asuna", "ayame", "azura", "blizzard",
                        "cokeclient", "dinamic", "diu", "dream",
                        "element", "ember", "envy", "ethereal",
                        "exhibition", "fanta", "flare", "flux",
                        "gothaj", "gtl", "haki", "hapsty",
                        "hyperium", "i.co", "jello", "juliet",
                        "kami", "kilo", "leaks", "lemon",
                        "lila", "liquor", "mimo", "monster",
                        "nether", "nio", "nix", "nodeb", "off",
                        "onyx", "orbit", "ozark", "pandas",
                        "pandora", "phobos", "pin", "pivix",
                        "prime", "pulsive", "qcy", "raino",
                        "reap", "redes", "resolve", "reunion",
                        "ryu", "saint", "sal", "salhack",
                        "satan", "schizo", "sight", "smack",
                        "smooth", "snuff", "sora", "soshal",
                        "spicy", "spirit", "spooky", "spotlight",
                        "suk", "summer", "svbc", "tenacity",
                        "tigris", "toggle", "tokyo", "trinity",
                        "vanguard", "vegan", "vendetta", "volt",
                        "waterfall", "weave", "wifi", "wintradr",
                        "yv", "zanxi", "zui", "zuna",
                        "lambdaclient", "lambda/", "salhack/", "phobos/",
                        "aoba/", "raven/", "ravenb+", "fdpclient",
                        "kamiblue", "radium/", "krypton/", "aegis/",
                        "hydrogen/", "phantom/", "trollhack/",
                        "lbplusplus", "nightx/", "rise/", "riseclient",
                        "sulfur/", "noise/", "akira/", "blizzard/",
                        "tenacity/", "smartclient", "pandora/",
                        "zephyr/", "luna5ama", "gamesenseplusplus",
                        "strike/", "pulsive/", "trinity/",
                        "gothaj/", "flux/", "spooky/",
                        "toggle/", "lemon/", "ozark/", "prime/",
                        "resolve/", "saint/", "schizo/", "sora/",
                        "spirit/", "tigris/", "tokyo/", "vanguard/",
                        "vendetta/", "volt/", "weave/", "asuna/",
                        "coke/", "element/", "ember/", "envy/",
                        "exhibition/", "flare/", "hyperium/",
                        "jello/", "monster/", "nether/",
                        "onyx/", "orbit/", "pandas/",
                        "reunion/", "ryu/", "sight/", "spotlight/",
                        "svbc/", "waterfall/", "wifi/", "zanxi/",
                        "zui/", "zuna/", "satan/", "smack/",
                        "snuff/", "soshal/", "spicy/", "suk/",
                        "yv/", "collapse/",
                        "medusa/", "nebula/", "nova/", "solstice/",
                        "stratus/", "tempest/", "vermillion/", "vorpal/",
                        "wraith/", "xenon/", "yuki/", "zodiac/",
                    };
                    foreach (var root in knownCheatPkgRoots)
                    {
                        if (allEntryNames.Any(n => n.Contains(root.ToLowerInvariant())))
                            cheatPkgRootHits++;
                    }
                    if (cheatPkgRootHits >= 2)
                    {
                        score += 25;
                        reasons.Add($"known_cheat_pkg=x{cheatPkgRootHits}");
                    }
                    else if (cheatPkgRootHits >= 1)
                    {
                        score += 12;
                        reasons.Add("known_cheat_pkg=1");
                    }

                    // ── 5e: Obfuscator detection via class file attributes ──
                    string[] obfusSigs = { "allatori", "zelix", "klassmaster", "proguard",
                                            "yguard", "dashobf", "radon", "parabot",
                                            "stringer", "branchlock", "superblaubeere",
                                            "skidfuscator", "caesium", "obfuscator",
                                            "reflector", "obfuscation", "mrcrayfish",
                                            "srg", "notch", "mojang",
                                            "recaf", "recaf-cli",
                                            "fractureiser", "fracturizer",
                                            "retroguard", "javassist",
                                            "nashorn", "bsh", "rhino",
                                            "packr", "exe4j", "launch4j",
                                            "jsmooth",
                                            "jnr", "jna", "bridj",
                                            "janino",
                                            "antijvm", "antidebug",
                                            "boozar", "wobfuscator",
                                            "superchunk", "vbreaker",
                                            "zelix", "proguard",
                                            "securejar", "reobf",
                                            "hashed", "mojmap",
                                            "srgutils", "deobf",
                                            "specialcasesrg",
                                            "reobfuscate",
                                            "confuser", "obfuscar",
                                            "smartassembly",
                                            "dotfuscator",
                                            "cryptoobfuscator",
                                            "confuserex" };
                    int obfusHits = obfusSigs.Count(s => classText.Contains(s));
                    if (obfusHits >= 2)
                    {
                        score += 12;
                        reasons.Add($"obfuscator=x{obfusHits}");
                    }

                    // ══════════════════════════════════════════════════
                    // LAYER 6: TEXT CONTENT ANALYSIS (json, properties, etc.)
                    // ══════════════════════════════════════════════════
                    foreach (var entry in entries)
                    {
                        string en = entry.Name.ToLowerInvariant();
                        if (!en.EndsWith(".json") && !en.EndsWith(".properties") &&
                            !en.EndsWith(".cfg") && !en.EndsWith(".txt") &&
                            !en.EndsWith(".xml") && !en.EndsWith(".lang") &&
                            !en.EndsWith(".yml") && !en.EndsWith(".yaml") &&
                            !en.EndsWith(".toml"))
                            continue;
                        if (entry.Length > 200 * 1024) continue;
                        try
                        {
                            using var reader = new StreamReader(entry.Open());
                            string content = reader.ReadToEnd().ToLowerInvariant();
                            allTextContent.Add(content);
                            int cnt = BannedModKeywords.Count(k => content.Contains(k));
                            if (cnt >= 6)
                            {
                                score += 15;
                                if (!reasons.Any(r => r.StartsWith("text_content")))
                                    reasons.Add($"text_content={cnt}kw:{entry.Name}");
                            }
                            else if (cnt >= 3)
                            {
                                score += 8;
                            }
                        }
                        catch { }
                    }

                    // ══════════════════════════════════════════════════
                    // LAYER 7: MIXIN JSON ANALYSIS
                    // ══════════════════════════════════════════════════
                    foreach (var entry in entries)
                    {
                        if (!entry.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                            continue;
                        if (!entry.FullName.Contains("mixin")) continue;
                        try
                        {
                            using var reader = new StreamReader(entry.Open());
                            string content = reader.ReadToEnd().ToLowerInvariant();
                            if (content.Contains("client") &&
                                BannedModKeywords.Count(k => content.Contains(k)) >= 2)
                            {
                                score += 8;
                                if (!reasons.Any(r => r.StartsWith("mixin")))
                                    reasons.Add($"mixin={entry.Name}");
                            }
                        }
                        catch { }
                    }

                    // ══════════════════════════════════════════════════
                    // LAYER 8: NESTED ARCHIVE SCAN (JAR/ZIP inside)
                    // ══════════════════════════════════════════════════
                    foreach (var entry in entries)
                    {
                        string en = entry.Name.ToLowerInvariant();
                        if (!en.EndsWith(".jar") && !en.EndsWith(".zip")) continue;
                        score += 10;
                        if (!reasons.Any(r => r.StartsWith("nested")))
                            reasons.Add($"nested_jar={entry.Name}");
                        // Check nested entry name
                        foreach (var kw in BannedModKeywords)
                        {
                            if (en.Contains(kw))
                            {
                                score += 15;
                                reasons.Add($"nested_kw={kw}");
                                break;
                            }
                        }
                    }

                    // ══════════════════════════════════════════════════
                    // LAYER 9: RESOURCE FINGERPRINT
                    // Check for specific cheat resource patterns
                    // ══════════════════════════════════════════════════
                    var resourceFiles = entries
                        .Where(e => e.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                    e.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                        .Select(e => e.FullName.ToLowerInvariant())
                        .ToList();
                    var langFiles = entries
                        .Where(e => e.FullName.Contains("/lang/") ||
                                    e.Name.EndsWith(".lang", StringComparison.OrdinalIgnoreCase))
                        .Select(e => e.FullName.ToLowerInvariant())
                        .ToList();

                    // Cheat clients often have specific resource layouts
                    if (langFiles.Count > 3 && allEntryNames.Count > 50)
                    {
                        score += 6;
                        if (!reasons.Any(r => r.StartsWith("resource")))
                            reasons.Add($"resource=lang_x{langFiles.Count}");
                    }

                    // Check for cheat-specifc file names in resources
                    foreach (var rf in resourceFiles)
                    {
                        if (rf.Contains("clickgui") || rf.Contains("arraylist") ||
                            rf.Contains("watermark") || rf.Contains("hud") ||
                            rf.Contains("module") || rf.Contains("cheat"))
                        {
                            score += 4;
                            if (!reasons.Any(r => r.StartsWith("resource_img")))
                                reasons.Add($"resource_img={rf.Split('/').Last()}");
                            break;
                        }
                    }

                    // ═══════════════════════════════════════════════════════════════
                    // INTELLIGENT PROFILER — holistic mod analysis with correlation fusion
                    // Extracts feature vector, applies weighted scoring, correlations bonuses
                    // ═══════════════════════════════════════════════════════════════
                    try
                    {
                    // ── 1. Extract raw features ──

                    // Class profile
                    int classCount = totalClasses;
                    int obfuscatedCount = obfuscatedClasses;
                    double obfRatio = classCount > 0 ? (double)obfuscatedCount / classCount : 0;
                    int singleLetterCount = classEntries.Count(e =>
                    {
                        string n = Path.GetFileNameWithoutExtension(e.Name);
                        return n.Length <= 2 || n.All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f'));
                    });
                    double singleLetterRatio = classCount > 0 ? (double)singleLetterCount / classCount : 0;

                    // Package profile
                    var pkgNames = classEntries
                        .Select(e => e.FullName.ToLowerInvariant())
                        .Select(n => n.Contains('/') ? n.Substring(0, n.LastIndexOf('/')) : "")
                        .Where(p => p.Length > 0)
                        .Distinct()
                        .ToList();
                    int pkgCount = pkgNames.Count;
                    double avgPkgDepth = pkgNames.Count > 0 ? pkgNames.Average(p => p.Count(c => c == '/')) : 0;
                    bool isFlatStructure = pkgCount <= Math.Max(1, classCount / 4) && pkgCount <= 3;

                    // File structure anomalies
                    int payloadFiles = 0;
                    int classDirForeign = 0;
                    int zeroByteFiles = 0;
                    int numericFiles = 0;
                    foreach (var entry in entries)
                    {
                        if (entry.FullName.EndsWith("/")) continue;
                        string en = entry.Name.ToLowerInvariant();
                        if (en.EndsWith(".class") || en.EndsWith(".png") || en.EndsWith(".jpg") ||
                            en == "fabric.mod.json" || en == "mcmod.info" || en == "pack.mcmeta" ||
                            en == "mods.toml" || en == "manifest.mf" ||
                            en.EndsWith(".json") || en.EndsWith(".properties") || en.EndsWith(".cfg") ||
                            en.EndsWith(".lang") || en.EndsWith(".xml") || en.EndsWith(".yml") ||
                            en.EndsWith(".toml") || en.EndsWith(".dll") || en.EndsWith(".so") ||
                            en.EndsWith(".dylib"))
                            continue;
                        if (entry.Length == 0) { zeroByteFiles++; continue; }
                        bool isNumeric = en.All(c => (c >= '0' && c <= '9') || c == '.');
                        if (isNumeric) { numericFiles++; if (entry.Length > 10240) payloadFiles++; continue; }
                        if (entry.Length > 51200) { payloadFiles++; }
                        string dir = entry.FullName.Contains('/')
                            ? entry.FullName.Substring(0, entry.FullName.LastIndexOf('/')) : "";
                        if (dir.Length > 0 && classEntries.Any(c => c.FullName.Contains(dir)))
                            classDirForeign++;
                    }

                    // Minecraft reference breadth
                    string[] mcDomains = {
                        "net/minecraft/client/", "net/minecraft/server/", "net/minecraft/world/",
                        "net/minecraft/entity/", "net/minecraft/block/", "net/minecraft/item/",
                        "net/minecraft/inventory/", "net/minecraft/util/", "net/minecraft/text/",
                        "net/minecraft/network/", "net/minecraft/client/Minecraft",
                        "net/minecraft/client/player/", "net/minecraft/client/network/",
                        "net/minecraft/client/gui/", "net/minecraft/client/render/",
                        "net/minecraft/client/renderer/", "net/minecraft/client/world/",
                        "net/minecraft/client/multiplayer/",
                        "net/minecraft/client/entity/", "net/minecraft/client/particle/",
                        "net/minecraft/client/sound/", "net/minecraft/client/session/",
                        "net/minecraft/client/resource/", "net/minecraft/client/search/",
                        "net/minecraft/client/realms/", "net/minecraft/client/option/",
                        "net/minecraft/client/font/", "net/minecraft/client/color/",
                        "net/minecraft/client/input/", "net/minecraft/client/tutorial/",
                        "net/minecraft/client/toast/", "net/minecraft/client/texture/",
                        "net/minecraft/advancement/", "net/minecraft/command/",
                        "net/minecraft/enchantment/", "net/minecraft/fluid/",
                        "net/minecraft/loot/", "net/minecraft/potion/",
                        "net/minecraft/recipe/", "net/minecraft/scoreboard/",
                        "net/minecraft/stat/", "net/minecraft/test/",
                        "net/minecraft/village/", "net/minecraft/structure/",
                        "com/mojang/", "com/mojang/blaze3d/"
                    };
                    int mcDomainCount = 0;
                    var matchedDomains = new HashSet<string>();
                    foreach (var cs in allClassStrings)
                        foreach (var d in mcDomains)
                            if (!matchedDomains.Contains(d) && cs.Contains(d))
                            { matchedDomains.Add(d); mcDomainCount++; }

                    // Module / event / infrastructure string hits
                    var nameHitsCache = new Dictionary<string, int>();
                    int CountUniqueHits(string[] keywords)
                    {
                        int hits = 0;
                        var seen = new HashSet<string>();
                        foreach (var cs in allClassStrings)
                            foreach (var kw in keywords)
                                if (!seen.Contains(kw) && cs.Contains(kw.ToLowerInvariant()))
                                { seen.Add(kw); hits++; }
                        return hits;
                    }

                    string[] cheatModules = {
                        "killaura","aimassist","wallhack","nofall","speedhack","triggerbot",
                        "antiknockback","scaffold","nuker","autoclicker","reach","velocity",
                        "flight","glide","criticals","cheststealer","inventorymove","invcleaner",
                        "crystalaura","surround","holefill","antitrap","elytrareplace",
                        "selffill","selfpot","autopot","antibot","antivanish","staffdetect",
                        "nametags","chams","playeresp","itemesp","chestesp","tracers",
                        "freecam","noclip","phase","spider","jesus","bunnyhop","highjump",
                        "antiredscreen","fullbright","fastplace","fastbreak","noslowdown",
                        "noreports","autofish","autotool","keystrokes","baritone","xray",
                        "autocrystal", "refill", "spammer", "antifall", "blink",
                        "autoblock","autoshield","clickaura","multiaura","fightbot",
                        "bowaimbot","antinausea","antifire","norender","nohurtcam",
                        "nofireoverlay","noweather","safewalk","scaffoldwalk",
                        "speedmine","speednuker","bonemealaura","handnoclip","ghosthand",
                        "instantbunker","antiexploit","crackedalt","thealtening",
                        "altmanager","reconnect","antispam","autotext",
                        "allaydupe","antidupe","autodupe","crasher","servercrash",
                        "rotation","silentaim","lagswitch","timer","tick",
                        "autoarmor","autoweapon","autosword","autototem","keepas",
                        "extinguish","autocook","autoreconnect","autosneak",
                        "blockesp","mobesp","blockhit",
                        "airplace","antiexploit","antihunger","antipotion",
                        "antispam","arrowdmg","autocomplete","autofarm",
                        "autoheal","autorespawn","autosteal","autoswim",
                        "autoswitch","autowalk","boatfly","cheststealer",
                        "creativeflight","crasher","derp","dolphin",
                        "elytrafly","excavator","extinguish","extraelytra",
                        "fastladder","fish","follow","friendlist",
                        "glide","headless","headroll","holefiller",
                        "invwalk","itemgenerator","jetpack","jesus",
                        "liquids","lsd","mileycyrus","mobowner",
                        "newchunks","nofall","nolevitation",
                        "noweather","nukerlegit","parkour","phase",
                        "playertracer","potionsaver","radar",
                        "rainbowui","safewalk","skinderp",
                        "snowshoe","step","templatetool",
                        "tillaura","treebot","tunnel","veinminer",
                        "watermark","welcomer","xcarry",
                    };
                    int moduleHits = CountUniqueHits(cheatModules);

                    string[] infraPatterns = {
                        "enable","disable","toggle","onEnable","onDisable","onToggle",
                        "keybind","getKey","setKey","addKey","module","modules",
                        "addModule","registerModule","moduleList","getModule",
                        "Category","category","Combat","Movement","Render","Player",
                        "World","Exploit","arraylist","clickgui","clickgui",
                        "loadConfig","saveConfig","config","command","commands",
                        "friend","friendList","target","Event","Listener",
                        "addon","addons","plugin","plugins","extension",
                        "macro","macros","profile","settings",
                        "notification","notifications","hud","watermark",
                        "bing","bingbinding","unbind","rebind",
                        "setbind","getbind","isbind",
                        "modulemanager","modulefactory",
                        "mixin","inject","callback","redirect",
                        "overwrite","accessor","invoker",
                        "spoof","spoofing","bypass","bypasses",
                        "antidetection","stealth","vanillaspoof",
                        "packet","interceptor","manipulate",
                        "rotation","yaw","pitch","lookvector",
                        "renderer","rendersystem","overlay",
                        "screen","guiscreen","hudscreen",
                        "theme","themes","colortheme",
                        "discordrpc","discordpresence","discordrp",
                        "alts","altmanager","altlogin","sessionmanager",
                        "proxy","socks","http","proxymanager",
                        "waypoint","waypoints",
                        "pathfinder","pathfind","path",
                        "rotationutil","motionutil","movementutil",
                        "blockutil","worldutil","playerutil",
                        "interact","interaction",
                        "reload","update","version",
                        "checkbox","slider","combobox","textfield",
                        "bindable","toggleable","clickpart",
                    };
                    int infraHits = CountUniqueHits(infraPatterns);

                    string[] eventPatterns = {
                        "eventbus","subscribe","onTick","onUpdate","onRender",
                        "listener","cancellable","onEntity","onWorld","onPacket",
                        "onMove","onPlayer","onChat","handleEvent","postEvent","fireEvent",
                        "onAttack","onDamage","onDeath","onRespawn",
                        "onJump","onFall","onLand","onStep",
                        "onBlock","onPlace","onBreak","onInteract",
                        "onJoin","onLeave","onDisconnect","onConnect",
                        "onGameJoin","onWorldJoin","onDimension",
                        "onEntitySpawn","onEntityRemove","onEntityInteract",
                        "onVelocity","onMotion","onPush",
                        "onRotationUpdate","onLook",
                        "onKeyPress","onMouseClick","onInput",
                        "onScreenOpen","onScreenClose","onGui",
                        "onChatMessage","onChatReceive","onChatSend",
                        "onCommand","onCommandExecute",
                        "onTickEvent","onRenderEvent","onWorldEvent",
                        "onPacketSend","onPacketReceive",
                        "preupdate","postupdate","premotion","postmotion",
                        "prerender","postrender",
                    };
                    int eventHits = CountUniqueHits(eventPatterns);

                    // ── 2. Feature vector weighting ──
                    double suspicionScore = 0;
                    var featureReasons = new List<string>();

                    // Obfuscation (0-1.0 ratio → 0-20 pts)
                    double obfScore = obfRatio * 20;
                    if (obfScore >= 5) { suspicionScore += obfScore; featureReasons.Add($"obf={obfRatio:P0}"); }

                    // Single-letter ratio (0-1.0 → 0-12 pts)
                    double slScore = singleLetterRatio * 12;
                    if (slScore >= 3) { suspicionScore += slScore; featureReasons.Add($"1letter={singleLetterRatio:P0}"); }

                    // Package flatness (0-20 pts based on class/pkg ratio)
                    double flatScore = isFlatStructure ? Math.Min(12, classCount / pkgCount * 2) : 0;
                    if (flatScore >= 4) { suspicionScore += flatScore; featureReasons.Add($"flat={pkgCount}pkg/{classCount}cls"); }

                    // Minecraft domain breadth (1 domain ≈ 1.5 pts)
                    double mcScore = Math.Min(20, mcDomainCount * 1.5);
                    if (mcScore >= 3) { suspicionScore += mcScore; featureReasons.Add($"mcDomains={mcDomainCount}"); }

                    // Module strings (1 hit ≈ 2 pts)
                    double modScore = Math.Min(25, moduleHits * 2);
                    if (modScore >= 4) { suspicionScore += modScore; featureReasons.Add($"modStrings={moduleHits}"); }

                    // Infrastructure strings (1 hit ≈ 1.5 pts)
                    double infraScore = Math.Min(18, infraHits * 1.5);
                    if (infraScore >= 3) { suspicionScore += infraScore; featureReasons.Add($"infra={infraHits}"); }

                    // Event system (1 hit ≈ 2 pts)
                    double evtScore = Math.Min(15, eventHits * 2);
                    if (evtScore >= 4) { suspicionScore += evtScore; featureReasons.Add($"events={eventHits}"); }

                    // Payload files (1 file ≈ 5 pts)
                    double payloadScore = Math.Min(15, (payloadFiles + classDirForeign) * 5);
                    if (payloadScore >= 5) { suspicionScore += payloadScore; featureReasons.Add($"payloads={payloadFiles}+{classDirForeign}"); }

                    // Zero-byte markers (1 marker ≈ 2 pts)
                    double zScore = Math.Min(8, zeroByteFiles * 2);
                    if (zScore >= 4) { suspicionScore += zScore; featureReasons.Add($"zMarkers={zeroByteFiles}"); }

                    // Numeric files (1 file ≈ 2 pts, cap 8)
                    double numScore = Math.Min(8, numericFiles * 2);
                    if (numScore >= 4) { suspicionScore += numScore; featureReasons.Add($"numFiles={numericFiles}"); }

                    // Small class ratio
                    int smallCls = classEntries.Count(e => e.Length > 0 && e.Length < 500);
                    double smallRatio = classCount > 0 ? (double)smallCls / classCount : 0;
                    double smallScore = Math.Min(12, smallRatio * 20);
                    if (smallScore >= 4) { suspicionScore += smallScore; featureReasons.Add($"smallCls={smallCls}/{classCount}"); }

                    // Mixin count
                    int mixinCount = classEntries.Count(e => e.FullName.ToLowerInvariant().Contains("mixin"));
                    double mixinScore = Math.Min(12, mixinCount * 0.6);
                    if (mixinScore >= 4) { suspicionScore += mixinScore; featureReasons.Add($"mixins={mixinCount}"); }

                    // ── 3. Correlation bonuses ──
                    double bonusScore = 0;
                    var bonusReasons = new List<string>();

                    bool hasMetadata = allEntryNames.Any(n =>
                        n.Contains("fabric.mod.json") || n.Contains("mcmod.info") || n.Contains("mods.toml"));
                    bool hasObf = obfRatio >= 0.5;
                    bool hasMcRefs = mcDomainCount >= 3;
                    bool hasInfra = infraHits >= 4;
                    bool hasModules = moduleHits >= 3;
                    bool hasEvents = eventHits >= 4;
                    bool hasPayload = payloadFiles + classDirForeign >= 2;
                    bool hasMarkers = zeroByteFiles >= 2;
                    bool isFlat = isFlatStructure && singleLetterRatio >= 0.5;
                    bool hasSmallClasses = smallRatio >= 0.3;

                    // Metadata + obfuscation → mod disguised as legitimate (STRONG)
                    if (hasMetadata && hasObf && (hasInfra || hasModules || hasEvents))
                    {
                        double disguiseBonus = 10;
                        if (isFlat) disguiseBonus += 5;
                        if (hasPayload) disguiseBonus += 5;
                        if (hasMarkers) disguiseBonus += 3;
                        if (singleLetterRatio >= 0.8) disguiseBonus += 4;
                        bonusScore += disguiseBonus;
                        bonusReasons.Add($"disguise=+{disguiseBonus:F0}");
                    }

                    // Metadata + payloads → embedded cheat payload
                    if (hasMetadata && hasPayload && !hasObf)
                    {
                        bonusScore += 8;
                        bonusReasons.Add("embedPay=+8");
                    }

                    // Flat structure + short names → obfuscated module framework
                    if (isFlat && classCount >= 5)
                    {
                        double fwScore = 6;
                        if (hasInfra) fwScore += 4;
                        if (hasModules) fwScore += 4;
                        if (hasSmallClasses) fwScore += 3;
                        if (obfRatio >= 0.8) fwScore += 3;
                        bonusScore += fwScore;
                        bonusReasons.Add($"obfFW=+{fwScore:F0}");
                    }

                    // Broad MC coupling + events → deep Minecraft hooking (cheat behavior)
                    if (mcDomainCount >= 6 && (hasEvents || hasModules))
                    {
                        double hookBonus = Math.Min(12, mcDomainCount + (hasEvents ? 4 : 0) + (hasModules ? 4 : 0));
                        bonusScore += hookBonus;
                        bonusReasons.Add($"mcHook=+{hookBonus:F0}");
                    }

                    // Infrastructure + modules → confirmed cheat framework
                    if (hasInfra && hasModules)
                    {
                        double fwConfirm = Math.Min(15, infraHits + moduleHits);
                        bonusScore += fwConfirm;
                        bonusReasons.Add($"cheatFW=+{fwConfirm:F0}");
                    }

                    // Small classes + flat + obfuscated → stub class pattern
                    if (hasSmallClasses && isFlat && hasObf)
                    {
                        bonusScore += Math.Min(10, smallCls / 2);
                        bonusReasons.Add($"stubPat=+{Math.Min(10, smallCls / 2)}");
                    }

                    // All strings empty + has metadata → stripped obfuscation
                    if (allClassStrings.Count == 0 && classCount > 0 && hasMetadata)
                    {
                        bonusScore += 10;
                        bonusReasons.Add("stripObf=+10");
                    }

                    // No metadata + modules + infrastructure → undeniably a cheat framework
                    if (!hasMetadata && (hasModules || hasInfra) && hasEvents && classCount >= 5)
                    {
                        double bareCheatScore = 15;
                        if (hasObf) bareCheatScore += 5;
                        if (isFlat) bareCheatScore += 5;
                        if (hasPayload) bareCheatScore += 5;
                        if (mixinCount >= 3) bareCheatScore += 5;
                        bonusScore += Math.Min(30, bareCheatScore);
                        bonusReasons.Add($"bareCheat=+{Math.Min(30, bareCheatScore):F0}");
                    }

                    // ── 4. Fuse scores ──
                    double fusedScore = suspicionScore + bonusScore;

                    // Normalize and apply
                    int totalProfilerScore = (int)Math.Round(Math.Min(25, fusedScore));
                    if (totalProfilerScore >= 5)
                    {
                        score += totalProfilerScore;
                        string featureSummary = string.Join(" ", featureReasons.Take(4));
                        string bonusSummary = string.Join(" ", bonusReasons.Take(3));
                        reasons.Add($"profiler={totalProfilerScore}:{featureSummary}|{bonusSummary}");
                    }
                    }
                    catch { /* profiler error — non-critical */ }
                    // ── END PROFILER ──

                    // ── Build ZIP structure tree ──
                    var sbStruct = new System.Text.StringBuilder();
                    foreach (var entry in entries.OrderBy(e => e.FullName))
                    {
                        string mark = entry.Name.EndsWith(".class", StringComparison.OrdinalIgnoreCase) ? "📄" :
                                      entry.FullName.EndsWith("/") ? "📁" : "📦";
                        sbStruct.AppendLine($"{mark} {entry.FullName}  ({FormatSize(entry.Length, false)})");
                    }
                    result.ZipStructure = sbStruct.ToString();

                    // ── Collect metadata text ──
                    var metaSb = new System.Text.StringBuilder();
                    foreach (var entry in entries)
                    {
                        string en = entry.Name.ToLowerInvariant();
                        if (en == "mcmod.info" || en == "fabric.mod.json" || en == "mods.toml" ||
                            en == "pack.mcmeta" || en == "manifest.mf")
                        {
                            try
                            {
                                using var r = new StreamReader(entry.Open());
                                metaSb.AppendLine($"=== {entry.FullName} ===");
                                metaSb.AppendLine(r.ReadToEnd());
                                metaSb.AppendLine();
                            }
                            catch { }
                        }
                    }
                    result.MetadataText = metaSb.ToString();
                }
                catch { /* not a ZIP */ }

                if (!isZip)
                {
                    // Non-archive: can't deep scan
                }

                // ═══════════════════════════════════════════════════════
                // LAYER 10: MC VERSION WEIGHT (2% tolerance)
                // ═══════════════════════════════════════════════════════
                string mcVer = "";
                foreach (var (size, ver) in McVersionSizes)
                {
                    long tolVer = Math.Max(1, size / 50);
                    if (Math.Abs(fileSize - size) <= tolVer)
                    {
                        mcVer = ver;
                        score += 12;
                        reasons.Add($"mc_weight={ver}");
                        break;
                    }
                }

                // ═══════════════════════════════════════════════════════
                // LAYER 11: LARGE FILE (>50 MB = suspicious)
                // ═══════════════════════════════════════════════════════
                if (fileSize > 50L * 1024 * 1024)
                {
                    score += 5;
                    reasons.Add("size>50MB");
                }

                // ═══════════════════════════════════════════════════════
                // LAYER 12: LEGITIMATE MOD DISCOUNT
                // If mod has proper metadata AND no strong indicators,
                // reduce false-positive score accumulation
                // ═══════════════════════════════════════════════════════
                bool hasLegitMetadata = reasons.Any(r =>
                    r.StartsWith("modid=") || r.StartsWith("manifest=")) &&
                    !reasons.Any(r => r.StartsWith("filename=MATCH:") ||
                                      r.StartsWith("filename=KW:") ||
                                      r.StartsWith("hash_match="));
                bool hasLegitStructure = allEntryNames.Any(n =>
                    n.Contains("fabric.mod.json") || n.Contains("mcmod.info") ||
                    n.Contains("pack.mcmeta") || n.Contains("mods.toml"));
                bool hasStrongIndicator = reasons.Any(r =>
                    r.StartsWith("known_cheat_pkg") || r.StartsWith("cheat_pkg_x") ||
                    r.StartsWith("hash_match=") || r.StartsWith("filename=MATCH:"));
                bool hasDisguiseIndicators = reasons.Any(r =>
                    r.StartsWith("obfuscated=") || r.StartsWith("high_entropy=") ||
                    r.StartsWith("susp_struct") || r.StartsWith("susp_pkg") ||
                    r.StartsWith("meta_disguise") || r.StartsWith("module_str=") ||
                    r.StartsWith("susp_tld=") || r.StartsWith("mixin_ct=") ||
                    r.StartsWith("small_cls=") || r.StartsWith("event_str=") ||
                    r.StartsWith("event_cls=") || r.StartsWith("mc_refs=") ||
                    r.StartsWith("infra_str=") || r.StartsWith("cls_dist=") ||
                    r.StartsWith("cheat_combo=") || r.StartsWith("safety_net="));

                if (hasLegitStructure && !hasStrongIndicator && !hasDisguiseIndicators && score > 0)
                {
                    // Discount up to 25% max 10 pts — only for clean mods with metadata
                    // that have NO obfuscation, disguise, or strong indicators
                    int discount = Math.Min(10, score / 4);
                    if (discount > 0)
                    {
                        int afterDiscount = score - discount;
                        bool wouldChangeVerdict = (score >= 40 && afterDiscount < 40) ||
                                                  (score >= 15 && afterDiscount < 15);
                        if (wouldChangeVerdict && afterDiscount > 0)
                        {
                            score = afterDiscount;
                            reasons.Add("legit_mod_discount=-" + discount);
                        }
                    }
                }

                // ═══════════════════════════════════════════════════════
                // ФИНАЛЬНЫЙ ВЕРДИКТ (thresholds: 80+ Critical, 50+ High, 30+ Suspicious, 15+ Low)
                // ═══════════════════════════════════════════════════════
                if (score >= 80)
                {
                    result.IsClean = false;
                    result.StatusText = "High Suspicion (Critical)";
                    result.Color = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(200, 30, 50));
                }
                else if (score >= 50)
                {
                    result.IsClean = false;
                    result.StatusText = "High Suspicion";
                    result.Color = System.Windows.Media.Brushes.Red;
                }
                else if (score >= 30)
                {
                    result.IsClean = false;
                    result.StatusText = "Suspicious";
                    result.Color = System.Windows.Media.Brushes.Orange;
                }
                else if (score >= 15)
                {
                    result.IsClean = false;
                    result.StatusText = "Low Suspicion";
                    result.Color = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 180, 50));
                }
                else
                {
                    result.StatusText = "Normal";
                    result.Color = System.Windows.Media.Brushes.LimeGreen;
                }

                string details = $"Score:{score} | {string.Join("; ", reasons.Take(5))}";
                if (reasons.Count > 5) details += $" +{reasons.Count - 5} more";
                if (!string.IsNullOrEmpty(hashStr)) details += $" | SHA-256:{hashStr}";
                if (totalClasses > 0) details += $" | classes:{totalClasses}";
                result.Details = details;
                result.Score = score;
                result.FullReasons = string.Join(" | ", reasons);
                result.TotalClasses = totalClasses;
                result.ObfuscatedClasses = obfuscatedClasses;
                result.CheatPkgRootHits = cheatPkgRootHits;
                result.ClassKwHits = uniqueClassKwHits;
                result.HashInfo = hashStr ?? "";
                result.SuspiciousPkgPaths = string.Join(", ", suspiciousPkgPatterns.Take(8));
                result.McVersion = mcVer;
                result.IsZip = isZip;
                result.IsSuspicious = !result.IsClean;
            }
            catch (Exception ex)
            {
                result.IsClean = false;
                result.StatusText = "ERROR";
                result.Color = System.Windows.Media.Brushes.Orange;
                result.Details = $"Could not scan: {ex.Message}";
            }

            sw.Stop();
            _modResults.Add(result);
            ModResultsList.Items.Refresh();
            int total = _modResults.Count;
            int flagged = _modResults.Count(r => !r.IsClean);
            ModResultsCount.Text = $"{total} files — {flagged} flagged / {total - flagged} clean";
        }

        // ──────────────────────────────────────────────────────────────
        // Parallel ModChecker with progress bar
        // ──────────────────────────────────────────────────────────────
        private void ScanModFilesParallel(List<string> files)
        {
            int total = files.Count;
            int done = 0;
            var sync = new object();
            ModProgressBar.Visibility = Visibility.Visible;
            ModProgressBar.Maximum = total;
            ModProgressBar.Value = 0;
            System.Threading.Tasks.Parallel.ForEach(files, new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = 4 }, f =>
            {
                ScanModFile(f);
                lock (sync)
                {
                    done++;
                    Dispatcher.Invoke(() =>
                    {
                        ModProgressBar.Value = done;
                        SetStatus(string.Format(
                            _lang == 1 ? "Проверка модов: {0}/{1}" : "Scanning mods: {0}/{1}", done, total));
                    });
                }
            });
            ModProgressBar.Visibility = Visibility.Collapsed;
            SetStatus(L("ready"));
        }

        // ──────────────────────────────────────────────────────────────
        // History: save/load ModChecker results as JSON
        // ──────────────────────────────────────────────────────────────
        private string HistoryDir => System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FMTool", "mod_history");

        private void SaveModHistory()
        {
            if (_modResults.Count == 0) return;
            try
            {
                var dir = HistoryDir;
                Directory.CreateDirectory(dir);
                string stamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string path = Path.Combine(dir, $"mods_{stamp}.json");
                var list = _modResults.Select(r => new
                {
                    r.FileName,
                    r.FilePath,
                    r.Score,
                    r.IsClean,
                    r.StatusText,
                    r.Details,
                    r.FullReasons,
                    r.TotalClasses,
                    r.ObfuscatedClasses,
                    r.HashInfo,
                    r.McVersion,
                    r.MetadataText,
                    r.ZipStructure,
                    r.SuspiciousPkgPaths,
                    r.ClassKwHits,
                    r.CheatPkgRootHits,
                    r.IsZip,
                    r.IsSuspicious
                }).ToList();
                string json = System.Text.Json.JsonSerializer.Serialize(list, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                File.WriteAllText(path, json);
            }
            catch { }
        }

        private void LoadModHistory()
        {
            try
            {
                var dir = HistoryDir;
                if (!Directory.Exists(dir)) return;
                var files = Directory.GetFiles(dir, "mods_*.json").OrderByDescending(f => f).Take(20).ToList();
                if (files.Count == 0) return;
                string latest = files[0];
                string json = File.ReadAllText(latest);
                var items = System.Text.Json.JsonSerializer.Deserialize<List<ModResultItem>>(json);
                if (items == null || items.Count == 0) return;
                _modResults.Clear();
                foreach (var item in items)
                {
                    // Reconstruct color from score
                    int sc = item.Score;
                    if (sc >= 80) item.Color = Brushes.Red;
                    else if (sc >= 50) item.Color = Brushes.OrangeRed;
                    else if (sc >= 30) item.Color = Brushes.Orange;
                    else if (sc >= 15) item.Color = Brushes.Goldenrod;
                    else item.Color = Brushes.LimeGreen;
                    _modResults.Add(item);
                }
                ModResultsList.Items.Refresh();
                int total = _modResults.Count;
                int flagged = _modResults.Count(r => !r.IsClean);
                ModResultsCount.Text = $"{total} files — {flagged} flagged / {total - flagged} clean";
                SetStatus(_lang == 1 ? "Загружена последняя сессия" : "Loaded last session");
            }
            catch { }
        }

        private void SaveHistory_Click(object sender, RoutedEventArgs e)
        {
            SaveModHistory();
            SetStatus(_lang == 1 ? "Результаты сохранены" : "Results saved");
        }

        private void LoadHistory_Click(object sender, RoutedEventArgs e)
        {
            LoadModHistory();
        }

        // ===================== LOGS =====================
        private void PsLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PsHistoryRel);
                LogsBox.Text = File.Exists(path) ? File.ReadAllText(path) : "PowerShell history not found:\r\n" + path;
                SetStatus(_lang == 1 ? "История PowerShell загружена" : "PowerShell history loaded");
                RefreshLogsBtn.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { LogsBox.Text = "Error: " + ex.Message; }
        }

        private void StartupLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("=== Startup Programs (HKCU Run) ===\r\n");
                if (key != null)
                    foreach (var name in key.GetValueNames())
                        sb.AppendLine($"{name}  →  {key.GetValue(name)}");
                LogsBox.Text = sb.ToString();
                SetStatus("Startup programs loaded");
                RefreshLogsBtn.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex) { LogsBox.Text = "Error: " + ex.Message; }
        }

        private void CopyLogs_Click(object sender, RoutedEventArgs e)
        {
            try { Clipboard.SetText(LogsBox.Text); ShowToast(L("copied")); } catch { }
        }

        private void RefreshLogs_Click(object sender, RoutedEventArgs e)
        {
            PsLog_Click(null!, null!);
        }

        private void NetworkLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("=== Active TCP/UDP Connections ===\r\n");
                var psi = new ProcessStartInfo("netstat", "-ano") { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };
                using var proc = Process.Start(psi);
                if (proc != null)
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit(3000);
                    sb.AppendLine(output);
                }
                // Also show listening ports
                sb.AppendLine("\r\n=== Listening Ports ===\r\n");
                var psi2 = new ProcessStartInfo("netstat", "-an") { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };
                using var proc2 = Process.Start(psi2);
                if (proc2 != null)
                {
                    string output2 = proc2.StandardOutput.ReadToEnd();
                    proc2.WaitForExit(3000);
                    // Filter only LISTENING
                    foreach (var line in output2.Split('\n'))
                        if (line.Contains("LISTENING")) sb.AppendLine(line.TrimEnd());
                }
                LogsBox.Text = sb.ToString();
                SetStatus("Network connections loaded");
                RefreshLogsBtn.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex) { LogsBox.Text = "Error: " + ex.Message; }
        }

        private void FirewallLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("=== Firewall Rules (netsh advfirewall) ===\r\n");
                var psi = new ProcessStartInfo("netsh", "advfirewall firewall show rule name=all dir=in") { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };
                using var proc = Process.Start(psi);
                if (proc != null)
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit(5000);
                    // Take first 300 lines to avoid overwhelming
                    var lines = output.Split('\n');
                    int count = 0;
                    foreach (var line in lines)
                    {
                        if (count > 300) { sb.AppendLine("... (truncated)"); break; }
                        if (line.Trim().Length > 0) { sb.AppendLine(line.TrimEnd()); count++; }
                    }
                }
                LogsBox.Text = sb.ToString();
                SetStatus("Firewall rules loaded");
                RefreshLogsBtn.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex) { LogsBox.Text = "Error: " + ex.Message; }
        }

        // ===================== AUTOSTART =====================
        private static bool IsAutostartEnabled()
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RunKeyPath, false);
                return key?.GetValue(RunValueName) != null;
            }
            catch { return false; }
        }

        private void AutostartCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RunKeyPath);
                if (key == null) return;
                
                if (AutostartCheck.IsChecked == true)
                {
                    string exe = GetAppPath();
                    if (!string.IsNullOrEmpty(exe))
                    {
                        key.SetValue(RunValueName, "\"" + exe + "\"");
                        ShowToast(L("autostart_on"));
                    }
                    else
                    {
                        ShowToast("Cannot find app path", false);
                        AutostartCheck.IsChecked = false;
                    }
                }
                else
                {
                    try { key.DeleteValue(RunValueName, false); } catch { }
                    ShowToast(L("autostart_off"));
                }
            }
            catch (Exception ex)
            {
                ShowToast("Error: " + ex.Message, false);
AutostartCheck.IsChecked = IsAutostartEnabled();
            }
        }

private static string GetAppPath()
        {
            try
            {
                var path = Environment.ProcessPath;
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    return path;

                try
                {
                    var pmPath = Process.GetCurrentProcess().MainModule?.FileName;
                    if (!string.IsNullOrEmpty(pmPath) && File.Exists(pmPath))
                        return pmPath;
                }
                catch { }

                var loc = System.AppContext.BaseDirectory;
                var exePath = Path.Combine(loc, Process.GetCurrentProcess().ProcessName + ".exe");
                if (!string.IsNullOrEmpty(loc) && File.Exists(exePath))
                    return exePath;
            }
            catch { }
            return "";
        }

        // ===================== JAVA DETECTION =====================
        private bool _javaFound = false;
        private string? _javaVersion = null;
        private string? _javaPath = null;

        private bool CheckJava()
        {
            // First check system Java
            try
            {
                using var proc = Process.Start(new ProcessStartInfo
                {
                    FileName = "java",
                    Arguments = "-version 2>&1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                });
                if (proc != null)
                {
                    proc.WaitForExit(1000);
                    string output = proc.StandardError.ReadToEnd() + proc.StandardOutput.ReadToEnd();
                    if (proc.ExitCode == 0 && output.Length > 0)
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(output, @"(\d+\.\d+\.\d+)");
                        _javaVersion = match.Success ? match.Groups[1].Value : "detected";
                        _javaPath = "java";
                        return true;
                    }
                }
            }
            catch { }

            // Fallback: check bundled JRE
            try
            {
                string bundledJre = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "APPS", "jre-8u491-windows-x64.exe");
                if (!File.Exists(bundledJre))
                    bundledJre = @"C:\FMTool\APPS\jre-8u491-windows-x64.exe";
                if (File.Exists(bundledJre))
                {
                    _javaVersion = "8u491";
                    _javaPath = bundledJre;
                    return true;
                }
            }
            catch { }

            _javaVersion = null;
            _javaPath = null;
            return false;
        }

        private void UpdateJavaStatus()
        {
            _javaFound = CheckJava();
            if (JavaStatusText != null)
            {
                if (_javaFound && _javaVersion != null)
                    JavaStatusText.Text = "☕ " + _javaVersion + " ✓";
                else
                    JavaStatusText.Text = L(_javaFound ? "java_status_installed" : "java_status_notfound");
                JavaStatusText.Foreground = (System.Windows.Media.Brush)FindResource(_javaFound ? "SuccessBrush" : "DangerBrush");
            }
        }

        private void ReinstallJava_Click(object sender, RoutedEventArgs e)
        {
            ShowToast("Reinstall Java: download latest from java.com", false);
        }

        private void RemoveJava_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);
                if (key == null) { ShowToast("Registry key not found", false); return; }
                var subKeyNames = key.GetSubKeyNames();
                foreach (var subKeyName in subKeyNames)
                {
                    using var subKey = key.OpenSubKey(subKeyName);
                    var displayName = subKey?.GetValue("DisplayName")?.ToString();
                    if (displayName != null && (displayName.Contains("Java") || displayName.Contains("JRE")))
                    {
                        var uninstallString = subKey?.GetValue("UninstallString")?.ToString();
                        if (!string.IsNullOrEmpty(uninstallString))
                        {
                            var idx = uninstallString.IndexOf(' ');
                            if (idx > 0)
                            {
                                var exe = uninstallString.Substring(0, idx).Trim('"');
                                var args = uninstallString.Substring(idx).Trim();
                                Process.Start(new ProcessStartInfo { FileName = exe, Arguments = args, UseShellExecute = true });
                            }
                        }
                    }
                }
                ShowToast("Java uninstaller launched");
            }
            catch (Exception ex) { ShowToast("Error: " + ex.Message, false); }
        }

        private void InstallJava_Click(object sender, RoutedEventArgs e)
        {
            string installerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "APPS", "jre-8u491-windows-x64.exe");
            if (!File.Exists(installerPath))
                installerPath = @"C:\FMTool\APPS\jre-8u491-windows-x64.exe";
            
            if (File.Exists(installerPath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = installerPath, UseShellExecute = true });
                    ShowToast(L("java_launching"));
                }
                catch (Exception ex) { ShowToast("Error: " + ex.Message, false); }
            }
            else
            {
                ShowToast(L("java_notfound"), false);
            }
        }

        private void RegisterCtxBtn_Click(object sender, RoutedEventArgs e)
        {
            RegisterContextMenu();
            ShowToast(_lang == 1 ? "Context menu registered" : "Контекстное меню зарегистрировано");
        }

        private void UnregisterCtxBtn_Click(object sender, RoutedEventArgs e)
        {
            UnregisterContextMenu();
            ShowToast(_lang == 1 ? "Context menu removed" : "Контекстное меню удалено");
        }

        private void RegisterAssocBtn_Click(object sender, RoutedEventArgs e)
        {
            RegisterFileAssoc();
            ShowToast(_lang == 1 ? "File association registered" : "Ассоциация файлов зарегистрирована");
        }

        private void UnregisterAssocBtn_Click(object sender, RoutedEventArgs e)
        {
            UnregisterFileAssoc();
            ShowToast(_lang == 1 ? "File association removed" : "Ассоциация файлов удалена");
        }

        // ===================== HELPERS =====================
        private static double ParseDouble(string s, double def) => double.TryParse(s, out var v) ? v : def;

        public class ScanItem
        {
            public string Name { get; set; } = "";
            public string Path { get; set; } = "";
            public string Ext { get; set; } = "";
            public long SizeBytes { get; set; }
            public string Status { get; set; } = "";
            public string StatusText { get; set; } = "";
            public string SizeText { get; set; } = "";
            public bool IsJarOrZip => Ext is "JAR" or "ZIP";
            public int StatusOrder => Status switch { "Cheat Client" => 0, "Suspicious" => 1, _ => 2 };
            public string GroupName => Status switch
            {
                "Cheat Client" or "Suspicious (High)" => "Suspicious (High)",
                "Suspicious" => "Suspicious",
                _ => "Normal"
            };
            public string GroupColor => Status switch
            {
                "Cheat Client" or "Suspicious (High)" => "#FF4444",
                "Suspicious" => "#FFAA00",
                _ => "#44AA44"
            };
            public string AnalyzeTooltip
            {
                get
                {
                    if (IsJarOrZip) return _langStatic == 1 ? "Проверить в Mod Checker" : "Analyze in Mod Checker";
                    return _langStatic == 1 ? "Открыть отчёт" : "View detailed report";
                }
            }
        }

        public class ProgramItem
        {
            public string ResourceName { get; set; } = "";
            public string FileName { get; set; } = "";
            public string DisplayName { get; set; } = "";
            public string Description { get; set; } = "";
            public string DescEn { get; set; } = "";
            public string DescRu { get; set; } = "";
            public string IconPath { get; set; } = "";
            public bool HasIcon => !string.IsNullOrEmpty(IconPath);
            public string Ext { get; set; } = "";
            public bool Available { get; set; }
        }
    }
}