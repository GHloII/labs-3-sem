using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;



namespace lab_1.Tests
{
    [TestClass]
    public class WindowsNameValidatorTests
    {
        [TestMethod]
        public void IsInvalidWindowsName_EmptyString_ReturnsTrue()
        {
            var result = WindowsNameValidator.IsInvalidWindowsName("   ", out var reason);

            Assert.IsTrue(result);
            Assert.AreEqual("Путь пустой или состоит из пробелов.", reason);
        }


        public void IsInvalidWindowsName_InvalidChars_ReturnsTrue()
        {
            var result = WindowsNameValidator.IsInvalidWindowsName("some<>file.txt", out var reason);

            Assert.IsTrue(result);
            Assert.AreEqual("Имя файла содержит недопустимые символы.", reason);
        }

        [TestMethod]
        public void IsInvalidWindowsName_NoFileName_ReturnsTrue()
        {
            var result = WindowsNameValidator.IsInvalidWindowsName(@"C:\folder\", out var reason);

            Assert.IsTrue(result);
            Assert.AreEqual("Имя файла отсутствует в пути.", reason);
        }

        [TestMethod]
        public void IsInvalidWindowsName_ReservedName_ReturnsTrue()
        {
            var result = WindowsNameValidator.IsInvalidWindowsName("CON.txt", out var reason);

            Assert.IsTrue(result);
            Assert.AreEqual("Имя \"CON\" является зарезервированным в Windows.", reason);
        }

        [TestMethod]
        public void IsInvalidWindowsName_ValidName_ReturnsFalse()
        {
            var result = WindowsNameValidator.IsInvalidWindowsName(@"C:\folder\example.txt", out var reason);

            Assert.IsFalse(result);
            Assert.AreEqual("", reason);
        }
    }



    [TestClass]
   
    public class MathGeometrySolverTests
    {
        [TestMethod]
        public void Distance_TwoPoints_ReturnsCorrectDistance()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(3, 4);
            var dist = MathGeometrySolver.Distance(p1, p2);
            Assert.IsTrue(MathGeometrySolver.IsEqual(dist, 5));
        }

        [TestMethod]
        public void IsEqual_CloseValues_ReturnsTrue()
        {
            Assert.IsTrue(MathGeometrySolver.IsEqual(1.0000001, 1.0000002));
        }

        [TestMethod]
        public void IsEqual_DifferentValues_ReturnsFalse()
        {
            Assert.IsFalse(MathGeometrySolver.IsEqual(1.0001, 1.002));
        }

        [TestMethod]
        public void IsAngle90_RightAngle_ReturnsTrue()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(0, 1);
            var p3 = new Point(1, 1);
            Assert.IsTrue(MathGeometrySolver.IsAngle90(p1, p2, p3));
        }

        [TestMethod]
        public void IsAngle90_NotRightAngle_ReturnsFalse()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 1);
            var p3 = new Point(2, 2);
            Assert.IsFalse(MathGeometrySolver.IsAngle90(p1, p2, p3));
        }

        [TestMethod]
        public void HasOverlap_ColinearAndOverlapping_ReturnsTrue()
        {
            var s1 = new Segment(new Point(0, 0), new Point(2, 0));
            var s2 = new Segment(new Point(1, 0), new Point(3, 0));
            Assert.IsTrue(MathGeometrySolver.HasOverlap(s1, s2));
        }

        [TestMethod]
        public void HasOverlap_ColinearButNotOverlapping_ReturnsFalse()
        {
            var s1 = new Segment(new Point(0, 0), new Point(1, 0));
            var s2 = new Segment(new Point(2, 0), new Point(3, 0));
            Assert.IsFalse(MathGeometrySolver.HasOverlap(s1, s2));
        }

        [TestMethod]
        public void HasOverlap_NotColinear_ReturnsFalse()
        {
            var s1 = new Segment(new Point(0, 0), new Point(1, 1));
            var s2 = new Segment(new Point(0, 1), new Point(1, 2));
            Assert.IsFalse(MathGeometrySolver.HasOverlap(s1, s2));
        }

        [TestMethod]
        public void FindIntersection_IntersectingSegments_ReturnsIntersectionPoint()
        {
            var s1 = new Segment(new Point(0, 0), new Point(2, 2));
            var s2 = new Segment(new Point(0, 2), new Point(2, 0));
            bool isOverlap = false;

            var intersection = MathGeometrySolver.FindIntersection(s1, s2, ref isOverlap);

            Assert.IsNotNull(intersection);
            Assert.IsTrue(MathGeometrySolver.IsEqual(intersection.x, 1));
            Assert.IsTrue(MathGeometrySolver.IsEqual(intersection.y, 1));
            Assert.IsFalse(isOverlap);
        }

        [TestMethod]
        public void FindIntersection_OverlappingSegments_ReturnsNullAndOverlapTrue()
        {
            var s1 = new Segment(new Point(0, 0), new Point(2, 0));
            var s2 = new Segment(new Point(1, 0), new Point(3, 0));
            bool isOverlap = false;

            var result = MathGeometrySolver.FindIntersection(s1, s2, ref isOverlap);

            Assert.IsNull(result);
            Assert.IsTrue(isOverlap);
        }

        [TestMethod]
        public void FindIntersection_ParallelNonOverlapping_ReturnsNull()
        {
            var s1 = new Segment(new Point(0, 0), new Point(1, 0));
            var s2 = new Segment(new Point(0, 1), new Point(1, 1));
            bool isOverlap = false;

            var result = MathGeometrySolver.FindIntersection(s1, s2, ref isOverlap);

            Assert.IsNull(result);
            Assert.IsFalse(isOverlap);
        }

        [TestMethod]
        public void FindIntersection_NotIntersecting_ReturnsNull()
        {
            var s1 = new Segment(new Point(0, 0), new Point(1, 1));
            var s2 = new Segment(new Point(2, 2), new Point(3, 3));
            bool isOverlap = false;

            var result = MathGeometrySolver.FindIntersection(s1, s2, ref isOverlap);

            Assert.IsNull(result);
            Assert.IsFalse(isOverlap);
        }
    }





}
