/****** Object:  StoredProcedure [dbo].[doctorsdetails_Update]    Script Date: 12/30/2014 6:25:44 PM ******/
DROP PROCEDURE [dbo].[doctorsdetails_Update]
GO

/****** Object:  StoredProcedure [dbo].[doctorsdetails_Update]    Script Date: 12/30/2014 6:25:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[doctorsdetails_Update]
	@uid int
,	@doctorid int
,	@doctordetailsid int	
,	@selectedtype int
,	@details varchar(1000)

AS
IF (@doctordetailsid is not NULL)
		BEGIN
			UPDATE doctorsdetails SET type = @selectedtype,details=@details, doctorid = @doctorid ,modified_by = @uid,modified_on = GETDATE() WHERE doctordetailsid = @doctordetailsid
		END
		ELSE
		BEGIN
			INSERT INTO dbo.doctorsdetails 
			(doctorid
			,	type
			,	details
			,	created_by
			,	modified_by
			,	created_on
			,	modified_on)
			VALUES 
				(@doctorid
			,	@selectedtype
			,	@details
			,	@uid
			,	@uid
			,	GETDATE()
			,	GETDATE())
		End



GO


