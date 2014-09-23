/****** Object:  StoredProcedure [dbo].[get_allTagsWithCountOfAnsweredQuestions]    Script Date: 9/23/2014 7:07:42 PM ******/
DROP PROCEDURE [dbo].[get_allTagsWithCountOfAnsweredQuestions]
GO

/****** Object:  StoredProcedure [dbo].[get_allTagsWithCountOfAnsweredQuestions]    Script Date: 9/23/2014 7:07:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<priyanka priyadarshni>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Update Log : retun with tag_seo
-- =============================================
CREATE PROCEDURE [dbo].[get_allTagsWithCountOfAnsweredQuestions]
AS
BEGIN
Declare @tempTags as table
(
   id int primary key identity(1,1),
   tagid int,
   tagname varchar(200),
   tag_seo varchar(200),
   counts varchar(200)
  )
   insert into @tempTags
   SELECT tagid as tagid,tagname as tagname,tag_seo,'' as counts from tag

Declare @count varchar(10)
Declare @id int
Declare @tagid int

select @count=count(*) from @tempTags
set @id=1

	while(@count>0)
		BEGIN  
			select @tagid=tagid from @tempTags where id=@id
			declare @anscount int
			select @anscount=count(questionid) from (Select distinct questionid from (select questiontags.questionid from tag inner join questiontags
				on tag.tagid = questiontags.tagid inner join questions on questions.questionid = questiontags.questionid
				where tag.tagid=@tagid and questions.status = 1 and questiontags.questionid in(select questionid from answers)
				union
				select questions.questionid from questions where questions.questiontext like ('%' + (select tagname from @tempTags where id=@id) + '%') and questions.status = 1 and questions.questionid in(select questionid from answers)) as allQuestion) as questionanswerCount
			
			update @tempTags set counts=@anscount where id=@id
			set @id+=1
			set @count-=1
		END
select * from @tempTags
END

--BEGIN
--Declare @tempTags as table
--(
--   id int primary key identity(1,1),
--   tagid int,
--   tagname varchar(200),
--   counts varchar(200)
--  )
--   insert into @tempTags
--   SELECT tagid as tagid,tagname as tagname,'' as counts from tag

--Declare @count varchar(10)
--Declare @id int
--Declare @tagid int

--select @count=count(*) from @tempTags
--set @id=1

--	while(@count>0)
--		BEGIN  
--			select @tagid=tagid from @tempTags where id=@id
--			declare @anscount varchar(50)
--			select @anscount=count(questionid) from (select questionid from tag inner join questiontags on tag.tagid = questiontags.tagid where tag.tagid=@tagid and questionid in(select questionid from answers)) as questionanswerCount
--			update @tempTags set counts=@anscount where id=@id
--			set @id+=1
--			set @count-=1
--		END
--select * from @tempTags
--END





GO


