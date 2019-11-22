using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InMemory.Crud;
using System.Linq;
using Crud;

// ReSharper disable RedundantLambdaSignatureParentheses
namespace InMemory.Repository
{
    [TestClass]
    public class InMemoryRepositoryTest
    {
        private InMemoryRepository<Entity> _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new InMemoryRepository<Entity>();
        }
        private static void VerifyInsertedData(IRepository<Entity> target, Entity newEntity,
            Guid id, string expectedData)
        {
            var result = target.GetById(id);
            Assert.AreEqual(1, target.GetAll().Count());
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedData, result.Data);
            Assert.AreNotSame(newEntity, result);
        }

        [TestMethod]
        public void ShouldInsertWhenNotPresentIdNotSet()
        {
            //Given
            
            const string expectedData = "1";
            var newEntity = new Entity { Data = expectedData };

            //When
            var id = _target.Save(newEntity);

            //Then
            VerifyInsertedData(_target, newEntity, id, expectedData);
        }

        [TestMethod]
        public void ShouldAllowInsertionWithGivenId()
        {
            //Given
            const string expectedData = "1";
            var expectedId = Guid.NewGuid();
            var newEntity = new Entity { Data = expectedData, Id = expectedId };
            Assert.IsNull(_target.GetById(newEntity.Id));

            //When
            _target.Save(newEntity);

            //Then
            Assert.IsNotNull(_target.GetById(newEntity.Id));
        }

        [TestMethod]
        public void ShouldUpdateWhenIdNotEmpty()
        {
            //Given
            const string expectedData = "2";
            var newEntity = new Entity { Data = "1" };
            var id = _target.Save(newEntity);

            //When
            newEntity.Data = expectedData;
            _target.Save(newEntity);

            //Then
            VerifyInsertedData(_target, newEntity, id, expectedData);
        }

        [TestMethod]
        public void ShouldIgnoreNotExistingDeletion()
        {
            //Given
            Assert.AreEqual(0, _target.GetAll().Count());

            //When Then
            _target.Delete(Guid.NewGuid());
        }

        [TestMethod]
        public void ShouldDeleteExistingItems()
        {
            //Given
            var newEntity = new Entity();
            var id = _target.Save(newEntity);
            Assert.AreEqual(1, _target.GetAll().Count());

            //When
            _target.Delete(id);

            //Then
            Assert.AreEqual(0, _target.GetAll().Count());
        }

        [TestMethod]
        public void ShouldSelectAllEntitiesWithGetAll()
        {
            //Given
            var data = new[] { "1", "2", "3" };
            foreach (var item in data)
            {
                _target.Save(new Entity { Data = item });
            }

            //When
            var result = _target.GetAll().ToList();

            //Then
            Assert.AreEqual(data.Length, result.Count);
            foreach (var item in data)
            {
                Assert.AreEqual(1, result.Count(a => a.Data == item));
            }
        }

        [TestMethod]
        public void ShouldUseTheGetAllExpression()
        {
            //Given
            var data = new[] { "1", "2", "2" };
            foreach (var item in data)
            {
                _target.Save(new Entity { Data = item });
            }

            //When
            var result = _target.GetAll((a)=>a.Data=="2").ToList();

            //Then
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All((a) => a.Data == "2"));
        }
    }
}
