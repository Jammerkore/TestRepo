--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row into the hierarchy join table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_NODE_JOIN_INSERT]
	@PH_RID int,
	@PARENT_HN_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO HIER_NODE_JOIN(PH_RID, PARENT_HN_RID, HN_RID) 
	VALUES (@PH_RID, @PARENT_HN_RID, @HN_RID)

	SELECT @@ROWCOUNT;

END
GO
