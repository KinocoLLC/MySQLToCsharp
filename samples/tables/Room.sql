-- RowVersion in SQLServer will convert to byte[] + [TIMESTAMP] attribute
CREATE TABLE `Room` (
  `Id` INT NOT NULL,
  `RowVersion` TIMESTAMP NOT NULL,
  PRIMARY KEY (`Id`)
);
