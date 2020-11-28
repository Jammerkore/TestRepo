--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads a Velocity Basis Method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_BASIS_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mvb.BASIS_SEQUENCE as 'BasisSequence',
		COALESCE(mvb.BASIS_HN_RID, -1) as 'BasisHNRID',
		COALESCE(mvb.BASIS_PH_RID, -1) as 'BasisPHRID',
		COALESCE(mvb.BASIS_PHL_SEQUENCE, 0) as 'BasisPHLSequence',
		COALESCE(mvb.BASIS_FV_RID, -1) as 'PeriodSequence',
		mvb.CDR_RID as 'cdrRID',
		mvb.SALES_WEIGHT as 'Weight'
    FROM METHOD_VELOCITY_BASIS mvb
    WHERE METHOD_RID = @METHOD_RID
	ORDER BY 
		BASIS_SEQUENCE ASC
END
GO