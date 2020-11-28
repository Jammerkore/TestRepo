--dv =============================================
--dv Create date: 3/20/2018
--dv Description:	Clears all extract dates so will be extracted again
--dv =============================================
CREATE PROCEDURE [dbo].[MID_EXTRACT_PLANNING_CONTROL_CLEAR_EXTRACT_DATES]
AS
BEGIN
SET NOCOUNT ON

UPDATE EXTRACT_PLANNING_CONTROL
   SET EXTRACT_DATE = null
 
END
GO


















