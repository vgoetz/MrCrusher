using MrCrusher.Framework.Core;
using NUnit.Framework;

namespace MrCrusher.Test
{
    [TestFixture]
    public class TestClassDegreesClass {

        [Test]
        public void DegreePositive() {
            var degree1 = new Degree(0);
            Assert.AreEqual((decimal) 0, degree1.Value, "degree1 value after constructor");

            var degree2 = new Degree(10);
            Assert.AreEqual((decimal) 10, degree2.Value, "degree2 value after constructor");

            var degree3 = new Degree(90);
            Assert.AreEqual((decimal) 90, degree3.Value, "degree3 value after constructor");

            var degree4 = new Degree(270);
            Assert.AreEqual((decimal) 270, degree4.Value, "degree4 value after constructor");
        }

        [Test]
        public void DegreePositiveModulo() {
            var degree5 = new Degree(360);
            Assert.AreEqual((decimal) 0, degree5.Value, "degree5 value after constructor");

            var degree6 = new Degree(361); // = 1
            Assert.AreEqual((decimal) 1, degree6.Value, "degree6 value after constructor");

            var degree7 = new Degree(1000); // = 280
            Assert.AreEqual((decimal) 280, degree7.Value, "degree7 value after constructor");
        }

        [Test]
        public void DegreeNegative() {
            var degree1 = new Degree(-0);
            Assert.AreEqual((decimal) 0, degree1.Value, "degree1 value after constructor");

            var degree2 = new Degree(-10);
            Assert.AreEqual((decimal) 350, degree2.Value, "degree2 value after constructor");

            var degree3 = new Degree(-90);
            Assert.AreEqual((decimal) 270, degree3.Value, "degree3 value after constructor");

            var degree4 = new Degree(-270);
            Assert.AreEqual((decimal) 90, degree4.Value, "degree4 value after constructor");
        }

        [Test]
        public void DegreeNegativeModulo() {
            var degree5 = new Degree(-360);
            Assert.AreEqual((decimal) 0, degree5.Value, "degree5 value after constructor");

            var degree6 = new Degree(-361); // = 359
            Assert.AreEqual((decimal) 359, degree6.Value, "degree6 value after constructor");

            var degree7 = new Degree(-1000); // = 80
            Assert.AreEqual((decimal) 80, degree7.Value, "degree7 value after constructor");

            var degree8 = new Degree(-1); // = 359
            Assert.AreEqual((decimal) 359, degree8.Value, "degree8 value after constructor");
        }

        [Test]
        public void DegreeEqualMethode_TwoDegreeObjects1() {
            var degreeA = new Degree(0);
            var degreeB = new Degree(0);

            Assert.That(degreeA.Equals(degreeB), "degreeA.Equals(degreeB)");
        }

        [Test]
        public void DegreeEqualMethode_TwoDegreeObjects2() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(1);

            Assert.That(degreeA.Equals(degreeB), "degreeA.Equals(degreeB)");
        }

        [Test]
        public void DegreeEqualMethode_OneDegreeObjectAndOneDouble1() {
            var degreeA = new Degree(0);
            const double degreeB = 0;

            Assert.That(degreeA.Equals(degreeB), "degreeA.Equals(degreeB)");
        }

        [Test]
        public void DegreeEqualMethode_OneDegreeObjectAndOneDouble2() {
            var degreeA = new Degree(1);
            const double degreeB = 1;

            Assert.That(degreeA.Equals(degreeB), "degreeA.Equals(degreeB)");
        }

        [Test]
        public void DegreeEqualOperator_DegreeObject1() {
            var degreeA = new Degree(0);
            var degreeB = new Degree(0);

            Assert.That(degreeA == degreeB, "degreeA == degreeB");
        }

        [Test]
        public void DegreeEqualOperator_DegreeObject2() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(1);

