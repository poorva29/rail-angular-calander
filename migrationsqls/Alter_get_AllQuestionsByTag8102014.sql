/****** Object:  StoredProcedure [dbo].[get_AllQuestionsByTag]    Script Date: 10/9/2014 11:47:08 AM ******/
DROP PROCEDURE [dbo].[get_AllQuestionsByTag]
GO

/****** Object:  StoredProcedure [dbo].[get_AllQuestionsByTag]    Script Date: 10/9/2014 11:47:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<priyanka priyadarshni>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Update Log : Added question_seo in line no.34 and 68
-- =============================================
CREATE PROCEDURE [dbo].[get_AllQuestionsByTag] 
		@questionText varchar(200)
   ,	@questionStatus int		
AS
BEGIN
Declare @tempQuestion as table
(
   id int primary key identity(1,1),
   QuestionsRowNumber int,
   doctorname varchar(200),
   questionid int,
   questiontext varchar(200),
   question_seo varchar(200),
   counts varchar(100),
   createdate datetime,
   RowNumber int
)
   insert into @tempQuestion
   SELECT top 10 ROW_NUMBER() OVER (ORDER BY createdate asc) as QuestionsRowNumber,*
	FROM ( SELECT 'Dr. ' + users.firstname + ' ' + users.lastname as doctorname, questions.questionid,
	questions.questiontext,questions.question_seo,'' as counts,questions.createdate,ROW_NUMBER() OVER (PARTITION BY questions.questionid ORDER BY questions.createdate desc) AS RowNumber
	FROM answers INNER join users ON answers.userid = users.userid Right outer join questions ON answers.questionid = questions.questionid Inner JOIN
	questiontags ON questions.questionid = questiontags.questionid INNER JOIN 
	tag ON questiontags.tagid = tag.tagid where questions.status = @questionStatus and tag.tagname in(select value from SplitStringAsTable(@questionText,' ')) 
	)AS a
	WHERE a.RowNumber = 1

Declare @count varchar(10)
Declare @questionid int
Declare @id int

select @count=count(*) from @tempQuestion
set @id=1

	while(@count>0)
		BEGIN  
			select @questionid=questionid from @tempQuestion where id=@id
			declare @anscount varchar(50)
			select @anscount=count (*) from answers where answers.questionid=@questionid
			if(@anscount>0)
				BEGIN
				    update @tempQuestion set counts=@anscount-1 where id=@id
				END
			set @id+=1
			set @count-=1
		END
select doctorname ,questionid,questiontext,question_seo, case when counts = '' then 'No answers received' when counts = 0 then doctorname + ' answered'  else doctorname + ' answered and ' + counts + ' others answered ' END as counts  from @tempQuestion
END



GO


