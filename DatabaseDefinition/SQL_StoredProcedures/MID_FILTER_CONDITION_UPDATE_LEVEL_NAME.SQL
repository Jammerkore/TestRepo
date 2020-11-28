--dv =============================================
--dv Create date:	4/27/2015
--dv Description:	Inserts a filter condition and returns the new condition RID
--dv History:		4/27/2015 TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_CONDITION_UPDATE_LEVEL_NAME]
	@FILTER_RID int,
    @OLD_NAME varchar(200),
    @NEW_NAME varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FILTER_CONDITION
		SET VALUE_TO_COMPARE = @NEW_NAME
	WHERE ELEMENT_GROUP_TYPE_INDEX=42 AND FILTER_RID=@FILTER_RID AND VALUE_TO_COMPARE = @OLD_NAME
	
	SELECT @@ROWCOUNT

END
GO
