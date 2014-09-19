/****** Object:  StoredProcedure [dbo].[getFeedListByLastQuestionNo]    Script Date: 9/19/2014 12:52:06 PM ******/
DROP PROCEDURE [dbo].[getFeedListByLastQuestionNo]
GO

/****** Object:  StoredProcedure [dbo].[getFeedListByLastQuestionNo]    Script Date: 9/19/2014 12:52:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Alter Log: Added question_seo Line no 63 and changes Regading
-- =============================================
CREATE PROCEDURE [dbo].[getFeedListByLastQuestionNo]
	@lastQuestionNo int
,	@RecordSize int,
    @userId int,
	@myFeed bit 
,   @questionStatus int
AS
DECLARE @Index INT;
DECLARE @PageSize INT;

SET @Index = @lastQuestionNo;
SET @PageSize =@RecordSize;

Declare @tempfeedlist as table
(
	 id int primary key identity(1,1),
	 questionid int,
	 gender int,
	 QuestionsRowNumber int,  
	 quetionaskedby int,
	 status int,
	 askeddate datetime,
	 questiontext varchar(max),
	 answerid int,
	 docid varchar(max),
	 docLastname varchar(50),
	 doctEmail varchar(50),
	 docmobileno varchar(15),
	 answerreplyedby varchar(max),
	 photourl varchar(max),
	 doctorimg varchar(max),
	 title varchar(max),
	 answertext varchar(max),
	 answerimg varchar(max),
	 replyeddate datetime,
	 ispatientthank bit DEFAULT 0,
	 isendorse bit DEFAULT 0,
	 thanxcount int,
	 endorsecount int,
	 isdocconnectuser bit,
	 name_seo varchar(150),
	 question_seo varchar(255)
)

if(@myFeed  =  1)
begin
insert into @tempfeedlist
	 select questionid, gender, QuestionsRowNumber,quetionaskedby,status,askeddate,questiontext,answerid,userid,docLastname,doctEmail,docmobileno,answerreplyedby,photourl,doctorimg,title,answertext,answerimg,replyeddate,ispatientthank,'' ,'','' ,isdocconnectuser,name_seo,question_seo
	 from  (SELECT  ROW_NUMBER() OVER (ORDER BY answers.createdate desc) as QuestionsRowNumber,
		 questions.questionid,users.gender as gender, questions.userid AS quetionaskedby,
				questions.status, questions.createdate AS askeddate,questions.questiontext, answers.answerid, answers.userid,users.lastname as docLastname ,users.email as doctEmail,users.mobileno as docmobileno ,
				(users.firstname+' '+users.lastname) AS answerreplyedby,users.photourl as photourl, users.photopath as doctorimg, answers.title, answers.answertext, answers.image as answerimg , 
				answers.createdate AS replyeddate, 0 as ispatientthank, users.isdocconnectuser, users.name_seo,questions.question_seo
				FROM questions INNER JOIN
				answers ON questions.questionid = answers.questionid inner join users on users.userid = answers.userid
				 where  answers.userid= @userId and questions.status = @questionStatus)  as questionAnswers    WHERE QuestionsRowNumber BETWEEN  @Index+1 AND @Index + @PageSize 
end
else
begin
insert into @tempfeedlist
	 select questionid,gender,QuestionsRowNumber,quetionaskedby,status,askeddate,questiontext,answerid,userid,docLastname,doctEmail,docmobileno,answerreplyedby,photourl,doctorimg,title,answertext,answerimg,replyeddate,ispatientthank,'' ,'','' ,isdocconnectuser,name_seo,question_seo
	 from (SELECT  ROW_NUMBER() OVER (ORDER BY replyeddate desc) as QuestionsRowNumber,*
		FROM  (SELECT questions.questionid,users.gender, questions.userid AS quetionaskedby,
				questions.status, questions.createdate AS askeddate,questions.questiontext, answers.answerid, answers.userid,users.lastname as docLastname ,users.email as doctEmail,users.mobileno as docmobileno ,
				(users.firstname+' '+users.lastname) AS answerreplyedby,users.photourl as photourl, users.photopath as doctorimg, answers.title, answers.answertext, answers.image as answerimg , 
				answers.createdate AS replyeddate, 0 as ispatientthank, users.isdocconnectuser, users.name_seo,questions.question_seo,
				ROW_NUMBER() OVER (PARTITION BY questions.questionid ORDER BY answers.createdate desc)  AS RowNumber
				FROM questions INNER JOIN
				answers ON questions.questionid = answers.questionid inner join users on users.userid = answers.userid
				where questions.status = @questionStatus) AS a
		WHERE   a.RowNumber = 1) as questionAnswers  WHERE QuestionsRowNumber BETWEEN  @Index+1 AND @Index + @PageSize 
end

Declare @count int 
DECLARE @answerid int
Declare @id int
Declare @isuserthanked bit=0	
Declare @countThanx int
Declare @countEndorse int
set @id=1
select @count=count(*) from @tempfeedlist
while(@count>0)
  BEGIN
     select  @answerid=answerid from @tempfeedlist where id=@id
	 print @answerid
	 SELECT @countThanx = COUNT(*) from patientthanks where answerid = @answerid
	 SELECT @countEndorse = COUNT(*) from doctoragrees where answerid = @answerid
        if exists(select * from patientthanks where answerid=@answerid and userid=@userId)
		  BEGIN
				set @isuserthanked=1
				update @tempfeedlist set ispatientthank=@isuserthanked where id=@id
		  END
		if exists(select * from doctoragrees where answerid=@answerid and doctorid=@userid)
		 BEGIN
				set @isuserthanked=1
				update @tempfeedlist set isendorse = @isuserthanked where id=@id
		  END
		update @tempfeedlist set thanxcount= @countThanx ,  endorsecount = @countEndorse where id=@id
		Set @count-=1
		set @id+=1;		
  END
  select * from @tempfeedlist





GO


