CREATE TABLE BinaryData (
  `Id` INT NOT NULL AUTO_INCREMENT, 
  `Data` BINARY(32) NOT NULL, 
  `NullableData` BINARY(8) NULL, 
  `VarData` VARBINARY(128) NOT NULL, 
  `NullableVarData` VARBINARY(512) NULL, 
  `Tiny` TINYBLOB NOT NULL, 
  `NullableTiny` TINYBLOB NULL,
  `Blob` BLOB NOT NULL, 
  `NullableBlob` BLOB NULL,
  `Medium` MEDIUMBLOB NOT NULL, 
  `NullableMedium` MEDIUMBLOB NULL,
  `Long` LONGBLOB NOT NULL, 
  `NullableLong` LONGBLOB NULL,
  PRIMARY KEY(`Id`)
);