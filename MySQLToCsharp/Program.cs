using Antlr4.Runtime.Tree;
using System;

namespace MySQLToCSharp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var listener = new StatementDetectListener();
            var parser = new Parser();
            parser.Parse("select * from hoge where a = 'b';", listener);
            var hoge = listener.IsSelectStatementFound;

            var listener2 = new StatementDetectListener();
            var parser2 = new Parser();
            parser2.Parse(@"CREATE TABLE `Achievements` (
	`Id` BIGINT(20) NOT NULL AUTO_INCREMENT,
	`UserId` INT(11) NOT NULL,
	`MasterId` INT(11) NOT NULL,
	`Value` INT(11) NOT NULL DEFAULT '0',
	`Status` TINYINT(3) UNSIGNED NOT NULL DEFAULT '1',
	`Created` DATETIME(6) NOT NULL,
    PRIMARY KEY(`Id`),
    INDEX `IDX_User_Status` (`UserId`, `Status`),
	INDEX `IDX_MasterId_Status` (`MasterId`, `Status`)
)
COLLATE = 'utf8mb4_general_ci'
ENGINE = InnoDB
AUTO_INCREMENT = 9
;", listener2);
            var fuga = listener.IsCreateTableStatement;
        }
    }
}
