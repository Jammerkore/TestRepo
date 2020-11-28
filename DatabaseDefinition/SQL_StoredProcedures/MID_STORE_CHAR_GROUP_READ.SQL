--dv =============================================
--dv Create date: 5/11/2015
--dv Description: Get store char group info from the scg rid.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_GROUP_READ]
	@SCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SCG_RID,
		SCG_ID,
		SCG_TYPE,
		SCG_LIST_IND
	FROM STORE_CHAR_GROUP WITH (NOLOCK)
	WHERE SCG_RID = @SCG_RID

END
GO
