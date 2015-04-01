
/****** Object:  StoredProcedure [dbo].[get_AllQuestions]    Script Date: 3/30/2015 6:12:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Mrunal>
-- Create date: <30-12-2013>
-- Description:	<Description,,>
-- Update Log
-- Added new condition for display unanswered to admin from line no 47 to 6
-- Added question_seo Line no 50,67,86 and changes Regading
-- Ravindra Kale(30/3/2015) applied filter for econsult question, changes in line no (51,69,88)
-- =============================================
--[dbo].[get_AllQuestions] 50,1,0
ALTER PROCEDURE [dbo].[get_AllQuestions]
		@RecordSize int,
		@flag int,
		@lastQuestionNo int	
AS
BEGIN
DECLARE @Index INT;
DECLARE @PageSize INT;
Declare @To INT;
DECLARE @strQuery varchar(max)

SET @Index = @lastQuestionNo;
SET @PageSize =@RecordSize;
set @To=@Index + @PageSize

 --0 for unassign
 if(@flag=1)---Show all
 Begin  	 select * from 
	           (SELECT  ROW_NUMBER() OVER (ORDER BY createdate desc) as QuestionsRowNumber,* from
				(SELECT  ROW_NUMBER() OVER (PARTITION BY  Q.questionid ORDER BY createdate desc) as RowNumber,
				 Q.questionid ,              
	            Q.questiontext as questiontext,
				dc.docquestionid,
				status,
				questionstatus=case when dc.docquestionid IS NULL THEN 0 ELSE 1 End,						   
				Q. createdate,
				Q.question_seo from
				questions Q with(nolock) 		   
				left outer join doctorquestions dc with(nolock) on Q.questionid = dc.questionid 
				where status = 0 or status=1 and (question_type != 1 or question_type is null)) as question
				where question.RowNumber = 1)as a			
				WHERE a.QuestionsRowNumber BETWEEN @Index  AND  @To
END
ELSE IF(@flag=2)--for unAnswerd Questions
BEGIN
	  	 select * from 
	           (SELECT  ROW_NUMBER() OVER (ORDER BY createdate desc) as QuestionsRowNumber,* from
				(SELECT  ROW_NUMBER() OVER (PARTITION BY  Q.questionid ORDER BY createdate desc) as RowNumber,
				 Q.questionid ,              
	            Q.questiontext as questiontext,
				dc.docquestionid,
				status,
				questionstatus=case when dc.docquestionid IS NULL THEN 0 ELSE 1 End,						   
				Q. createdate,
				Q.question_seo from
				questions Q with(nolock) 		   
				left outer join doctorquestions dc with(nolock) on Q.questionid = dc.questionid
				where status = 0 or status=1 and (question_type != 1 or question_type is null) and Q.questionid not in(select questionid from answers)) as question
				where question.RowNumber = 1 and docquestionid is not null)as a			
				where a.QuestionsRowNumber BETWEEN @Index AND @To
END
ELSE
BEGIN
--not Assigned
     SELECT  * from
	           (SELECT  ROW_NUMBER() OVER (ORDER BY createdate desc) as QuestionsRowNumber,* from
				(SELECT  ROW_NUMBER() OVER (PARTITION BY  Q.questionid ORDER BY createdate desc) as RowNumber,
				Q.questionid ,              
	            Q.questiontext as questiontext,
				status,
				dc.docquestionid,
				questionstatus=case when dc.docquestionid is null then 0 else 1 End,						   
				Q. createdate,
				Q.question_seo from
				questions Q with(nolock) 		   
				left outer join doctorquestions dc with(nolock) on Q.questionid = dc.questionid
				 where dc.docquestionid is null and Q.status=0 and (question_type != 1 or question_type is null)) as question
				 where question.RowNumber = 1)as a				
				WHERE a. QuestionsRowNumber BETWEEN @Index  AND  @To	
END
END





