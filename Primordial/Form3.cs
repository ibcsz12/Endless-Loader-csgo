using Guna.UI2.WinForms;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Primordial.Form2;

namespace Primordial
{
    public partial class Form3 : Form
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0x0;
        private const int SW_SHOW = 0x5;

        private static Random random = new Random();

        public Form3()
        {
            InitializeComponent();
            shadowpanel.FillColor = Color.FromArgb(22, 21, 22);
            this.BackColor = Color.FromArgb(32, 31, 32);
            shadowpanel.ShadowDepth = 0;
            shadowpanel.ShadowShift = 0;
            guna2ProgressBar1.Hide();
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            guna2ProgressBar1.Hide();

            try
            {
                if (CheatConfig.SelectedCheat == "neverlose")
                {
                    // ШАГ 1: Распаковка всех необходимых файлов в папку приложения
                    label4.Location = new Point(145, 194);
                    label4.Text = "extracting files...";

                    string serverPath = ExtractResourceToFile("server", "server.exe");
                    string injectorPath = ExtractResourceToFile("injector", "injector.exe");
                    string neverloseDllPath = ExtractResourceToFile("neverlose", "neverlose.dll"); // Распаковываем neverlose.dll

                    // ШАГ 2: Запуск сервера в скрытом режиме
                    label4.Text = "starting local server...";

                    ProcessStartInfo serverInfo = new ProcessStartInfo
                    {
                        FileName = serverPath,
                        UseShellExecute = true,
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
                    };
                    Process.Start(serverInfo);

                    // Пауза 2 секунды после запуска сервера
                    await Task.Delay(2000);

                    // ШАГ 3: Запуск инжектора
                    label4.Text = "starting injector process...";

                    ProcessStartInfo injectorInfo = new ProcessStartInfo
                    {
                        FileName = injectorPath,
                        UseShellExecute = true,
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
                    };
                    Process.Start(injectorInfo);

                    // Пауза 2 секунды после запуска инжектора
                    await Task.Delay(2000);
                }

                // ШАГ 4: Запуск и ожидание CS
                Process[] gameProcesses = Process.GetProcessesByName("csgo"); // Или "cs2"

                if (gameProcesses.Length == 0)
                {
                    label4.Location = new Point(145, 194);
                    label4.Text = "starting game...";
                    Process.Start("steam://rungameid/4465480");

                    int timeoutSeconds = 60;
                    int elapsed = 0;

                    while (Process.GetProcessesByName("csgo").Length == 0)
                    {
                        await Task.Delay(1000);
                        elapsed++;
                        if (elapsed >= timeoutSeconds)
                        {
                            MessageBox.Show("Превышено время ожидания запуска игры!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // ШАГ 5: Индикация готовности и завершение
                label4.Location = new Point(147, 194);
                label4.Text = "waiting for process window...";
                await Task.Delay(3000);

                label4.Location = new Point(150, 194);
                label4.Text = "completing setup...";

                guna2ProgressBar1.Value = 0;
                guna2ProgressBar1.Show();

                for (int i = 0; i <= 100; i += 5)
                {
                    guna2ProgressBar1.Value = i;
                    await Task.Delay(30);
                }

                if (CheatConfig.SelectedCheat == "primordial")
                {
                    label4.Text = "injecting primordial...";
                    string dllPath = ExtractResourceToFile("primordial_fix", "primordial_fix.dll");

                    bool success = await Task.Run(() => Injector.Run(dllPath));
                    if (!success)
                    {
                        MessageBox.Show("Ошибка инициализации модуля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                label4.Text = "successfully loaded!";
                await Task.Delay(1000);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время выполнения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод распаковки файлов из ресурсов в папку с приложением
        private string ExtractResourceToFile(string resourceName, string fileName)
        {
            // Получаем путь к текущей папке, где запущен наш .exe
            string appFolder = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(appFolder, fileName);

            byte[] resourceBytes = (byte[])Properties.Resources.ResourceManager.GetObject(resourceName);

            if (resourceBytes == null)
            {
                throw new Exception($"Ресурс '{resourceName}' не найден в Resources.resx!");
            }

            // Записываем файл прямо рядом с нашим .exe
            File.WriteAllBytes(filePath, resourceBytes);
            return filePath;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}