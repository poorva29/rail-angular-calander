/****** Object:  StoredProcedure [dbo].[getQuestionListByDoctorid]    Script Date: 9/19/2014 12:44:00 PM ******/
DROP PROCEDURE [dbo].[getQuestionListByDoctorid]
GO

/****** Object:  StoredProcedure [dbo].[getQuestionListByDoctorid]    Script Date: 9/19/2014 12:44:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =================================================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Alter Log: Added question_seo Line no 33 and changes Regading
-- =================================================================
CREATE PROCEDURE [dbo].[getQuestionListByDoctorid]
    @userID int
AS

Declare @tempfeedlist as table
(
 id int primary key identity(1,1),
 questionid int,  
 QuestionsRowNumber int,
 questiontext varchar(max),
 answertext  varchar(max),
 asignedDoc int,
 answeredby int,
 createdate datetime,
 question_seo varchar(225)
)

insert into @tempfeedlist
select questionid,QuestionsRowNumber,questiontext,answertext ,asignedDoc, answeredby ,createdate,question_seo
 from (SELECT  ROW_NUMBER() OVER (ORDER BY questionid asc) as QuestionsRowNumber,*
    FROM  (select questions.questionid, questions.questiontext, doctorquestions.doctorid as asignedDoc,
                             answers.answertext, answers.userid  as answeredby, questions.createdate,questions.question_seo,
							 ROW_NUMBER() OVER (PARTITION BY questions.questionid ORDER BY answers.createdate desc)  AS RowNumber from questions inner join
                             doctorquestions on questions.questionid =  doctorquestions.questionid  
                             left outer join answers on questions.questionid = answers.questionid where doctorquestions.doctorid = @userID and questions.status = 1
			) AS a
    WHERE   a.RowNumber = 1 ) as questionAnswers where asignedDoc = @userID order by createdate desc


Declare @count int 
DECLARE @answertext varchar(max)
Declare @id int
Declare @questionid int
set @id=1
select @count=count(*) from @tempfeedlist
while(@count>0)
  BEGIN
	 select  @questionid = questionid from @tempfeedlist where id=@id
     select  @answertext = answertext from answers where userid = @userID and questionid = @questionid
	 if   @answertext IS NOT NULL 
	 begin
		update @tempfeedlist set answertext =  @answertext, answeredby = @userID  where id=@id   
	end
		Set @count-=1
		set @id+=1;		
		set @answertext = null
  END
select * from @tempfeedlist



GO


