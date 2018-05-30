using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYSExam
{
    public partial class Form1 : Form
    {
        bool option1Selected = true;
        bool option2Selected = false;
        bool option3Selected = false;
        static string dockerURL = "http://localhost:32785/";
        #region UI code
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_ClickAsync(sender, e);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1 && textBox1.Text.Length % 8 != 0)
            {
                label2.Visible = true;
                button1.Enabled = false;
            }
            else
            {
                label2.Visible = false;
                button1.Enabled = true;
            }
        }
        #region Hover
        private void changePanelHover(object sender, EventArgs e, bool hover)
        {
            Panel pan = sender as Panel;
            pan.BackColor = Color.FromArgb(255, 79, 87, 96);
            if (hover) pan.BackColor = Color.FromArgb(255, 79, 87, 96);
            else pan.BackColor = Color.FromArgb(255, 53, 62, 73);
        }

        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            if (!option2Selected) changePanelHover(sender, e, true);
        }

        private void panel3_MouseLeave(object sender, EventArgs e)
        {
            if (!option2Selected) changePanelHover(sender, e, false);
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            if (!option2Selected) changePanelHover(this.panel3, e, true);
        }

        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            if (!option3Selected) changePanelHover(sender, e, true);
        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            if (!option3Selected) changePanelHover(sender, e, false);
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            if (!option3Selected) changePanelHover(this.panel4, e, true);
        }

        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            if (!option1Selected) changePanelHover(sender, e, true);
        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            if (!option1Selected) changePanelHover(sender, e, false);
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            if (!option1Selected) changePanelHover(this.panel2, e, true);
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length % 8 != 0)
            {
                label8.Visible = true;
                if (textBox6.Text.Equals("help"))
                {
                    richTextBox1.Visible = true;
                }
            }
            else
            {
                label8.Visible = false;
            }
        }
        private void panel3_Click(object sender, EventArgs e)
        {
            option1Selected = false;
            option2Selected = true;
            option3Selected = false;
            panelCombined.Visible = true;
            panelPerformance.Visible = false;
            panel2Functions.Visible = false;
            Panel pan = sender as Panel;
            pan.BackColor = Color.FromArgb(255, 0, 150, 136);
            changePanelHover(this.panel2, e, false);
            changePanelHover(this.panel4, e, false);
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            option1Selected = true;
            option2Selected = false;
            option3Selected = false;
            panelCombined.Visible = false;
            panelPerformance.Visible = true;
            panel2Functions.Visible = false;
            Panel pan = sender as Panel;
            pan.BackColor = Color.FromArgb(255, 0, 150, 136);
            changePanelHover(this.panel3, e, false);
            changePanelHover(this.panel4, e, false);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            panel2_Click(this.panel2, e);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            panel3_Click(this.panel3, e);
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            option1Selected = false;
            option2Selected = false;
            option3Selected = true;
            panelCombined.Visible = false;
            panelPerformance.Visible = false;
            panel2Functions.Visible = true;
            Panel pan = sender as Panel;
            pan.BackColor = Color.FromArgb(255, 0, 150, 136);
            changePanelHover(this.panel3, e, false);
            changePanelHover(this.panel2, e, false);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            panel4_Click(this.panel4, e);
        }
        #endregion
        #endregion

        //Performance button
        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            textBox2.Text = "";
            string urlAzure = null;
            string urlDocker = null;
            string name = null;
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    name = "Reverse a String";
                    urlAzure = "https://reverseastring2.azurewebsites.net/api/ReverseAString";
                    urlDocker = dockerURL + "api/reverseString?str=";
                    break;
                case 1:
                    name = "Binary to Text";
                    urlAzure = "https://binarytotext2.azurewebsites.net/api/BinaryToText";
                    urlDocker = dockerURL + "api/binaryToText?str=";
                    break;
                case 2:
                    name = "Sort a String alphabetically";
                    urlAzure = "https://stringsortedalphabetically2.azurewebsites.net/api/SortString";
                    urlDocker = dockerURL + "api/sortString?str=";
                    break;
                case 3:
                    name = "Encrypt a String";
                    urlAzure = "https://stringencryption2.azurewebsites.net/api/StringEncryption";
                    urlDocker = dockerURL + "api/encryptString?str=";
                    break;
            }
            textBox7.AppendText(name + Environment.NewLine);
       
            if (checkBox1.Checked)
            {
                Stopwatch t = new Stopwatch();
                t.Start();
                textBox2.Text = await azureFunction(textBox1.Text, urlAzure);
                t.Stop();
                textBox7.AppendText("Azure Functions = " + t.ElapsedMilliseconds + "ms" + Environment.NewLine);
            }
            if (checkBox2.Checked)
            {
                Stopwatch t = new Stopwatch();
                t.Start();
                textBox2.Text = await dockerFunction(textBox1.Text, urlDocker);
                t.Stop();
                textBox7.AppendText("Docker = " + t.ElapsedMilliseconds + "ms" + Environment.NewLine);
            }
            textBox7.AppendText("---------------------" + Environment.NewLine);
        }

        //Combined button
        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            textBox3.Text = "";
            if (textBox4.Text.Length > 0 && textBox8.Text.Length > 0)
            {

                string username = textBox4.Text;
                string password = textBox8.Text;
                if (await loginFunction(username, password))
                {
                    textBox3.ForeColor = Color.Green;
                    textBox3.Text = "Access granted";

                } else
                {
                    textBox3.ForeColor = Color.Red;
                    textBox3.Text = "Access denied";
                }
            }
            else
            {
                textBox3.ForeColor = Color.Red;
                textBox3.Text = "Please fulfill the required fields";
            }
        }

        //All together Button
        private async void button3_ClickAsync(object sender, EventArgs e)
        {
            textBox5.Text = "";
            label6.Visible = false;
            if ((textBox6.Text.Length % 8) == 0)
            {
                Stopwatch t = new Stopwatch();
                t.Start();
                string url = "https://alltogether.azurewebsites.net/api/AllTogether";
                textBox5.Text = await azureFunction(textBox6.Text, url);
                t.Stop();
                label6.Text = "Elapsed time: " + t.ElapsedMilliseconds + "ms";
                label6.Visible = true;
            }
        }

        public static async Task<string> azureFunction(string text, string url)
        {
            var _httpClient = new HttpClient();

            string str = "{\"str\":\"" + text + "\"}";
            _httpClient.DefaultRequestHeaders
             .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var content = new StringContent(str, Encoding.UTF8, "application/json"))
            {
       
                var result = await _httpClient.PostAsync($"{url}", content).ConfigureAwait(false);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return await result.Content.ReadAsStringAsync();
                }
                else
                {
                    // Something wrong happened
                    return "Ups! Something wrong happened";
                }
            }
        }

        public static async Task<string> dockerFunction(string text, string url)
        {
            var _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders
             .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await _httpClient.GetAsync($"{url + text}").ConfigureAwait(false);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return await result.Content.ReadAsStringAsync();
            }
            else
            {
                // Something wrong happened
                return "Ups! Something wrong happened";
            }
        }

        public static async Task<bool> loginFunction(string user, string password)
        {
            var _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders
             .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string url = dockerURL + "api/login?user=" + user + "&password=" + password;

            var result = await _httpClient.GetAsync($"{url}").ConfigureAwait(false);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                if (result.Content.ReadAsStringAsync().Result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }
    }
}
