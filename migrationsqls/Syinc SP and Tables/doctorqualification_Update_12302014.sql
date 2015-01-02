/****** Object:  StoredProcedure [dbo].[doctorqualification_Update]    Script Date: 12/30/2014 6:16:33 PM ******/
DROP PROCEDURE [dbo].[doctorqualification_Update]
GO

/****** Object:  StoredProcedure [dbo].[doctorqualification_Update]    Script Date: 12/30/2014 6:16:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[doctorqualification_Update]
	@uid int
,	@doctorid int
,	@selecteddegreeid int
,	@lastdegreeiD int
,	@University varchar(100)
,   @otherdegree varchar(100)
AS
DECLARE @duplicate as BIT
SET @duplicate = 0
IF(@lastdegreeiD is NULL or @lastdegreeiD <> @selecteddegreeid)
begin
	IF(exists(SELECT * from doctorqualification where degreeid =@selecteddegreeid and doctorid = @doctorid))
		BEGIN
			 SET @duplicate = 1
		END
	ELSE
		BEGIN	
			SET @duplicate = 0
		END	
end
IF(@lastdegreeiD is NULL and @duplicate = 0) -------not removed here
		BEGIN
			UPDATE doctorqualification SET degreeid = @selecteddegreeid,university=@University,otherdegree =  @otherdegree,modified_by = @uid,modified_on = GETDATE()
			WHERE doctorid = @doctorid and degreeid = @lastdegreeiD
		END
IF(@lastdegreeiD = @selecteddegreeid)
		BEGIN
			UPDATE doctorqualification SET degreeid = @selecteddegreeid,university=@University,otherdegree =  @otherdegree,modified_by = @uid,modified_on = GETDATE()
			WHERE doctorid = @doctorid and degreeid = @lastdegreeiD
		END
IF(@lastdegreeiD is Null)
		BEGIN
			INSERT INTO doctorqualification(doctorid,degreeid,university,otherdegree,created_by,created_on,modified_by,modified_on)
			VALUES(@doctorid,@selecteddegreeid,@university,@otherdegree,@uid,GETDATE(),@uid,GETDATE())
		End
IF(@lastdegreeiD is not NULL and @duplicate = 0)
		BEGIN
			UPDATE doctorqualification SET degreeid = @selecteddegreeid,university=@University,modified_by = @uid,modified_on = GETDATE() 
			WHERE doctorid = @doctorid and degreeid = @lastdegreeiD
		END
SELECT @duplicate



GO


