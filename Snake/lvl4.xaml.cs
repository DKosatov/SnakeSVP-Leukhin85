using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using Snake;

namespace Snake
{
    public partial class lvl4 : Window
    {
        public lvl4()
        {
            InitializeComponent();
            initGame();
            drawborders();
        }

        private const int SIZE = 512;   
        private const int DOT_SIZE = 16;    

        private const int ALL_DOTS = SIZE / DOT_SIZE;

        private bool right = false;
        private bool left = false;

        private bool isGameOver = false;

        private int dots = 2;   
        private int counterScore = 0;   
        private int[] x = new int[ALL_DOTS];
        private int[] y = new int[ALL_DOTS];
        private int[] topy = new int[ALL_DOTS];

        private DispatcherTimer timer;    

        private const int startSpeed = 70;
        private const int speedSubstractor = 2;
        private int speed = startSpeed;    

        SoundPlayer GameOverSound = new SoundPlayer("../../Resources/gameover.wav");
        SoundPlayer ButtonClick = new SoundPlayer("../../Resources/knopka.wav");
        MediaPlayer BackgroundMusic = new MediaPlayer();

        internal static string language = App.language;

        string score_t, speed_t, status_t;


        public Rectangle field;
        public Rectangle snakeDot;
        public Rectangle borderTop;
        public Rectangle borderBottom;
        public Rectangle borderMiddle;

        borders genb;

        private void MainGameLoop(object sender, EventArgs e)//
        {
            move();
            checkCollisions();

            if (!isGameOver)
            {
                drawSnake();
                drawborders();
                textbox.SetResourceReference(TagProperty, "In_Game");
                gameStatusLabel.Content = status_t + textbox.Tag;
                lvl4Field.Children.Remove(GameOverLb);
                lvl4Field.Children.Remove(GamePausedLb);
                lvl4Field.Children.Remove(ScoreLb);
                lvl4Field.Children.Remove(PressEnterLb);
                lvl4Field.Children.Remove(PressSpaceLb);
            }
            else
            {
                timer.Stop();
                textbox.SetResourceReference(TagProperty, "Game_Over");
                gameStatusLabel.Content = status_t + textbox.Tag;
                BackgroundMusic.Stop();
                GameOverSound.Play();
                drawGameOver();
                drawScore();
                drawPressSpace();
            }

            Thread.Sleep(speed);
        }

        private void initGame()//
        { 
            genb = new borders();
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            scoreLabel.SetResourceReference(DataContextProperty, "Score");
            score_t = scoreLabel.Content.ToString();
            scoreLabel.Content += " : " + counterScore;
            speedLabel.SetResourceReference(DataContextProperty, "Speed");
            speed_t = speedLabel.Content.ToString();
            gameStatusLabel.SetResourceReference(DataContextProperty, "Status");
            status_t = gameStatusLabel.Content.ToString();

            for (int i = 0; i < dots; i++)
            {
                x[i] = 256;
                y[i] = 432;
            }

            timer = new DispatcherTimer();
            timer.Tick += MainGameLoop;
            timer.Start();
            UpdateSpeed(); 
            genb.generateBorders();
            BackgroundMusic.Open(new Uri("../../Resources/iwbplus.mp3", UriKind.RelativeOrAbsolute));
            BackgroundMusic.Play();
        }

        private void move()//
        {
            if (topy[0] < 496)
            {
                topy[0] += DOT_SIZE; 
            }
            else { topy[0] = 0; }
            if (topy[0] == 0)
            {
                genb.generateBorders(); counterScore++; textbox.SetResourceReference(TagProperty, "Score"); scoreLabel.Content = textbox.Tag + " : " + counterScore; UpdateSpeed();
            }
            if (right && x[0] <= (SIZE-2*DOT_SIZE))
            {
                x[0] += DOT_SIZE;
            }
            if (left && x[0] > 16)
            {
                x[0] -= DOT_SIZE;
            }
        }

        private void checkCollisions()//
        {
            if (topy[0]==432 && ((x[0]<=genb.borderssize2 && x[0] >= genb.borderssize1) || 
                (x[0]  <= genb.borderssize4 && x[0]  >= genb.borderssize3) || (x[0]  <= genb.borderssize6 && x[0] >= genb.borderssize5)))
            {
                isGameOver = true;
                return;
            }
        }

