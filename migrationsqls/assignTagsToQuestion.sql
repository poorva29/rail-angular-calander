USE [AsquareMirai]
GO
/****** Object:  StoredProcedure [dbo].[assignTagsToQuestion]    Script Date: 8/26/2014 7:18:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE[dbo].[assignTagsToQuestion]
    @addtags doctags READONLY,
	@deletetags doctags READONLY
,	@questionId INT =0
AS
BEGIN
declare @count int
	if exists(select * from questiontags where questionid = @questionId )
	begin 
		 delete from questiontags where tagid IN(select tagid from @deletetags ) and questionid = @questionId;
	     INSERT INTO dbo.questiontags(questionid,tagid) select @questionId, tagid from @addtags;
	end
	else
	begin
		INSERT INTO dbo.questiontags(questionid,tagid) select @questionId, tagid from @addtags;
	end
End

