--dv =============================================
--dv Create date: 12/11/2013
--dv Description:	Delete a component criteria record.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPONENT_CRITERIA_DELETE]
	@COMP_CRIT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM COMPONENT_CRITERIA
	WHERE COMP_CRIT_RID = @COMP_CRIT_RID

	SELECT @@ROWCOUNT

END
GO























