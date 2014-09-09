USE [AsquareMirai]
GO
/****** Object:  StoredProcedure [dbo].[askmirai_doctor_Insert]    Script Date: 09/Sep/14 6:01:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE[dbo].[askmirai_doctor_Insert]
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
SET @emailAvailable = dbo.isEmailAvailable(@email,@userid)	 
IF(@emailAvailable =1)
BEGIN
   IF (@userid=0)
	BEGIN
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
		,   @firstname + '-' +@lastname
		)
		
		SET @userid = @@IDENTITY

		INSERT INTO dbo.doctorspecialities (doctorid,specialityid) select @userid,specility_id from @specialities; 

		update users set photopath=cast(@userid as varchar) + @photopath where userid=@userid
	END
END
SELECT @emailAvailable as EmailAvailable,@userid as UserId


