USE [AsquareMirai]
GO
/****** Object:  StoredProcedure [dbo].[answer_Insert]    Script Date: 29/Sep/14 6:10:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================

-- Author: <pavan shevle>

-- Create date: 29/09/2014

-- Description: answertext datatype changed varchar(1500)

-- =============================================
ALTER PROCEDURE[dbo].[answer_Insert]
    @questionId int
,	@userId int
,	@createDate  datetime
,   @title varchar(100)
,	@answer varchar(1500)
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



