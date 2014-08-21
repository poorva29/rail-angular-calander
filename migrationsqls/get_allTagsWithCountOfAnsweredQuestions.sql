USE [AsquareMirai]
GO
/****** Object:  StoredProcedure [dbo].[get_AllQuestionsByTag]    Script Date: 8/20/2014 11:03:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<yashwant singh>
-- Create date: <8/20/214,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[get_allTagsWithCountOfAnsweredQuestions]
AS
BEGIN
Declare @tempTags as table
(
   id int primary key identity(1,1),
   tagid int,
   tagname varchar(200),
   counts varchar(200)
  )
   insert into @tempTags
   SELECT tagid as tagid,tagname as tagname,'' as counts from tag

Declare @count varchar(10)
Declare @id int
Declare @tagid int

select @count=count(*) from @tempTags
set @id=1

	while(@count>0)
		BEGIN  
			select @tagid=tagid from @tempTags where id=@id
			declare @anscount varchar(50)
			select @anscount=count(questionid) from (select questionid from tag inner join questiontags on tag.tagid = questiontags.tagid where tag.tagid=@tagid and questionid in(select questionid from answers)) as questionanswerCount
			update @tempTags set counts=@anscount where id=@id
			set @id+=1
			set @count-=1
		END
select * from @tempTags
END
