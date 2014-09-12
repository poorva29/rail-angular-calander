-- =============================================

-- Author: <pavan shevle>

-- Create date: 12/09/2014

-- Description: changed answertext column of answers table as 1000 char

-- =============================================

Alter table answers
Alter column [answertext] varchar(1000)