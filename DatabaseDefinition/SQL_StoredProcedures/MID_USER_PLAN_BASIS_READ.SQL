--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads all basises for all plans for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_BASIS_READ]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		BASIS_RID AS BasisID,
		'Basis ' AS BasisName 
	FROM USER_PLAN_BASIS
	WHERE USER_RID = @USER_RID
	ORDER BY BASIS_RID
            
END
GO