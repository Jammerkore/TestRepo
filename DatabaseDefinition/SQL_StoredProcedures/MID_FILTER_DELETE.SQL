--dv =============================================
--dv Create date:	6/25/2014
--dv Description:	Deletes a filter
--dv History:		TT#1677-MD -RMatelic -Need to clean FilterRID's out of the USER_ALLOCATION  & USER_PLAN tables after deleting Store Filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_DELETE]
   	@FILTER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_ALLOCATION WITH (ROWLOCK)
	SET FILTER_RID = NULL
	WHERE FILTER_RID = @FILTER_RID
	
	UPDATE USER_PLAN WITH (ROWLOCK)
	SET FILTER_RID = -1
	WHERE FILTER_RID = @FILTER_RID
	
	DELETE
	FROM FILTER
	WHERE FILTER_RID=@FILTER_RID

	SELECT @@ROWCOUNT

END
GO
