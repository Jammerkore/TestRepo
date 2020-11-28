--dv =============================================
--dv Create date: 4/30/2018
--dv Description:	Determines if the database contains the proper table
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Database_Exists]
AS
BEGIN
SET NOCOUNT ON

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Extract_Session'))
BEGIN
    SELECT 1 AS [MyCount]
END
ELSE
BEGIN
    SELECT 0 AS [MyCount]
END

END
GO





















