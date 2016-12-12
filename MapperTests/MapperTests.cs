using System;
using System.Collections.Generic;
using System.Linq;
using Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapperTests.TestClasses;

namespace MapperTests
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void GlobalTest()
        {
            var mapper = new Mapper.Mapper();
            var a = new TestA
            {
                A = "heh",
                B = 123,
                C = true
            };

            var b = mapper.QuickMap<TestA, TestB>(a);

            Assert.AreEqual(a.A, b.A);
            Assert.AreEqual(a.C, b.C);

            b.A = "wow";
            b.C = false;

            Assert.AreNotEqual(a.A, b.A);
            Assert.AreNotEqual(a.C, b.C);

            a = mapper.QuickMap(b, a);

            Assert.AreEqual(a.A, b.A);
            Assert.AreEqual(a.C, b.C);

            var collectA = new List<TestA>()
            {
                new TestA {A = "WOW", B = 123, C = true},
                new TestA {A = "SO", B = 234, C = false},
                new TestA {A = "HAPPY", B = 345, C = false}
            };

            var collectB = mapper.QuickMap<TestA, TestB>(collectA).ToList();

            Assert.AreEqual(collectA.Count, collectB.Count);

            for (var i = 0; i < collectA.Count; i++)
            {
                Assert.AreEqual(collectA.ElementAt(i).A, collectB.ElementAt(i).A);
                Assert.AreEqual(collectA.ElementAt(i).C, collectB.ElementAt(i).C);
            }
            collectA = mapper.QuickMap<TestB, TestA>(collectB, collectA).ToList();

            for (var i = 0; i < collectA.Count; i++)
            {
                Assert.AreEqual(collectA.ElementAt(i).A, collectB.ElementAt(i).A);
                Assert.AreEqual(collectA.ElementAt(i).C, collectB.ElementAt(i).C);
            }

            var c = new TestA { A = "start", B = 12, C = false };

            var d = mapper.Map<TestA, TestB>(c)
                .Set(x => x.A = "Finish")
                .Set(x => x.A += $" At {DateTime.Now.ToShortDateString()}")
                .Set(x => x.C = true)
                .Apply();

            Assert.AreNotEqual(c.A, d.A);
            Assert.AreNotEqual(c.C, d.C);

            c = mapper.Map(d, c)
                .PickSource((x, y) => x.A = y.A)
                .Set(x => x.B = 777)
                .Apply();

            var profiledMapper = new ProfiledMapper();

            profiledMapper.Initialize(
                () => profiledMapper.AddMap<TestB, TestA>((src, dest) =>
                {
                    profiledMapper.Map(src, dest)
                        .Set(x => x.A = "Yay")
                        .Set(x => x.A += $" At {DateTime.Now.ToShortDateString()}")
                        .Set(x => x.C = false);
                })
                ,
                () => profiledMapper.AddMap<TestA, TestB>((src, dest) => profiledMapper.Map(src, dest).Set(x => x.C = false))
            );


            c = profiledMapper.MapByProfile(d, c);
            d = profiledMapper.MapByProfile(c, d);

            Assert.AreEqual(c.A, d.A);
            Assert.AreEqual(c.C, d.C);


        }
    }
}
