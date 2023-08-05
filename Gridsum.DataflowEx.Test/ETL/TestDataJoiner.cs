﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Gridsum.DataflowEx.Test.ETL
//{
//    using System.Data;
//    using System.Diagnostics;
//    using System.Linq.Expressions;
//    using System.Threading.Tasks.Dataflow;

//    using Gridsum.DataflowEx.Databases;
//    using Gridsum.DataflowEx.ETL;
//    using Gridsum.DataflowEx.Test.DatabaseTests;
//    using Microsoft.EntityFrameworkCore;
//    using Microsoft.VisualStudio.TestTools.UnitTesting;

//    [TestClass]
//    public class TestDataJoiner
//    {
//        class JoinerWithAsserter : DbDataJoiner<Trunk, string>
//        {
//            public int Count;

//            public JoinerWithAsserter(Expression<Func<Trunk, string>> joinOn, TargetTable dimTableTarget, int batchSize)
//                : base(joinOn, dimTableTarget, DataflowOptions.Default, batchSize)
//            {
//                this.Count = 0;
//            }

//            protected override void OnSuccessfulLookup(Trunk input, IDimRow<string> rowInDimTable)
//            {
//                Assert.AreEqual(input.Pointer, rowInDimTable.JoinOn);
//                //Assert.AreEqual("Str" + input.Pointer, rowInDimTable["StrValue"]);
//                this.Count++;
//            }
//        }

//        [TestMethod]
//        public async Task TestDataJoinerJoining()
//        {
//            var connectString = TestUtils.GetLocalDBConnectionString("TestDataJoinerJoining");
//            var context = new InsertContext(connectString);

//            for (int i = 0; i < 10; i++)
//            {
//                context.Leafs.Add(new Leaf() {IntValue = i, Key = i.ToString(), StrValue = "Str" + i});    
//            }
            
//            context.SaveChanges();

//            var joiner = new JoinerWithAsserter(
//                t => t.Pointer,
//                new TargetTable(Leaf.DimLeaf, connectString, "Leafs"),
//                2);

//            joiner.DataflowOptions.MonitorInterval = TimeSpan.FromSeconds(3);
//            joiner.LinkLeftToNull();

//            var dataArray = new[]
//                                {
//                                    new Trunk() { Pointer = "3" }, 
//                                    new Trunk() { Pointer = "5" },
//                                    new Trunk() { Pointer = "7" }, 
//                                    new Trunk() { Pointer = "7" },
//                                    new Trunk() { Pointer = "9" },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                    new Trunk() { Pointer = "99", Leaf = new Leaf() { StrValue = "Str99" } },
//                                };

//            await joiner.ProcessAsync(dataArray);
            
//            Assert.AreEqual(dataArray.Length, joiner.Count);
//        }
//    }

//    public class Trunk
//    {
//        public int Id { get; set; }

//        [DBColumnMapping(Leaf.DimLeaf, "Key")]
//        public string Pointer { get; set; }

//        public string Name { get; set; }
//        public Leaf Leaf { get; set; }
//    }

//    public class Leaf
//    {
//        public const string DimLeaf = "DimLeaf";

//        public int Id { get; set; }

//        public string Key { get; set; }

//        [DBColumnMapping(DimLeaf)]
//        public string StrValue { get; set; }

//        [DBColumnMapping(DimLeaf)]
//        public int IntValue { get; set; }
//    }

//    public class InsertContext : DbContextBase<InsertContext>
//    {
//        public InsertContext(string conn) 
//            : base(conn)
//        {
//        }

//        public DbSet<Trunk> Trunks { get; set; }
//        public DbSet<Leaf> Leafs { get; set; }
//    }
//}
