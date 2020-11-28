--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads rows from the user allocation basis table based on the user rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_BASIS_READ]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		BASIS_SEQUENCE AS BasisSequence,
		COALESCE(BASIS_HN_RID, -1) BASIS_HN_RID,
		COALESCE(BASIS_PH_RID, -1) BASIS_PH_RID,
		COALESCE(BASIS_PHL_SEQUENCE,0) BASIS_PHL_SEQUENCE,
		COALESCE(BASIS_FV_RID, -1) BASIS_FV_RID,
		CDR_RID, 
		SALES_WEIGHT
    FROM USER_ALLOCATION_BASIS
    WHERE USER_RID = @USER_RID
    ORDER BY BASIS_SEQUENCE ASC

END
GO
