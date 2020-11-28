--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads all basis details for all plans for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_BASIS_DETAILS_READ]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		BASIS_RID, 
		'', 
		HN_RID, 
		DESCRIPTION, 
		D.FV_RID, 
		'',
		CDR_RID, 
		'', 
		WEIGHT, 
		IS_INCLUDED_IND, 
		'' 
	FROM USER_PLAN_BASIS_DETAILS D, 
		 FORECAST_VERSION FV
	WHERE D.FV_RID = FV.FV_RID
		  AND USER_RID = @USER_RID
	ORDER BY BASIS_RID, SEQ_ID
            
END
GO