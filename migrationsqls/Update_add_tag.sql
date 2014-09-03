GO
/****** Object:  StoredProcedure [dbo].[add_tag]    Script Date: 9/3/2014 10:53:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE[dbo].[add_tag]

   @newtag varchar(max),

   @questionid int = 0

as

BEGIN

declare @tagid as int

set @tagid = 0

	if not exists(select * from tag where tagname = @newtag) 

	begin

		insert into tag(tagname,tag_seo) values(@newtag, LOWER(dbo.RemoveSpecialChars(REPLACE(@newtag,' ','-')))) SET @tagid=@@IDENTITY

		if (@questionid != 0)

		begin

			INSERT INTO dbo.questiontags(questionid,tagid) select @questionId, @tagid;

		end

	end

END	

select @tagid




