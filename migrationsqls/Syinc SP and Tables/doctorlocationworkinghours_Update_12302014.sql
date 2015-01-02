/****** Object:  StoredProcedure [dbo].[doctorlocationworkinghours_Update]    Script Date: 12/30/2014 6:24:57 PM ******/
DROP PROCEDURE [dbo].[doctorlocationworkinghours_Update]
GO

/****** Object:  StoredProcedure [dbo].[doctorlocationworkinghours_Update]    Script Date: 12/30/2014 6:24:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[doctorlocationworkinghours_Update]
	@uid int
,	@docLocid int
,	@docworkingid int
,	@fromtime varchar(4)
,	@totime varchar(4)
,	@Monday bit=0
,	@Tuesday bit=0
,	@Wednesday bit=0
,	@Thursday bit=0
,	@Friday bit=0
,	@Saturday bit=0
,	@Sunday bit=0
as




IF (@docworkingid <>0)
		BEGIN
			UPDATE doctorlocationworkinghours 
			SET doclocationid = @docLocid,
			    fromtime = @fromtime,
			    totime = @totime,
			    Monday= @Monday, 
				Tuesday=@Tuesday ,
				Wednesday=@Wednesday ,
				Thursday=@Thursday ,
				Friday=@Friday ,
				Saturday=@Saturday ,
				Sunday=@Sunday ,
				modified_by = @uid ,
				modified_on = GETDATE()
        WHERE doclocworkinghoursid = @docworkingid
		END
ELSE
		BEGIN
			INSERT INTO dbo.doctorlocationworkinghours (
				doclocationid
			,	fromtime
			,	totime
			,	Monday
			,	Tuesday
			,	Wednesday
			,	Thursday
			,	Friday
			,	Saturday
			,	Sunday
			,	created_by
			,	modified_by
			,	created_on
			,	modified_on
			)
			VALUES (
				@docLocid
			,	@fromtime
			,	@totime
			,	@Monday
			,	@Tuesday
			,	@Wednesday
			,	@Thursday
			,	@Friday
			,	@Saturday
			,	@Sunday
			,	@uid
			,	@uid
			,	GETDATE()
			,	GETDATE()
			)
		End
	




GO


