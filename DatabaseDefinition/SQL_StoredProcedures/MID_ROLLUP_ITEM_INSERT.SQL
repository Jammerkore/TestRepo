--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Inserts a rollup item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_INSERT]
	@PROCESS int,
	@HN_RID int,
	@PH_RID int,
	@HOME_LEVEL int,
	@TIME_ID int,
	@ITEM_TYPE int,
	@FV_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT ROLLUP_ITEM 
		(
		PROCESS, 
		HN_RID, 
		PH_RID, 
		HOME_LEVEL, 
		TIME_ID, 
		ITEM_TYPE, 
		FV_RID
		)
	VALUES 
		(
		@PROCESS, 
		@HN_RID, 
		@PH_RID, 
		@HOME_LEVEL, 
		@TIME_ID, 
		@ITEM_TYPE, 
		@FV_RID 
		)

	SELECT @@ROWCOUNT;
END
GO