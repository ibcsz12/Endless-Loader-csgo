using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Primordial
{
    public partial class Form2 : Form
    {
        private Timer fadeTimer = new Timer();
        private Random random = new Random();

        public Form2()
        {
            InitializeComponent();
            guna2Button2.Hide();
            label3.Hide();
            label4.Hide();
            label5.Hide();
            load.Hide();
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
                fadeTimer.Tick -= fadeIn;
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
                Environment.Exit(0);
            }
            else
            {
                Opacity -= 0.05;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Opacity = 0;

            fadeTimer.Interval = 10;
            fadeTimer.Tick += fadeIn;
            fadeTimer.Start();

            label9.Text = Properties.Settings.Default.userName;
            guna2PictureBox6.Hide(); // Скрываем логотип Neverlose
            guna2PictureBox1.Show(); // Показываем логотип Primordial

            // --- ПРИВЯЗЫВАЕМ КЛИКИ ПО ИКОНКАМ И ТЕКСТУ СРАЗУ ПРИ ЗАГРУЗКЕ ---
            // Элементы внутри Primordial
            label1.Click += guna2ShadowPanel2_Click;
            label2.Click += guna2ShadowPanel2_Click;
            guna2PictureBox3.Click += guna2ShadowPanel2_Click;

            // Элементы внутри Neverlose
            label12.Click += guna2ShadowPanel5_Click;
            label11.Click += guna2ShadowPanel5_Click;
            guna2PictureBox5.Click += guna2ShadowPanel5_Click;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Opacity > 0)
            {
                e.Cancel = true;

                fadeTimer.Stop();
                fadeTimer.Tick -= fadeIn;
                fadeTimer.Tick -= fadeOut;

                fadeTimer.Tick += fadeOut;
                fadeTimer.Start();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.Show();
            this.Hide();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start("https://dsc.gg/grclan");
        }

        public static class CheatConfig
        {
            public static string SelectedCheat = "primordial";
        }

        private void guna2ShadowPanel2_Click(object sender, EventArgs e)
        {
            CheatConfig.SelectedCheat = "primordial";

            guna2PictureBox1.Show(); // Показываем лого Primordial
            guna2PictureBox6.Hide(); // Скрываем лого Neverlose

            guna2ShadowPanel2.FillColor = Color.FromArgb(32, 31, 32);
            guna2ShadowPanel5.FillColor = Color.FromArgb(22, 21, 22);

            this.BackColor = Color.FromArgb(22, 21, 22);
            this.shadowpanel.FillColor = Color.FromArgb(32, 31, 32);

            // Розовые акценты
            this.guna2Shapes1.FillColor = Color.FromArgb(194, 155, 165);
            this.guna2Shapes1.BorderColor = Color.FromArgb(194, 155, 165);

            // Карточки выбора
            this.guna2ShadowPanel1.FillColor = Color.FromArgb(22, 21, 22);

            // Информационные панели
            this.guna2ShadowPanel4.FillColor = Color.FromArgb(32, 31, 32);
            this.guna2ShadowPanel3.FillColor = Color.FromArgb(32, 31, 32);

            load.Show();
            label8.Show();
            label6.Show();
            label7.Show();
            label3.Show();
            label3.Text = "csgo - Primordial";
            label4.Show();
            label5.Show();
        }

        private void guna2ShadowPanel5_Click(object sender, EventArgs e)
        {
            CheatConfig.SelectedCheat = "neverlose";

            guna2PictureBox1.Hide(); // Скрываем лого Primordial
            guna2PictureBox6.Show(); // Показываем лого Neverlose

            // Меняем цвета панелей
            guna2ShadowPanel5.FillColor = Color.FromArgb(32, 31, 32);
            guna2ShadowPanel2.FillColor = Color.FromArgb(22, 21, 22);

            this.BackColor = Color.FromArgb(12, 16, 24);
            this.shadowpanel.FillColor = Color.FromArgb(18, 24, 36);

            // Голубые/Cyan акценты Neverlose
            this.guna2Shapes1.FillColor = Color.FromArgb(0, 180, 255);
            this.guna2Shapes1.BorderColor = Color.FromArgb(0, 180, 255);

            // Карточки выбора
            this.guna2ShadowPanel1.FillColor = Color.FromArgb(12, 16, 24);
            this.guna2ShadowPanel2.FillColor = Color.FromArgb(22, 32, 48);

            // Информационные панели
            this.guna2ShadowPanel4.FillColor = Color.FromArgb(18, 24, 36);
            this.guna2ShadowPanel3.FillColor = Color.FromArgb(18, 24, 36);

            load.Show();
            label8.Show();
            label6.Show();
            label7.Show();
            label3.Show();
            label3.Text = "csgo - Neverlose";
            label4.Show();
            label5.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2ShadowPanel2_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void shadowpanel_Paint(object sender, PaintEventArgs e) { }
    }
}