/****** Object:  StoredProcedure [dbo].[doctor_Insert]    Script Date: 12/30/2014 6:19:01 PM ******/
DROP PROCEDURE [dbo].[doctor_Insert]
GO

/****** Object:  StoredProcedure [dbo].[doctor_Insert]    Script Date: 12/30/2014 6:19:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- ================================================
-- Author:		
-- Create date: 
-- Description:	
-- Update Log: changes for inserting uniqe name_seo
--	From line no 54 to 57
-- ================================================

CREATE PROCEDURE[dbo].[doctor_Insert]
@firstname varchar(50)
,	@lastname varchar(50)
,	@email varchar(50)
,	@mobileno varchar(15)
,	@secondaryemail varchar(50)
,	@secondarymobileno varchar(15)
,	@gender int
,	@dateofbirth datetime
,	@totalexperience int
,	@photopath varchar(256)
,	@password varchar(30)
,	@status int
,	@registrationdate datetime
,	@registrationvalidity datetime
,	@Monday bit
,	@Tuesday bit
,	@Wednesday bit
,	@Thursday bit
,	@Friday bit
,	@Saturday bit
,	@Sunday bit
,   @specialities docspecialities READONLY
,	@location dcdoctorlocations READONLY
,   @qualification docqualification READONLY 
,	@usertype varchar(10)='Doctor'
,	@userid INT =0
,	@createdBy INT =0
,   @isgoogleaccount bit
,   @isfacebookaccount bit
,   @countrycode1 int
,   @countrycode2 int
,   @photourl varchar(max)
AS
Declare @isGoogleORFacebook bit
DECLARE @emailAvailable as BIT
declare @name_seo as varchar(200)
set @isGoogleORFacebook =0
SET @emailAvailable=1
SET @emailAvailable = dbo.isEmailAvailable(@email,@userid)	 
IF(@emailAvailable =1)
BEGIN
   IF (@userid=0)
	BEGIN
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
		INSERT INTO dbo.users (
			firstname
		,	lastname
		,	email
		,	mobileno
		,   secondaryemail
		,	secondarymobileno
		,	gender
		,	dateofbirth
		,	totalexperience
		,	photopath
		,	password
		,	registrationdate
		,	created_by
		,	modified_by
		,	modified_on
		,   usertype
		,   isdocconnectuser
		,   isgoogleaccount
		,   isfacebookaccount
		,   countrycode1
		,   countrycode2
		,   photourl
		,   name_seo
		)
		VALUES(
			@firstname
		,	@lastname
		,	@email
		,	@mobileno
		,	@secondaryemail
		,	@secondarymobileno
		,	@gender
		,	@dateofbirth
		,	@totalexperience
		,	@photopath
		,	@password
		,	@registrationdate
		,	@createdBy
		,	@createdBy
		,	GETDATE()
		,   1
		,   1
		,   @isgoogleaccount
		,   @isfacebookaccount
		,   @countrycode1
		,   @countrycode2
		,   @photourl	
		,   @name_seo
		)
		SET @userid = @@IDENTITY

		 if(@isgoogleaccount=1 or @isfacebookaccount =1 )
		BEGIN
		  set @isGoogleORFacebook = 1
		  update users set status = 1, isemailverified=1 where userid = @userid
		End
		INSERT INTO dbo.doctorspecialities (doctorid,specialityid,created_by,modified_by,created_on,modified_on)
		select @userid,specility_id,@userid,@userid,GETDATE(),GETDATE() from @specialities; 

		INSERT INTO dbo.doctorqualification (doctorid,degreeid,created_by,modified_by,created_on,modified_on)
		select @userid, degree_id,@userid,@userid,GETDATE(),GETDATE() from @qualification;

		INSERT INTO dbo.doctorlocations (doctorid,countryid,stateid,cityid,locationid,hospitalid,address,telephone,isprimary,hospitalname,created_by,modified_by,created_on,modified_on)
		select @userid,country_id,state_id,city_id,loction_id, hospital_id,address,telephone_no,isprimary,hospital_name,@userid,@userid,GETDATE(),GETDATE() from @location;

	END
END
SELECT @emailAvailable as EmailAvailable,@userid as UserId, @isGoogleORFacebook as SignInUsingGoogleORFacebook 

GO


