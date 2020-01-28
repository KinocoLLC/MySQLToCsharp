﻿using Cocona;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MySQLToCsharp.Tests.Helper;
using System.Linq;
using Xunit;

namespace MySQLToCsharp.Tests
{
    // collection test will execute serial
    [Collection("cli")]
    public class CommandLineQueryTest
    {
        [Fact]
        public void QueryExecutionTest()
        {
            var args = new[] { "query", "-i", "create table ships_guns(guns_id int, ship_id int);", "-o", "hoge", "-n", "Fuga" };
            CoconaApp.Create().ConfigureLogging(logging =>
            {
                logging.ReplaceToTraceLogger();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .Run<QueryToCSharp>(args);
            var expected = @"// Code generated by SqlToCsharp

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
            var (logLevel, msg) = TraceLogger.Stack.First(x => x.logLevel == LogLevel.Trace);
            msg.Should().Be(expected);
        }

        [Fact]
        public void QueryExecutionAnnotationTest()
        {
            var args = new[] { "query", "-i", "create table quengine(id int auto_increment key, class varchar(10), data binary) engine='InnoDB';", "-o", "hoge", "-n", "Fuga" };
            CoconaApp.Create().ConfigureLogging(logging =>
            {
                logging.ReplaceToTraceLogger();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .Run<QueryToCSharp>(args);
            var expected = @"// Code generated by SqlToCsharp

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
            var (logLevel, msg) = TraceLogger.Stack.First(x => x.logLevel == LogLevel.Trace);
            msg.Should().Be(expected);
        }
    }
}
