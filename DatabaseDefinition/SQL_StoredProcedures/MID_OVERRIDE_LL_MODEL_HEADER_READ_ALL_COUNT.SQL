--dv =============================================
--dv Create date:	01/30/2014
--dv Description:	Returns a count of Header Override LL Models that match the name and user
--dv History:		09/04/2014 TT#1279-MD -jsobek - Save As changes ownership from global to user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT]
	@NAME varchar(250),
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) 'MyCount'
    FROM OVERRIDE_LL_MODEL_HEADER ollmh
		INNER JOIN USER_ITEM ui ON ollmh.OLL_RID = ui.ITEM_RID 
    WHERE ollmh.NAME = @NAME
		AND ui.OWNER_USER_RID = @USER_RID

END
GO