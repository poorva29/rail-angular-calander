-- =============================================
-- Author:		Pavan Shevle
-- Create date: 12/09/2014
-- Alter Log: Added name_seo
-- =============================================
ALTER PROCEDURE [dbo].[get_questiondetailsbyId]
		@questionid int,
		@userid int,
		@assignQuestion int =0,
		@questionStatus int
AS
BEGIN
declare @count int
declare @id int

if(@assignQuestion = 0)
begin
    Declare @tempQuestionDetail as table
   (
    id int primary key identity(1,1),
    questionid int,
	userid int,
	gender int,
	patientlastname varchar(50),
	patientemail varchar(50),
	lastname varchar(50),
	Email varchar(50),
	mobileno varchar(15),
	status int,
	createdate datetime,
	questiontext varchar(200),
	answerid int ,
	doctorimg varchar(max),
	title varchar(max),
	answertext varchar(max),
	answerimg varchar(max),
	answerdate datetime,
	Docid int,
	doctor varchar(100),
	ispatientthank bit DEFAULT 0,
	isendorse bit DEFAULT 0,
	thanxcount int,
	endorsecount int,
	isdocconnectuser bit,
	name_seo varchar(150)
 	)

	insert into @tempQuestionDetail
	select questions.questionid,questions.userid,users.gender,(select (firstname + ' ' + lastname) As patientlastname from users 
	where userid=questions.userid ), (select email from users where userid=questions.userid) AS patientemail, users.lastname,
	users.email,users.mobileno, questions.status,questions.createdate,questions.questiontext,answers.answerid,(users.photourl+ users.photopath),
	answers.title,answers.answertext,answers.image,answers.createdate as answerdate,answers.userid as Docid, users.firstname + ' ' + users.lastname as doctor,
	'','','','',users.isdocconnectuser,users.name_seo from questions
                              left outer join answers  on questions.questionid = answers.questionid
                              left outer join users on answers.userid = users.userid
                              where questions.questionid = @questionid and questions.status = @questionStatus 
	DECLARE @answerid int
	Declare @isuserthanked bit=0
	Declare @countThanx int
	Declare @countEndorse int	
	set @id=1
	select @count=count(*) from @tempQuestionDetail
	while(@count>0)
	BEGIN
		select  @answerid=answerid from @tempQuestionDetail where id=@id
		SELECT @countThanx = COUNT(*) from patientthanks where answerid = @answerid
		SELECT @countEndorse = COUNT(*) from doctoragrees where answerid = @answerid
		if exists(select * from patientthanks where answerid=@answerid and userid=@userid)
			BEGIN
				set @isuserthanked=1
				update @tempQuestionDetail set ispatientthank=@isuserthanked where id=@id
			END
		if exists(select * from doctoragrees where answerid=@answerid and doctorid=@userid)
			BEGIN
				set @isuserthanked=1
				update @tempQuestionDetail set isendorse = @isuserthanked where id=@id
			END
		update @tempQuestionDetail set thanxcount= @countThanx ,  endorsecount = @countEndorse where id=@id
		Set @count-=1
		set @id+=1;		
	END
	select *, '' as docconnectdoctorid from @tempQuestionDetail 
	select tagid from questiontags where questionid =@questionid
	select userid from answers where  questionid = @questionid and userid = @userid
end
else 
begin
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
		specialities varchar(max),
		cities varchar(max),
		locations varchar(max)
	)	
	
	declare @city varchar(max)
	declare @location varchar(max)
	
	set @id  = 1	
	declare @specialities varchar(max)
	declare @doccount int
	Declare @docuserid int
	Declare @doctorcount int
	Declare @doctorid int

	insert into @docList
		select users.userid,'Dr. ' + users.firstname + ' ' + users.lastname as name,'' ,
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

		insert into @doccities select distinct @docuserid,city.name from @docList LEFT OUTER JOIN doctorlocations
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
	delete from @docspeciality
	delete from @specialitytemp
	

	select questiontext from questions where questionid =@questionid
	select tagid from questiontags where questionid =@questionid
	select * from @docList
end
end





