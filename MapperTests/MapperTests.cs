﻿using System;
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
            var wm = new Mapper.Mapper();
            var a = new TestA
            {
                A = "heh",
                B = 123,
                C = true
            };

            var b = wm.QuickMap<TestA, TestB>(a);

            Assert.AreEqual(a.A, b.A);
            Assert.AreEqual(a.C, b.C);

            b.A = "wow";
            b.C = false;

            Assert.AreNotEqual(a.A, b.A);
            Assert.AreNotEqual(a.C, b.C);

            a = wm.QuickMap(b, a);

            Assert.AreEqual(a.A, b.A);
            Assert.AreEqual(a.C, b.C);

            var collectA = new List<TestA>()
            {
                new TestA {A = "WOW", B = 123, C = true},
                new TestA {A = "SO", B = 234, C = false},
                new TestA {A = "HAPPY", B = 345, C = false}
            };

            var collectB = wm.QuickMap<TestA, TestB>(collectA).ToList();

            Assert.AreEqual(collectA.Count, collectB.Count);

            for (var i = 0; i < collectA.Count; i++)
            {
                Assert.AreEqual(collectA.ElementAt(i).A, collectB.ElementAt(i).A);
                Assert.AreEqual(collectA.ElementAt(i).C, collectB.ElementAt(i).C);
            }
            collectA = wm.QuickMap<TestB, TestA>(collectB, collectA).ToList();

            for (var i = 0; i < collectA.Count; i++)
            {
                Assert.AreEqual(collectA.ElementAt(i).A, collectB.ElementAt(i).A);
                Assert.AreEqual(collectA.ElementAt(i).C, collectB.ElementAt(i).C);
            }

            var p = new TestA() { A = "start", B = 12, C = false };

            var pp = wm.Map<TestA, TestB>(p)
                .Set(x => x.A = "Finish")
                .Set(x => x.A += $" At {DateTime.Now.ToShortDateString()}")
                .Set(x => x.C = true)
                .Apply();

            Assert.AreNotEqual(p.A, pp.A);
            Assert.AreNotEqual(p.C, pp.C);

            p = wm.Map(pp, p)
                .PickSource((x, y) => x.A = y.A)
                .Set(x => x.B = 777)
                .Apply();

            var ppppp = new ProfiledMapper();

            ppppp.Initialize(new List<Action>
            {
                () => ppppp.AddMap<TestB, TestA>((src, dest) =>
                {
                    ppppp.Map(src, dest)
                        .Set(x => x.A = "Yay")
                        .Set(x => x.A += $" At {DateTime.Now.ToShortDateString()}")
                        .Set(x => x.C = false);
                })
                ,
                () => ppppp.AddMap<TestA, TestB>((src, dest) => ppppp.Map(src, dest).Set(x => x.C = false))
            });


            p = ppppp.MapByProfile(pp, p);
            pp = ppppp.MapByProfile(p, pp);

            Assert.AreEqual(p.A, pp.A);
            Assert.AreEqual(p.C, pp.C);


        }
    }
}