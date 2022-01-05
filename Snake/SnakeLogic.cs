using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class SnakeLogic
    {
        private int appleX;
        private int appleY;

        private Random myLocalRandom = new Random();


        public void createApple(int SIZE, int DOT_SIZE, int[] x, int[] y)  
        {
            LabelX:
            appleX = myLocalRandom.Next(1, (SIZE - DOT_SIZE) / DOT_SIZE) * DOT_SIZE;
            for (int i = 0; i < x.Length; i++)
            {
                if (appleX == x[i])
                    goto LabelX;
            }

            LabelY:
            appleY = myLocalRandom.Next(1, (SIZE - DOT_SIZE) / DOT_SIZE) * DOT_SIZE;
            for (int i = 0; i < y.Length; i++)
            {
                if (appleY == x[i])
                    goto LabelY;
            }
        }

        public int getAppleX => appleX;
        public int getAppleY => appleY;
        public int setAppleX(int x) => appleX = x;
        public int setAppleY(int y) => appleY = y;
    }

    public class applegen4
    {
        private int appleX;
        private int appleY;

        private Random myLocalRandom = new Random();


        public void createApple(int SIZE, int DOT_SIZE, int[] x, int[] y)  
        {
        LabelX:
            appleX = myLocalRandom.Next(1, (SIZE - DOT_SIZE) / DOT_SIZE) * DOT_SIZE;
            for (int i = 0; i < x.Length; i++)
            {
                if (appleX == x[i])
                    goto LabelX;
            }

        LabelY:
            appleY = 432;
            for (int i = 0; i < y.Length; i++)
            {
                if (appleY == x[i])
                    goto LabelY;
            }
        }

        public int getAppleX => appleX;
        public int getAppleY => appleY;
        public int setAppleX(int x) => appleX = x;
        public int setAppleY(int y) => appleY = y;
    }

    public class SpecialSnakeLogic
    {
        public int appleX;
        public int appleY;
        public int zonenum;

        private Random myLocalRandom = new Random();

        public void createApple(int SIZE, int DOT_SIZE, int[] x, int[] y)  
        {
        zonenum = myLocalRandom.Next(1, 6);
            if (zonenum == 1)
            {
            LabelX:
                appleX = myLocalRandom.Next(1, 688/DOT_SIZE)*DOT_SIZE;
                for (int i = 0; i < x.Length; i++)
                {
                    if (appleX == x[i])
                        goto LabelX;
                }
            LabelY:
                appleY = myLocalRandom.Next(1, 144 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < y.Length; i++)
                {
                    if (appleY == x[i])
                        goto LabelY;
                }
            }
            if (zonenum == 2)
            {
            LabelX:
                appleX = myLocalRandom.Next(1, 688 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < x.Length; i++)
                {
                    if (appleX == x[i])
                        goto LabelX;
                }
            LabelY:
                appleY = myLocalRandom.Next(576/DOT_SIZE, 688 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < y.Length; i++)
                {
                    if (appleY == x[i])
                        goto LabelY;
                }
            }
            if (zonenum == 3)
            {
            LabelX:
                appleX = myLocalRandom.Next(1, 128 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < x.Length; i++)
                {
                    if (appleX == x[i])
                        goto LabelX;
                }

            LabelY:
                appleY = myLocalRandom.Next(1, 688 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < y.Length; i++)
                {
                    if (appleY == x[i])
                        goto LabelY;
                }
            }
            if (zonenum == 4)
            {
            LabelX:
                appleX = myLocalRandom.Next(560/DOT_SIZE, 688 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < x.Length; i++)
                {
                    if (appleX == x[i])
                        goto LabelX;
                }

            LabelY:
                appleY = myLocalRandom.Next(1, 688 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < y.Length; i++)
                {
                    if (appleY == x[i])
                        goto LabelY;
                }
            }
            if (zonenum == 5)
            {
            LabelX:
                appleX = myLocalRandom.Next(144/DOT_SIZE, 544 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < x.Length; i++)
                {
                    if (appleX == x[i])
                        goto LabelX;
                }

            LabelY:
                appleY = myLocalRandom.Next(160/DOT_SIZE, 528 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < y.Length; i++)
                {
                    if (appleY == x[i])
                        goto LabelY;
                }
            }
            if (zonenum == 6)
            {
            LabelX:
                appleX = myLocalRandom.Next(128/DOT_SIZE, 144 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < x.Length; i++)
                {
                    if (appleX == x[i])
                        goto LabelX;
                }

            LabelY:
                appleY = myLocalRandom.Next(304/DOT_SIZE, 368 / DOT_SIZE) * DOT_SIZE;
                for (int i = 0; i < y.Length; i++)
                {
                    if (appleY == x[i])
                        goto LabelY;
                }
            }
        }
        public int getAppleX => appleX;
        public int getAppleY => appleY;
        public int setAppleX(int x) => appleX = x;
        public int setAppleY(int y) => appleY = y;
    }

    public class borders
    {
        public Random BordersRandom = new Random();
        public int borderssize1, borderssize2, borderssize3, borderssize4, borderssize5, borderssize6;
        public void generateBorders()
        {
            borderssize1 = BordersRandom.Next(0, 64);
            borderssize2 = BordersRandom.Next(80, 160);
            borderssize3 = BordersRandom.Next(160, 224);
            borderssize4 = BordersRandom.Next(240, 352);
            borderssize5 = BordersRandom.Next(352, 416);
            borderssize6 = BordersRandom.Next(448, 512);
        }

    }
}
