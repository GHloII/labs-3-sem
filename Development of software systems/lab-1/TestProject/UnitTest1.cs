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
            Assert.AreEqual("Имя \"COM1\" является зарезервированным в Windows.", reason);
        }

        [TestMethod]
        public void IsInvalidWindowsName_ValidName_ReturnsFalse()
        {
            var result = WindowsNameValidator.IsInvalidWindowsName(@"C:\folder\example.txt", out var reason);

            Assert.IsFalse(result);
            Assert.AreEqual("", reason);
        }
    }
}
