/****** Object:  StoredProcedure [dbo].[Sp_Import_Patients]    Script Date: 12/30/2014 6:29:21 PM ******/
DROP PROCEDURE [dbo].[Sp_Import_Patients]
GO

/****** Object:  StoredProcedure [dbo].[Sp_Import_Patients]    Script Date: 12/30/2014 6:29:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ravindra Kale
-- Create date: 16/9/2014
-- Description:	insert data into Uses Table
-- Update Log:
-- =============================================
CREATE PROCEDURE [dbo].[Sp_Import_Patients]
AS
BEGIN
	declare @fname varchar(50)
	declare @lname varchar(50)
	declare @phno varchar(50)
	declare db_cursor Cursor for select firstname,lastname,mobileno from Sheet1$
	open db_cursor
	fetch next from db_cursor into @fname,@lname,@phno

	WHILE @@FETCH_STATUS = 0  
	BEGIN   
       insert into users(firstname,lastname,mobileno,email,usertype,registrationdate,created_by,modified_by,modified_on) 
	   values(@fname,@lname,@phno,'',2,GETDATE(),0,0,GETDATE())
       	fetch next from db_cursor into @fname,@lname,@phno
	END   
	CLOSE db_cursor   
	DEALLOCATE db_cursor
	delete from Sheet1$
END

GO


