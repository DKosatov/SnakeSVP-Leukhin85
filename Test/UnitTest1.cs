using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snake;
namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MyAppleTest()

        {
            SnakeLogic appleLogic = new SnakeLogic();

            int fieldSize = Snake.lvl1.SIZE;

            int dotSize = Snake.lvl1.DOT_SIZE;
            const int ALL_DOTS = Snake.lvl1.SIZE / Snake.lvl1.DOT_SIZE;
            int[] x = new int[ALL_DOTS];
            int[] y = new int[ALL_DOTS];
            appleLogic.createApple(fieldSize, dotSize,x,y);

            int currentX = appleLogic.getAppleX;

            int currentY = appleLogic.getAppleY;

            bool isXTrue = false;

            bool isYTrue = false;

            if (currentX >= dotSize && currentX <= (fieldSize - dotSize))
            {
                isXTrue = true;
            }

            if (currentY >= dotSize && currentY <= (fieldSize - dotSize))
            {
                isYTrue = true;
            }

            if (!(isXTrue && isYTrue))
            {
                // Îøèáêà
                Assert.Fail("Error has been detected! Apple coordinates: X = " + currentX + " Y" + currentY);
}

        }
    }
}
