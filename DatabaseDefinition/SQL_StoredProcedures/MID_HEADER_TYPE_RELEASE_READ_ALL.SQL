--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Reads all header release types
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_TYPE_RELEASE_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		hdr.HEADER_TYPE, 
		hdr.RELEASE_HEADER_TYPE_IND, 
		at.TEXT_VALUE
	FROM HEADER_TYPE_RELEASE hdr, 
		 APPLICATION_TEXT at
	WHERE hdr.HEADER_TYPE = at.TEXT_CODE

END
GO