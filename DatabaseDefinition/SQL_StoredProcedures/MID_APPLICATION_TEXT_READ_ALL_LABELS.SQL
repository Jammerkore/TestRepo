--dv =============================================
--dv Create date:	1/16/2014
--dv Description:	Reads application text labels
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_ALL_LABELS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		TEXT_CODE, 
		TEXT_VALUE 
	FROM APPLICATION_TEXT 
    WHERE TEXT_CODE >= 900000

END
GO
