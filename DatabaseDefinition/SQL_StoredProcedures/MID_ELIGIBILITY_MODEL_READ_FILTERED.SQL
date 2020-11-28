--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered eligibility models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ELIGIBILITY_MODEL_READ_FILTERED]
	@EM_ID_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		EM_RID as MODEL_RID, 
		EM_ID as MODEL_NAME 
	FROM ELIGIBILITY_MODEL
    WHERE EM_ID LIKE @EM_ID_FILTER
	ORDER BY EM_ID

END
GO