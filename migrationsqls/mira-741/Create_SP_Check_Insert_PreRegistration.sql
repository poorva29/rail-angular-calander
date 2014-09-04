
/****** Object:  StoredProcedure [dbo].[askmirai_doctor_Insert]    Script Date: 9/4/2014 5:39:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Alter PROCEDURE[dbo].[SP_Check_Insert_PreRegistration]
    @firstname varchar(50)
,	@lastname varchar(50)
,	@email varchar(50)
,	@mobileno varchar(15)
,   @userid Int = 0
AS
DECLARE @isUserRegistered as BIT
IF exists(select * from pre_registration where email = @email)
  BEGIN
     IF Not exists(select * from users where email = @email)
	   BEGIN
	     delete from pre_registration where email = @email
	     INSERT INTO dbo.users(
		 	firstname
		   ,lastname
		   ,email
		   ,mobileno
		   )
		 values(
			@firstname
			,@lastname
			,@email
			,@mobileno
			)
         set @userid = @@IDENTITY
		 set @isUserRegistered = 0
		 select userid,email,@isUserRegistered as isUserRegistered from users where email = @email
	   END
      else
	    BEGIN
		  set @isUserRegistered = 1
		  select userid,email,@isUserRegistered as isUserRegistered from users where email = @email
		END
  END
else
  BEGIN
     if exists(select * from users where email = @email)
	   BEGIN
		   set @isUserRegistered = 1
		   select userid,email,@isUserRegistered as isUserRegistered from users where email = @email
	   END
  END
