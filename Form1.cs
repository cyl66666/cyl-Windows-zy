using NAudio.Wave;
using NAudio.Vorbis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsMusic1
{   
    public partial class Form1 : Form
    {
        string[] files;
        List<string> localmusiclist = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void musicplay(string filename)
        { 
            string extension=Path.GetExtension(filename);
            if (extension==".ogg")
            {
                Console.WriteLine("this is ogg file");
            }
            else 
            {
                Console.WriteLine("this is not ogg");
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "选择音频|*.mp3;*.wav;*.flac;*.ogg";
            openFileDialog1.Multiselect = true;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            { 
                listBox1.Items.Clear();
                if (files != null)
                {
                    Array.Clear(files, 0, files.Length);
                }
                files=openFileDialog1.FileNames;
                string[] array = files;
                foreach(string x in array) 
                { 
                    listBox1.Items.Add(x);
                    localmusiclist.Add(x);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(localmusiclist.Count > 0)
            {
                axWindowsMediaPlayer1.URL = localmusiclist[listBox1.SelectedIndex];
                musicplay(axWindowsMediaPlayer1.URL);
                label1.Text = Path.GetFileNameWithoutExtension(localmusiclist[listBox1.SelectedIndex]);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
            label2.Text = trackBar1.Value + "%";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int nextIndex = listBox1.SelectedIndex + 1;
            if (nextIndex >= localmusiclist.Count)
            {
                nextIndex = 0;
            }

            axWindowsMediaPlayer1.URL = localmusiclist[nextIndex];
            musicplay(axWindowsMediaPlayer1.URL);
            label1.Text = Path.GetFileNameWithoutExtension(localmusiclist[nextIndex]);
            listBox1.SelectedIndex = nextIndex;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string oggFilePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "播放音频|*.ogg";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            { 
                oggFilePath = openFileDialog.FileName; 
            }

            using (var vorbisReader = new VorbisWaveReader(oggFilePath))
            {
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(vorbisReader);
                    outputDevice.Play();

                    // 等待播放完成  
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
