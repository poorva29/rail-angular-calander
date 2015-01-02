/****** Object:  StoredProcedure [dbo].[Insert_Update_doctorassistant]    Script Date: 12/30/2014 6:26:44 PM ******/
DROP PROCEDURE [dbo].[Insert_Update_doctorassistant]
GO

/****** Object:  StoredProcedure [dbo].[Insert_Update_doctorassistant]    Script Date: 12/30/2014 6:26:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Insert_Update_doctorassistant]
	@uid int
,	@doctorid int=0
,	@firstname varchar(50)
,	@lastname varchar(50)
,	@email varchar(50)
,	@mobileno varchar(15)
,	@gender int
,	@joiningdate datetime
,   @usertype varchar(10)='Assistant'
,   @userid int=0 
,	@verifyemail bit=0
,	@secPrimaryId int
,   @countrycode1 int
AS
DECLARE @emailAvailable as BIT
DECLARE @changeUserType as INT
SET @emailAvailable=1
SET @emailAvailable = dbo.isEmailAvailable(@email,@userid)

	if(@doctorid <> 0)
	BEGIN
		SET @changeUserType  = 3
		SET @secPrimaryId = @doctorid
	END
	ELSE IF @secPrimaryId = 0
	BEGIN
		SET @changeUserType  = 4
		SET @secPrimaryId = 0
	END
	ELSE IF @secPrimaryId <> 0
	BEGIN 
		SET @changeUserType  = 5
	END
if(@emailAvailable=1)
BEGIN
  IF(@userid=0)
     BEGIN
		INSERT INTO dbo.users (
			firstname
		,	lastname
		,	email
		,	mobileno
		,	gender
		,	joiningdate
		,	reference_id
		,   usertype
		,   isdocconnectuser
		,   countrycode1
		,	created_by
		,	modified_by
		,	modified_on
		)
		VALUES (
	        @firstname
		,	@lastname
		,	@email
		,	@mobileno
		,	@gender
		,	@joiningdate
		,	@secPrimaryId
		,   @changeUserType
		,   1
		,   @countrycode1
		,	@uid
		,	@uid
		,	GETDATE()
		)

	   set @userid=@@IDENTITY
    END
  ELSE   
     BEGIN 	
       DECLARE @oldemail as varchar(50)	
		select @oldemail=email from users where userid=@userid
		 if(@oldemail <> @email)		
			BEGIN			  
			   UPDATE dbo.users SET
					firstname = @firstname
				,	lastname = @lastname
				,	email = @email
				,	mobileno = @mobileno
				,	gender = @gender
				,	joiningdate = @joiningdate
				,   isemailverified=@verifyemail
				,	reference_id=@secPrimaryId
				,   countrycode1=@countrycode1
				,	modified_by = @uid
				,	modified_on = GETDATE()
				WHERE
					userid = @userid
			   END	
		else
		  BEGIN		    
				UPDATE dbo.users SET
					firstname = @firstname
				,	lastname = @lastname
				,	email = @email
				,	mobileno = @mobileno
				,	gender = @gender
				,	joiningdate = @joiningdate	
				,	reference_id=@secPrimaryId
				,   countrycode1=@countrycode1	
				,	modified_by = @uid
				,	modified_on = GETDATE()	
				WHERE
					userid = @userid
		 END		
    END  
 END       
SELECT @emailAvailable as EmailAvailable,@userid as UserId








GO


