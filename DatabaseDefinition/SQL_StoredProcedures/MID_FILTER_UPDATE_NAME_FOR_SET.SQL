--dv =============================================
--dv Create date:	4/27/2015
--dv Description:	Updates the filter name and its corresponding name condition.
--dv History:		4/27/2015 TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_UPDATE_NAME_FOR_SET]
	@FILTER_RID int,
    @NEW_NAME varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FILTER
    SET 
		FILTER_NAME = @NEW_NAME
    WHERE FILTER_RID = @FILTER_RID

	DECLARE @FILTER_NAME_CONDITION_SEQ INT = (SELECT SEQ FROM FILTER_CONDITION WITH (NOLOCK) WHERE FILTER_RID=@FILTER_RID AND ELEMENT_GROUP_TYPE_INDEX=0)

	UPDATE FILTER_CONDITION
    SET 
		VALUE_TO_COMPARE = @NEW_NAME
    WHERE FILTER_RID = @FILTER_RID AND SEQ = @FILTER_NAME_CONDITION_SEQ



	SELECT @@ROWCOUNT

END
GO
