CREATE TABLE `Multi` (
  `Id`       INT            NOT NULL,
  `UserId`   INT            NOT NULL,
  `MasterId` INT            NOT NULL,
  `Desc`     VARCHAR(100)   NOT NULL,
  PRIMARY KEY (`Id`, `UserId`)
);