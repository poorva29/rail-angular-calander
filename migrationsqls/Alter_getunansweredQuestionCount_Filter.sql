
/****** Object:  StoredProcedure [dbo].[getunansweredQuestionCount]    Script Date: 3/30/2015 5:46:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Ravindra Kale(30/3/2015) applied filter for econsult question,changes in line no (18,19)

ALTER PROCEDURE[dbo].[getunansweredQuestionCount]--[dbo].[getunansweredQuestionCount] 4 
  @userid as int
AS
BEGIN
select count(dq.questionid) as NoOfquestions 
from doctorquestions as dq with(Nolock) left outer join answers as a with(Nolock) 
on dq.questionid=a.questionid and dq.doctorid=a.userid
inner join questions q on dq.questionid = q.questionid
where a.questionid is Null and a.userid is null and dq.doctorid=@userid and q.status = 1 and (q.question_type != 1 or q.question_type is null)
End



