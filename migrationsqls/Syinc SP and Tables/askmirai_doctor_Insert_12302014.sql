
/****** Object:  StoredProcedure [dbo].[askmirai_doctor_Insert]    Script Date: 12/30/2014 6:09:21 PM ******/
DROP PROCEDURE [dbo].[askmirai_doctor_Insert]
GO

/****** Object:  StoredProcedure [dbo].[askmirai_doctor_Insert]    Script Date: 12/30/2014 6:09:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- ================================================
-- Author:		
-- Create date: 
-- Description:	
-- Update Log: changes for inserting uniqe name_seo
--	From line no 41 to 60
-- ================================================

CREATE PROCEDURE[dbo].[askmirai_doctor_Insert]
    @firstname varchar(50)
,	@lastname varchar(50)
,	@gender int
,	@dateofbirth datetime
,	@email varchar(50)
,	@mobileno varchar(15)
,	@password varchar(70)
,	@photopath varchar(256)
,	@username varchar(30)
,	@status int
,	@usertype varchar(10)
,	@registrationdate datetime
,   @specialities docspecialities READONLY
,	@location doclocations READONLY
,   @registrationnumber varchar(15)
,   @registrationcouncil int
,   @aboutme varchar(200)
,	@userid INT =0
,   @countryId int 
,   @photourl varchar(max)
AS
DECLARE @emailAvailable as BIT
SET @emailAvailable=1
declare @name_seo as varchar(200)
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
		INSERT INTO dbo.users(
			firstname
		,	lastname
		,	gender
		,	dateofbirth
		,	email
		,	mobileno
		,	password
		,	photopath
		,	username
		,   status
		,   usertype
		,	registrationdate
		,   registrationnumber
		,   registrationcouncil
		,   aboutme
		,   countryid
		,   photourl
		,   name_seo
		,	modified_on
		)
		VALUES(
			@firstname
		,	@lastname
		,	@gender
		,	@dateofbirth
		,	@email
		,	@mobileno	
	    ,	@password	
		,	@photopath
		,	@username
	    ,   @status
		,   @usertype
		,	@registrationdate
		,   @registrationnumber
		,   @registrationcouncil
		,   @aboutme
		,   @countryId
		,	@photourl
		,   @name_seo
		,	@registrationdate
		)
		
		SET @userid = @@IDENTITY

		INSERT INTO dbo.doctorspecialities (doctorid,specialityid,created_by,created_on,modified_by,modified_on) 
		select @userid,specility_id,@userid as created_by,GETDATE() as created_on,@userid as modified_by,GETDATE() as modified_on 
		from @specialities; 

		update users set photopath=cast(@userid as varchar) + @photopath where userid=@userid
	END
END
SELECT @emailAvailable as EmailAvailable,@userid as UserId


GO


