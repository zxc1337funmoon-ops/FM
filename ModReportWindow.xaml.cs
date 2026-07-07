using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FMTool
{
    public partial class ModReportWindow : Window
    {
        private MainWindow.ModResultItem? _item;
        private int _lang = 0;

        public ModReportWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var owner = Owner as MainWindow;
            if (owner != null)
            {
                // Detect language from parent
                _lang = owner.GetLanguage();
                Left = owner.Left + Math.Max(0, (owner.Width - Width) / 2);
                Top = owner.Top + Math.Max(0, (owner.Height - Height) / 2);
            }

            // Fade + slide animation
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(320))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
            BeginAnimation(OpacityProperty, fadeIn);
            var slideIn = new DoubleAnimation(20, 0, TimeSpan.FromMilliseconds(350))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
            var content = Content as FrameworkElement;
            if (content != null)
            {
                content.BeginAnimation(TranslateTransform.YProperty, null);
                var tt2 = new TranslateTransform();
                content.RenderTransform = tt2;
                tt2.BeginAnimation(TranslateTransform.YProperty, slideIn);
            }

            // Localize tabs, buttons, tooltips
            ReportTitle.Text = L("Mod Analysis Report", "Анализ мода");
            TabReport.Header = L("Detection Report", "Детект");
            TabClasses.Header = L("Class Browser", "Классы");
            TabStructure.Header = L("ZIP Structure", "Структура ZIP");
            TabMetadata.Header = L("Metadata", "Метаданные");
            ClassListTitle.Text = L("Classes", "Классы");
            LegendBanned.Text = L("High Suspicion", "Высокое подозрение");
            LegendSusp.Text = L("Suspicious", "Подозр.");
            LegendClean.Text = L("Normal", "Нормальный");
            ExtractBtn.Content = L("📦 Extract JAR...", "📦 Извлечь JAR...");
            ExportBtn.Content = L("💾 Save Report", "💾 Сохранить отчёт");
            CloseBtn2.Content = L("× Close", "× Закрыть");
            LuytenBtn.Content = L("☕ Open with Luyten", "☕ Открыть в Luyten");
            CloseBtn.ToolTip = L("Close (Esc)", "Закрыть (Esc)");
            LegendDotHigh.ToolTip = L("High suspicion indicators", "Индикаторы высокого риска");
            LegendDotSusp.ToolTip = L("Suspicious indicators", "Подозрительные индикаторы");
            LegendDotNorm.ToolTip = L("Normal indicators", "Нормальные индикаторы");
        }

        // Helper: get localized string
        private string L(string en, string ru)
        {
            return _lang == 1 ? ru : en;
        }

        public void LoadReport(MainWindow.ModResultItem item)
        {
            _item = item;
            ReportFileName.Text = item.FileName;

            // Header
            HeaderDot.Background = item.Color;
            VerdictLabel.Text = item.StatusText;
            VerdictLabel.Foreground = item.Color;
            ScoreLabel.Text = L("Score:", "Баллы:") + " " + item.Score;

            // Build detection report (Tab 1)
            BuildReportTab(item);

            // Build class browser (Tab 2)
            BuildClassTab(item);

            // Build ZIP structure (Tab 3)
            BuildStructureTab(item);

            // Build metadata (Tab 4)
            BuildMetadataTab(item);
        }

        private void BuildReportTab(MainWindow.ModResultItem item)
        {
            ReportBody.Children.Clear();

            // Summary card
            var summaryCard = new Border
            {
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(14),
                Margin = new Thickness(0, 0, 0, 14),
                Background = new SolidColorBrush(Color.FromArgb((byte)15, (byte)255, (byte)255, (byte)255))
            };
            var summaryGrid = new Grid();
            summaryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            summaryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            summaryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var scoreBox = MakeStatBox(L("Score", "Баллы"), item.Score.ToString(),
                item.StatusText.Contains("Critical") ? "#C81E32" : item.StatusText.Contains("High Suspicion") ? "#FF4444" : item.StatusText == "Suspicious" ? "#FFAA00" : item.StatusText == "Low Suspicion" ? "#FFB432" : "#44AA44");
            Grid.SetColumn(scoreBox, 0);
            summaryGrid.Children.Add(scoreBox);

            var classBox = MakeStatBox(L("Classes", "Классы"), item.TotalClasses.ToString(),
                item.TotalClasses > 0 ? "#88CCFF" : "#666666");
            Grid.SetColumn(classBox, 1);
            summaryGrid.Children.Add(classBox);

            var verdictBox = MakeStatBox(L("Verdict", "Вердикт"), item.StatusText,
                item.StatusText.Contains("Critical") ? "#C81E32" : item.StatusText.Contains("High Suspicion") ? "#FF4444" : item.StatusText == "Suspicious" ? "#FFAA00" : item.StatusText == "Low Suspicion" ? "#FFB432" : "#44AA44");
            Grid.SetColumn(verdictBox, 2);
            summaryGrid.Children.Add(verdictBox);

            summaryCard.Child = summaryGrid;
            ReportBody.Children.Add(summaryCard);

            // ── Conclusion Summary ──
            if (item.Score > 0 || !string.IsNullOrEmpty(item.FullReasons))
            {
                var reasons = !string.IsNullOrEmpty(item.FullReasons)
                    ? item.FullReasons.Split('|').Select(r => r.Trim()).Where(r => r.Length > 0).ToList()
                    : new List<string>();

                var conclusionSb = new System.Text.StringBuilder();
                int score = item.Score;
                if (score >= 80) conclusionSb.Append(L("CRITICAL — ", "КРИТИЧНО — "));
                else if (score >= 50) conclusionSb.Append(L("HIGH SUSPICION — ", "ВЫСОКОЕ ПОДОЗРЕНИЕ — "));
                else if (score >= 30) conclusionSb.Append(L("SUSPICIOUS — ", "ПОДОЗРИТЕЛЬНО — "));
                else if (score >= 15) conclusionSb.Append(L("LOW SUSPICION — ", "НИЗКОЕ ПОДОЗРЕНИЕ — "));
                else conclusionSb.Append(L("NORMAL — ", "НОРМАЛЬНО — "));

                var topReasons = reasons.Take(3).Select(GetReasonSummary).Where(s => s.Length > 0).ToList();
                if (topReasons.Count > 0)
                    conclusionSb.Append(L("Main reasons: ", "Основные причины: ") + string.Join("; ", topReasons));
                else
                    conclusionSb.Append(L("No significant issues detected", "Значимых проблем не обнаружено"));

                ReportBody.Children.Add(MakeSectionHeader(
                    L("Conclusion", "Заключение"), ""));
                ReportBody.Children.Add(MakeSeparator());
                ReportBody.Children.Add(MakeReasonRow(conclusionSb.ToString(),
                    score >= 50 ? Brushes.Red : score >= 30 ? Brushes.Orange : score >= 15 ? Brushes.Goldenrod : Brushes.LimeGreen));
            }

            // ── Weighted Scoring Breakdown ──
            if (!string.IsNullOrEmpty(item.FullReasons))
            {
                var reasons = item.FullReasons.Split('|').Select(r => r.Trim()).Where(r => r.Length > 0).ToList();
                if (reasons.Count > 0)
                {
                    ReportBody.Children.Add(MakeSectionHeader(
                        L("🔍 Analysis: Weighted Scoring", "🔍 Анализ: Взвешенный расчёт"), ""));
                    ReportBody.Children.Add(MakeSeparator());

                    // Build weighted category breakdown
                    foreach (var cat in BuildWeightedBreakdown(reasons))
                    {
                        ReportBody.Children.Add(cat);
                    }
                }
            }

            // Additional info cards
            if (item.TotalClasses > 0 || item.ObfuscatedClasses > 0)
            {
                ReportBody.Children.Add(MakeSectionHeader(
                    L("Class Analysis", "Анализ классов"), ""));
                ReportBody.Children.Add(MakeSeparator());
                var infoGrid = new WrapGrid
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 6, 0, 6)
                };
                infoGrid.Children.Add(MakeInfoChip(L("Total classes", "Всего классов"), item.TotalClasses.ToString()));
                infoGrid.Children.Add(MakeInfoChip(L("Obfuscated", "Обфусцировано"), item.ObfuscatedClasses.ToString()));
                if (item.TotalClasses > 0)
                    infoGrid.Children.Add(MakeInfoChip(L("Obfuscation rate", "Уровень обфускации"),
                        $"{(double)item.ObfuscatedClasses / item.TotalClasses * 100:F1}%"));
                ReportBody.Children.Add(infoGrid);
            }

            if (item.ClassKwHits > 0)
            {
                ReportBody.Children.Add(MakeSectionHeader(
                    L("Keyword Matches", "Совпадения по ключевым словам"), ""));
                ReportBody.Children.Add(MakeSeparator());
                ReportBody.Children.Add(MakeReasonRow(
                    L("Unique keyword hits in class pool:", "Уникальных ключевых слов в пуле классов:") + " " + item.ClassKwHits,
                    item.ClassKwHits >= 4 ? Brushes.OrangeRed : Brushes.Goldenrod));
            }

            if (!string.IsNullOrEmpty(item.McVersion))
            {
                ReportBody.Children.Add(MakeSectionHeader(
                    L("Minecraft Version", "Версия Minecraft"), ""));
                ReportBody.Children.Add(MakeSeparator());
                ReportBody.Children.Add(MakeReasonRow(
                    L("Detected:", "Обнаружена:") + " " + item.McVersion, Brushes.LightGreen));
            }

            if (!string.IsNullOrEmpty(item.SuspiciousPkgPaths))
            {
                ReportBody.Children.Add(MakeSectionHeader(
                    L("Suspicious Package Paths", "Подозрительные пути пакетов"), ""));
                ReportBody.Children.Add(MakeSeparator());
                ReportBody.Children.Add(MakeReasonRow(item.SuspiciousPkgPaths, Brushes.SandyBrown));
            }

            if (!string.IsNullOrEmpty(item.HashInfo))
            {
                ReportBody.Children.Add(MakeSectionHeader(L("Fingerprints", "Отпечатки"), ""));
                ReportBody.Children.Add(MakeSeparator());
                ReportBody.Children.Add(MakeReasonRow(L("SHA-256:", "SHA-256:") + " " + item.HashInfo, Brushes.Gray));
            }

            ReportBody.Children.Add(MakeSectionHeader(
                L("Archive Info", "Информация об архиве"), ""));
            ReportBody.Children.Add(MakeSeparator());
            ReportBody.Children.Add(MakeReasonRow(
                L("Type:", "Тип:") + " " + (item.IsZip ? "ZIP/JAR" : L("Not an archive", "Не архив")), Brushes.Gray));

            // Fade-in animation for the report
            ReportBody.Opacity = 0;
            var fadeIn = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            ReportBody.BeginAnimation(OpacityProperty, fadeIn);
        }

        private (int Points, int Severity) ParseReasonPoints(string r)
        {
            // Returns (points, severity) where 0=critical, 1=high, 2=medium, 3=low
            // Matches exact scoring from ScanModFile in MainWindow.xaml.cs

            // Layer 1: Filename
            if (r.StartsWith("filename=MATCH:")) return (60, 0);
            if (r.StartsWith("filename=KW:")) return (15, 1);

            // Layer 2: Manifest
            if (r.StartsWith("manifest=")) return (35, 0);

            // Layer 3: Metadata / modid
            if (r.StartsWith("metadata=banned_modname")) return (30, 0);
            if (r.StartsWith("metadata=suspicious_kw")) return (18, 1);
            if (r.StartsWith("modid=")) return (20, 1);

            // Layer 4: Known cheat package roots
            if (r.StartsWith("known_cheat_pkg=x"))
            {
                var val = r.Replace("known_cheat_pkg=x", "");
                if (int.TryParse(val, out int v) && v >= 2) return (25, 0);
                return (12, 1);
            }
            if (r.StartsWith("known_cheat_pkg=1")) return (12, 1);

            // Layer 5: Class pool keywords
            if (r.StartsWith("class_pool="))
            {
                var rest = r.Replace("class_pool=", "").Replace("kw", "");
                if (int.TryParse(rest, out int v))
                    return v >= 15 ? (25, 1) : v >= 8 ? (12, 2) : (5, 3);
                return (5, 3);
            }

            // Layer 6: Class name keywords (2 per class)
            if (r.StartsWith("class_name_kw="))
            {
                var rest = r.Replace("class_name_kw=", "");
                if (int.TryParse(rest, out int v))
                    return (v * 2, 3);
                return (2, 3);
            }

            // Layer 7: Obfuscation
            if (r.StartsWith("obfuscated=")) return (15, 2);
            if (r.StartsWith("obfuscator=")) return (12, 2);

            // Layer 8: Suspicious package paths
            if (r.StartsWith("cheat_pkg_x")) return (15, 2);

            // Layer 9: Entry keywords
            if (r.StartsWith("entry_kw=x"))
            {
                var rest = r.Replace("entry_kw=x", "");
                if (int.TryParse(rest, out int v))
                    return v >= 3 ? (8, 3) : (4, 3);
                return (4, 3);
            }

            // Layer 10: Mixin
            if (r.StartsWith("mixin=")) return (8, 2);

            // Layer 11: Text content
            if (r.StartsWith("text_content=")) return (15, 2);

            // Layer 12: Nested archives
            if (r.StartsWith("nested_jar=")) return (10, 2);
            if (r.StartsWith("nested_kw=")) return (15, 2);

            // Layer 13: Resources
            if (r.StartsWith("resource_img=")) return (4, 3);
            if (r.StartsWith("resource=lang")) return (6, 3);

            // Layer 14: Size
            if (r.StartsWith("size=banned")) return (30, 1);
            if (r.StartsWith("size>50MB")) return (5, 3);

            // Layer 15: Duplicate sizes
            if (r.StartsWith("dup_sizes=")) return (10, 2);
            if (r.StartsWith("small_classes=")) return (6, 3);
            if (r.StartsWith("high_entropy=")) return (12, 2);

            // Layer 16: MC version weight
            if (r.StartsWith("mc_weight=")) return (12, 3);

            // Layer 17: Known hash match
            if (r.StartsWith("hash_match=")) return (60, 0);

            // Layer 18: Legitimate mod discount (negative)
            if (r.StartsWith("legit_mod_discount="))
            {
                var val = r.Replace("legit_mod_discount=-", "").Replace("legit_mod_discount=", "");
                if (int.TryParse(val, out int v))
                    return (-v, 9);
                return (0, 9);
            }

            // Unrecognized
            return (0, 9);
        }

        private int EstimateReasonPoints(string r)
        {
            // For unrecognized reasons — estimate from text hints
            if (r.Contains("filename") || r.Contains("manifest")) return 20;
            if (r.Contains("class_pool") || r.Contains("obfus")) return 10;
            if (r.Contains("cheat_pkg") || r.Contains("modid")) return 10;
            if (r.Contains("size") || r.Contains("text_content")) return 8;
            return 4;
        }

        private (string Label, string Detail) FormatReasonWithLabel(string r)
        {
            if (r.StartsWith("filename=MATCH:"))
                return (L("Filename MATCH", "Совпадение имени"), r.Replace("filename=MATCH:", ""));
            if (r.StartsWith("filename=KW:"))
                return (L("Filename Keyword", "Ключ. слово имени"), r.Replace("filename=KW:", ""));
            if (r.StartsWith("manifest="))
                return (L("Manifest", "Манифест"), r.Replace("manifest=", ""));
            if (r.StartsWith("metadata=banned_modname"))
                return (L("Metadata", "Метаданные"), L("banned mod name", "запрещённый мод"));
            if (r.StartsWith("metadata=suspicious_kw"))
                return (L("Metadata", "Метаданные"), L("suspicious keywords", "подозрительные ключ.слова"));
            if (r.StartsWith("modid="))
                return (L("Mod ID", "ID мода"), r.Replace("modid=", ""));
            if (r.StartsWith("known_cheat_pkg"))
                return (L("Known Cheat Pkg", "Изв. пакет чита"), r.Replace("known_cheat_pkg=x", "").Replace("known_cheat_pkg=1", L("1 hit", "1 совпадение")));
            if (r.StartsWith("class_pool="))
                return (L("Class Pool", "Пул классов"), r.Replace("class_pool=", ""));
            if (r.StartsWith("class_name_kw="))
                return (L("Class Name", "Имя класса"), r.Replace("class_name_kw=", "") + L(" keyword hits", " совпадений ключ.слов"));
            if (r.StartsWith("obfuscated="))
                return (L("Obfuscated", "Обфусцирован"), r.Replace("obfuscated=", ""));
            if (r.StartsWith("obfuscator="))
                return (L("Obfuscator", "Обфускатор"), r.Replace("obfuscator=", ""));
            if (r.StartsWith("cheat_pkg_x"))
                return (L("Cheat Pkg Path", "Путь пакета чита"), r.Replace("cheat_pkg_x", "") + L(" hits", " совпад."));
            if (r.StartsWith("entry_kw=x"))
                return (L("Entry Keywords", "Ключ.слова Entry"), r.Replace("entry_kw=x", "") + L(" hits", " совпад."));
            if (r.StartsWith("mixin="))
                return (L("Mixin", "Mixin"), r.Replace("mixin=", ""));
            if (r.StartsWith("text_content="))
                return (L("Text Content", "Текст.содерж."), r.Replace("text_content=", ""));
            if (r.StartsWith("nested_jar="))
                return (L("Nested JAR", "Влож. JAR"), r.Replace("nested_jar=", ""));
            if (r.StartsWith("nested_kw="))
                return (L("Nested Keyword", "Ключ.слова влож."), r.Replace("nested_kw=", ""));
            if (r.StartsWith("resource_img="))
                return (L("Resource Image", "Ресурс-изобр."), r.Replace("resource_img=", ""));
            if (r.StartsWith("resource=lang"))
                return (L("Resource Lang", "Ресурс-язык"), r.Replace("resource=lang", ""));
            if (r.StartsWith("size=banned"))
                return (L("Size Match", "Совпад.размера"), L("banned size", "запрещ.размер"));
            if (r.StartsWith("size>50MB"))
                return (L("Large File", "Большой файл"), "> 50 MB");
            if (r.StartsWith("dup_sizes="))
                return (L("Dup Sizes", "Дубл.размеры"), r.Replace("dup_sizes=", ""));
            if (r.StartsWith("small_classes="))
                return (L("Small Classes", "Мал.классы"), r.Replace("small_classes=", ""));
            if (r.StartsWith("high_entropy="))
                return (L("High Entropy", "Выс.энтропия"), r.Replace("high_entropy=", ""));
            if (r.StartsWith("mc_weight="))
                return (L("MC Version", "Версия MC"), r.Replace("mc_weight=", ""));
            if (r.StartsWith("hash_match="))
                return (L("Hash Match", "Совпад.хеша"), r.Replace("hash_match=", ""));
            if (r.StartsWith("legit_mod_discount="))
                return (L("Legit Discount", "Скидка легит."), r.Replace("legit_mod_discount=", ""));
            return (r, "");
        }

        private void BuildClassTab(MainWindow.ModResultItem item)
        {
            if (item.Classes.Count == 0)
            {
                ClassListTitle.Text = L("No classes found", "Классы не найдены");
                return;
            }

            ClassListBox.ItemsSource = item.Classes;
            ClassDetailCount.Text = item.Classes.Count + " " + L("classes", "классов");
        }

        private void BuildStructureTab(MainWindow.ModResultItem item)
        {
            if (!string.IsNullOrEmpty(item.ZipStructure))
            {
                var lines = item.ZipStructure.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                int fileCount = lines.Count(l => l.StartsWith("📄") || l.StartsWith("📦"));
                int dirCount = lines.Count(l => l.StartsWith("📁"));
                StructStats.Text = L("Files:", "Файлов:") + " " + fileCount + "  " +
                                   L("Dirs:", "Папок:") + " " + dirCount + "  " +
                                   L("Total entries:", "Всего записей:") + " " + (fileCount + dirCount);
                StructureBox.Text = item.ZipStructure;
            }
            else
            {
                StructureBox.Text = L("Not a ZIP archive", "Не ZIP архив");
            }
        }

        private void BuildMetadataTab(MainWindow.ModResultItem item)
        {
            if (!string.IsNullOrEmpty(item.MetadataText))
            {
                MetaTitle.Text = L("Metadata files found in archive", "Метаданные в архиве");
                MetadataBox.Text = item.MetadataText;
            }
            else
            {
                MetaTitle.Text = L("No metadata files", "Метаданные не найдены");
                MetadataBox.Text = "";
            }
        }

        private void ClassListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassListBox.SelectedItem is MainWindow.ModClassInfo ci)
            {
                ClassDetailName.Text = ci.Name;
                ClassDetailVer.Text = L("Java", "Java") + " " + MapClassVersion(ci.MajorVersion);
                ClassStringsList.ItemsSource = ci.ConstantPool;
            }
        }

        private static string MapClassVersion(int majorVer)
        {
            return majorVer switch
            {
                45 => "1.1",
                46 => "1.2",
                47 => "1.3",
                48 => "1.4",
                49 => "5",
                50 => "6",
                51 => "7",
                52 => "8",
                53 => "9",
                54 => "10",
                55 => "11",
                56 => "12",
                57 => "13",
                58 => "14",
                59 => "15",
                60 => "16",
                61 => "17",
                62 => "18",
                63 => "19",
                64 => "20",
                65 => "21",
                _ => majorVer.ToString()
            };
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_item == null) return;
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(_item.FileName) + "_report.txt",
                Filter = "Text file (*.txt)|*.txt|JSON report (*.json)|*.json"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    if (dlg.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        var obj = new
                        {
                            File = _item.FileName,
                            Path = _item.FilePath,
                            Score = _item.Score,
                            Verdict = _item.StatusText,
                            FullReasons = _item.FullReasons,
                            TotalClasses = _item.TotalClasses,
                            ObfuscatedClasses = _item.ObfuscatedClasses,
                            McVersion = _item.McVersion,
                            HashInfo = _item.HashInfo
                        };
                        string json = System.Text.Json.JsonSerializer.Serialize(obj,
                            new System.Text.Json.JsonSerializerOptions
                            {
                                WriteIndented = true,
                                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                            });
                        File.WriteAllText(dlg.FileName, json);
                    }
                    else
                    {
                        using var writer = new StreamWriter(dlg.FileName);
                        string sep = new string('=', 60);
                        writer.WriteLine(sep);
                        writer.WriteLine(L("FMTool Mod Analysis Report", "FMTool Анализ мода"));
                        writer.WriteLine(sep);
                        writer.WriteLine();
                        writer.WriteLine(L("File:", "Файл:") + " " + _item.FileName);
                        writer.WriteLine(L("Score:", "Баллы:") + " " + _item.Score);
                        writer.WriteLine(L("Verdict:", "Вердикт:") + " " + _item.StatusText);
                        writer.WriteLine(L("Color:", "Цвет:") + " " + (_item.Color?.ToString() ?? ""));
                        writer.WriteLine();
                        if (!string.IsNullOrEmpty(_item.McVersion))
                            writer.WriteLine(L("MC Version:", "Версия MC:") + " " + _item.McVersion);
                        if (_item.TotalClasses > 0)
                            writer.WriteLine(L("Classes:", "Классы:") + " " + _item.TotalClasses + " (" + _item.ObfuscatedClasses + L(" obfuscated", " обфусцировано") + ")");
                        if (!string.IsNullOrEmpty(_item.HashInfo))
                            writer.WriteLine(L("SHA-256:", "SHA-256:") + " " + _item.HashInfo);
                        if (!string.IsNullOrEmpty(_item.FullReasons))
                        {
                            writer.WriteLine();
                            writer.WriteLine(L("--- Detection Reasons ---", "--- Причины детекта ---"));
                            foreach (var r in _item.FullReasons.Split('|', StringSplitOptions.RemoveEmptyEntries))
                            {
                                var (text, _) = FormatReason(r.Trim());
                                writer.WriteLine("  " + text);
                            }
                        }
                        writer.WriteLine();
                        writer.WriteLine(sep);
                    }
                    MessageBox.Show(L("Report saved", "Отчёт сохранён") + $"\n{dlg.FileName}",
                        L("Success", "Успешно"), MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(L("Save error:", "Ошибка сохранения:") + "\n" + ex.Message,
                        L("Error", "Ошибка"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LuytenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_item == null || string.IsNullOrEmpty(_item.FilePath)) return;
            if (!File.Exists(_item.FilePath))
            {
                MessageBox.Show(L("File not found:", "Файл не найден:") + " " + _item.FilePath,
                    L("Error", "Ошибка"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                string luytenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "APPS", "luyten-0.5.4.exe");
                if (!File.Exists(luytenPath))
                {
                    luytenPath = @"C:\FMTool\APPS\luyten-0.5.4.exe";
                }
                if (File.Exists(luytenPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = luytenPath,
                        Arguments = $"\"{_item.FilePath}\"",
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show(L("Luyten not found at:", "Luyten не найден:") + "\n" + luytenPath,
                        L("Error", "Ошибка"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(L("Error launching Luyten:", "Ошибка запуска Luyten:") + "\n" + ex.Message,
                    L("Error", "Ошибка"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExtractBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_item == null || string.IsNullOrEmpty(_item.FilePath)) return;
            if (!File.Exists(_item.FilePath))
            {
                MessageBox.Show(L("File not found:", "Файл не найден:") + " " + _item.FilePath,
                    L("Error", "Ошибка"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var owner = Owner as MainWindow;
            if (owner == null) return;
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.Description = L("Select extraction folder", "Выберите папку для извлечения");
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IsEnabled = false;
                try
                {
                    string destDir = Path.Combine(dlg.SelectedPath,
                        Path.GetFileNameWithoutExtension(_item.FileName) + "_extracted");
                    Directory.CreateDirectory(destDir);

                    using var archive = ZipFile.OpenRead(_item.FilePath);
                    int total = archive.Entries.Count;
                    int done = 0;
                    foreach (var entry in archive.Entries)
                    {
                        if (string.IsNullOrEmpty(entry.Name)) continue;
                        string entryDest = Path.Combine(destDir, entry.FullName);
                        Directory.CreateDirectory(Path.GetDirectoryName(entryDest)!);
                        if (!entry.FullName.EndsWith("/") && !entry.FullName.EndsWith("\\"))
                            entry.ExtractToFile(entryDest, true);
                        done++;
                        if (done % Math.Max(1, total / 20) == 0 || done == total)
                            System.Windows.Forms.Application.DoEvents();
                    }

                    MessageBox.Show(L("Extracted", "Извлечено") + $" {done} " +
                        L("files to:", "файлов в:") + "\n" + destDir,
                        L("Success", "Успешно"), MessageBoxButton.OK, MessageBoxImage.Information);
                    try { Process.Start(new ProcessStartInfo { FileName = destDir, UseShellExecute = true }); }
                    catch { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(L("Extract error:", "Ошибка извлечения:") + "\n" + ex.Message,
                        L("Error", "Ошибка"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally { IsEnabled = true; }
            }
        }

        // ── AI Weighted Scoring System ──

        private static readonly (string Prefix, string Category, double Weight, Color Color)[] ReasonCategories = {
            ("filename=MATCH:", "Filename Match",        2.5, Color.FromRgb(255,50,50)),
            ("filename=KW:",    "Filename Keyword",      1.5, Color.FromRgb(255,80,40)),
            ("hash_match=",     "Known Hash Match",      3.0, Color.FromRgb(255,30,30)),
            ("manifest=",       "Manifest Analysis",     1.8, Color.FromRgb(255,120,0)),
            ("modid=",          "Mod ID Check",          1.5, Color.FromRgb(255,140,0)),
            ("metadata=",       "Metadata Check",        1.4, Color.FromRgb(255,160,0)),
            ("known_cheat_pkg", "Known Cheat Pkg",       2.0, Color.FromRgb(255,50,50)),
            ("class_pool=",     "Class Pool Keywords",   1.0, Color.FromRgb(220,180,40)),
            ("class_name_kw=",  "Class Name Analysis",   0.8, Color.FromRgb(200,170,50)),
            ("obfuscated=",     "Obfuscation Level",     1.0, Color.FromRgb(255,120,0)),
            ("obfuscator=",     "Obfuscator Detect",     1.2, Color.FromRgb(255,80,0)),
            ("cheat_pkg_x",     "Suspicious Pkg Paths",  1.2, Color.FromRgb(255,100,60)),
            ("entry_kw=x",      "Entry Keywords",        0.6, Color.FromRgb(220,180,40)),
            ("mixin=",          "Mixin Config",          1.0, Color.FromRgb(255,120,0)),
            ("text_content=",   "Text Content",          0.8, Color.FromRgb(160,210,60)),
            ("nested_",         "Nested Archive",        0.9, Color.FromRgb(255,140,0)),
            ("resource_",       "Resource Fingerprint",  0.4, Color.FromRgb(244,164,96)),
            ("size=banned",     "Size Fingerprint",      1.0, Color.FromRgb(100,180,255)),
            ("size>50MB",       "Large File Alert",      0.3, Color.FromRgb(128,128,128)),
            ("dup_sizes=",      "Duplicate Sizes",       0.8, Color.FromRgb(220,180,40)),
            ("small_classes=",  "Small Classes",         0.5, Color.FromRgb(200,170,50)),
            ("high_entropy=",   "High Entropy (Packed)", 0.9, Color.FromRgb(255,140,30)),
            ("mc_weight=",      "MC Version Match",      0.5, Color.FromRgb(100,220,100)),
            ("susp_struct",     "Suspicious Structure",  1.0, Color.FromRgb(255,100,70)),
            ("susp_pkg",        "Flat Package Structure",0.9, Color.FromRgb(255,120,70)),
            ("meta_disguise",   "Metadata Disguise",     1.2, Color.FromRgb(255,60,40)),
            ("class_ct=",       "Class Count Tiers",     0.8, Color.FromRgb(180,100,200)),
            ("module_str=",     "Cheat Module Strings",  1.5, Color.FromRgb(255,40,40)),
            ("susp_tld=",       "Suspicious TLD Pkg",    1.1, Color.FromRgb(255,80,100)),
            ("mixin_ct=",       "Mixin Class Count",     0.9, Color.FromRgb(255,140,70)),
            ("small_cls=",      "Small Class Files",     0.6, Color.FromRgb(200,150,100)),
            ("event_str=",      "Event System Strings",  1.0, Color.FromRgb(100,200,255)),
            ("event_cls=",      "Event Subscriber Cls",  0.9, Color.FromRgb(100,180,230)),
            ("mc_refs=",        "Minecraft References",  1.1, Color.FromRgb(100,180,255)),
            ("infra_str=",      "Module Infrastructure", 1.2, Color.FromRgb(255,60,80)),
            ("cls_dist=",       "Class Size Distr.",     0.8, Color.FromRgb(200,150,100)),
            ("profiler=",       "Intelligent Profiler",  1.8, Color.FromRgb(255,60,60)),
            ("cheat_combo=",    "Combined Indicators",   1.2, Color.FromRgb(255,30,30)),
            ("safety_net=",     "Safety Net (Fallback)", 0.6, Color.FromRgb(255,80,80)),
            ("obf=",            "Obfuscation Feature",   0.8, Color.FromRgb(255,130,30)),
            ("1letter=",        "Short Class Names",     0.6, Color.FromRgb(200,180,40)),
            ("pkg_flat=",       "Package Flatness",      0.7, Color.FromRgb(200,140,60)),
            ("domain_hits=",    "MC Domain Coverage",    0.8, Color.FromRgb(100,200,255)),
            ("correlation=",    "Correlation Bonus",     1.0, Color.FromRgb(255,80,80)),
            ("legit_mod_discount=", "Legit Mod Discount",-0.6, Color.FromRgb(68,170,68)),
        };

        private List<FrameworkElement> BuildWeightedBreakdown(List<string> reasons)
        {
            var elements = new List<FrameworkElement>();

            // Categorize each reason
            var catData = new Dictionary<string, (string Prefix, string Category, double Weight, Color Color,
                List<string> Details, int RawPoints, bool IsDiscount)>();

            foreach (var r in reasons)
            {
                var match = ReasonCategories.FirstOrDefault(c => r.StartsWith(c.Prefix));
                if (match.Prefix == null) continue;

                var (points, _) = ParseReasonPoints(r);
                if (points == 0) continue;

                if (!catData.ContainsKey(match.Category))
                {
                    catData[match.Category] = (match.Prefix, match.Category, match.Weight, match.Color,
                        new List<string>(), 0, match.Weight < 0);
                }

                var entry = catData[match.Category];
                entry.RawPoints += points;
                entry.Details.Add(r);
                catData[match.Category] = entry;
            }

            if (catData.Count == 0)
            {
                elements.Add(MakeReasonRow(L("No categorized data", "Нет категоризированных данных"), Brushes.Gray));
                return elements;
            }

            // Compute weighted scores
            double totalRaw = 0;
            double totalWeighted = 0;
            var catList = catData.Values
                .OrderByDescending(c => c.Weight)
                .ThenByDescending(c => c.RawPoints)
                .ToList();

            foreach (var cat in catList)
            {
                double weighted = cat.RawPoints * cat.Weight;
                totalRaw += cat.RawPoints;
                totalWeighted += weighted;

                // Build category card
                var card = new Border
                {
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(12, 8, 12, 8),
                    Margin = new Thickness(0, 3, 0, 3),
                    BorderBrush = new SolidColorBrush(cat.Color),
                    BorderThickness = new Thickness(1),
                    Background = new SolidColorBrush(Color.FromArgb((byte)(cat.IsDiscount ? 8 : 10), cat.Color.R, cat.Color.G, cat.Color.B))
                };

                var innerStack = new StackPanel();

                // Header row: icon + name + calculation
                var headerGrid = new Grid();
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                string icon = cat.IsDiscount ? "✅" :
                              cat.Weight >= 2.0 ? "🔴" :
                              cat.Weight >= 1.0 ? "🟠" :
                              cat.Weight >= 0.7 ? "🟡" : "🟢";

                var nameTb = new TextBlock
                {
                    Text = $"{icon} {cat.Category}",
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(cat.Color),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(nameTb, 0);
                headerGrid.Children.Add(nameTb);

                string calcText = cat.IsDiscount
                    ? $"-{cat.RawPoints} × {Math.Abs(cat.Weight):F1} = {weighted:F0}"
                    : $"{cat.RawPoints} × {cat.Weight:F1} = {weighted:F0}";
                var calcTb = new TextBlock
                {
                    Text = calcText,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = cat.IsDiscount ? Brushes.LimeGreen : Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(calcTb, 2);
                headerGrid.Children.Add(calcTb);

                innerStack.Children.Add(headerGrid);

                // Detail line
                if (cat.Details.Count > 0)
                {
                    var detailTb = new TextBlock
                    {
                        Text = string.Join(", ",
                            cat.Details.Select(d => FormatReasonShort(d)).Take(3)),
                        FontSize = 10,
                        Foreground = new SolidColorBrush(Color.FromArgb((byte)180, cat.Color.R, cat.Color.G, cat.Color.B)),
                        Margin = new Thickness(16, 2, 0, 0),
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        MaxWidth = 480
                    };
                    innerStack.Children.Add(detailTb);
                }

                card.Child = innerStack;
                elements.Add(card);
            }

            // ── Total weighted score card ──
            var totalCard = new Border
            {
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(14, 10, 14, 10),
                Margin = new Thickness(0, 8, 0, 4),
                Background = new SolidColorBrush(Color.FromArgb((byte)20, (byte)255, (byte)255, (byte)255)),
                BorderBrush = new SolidColorBrush(Color.FromArgb((byte)60, (byte)200, (byte)200, (byte)200)),
                BorderThickness = new Thickness(1)
            };
            var totalStack = new StackPanel();

            var totalHeader = new Grid();
            totalHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            totalHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            totalHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var totalLbl = new TextBlock
            {
                Text = L("📊 WEIGHTED TOTAL", "📊 ИТОГО (взвешенно)"),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            };
            Grid.SetColumn(totalLbl, 0);
            totalHeader.Children.Add(totalLbl);

            var totalVal = new TextBlock
            {
                Text = totalWeighted.ToString("F0"),
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = totalWeighted >= 40 ? Brushes.Red : totalWeighted >= 15 ? Brushes.Orange : Brushes.LimeGreen,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            Grid.SetColumn(totalVal, 2);
            totalHeader.Children.Add(totalVal);

            totalStack.Children.Add(totalHeader);

            // Raw total line
            var rawLine = new TextBlock
            {
                Text = L($"Raw score: {totalRaw:F0}  |  Verdict: {GetVerdictText(totalWeighted)}",
                         $"Сырые баллы: {totalRaw:F0}  |  Вердикт: {GetVerdictText(totalWeighted)}"),
                FontSize = 10,
                Foreground = Brushes.Gray,
                Margin = new Thickness(0, 2, 0, 0)
            };
            totalStack.Children.Add(rawLine);
            totalCard.Child = totalStack;
            elements.Add(totalCard);

            // ── Interpretation ──
            elements.Add(MakeInterpretation(totalWeighted, catList));

            return elements;
        }

        private static string FormatReasonShort(string r)
        {
            foreach (var c in ReasonCategories)
            {
                if (r.StartsWith(c.Prefix))
                    return r.Substring(c.Prefix.Length);
            }
            return r;
        }

        private static string GetVerdictText(double weightedScore)
        {
            if (weightedScore >= 40) return "Suspicious (High)";
            if (weightedScore >= 15) return "Suspicious";
            return "Normal";
        }

        private FrameworkElement MakeInterpretation(double totalWeighted,
            List<(string Prefix, string Category, double Weight, Color Color, List<string> Details, int RawPoints, bool IsDiscount)> catList)
        {
            var border = new Border
            {
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12, 8, 12, 8),
                Margin = new Thickness(0, 6, 0, 0),
                Background = new SolidColorBrush(Color.FromArgb((byte)10, (byte)100, (byte)200, (byte)255)),
                BorderBrush = new SolidColorBrush(Color.FromArgb((byte)30, (byte)100, (byte)200, (byte)255)),
                BorderThickness = new Thickness(1)
            };

            var stack = new StackPanel();
            var titleTb = new TextBlock
            {
                Text = L("🤖 Interpretation", "🤖 Интерпретация"),
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(100, 200, 255)),
                Margin = new Thickness(0, 0, 0, 4)
            };
            stack.Children.Add(titleTb);

            // Build interpretation text
            var parts = new List<string>();

            bool hasFilename = catList.Any(c => c.Category == "Filename Match" || c.Category == "Filename Keyword");
            bool hasHash = catList.Any(c => c.Category == "Known Hash Match");
            bool hasCheatPkg = catList.Any(c => c.Category == "Known Cheat Pkg");
            bool hasManifest = catList.Any(c => c.Category == "Manifest Analysis" || c.Category == "Mod ID Check");
            bool hasObfus = catList.Any(c => c.Category == "Obfuscation Level" || c.Category == "Obfuscator Detect");
            bool hasPkg = catList.Any(c => c.Category == "Suspicious Pkg Paths");
            bool hasClassPool = catList.Any(c => c.Category == "Class Pool Keywords");
            bool hasDiscount = catList.Any(c => c.IsDiscount);

            if (totalWeighted >= 80)
                parts.Add(L("🔴 CRITICAL: Multiple strong suspicion indicators detected.",
                           "🔴 КРИТИЧЕСКИЙ: Обнаружено несколько сильных индикаторов подозрения."));
            else if (totalWeighted >= 40)
                parts.Add(L("🔴 High suspicion — strong signature match.",
                           "🔴 Высокое подозрение — сильное совпадение сигнатуры."));
            else if (totalWeighted >= 25)
                parts.Add(L("🟠 Notable suspicion — multiple indicators overlap.",
                           "🟠 Заметное подозрение — несколько индикаторов перекрываются."));
            else if (totalWeighted >= 15)
                parts.Add(L("🟡 Some suspicion — some indicators present, not conclusive.",
                           "🟡 Некоторое подозрение — некоторые индикаторы есть, но не окончательно."));
            else
                parts.Add(L("🟢 Normal — no significant indicators detected.",
                           "🟢 Нормально — значимых индикаторов не обнаружено."));

            if (hasFilename)
                parts.Add(L("⚠ Filename matches known cheat pattern — strong suspicion.",
                           "⚠ Имя файла совпадает с известным читом — сильное подозрение."));
            if (hasHash)
                parts.Add(L("⛔ SHA-256 hash matches known cheat database — strong suspicion.",
                           "⛔ SHA-256 хеш совпадает с базой читов — сильное подозрение."));
            if (hasCheatPkg)
                parts.Add(L("⛔ Known cheat package classes found inside archive.",
                           "⛔ Внутри архива найдены классы известного чита."));
            if (hasManifest && !hasDiscount)
                parts.Add(L("📋 Suspicious manifest/mod metadata detected.",
                           "📋 Обнаружены подозрительные манифест/metadata."));
            if (hasObfus)
                parts.Add(L("🔐 Heavy obfuscation detected — can indicate cheat mods.",
                           "🔐 Обнаружена сильная обфускация — может указывать на чит."));
            if (hasPkg)
                parts.Add(L("📁 Suspicious package paths found in class structure.",
                           "📁 Подозрительные пути пакетов в структуре классов."));
            if (hasClassPool)
                parts.Add(L("🔑 Cheat-related keywords found in class constant pool.",
                           "🔑 Ключевые слова читов в constant pool классов."));
            if (hasDiscount)
                parts.Add(L("✅ Legitimate mod metadata — score discounted.",
                           "✅ Обнаружены метаданные легитимного мода — баллы снижены."));

            if (parts.Count == 0)
                parts.Add(L("ℹ No specific analysis available.",
                           "ℹ Нет доступного анализа."));

            var textTb = new TextBlock
            {
                Text = string.Join("\n", parts),
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb((byte)200, (byte)200, (byte)220)),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 18
            };
            stack.Children.Add(textTb);
            border.Child = stack;
            return border;
        }

        private static Border MakeStatBox(string label, string value, string colorHex)
        {
            var border = new Border
            {
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12, 8, 12, 8),
                Margin = new Thickness(4, 4, 4, 4),
                Background = new SolidColorBrush(Color.FromArgb((byte)10, (byte)255, (byte)255, (byte)255))
            };
            var color = (Color)ColorConverter.ConvertFromString(colorHex);
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock
            {
                Text = label,
                FontSize = 11,
                Foreground = Brushes.Gray
            });
            stack.Children.Add(new TextBlock
            {
                Text = value,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(color),
                Margin = new Thickness(0, 2, 0, 0)
            });
            border.Child = stack;
            return border;
        }

        private static Border MakeInfoChip(string label, string value)
        {
            var border = new Border
            {
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10, 4, 10, 4),
                Margin = new Thickness(0, 2, 8, 2),
                Background = new SolidColorBrush(Color.FromArgb((byte)8, (byte)255, (byte)255, (byte)255))
            };
            border.Child = new TextBlock
            {
                Text = value,
                FontSize = 12,
                Foreground = Brushes.LightGray
            };
            return border;
        }

        private static FrameworkElement MakeSectionHeader(string title, string subtitle)
        {
            var grid = new Grid { Margin = new Thickness(0, 12, 0, 4) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(new TextBlock
            {
                Text = title,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                VerticalAlignment = VerticalAlignment.Center
            });

            if (!string.IsNullOrEmpty(subtitle))
            {
                var sub = new TextBlock
                {
                    Text = subtitle,
                    FontSize = 11,
                    Foreground = Brushes.Gray,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                Grid.SetColumn(sub, 1);
                grid.Children.Add(sub);
            }
            return grid;
        }

        private static Border MakeSeparator()
        {
            return new Border
            {
                Height = 1,
                Background = new SolidColorBrush(Color.FromArgb((byte)20, (byte)200, (byte)200, (byte)200)),
                Margin = new Thickness(0, 0, 0, 6)
            };
        }

        private static Border MakeReasonRow(string text, Brush color)
        {
            var border = new Border
            {
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(0, 2, 0, 2),
                Background = new SolidColorBrush(Color.FromArgb((byte)12, (byte)255, (byte)255, (byte)255))
            };
            border.Child = new TextBlock
            {
                Text = text,
                FontSize = 12,
                Foreground = color,
                TextWrapping = TextWrapping.Wrap
            };
            return border;
        }

        private static string GetReasonSummary(string r)
        {
            if (r.StartsWith("filename=MATCH:")) return "file=" + r.Replace("filename=MATCH:", "");
            if (r.StartsWith("hash_match=")) return "hash=cheat";
            if (r.StartsWith("known_cheat_pkg")) return "cheat_package";
            if (r.StartsWith("cheat_pkg_x")) return "suspicious_packages";
            if (r.StartsWith("obfuscated="))
            {
                var parts = r.Replace("obfuscated=", "").Split('/');
                if (parts.Length == 2 && int.TryParse(parts[1], out int t) && t > 0)
                    return $"obfuscation={int.Parse(parts[0]) * 100 / t}%";
                return "obfuscation";
            }
            if (r.StartsWith("profiler="))
            {
                var val = r.Replace("profiler=", "");
                var idx = val.IndexOf(':');
                return idx > 0 ? $"profile={val.Substring(0, idx)}pts" : $"profile={val}pts";
            }
            if (r.StartsWith("class_pool=")) return "cheat_keywords";
            if (r.StartsWith("manifest=")) return "bad_manifest";
            if (r.StartsWith("modid=")) return "bad_modid";
            if (r.StartsWith("size=banned")) return "cheat_size";
            if (r.StartsWith("metadata=banned_modname")) return "bad_metadata";
            if (r.StartsWith("mc_weight=")) return "mc_version";
            if (r.StartsWith("legit_mod_discount")) return "";
            if (r.StartsWith("hash_match")) return "hash_match";
            if (r.StartsWith("bareCheat")) return "bare_cheat_framework";
            if (r.StartsWith("stripObf")) return "stripped_obfuscation";
            return r.Length > 30 ? r.Substring(0, 30) + "..." : r;
        }

        private (string, Brush) FormatReason(string r)
        {
            if (r.StartsWith("filename=MATCH:"))
                return ("⚠ " + L("File name matches banned mod: ", "Имя файла совпадает с запрещённым модом: ") + r.Replace("filename=MATCH:", ""), Brushes.Red);
            if (r.StartsWith("filename=KW:"))
                return ("! " + L("Suspicious keyword in filename: ", "Подозрительное слово в имени: ") + r.Replace("filename=KW:", ""), Brushes.Orange);
            if (r.StartsWith("manifest="))
                return ("⛔ " + L("MANIFEST.MF references banned mod: ", "MANIFEST.MF ссылается на запрещённый мод: ") + r.Replace("manifest=", ""), Brushes.Red);
            if (r.StartsWith("known_cheat_pkg"))
            {
                string val = r.Replace("known_cheat_pkg=x", "").Replace("known_cheat_pkg=1", "1");
                return ("⛔ " + L("Known cheat package structure found (", "Найдена структура пакета чита (") + val + L(" hits)", " совпад.)"), Brushes.Red);
            }
            if (r.StartsWith("class_pool="))
            {
                string cnt = r.Replace("class_pool=", "");
                int n = int.TryParse(cnt.Replace("kw", ""), out int v) ? v : 0;
                return (L("Cheat keywords in class pool: ", "Ключевые слова читов в пуле классов: ") + cnt + L(" — indicates cheat functionality", " — указывает на функционал чита"), n >= 10 ? Brushes.Red : Brushes.Coral);
            }
            if (r.StartsWith("class_name_kw="))
                return (L("Class names contain cheat keywords: ", "Имена классов содержат ключевые слова читов: ") + r.Replace("class_name_kw=", ""), Brushes.Goldenrod);
            if (r.StartsWith("obfuscated="))
            {
                string val = r.Replace("obfuscated=", "");
                var parts = val.Split('/');
                double pct = parts.Length == 2 && int.TryParse(parts[1], out int tot) && tot > 0
                    ? int.Parse(parts[0]) * 100.0 / tot : 0;
                return (L("Classes obfuscated: ", "Обфусцировано классов: ") + val + $" ({pct:F0}%)" + L(" — logic hidden intentionally", " — логика намеренно скрыта"), pct >= 50 ? Brushes.Red : Brushes.Gold);
            }
            if (r.StartsWith("obfuscator="))
                return (L("Obfuscator detected: ", "Обнаружен обфускатор: ") + r.Replace("obfuscator=", "") + L(" — used to hide cheat code", " — используется для сокрытия кода чита"), Brushes.OrangeRed);
            if (r.StartsWith("cheat_pkg_x"))
            {
                string val = r.Replace("cheat_pkg_x", "").Replace("(strong:", " (strong:");
                return (L("Suspicious package paths: ", "Подозрительные пути пакетов: ") + val, Brushes.Coral);
            }
            if (r.StartsWith("modid="))
                return (L("Mod ID matches banned mod: ", "ID мода совпадает с запрещённым: ") + r.Replace("modid=", ""), Brushes.Red);
            if (r.StartsWith("mixin="))
                return (L("Mixin config targets cheats: ", "Mixin конфиг нацелен на читы: ") + r.Replace("mixin=", ""), Brushes.DarkOrange);
            if (r.StartsWith("entry_kw="))
                return (L("Archive entries contain keywords: ", "Записи архива содержат ключевые слова: ") + r.Replace("entry_kw=", "") + L(" matches", " совпад."), Brushes.Goldenrod);
            if (r.StartsWith("text_content="))
                return (L("Config/JSON contains cheat strings: ", "Конфиг/JSON содержит строки читов: ") + r.Replace("text_content=", ""), Brushes.YellowGreen);
            if (r.StartsWith("nested_jar=") || r.StartsWith("nested_kw="))
                return (L("Nested archive with cheat content: ", "Вложенный архив с содержимым чита: ") + r.Replace("nested_", ""), Brushes.Orange);
            if (r.StartsWith("resource_img="))
                return (L("Cheat resource image found: ", "Найдено изображение чита: ") + r.Replace("resource_img=", ""), Brushes.SandyBrown);
            if (r.StartsWith("resource=lang_x"))
                return (L("Unusual resource structure: ", "Необычная структура ресурсов: ") + r.Replace("resource=lang_x", ""), Brushes.SandyBrown);
            if (r.StartsWith("size=banned"))
                return (L("File size matches known cheat pattern", "Размер файла совпадает с известным читом"), Brushes.LightBlue);
            if (r.StartsWith("size>50MB"))
                return (L("File exceeds 50 MB — unusual for a mod", "Файл больше 50 МБ — необычно для мода"), Brushes.LightBlue);
            if (r.StartsWith("mc_weight="))
                return (L("Minecraft version detected: ", "Версия Minecraft: ") + r.Replace("mc_weight=", "") + L(" — may be modded", " — возможно модифицирована"), Brushes.LightGreen);
            if (r.StartsWith("dup_sizes="))
                return (L("Duplicate class sizes — obfuscation artifact: ", "Дублирующиеся размеры классов — артефакт обфускации: ") + r.Replace("dup_sizes=", ""), Brushes.Gold);
            if (r.StartsWith("small_classes="))
                return (L("Many small class files — stub/wrapper pattern: ", "Много маленьких классов — заглушки/обёртки: ") + r.Replace("small_classes=", ""), Brushes.Gold);
            if (r.StartsWith("high_entropy="))
                return (L("High byte entropy in classes — packed/obfuscated: ", "Высокая энтропия байт — упаковка/обфускация: ") + r.Replace("high_entropy=", ""), Brushes.Gold);
            if (r.StartsWith("hash_match="))
                return ("⚠ " + L("SHA-256 hash matches known cheat: ", "SHA-256 совпадает с известным читом: ") + r.Replace("hash_match=", ""), Brushes.Red);
            if (r.StartsWith("legit_mod_discount=-"))
                return ("✓ " + L("Legitimate mod discount applied: −", "Применена скидка для легитного мода: −") + r.Replace("legit_mod_discount=-", ""), Brushes.LimeGreen);
            if (r.StartsWith("profiler="))
            {
                string val = r.Replace("profiler=", "");
                return (L("Deep analysis score: ", "Глубокий анализ: ") + val, Brushes.LightSalmon);
            }
            if (r.StartsWith("metadata=banned_modname"))
                return (L("Metadata contains banned mod name", "Метаданные содержат имя запрещённого мода"), Brushes.Red);
            if (r.StartsWith("metadata=suspicious_kw"))
                return (L("Metadata contains suspicious keywords", "Метаданные содержат подозрительные слова"), Brushes.Orange);
            if (r.StartsWith("moddisguise"))
                return (L("Mod disguised as legitimate — metadata + obfuscation", "Мод замаскирован под легитный — метаданные + обфускация"), Brushes.Red);
            if (r.StartsWith("embedPay"))
                return (L("Embedded cheat payload detected in mod", "Обнаружена встроенная нагрузка чита в моде"), Brushes.Red);
            if (r.StartsWith("obfFW"))
                return (L("Obfuscated module framework detected", "Обнаружен обфусцированный каркас модулей"), Brushes.Red);
            if (r.StartsWith("mcHook"))
                return (L("Deep Minecraft hooking — cheat behavior pattern", "Глубокое внедрение в Minecraft — паттерн чита"), Brushes.Red);
            if (r.StartsWith("cheatFW"))
                return (L("Confirmed cheat framework: infrastructure + modules", "Подтверждённый каркас чита: инфраструктура + модули"), Brushes.Red);
            if (r.StartsWith("stubPat"))
                return (L("Stub class pattern — small + flat + obfuscated", "Паттерн классов-заглушек — маленькие + плоские + обфусцированные"), Brushes.Red);
            if (r.StartsWith("stripObf"))
                return (L("Stripped obfuscation — metadata present but classes empty", "Обфускация удалена — метаданные есть, классы пустые"), Brushes.Red);
            if (r.StartsWith("bareCheat"))
                return (L("No metadata + modules + events — bare cheat framework", "Нет метаданных + модули + события — чистый каркас чита"), Brushes.Red);

            return (r, Brushes.LightGray);
        }

        private void MainTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tc && tc.SelectedItem is TabItem tab)
            {
                var content = tab.Content as FrameworkElement;
                if (content == null) return;
                content.Opacity = 0.8;
                var fadeIn = new DoubleAnimation(0.8, 1, TimeSpan.FromMilliseconds(200))
                { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
                content.BeginAnimation(OpacityProperty, fadeIn);
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            var fadeOut = new DoubleAnimation(Opacity, 0, TimeSpan.FromMilliseconds(180))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } };
            fadeOut.Completed += (s, a) => Close();
            BeginAnimation(OpacityProperty, fadeOut);
        }
    }

    // Simple wrap panel for info chips
    public class WrapGrid : Panel
    {
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapGrid),
                new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
                if (Orientation == Orientation.Horizontal)
                {
                    size.Width += child.DesiredSize.Width;
                    size.Height = Math.Max(size.Height, child.DesiredSize.Height);
                }
                else
                {
                    size.Width = Math.Max(size.Width, child.DesiredSize.Width);
                    size.Height += child.DesiredSize.Height;
                }
            }
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0, y = 0, rowH = 0;
            foreach (UIElement child in Children)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    if (x + child.DesiredSize.Width > finalSize.Width && x > 0)
                    {
                        x = 0;
                        y += rowH;
                        rowH = 0;
                    }
                    child.Arrange(new Rect(x, y, child.DesiredSize.Width, child.DesiredSize.Height));
                    x += child.DesiredSize.Width;
                    rowH = Math.Max(rowH, child.DesiredSize.Height);
                }
                else
                {
                    child.Arrange(new Rect(0, y, finalSize.Width, child.DesiredSize.Height));
                    y += child.DesiredSize.Height;
                }
            }
            return finalSize;
        }
    }
}
