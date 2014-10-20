using MrCrusher.Framework.Core;
using NUnit.Framework;

namespace MrCrusher.Test {

    [TestFixture]
    public class TestClassCalculateOrientation_SmallestAngleBetweenPointAandB {

        //[Test]
        //public void WithVectors() {

        //    Vector v2 = new Vector(new Point(1, 0)).Normalized();
        //    Vector v1 = new Vector(new Point(-1, 1)).Normalized();

        //    Console.WriteLine("v1: {0}", v1);
        //    Console.WriteLine("v2: {0}", v2);

        //    var dotProduct = v1.DotProduct(v2);

        //    Console.WriteLine("dotProduct: {0}", dotProduct);

        //    double degree = Math.Round(Math.Acos(dotProduct)* 180 / Math.PI , MidpointRounding.ToEven);

        //    Assert.AreEqual(135, degree);
        //}
        
        [TestCase(0, 10  , Result = 10)]
        [TestCase(0, 90  , Result = 90)]
        [TestCase(0, 100 , Result = 100)]
        [TestCase(0, 179 , Result = 179)]
        [TestCase(0, 180 , Result = -180)]
        [TestCase(0, 181 , Result = -179)]
        [TestCase(0, 200 , Result = -160)]
        [TestCase(0, 270 , Result = -90)]
        [TestCase(0, 350 , Result = -10)]
        [TestCase(0, 359 , Result = -1)]
        [TestCase(0, 360 , Result = 0)]
        public double From0_to_TargetX_(int current, int target) {

            return Calculator.CalculateDegreeDifferenceBetweenToDegrees(current, target);
        }

        [TestCase(250, 10,  Result = 120)]
        [TestCase(250, 90,  Result = -160)]
        [TestCase(250, 100, Result = -150)]
        [TestCase(250, 179, Result = -71)]
        [TestCase(250, 180, Result = -70)]
        [TestCase(250, 181, Result = -69)]
        [TestCase(250, 200, Result = -50)]
        [TestCase(250, 270, Result = 20)]
        [TestCase(250, 350, Result = 100)]
        [TestCase(250, 359, Result = 109)]
        [TestCase(250, 360, Result = 110)]
        public double From250_to_TargetX_(int current, int target) {

            return Calculator.CalculateDegreeDifferenceBetweenToDegrees(current, target);
        }

        [TestCase(10, 0, Result = -10)]
        [TestCase(90, 0, Result = -90)]
        [TestCase(100,0, Result = -100)]
        [TestCase(179,0, Result = -179)]
        [TestCase(180,0, Result = -180)]
        [TestCase(181,0, Result = +179)]
        [TestCase(200,0, Result = +160)]
        [TestCase(270,0, Result = +90)]
        [TestCase(350,0, Result = +10)]
        [TestCase(359,0, Result = +1)]
        [TestCase(360,0, Result = 0)]
        public double FromTargetX_to_0(int current, int target) {

            return Calculator.CalculateDegreeDifferenceBetweenToDegrees(current, target);
        }

        [TestCase(10,  5,   Result = -5)]
        [TestCase(90,  50,  Result = -40)]
        [TestCase(100, 90,  Result = -10)]
        [TestCase(179, 110, Result = -69)]
        [TestCase(180, 180, Result = 0)]
        [TestCase(181, 200, Result = +19)]
        [TestCase(200, 210, Result = +10)]
        [TestCase(270, 250, Result = -20)]
        [TestCase(350, 299, Result = -51)]
        [TestCase(359, 350, Result = -9)]
        [TestCase(359, 360, Result = +1)]
        [TestCase(360, 0,   Result = 0)]
        [TestCase(360, 15,  Result = 15)]
        public double FromTargetX_to_TargetY(int current, int target) {

            return Calculator.CalculateDegreeDifferenceBetweenToDegrees(current, target);
        }
    }
}
