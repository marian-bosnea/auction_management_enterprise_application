namespace ServiceLayer.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="StringUtils"/> class.
    /// </summary>
    [TestClass]
    public class StringUtilsTest
    {
        /// <summary>
        /// Tests that the <see cref="StringUtils.CalculateLevenshteinDistance"/> method returns 0
        /// when both input strings are empty.
        /// </summary>
        [TestMethod]
        public void CalculateLevenshteinDistance_BothStringsEmpty_ReturnsZero()
        {
            // Arrange
            string source = string.Empty;
            string target = string.Empty;

            // Act
            int result = StringUtils.CalculateLevenshteinDistance(source, target);

            // Assert
            Assert.AreEqual(0, result, "Distance between two empty strings should be 0.");
        }

        /// <summary>
        /// Tests that the <see cref="StringUtils.CalculateLevenshteinDistance"/> method returns the length of the target string
        /// when the source string is empty and the target string is not empty.
        /// </summary>
        [TestMethod]
        public void CalculateLevenshteinDistance_SourceEmpty_TargetNotEmpty_ReturnsTargetLength()
        {
            // Arrange
            string source = string.Empty;
            string target = "test";

            // Act
            int result = StringUtils.CalculateLevenshteinDistance(source, target);

            // Assert
            Assert.AreEqual(target.Length, result, $"Distance should be equal to the length of the target string ({target.Length}).");
        }

        /// <summary>
        /// Tests that the <see cref="StringUtils.CalculateLevenshteinDistance"/> method returns the length of the source string
        /// when the target string is empty and the source string is not empty.
        /// </summary>
        [TestMethod]
        public void CalculateLevenshteinDistance_SourceNotEmpty_TargetEmpty_ReturnsSourceLength()
        {
            // Arrange
            string source = "test";
            string target = string.Empty;

            // Act
            int result = StringUtils.CalculateLevenshteinDistance(source, target);

            // Assert
            Assert.AreEqual(source.Length, result, $"Distance should be equal to the length of the source string ({source.Length}).");
        }

        /// <summary>
        /// Tests that the <see cref="StringUtils.CalculateLevenshteinDistance"/> method returns 0 when both input strings are identical.
        /// </summary>
        [TestMethod]
        public void CalculateLevenshteinDistance_SameStrings_ReturnsZero()
        {
            // Arrange
            string source = "test";
            string target = "test";

            // Act
            int result = StringUtils.CalculateLevenshteinDistance(source, target);

            // Assert
            Assert.AreEqual(0, result, "Distance between identical strings should be 0.");
        }

        /// <summary>
        /// Tests that the <see cref="StringUtils.CalculateLevenshteinDistance"/> method returns the correct distance
        /// between two different strings.
        /// </summary>
        [TestMethod]
        public void CalculateLevenshteinDistance_DifferentStrings_ReturnsCorrectDistance()
        {
            // Arrange
            string source = "kitten";
            string target = "sitting";

            // Act
            int result = StringUtils.CalculateLevenshteinDistance(source, target);

            // Assert
            Assert.AreEqual(3, result, "Distance between 'kitten' and 'sitting' should be 3.");
        }

        /// <summary>
        /// Tests that the <see cref="StringUtils.CalculateLevenshteinDistance"/> method returns 1 when there is only one character difference
        /// between two strings.
        /// </summary>
        [TestMethod]
        public void CalculateLevenshteinDistance_OneCharacterDifference_ReturnsOne()
        {
            // Arrange
            string source = "test";
            string target = "text";

            // Act
            int result = StringUtils.CalculateLevenshteinDistance(source, target);

            // Assert
            Assert.AreEqual(1, result, "Distance between 'test' and 'text' should be 1.");
        }

        /// <summary>
        /// Tests that the <see cref="StringUtils.CalculateLevenshteinDistance"/> method returns 1 for single character strings
        /// that are different.
        /// </summary>
        [TestMethod]
        public void CalculateLevenshteinDistance_SingleCharacterStrings_ReturnsCorrectDistance()
        {
            // Arrange
            string source = "a";
            string target = "b";

            // Act
            int result = StringUtils.CalculateLevenshteinDistance(source, target);

            // Assert
            Assert.AreEqual(1, result, "Distance between 'a' and 'b' should be 1.");
        }

        /// <summary>
        /// Tests that the <see cref="StringUtils.CalculateLevenshteinDistance"/> method returns the correct distance
        /// between two long strings.
        /// </summary>
        [TestMethod]
        public void CalculateLevenshteinDistance_LongStrings_ReturnsCorrectDistance()
        {
            // Arrange
            string source = "kitten";
            string target = "kittens";

            // Act
            int result = StringUtils.CalculateLevenshteinDistance(source, target);

            // Assert
            Assert.AreEqual(1, result, "Distance between 'kitten' and 'kittens' should be 1.");
        }
    }
}