        public void drawborders()
        {
            if (borderBottom==null&&borderMiddle==null&&borderTop==null)
            {
                borderTop = new Rectangle();
                borderBottom = new Rectangle();
                borderMiddle = new Rectangle();
                borderTop.Fill = borderBottom.Fill = borderMiddle.Fill = Brushes.Black;
                borderTop.Stroke = borderBottom.Stroke = borderMiddle.Stroke = Brushes.Black;
                borderTop.HorizontalAlignment = borderBottom.HorizontalAlignment = borderMiddle.HorizontalAlignment = HorizontalAlignment.Left;
                borderTop.VerticalAlignment = borderBottom.VerticalAlignment = borderMiddle.VerticalAlignment = VerticalAlignment.Top;
            }
            borderTop.Width = genb.borderssize2+18 - genb.borderssize1;
            borderMiddle.Width = genb.borderssize4+18 - genb.borderssize3;
            borderBottom.Width = genb.borderssize6 - genb.borderssize5;
            borderTop.Height = borderBottom.Height = borderMiddle.Height = DOT_SIZE;
            lvl4Field.Children.Remove(borderTop);
                borderTop.Margin = new Thickness
                {
                    Left = genb.borderssize1,
                    //Right = genb.borderssize2,
                    Top = topy[0],
                };
                lvl4Field.Children.Add(borderTop);

                lvl4Field.Children.Remove(borderMiddle);
                borderMiddle.Margin = new Thickness
                {
                    Left = genb.borderssize3,
                    //Right = genb.borderssize4,
                    Top = topy[0],
                };
                lvl4Field.Children.Add(borderMiddle);

                lvl4Field.Children.Remove(borderBottom);
                borderBottom.Margin = new Thickness
                {
                    Left = genb.borderssize5,
                    //Right = genb.borderssize6,
                    Top = topy[0],
                };
                lvl4Field.Children.Add(borderBottom);
        }

        private void drawSnake()//
        {
            if (snakeDot == null )
            {
                snakeDot = new Rectangle();

                snakeDot.Fill = Brushes.DarkGreen;
                snakeDot.Stroke = Brushes.DarkGreen;
                snakeDot.HorizontalAlignment = HorizontalAlignment.Left;
                snakeDot.VerticalAlignment = VerticalAlignment.Top;

                snakeDot.Width = DOT_SIZE;
                snakeDot.Height = DOT_SIZE*4;
             }


            lvl4Field.Children.Remove(snakeDot);
            snakeDot.Margin = new Thickness
            {
                Left = x[0],
                Top = y[0],
            };
            lvl4Field.Children.Add(snakeDot);
        }

        Label GamePausedLb = new Label();
        private void drawGamePaused()//
        {
            textbox.SetResourceReference(TagProperty, "Paused");
            GamePausedLb.Content = textbox.Tag;
            GamePausedLb.Margin = new Thickness
            {
                Left = 175,
                Top = 156
            };
            GamePausedLb.Height = 54;
            GamePausedLb.Width = 174;
            GamePausedLb.FontSize = 35;
            GamePausedLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            lvl4Field.Children.Remove(GamePausedLb);
            lvl4Field.Children.Add(GamePausedLb);

            return;
        }

        Label GameOverLb = new Label();
        private void drawGameOver()//
        {
            GameOverLb.Content = "GAME OVER";
            GameOverLb.Margin = new Thickness
            {
                Left = 138,
                Top = 158
            };
            GameOverLb.Height = 54;
            GameOverLb.Width = 251;
            GameOverLb.FontSize = 35;
            GameOverLb.BorderBrush = Brushes.White;
            GameOverLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            lvl4Field.Children.Remove(GameOverLb);
            lvl4Field.Children.Add(GameOverLb);

            return;
        }

        Label ScoreLb = new Label();
        private void drawScore()//
        {
            textbox.SetResourceReference(TagProperty, "Your Score");
            ScoreLb.Content = textbox.Tag + " : " + counterScore;
            ScoreLb.Margin = new Thickness
            {
                Left = 165,
                Top = 202
            };
            ScoreLb.Height = 38;
            ScoreLb.Width = 200;
            ScoreLb.FontSize = 20;
            ScoreLb.BorderBrush = Brushes.White;

            if (App.language == "en-US")
            {
                ScoreLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            }
            else
            if (App.language == "ru-RU")
            {
                ScoreLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            }
            else
            if (App.language == "de-DE")
            {
                ScoreLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            }
            else
            if (App.language == "it-IT")
            {
                ScoreLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            }

            lvl4Field.Children.Remove(ScoreLb);
            lvl4Field.Children.Add(ScoreLb);

            return;
        }

