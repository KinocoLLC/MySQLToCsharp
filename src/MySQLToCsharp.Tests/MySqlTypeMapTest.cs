using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MySQLToCsharp.Tests
{
    public class MySqlTypeMapTest
    {
        [Theory]
        [InlineData("CHAR")]
        [InlineData("CHAR(20)")]
        [InlineData("VARCHAR")]
        [InlineData("VARCHAR(255)")]
        [InlineData("BINARY")]
        [InlineData("BINARY(10)")]
        [InlineData("VARBINARY")]
        [InlineData("VARBINARY(100)")]
        [InlineData("TINYBLOB")]
        [InlineData("BLOB")]
        [InlineData("MEDIUMBLOB")]
        [InlineData("LONGBLOB")]
        [InlineData("TINYTEXT")]
        [InlineData("TEXT")]
        [InlineData("MEDIUMTEXT")]
        [InlineData("LONGTEXT")]
        public void StringType(string typeName)
        {
            var mapper = new MySqlTypeMapper();
            var type = mapper.Map(typeName);
            Assert.IsType<StringMySqlType>(type);
        }

        // TODO: should be fail as it were invalid
        [Theory]
        [InlineData("TINYBLOB(100)")]
        [InlineData("BLOB(100)")]
        [InlineData("MEDIUMBLOB(100)")]
        [InlineData("LONGBLOB(100)")]
        [InlineData("TINYTEXT(100)")]
        [InlineData("TEXT(100)")]
        [InlineData("MEDIUMTEXT(100)")]
        [InlineData("LONGTEXT(100)")]
        public void StringType2(string typeName)
        {
            var mapper = new MySqlTypeMapper();
            var type = mapper.Map(typeName);
            Assert.IsType<StringMySqlType>(type);
        }

        [Theory]
        [InlineData("TINYBLOB(100")]
        [InlineData("BLOB(100")]
        [InlineData("MEDIUMBLOB(100")]
        [InlineData("LONGBLOB(100")]
        [InlineData("TINYTEXT(100")]
        [InlineData("TEXT(100")]
        [InlineData("MEDIUMTEXT(100")]
        [InlineData("LONGTEXT(100")]
        public void ThrowsStringTypeInvalidParentheses(string typeName)
        {
            var mapper = new MySqlTypeMapper();
            Assert.Throws<ArgumentOutOfRangeException>(() => mapper.Map(typeName));
        }

        [Theory]
        [InlineData("BIT")]
        [InlineData("BIT(64)")]
        [InlineData("TINYINT")]
        [InlineData("TINYINT(4)")]
        [InlineData("SMALLINT")]
        [InlineData("SMALLINT(6)")]
        [InlineData("MEDIUMINT")]
        [InlineData("MEDIUMINT(9)")]
        [InlineData("INT")]
        [InlineData("INT(11)")]
        [InlineData("BIGINT")]
        [InlineData("BIGINT(20)")]
        [InlineData("FLOAT")]
        [InlineData("FLOAT(23)")]
        [InlineData("FLOAT(7,4)")]
        [InlineData("DOUBLE")]
        [InlineData("DOUBLE(30)")]
        [InlineData("DOUBLE(10,4)")]
        public void NumericType(string typeName)
        {
            var mapper = new MySqlTypeMapper();
            var type = mapper.Map(typeName);
            Assert.IsType<NumericMySqlType>(type);
        }

        [Theory]
        [InlineData("BIT(64")]
        [InlineData("TINYINT(4")]
        [InlineData("SMALLINT(6")]
        [InlineData("MEDIUMINT(9")]
        [InlineData("INT(11")]
        [InlineData("BIGINT(20")]
        [InlineData("FLOAT(23")]
        [InlineData("FLOAT(7,4")]
        [InlineData("DOUBLE(30")]
        [InlineData("DOUBLE(10,4")]
        public void ThrowsNumericTypeInvalidParentheses(string typeName)
        {
            var mapper = new MySqlTypeMapper();
            Assert.Throws<ArgumentOutOfRangeException>(() => mapper.Map(typeName));
        }

        [Theory]
        [InlineData("FLOAT(7,")]
        [InlineData("FLOAT(7,3,4")]
        public void ThrowsNumericTypeInvalidParenthesesItems(string typeName)
        {
            var mapper = new MySqlTypeMapper();
            Assert.Throws<ArgumentOutOfRangeException>(() => mapper.Map(typeName));
        }

        [Theory]
        [InlineData("DATE")]
        [InlineData("TIME")]
        [InlineData("TIME(6)")]
        [InlineData("DATETIME")]
        [InlineData("DATETIME(6)")]
        [InlineData("TIMESTAMP")]
        [InlineData("TIMESTAMP(6)")]
        [InlineData("YEAR")]
        public void DateType(string typeName)
        {
            var mapper = new MySqlTypeMapper();
            var type = mapper.Map(typeName);
            Assert.IsType<DateMySqlType>(type);
        }

        [Theory]
        [InlineData("GEOMETRY")]
        [InlineData("POINT")]
        [InlineData("LINESTRING")]
        [InlineData("POLYGON")]
        [InlineData("MULTIPOINT")]
        [InlineData("MUTILINESTRING")]
        [InlineData("MULTIPOLYGON")]
        [InlineData("GEOMETRYCOLLECTION")]
        [InlineData("SET")]
        public void FollbackType(string typeName)
        {
            var mapper = new MySqlTypeMapper();
            var type = mapper.Map(typeName);
            Assert.IsType<FollbackMySqlType>(type);
        }

    }
}
