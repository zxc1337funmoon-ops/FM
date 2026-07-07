using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FMTool
{
    public partial class ScanReportWindow : Window
    {
        private string _filePath = "";
        private string _sha256 = "";
        private int _lang = 0;

        public ScanReportWindow()
        {
            InitializeComponent();
        }

        private string L(string en, string ru) => _lang == 1 ? ru : en;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var owner = Owner as MainWindow;
            if (owner != null)
            {
                _lang = owner.GetLanguage();
                Left = owner.Left + Math.Max(0, (owner.Width - Width) / 2);
                Top = owner.Top + Math.Max(0, (owner.Height - Height) / 2);
            }

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(320))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
            BeginAnimation(OpacityProperty, fadeIn);

            var slideIn = new DoubleAnimation(20, 0, TimeSpan.FromMilliseconds(350))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };
            var content = Content as FrameworkElement;
            if (content == null) return;
            content.BeginAnimation(TranslateTransform.YProperty, null);
            var tt = new TranslateTransform();
            content.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, slideIn);
        }

        public void LoadFile(MainWindow.ScanItem item)
        {
            _filePath = item.Path;
            ReportFileName.Text = item.Name;

            ReportTitle.Text = L("File Report", "Отчёт о файле");
            StatusTitle.Text = L("Status", "Статус");
            TypeTitle.Text = L("Type", "Тип");
            SizeTitle.Text = L("Size", "Размер");
            DetailTitle.Text = L("File Information", "Информация о файле");
            LblName.Text = L("Name:", "Имя:");
            LblPath.Text = L("Path:", "Путь:");
            LblType.Text = L("Type:", "Тип:");
            LblHash.Text = "SHA-256:";
            LblVerdict.Text = L("Verdict:", "Вердикт:");
            OpenFolderBtn.Content = L("📂 Open Folder", "📂 Открыть папку");
            CopyPathBtn.Content = L("📋 Copy Path", "📋 Копировать путь");
            CopyHashBtn.Content = L("🔑 Copy SHA-256", "🔑 Копировать SHA-256");
            SendToModBtn.Content = L("🔍 Analyze in Mod Checker", "🔍 Анализ в Mod Checker");
            CloseBtn2.Content = L("× Close", "× Закрыть");
            CloseBtn.ToolTip = L("Close (Esc)", "Закрыть (Esc)");

            // Status
            HeaderDot.Background = item.Status switch
            {
                "Cheat Client" => Brushes.Red,
                "Suspicious" => Brushes.Orange,
                _ => Brushes.LimeGreen
            };
            StatusLabel.Text = item.StatusText;
            StatusLabel.Foreground = HeaderDot.Background;
            TypeLabel.Text = item.Ext;
            SizeLabel.Text = item.SizeText;

            // Details
            DetailName.Text = item.Name;
            DetailPath.Text = item.Path;
            DetailVerdict.Text = item.StatusText;
            DetailVerdict.Foreground = HeaderDot.Background;

            // Compute SHA-256
            try
            {
                if (File.Exists(_filePath))
                {
                    var fi = new FileInfo(_filePath);
                    if (fi.Length > 0 && fi.Length < 500L * 1024 * 1024)
                    {
                        using var stream = File.OpenRead(_filePath);
                        byte[] hash = SHA256.HashData(stream);
                        _sha256 = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                        DetailHash.Text = _sha256;
                    }
                    else
                    {
                        DetailHash.Text = _lang == 1 ? "Файл слишком большой (>500 МБ)" : "File too large (>500 MB)";
                    }
                }
            }
            catch
            {
                DetailHash.Text = _lang == 1 ? "Ошибка вычисления" : "Hash error";
            }

            // Hide Send to Mod Checker button for non-JAR/ZIP
            string ext = item.Ext.ToLowerInvariant();
            if (ext != "jar" && ext != "zip")
                SendToModBtn.Visibility = Visibility.Collapsed;
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = "/select,\"" + _filePath + "\"",
                    UseShellExecute = true
                });
            }
            catch { }
        }

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(_filePath);
            }
            catch { }
        }

        private void CopyHash_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_sha256))
                    Clipboard.SetText(_sha256);
            }
            catch { }
        }

        private void SendToModChecker_Click(object sender, RoutedEventArgs e)
        {
            var owner = Owner as MainWindow;
            if (owner != null && File.Exists(_filePath))
            {
                owner.SendToModChecker(_filePath);
                Close();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            // Fade out animation
            var fadeOut = new DoubleAnimation(Opacity, 0, TimeSpan.FromMilliseconds(180))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } };
            fadeOut.Completed += (s, a) => Close();
            BeginAnimation(OpacityProperty, fadeOut);
        }
    }
}
