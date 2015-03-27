
/****** Object:  StoredProcedure [dbo].[question_Insert]    Script Date: 3/9/2015 12:54:46 PM ******/
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
-- ================================================


ALTER PROCEDURE[dbo].[question_Insert]

    @userId int

,	@status int

,	@createdate datetime

,	@questionText varchar(200)

,   @questionType int

AS
	BEGIN
		DECLARE @questionsSeo varchar(255) 
		SET @questionsSeo = LOWER(dbo.RemoveSpecialChars(REPLACE((LTRIM(RTRIM(@questionText))), ' ', '-')))
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

		,   question_type
		)

		VALUES(

			@userId

		,	@status

		,	@createdate

		,	@questionText

		,   @questionsSeo

		,   @questionType
		)

	END	

Select @@IDENTITY



