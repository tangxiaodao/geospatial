﻿using System;
using MbUnit.Framework;
using GIS.Framework.SQL2008.SQLGeography;
using GIS.Framework.SQL2008.SQLGeography.Features;
using GIS.Framework.SQL2008.SQLGeography.Layers;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using Jayrock.Json;

namespace GIS.Framework.SQL2008.UnitTests.SQLGeography
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="SQL2008PointLayer"/> class.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(SQL2008PointLayer))]
    public class SQL2008PointLayerTest
    {
        #region Fields, SetUp and TearDown
        private SQL2008PointLayer target = null;
        
        /// <summary>
        /// Sets up the fixture
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            
        }
        /// <summary>
        /// Cleans up the fixture
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            IDisposable disposable = this.target as IDisposable;
            if (disposable!=null)
                disposable.Dispose();
        }
        #endregion
    
        #region Test cases
        [Test]
        public void Test()
        {
            string connStr = @"Data Source=vish-pc;Initial Catalog=VishDB;Integrated Security=True;Connection Timeout=180;";

            using(SqlConnection sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using(SqlCommand cmd = new SqlCommand("Select * from AddressGeog", sqlConn))
                {
                    target = new SQL2008PointLayer(cmd, "geog", "Address", "ID");
                    target.Search(null);

                    StringBuilder text = new StringBuilder();
                    StringWriter writer = new StringWriter(text);
                    JsonTextWriter jwriter = new JsonTextWriter(writer);
                    target.ToJSON(jwriter);
                    string result = text.ToString();
                    Assert.IsTrue(!String.IsNullOrEmpty(result), "Serialization of Point FC layer failed");
                }
            }
        }
        #endregion    
    }
}
	