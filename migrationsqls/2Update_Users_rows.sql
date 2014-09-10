USE [AsquareMirai]
GO

  UPDATE [dbo].[users]
  SET 
  [name_seo] =LOWER(dbo.RemoveSpecialChars(REPLACE(LTRIM(RTRIM(firstname)) + '-' +LTRIM(RTRIM(lastname)), ' ', '-')))
  WHERE name_seo is null

