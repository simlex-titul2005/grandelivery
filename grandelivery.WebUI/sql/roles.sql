/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 20.09.2016 20:16:51
 ************************************************************/

IF NOT EXISTS (
       SELECT *
       FROM   AspNetRoles AS anr
       WHERE  anr.Name = 'customer'
   )
    INSERT INTO AspNetRoles
      (
        Id,
        NAME,
        [Description],
        Discriminator
      )
    VALUES
      (
        NEWID(),
        'customer',
        N'Заказчик',
        'SxAppRole'
      )
      GO
      
IF NOT EXISTS (
       SELECT *
       FROM   AspNetRoles AS anr
       WHERE  anr.Name = 'carrier'
   )
    INSERT INTO AspNetRoles
      (
        Id,
        NAME,
        [Description],
        Discriminator
      )
    VALUES
      (
        NEWID(),
        'carrier',
        N'Перевозчик',
        'SxAppRole'
      )
      GO