USE [AsquareMirai_Staging]
GO

/****** Object:  StoredProcedure [dbo].[patient_Insert_Update]    Script Date: 12/30/2014 6:28:08 PM ******/
DROP PROCEDURE [dbo].[patient_Insert_Update]
GO

/****** Object:  StoredProcedure [dbo].[patient_Insert_Update]    Script Date: 12/30/2014 6:28:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[patient_Insert_Update]
	@uid int
,	@firstname varchar(50)
,	@lastname varchar(50)
,	@email varchar(100)
,	@mobileno varchar(15)
,	@gender int
,	@dateofbirth datetime
,	@city varchar(100)
,	@password varchar(70)
,	@cgname varchar(100)
,	@cgemail varchar(100)
,	@cgmobileno varchar(15)
,   @isremonedayprior BIT =0
,   @isremmorningonapptday BIT =0
,   @isremoncancellationoflasttwo BIT =0
,	@usertype varchar(10)='Patient'
,	@userid INT =0
,   @verifyemail bit=0
,   @isgoogleaccount bit=0
,   @isfacebookaccount bit=0
,   @countrycode1 int
,   @cgcountrycode int
AS

DECLARE @emailAvailable as BIT
DECLARE @isGoogleORFacebook bit
set @isGoogleORFacebook =0
SET @emailAvailable=1
SET @emailAvailable = dbo.isEmailAvailable(@email,@userid)	 
IF(@emailAvailable =1)
BEGIN
   IF (@userid=0)
	BEGIN
	 if(@isgoogleaccount=1 or @isfacebookaccount=1)
	     BEGIN
		 INSERT INTO dbo.users (
			firstname
	   ,	lastname
	   ,	email
	   ,	mobileno
	   ,	gender
	   ,	dateofbirth
	   ,	patientcity
	   ,	password
	   ,	cgname
	   ,	cgemail
	   ,	cgmobileno
	   ,    usertype 
	   ,    isdocconnectuser
	   ,    status
	   ,    isemailverified
       ,    isgoogleaccount 
	   ,    isfacebookaccount 
	   ,    countrycode1 
       ,    cgcountrycode 
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
		 ,	@dateofbirth
		 ,	@city
		 ,	@password
		 ,	@cgname
		 ,	@cgemail
		 ,	@cgmobileno
		 ,  2
		 ,  1
		 ,  1
		 ,  1
         ,  @isgoogleaccount
		 ,  @isfacebookaccount
		 ,  @countrycode1 
		 ,  @cgcountrycode
		 ,	@uid
		 ,	@uid
		 ,	GETDATE()
		 )
		 SET @userid=@@IDENTITY
		 set @isGoogleORFacebook=1
		end
		else
	     BEGIN
		 INSERT INTO dbo.users (
			firstname
	   ,	lastname
	   ,	email
	   ,	mobileno
	   ,	gender
	   ,	dateofbirth
	   ,	patientcity
	   ,	password
	   ,	cgname
	   ,	cgemail
	   ,	cgmobileno
	   ,    usertype 
	   ,    isdocconnectuser
	   ,    status
       ,    isgoogleaccount 
	   ,    isfacebookaccount
	   ,    countrycode1 
       ,    cgcountrycode  
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
		 ,	@dateofbirth
		 ,	@city
		 ,	@password
		 ,	@cgname
		 ,	@cgemail
		 ,	@cgmobileno
		 ,  2
		 ,  1
		 ,  0
         ,  @isgoogleaccount
		 ,  @isfacebookaccount
		 ,  @countrycode1 
		 ,  @cgcountrycode
		 ,	@uid
		 ,	@uid
		 ,	GETDATE()
		 )
		 SET @userid=@@IDENTITY
		end
	END
   ELSE   
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
				   cgname=@cgname,
				   cgemail=@cgemail,
				   cgmobileno=@cgmobileno,	
				   isemailverified=@verifyemail,
				   isremonedayprior=@isremonedayprior,
				   isremmorningonapptday=@isremmorningonapptday, 
				   countrycode1=@countrycode1, 
				   cgcountrycode= @cgcountrycode,
				   created_by = @uid,
				   modified_by = @uid,
				   modified_on = GETDATE(),
				   isremoncancellationoflasttwo=@isremoncancellationoflasttwo WHERE userid=@userid
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
		   cgname=@cgname,
		   cgemail=@cgemail,
		   cgmobileno=@cgmobileno,		  
		   isremonedayprior=@isremonedayprior,
		   isremmorningonapptday=@isremmorningonapptday,
		   countrycode1=@countrycode1, 
		   cgcountrycode=@cgcountrycode,
			created_by = @uid,
			modified_by = @uid,
			modified_on = GETDATE(),
		   isremoncancellationoflasttwo=@isremoncancellationoflasttwo WHERE userid=@userid
			END	
	   END 
 END 
	  
SELECT @emailAvailable as EmailAvailable,@userid as UserId, @isGoogleORFacebook as SignInUsingGoogleORFacebook














GO


