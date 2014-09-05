
/****** Object:  StoredProcedure [dbo].[SP_Check_Insert_PreRegistration]    Script Date: 9/5/2014 6:24:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--@isUserRegistered = 0 for newly registered user from pre_registration table
--@isUserRegistered = 1 for existing user
Create PROCEDURE[dbo].[SP_Check_Insert_PreRegistration]     
	@email varchar(50)
,   @password varchar(100)
,   @userid Int = 0
,   @status int
,   @usertype varchar(10)
,   @registrationdate datetime
,   @isemailverified bit
,   @questionid int
AS
DECLARE @isUserRegistered as BIT
IF exists(select * from tbl_pre_registration where email = @email)
  BEGIN
     IF exists(select * from users where email = @email)
	   BEGIN
		  set @isUserRegistered = 1
		  select userid,firstname,lastname, email, password ,@isUserRegistered as isUserRegistered from users where email = @email
	   end
	else
	BEGIN
	    DECLARE @firstname VARCHAR(50)
		DECLARE @lastname VARCHAR(50)
		DECLARE @mobileno VARCHAR(50)

		SELECT @firstname = firstname, @lastname = lastname, @mobileno = mobileno FROM tbl_pre_registration WHERE email = @email

	    INSERT INTO dbo.users(
		firstname
		,lastname
		,email
		,mobileno
		,gender
		,password
		,status
		,usertype
		,registrationdate
		,isemailverified
		)
		values(
		 @firstname
		,@lastname
		,@email
		,@mobileno
		,0
		,@password
		,@status
		,@usertype
		,@registrationdate
		,@isemailverified

		)

		delete from tbl_pre_registration where email = @email
        set @userid = @@IDENTITY
		set @isUserRegistered = 0  
		Insert into doctorquestions(questionid,doctorid)values(@questionid,@userid)
		select userid,firstname,lastname, email, password, @isUserRegistered as isUserRegistered from users where email = @email
	END
  END
else
  BEGIN
     if exists(select * from users where email = @email)
	   BEGIN
		   set @isUserRegistered = 1
		   select userid,firstname,lastname, email, password, @isUserRegistered as isUserRegistered from users where email = @email
	   END
	
  END
