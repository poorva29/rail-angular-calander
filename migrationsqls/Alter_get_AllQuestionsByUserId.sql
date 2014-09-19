/****** Object:  StoredProcedure [dbo].[get_AllQuestionsByUserId]    Script Date: 9/19/2014 12:47:16 PM ******/
DROP PROCEDURE [dbo].[get_AllQuestionsByUserId]
GO

/****** Object:  StoredProcedure [dbo].[get_AllQuestionsByUserId]    Script Date: 9/19/2014 12:47:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Yashwant>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Alter Log: Added question_seo Line no 31 and changes Regading
-- =============================================
CREATE PROCEDURE [dbo].[get_AllQuestionsByUserId]
		@Userid int	
AS
BEGIN
Declare @tempQuestion as table
(
   id int primary key identity(1,1),
   doctorname varchar(200),
   questionid int,
   questiontext varchar(200),
   counts varchar(100),
   createdate datetime,
   question_seo varchar(255)
 )
declare @anscount varchar(50)
declare @doctorname varchar(200)
Declare @count varchar(10)
Declare @questionid int
Declare @id int

   insert into @tempQuestion (questionid,questiontext,counts,createdate,question_seo)
							   select questions.questionid ,questions.questiontext as questiontext,
								'' as counts,questions. createdate,question_seo  from questions 
								where questions.userid = @Userid and status != 2 order by createdate DESC

select @count=count(*) from @tempQuestion
set @id=1

	while(@count>0)
	BEGIN  
		select @questionid=questionid from @tempQuestion where id=@id
			SET  @doctorname = ( SELECT top(1) ( 'Dr. '+ users.firstname+' '+users.lastname ) 
			                    FROM answers INNER JOIN users on answers.userid = users.userid 
								WHERE answers.questionid=@questionid order by answers.createdate )
		select @anscount=count (*) from answers where answers.questionid=@questionid and answers.questionid in(select questionid from questions where status=1)
		if(@anscount>0)
			BEGIN
				update @tempQuestion set counts=@anscount-1 where id=@id
			END
        update @tempQuestion set doctorname=@doctorname  where id=@id
		set @id+=1
		set @count-=1
	END
	select questionid, questiontext, case when counts = '' 
	then 'No answers received' when counts = 0 then doctorname + ' answered' 
	else doctorname + ' answered and ' + counts + ' others answered ' END as counts, createdate,question_seo from @tempQuestion 
END




GO


