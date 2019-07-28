CREATE TABLE `Character` (
  `Id`       INT            NOT NULL,
  `MasterId` INT            NOT NULL,
  `Name`     VARCHAR (20)   NOT NULL,
  `Exp`      INT            NOT NULL,
  PRIMARY KEY (`Id`)
);