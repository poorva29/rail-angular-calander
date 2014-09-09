
/****** Object:  StoredProcedure [dbo].[SP_Check_Insert_PreRegistration]    Script Date: 9/8/2014 11:00:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--@isUserRegistered = 0 for newly registered user from pre_registration table
--@isUserRegistered = 1 for existing user
Create PROCEDURE[dbo].[SP_Check_Insert_PreRegistration]-- 'Demo','doctor','yassh88@outlook.com','1234567890','58sQsYODZz0=',0,2,1,'05/Sep/14 4:06:05 PM',1,2355   
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
DECLARE @doctorid as int
IF exists(select * from tbl_pre_registration where email = @email)
  BEGIN
     IF exists(select * from users where email = @email)
	   BEGIN
		  set @isUserRegistered = 1
		  select @doctorid = userid from users where email = @email and status = 2 and isemailverified = 1
		  select userid,firstname,lastname, email, password ,@isUserRegistered as isUserRegistered from users where email = @email and status = 2 and isemailverified = 1 and usertype = 1
		  if not exists(select * from doctorquestions where questionid = @questionid and doctorid = @doctorid)
		  BEGIN
			  Insert into doctorquestions(questionid,doctorid)values(@questionid,@doctorid)
			  update questions set status=1 where questionid=@questionid
		  END
	   end
	else
	BEGIN
		DECLARE @firstname VARCHAR(50)
		DECLARE @lastname VARCHAR(50)
		

		SELECT @firstname = firstname, @lastname = lastname FROM tbl_pre_registration WHERE email = @email


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
		,''
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
		 if not exists(select * from doctorquestions where questionid = @questionid and doctorid = @doctorid)
		  BEGIN
			  Insert into doctorquestions(questionid,doctorid)values(@questionid,@userid)
			  update questions set status=1 where questionid=@questionid
		  END
		select userid,firstname,lastname, email, password, @isUserRegistered as isUserRegistered from users where email = @email and status = 2 and isemailverified = 1 and usertype = 1
	END
  END
else
  BEGIN
     if exists(select * from users where email = @email)
	   BEGIN
		   set @isUserRegistered = 1
		   select @doctorid = userid from users where email = @email and status = 2 and isemailverified = 1
		   select userid,firstname,lastname,email, password, @isUserRegistered as isUserRegistered from users where email = @email and status = 2 and isemailverified = 1 and usertype = 1
		   if not exists(select * from doctorquestions where questionid = @questionid and doctorid = @doctorid)
			  BEGIN
				  Insert into doctorquestions(questionid,doctorid)values(@questionid,@doctorid)
				  update questions set status=1 where questionid=@questionid
			  END
	   END
	
  END
