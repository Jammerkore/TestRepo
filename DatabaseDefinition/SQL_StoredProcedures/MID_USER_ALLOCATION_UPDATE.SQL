--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Updates the user allocation table for the user rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_UPDATE]
	@USER_RID int,
	@VIEW_TYPE int,
	@SG_RID int,
	@FILTER_RID int,
	@GROUP_BY_ID int,
	@VIEW_RID int,
	@INCLUDE_INELIGIBLE_STORES char,
	@BEGIN_CDR_RID int,
	@END_CDR_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_ALLOCATION
	SET 
		VIEW_TYPE = @VIEW_TYPE,
		SG_RID =  @SG_RID,
		FILTER_RID = @FILTER_RID,
		GROUP_BY_ID = @GROUP_BY_ID,
		VIEW_RID = @VIEW_RID,
		INCLUDE_INELIGIBLE_STORES = @INCLUDE_INELIGIBLE_STORES,
		BEGIN_CDR_RID = @BEGIN_CDR_RID,
		END_CDR_RID = @END_CDR_RID,
		HN_RID = @HN_RID
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;

END
GO
