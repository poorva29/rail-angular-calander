USE [AsquareMirai]
GO

UPDATE [dbo].[users]
   SET 
      [name_seo] =LOWER(dbo.RemoveSpecialChars(REPLACE(firstname + '-' +lastname, ' ', '-')))
      WHERE name_seo is null
GO

