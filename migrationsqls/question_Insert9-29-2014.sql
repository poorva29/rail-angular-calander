USE [AsquareMirai]
GO
/****** Object:  StoredProcedure [dbo].[question_Insert]    Script Date: 9/26/2014 1:41:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- Author:		
-- Create date: 
-- Description:	
-- Update Log: changes for inserting uniqe name_seo
--	From line no 29 to 44
-- chages for storing 10 word in question seo from line 32 to 36
-- ================================================================


ALTER PROCEDURE[dbo].[question_Insert]

    @userId int

,	@status int

,	@createdate datetime

,	@questionText varchar(200)

AS
	BEGIN
		DECLARE @questionsSeo varchar(255) 

		SELECT TOP 10 @questionsSeo = COALESCE(@questionsSeo + '-', '') + Q.Value 
		FROM  DBO.SplitStringAsTable((dbo.RemoveSpecialChars(REPLACE((LTRIM(RTRIM(@questionText))), ' ', '-'))),'-') Q

		WHILE EXISTS(SELECT TOP 1 * FROM questions WHERE question_seo=@questionsSeo)
		BEGIN
			SELECT TOP 1 @questionsSeo = question_seo FROM questions WHERE question_seo=@questionsSeo
			declare @lastChar varchar(4)
			set	@lastChar = substring(@questionsSeo,len(@questionsSeo),len(@questionsSeo)+1)
			IF TRY_CONVERT(INT, @lastChar) IS NOT NULL
			begin
				set @questionsSeo = CONCAT(substring(@questionsSeo,0,len(@questionsSeo)),CAST(@lastChar as int)+1)
			end 
			ELSE
			begin
				set @questionsSeo = CONCAT(@questionsSeo,'-1')
			end
		END 
		INSERT INTO dbo.questions (

			userid

		,	status

		,	createdate

		,	questiontext

		,   question_seo

		)

		VALUES(

			@userId

		,	@status

		,	@createdate

		,	@questionText

		,   LOWER(@questionsSeo)

		)

	END	

Select @@IDENTITY


