using NUnit.Framework;
using BPCalculator;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BPCA1UnitTest
{
    public class BloodPressureTests
    {
        private BloodPressure _bloodPressure = null!;

        [SetUp]
        public void Setup()
        {
            _bloodPressure = new BloodPressure();
        }

        // Test cases to cover BP categories
        [TestCase(85, 55, BPCategory.Low, TestName = "BP Category - Low")]
        [TestCase(115, 75, BPCategory.Ideal, TestName = "BP Category - Ideal")]
        [TestCase(130, 85, BPCategory.PreHigh, TestName = "BP Category - Pre-High")]
        [TestCase(145, 95, BPCategory.High, TestName = "BP Category - High")]
        public void BPCategory_ShouldReturnCorrectCategory(int systolic, int diastolic, BPCategory expectedCategory)
        {
            // Arrange
            _bloodPressure.Systolic = systolic;
            _bloodPressure.Diastolic = diastolic;

            // Act
            var category = _bloodPressure.Category;

            // Assert
            Assert.That(category, Is.EqualTo(expectedCategory));
        }

        // Test for out-of-range systolic values
        [Test]
        public void Systolic_OutOfRange_ShouldThrowValidationError()
        {
            // Arrange
            _bloodPressure.Systolic = 200; // Invalid value
            var context = new ValidationContext(_bloodPressure) { MemberName = nameof(_bloodPressure.Systolic) };
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(_bloodPressure.Systolic, context, validationResults);

            // Assert
            Assert.IsFalse(isValid);
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("Invalid Systolic Value"));
        }

        // Test for out-of-range diastolic values
        [Test]
        public void Diastolic_OutOfRange_ShouldThrowValidationError()
        {
            // Arrange
            _bloodPressure.Diastolic = 110; // Invalid value
            var context = new ValidationContext(_bloodPressure) { MemberName = nameof(_bloodPressure.Diastolic) };
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(_bloodPressure.Diastolic, context, validationResults);

            // Assert
            Assert.IsFalse(isValid);
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("Invalid Diastolic Value"));
        }

        // Test for valid systolic and diastolic range
        [TestCase(70, 40, true, TestName = "Valid Range - Min Values")]
        [TestCase(190, 100, true, TestName = "Valid Range - Max Values")]
        [TestCase(100, 80, true, TestName = "Valid Range - Normal Values")]
        public void ValidRange_ShouldPassValidation(int systolic, int diastolic, bool expectedResult)
        {
            // Arrange
            _bloodPressure.Systolic = systolic;
            _bloodPressure.Diastolic = diastolic;
            var context = new ValidationContext(_bloodPressure);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(_bloodPressure, context, validationResults, true);

            // Assert
            Assert.That(isValid, Is.EqualTo(expectedResult));
        }

        // Test for edge cases in systolic and diastolic values
        [TestCase(69, 50, "Invalid Systolic Value", TestName = "Systolic Below Min")]
        [TestCase(191, 80, "Invalid Systolic Value", TestName = "Systolic Above Max")]
        [TestCase(120, 39, "Invalid Diastolic Value", TestName = "Diastolic Below Min")]
        [TestCase(120, 101, "Invalid Diastolic Value", TestName = "Diastolic Above Max")]
        public void OutOfRange_ShouldThrowValidationError(int systolic, int diastolic, string expectedErrorMessage)
        {
            // Arrange
            _bloodPressure.Systolic = systolic;
            _bloodPressure.Diastolic = diastolic;
            var context = new ValidationContext(_bloodPressure);
            var validationResults = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(_bloodPressure, context, validationResults, true);

            // Assert
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo(expectedErrorMessage));
        }

        // Test category behavior with valid range combinations
        [TestCase(120, 81, BPCategory.PreHigh, TestName = "BP Category - PreHigh borderline")]
        [TestCase(121, 70, BPCategory.PreHigh, TestName = "BP Category - Systolic above ideal")]
        public void BPCategory_ShouldHandleBorderlineCases(int systolic, int diastolic, BPCategory expectedCategory)
        {
            // Arrange
            _bloodPressure.Systolic = systolic;
            _bloodPressure.Diastolic = diastolic;

            // Act
            var category = _bloodPressure.Category;

            // Assert
            Assert.That(category, Is.EqualTo(expectedCategory));
        }
    }
}