        Label PressEnterLb = new Label();
        private void drawPressEnter()//
        {
            textbox.SetResourceReference(TagProperty, "Enter_continue");
            PressEnterLb.Content = textbox.Tag;
            if (App.language == "ru-RU")
            {
                PressEnterLb.Margin = new Thickness
                {
                    Left = 58,
                    Top = 233
                };
                PressEnterLb.Height = 34;
                PressEnterLb.Width = 429;
                PressEnterLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            }
            else
            {
                if (App.language == "en-US")
                {
                    PressEnterLb.Margin = new Thickness
                    {
                        Left = 112,
                        Top = 231
                    };
                    PressEnterLb.Height = 34;
                    PressEnterLb.Width = 310;
                    PressEnterLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
                }
            }
            if (App.language == "de-DE")
            {
                PressEnterLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
                PressEnterLb.Height = 34;
                PressEnterLb.Width = 8000;
                PressEnterLb.Margin = new Thickness
                {
                    Left = 61,
                    Top = 233
                };
            }

            if (App.language == "it-IT")
            {
                PressEnterLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
                PressEnterLb.Height = 34;
                PressEnterLb.Width = 8000;
                PressEnterLb.Margin = new Thickness
                {
                    Left = 61,
                    Top = 233
                };
            }


            PressEnterLb.FontSize = 20;
            PressEnterLb.BorderBrush = Brushes.White;
            lvl4Field.Children.Remove(PressEnterLb);
            lvl4Field.Children.Add(PressEnterLb);

            return;
        }

        Label PressSpaceLb = new Label();
        private void drawPressSpace()//
        {
            textbox.SetResourceReference(TagProperty, "Space_restart");
            PressSpaceLb.Content = textbox.Tag;
            if (App.language == "ru-RU")
            {
                PressSpaceLb.Margin = new Thickness
                {
                    Left = 58,
                    Top = 233
                };
                PressSpaceLb.Height = 34;
                PressSpaceLb.Width = 429;
                PressSpaceLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            }
            else
            {
                if (App.language == "en-US")
                {
                    PressSpaceLb.Margin = new Thickness
                    {
                        Left = 112,
                        Top = 231
                    };
                    PressSpaceLb.Height = 34;
                    PressSpaceLb.Width = 310;
                    PressSpaceLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
                }
            }
            if (App.language == "de-DE")
            {
                PressSpaceLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
                PressSpaceLb.Height = 34;
                PressSpaceLb.Width = 8000;
                PressSpaceLb.Margin = new Thickness
                {
                    Left = 61,
                    Top = 233
                };
            }

            if (App.language == "it-IT")
            {
                PressSpaceLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
                PressSpaceLb.Height = 34;
                PressSpaceLb.Width = 8000;
                PressSpaceLb.Margin = new Thickness
                {
                    Left = 61,
                    Top = 233
                };
            }
            PressSpaceLb.FontSize = 20;
            PressSpaceLb.BorderBrush = Brushes.White;

            lvl4Field.Children.Remove(PressSpaceLb);
            lvl4Field.Children.Add(PressSpaceLb);

            return;
        }

        private void UpdateSpeed()//
        {
            if (speed == speedSubstractor)
                return;

            if (counterScore % 2 == 0)
            {
                speed -= speedSubstractor;
            }

            speedLabel.Content = speed_t + (startSpeed - speed) / speedSubstractor;
        }

        private void myKeyDown(object sender, KeyEventArgs e)//
        {
            switch (e.Key)
            {
                case Key.Right:
                case Key.D:
                    left = false;
                    right = true;
                    break;
                case Key.Left:
                case Key.A:
                    right = false;
                    left = true;
                    break;
                case Key.Escape:
                    ButtonClick.Play();
                    timer.Stop();
                    textbox.SetResourceReference(TagProperty, "Paused");
                    gameStatusLabel.Content = status_t + textbox.Tag;
                    BackgroundMusic.Pause();
                    drawGamePaused();
                    drawScore();
                    drawPressEnter();
                    break;
                case Key.Return:
                    timer.Start();
                    ButtonClick.Play();
                    BackgroundMusic.Play();
                    break;
                case Key.Space:
                    if (isGameOver)
                    {
                        GameOverSound.Stop();
                        speed = 70;
                        UpdateSpeed();
                        dots = 2;
                        counterScore = 0;

                        for (int i = 0; i < dots; i++)
                        {
                            x[i] = 256;
                            y[i] = 432;
                        }
                        scoreLabel.Content = score_t + " : " + counterScore;
                        isGameOver = false;
                        BackgroundMusic.Open(new Uri("../../Resources/iwbplus.mp3", UriKind.RelativeOrAbsolute));
                        BackgroundMusic.Play();
                        timer.Start();
                    }
                    break;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Stop();
            ButtonClick.Play();
            timer.Stop();
            Window1 menu = new Window1();
            menu.Show();
            Close();
        }

        public Rectangle Clone(Rectangle rectangle)
        {
            return new Rectangle
            {
                Fill = rectangle.Fill,
                Stroke = rectangle.Stroke,
                HorizontalAlignment = rectangle.HorizontalAlignment,
                VerticalAlignment = rectangle.VerticalAlignment,
                Width = rectangle.Width,
                Height = rectangle.Height
            };
        }
    }
}
