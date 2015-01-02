/****** Object:  StoredProcedure [dbo].[askmirai_patient_Insert_Update]    Script Date: 12/30/2014 6:13:52 PM ******/
DROP PROCEDURE [dbo].[askmirai_patient_Insert_Update]
GO

/****** Object:  StoredProcedure [dbo].[askmirai_patient_Insert_Update]    Script Date: 12/30/2014 6:13:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE PROCEDURE [dbo].[askmirai_patient_Insert_Update]
	@firstname varchar(50)
,	@lastname varchar(50)
,	@email varchar(100)
,	@mobileno varchar(15)
,	@gender int
,	@dateofbirth datetime
,	@countryid int
,	@stateid int
,	@locationid int
,	@cityid int
,	@password varchar(70)
,	@height decimal
,	@weight decimal
,	@address varchar(70)
,	@pincode int
,	@userid int =0
,   @registrationdate datetime
,   @status int
,   @Usertype int
,	@username  varchar(150)
,   @emailverified bit = 0

AS

DECLARE @emailAvailable as BIT
SET @emailAvailable=1
SET @emailAvailable = dbo.isEmailAvailable(@email, @userid)
print @emailAvailable
IF( @emailAvailable =1)
	BEGIN
	IF (@userid=0)
	BEGIN
		INSERT INTO dbo.users (
				firstname
		   ,	lastname
		   ,	email
		   ,	mobileno
		   ,	gender
		   ,	dateofbirth
		   ,	password
		   ,	countryid
		   ,	stateid
		   ,	cityid
		   ,	locationid
		   ,	height
		   ,	weight
		   ,	registrationdate
		   ,	usertype
		   ,    username
		   ,	status
		   ,    pincode
		   ,    address
		   ,	modified_on
		)
		VALUES (
				@firstname
		   ,	@lastname
		   ,	@email
		   ,	@mobileno
		   ,	@gender
		   ,	@dateofbirth
		   ,    @password
		   ,	@countryid
		   ,	@stateid
		   ,	@cityid
		   ,	@locationid
		   ,	@height
		   ,	@weight
		   ,    @registrationdate
		   ,    @Usertype
		   ,    @username
		   ,    @status
		   ,	@pincode
		   ,    @address
		   ,	GETDATE()
		)
		SET @userid=@@IDENTITY
	END
	else
	BEGIN
		DECLARE @oldemail as varchar(50)	
		select @oldemail=email from users where userid=@userid
        if(@oldemail <> @email)
			BEGIN
			   Update users 
			   set firstname=@firstname,
			   lastname=@lastname,
			   email=@email,
			   mobileno=@mobileno,
			   gender=@gender,
			   dateofbirth=@dateofbirth,
			   countryid=@countryid,
			   stateid=	@stateid,
			   cityid=	@cityid,
			   locationid=@locationid,
			   height=@height,
			   weight=@weight,
			   isemailverified=@emailverified,
			   registrationdate= @registrationdate,
			   username=@username,
			   pincode=@pincode,
			   address=@address,
			   modified_on = GETDATE(),
			   modified_by = @userid
			   WHERE userid=@userid
			END
		else
			BEGIN
			   Update users 
			   set firstname=@firstname,
			   lastname=@lastname,
			   email=@email,
			   mobileno=@mobileno,
			   gender=@gender,
			   dateofbirth=@dateofbirth,
			   countryid=@countryid,
			   stateid=	@stateid,
			   cityid=	@cityid,
			   locationid=@locationid,
			   height=@height,
			   weight=@weight,
			   registrationdate= @registrationdate,
			   username=@username,
			   pincode=@pincode,
			   address=@address,
			   modified_on = GETDATE(),
			   modified_by = @userid
			   WHERE userid=@userid
			END
END	

end
SELECT @emailAvailable as EmailAvailable,@userid as UserId




GO


