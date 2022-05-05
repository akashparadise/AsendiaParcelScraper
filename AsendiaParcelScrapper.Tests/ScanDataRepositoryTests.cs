using Moq;
using Xunit;
using AsendiaParcelScraper.Repository;
using AsendiaParcelScraper;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace AsendiaParcelScrapper.Tests
{
    public class ScanDataRepositoryTests
    {
        private readonly IServiceProvider _serviceProvider;

        public ScanDataRepositoryTests()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(IScanDataRepository))).Returns(new ScanDataRepository());

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);

            serviceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactory.Object);

            _serviceProvider = serviceProvider.Object;
        }


        [Fact]
        public void ReadDataFromCSVFiles_ValidCall()
        {
            //arrange
            var dirPath = @"D:\CodingAssignments\Asendia";

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var stringReader = new StringReader(dirPath);
            Console.SetIn(stringReader);

            //act
            var actual = Program.LoadDataUsingDI(_serviceProvider);
            var expected = "Operation is successful";

            //assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadDataFromCSVFiles_FailedCall()
        {
            //arrange
            var dirPath = @"D:\CodingAssignments\Asendia";

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var stringReader = new StringReader(dirPath);
            Console.SetIn(stringReader);

            //act
            var actual = Program.LoadDataUsingDI(_serviceProvider);
            var expected = "Operation failed";

            //assert
            Assert.Equal(expected, actual);
        }
    }
}
