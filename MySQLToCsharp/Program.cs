﻿using MySQLToCsharp.Listeners;
using System;

namespace MySQLToCSharp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var query = @"CREATE TABLE `Samples` (
	`Id` BIGINT(20) NOT NULL AUTO_INCREMENT,
	`SampleId` INT(11) NOT NULL,
	`MasterId` INT(11) NOT NULL,
	`Value` INT(11) NOT NULL DEFAULT '0',
	`Status` TINYINT(3) UNSIGNED NOT NULL DEFAULT '1',
	`Created` DATETIME(6) NOT NULL,
    PRIMARY KEY(`Id`),
    INDEX `IDX_User_Status` (`SampleId`, `Status`),
	INDEX `IDX_MasterId_Status` (`MasterId`, `Status`)
)
COLLATE = 'utf8mb4_general_ci'
ENGINE = InnoDB
AUTO_INCREMENT = 9
;";

            var createTableListener = new CreateTableStatementDetectListener();
            var parser = new Parser();
            parser.Parse(query, createTableListener);
            parser.PrintTokens();
            var definition = createTableListener.TableDefinition;
        }
    }
}
