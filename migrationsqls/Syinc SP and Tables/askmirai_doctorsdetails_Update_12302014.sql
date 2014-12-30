/****** Object:  StoredProcedure [dbo].[askmirai_doctorsdetails_Update]    Script Date: 12/30/2014 6:13:08 PM ******/
DROP PROCEDURE [dbo].[askmirai_doctorsdetails_Update]
GO

/****** Object:  StoredProcedure [dbo].[askmirai_doctorsdetails_Update]    Script Date: 12/30/2014 6:13:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[askmirai_doctorsdetails_Update]
	@uid int
,	@docid int
,	@doctordetailsid int	
,	@certification varchar(150)
,	@society varchar(150)

AS
IF (@doctordetailsid is not NULL)
	BEGIN
		UPDATE doctorsdetails SET certification= @certification,society=@society, doctorid = @docid,modified_by = @uid,modified_on = GETDATE()
		WHERE doctordetailsid  = @doctordetailsid
	END
ELSE
    BEGIN
		INSERT INTO doctorsdetails 
		   (doctorid
		   ,certification
		   ,society
		   ,created_by
		   ,created_on
		   ,modified_by
		   ,modified_on)
		VALUES 
			(@docid
		   , @certification
		   , @society
		   , @uid
		   , GETDATE()
		   , @uid
		   , GETDATE())
   End


   
   



GO


