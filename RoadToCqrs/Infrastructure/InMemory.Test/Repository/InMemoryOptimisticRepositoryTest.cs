using Microsoft.VisualStudio.TestTools.UnitTesting;
using InMemory.Crud;
using System.Linq;
using Crud;

namespace InMemory.Repository
{
    /// <summary>
    /// Summary description for InMemoryOptimisticRepositoryTest
    /// </summary>
    [TestClass]
    public class InMemoryOptimisticRepositoryTest
    {
        private InMemoryOptimisticRepository<OptimisticEntity> _target;

        private void SaveTwiceSettingToOneTheVersion(OptimisticEntity newItem)
        {
            _target.Save(newItem);
            _target.Save(newItem);
        }

        [TestInitialize]
        public void Setup()
        {
            _target = new InMemoryOptimisticRepository<OptimisticEntity>();
        }

        [TestMethod]
        public void ShouldInsertWithVersionZero()
        {
            //Given
            var newItem = new OptimisticEntity();

            //When
            var id = _target.Save(newItem);

            //Then
            var result = _target.GetByIdVersion(id, 0);
            Assert.IsNotNull(result);
            Assert.AreNotSame(newItem, result);
        }

        [TestMethod]
        public void ShouldIncrementVersionByOneUpdating()
        {
            //Given
            var newItem = new OptimisticEntity();
            var id = _target.Save(newItem);

            //When
            _target.Save(newItem);

            //Then
            Assert.AreEqual(1, _target.GetAll().Count());
            var result = _target.GetByIdVersion(id, 1);
            Assert.IsNotNull(result);
            Assert.AreNotSame(newItem, result);
        }

        [TestMethod]
        public void ShouldThrowExceptionUpdatingWithPastVersion()
        {
            //Given
            var newItem = new OptimisticEntity();
            SaveTwiceSettingToOneTheVersion(newItem);
            newItem.Version = 0;

            //When
            Assert.ThrowsException<OptimisticWriteException>(() => _target.Save(newItem));
        }

        [TestMethod]
        public void ShouldThrowExceptionUpdatingWithFutureVersion()
        {
            //Given
            var newItem = new OptimisticEntity();
            SaveTwiceSettingToOneTheVersion(newItem);
            newItem.Version = 2;

            //When
            Assert.ThrowsException<OptimisticWriteException>(() => _target.Save(newItem));
        }
    }
}
