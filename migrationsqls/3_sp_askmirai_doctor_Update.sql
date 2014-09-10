USE [AsquareMirai]
GO
/****** Object:  StoredProcedure [dbo].[askmirai_doctor_Update]    Script Date: 10/Sep/14 11:22:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE[dbo].[askmirai_doctor_Update]
    @doctorid int
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
SET @emailAvailable=1
SET @UserId=@doctorid
select @oldemail=email from users where userid=@doctorid

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
,   name_seo=LOWER(dbo.RemoveSpecialChars(REPLACE(LTRIM(RTRIM(@firstname)) + '-' +LTRIM(RTRIM(@lastname)), ' ', '-')))
	WHERE
	userid = @doctorid	
		DELETE FROM dbo.doctorspecialities WHERE doctorid= @doctorid;
		INSERT INTO dbo.doctorspecialities (doctorid,specialityid) select @doctorid,specility_id from @specialities; 
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
,   name_seo=LOWER(dbo.RemoveSpecialChars(REPLACE(LTRIM(RTRIM(@firstname)) + '-' +LTRIM(RTRIM(@lastname)), ' ', '-')))
	WHERE
	userid = @doctorid	
		DELETE FROM dbo.doctorspecialities WHERE doctorid= @doctorid;
		INSERT INTO dbo.doctorspecialities (doctorid,specialityid) select @doctorid,specility_id from @specialities; 
end
SELECT @emailAvailable as EmailAvailable,@UserId as UserId




