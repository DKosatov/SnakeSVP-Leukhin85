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
    public partial class lvl3 : Window
    {
        public lvl3()
        {
            InitializeComponent();
            initGame();
        }

        private const int SIZE = 700;   
        private const int DOT_SIZE = 16;    

        private const int ALL_DOTS = SIZE / DOT_SIZE;

        private bool right = true;
        private bool left = false;
        private bool up = false;
        private bool down = false;

        private bool isGameOver = false;

        private int dots = 2; 
        private int counterScore = 0;   
        private int[] x = new int[ALL_DOTS];
        private int[] y = new int[ALL_DOTS];

        private DispatcherTimer timer; 

        private const int startSpeed = 160;
        private const int speedSubstractor = 5;

        private int speed = startSpeed;    

        SoundPlayer GameOverSound = new SoundPlayer("../../Resources/gameover.wav");
        SoundPlayer AppleEaten = new SoundPlayer("../../Resources/AppleEaten.wav");
        SoundPlayer ButtonClick = new SoundPlayer("../../Resources/knopka.wav");
        MediaPlayer BackgroundMusic = new MediaPlayer();

        internal static string language = App.language;

        string score_t, speed_t, status_t;

        private Image myApple;
        private Image SnakeHead;

        public Image field;
        private Rectangle snakeDot;
        private List<Rectangle> allSnake;

        SpecialSnakeLogic myLogic;


        private void MainGameLoop(object sender, EventArgs e)//
        {
            move();
            checkCollisions();

            if (!isGameOver)
            {
                drawApple();
                drawSnake();
                checkApple();
                textbox.SetResourceReference(TagProperty, "In_Game");
                gameStatusLabel.Content = status_t + textbox.Tag;
                lvl3Field.Children.Remove(PressSpaceLb);
                lvl3Field.Children.Remove(GameOverLb);
                lvl3Field.Children.Remove(ScoreLb);
                lvl3Field.Children.Remove(PressEnterLb);
                lvl3Field.Children.Remove(GamePausedLb);
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
                for (int i = 1; i < dots; i++)
                {
                    lvl3Field.Children.Remove(allSnake[i]);
                }
            }

            Thread.Sleep(speed);
        }

        private void initGame()//
        {
            myLogic = new SpecialSnakeLogic();  
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
                x[i] = 304 - (i * DOT_SIZE);
                y[i] = 352;
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
            if (x[0] < 0)
            {
                x[0] = (SIZE - DOT_SIZE);
            }
            if (y[0] < 0)
            {
                y[0] = (SIZE - DOT_SIZE);
            }
            if (y[0] >= (SIZE))
            {
                y[0] = 0;
            }

            if ((x[0] >= (129) && x[0] <= (560-16) && y[0] >= 144 && y[0] <= 160-16)||((x[0] >= (129) && x[0] <= (560) && y[0] >= 544 && y[0] <= 560-16))) 
            {
                isGameOver = true;
                return;
            }

            if ((x[0] >= (546-16) && x[0] <= (560-16) && y[0] >= 144 && y[0] <= 560-16))
            {
                isGameOver = true;
                return;
            }

            if ((x[0] >= (129-16) && x[0] <= (144-16) && y[0] >= 144 && y[0] <= 315))
            {
                isGameOver = true;
                return;
            }

            if ((x[0] >= (129-16) && x[0] <= (144-16) && y[0] >= 384 && y[0] <= 560))
            {
                isGameOver = true;
                return;
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

        private void drawSnake()
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

            lvl3Field.Children.Remove(SnakeHead);

            SnakeHead.Margin = new Thickness
            {
                Left = x[0],
                Top = y[0],
            };

            lvl3Field.Children.Add(SnakeHead);

            for (int i = 1; i < dots; i++) 
            {
                lvl3Field.Children.Remove(allSnake[i]);

                allSnake[i].Margin = new Thickness
                {
                    Left = x[i],
                    Top = y[i],
                };

                lvl3Field.Children.Add(allSnake[i]);
            }
            
        }

        Label GamePausedLb = new Label();
        private void drawGamePaused()
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
            lvl3Field.Children.Remove(GamePausedLb);
            lvl3Field.Children.Add(GamePausedLb);

            return;
        }

        Label GameOverLb = new Label();
        private void drawGameOver()
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

            lvl3Field.Children.Remove(GameOverLb);
            lvl3Field.Children.Add(GameOverLb);

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

            lvl3Field.Children.Remove(ScoreLb);
            lvl3Field.Children.Add(ScoreLb);
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
            lvl3Field.Children.Remove(PressEnterLb);
            lvl3Field.Children.Add(PressEnterLb);

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

            lvl3Field.Children.Remove(PressSpaceLb);
            lvl3Field.Children.Add(PressSpaceLb);

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
            lvl3Field.Children.Remove(myApple);

            // apple
            myApple.Margin = new Thickness
            {
                Left = myLogic.getAppleX,
                Top = myLogic.getAppleY,
            };

            lvl3Field.Children.Add(myApple);
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
                            x[i] = 304 - (i * DOT_SIZE);
                            y[i] = 352;
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
