CREATE TABLE `CharacterSlots` (
  `Id` INT NOT NULL,
  `SlotIndex` INT NOT NULL,
  `CharacterId` INT NOT NULL,
  `WeaponId` INT NULL,
  `WeaponId2` INT NULL,
  `Exp` INT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `AK_CharacterSlot_Column` (`SlotIndex` ASC, `CharacterId` ASC)
);
