--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Reads the color codes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		COLOR_CODE_RID, 
		COLOR_CODE_ID,
        COLOR_CODE_NAME, 
		COALESCE(COLOR_CODE_GROUP, ' ') COLOR_CODE_GROUP, 
		COALESCE(VIRTUAL_IND, '0') VIRTUAL_IND,
        COALESCE(PURPOSE, '0') PURPOSE
	FROM COLOR_CODE

END
GO
