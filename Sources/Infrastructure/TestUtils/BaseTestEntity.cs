using Bus;
using Crud;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public class BaseTestEntity<T>: BaseBusTest where T:IEntity
    {
        protected T _saved;
        protected Mock<IRepository<T>> _repository;

        protected override void Initialize()
        {
            base.Initialize();
            _saved = default(T);
            _repository = new Mock<IRepository<T>>();
            _repository.Setup(repository => repository.Save(It.IsAny<T>()))
                .Returns<T>((entity) =>
                {
                    _saved = entity;
                    return entity.Id;
                });
            _repository.Setup(repository => repository.GetById(It.IsAny<Guid>()))
                .Returns<Guid>((id) =>
                {
                    if (_saved == null) return default(T);
                    return _saved.Id == id ? _saved : default(T);
                });

            _repository.Setup(repository => repository
                .GetAll(It.IsAny<Func<T, bool>>()))
                .Returns<Func<T, bool>>(data =>
                {
                    if (_saved != null && data.Invoke(_saved)) return new List<T> { _saved };
                    return new List<T>();
                });
        }
    }
}
