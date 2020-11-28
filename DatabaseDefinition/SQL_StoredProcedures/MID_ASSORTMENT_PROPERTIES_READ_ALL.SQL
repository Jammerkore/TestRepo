--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads all assortment properties for headers
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_PROPERTIES_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eHeaderType_Assortment INT = 800739;

	SELECT *
    FROM ASSORTMENT_PROPERTIES ha, 
	HEADER hd
	WHERE hd.HDR_RID = ha.HDR_RID
	AND hd.DISPLAY_TYPE = @eHeaderType_Assortment

END
GO