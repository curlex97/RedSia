using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using NAudio.Wave;
using RedSiaCore.Core;
using RedSiaCore.SiaLibrary;
using RedSiaCore.Utils;
using System.Reflection;
using System.Speech.Synthesis;
using RedSiaCore.IPT;
using MessageBox = System.Windows.MessageBox;

namespace RedSia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        WaveIn waveIn;
        WaveFileWriter writer;
        string outputFilename = "demo.wav";
        bool ON = false;
        Timer timer1 = new Timer();
      
      
    public MainWindow()
        {
            InitializeComponent();

           

            timer1.Enabled = false;
            timer1.Tick += timer1_Tick;
            timer1.Interval = 46;
           // RedSiaExecutor.Execute("Здравствуй");

            Timer timer = new Timer();
            timer.Tick += (o, args) =>
            {
                try
                {
                    if (InputEmulation.GetAsyncKeyState(Keys.Home) && !ON)
                    {
                        ON = true;
                        waveIn = new WaveIn();
                        waveIn.DeviceNumber = 0;
                        waveIn.DataAvailable += waveIn_DataAvailable;
                        waveIn.RecordingStopped +=
                            new EventHandler<NAudio.Wave.StoppedEventArgs>(waveIn_RecordingStopped);
                        waveIn.WaveFormat = new WaveFormat(16000, 1);
                        writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                        label1.Text = "Идет запись...";
                        waveIn.StartRecording();
                    }
                    else if (!InputEmulation.GetAsyncKeyState(Keys.Home) && ON)
                    {
                        waveIn.StopRecording();
                        label1.Text = "";
                        ON = false;
                        timer1.Enabled = true;
                    }
                }
                catch
                {

                }


            };
            timer.Interval = 46;
            timer.Enabled = true;

        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                writer.WriteData(e.Buffer, 0, e.BytesRecorded);

            }
            catch
            {

            }
        }

        void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            try
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
            catch
            {

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            try
            {
                WebRequest request = WebRequest.Create("https://www.google.com/speech-api/v2/recognize?output=json&lang=ru-RU&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw");
                //
                request.Method = "POST";
                byte[] byteArray = File.ReadAllBytes(outputFilename);
                request.ContentType = "audio/l16; rate=16000"; //"16000";
                request.ContentLength = byteArray.Length;
                request.GetRequestStream().Write(byteArray, 0, byteArray.Length);
                // Получить ответ.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Откройте поток, используя StreamReader для легкого доступа.
                StreamReader reader = new StreamReader(response.GetResponseStream());
                //Читайте содержание.
                string str = reader.ReadToEnd();
                string phrase = ParseJson(str)[0];
                label1.Text = phrase;
                RedSiaExecutor.Execute(phrase);
               // KeyboardImulation.Print(phrase);
              //  KeyboardImulation.PressEnter();
                // Очистите потоки.
                reader.Close();
                response.Close();
                timer1.Enabled = false; //Получить результат
                //button1_Click(this, EventArgs.Empty); //Распознавать после ответа 
            }
            catch(Exception ex)
            {

                if (ex is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                }
            }


        }

        string[] ParseJson(string json)
        {
            List<string> list = new List<string>();
            try
            {
                string[] lines = json.Split(new[] { "\"transcript\":\"" }, StringSplitOptions.RemoveEmptyEntries);


                for (int i = 1; i < lines.Length; i++)
                    list.Add(lines[i].Substring(0, lines[i].IndexOf("\"", StringComparison.Ordinal)));
            }
            catch
            {

            }

            return list.ToArray();
        }
    }
}
