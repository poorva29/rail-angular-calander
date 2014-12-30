/****** Object:  StoredProcedure [dbo].[askmirai_doctorlocations_Update_and_Insert]    Script Date: 12/30/2014 6:12:10 PM ******/
DROP PROCEDURE [dbo].[askmirai_doctorlocations_Update_and_Insert]
GO

/****** Object:  StoredProcedure [dbo].[askmirai_doctorlocations_Update_and_Insert]    Script Date: 12/30/2014 6:12:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[askmirai_doctorlocations_Update_and_Insert]
	@uid int
,	@userid int
,	@doclocid int
,	@countryid int
,	@stateid int
,	@cityid int
,	@locationid int
,	@clinicname varchar(100)
,	@address varchar(250)
,	@telephone varchar(15)

AS
BEGIN
	if(@doclocid > 0)	
			begin
			UPDATE  doctorlocations SET
				doctorid = @userid
			,	countryid = @countryid
			,	stateid = @stateid
			,	cityid = @cityid
			,	locationid = @locationid
			,	clinicname = @clinicname
			,	address = @address
			,	telephone = @telephone
			,	modified_by = @uid
			,	modified_on = GETDATE()
			WHERE
				doctorlocationid = @doclocid
			end
		
		else		
			begin				
				INSERT INTO dbo.doctorlocations (
					doctorid
				,	countryid
				,	stateid
				,	cityid
				,	locationid
				,	clinicname
				,	address
				,	telephone
				,	modified_by
				,	modified_on
				,	created_by
				,	created_on
				)
				VALUES (
					@userid 
				,	@countryid 
				,	@stateid 
				,	@cityid 
				,	@locationid
				,	@clinicname
				,	@address 
				,	@telephone
				,	@uid
				,	GETDATE()
				,	@uid
				,	GETDATE()
				)
				SET @doclocid = (SELECT @@IDENTITY)
			end

END
select @doclocid as doclocid





GO


