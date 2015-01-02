
/****** Object:  StoredProcedure [dbo].[askmirai_doctor_Update]    Script Date: 12/30/2014 6:11:13 PM ******/
DROP PROCEDURE [dbo].[askmirai_doctor_Update]
GO

/****** Object:  StoredProcedure [dbo].[askmirai_doctor_Update]    Script Date: 12/30/2014 6:11:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ================================================
-- Author:		
-- Create date: 
-- Description:	
-- Update Log: changes for inserting uniqe name_seo
--	From line no 38 to 57
-- ================================================

CREATE PROCEDURE[dbo].[askmirai_doctor_Update]
	@uid int
,   @doctorid int
,   @firstname varchar(50)
,	@lastname varchar(50)
,	@gender int
,	@dateofbirth datetime
,	@email varchar(50)
,	@mobileno varchar(15)
,	@photopath varchar(256)
,	@username varchar(30)
,   @specialities docspecialities READONLY
,   @registrationnumber varchar(15)
,   @registrationcouncil int
,   @aboutme varchar(200)
,   @isemailverified bit
,   @countryId int
,   @photourl varchar(max)

AS
DECLARE @UserId as int
DECLARE @oldemail as varchar(50)
DECLARE @emailAvailable as BIT
DECLARE @name_seo as varchar(200)

SET @emailAvailable=1
SET @UserId=@doctorid
select @oldemail=email from users where userid=@doctorid

	SET @name_seo = LOWER(dbo.RemoveSpecialChars(REPLACE(LTRIM(RTRIM(@firstname)) + '-' +LTRIM(RTRIM(@lastname)), ' ', '-')))
		WHILE EXISTS(SELECT TOP 1 * FROM users WHERE name_seo=@name_seo)
		BEGIN
			declare @lastChar varchar(4)
			set	@lastChar = substring(@name_seo,len(@name_seo),len(@name_seo)+1)
			IF TRY_CONVERT(INT, @lastChar) IS NOT NULL
			begin
				set @name_seo = CONCAT(substring(@name_seo,0,len(@name_seo)),CAST(@lastChar as int)+1)
			end 
			ELSE
			begin
				set @name_seo = CONCAT(@name_seo,'-1')
			end
		END

if(@oldemail <> @email)
begin
	SET @emailAvailable = dbo.isEmailAvailable(@email,@doctorid)	 
	IF(@emailAvailable =1)
	BEGIN
		UPDATE dbo.users SET
	firstname = @firstname
,	lastname = @lastname
,	gender = @gender
,	dateofbirth = @dateofbirth
,	email = @email
,	mobileno = @mobileno
,	photopath = @photopath
,   registrationnumber=@registrationnumber
,   registrationcouncil=@registrationcouncil
,   aboutme=@aboutme
,   isemailverified=@isemailverified
,   countryid=@countryId
,   photourl=@photourl
,   name_seo=@name_seo
,	modified_on = GETDATE()
,	modified_by = @uid
	WHERE
	userid = @doctorid	
		DELETE FROM dbo.doctorspecialities WHERE doctorid= @doctorid;
		INSERT INTO dbo.doctorspecialities (doctorid,specialityid,created_by,created_on,modified_by,modified_on) 
		select @doctorid,specility_id,@uid as created_by,GETDATE() as created_on,@uid as modified_by,GETDATE() as modified_on
		from @specialities; 
	END
End
else
begin
UPDATE dbo.users SET
	firstname = @firstname
,	lastname = @lastname
,	gender = @gender
,	dateofbirth = @dateofbirth
,	email = @email
,	mobileno = @mobileno
,	photopath = @photopath

,   registrationnumber=@registrationnumber
,   registrationcouncil=@registrationcouncil
,   aboutme=@aboutme
,   countryid=@countryId
,   photourl=@photourl
,   name_seo=@name_seo
,	modified_on = GETDATE()
,	modified_by = @uid
	WHERE
	userid = @doctorid	
		DELETE FROM dbo.doctorspecialities WHERE doctorid= @doctorid;
		INSERT INTO dbo.doctorspecialities (doctorid,specialityid,created_by,created_on,modified_by,modified_on) 
		select @doctorid,specility_id,@uid as created_by,GETDATE() as created_on,@uid as modified_by,GETDATE() as modified_on
		from @specialities; 
end
SELECT @emailAvailable as EmailAvailable,@UserId as UserId

GO


