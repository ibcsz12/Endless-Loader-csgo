using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Primordial
{
    public partial class Form1 : Form
    {
        private Timer fadeTimer = new Timer();
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            guna2PictureBox4.Hide();
            guna2Button2.Hide();
        }

        public string Rstr(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Анимация ПОЯВЛЕНИЯ
        private void fadeIn(object sender, EventArgs e)
        {
            if (Opacity >= 1)
            {
                fadeTimer.Stop();
                fadeTimer.Tick -= fadeIn; // Отвязываем метод появления после завершения
            }
            else
            {
                Opacity += 0.05;
            }
        }

        // Анимация ЗАТУХАНИЯ
        private void fadeOut(object sender, EventArgs e)
        {
            if (Opacity <= 0)
            {
                fadeTimer.Stop();
                fadeTimer.Tick -= fadeOut;

                // Полностью завершаем процесс приложения
                Environment.Exit(0);
            }
            else
            {
                Opacity -= 0.05;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.userName != string.Empty)
            {
                username.Text = Properties.Settings.Default.userName;
                password.Text = Properties.Settings.Default.passUser;
            }

            Opacity = 0; // Стартуем с нулевой прозрачности

            fadeTimer.Interval = 10;
            fadeTimer.Tick += fadeIn; // Подключаем анимацию появления
            fadeTimer.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Если форма ещё видима — отменяем мгновенное закрытие и запускаем затухание
            if (Opacity > 0)
            {
                e.Cancel = true;

                fadeTimer.Stop();          // Останавливаем текущий ход таймера
                fadeTimer.Tick -= fadeIn;  // Сбрасываем подписку на появление
                fadeTimer.Tick -= fadeOut; // Сбрасываем старую подписку на затухание

                fadeTimer.Tick += fadeOut; // Подключаем плавно затухание
                fadeTimer.Start();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.userName = username.Text;
            Properties.Settings.Default.passUser = password.Text;
            Properties.Settings.Default.Save();

            // Переход на Form2
            Form2 form = new Form2();
            form.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void shadowpanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}