-- =============================================
-- Author:		Pavan Shevle
-- Create date: 12/09/2014
-- Alter Log: Added name_seo
-- =============================================
  UPDATE [dbo].[users]
  SET 
  [name_seo] =LOWER(dbo.RemoveSpecialChars(REPLACE(LTRIM(RTRIM(firstname)) + '-' +LTRIM(RTRIM(lastname)), ' ', '-')))
  WHERE name_seo is null

