/****** Object:  StoredProcedure [dbo].[doctorlocations_Update_and_Insert]    Script Date: 12/30/2014 6:23:15 PM ******/
DROP PROCEDURE [dbo].[doctorlocations_Update_and_Insert]
GO

/****** Object:  StoredProcedure [dbo].[doctorlocations_Update_and_Insert]    Script Date: 12/30/2014 6:23:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[doctorlocations_Update_and_Insert]
	@uid int
,	@doctorid int
,	@doclocid int
,	@countryid int
,	@stateid int
,	@cityid int
,	@locationid int
,	@clinicname varchar(100)
,	@address varchar(250)
,	@telephone varchar(33)
,	@timeslot int
,	@fees float
,	@isprimary bit
,	@hospitalid int
,	@hospitalname varchar(100)
AS
DECLARE @LoctionAvailable as bit
DECLARE @OldLoctionid as int
DECLARE @ClinicNameTemp varchar(50)
SET @LoctionAvailable=1;
if(@hospitalid=0)
BEGIN
	IF(exists(select * from doctorlocations where locationid=@locationid and doctorid=@doctorid and  doctorlocationid <> @doclocid and(hospitalid = null or hospitalid = 0)))
		begin
			SET @LoctionAvailable=0
		end
END
ELSE
BEGIN
	IF(exists(select * from doctorlocations where locationid=@locationid and doctorid=@doctorid and  doctorlocationid <> @doclocid and hospitalid = @hospitalid ))
		begin
			SET @LoctionAvailable=0
		end
END	
if(@LoctionAvailable !=0)
BEGIN
	if(@doclocid > 0)
	
			begin			
			IF(@isprimary = 1 )
			begin
				update doctorlocations set isprimary = 0,modified_by = @uid,modified_on = GETDATE() where doctorid = @doctorid and countryid = @countryid AND isprimary=1
			end			
			UPDATE  doctorlocations SET
				doctorid = @doctorid
			,	countryid = @countryid
			,	stateid = @stateid
			,	cityid = @cityid
			,	locationid = @locationid
			,	clinicname = @clinicname
			,	address = @address
			,	telephone = @telephone
			,	timeslot = @timeslot
			,	consultationfees = @fees
			,	isprimary = @isprimary
			,	hospitalid = @hospitalid
			,	hospitalname = @hospitalname
			,	modified_by  = @uid
			,	modified_on = GETDATE()
			WHERE
				doctorlocationid = @doclocid
			end
		
		else
		
			begin
				IF(@isprimary = 1)
				begin
					update doctorlocations set isprimary = 0,modified_by = @uid,modified_on = GETDATE() where doctorid = @doctorid and countryid = @countryid and isprimary=1
				END
			
				IF(not exists(select * from doctorlocations where doctorid = @doctorid and countryid = @countryid and isprimary=1))
				BEGIN
					SET @isprimary = 1
				END	
				
				IF (@hospitalid <> 0)
				BEGIN
					SET @isprimary = 0
				END	
				INSERT INTO dbo.doctorlocations (
					doctorid
				,	countryid
				,	stateid
				,	cityid
				,	locationid
				,	clinicname
				,	address
				,	telephone
				,	timeslot
				,	consultationfees
				,	isprimary
				,	hospitalid 
				,	hospitalname
				,	created_by
				,	modified_by
				,	created_on
				,	modified_on
				)
				VALUES (
					@doctorid 
				,	@countryid 
				,	@stateid 
				,	@cityid 
				,	@locationid
				,	@clinicname
				,	@address 
				,	@telephone 
				,	@timeslot
				,	@fees 
				,	@isprimary 
				,	@hospitalid
				,	@hospitalname
				,	@uid
				,	@uid
				,	GETDATE()
				,	GETDATE()
				)
				SET @doclocid = (SELECT @@IDENTITY)
			end

END
select @doclocid as doclocid ,@LoctionAvailable as locationAvailable,@isprimary as IsPrimary  






GO


