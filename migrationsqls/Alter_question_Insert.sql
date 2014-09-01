USE [AsquareMiraiLiveBkp]
GO
/****** Object:  StoredProcedure [dbo].[question_Insert]    Script Date: 8/28/2014 6:46:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE[dbo].[question_Insert]

    @userId int

,	@status int

,	@createdate datetime

,	@questionText varchar(200)

AS

	BEGIN

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

		,   LOWER(dbo.RemoveSpecialChars(REPLACE(@questionText, ' ', '-')))

		)

	END	

Select @@IDENTITY