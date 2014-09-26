--========================================================
--<Auther>: Pavan Shevle
--<Date>: 26/09/2014
--<Description>: Hospital name field is added
--========================================================
ALTER PROCEDURE [dbo].[askmirai_get_alldoctorsdetails]
@DOCID int=-1,
@doctorStatus int =-1

AS

DECLARE @doctors TABLE 
(
    doctorId int
)
DECLARE @strQuery Varchar(1000)=''

SET NOCOUNT ON
IF @DOCID = -1
BEGIN
   SET @strQuery= 'SELECT userid from users WHERE usertype=1 And status not in (0)'
                              
    IF(@doctorStatus !=-1)
      BEGIN
	 
        SET @strQuery= @strQuery + ' And status = '+CONVERT(varchar(10),@doctorStatus) + '  
		                            
		                            order by firstname'
      END
	
    INSERT INTO @doctors   EXECUTE (@strQuery)
	

	SELECT * FROM users u INNER JOIN @doctors as a  ON u.userid =a.doctorId

	SELECT doctorspecialities.doctorid as userid, doctorspecialities.specialityid, specialities.name as speciality_name
	FROM  doctorspecialities INNER JOIN
				   specialities ON doctorspecialities.specialityid = specialities.specialityid  INNER JOIN @doctors as a  ON doctorspecialities.doctorid =a.doctorId
	 
	SELECT doctorlocations.doctorid as userid, doctorlocations.doctorlocationid, doctorlocations.countryid, country.name As country_name, doctorlocations.stateid, state.name AS state_name, doctorlocations.cityid, 
				   city.name AS city_name, doctorlocations.locationid, location.name AS location_name, doctorlocations.clinicname, doctorlocations.hospitalname, doctorlocations.address, doctorlocations.telephone
				   
	FROM  doctorlocations INNER JOIN
				   country ON doctorlocations.countryid = country.countryid INNER JOIN
				   state ON doctorlocations.stateid = state.stateid INNER JOIN
				   city ON doctorlocations.cityid = city.cityid INNER JOIN
				   location ON doctorlocations.locationid = location.locationid 
				    INNER JOIN @doctors as a  ON doctorlocations.doctorid =a.doctorId
	  

				                
	SELECT doctorqualification.doctorid as userid, doctorqualification.degreeid, degree.name as degree_name, doctorqualification.university
	FROM  doctorqualification INNER JOIN
				   degree ON doctorqualification.degreeid = degree.degreeid  INNER JOIN @doctors as a  ON doctorqualification.doctorid =a.doctorId
				   
SELECT doctorsdetails.doctordetailsid as docdetailsid, doctorsdetails.doctorid as userid, doctorsdetails.certification, doctorsdetails.society
	 from doctorsdetails  INNER JOIN 
					users on users.userid = doctorsdetails.doctorid 
					INNER JOIN @doctors as a  ON [dbo].[doctorsdetails].doctorid =a.doctorId
               
END
ElSE
BEGIN
	SELECT * FROM users where userid = @DOCID

	SELECT doctorspecialities.doctorid as userid, doctorspecialities.specialityid, specialities.name as speciality_name
	FROM  doctorspecialities INNER JOIN
				   specialities ON doctorspecialities.specialityid = specialities.specialityid where doctorspecialities.doctorid = @DOCID
	 
	SELECT doctorlocations.doctorid as userid, doctorlocations.doctorlocationid, doctorlocations.countryid, country.name As country_name, doctorlocations.stateid, state.name AS state_name, doctorlocations.cityid, 
				   city.name AS city_name, doctorlocations.locationid, location.name AS location_name, doctorlocations.clinicname, doctorlocations.hospitalname, doctorlocations.address, doctorlocations.telephone				  
	FROM  doctorlocations INNER JOIN
				   country ON doctorlocations.countryid = country.countryid INNER JOIN
				   state ON doctorlocations.stateid = state.stateid INNER JOIN
				   city ON doctorlocations.cityid = city.cityid INNER JOIN
				   location ON doctorlocations.locationid = location.locationid 
				   where doctorlocations.doctorid = @DOCID
	               
	SELECT doctorqualification.doctorid as userid, doctorqualification.degreeid, degree.name as degree_name, doctorqualification.university
	FROM  doctorqualification INNER JOIN
				   degree ON doctorqualification.degreeid = degree.degreeid where doctorqualification.doctorid = @DOCID

	SELECT doctorsdetails.doctordetailsid as docdetailsid, doctorsdetails.doctorid as userid, doctorsdetails.certification, doctorsdetails.society
	 from doctorsdetails  INNER JOIN 
					users on users.userid = doctorsdetails.doctorid where users.userid = @DOCID
END



