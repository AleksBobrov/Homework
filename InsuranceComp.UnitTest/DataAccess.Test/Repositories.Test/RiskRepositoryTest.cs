using InsuranceComp.DataAccess.Providers;
using InsuranceComp.DataAccess.Repositories;
using Moq;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.DataAccess.Test.Repositories.Test
{
    [TestFixture]
    public class RiskRepositoryTest
    {
        Mock<IRiskDataProvider> RiskDataProviderMock;
        RiskRepository RiskRepository;

        [SetUp]
        public void SetUp()
        {
            RiskDataProviderMock = new Mock<IRiskDataProvider>();
            RiskRepository = new RiskRepository(RiskDataProviderMock.Object);
        }

        [Test]
        public void Add_DataProviderAddWasCalled()
        {
            RiskRepository.Add(It.IsAny<RiskModel>());
            RiskDataProviderMock.Verify(mock => mock.Add(It.IsAny<RiskModel>()), Times.Once);
        }

        [Test]
        public void GetAll_DataProviderGetAllWasCalled()
        {
            RiskRepository.GetAll();
            RiskDataProviderMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Test]
        public void Get_DataProviderGetWasCalled()
        {
            RiskRepository.Get(It.IsAny<string>());
            RiskDataProviderMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Remove_DataProviderRemoveWasCalled()
        {
            RiskRepository.Remove(It.IsAny<string>());
            RiskDataProviderMock.Verify(mock => mock.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Edit_DataProviderRemoveWasCalled()
        {
            RiskRepository.Edit(new RiskModel()
            {
                Id = "testid"
            });

            RiskDataProviderMock.Verify(mock => mock.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Remove_DataProviderAddWasCalled()
        {
            RiskRepository.Edit(new RiskModel()
            {
                Id = "testid"
            });

            RiskDataProviderMock.Verify(mock => mock.Add(It.IsAny<RiskModel>()), Times.Once);
        }
    }
}

