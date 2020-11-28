--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads rows from the user allocation table based on the user rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_READ]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		USER_RID, 
		VIEW_TYPE, 
		SG_RID, 
		FILTER_RID, 
		GROUP_BY_ID,
		VIEW_RID, 
		INCLUDE_INELIGIBLE_STORES,
		BEGIN_CDR_RID, 
		END_CDR_RID, 
		HN_RID
	FROM USER_ALLOCATION
	WHERE USER_RID = @USER_RID

END
GO
