/****** Object:  StoredProcedure [dbo].[UpdateProfile]    Script Date: 12/30/2014 6:18:13 PM ******/
DROP PROCEDURE [dbo].[UpdateProfile]
GO

/****** Object:  StoredProcedure [dbo].[UpdateProfile]    Script Date: 12/30/2014 6:18:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[UpdateProfile]
@userId int,
@firstName varchar(100),
@LastName varchar(100),
@mobile varchar(100),
@Email varchar(100),
@IsNewEmail varchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @EXIST INT
	DECLARE @ISNEW INT

	SET @ISNEW = CONVERT(INT, @ISNEWEMAIL)


	IF(@ISNEW = 1)
	BEGIN
		SELECT @EXIST = COUNT(*) FROM USERS WHERE EMAIL = @EMAIL 

		IF(@EXIST = 0)
		BEGIN
			UPDATE USERS SET FIRSTNAME = @FIRSTNAME, LASTNAME = @LASTNAME, MOBILENO = @MOBILE, EMAIL = @EMAIL, ISEMAILVERIFIED = 0,modified_by = @userId,modified_on  = GETDATE()
			WHERE USERID = @USERID
			SELECT * FROM USERS WHERE USERID = @USERID
		END
		ELSE
		BEGIN
			SELECT 'Email Already Exist' as msg
		END
	END

	ELSE

	BEGIN
		UPDATE USERS SET FIRSTNAME = @FIRSTNAME, LASTNAME = @LASTNAME, MOBILENO = @MOBILE, EMAIL = @EMAIL, modified_by = @userId,modified_on = GETDATE()
		WHERE USERID = @USERID
	END
END


GO


