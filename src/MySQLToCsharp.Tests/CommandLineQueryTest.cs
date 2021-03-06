﻿using Cocona;
using FluentAssertions;
using MySQLToCsharp.Internal;
using System;
using System.Linq;
using Xunit;

namespace MySQLToCsharp.Tests
{
    public class CommandLineQueryTest
    {
        [Fact]
        public void QueryExecutionTest()
        {
            var id = Guid.NewGuid().ToString();
            var args = new[] { "query", "-i", "create table ships_guns(guns_id int, ship_id int);", "-o", "hoge", "-n", "Fuga", "--executionid", id };
            CoconaLiteApp.Create().Run<QueryToCSharp>(args);
            var expected = @"// ------------------------------------------------------------------------------
// <auto-generated>
// Code Generated by MySQLToCsharp
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuga
{
    public partial class ships_gun
    {
        public int guns_id { get; set; }
        public int ship_id { get; set; }
    }
}
";
            var msg = QueryToCSharp.Context.GetLogs(id).First();
            msg.Should().Be(InternalUtils.NormalizeNewLines(expected));
        }

        [Fact]
        public void QueryExecutionAnnotationTest()
        {
            var id = Guid.NewGuid().ToString();
            var args = new[] { "query", "-i", "create table quengine(id int auto_increment key, class varchar(10), data binary) engine='InnoDB';", "-o", "hoge", "-n", "Fuga", "--executionid", id };
            CoconaLiteApp.Run<QueryToCSharp>(args);
            var expected = @"// ------------------------------------------------------------------------------
// <auto-generated>
// Code Generated by MySQLToCsharp
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuga
{
    public partial class quengine
    {
        public int id { get; set; }
        [Required]
        [StringLength(10)]
        public string class { get; set; }
        [Required]
        public byte[] data { get; set; }
    }
}
";
            var msg = QueryToCSharp.Context.GetLogs(id).First();
            msg.Should().Be(InternalUtils.NormalizeNewLines(expected));
        }
    }
}
