using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace ReviTab.Forms
{
    public partial class Neuromorphism : Window
    {
        private SoundPlayer player;

        private Dictionary<int, string> soundList;
        public Neuromorphism()
        {
            soundList = new Dictionary<int, string>();
            soundList.Add(0, "Buzz");
            soundList.Add(1, "Temple Bell Small");
            soundList.Add(2, "Rooster Crowing");
            soundList.Add(3, "Stampede Large");
            soundList.Add(4, "Store Door Chime");
            soundList.Add(5, "Whistling");
            soundList.Add(6, "Woop Woop");
            soundList.Add(7, "Yahoo");
            soundList.Add(8, "Zombie In Pain");
            soundList.Add(9, "Bongocha");

            InitializeComponent();
            //System.IO.Stream str = Resource1.woop;
            //var res = Resource1.woop;
            var rm = Resource1.ResourceManager;
            Random ranNumber = new Random();
            int x = ranNumber.Next(soundList.Count() + 1);
            string currentSound = soundList[x];
            var sound = (System.IO.Stream)rm.GetObject(currentSound);
            player = new SoundPlayer(sound);
            textBox.Content = $"Sound name: {currentSound}";
            textBox.Visibility = Visibility.Hidden;
        }

        private void LoveIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                textBox.Visibility = Visibility.Visible;
                player.PlayLooping();
            }
            //Debug.WriteLine("UHAAAAAA");
            catch { }
        }

        private void LoveIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            player.Stop();
        }


    }
}