            Assert.That(degreeA == degreeB, "degreeA == degreeB");
        }

        [Test]
        public void DegreeEqualOperator_DegreeObjectAndDouble1() {
            var degreeA = new Degree(0);
            const double degreeB = 0;

            Assert.That(degreeA == degreeB, "degreeA == degreeB");
        }

        [Test]
        public void DegreeEqualOperator_DegreeObjectAndDouble2() {
            var degreeA = new Degree(1);
            const double degreeB = 1;

            Assert.That(degreeA == degreeB, "degreeA == degreeB");
        }

        [Test]
        public void DegreeAddition_TwoDegreeObjects1() {
            var degreeA = new Degree(0);
            var degreeB = new Degree(0);

            Degree result = degreeA + degreeB;

            Assert.AreEqual(new Degree(0), result, "new Degree(0), result");
        }

        [Test]
        public void DegreeAddition_TwoDegreeObjects2() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(1);

            Degree result = degreeA + degreeB;

            Assert.AreEqual(new Degree(2), result, "new Degree(2), result");
        }

        [Test]
        public void DegreeAddition_TwoDegreeObjects3() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(1);

            Degree result = degreeA + degreeB;

            Assert.That(new Degree(2).Equals(result), "new Degree(2).Equals(result)");
        }

        [Test]
        public void DegreeAddition_TwoDegreeObjects4() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(1);

            Degree result = degreeA + degreeB;

            Assert.That(new Degree(2).Equals(result), "new Degree(2).Equals(result)");
        }

        [Test]
        public void DegreeAddition_TwoDegreeObjects5() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(1);

            Degree result = degreeA + degreeB;

            Assert.That(new Degree(2) == result, "new Degree(2) == result");
        }

        [Test]
        public void DegreeAddition_TwoDegreeObjects6() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(1);

            Degree result = degreeA + degreeB;

            Assert.That(2 == result, "2 == result");
        }

        [Test]
        public void DegreeAddition_OneDegreeObjectAndDouble1() {
            var degreeA = new Degree(1);
            const double degreeB = 1;

            Degree result = degreeA + degreeB;

            Assert.That(2 == result, "2 == result");
        }

        [Test]
        public void DegreeAddition_OneDegreeObjectAndDouble2() {
            var degreeA = new Degree(1);
            const double degreeB = 1;

            Degree result = degreeB + degreeA;

            Assert.That(2 == result, "2 == result");
        }

        [Test]
        public void DegreeAddition_OneDegreeObjectAndDouble3() {
            var degreeA = new Degree(50);
            const double degreeB = 50;

            Degree result = degreeB + degreeA;

            Assert.That(100 == result, "100 == result");
        }

        [Test]
        public void DegreeAddition_OneDegreeObjectAndDouble4() {
            var degreeA = new Degree(360);
            const double degreeB = 0;

            Degree result = degreeB + degreeA;

            Assert.That(result.Equals(360), "result.Equals(360)");
        }

        [Test]
        public void DegreeAddition_OneDegreeObjectAndDouble5() {
            var degreeA = new Degree(360);
            const double degreeB = 1;

            Degree result = degreeB + degreeA;

            Assert.That(result.Equals(1), "result.Equals(1)");
        }

        [Test]
        public void DegreeAddition_OneDegreeObjectAndDouble6() {
            var degreeA = new Degree(360);
            const double degreeB = 360;

            Degree result = degreeB + degreeA;

            Assert.That(result.Equals(0), "result.Equals(0)");
        }

        [Test]
        public void DegreeAddition_OneDegreeObjectAndDouble7() {
            var degreeA = new Degree(360);
            const double degreeB = 720;

            Degree result = degreeB + degreeA;

            Assert.That(result.Equals(0), "result.Equals(0)");
        }

        [Test]
        public void DegreeSubstraction_TwoDegreeObjects1() {
            var degreeA = new Degree(0);
            var degreeB = new Degree(0);

            Degree result = degreeA - degreeB;

            Assert.That(result.Equals(0), "result.Equals(0)");
        }

        [Test]
        public void DegreeSubstraction_TwoDegreeObjects2() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(0);

            Degree result = degreeA - degreeB;

            Assert.That(result.Equals(1), "result.Equals(1)");
        }

        [Test]
        public void DegreeSubstraction_TwoDegreeObjects3() {
            var degreeA = new Degree(1);
            var degreeB = new Degree(1);

            Degree result = degreeA - degreeB;

            Assert.That(result.Equals(0), "result.Equals(0)");
        }

        [Test]
        public void DegreeSubstraction_TwoDegreeObjects4() {
            var degreeA = new Degree(0);
            var degreeB = new Degree(1);

            Degree result = degreeA - degreeB;

            Assert.AreEqual(new Degree(359), result, "result");
        }

        [Test]
        public void DegreeSubstraction_TwoDegreeObjects5() {
            var degreeA = new Degree(5);
            var degreeB = new Degree(10);

            Degree result = degreeA - degreeB;

            Assert.That(result.Equals(355), "result.Equals(355)");
        }

        [Test]
        public void DegreeSubstraction_OneDegreeObjectAndDouble1() {
            var degreeA = new Degree(0);
            const double degreeB = 0;

            Degree result = degreeA - degreeB;

            Assert.That(result.Equals(0), "result.Equals(0)");
        }

        [Test]
        public void DegreeSubstraction_OneDegreeObjectAndDouble2() {
            var degreeA = new Degree(1);
            const double degreeB = 0;

            Degree result = degreeA - degreeB;

            Assert.That(result.Equals(1), "result.Equals(1)");
        }

        [Test]
        public void DegreeSubstraction_OneDegreeObjectAndDouble3() {
            var degreeA = new Degree(1);
            const double degreeB = 1;

            Degree result = degreeA - degreeB;

            Assert.That(result.Equals(0), "result.Equals(0)");
        }

        [Test]
        public void DegreeSubstraction_OneDegreeObjectAndDouble4() {
            var degreeA = new Degree(0);
            const double degreeB = 1;

            Degree result = degreeA - degreeB;

            Assert.That(result.Equals(359), "result.Equals(359)");
        }

        [Test]
        public void DegreeSubstraction_OneDegreeObjectAndDouble5() {
            var degreeA = new Degree(10);
            const double degreeB = 1000;

            Degree result = degreeA - degreeB;

            Assert.AreEqual(new Degree(90), result, "result");
        }
    }
}
