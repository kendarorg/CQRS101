using Commons.Implementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Commons;
using Commons.Repository;

namespace Commons.Tests
{
    public class SimpleItem
    {
        public Guid Id { get; internal set; }
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class SimpleRepo : MockRepository<SimpleItem, Guid>
    {
        private static ConcurrentDictionary<string, SimpleItem> Storage;

        static SimpleRepo()
        {
            Storage = new ConcurrentDictionary<string, SimpleItem>();
        }

        public override Guid GetKey(SimpleItem entity)
        {
            return entity.Id;
        }

        public override void SetKey(SimpleItem entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, SimpleItem> GetStorage()
        {
            return Storage;
        }

        protected override Guid InitializeKey(Guid key)
        {
            if (key == Guid.Empty) return Guid.NewGuid();
            return key;
        }

        [TestFixture]
        public class TestClass
        {
            public IRepository<SimpleItem, Guid> GetItems()
            {
                var repo = new SimpleRepo();
                repo.GetStorage().Clear();
                repo.Save(new SimpleItem
                {
                    Text = "First",
                    Value = 1,
                    Id = Guid.NewGuid()
                });
                repo.Save(new SimpleItem
                {
                    Text = "Second",
                    Value = 2,
                    Id = Guid.NewGuid()
                });
                return repo;
            }
            [Test]
            public void ItShouldBePossibleToSelectWithFilters()
            {
                var repo = GetItems();
                var items = repo.Find(new Filter
                {
                    Field = "Text",
                    Type = FilterType.Equal,
                    Value = "Second"
                }).ToList();

                Assert.AreEqual(1, items.Count);
                Assert.AreEqual(2, items[0].Value);
            }


            [Test]
            public void ItShouldBePossibleToSelectWithCompositeFilters()
            {
                var repo = GetItems();
                var items = repo.Find(new Filter
                {
                    Type = FilterType.And,
                    Conditions = new List<Repository.Filter>
                    {
                        new Filter
                        {
                            Field = "Text",
                            Type = FilterType.Equal,
                            Value = "Second"
                        },
                        new Filter
                        {
                            Field = "Value",
                            Type = FilterType.Equal,
                            Value = "2"
                        }
                    }
                }).ToList();

                Assert.AreEqual(1, items.Count);
                Assert.AreEqual(2, items[0].Value);
            }
            [Test]
            public void ItShouldBePossibleToSelectWithFiltersInteger()
            {
                var repo = GetItems();
                var items = repo.Find(new Filter
                {
                    Field = "Value",
                    Type = FilterType.Equal,
                    Value = "2"
                }).ToList();

                Assert.AreEqual(1, items.Count);
                Assert.AreEqual(2, items[0].Value);
            }
        }
    }
}
