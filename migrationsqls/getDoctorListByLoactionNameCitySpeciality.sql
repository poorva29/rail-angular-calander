USE [AsquareMirai]
GO
/****** Object:  StoredProcedure [dbo].[getDoctorListByLoactionNameCitySpeciality]    Script Date: 8/28/2014 2:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================

-- Author:		<Author,,Name>

-- Create date: <Create Date,,>

-- Description:	<Description,,>

-- =============================================

ALTER PROCEDURE [dbo].[getDoctorListByLoactionNameCitySpeciality] 

	@cityid int

,	@locationid int

,	@specialityOrName varchar(150)

,   @userStatus int

AS



BEGIN

	declare  @docdegree table

	(

		userid int,

		name varchar(max)

	)

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

	declare  @degreetemp table

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

	declare  @avgresponsetimetemp table

	(

		id int primary key identity(1,1),

		userid int,

		avgresponsetime int

	)

	declare @docList table

	(

		id int primary key identity(1,1),

		userid varchar(max),
		
		gender int,

		name varchar(max),

		lastname varchar(max),

		email varchar(150),

		mobileno varchar(15),

		degree varchar(max),

		specialities varchar(max),

		photopath varchar(256),

		photourl varchar(200),

		cities varchar(max),

		locations varchar(max),

		isdocconnectuser bit,

		appointmentbuttonhits int,

		avgresponsetime varchar(max),

		appointmentcount int 

	)	

	declare @count int

	declare @degree varchar(max)

	declare @city varchar(max)

	declare @location varchar(max)

	declare @userid int

	declare @id int = 1	

	declare @specialities varchar(max)

	declare @doccount int

	Declare @docuserid int

	Declare @doctorcount int

	Declare @doctorid int



    if(@cityid is not null and @locationid is not null and @specialityOrName is null)

	begin

		insert into @docList

		 select DISTINCT users.userid,users.gender,'Dr. ' + users.firstname  + ' ' + users.lastname as name,users.lastname,users.email,users.mobileno,'' as degree, '' as specialities,

		 users.photourl, users.photopath,'','', users.isdocconnectuser, users.appointmentbuttonhits, '',''     from doctorlocations  Inner join users On users.userid=doctorlocations.doctorid LEFT OUTER join doctorspecialities On users.userid= doctorspecialities
.doctorid LEFT OUTER join specialities On doctorspecialities.specialityid=specialities.specialityid

         where users.usertype=1 and users.status = @userStatus and (users.firstname + '%' + users.lastname like '%'+@specialityOrName+'%' or users.firstname like '%'+@specialityOrName+'%' or users.lastname like '%'+@specialityOrName+'%' or doctorlocations.cityid=@cityid) and (doctorlocations.locationid=@locationid) 

	

	end

	else if(@locationid is null and @cityid is null and @specialityOrName is not null)

	begin

	    insert into @docList

		 select DISTINCT users.userid,users.gender,'Dr. ' + users.firstname  + ' ' + users.lastname as name,users.lastname,users.email,users.mobileno,'' as degree, '' as specialities,

		 users.photourl,users.photopath,'','', users.isdocconnectuser, users.appointmentbuttonhits, '',''      from users  Left Outer join doctorlocations On users.userid=doctorlocations.doctorid LEFT OUTER join doctorspecialities On users.userid= doctorspecialities.doctorid LEFT OUTER join specialities On doctorspecialities.specialityid=specialities.specialityid

        where users.usertype=1 and users.status = @userStatus and (users.firstname + '%' + users.lastname like '%'+@specialityOrName+'%' or users.firstname like '%'+@specialityOrName+'%' or users.lastname like '%'+@specialityOrName+'%' or specialities.name like '%'+@specialityOrName+'%')

	end

	else if(@locationid is null and @cityid is not null and @specialityOrName is not null)

	begin

	   insert into @docList

		 select DISTINCT users.userid,users.gender,'Dr. ' + users.firstname  + ' ' + users.lastname as name,users.lastname,users.email,users.mobileno,'' as degree, '' as specialities,

		 users.photourl, users.photopath,'','', users.isdocconnectuser, users.appointmentbuttonhits, '',''     from  users  Left Outer join doctorlocations On users.userid=doctorlocations.doctorid LEFT OUTER join doctorspecialities On users.userid= doctorspecialities.doctorid LEFT OUTER join specialities On doctorspecialities.specialityid=specialities.specialityid

        where users.usertype=1 and users.status = @userStatus and (users.firstname + '%' + users.lastname like '%'+@specialityOrName+'%' or users.firstname like '%'+@specialityOrName+'%' or users.lastname like '%'+@specialityOrName+'%' or specialities.name like '%'+@specialityOrName+'%') and doctorlocations.cityid=@cityid

	end

	else if(@cityid is not null and @locationid is null and @specialityOrName is null)

		BEGIN

		insert into @docList

		   select DISTINCT users.userid,users.gender,'Dr. ' + users.firstname  + ' ' + users.lastname as name,users.lastname,users.email,users.mobileno,'' as degree, '' as specialities,

		   users.photourl, users.photopath,'','', users.isdocconnectuser, users.appointmentbuttonhits, '',''  from  users  Left Outer join doctorlocations On users.userid=doctorlocations.doctorid LEFT OUTER join doctorspecialities On users.userid= doctorspecialities.doctorid LEFT OUTER join specialities On doctorspecialities.specialityid=specialities.specialityid 

		   where users.usertype=1 and users.status = @userStatus and doctorlocations.cityid=@cityid

		END

	else if(@cityid is not null and @locationid is not null and @specialityOrName is not null)

		BEGIN

		insert into @docList

		 select DISTINCT users.userid,users.gender,'Dr. ' + users.firstname  + ' ' + users.lastname as name,users.lastname,users.email,users.mobileno,'' as degree, '' as specialities,

		 users.photourl,users.photopath,'','', users.isdocconnectuser, users.appointmentbuttonhits, '',''     from doctorlocations  Inner join users On users.userid=doctorlocations.doctorid LEFT OUTER join doctorspecialities On users.userid= doctorspecialities.
doctorid LEFT OUTER join specialities On doctorspecialities.specialityid=specialities.specialityid

         where users.usertype=1 and users.status = @userStatus and (users.firstname + '%' + users.lastname like '%'+@specialityOrName+'%' or users.firstname like '%'+@specialityOrName+'%' or users.lastname like '%'+@specialityOrName+'%' or specialities.name like '%'+@specialityOrName+'%') and doctorlocations.cityid=@cityid and
 doctorlocations.locationid=@locationid

		END

	else

	   BEGIN

	       insert into @docList

		   select DISTINCT users.userid,users.gender,'Dr. ' + users.firstname  + ' ' + users.lastname as name,users.lastname,users.email,users.mobileno,'' as degree, '' as specialities,

		   users.photourl, users.photopath,'','', users.isdocconnectuser, users.appointmentbuttonhits, '',''  from  users  Left Outer join doctorlocations On users.userid=doctorlocations.doctorid LEFT OUTER join doctorspecialities On users.userid= doctorspecialities.doctorid LEFT OUTER join specialities On doctorspecialities.specialityid=specialities.specialityid 

		   where users.usertype=1 and users.status = @userStatus

	   END



	set @doctorid = 1

	select @doctorcount =count(*) from @docList

	while(@doctorcount>0)

	BEGIN

		select @docuserid=userid from @docList where id=@doctorid

		insert into @docdegree select  @docuserid, degree.name from @docList LEFT OUTER JOIN doctorqualification

		ON @docuserid = doctorqualification.doctorid INNER JOIN degree 

		ON degree.degreeid = doctorqualification.degreeid  where id=@doctorid



		insert into @docspeciality select  @docuserid,specialities.name from @docList LEFT OUTER JOIN doctorspecialities

		ON @docuserid = doctorspecialities.doctorid INNER JOIN specialities

		ON specialities.specialityid = doctorspecialities.specialityid  where id=@doctorid



		insert into @doccities select distinct  @docuserid,city.name from @docList LEFT OUTER JOIN doctorlocations

		ON @docuserid = doctorlocations.doctorid INNER JOIN city

		ON city.cityid = doctorlocations.cityid  where id=@doctorid



		insert into @doclocations select  DISTINCT @docuserid,location.name from @docList LEFT OUTER JOIN doctorlocations

		ON @docuserid = doctorlocations.doctorid INNER JOIN location

		ON location.locationid = doctorlocations.locationid  where id=@doctorid



		set @doctorcount-=1

		set @doctorid+=1

	ENd

	

	insert into @degreetemp

	select  userid,  STUFF((

	SELECT ', ' + CAST(name AS VARCHAR(MAX))FROM @docdegree where (userid = results.userid)

	FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,2,'')

	from @docdegree results   group by userid



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

	select @count = count(*) from @degreetemp



	while(@doccount > 0)

	begin

		if(@count > 0)

		begin		     

			select @degree = name, @userid = userid from @degreetemp where id = @id	

			update @docList set degree = @degree where userid = @userid

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

	select @count = count(*) from @locationstemp

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



	insert into @avgresponsetimetemp

	select  users.userid,avg(DATEDIFF(MINUTE,questions.createdate, answers.createdate) ) as avgresponsetime  from users

	left outer join answers on users.userid = answers.userid

	inner join questions on answers.questionid = questions.questionid

	 where usertype = 1 group by users.userid



	declare @avgresponse varchar(max)

	select @count = count(*) from @avgresponsetimetemp

	

	print @count

	set @id = 1

	while(@count > 0)

	begin

		select @userid = userid,@avgresponse = avgresponsetime from @avgresponsetimetemp where id = @id

		update  @docList set avgresponsetime = @avgresponse where userid = @userid

		set @count = @count -1

		set @id = @id  + 1

	end



	select @doccount = count(*) from @docList

	declare @askmiraiapptcount int

	declare @docid int
    
	declare @doctorids int

	set @docid =1
	
	while(@doccount > 0)

	begin
	
	    select @doctorids = userid from @docList where id = @docid

		select @userid = isdocconnectuser from @docList where id = @docid

		if @userid != 0 and @userid is not null

		begin

		select @askmiraiapptcount=askmiraiappointmentcount from users where userid = @doctorids 

		update @docList set appointmentcount = @askmiraiapptcount where id = @docid

		end

		set @doccount = @doccount - 1

		set @docid = @docid + 1

	end

	select @avgresponse= avg(avgresponsetime) from @avgresponsetimetemp



	delete from @doccities

	delete from @citiestemp

	delete from @doclocations

	delete from @locationstemp

	delete from @docdegree

	delete  from @docspeciality

	delete from @degreetemp

	delete  from @specialitytemp

	select *,@avgresponse as averageresponsetime from @docList order by lastname

	

END




