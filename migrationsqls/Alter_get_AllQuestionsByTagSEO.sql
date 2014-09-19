/****** Object:  StoredProcedure [dbo].[get_AllQuestionsByTagSEO]    Script Date: 9/19/2014 12:56:40 PM ******/
DROP PROCEDURE [dbo].[get_AllQuestionsByTagSEO]
GO

/****** Object:  StoredProcedure [dbo].[get_AllQuestionsByTagSEO]    Script Date: 9/19/2014 12:56:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================

-- Author:		<priyanka priyadarshni>

-- Create date: <Create Date,,>

-- Description:	<Description,,>

-- Alter Log: Added question_seo Line no 63 and changes Regading
-- =============================================

CREATE PROCEDURE [dbo].[get_AllQuestionsByTagSEO]

		@TagSEOText varchar(200)

   ,	@questionStatus int		

AS

BEGIN

Declare @tempQuestion as table
(
   id int primary key identity(1,1),

   QuestionsRowNumber int,

   doctorname varchar(200),

   questionid int,

   docImage varchar(200),

   docImageUrl varchar(200),

   questiontext varchar(200),

   counts varchar(100),

   createdate datetime,

   answertext varchar(500),

   ansImage varchar(200),

   RowNumber int,

   name_seo varchar(150),

   question_seo varchar(255)
)

   insert into @tempQuestion
   SELECT top 10 ROW_NUMBER() OVER (ORDER BY createdate asc) as QuestionsRowNumber,*
	FROM ( SELECT 'Dr. ' + users.firstname + ' ' + users.lastname as doctorname, questions.questionid,users.photopath as docImage,users.photourl as docImageUrl,
	questions.questiontext,'' as counts,questions.createdate, answers.answertext,answers.image as ansImage,
	ROW_NUMBER() OVER (PARTITION BY questions.questionid ORDER BY questions.createdate desc) AS RowNumber,users.name_seo,questions.question_seo
	FROM answers INNER join users ON answers.userid = users.userid inner join questions ON answers.questionid = questions.questionid Inner JOIN
	questiontags ON questions.questionid = questiontags.questionid INNER JOIN 
	tag ON questiontags.tagid = tag.tagid where questions.status = @questionStatus and tag.tag_seo = @TagSEOText
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
select doctorname ,questionid,questiontext, case when counts = '' then 'No answers received' when counts = 0 then doctorname + ' answered'  else doctorname + ' answered and ' + counts + ' others answered ' END as counts, answertext, docImage,ansImage,docImageUrl,name_seo,question_seo from @tempQuestion
END



GO


