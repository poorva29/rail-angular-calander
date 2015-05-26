--Priyanka Priyadarshni

Create PROCEDURE [dbo].[askmirai_CheckUserIsPresentInDb] --0,'sidhesh','abc1','def4231@gmail.com','1111111111',0,null,null,null,null,null,'3tWBixdjFF0=',null,null,null,null,0,'3/5/2015 6:10:13 PM',0,2,null,false
    @uid int
,	@firstname varchar(50)
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
BEGIN
declare  @userinformation table
(
	id int primary key identity(1,1),
	EmailAvailable int,
	regUserid int
)
--Checked user is present in unregpatient or not with firstname,lastname,fullname and mobile number
   IF not exists(select * from unregpatient where name = @firstName and mobileno = @mobileNo)
     BEGIN
	    If not exists(select * from unregpatient where name = @lastName and mobileno = @mobileNo)
		  BEGIN
		    If not exists(select * from unregpatient where name = @firstName + ' ' + @lastName and mobileno = @mobileNo)
			  BEGIN--user is present in users or not
			    IF exists(select * from users where firstname = @firstName and lastname = @lastName and mobileno=@mobileNo)
				   BEGIN
				   --Updated user if it is already exist in users table
					   Declare @existingUserId as int
					   set @existingUserId = (select userid from users where firstname = @firstName and lastname = @lastName and mobileno=@mobileNo)
					   set @userid = @existingUserId
						 insert into @userinformation exec askmirai_patient_Insert_Update @uid,@firstname,@lastname,@email,@mobileno,@gender,@dateofbirth,@countryid,@stateid,@locationid,@cityid,@password,@height,
						  @weight,@address,@pincode,@userid,@registrationdate,@status,@Usertype,
						  @username,@emailverified
						  update users set password = @password where userid = @existingUserId
				   END
                 else
				   BEGIN
				   --Inserted user into user table
				      insert into @userinformation exec askmirai_patient_Insert_Update @uid,@firstname,@lastname,@email,@mobileno,@gender,@dateofbirth,@countryid,@stateid,@locationid,@cityid,@password,@height,
						  @weight,@address,@pincode,@userid,@registrationdate,@status,@Usertype,
						  @username,@emailverified
				   END
			  END
            else
			 BEGIN
				Declare @unregPatientUserId as int				 
				set @unregPatientUserId = (select id from unregpatient where name = @firstName + ' ' +@lastName and mobileno=@mobileNo)
				 insert into @userinformation exec askmirai_patient_Insert_Update @uid,@firstname,@lastname,@email,@mobileno,@gender,@dateofbirth,@countryid,@stateid,@locationid,@cityid,@password,@height,
						  @weight,@address,@pincode,@userid,@registrationdate,@status,@Usertype,
						  @username,@emailverified
				   --updated followup history with new patient id
				if exists(select * from patientfollowuphistory where patientid = @unregPatientUserId)
					BEGIN
					Declare @regPatientid as int
					set @regPatientid = (select regUserid from @userinformation)
					update patientfollowuphistory set patientid = @regPatientid,modified_by=9999999,modified_on= GETDATE() where patientid = @unregPatientUserId
					END
					--updated appointments table with registerd user id
				if exists(select * from appointments where unregpatientid = @unregPatientUserId)
					BEGIN
					update appointments set patientid = @regPatientid,unregpatientid = null,lastmodifiedby=9999999,lastmodifiedat=GETDATE() where unregpatientid = @unregPatientUserId
					END
					--updated tble_doctor_patient table with registerd user id
				if exists(select * from tbl_doctor_patient where patientId = @unregPatientUserId)
				    BEGIN
					 update tbl_doctor_patient set patientId = @regPatientid where patientId = @unregPatientUserId
					END
					 --Deleted user from unregpatient table after updating
					 DELETE FROM unregpatient WHERE id = @unregPatientUserId
				 END			  
			END
		  else
		    BEGIN
			    Declare @unregPatientUserId1 as int
					set @unregPatientUserId1 = (select id from unregpatient where name = @lastName and mobileno=@mobileNo)
			         insert into @userinformation exec askmirai_patient_Insert_Update @uid,@firstname,@lastname,@email,@mobileno,@gender,@dateofbirth,@countryid,@stateid,@locationid,@cityid,@password,@height,
						  @weight,@address,@pincode,@userid,@registrationdate,@status,@Usertype,
						  @username,@emailverified
						--updated followup history with new patient id
					if exists(select * from patientfollowuphistory where patientid = @unregPatientUserId1)
					  BEGIN
					    Declare @regPatientid1 as int
						set @regPatientid1 = (select regUserid from @userinformation)
					    update patientfollowuphistory set patientid = @regPatientid1,modified_by=9999999,modified_on= GETDATE() where patientid = @unregPatientUserId1
					  END
					   --updated appointments table with registerd user id
                    if exists(select * from appointments where unregpatientid = @unregPatientUserId1)
					  BEGIN
					    update appointments set patientid = @regPatientid1,unregpatientid = null,lastmodifiedby=9999999,lastmodifiedat=GETDATE() where unregpatientid = @unregPatientUserId1
					  END
					  --updated tble_doctor_patient table with registerd user id
					if exists(select * from tbl_doctor_patient where patientId = @unregPatientUserId1)
					  BEGIN
						 update tbl_doctor_patient set patientId = @regPatientid1 where patientId = @unregPatientUserId1
					  END
					   --Deleted user from unregpatient table after updating
                    DELETE FROM unregpatient WHERE id = @unregPatientUserId1
			END
	 END
	 else
	  BEGIN
	    Declare @unregPatientUserId2 as int
		set @unregPatientUserId2 = (select id from unregpatient where name = @firstName and mobileno=@mobileNo)
		 insert into @userinformation exec askmirai_patient_Insert_Update @uid,@firstname,@lastname,@email,@mobileno,@gender,@dateofbirth,@countryid,@stateid,@locationid,@cityid,@password,@height,
						  @weight,@address,@pincode,@userid,@registrationdate,@status,@Usertype,
						  @username,@emailverified
			--updated followup history with new patient id		
		if exists(select * from patientfollowuphistory where patientid = @unregPatientUserId2)
			BEGIN
			Declare @regPatientid2 as int
			select @regPatientid2 = regUserid from @userinformation
			update patientfollowuphistory set patientid = @regPatientid2,modified_by=9999999,modified_on= GETDATE() where patientid = @unregPatientUserId2
			END
			 --updated appointments table with registerd user id
        if exists(select * from appointments where unregpatientid = @unregPatientUserId2)
			BEGIN
			update appointments set patientid = @regPatientid2,unregpatientid = null,lastmodifiedby=9999999,lastmodifiedat=GETDATE() where unregpatientid = @unregPatientUserId2
			END
			--updated tble_doctor_patient table with registerd user id
         if exists(select * from tbl_doctor_patient where patientId = @unregPatientUserId2)
			BEGIN
				update tbl_doctor_patient set patientId = @regPatientid2 where patientId = @unregPatientUserId2
			END
			 --Deleted user from unregpatient table after updating
		 DELETE FROM unregpatient WHERE id = @unregPatientUserId2
	  END
	  select EmailAvailable as EmailAvailable, regUserid as UserId from @userinformation
END
