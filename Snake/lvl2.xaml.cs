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
    public partial class lvl2 : Window
    {
        public lvl2()
        {
            InitializeComponent();
            initGame();
            drawField();
        }

        public const int SIZE = 512;   
        public const int DOT_SIZE = 16;

        private const int ALL_DOTS = SIZE / DOT_SIZE;//

        public bool right = true;
        public bool left = false;
        public bool up = false;
        public bool down = false;

        public bool isGameOver = false;

        public int dots = 2;   
        public int counterScore = 0;   
        public int[] x = new int[ALL_DOTS];
        public int[] y = new int[ALL_DOTS];

        public DispatcherTimer timer;

        private const int startSpeed = 160;//
        private const int speedSubstractor = 5;//

        public int speed = startSpeed;    
        SoundPlayer GameOverSound = new SoundPlayer("../../Resources/gameover.wav");
        SoundPlayer AppleEaten = new SoundPlayer("../../Resources/AppleEaten.wav");
        SoundPlayer ButtonClick = new SoundPlayer("../../Resources/knopka.wav");
        MediaPlayer BackgroundMusic = new MediaPlayer();

        internal static string language = App.language;

        string score_t, speed_t, status_t;

        private Image myApple;//
        private Image SnakeHead;//

        private Rectangle field;//
        private Rectangle snakeDot;//
        private List<Rectangle> allSnake;//

        SnakeLogic myLogic;

        private void MainGameLoop(object sender, EventArgs e)//
        {
            move();
            checkCollisions();

            if (!isGameOver)
            {
                drawField();
                drawApple();
                drawSnake();
                checkApple();
                textbox.SetResourceReference(TagProperty, "In_Game");
                gameStatusLabel.Content = status_t + textbox.Tag;
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
            myLogic = new SnakeLogic();   

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
                x[i] = 192 - (i * DOT_SIZE);
                y[i] = 144;
            }

            timer = new DispatcherTimer();
            timer.Tick += MainGameLoop;
            timer.Start();

            UpdateSpeed();
            myLogic.createApple(SIZE, DOT_SIZE, x, y);    

            BackgroundMusic.Open(new Uri("../../Resources/iwbgame.mp3", UriKind.RelativeOrAbsolute));
            BackgroundMusic.Play();
        }

        private void move()//
        {
            for (int i = dots; i > 0; i--)
            {
                x[i] = x[i - 1];
                y[i] = y[i - 1];
            }

            if (right)
            {
                x[0] += DOT_SIZE;
            }
            if (left)
            {
                x[0] -= DOT_SIZE;
            }
            if (down)
            {
                y[0] += DOT_SIZE;
            }
            if (up)
            {
                y[0] -= DOT_SIZE;
            }
        }

        private void checkApple()//
        {
            if (x[0] == myLogic.getAppleX && y[0] == myLogic.getAppleY)
            {
                AppleEaten.Play();
                dots++;
                myLogic.createApple(SIZE, DOT_SIZE, x, y);
                counterScore++; 
                UpdateSpeed();
                textbox.SetResourceReference(TagProperty, "Score");
                scoreLabel.Content = textbox.Tag + " : " + counterScore;

                allSnake.Add(Clone(snakeDot));
                allSnake[allSnake.Count - 1].Margin = new Thickness
                {
                    Left = x[allSnake.Count - 1],
                    Top = y[allSnake.Count - 1],
                };
            }
        }

        private void checkCollisions()//
        {
            if (x[0] >= (SIZE))
            {
                x[0] = 0;
            }
            if ( x[0] < 0)
            {
                x[0] = (SIZE - DOT_SIZE);
            }
            if (y[0] < 0)
            {
                y[0] = (SIZE - DOT_SIZE);
            }
            if (y[0] >= (SIZE ))
            {
                y[0] = 0;
            }

            for (int i = dots; i > 0; i--)
            {
                if (i > 4 && x[0] == x[i] && y[0] == y[i])
                {
                    isGameOver = true;
                    break;
                }
            }
        }

        private void drawField()//
        {
            if (field == null)
            {
                field = new Rectangle();
                field.Fill = Brushes.Gold;
                field.Stroke = Brushes.Gold;
                field.HorizontalAlignment = HorizontalAlignment.Left;
                field.VerticalAlignment = VerticalAlignment.Top;

                field.Width = field.Height = SIZE;
            }
            lvl2Field.Children.Remove(field);
            lvl2Field.Children.Add(field);
        }

        private void drawSnake()//
        {
            if (SnakeHead == null && snakeDot == null && allSnake == null)
            {
                SnakeHead = new Image(); 

                SnakeHead.Source = new BitmapImage(new Uri("Resources/headOfSnake.png", UriKind.Relative));
                SnakeHead.HorizontalAlignment = HorizontalAlignment.Left;
                SnakeHead.VerticalAlignment = VerticalAlignment.Top;
                SnakeHead.Width = SnakeHead.Height = DOT_SIZE;

                snakeDot = new Rectangle();

                snakeDot.Fill = Brushes.DarkGreen;
                snakeDot.Stroke = Brushes.DarkGreen;
                snakeDot.HorizontalAlignment = HorizontalAlignment.Left;
                snakeDot.VerticalAlignment = VerticalAlignment.Top;

                snakeDot.Width = snakeDot.Height = DOT_SIZE;

                allSnake = new List<Rectangle>();

                for (int i = 0; i < dots; i++)
                {
                    allSnake.Add(Clone(snakeDot));
                }
            }

            lvl2Field.Children.Remove(SnakeHead);

            SnakeHead.Margin = new Thickness
            {
                Left = x[0],
                Top = y[0],
            };

            lvl2Field.Children.Add(SnakeHead);

            for (int i = 1; i < dots; i++) 
            {
                lvl2Field.Children.Remove(allSnake[i]);

                allSnake[i].Margin = new Thickness
                {
                    Left = x[i],
                    Top = y[i],
                };

                lvl2Field.Children.Add(allSnake[i]);
            }
        }

        private void drawGamePaused()//
        {
            Label GameOverLb = new Label();
            textbox.SetResourceReference(TagProperty, "Paused");
            GameOverLb.Content = textbox.Tag;
            GameOverLb.Margin = new Thickness
            {
                Left = 175,
                Top = 156
            };
            GameOverLb.Height = 54;
            GameOverLb.Width = 174;
            GameOverLb.FontSize = 35;
            GameOverLb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#times");
            lvl2Field.Children.Add(GameOverLb);

            return;
        }

        private void drawGameOver()//
        {
            Label GameOverLb = new Label();
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

            lvl2Field.Children.Add(GameOverLb);

            return;
        }

        private void drawScore()//
        {
            Label ScoreLb = new Label();
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

            lvl2Field.Children.Add(ScoreLb);

            return;
        }

        private void drawPressEnter()//
        {
            Label PressSpaceLb = new Label();
            textbox.SetResourceReference(TagProperty, "Enter_continue");
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
            PressSpaceLb.FontSize = 20;
            PressSpaceLb.BorderBrush = Brushes.White;

            lvl2Field.Children.Add(PressSpaceLb);

            return;
        }

        private void drawPressSpace()//
        {
            Label PressSpaceLb = new Label();
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

            PressSpaceLb.FontSize = 20;
            PressSpaceLb.BorderBrush = Brushes.White;

            lvl2Field.Children.Add(PressSpaceLb);

            return;
        }

        private void drawApple()//
        {
            if (myApple == null)
            {
                myApple = new Image();
                myApple.Source = new BitmapImage(new Uri("Resources/Apple.png", UriKind.Relative));

                myApple.HorizontalAlignment = HorizontalAlignment.Left;
                myApple.VerticalAlignment = VerticalAlignment.Top;

                myApple.Width = myApple.Height = DOT_SIZE;
            }
            lvl2Field.Children.Remove(myApple);

            myApple.Margin = new Thickness
            {
                Left = myLogic.getAppleX,
                Top = myLogic.getAppleY,
            };

            lvl2Field.Children.Add(myApple);
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
                    down = up = left = false;
                    right = true;
                    break;
                case Key.Left:
                case Key.A:
                    down = up = right = false;
                    left = true;
                    break;
                case Key.Up:
                case Key.W:
                    left = right = down = false;
                    up = true;
                    break;
                case Key.Down:
                case Key.S:
                    left = right = up = false;
                    down = true;
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
                        speed = 160;
                        UpdateSpeed();
                        dots = 2;
                        counterScore = 0;

                        for (int i = 0; i < dots; i++)
                        {
                            x[i] = 192 - (i * DOT_SIZE);
                            y[i] = 144;
                        }
                        scoreLabel.Content = score_t + " : " + counterScore;
                        isGameOver = false;
                        BackgroundMusic.Open(new Uri("../../Resources/iwbgame.mp3", UriKind.RelativeOrAbsolute));
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
