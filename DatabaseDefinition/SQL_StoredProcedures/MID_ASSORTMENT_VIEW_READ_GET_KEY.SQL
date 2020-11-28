--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Returns a Assortment View Detail
--dv History:		TT#1456-MD - stodd - When saving views in Assortment with same name as a Group Allocation View, the Group Allocation view is overlaid.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_VIEW_READ_GET_KEY]
	@USER_RID INT,
	@VIEW_ID varchar(250),
	@ASSORTMENT_VIEW_TYPE INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		*
    FROM ASSORTMENT_VIEW 
	WHERE USER_RID = @USER_RID
		AND VIEW_ID = @VIEW_ID
		AND ASSORTMENT_VIEW_TYPE = @ASSORTMENT_VIEW_TYPE

END
GO