--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Inserts a grid view detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_DETAIL_INSERT] 
	@VIEW_RID int,
	@BAND_KEY varchar(50), 
	@COLUMN_KEY varchar(50), 
	@VISIBLE_POSITION int,
    @IS_HIDDEN char(1), 
	@IS_GROUPBY_COL char(1), 
	@SORT_DIRECTION int, 
	@SORT_SEQUENCE int, 
	@WIDTH int,
    @COLUMN_TYPE varchar(1), 
	@HCG_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO GRID_VIEW_DETAIL 
		(
		VIEW_RID, 
		BAND_KEY, 
		COLUMN_KEY,
        VISIBLE_POSITION, 
		IS_HIDDEN, 
		IS_GROUPBY_COL,
        SORT_DIRECTION, 
		SORT_SEQUENCE, 
		WIDTH, 
		COLUMN_TYPE, 
		HCG_RID
		)
	VALUES 
		(
		@VIEW_RID, 
		@BAND_KEY, 
		@COLUMN_KEY, 
		@VISIBLE_POSITION,
        @IS_HIDDEN, 
		@IS_GROUPBY_COL, 
		@SORT_DIRECTION, 
		@SORT_SEQUENCE, 
		@WIDTH,
        @COLUMN_TYPE, 
		@HCG_RID
		)

	SELECT @@ROWCOUNT;

END
GO