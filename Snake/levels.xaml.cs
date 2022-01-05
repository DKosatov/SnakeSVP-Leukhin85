using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Snake;

namespace Snake
{
    /// <summary>
    /// Логика взаимодействия для levels.xaml
    /// </summary>
    public partial class levels : Window
    {
        SoundPlayer ButtonClick = new SoundPlayer("../../Resources/knopka.wav");
        MediaPlayer TitleMusic = new MediaPlayer();

        public levels()
        {
            InitializeComponent();
        }

        private void lvl1_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick.Play();
            TitleMusic.Stop();
            lvl1 menu = new lvl1();
            menu.Show();
            Close();
        }

        private void lvlBack_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick.Play();
            TitleMusic.Stop();
            Window1 menu = new Window1();
            menu.Show();
            Close();
        }

        private void lvl2_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick.Play();
            TitleMusic.Stop();
            lvl2 menu = new lvl2();
            menu.Show();
            Close();
        }

        private void lvl3_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick.Play();
            TitleMusic.Stop();
            lvl3 menu = new lvl3();
            menu.Show();
            Close();
        }

        private void lvl4_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick.Play();
            TitleMusic.Stop();
            lvl4 menu = new lvl4();
            menu.Show();
            Close();
        }
    }
}
