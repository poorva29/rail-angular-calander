-- =============================================
-- Author:		Pavan Shevle
-- Create date: 12/09/2014
-- Alter Log: Added name_seo
-- =============================================
ALTER PROCEDURE [dbo].[getsimilarQuestionbyText]
	@questionText varchar(250)
,   @questionStatus int
AS
select questionid,QuestionsRowNumber,quetionaskedby,status,askeddate,questiontext,answerid,userid, isdocconnectuser,answerreplyedby,name_seo,doctorimg,gender,title,answertext,answerimg,replyeddate
	 from (SELECT  ROW_NUMBER() OVER (ORDER BY askeddate desc) as QuestionsRowNumber,*
		FROM  (SELECT questions.questionid, questions.userid AS quetionaskedby,
				questions.status, questions.createdate AS askeddate,questions.questiontext, answers.answerid, answers.userid, users.isdocconnectuser ,
				(users.firstname+' '+users.lastname) AS answerreplyedby,users.name_seo,(users.photourl + users.photopath) as doctorimg,users.gender as gender, answers.title, answers.answertext, answers.image as answerimg , 
				answers.createdate AS replyeddate, 0 as ispatientthank,
				ROW_NUMBER() OVER (PARTITION BY questions.questionid ORDER BY answers.createdate desc)  AS RowNumber
				FROM questions INNER JOIN
				answers ON questions.questionid = answers.questionid inner join users on users.userid = answers.userid
				 where questions.status = @questionStatus) AS a
		WHERE   a.RowNumber = 1  ) as questionAnswers WHERE questiontext like @questionText 



