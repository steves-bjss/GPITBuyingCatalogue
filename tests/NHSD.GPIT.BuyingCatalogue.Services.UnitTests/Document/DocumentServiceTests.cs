﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.GPIT.BuyingCatalogue.Framework.Logging;
using NHSD.GPIT.BuyingCatalogue.Services.Document;
using NUnit.Framework;

namespace NHSD.GPIT.BuyingCatalogue.Services.UnitTests.Document
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    internal static class DocumentServiceTests
    {
        [Test]
        public static void Constructor_NullLogger_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new DocumentService(
                null,
                Mock.Of<IAzureBlobDocumentRepository>()));
        }

        [Test]
        public static void Constructor_NullRepository_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new DocumentService(
                Mock.Of<ILogWrapper<DocumentService>>(),
                null));
        }

        [Test]
        public static async Task DownloadDocumentAsync_DocumentRepositoryException_IsLogged()
        {
            var expectedException = new DocumentRepositoryException(
                new InvalidOperationException(),
                StatusCodes.Status502BadGateway);

            var mockStorage = new Mock<IAzureBlobDocumentRepository>();
            mockStorage.Setup(s => s.DownloadAsync(It.IsAny<string>())).Throws(expectedException);

            var mockLogger = new Mock<ILogWrapper<DocumentService>>();

            var controller = new DocumentService(mockLogger.Object, mockStorage.Object);

            await controller.DownloadDocumentAsync("directory");

            Expression<Action<ILogWrapper<DocumentService>>> expression = l => l.LogError(
                expectedException,
                null);

            mockLogger.Verify(expression);
        }

        [Test]
        public static void DownloadDocumentAsync_Exception_DoesNotSwallow()
        {
            var exception = new InvalidOperationException();

            var mockStorage = new Mock<IAzureBlobDocumentRepository>();

            mockStorage.Setup(s => s.DownloadAsync(It.IsAny<string>())).Throws(exception);

            var controller = new DocumentService(Mock.Of<ILogWrapper<DocumentService>>(), mockStorage.Object);

            Assert.ThrowsAsync<InvalidOperationException>(() => controller.DownloadDocumentAsync("directory"));
        }

        [Test]
        public static async Task DownloadDocumentAsync_ReturnsFileStreamResult()
        {
            const string expectedContentType = "test/content-type";

            await using var expectedStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world!"));

            var downloadInfo = new Mock<IDocument>();
            downloadInfo.Setup(d => d.Content).Returns(expectedStream);
            downloadInfo.Setup(d => d.ContentType).Returns(expectedContentType);

            var mockStorage = new Mock<IAzureBlobDocumentRepository>();

            mockStorage.Setup(s => s.DownloadAsync(It.IsAny<string>())).ReturnsAsync(downloadInfo.Object);

            var controller = new DocumentService(Mock.Of<ILogWrapper<DocumentService>>(), mockStorage.Object);

            var result = await controller.DownloadDocumentAsync("directory") as FileStreamResult;

            Assert.NotNull(result);
            result.FileStream.IsSameOrEqualTo(expectedStream);
            result.ContentType.Should().Be(expectedContentType);
        }

        [Test]
        public static async Task DownloadSolutionDocumentAsync_DocumentRepositoryException_IsLogged()
        {
            var expectedException = new DocumentRepositoryException(
                new InvalidOperationException(),
                StatusCodes.Status502BadGateway);

            var mockStorage = new Mock<IAzureBlobDocumentRepository>();

            mockStorage.Setup(s => s.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(expectedException);

            var mockLogger = new Mock<ILogWrapper<DocumentService>>();

            var controller = new DocumentService(mockLogger.Object, mockStorage.Object);

            await controller.DownloadSolutionDocumentAsync("ID", "directory");

            Expression<Action<ILogWrapper<DocumentService>>> expression = l => l.LogError(
                expectedException,
                null);

            mockLogger.Verify(expression);
        }

        [Test]
        public static void DownloadSolutionDocumentAsync_Exception_DoesNotSwallow()
        {
            var exception = new InvalidOperationException();

            var mockStorage = new Mock<IAzureBlobDocumentRepository>();

            mockStorage.Setup(s => s.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(exception);

            var controller = new DocumentService(Mock.Of<ILogWrapper<DocumentService>>(), mockStorage.Object);

            Assert.ThrowsAsync<InvalidOperationException>(() => controller.DownloadSolutionDocumentAsync("ID", "directory"));
        }

        [Test]
        public static async Task DownloadSolutionDocumentAsync_ReturnsFileStreamResult()
        {
            const string expectedContentType = "test/content-type";

            await using var expectedStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world!"));

            var downloadInfo = new Mock<IDocument>();
            downloadInfo.Setup(d => d.Content).Returns(expectedStream);
            downloadInfo.Setup(d => d.ContentType).Returns(expectedContentType);

            var mockStorage = new Mock<IAzureBlobDocumentRepository>();

            mockStorage.Setup(s => s.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(downloadInfo.Object);

            var controller = new DocumentService(Mock.Of<ILogWrapper<DocumentService>>(), mockStorage.Object);

            var result = await controller.DownloadSolutionDocumentAsync("ID", "directory") as FileStreamResult;

            Assert.NotNull(result);
            result.FileStream.IsSameOrEqualTo(expectedStream);
            result.ContentType.Should().Be(expectedContentType);
        }

        [Test]
        public static void GetFileNamesAsync_ReturnsStorageResult()
        {
            var mockEnumerable = new Mock<IAsyncEnumerable<string>>();

            var mockStorage = new Mock<IAzureBlobDocumentRepository>();
            mockStorage.Setup(s => s.GetFileNamesAsync(It.IsAny<string>()))
                .Returns(mockEnumerable.Object);

            var controller = new DocumentService(Mock.Of<ILogWrapper<DocumentService>>(), mockStorage.Object);
            var result = controller.GetDocumentsBySolutionId("Foobar");

            result.Should().Be(mockEnumerable.Object);
            mockStorage.Verify(r => r.GetFileNamesAsync("Foobar"));
        }
    }
}