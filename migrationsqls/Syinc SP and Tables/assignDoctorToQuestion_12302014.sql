/****** Object:  StoredProcedure [dbo].[assignDoctorToQuestion]    Script Date: 12/30/2014 6:15:26 PM ******/
DROP PROCEDURE [dbo].[assignDoctorToQuestion]
GO

/****** Object:  StoredProcedure [dbo].[assignDoctorToQuestion]    Script Date: 12/30/2014 6:15:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE[dbo].[assignDoctorToQuestion]
	@uid int
,   @doctorIds varchar(max)
,	@questionId INT =0
AS
BEGIN
	
	declare  @docspeciality table
	(
		userid int,
		name varchar(max)
	)
	declare  @doccities table
	(
		userid int,
		name varchar(max)
	)
	declare  @doclocations table
	(
		userid int,
		name varchar(max)
	)
	declare  @citiestemp table
	(
		id int primary key identity(1,1),
		userid int,
		name varchar(max)
	)
	declare  @locationstemp table
	(
		id int primary key identity(1,1),
		userid int,
		name varchar(max)
	)
	declare  @specialitytemp table
	(
		id int primary key identity(1,1),
		userid int,
		name varchar(max)
	)
	declare @docList table
	(
		id int primary key identity(1,1),
		userid int,
		name varchar(100),
		lastname varchar(100),
		email varchar(max),
		mobileno varchar(max),
		specialities varchar(max),
		cities varchar(max),
		locations varchar(max)
		
	)	
	declare @count int
	declare @city varchar(max)
	declare @location varchar(max)
	declare @userid int
	declare @id int = 1	
	declare @specialities varchar(max)
	declare @doccount int
	Declare @docuserid int
	Declare @doctorcount int
	Declare @doctorid int

	INSERT INTO dbo.doctorquestions(questionid,doctorid,created_by,created_on,modified_by,modified_on) 
	select @questionId, userid,@uid as created_by,GETDATE() as created_on,@uid as modified_by,GETDATE() as modified_on from users 
	Where userid in (SELECT * FROM CommaSeparatedToString(@doctorIds))
	update questions set status=1 where questionid=@questionId
	insert into @docList
		select users.userid,'Dr. '+users.firstname+' '+ users.lastname as name,'Dr. '+users.lastname as lastname, users.email, users.mobileno,'',
		'',''  from users inner join doctorquestions on users.userid =  doctorquestions.doctorid
		 Where doctorquestions.questionid =@questionid
	set @doctorid = 1
	select @doctorcount =count(*) from @docList
	while(@doctorcount>0)
	BEGIN
		select @docuserid=userid from @docList where id=@doctorid

		insert into @docspeciality select  @docuserid,specialities.name from @docList LEFT OUTER JOIN doctorspecialities
		ON @docuserid = doctorspecialities.doctorid INNER JOIN specialities
		ON specialities.specialityid = doctorspecialities.specialityid  where id=@doctorid

		insert into @doccities select  @docuserid,city.name from @docList LEFT OUTER JOIN doctorlocations
		ON @docuserid = doctorlocations.doctorid INNER JOIN city
		ON city.cityid = doctorlocations.cityid  where id=@doctorid

		insert into @doclocations select  @docuserid,location.name from @docList LEFT OUTER JOIN doctorlocations
		ON @docuserid = doctorlocations.doctorid INNER JOIN location
		ON location.locationid = doctorlocations.locationid  where id=@doctorid

		set @doctorcount-=1
		set @doctorid+=1
	ENd
	insert into @specialitytemp
	select  userid,  STUFF((
	SELECT ', ' + CAST(name AS VARCHAR(MAX)) FROM @docspeciality where (userid = results.userid)
	FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,2,'')
	from @docspeciality results   group by userid

	insert into @locationstemp
	select  userid,  STUFF((
	SELECT ', ' + CAST(name AS VARCHAR(MAX)) FROM @doclocations where (userid = results.userid)
	FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,2,'')
	from @doclocations results   group by userid

	insert into @citiestemp
	select  userid,  STUFF((
	SELECT ', ' + CAST(name AS VARCHAR(MAX)) FROM @doccities where (userid = results.userid)
	FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,2,'')
	from @doccities results   group by userid

	select @doccount = count(*) from @docList
	select @count = count(*) from @specialitytemp
	set @id =1
	while(@doccount > 0)
	begin
		if(@count > 0)
		begin    
			select @specialities =  name, @userid = userid from @specialitytemp where id = @id
			update @docList set specialities = @specialities where userid = @userid
		end
		set @doccount = @doccount - 1
		set @id = @id + 1
	end

	select @doccount = count(*) from @doccities
	select @count = count(*) from @citiestemp
	set @id =1
	while(@doccount > 0)
	begin
		if(@count > 0)
		begin    
			select @city =  name, @userid = userid from @citiestemp where id = @id
			update @docList set cities = @city where userid = @userid
		end
		set @doccount = @doccount - 1
		set @id = @id + 1
	end

	select @doccount = count(*) from @docList
	select @count = count(*) from @specialitytemp
	set @id =1
	while(@doccount > 0)
	begin
		if(@count > 0)
		begin    
			select @location =  name, @userid = userid from @locationstemp where id = @id
			update @docList set locations = @location where userid = @userid
		end
		set @doccount = @doccount - 1
		set @id = @id + 1
	end

	delete from @doccities
	delete from @citiestemp
	delete from @doclocations
	delete from @locationstemp
	delete  from @docspeciality
	delete  from @specialitytemp
	select * from @docList
End






GO


