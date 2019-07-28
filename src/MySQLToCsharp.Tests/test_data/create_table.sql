CREATE TABLE `Samples` (
    `Id` BIGINT(20) NOT NULL AUTO_INCREMENT,
    `SampleId` INT(11) NOT NULL,
    `MasterId` INT(11) NOT NULL,
    `Value` INT(11) NOT NULL DEFAULT '0',
    `Status` TINYINT(3) UNSIGNED NOT NULL DEFAULT '1',
    `Created` DATETIME(6) NOT NULL,
    PRIMARY KEY(`Id`),
    UNIQUE INDEX `UQ_SampleId_MasterId` (`SampleId`, `MasterId`),
    INDEX `SampleId_Status` (`SampleId`, `Status`),
    INDEX `MasterId_Status` (`MasterId`, `Status`)
)
COLLATE = 'utf8mb4_general_ci'
ENGINE = InnoDB
AUTO_INCREMENT = 9
;