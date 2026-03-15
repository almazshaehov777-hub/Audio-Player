using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Threading;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        bool znak = false;
        string filePath;
        string fillName;
        List<string> song = new List<string>();
        List<string> songName = new List<string>();

        int index;
        TimeSpan dur;
        int sec = 0;
        int min = 0;

        MediaPlayer MP = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();

            slider.ValueChanged += Slider_ValueChanged;

            MP.MediaOpened += Mp_MediaOpened;

            slider.Visibility = Visibility.Hidden;
            b.Visibility = Visibility.Hidden;
            stackPanel.Visibility = Visibility.Hidden;
            slider2.Visibility = Visibility.Hidden;
            MuseSwiperRight.Visibility = Visibility.Hidden;
            MuseSwiperLeft.Visibility = Visibility.Hidden;
            label1.Content = "100";

            DispatcherTimer DT = new DispatcherTimer();
            DT.Interval = TimeSpan.FromSeconds(1);
            DT.Tick += DT_Tick;
            DT.Start();

            try
            {
                using (StreamReader sr = new StreamReader("E:\\GitHub\\WpfApp2\\WpfApp2\\songID.txt"))
                {
                    string listSong;
                    while((listSong = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(listSong))
                        {
                            song.Add(listSong);
                        }
                    }
                }
            }
            catch
            {

            }
            try
            {
                using(StreamReader sr = new StreamReader("E:\\GitHub\\WpfApp2\\WpfApp2\\Save.txt"))
                {
                    string LSN;
                    while((LSN = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(LSN))
                        {
                            listBox.Items.Add(LSN);
                            songName.Add(LSN);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void Mp_MediaOpened(object sender, EventArgs e)
        {
            if (MP.NaturalDuration.HasTimeSpan)
            {
                Dispatcher.Invoke(()=>slider2.Maximum = MP.NaturalDuration.TimeSpan.TotalSeconds);
            }
        }
        private void DT_Tick(object sender, EventArgs e)
        {
            if(MP.Position >= MP.NaturalDuration && index < song.Count() - 1)
            {
                ++index;
                MP.Open(new Uri(song[index]));
                MP.Play();
                slider2.Value = 0;
            }
            if(MP.Position >= MP.NaturalDuration && index == song.Count() - 1)
            {
                index = -1;
            }
            slider2.Value = slider2.Value + 1;
            if (MP != null)
            {
                if (MP.NaturalDuration.HasTimeSpan)
                {
                    ++sec;
                    if (sec >= 60)
                    {
                        while (sec >= 60)
                        {
                            ++min;
                            sec = sec - 60;
                        }
                    }
                }
            }
            label2.Content = $"{min}:{sec}";
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(mediaElement != null)
            {
                MP.Volume = slider.Value;
                label1.Content = Math.Truncate(slider.Value * 100);
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.mp3;*.wav";
            if(openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                fillName = System.IO.Path.GetFileName(filePath);
            }
            if (song.Contains(filePath) == false)
            {
                listBox.Items.Add(fillName);
                songName.Add(fillName);
                song.Add(filePath);
            }

        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBox_SelectionChanged_1(sender, e);
            ListBox listBox = sender as ListBox;
            index = listBox.SelectedIndex;
            string way = song[index];

            MP.Open(new Uri(way));
            stackPanel.Visibility = Visibility.Visible;
            slider.Visibility = Visibility.Visible;
            b.Visibility = Visibility.Visible;
            slider2.Visibility = Visibility.Visible;
            MuseSwiperRight.Visibility = Visibility.Visible;
            MuseSwiperLeft.Visibility = Visibility.Visible;
            b.Content = "| |";
            MP.Play();
            slider2.Value = 0;
            sec = 0;
            min = 0;
        }
        private void listBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(Convert.ToString(b.Content) == "▶")
            {
                MP.Play();
                b.Content = "| |";
            }
            else if(Convert.ToString(b.Content) == "| |")
            {
                MP.Pause();
                b.Content = "▶";
            }
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (slider2.IsMouseCaptureWithin)
            {
                TimeSpan duration = MP.NaturalDuration.TimeSpan;
                slider2.Maximum = duration.TotalSeconds;
                MP.Position = TimeSpan.FromSeconds(slider2.Value);
            }
            if (slider2.IsMouseCaptureWithin)
            {
                sec = Convert.ToInt32(MP.Position.TotalSeconds);
                slider2.ReleaseMouseCapture();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
                try
                {
                    using (StreamWriter sw = new StreamWriter("E:\\GitHub\\WpfApp2\\WpfApp2\\Save.txt"))
                    {
                    for (int i = 0; i < songName.Count(); ++i)
                    {
                        sw.WriteLine(songName[i]);
                    }
                    }
                }
                catch (Exception ex)
                {

                }
            try
            {
                using(StreamWriter sw = new StreamWriter("E:\\GitHub\\WpfApp2\\WpfApp2\\songID.txt"))
                {
                    for(int k = 0; k<song.Count(); ++k)
                    {
                        sw.WriteLine(song[k]);
                    }
                }
            }
            catch
            {

            }
            Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (index != song.Count() - 1)
            {
                ++index;
                MP.Open(new Uri(song[index]));
                MP.Play();
                slider2.Value = 0;
                sec = 0;
                min = 0;
            }
            else
            {
                index = 0;
                MP.Open(new Uri(song[index]));
                MP.Play();
                slider2.Value = 0;
                sec = 0;
                min = 0;
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (index != 0)
            {
                --index;
                MP.Open(new Uri(song[index]));
                MP.Play();
                slider2.Value = 0;
                sec = 0;
                min = 0;
            }
            else
            {
                index = song.Count() - 1;
                MP.Open(new Uri(song[index]));
                MP.Play();
                slider2.Value = 0;
                sec = 0;
                min = 0;
            }
        }
    }
}