using fastJSON;
using HTTPSimulator.DataGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTTPSimulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HTTPRequester.SetGenerator(new Sensor_01());
            comboBox1.Items.Add("LORA Data");
            comboBox1.SelectedIndex = 0;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                listBox1.Items.Add(textBox1.Text);

                var urlList = new List<string>();
                for (int i = 0; i < listBox1.Items.Count; i++)
                    urlList.Add(listBox1.Items[i].ToString());
                HTTPRequester.SetList(urlList);

                textBox1.Text = string.Empty;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int interval = -1;
            int.TryParse(textBox2.Text, out interval);

            var result = HTTPRequester.SetMode(interval);
            textBox2.Text = result.Key.ToString();
            button3.Text = result.Value ? "stop" : "start";
        }
    }
}
