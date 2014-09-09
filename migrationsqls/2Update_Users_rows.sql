USE [AsquareMirai]
GO

UPDATE [dbo].[users]
   SET 
      [name_seo] = firstname +'-'+ lastname
 WHERE name_seo is null
GO

