﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.CompilerServices;


namespace PROGPOE6212.UnitTests
{
    [TestClass]
    public class MainWindowIntegrationTests
    {
        private string connectionString = "your_test_db_connection_string";

        // Set up the test data before each test
        [TestInitialize]
        public void Setup()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Add initial data to the test database
                var command = new SqlCommand("INSERT INTO Lecturer (LecturerID, LName, LSName, HoursWorked, HourRate, Notes) VALUES (1, 'John', 'Doe', 40, 100, 'Some notes')", connection);
                command.ExecuteNonQuery();
            }
        }

        // Clean up test data after each test
        [TestCleanup]
        public void Cleanup()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Clean up the test data
                var command = new SqlCommand("DELETE FROM Lecturer", connection);
                command.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void SubmitBtn1_SuccessfullySavesDataToDatabase()
        {
            // Arrange
            var mainWindow = new MainWindow();

            // Act
            mainWindow.submitBtn1(); // Assuming this method uses the real connection string internally

            // Assert
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(*) FROM Lecturer WHERE LName = 'John'", connection);
                int count = (int)command.ExecuteScalar();

                Assert.AreEqual(1, count); // Check if data was inserted
            }
        }

        [TestMethod]
        public void UploadBtn_ValidFile_SuccessfulUpload()
        {
            // Arrange
            var mainWindow = new MainWindow();
            string filePath = "testFile.pdf";
            mainWindow.LecturerID = 1;

            // Act
            mainWindow.UploadFileToDatabase(filePath);

            // Assert
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT FileName FROM Files WHERE LecturerID = 1", connection);
                var result = command.ExecuteScalar()?.ToString();

                Assert.AreEqual("testFile.pdf", result);
            }
        }

        [TestMethod]
        public void TrackMethod_ReturnsExpectedData()
        {
            // Arrange
            var mainWindow = new MainWindow();
            int lecturerId = 1;

            // Act
            DataTable result = mainWindow.track(lecturerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Rows.Count);
            Assert.AreEqual(1, result.Rows[0]["TrackId"]);
            Assert.AreEqual("Pending", result.Rows[0]["TStatus"]);
            Assert.AreEqual("Claim123", result.Rows[0]["ClaimNo"]);
            Assert.AreEqual("testFile.pdf", result.Rows[0]["FileName"]);
        }
    }
}