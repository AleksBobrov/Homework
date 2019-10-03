using InsuranceComp.DataAccess.Providers;
using Moq;
using NUnit.Framework;

namespace InsuranceComp.UnitTest.DataAccess.Test.Repositories.Test
{
    [TestFixture]
    public class PolicyRepositoryTest
    {
        Mock<IPolicyDataProvider> PolicyDataProviderMock;
        PolicyRepository PolicyRepository;
        string DEFAULT_TEST_ID = "testid";

        [SetUp]
        public void SetUp()
        {
            PolicyDataProviderMock = new Mock<IPolicyDataProvider>();
            PolicyRepository = new PolicyRepository(PolicyDataProviderMock.Object);
        }

        [Test]
        public void Add_DataProviderAddWasCalled()
        {
            PolicyRepository.Add(It.IsAny<PolicyModel>());
            PolicyDataProviderMock.Verify(mock => mock.Add(It.IsAny<PolicyModel>()), Times.Once);
        }

        [Test]
        public void GetAll_DataProviderGetAllWasCalled()
        {
            PolicyRepository.GetAll();
            PolicyDataProviderMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Test]
        public void Get_DataProviderGetWasCalled()
        {
            PolicyRepository.Get(It.IsAny<string>());
            PolicyDataProviderMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Remove_DataProviderRemoveWasCalled()
        {
            PolicyRepository.Remove(It.IsAny<string>());
            PolicyDataProviderMock.Verify(mock => mock.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Edit_DataProviderRemoveWasCalled()
        {
            PolicyRepository.Edit(new PolicyModel() {
                Id = DEFAULT_TEST_ID
            });

            PolicyDataProviderMock.Verify(mock => mock.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Remove_DataProviderAddWasCalled()
        {
            PolicyRepository.Edit(new PolicyModel()
            {
                Id = DEFAULT_TEST_ID
            });

            PolicyDataProviderMock.Verify(mock => mock.Add(It.IsAny<PolicyModel>()), Times.Once);
        }
    }
}
