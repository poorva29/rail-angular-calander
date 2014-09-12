-- =============================================

-- Author: <pavan shevle>

-- Create date: 12/09/2014

-- Description: answer text as 1000 char

-- =============================================
ALTER PROCEDURE[dbo].[answer_Insert]
    @questionId int
,	@userId int
,	@createDate  datetime
,   @title varchar(100)
,	@answer varchar(1000)
,	@filename varchar(100)
as
	BEGIN
		insert into answers(	
				questionId
		   ,	userId
		   ,	createDate
		   ,	answertext
		   ,    image
		   ,	title)
		   VALUES (
				@questionId
		   ,	@userId
		   ,	@createDate
		   ,	@answer
		   ,	@filename
		   ,	@title
		   )
	END	
	Select @@IDENTITY


