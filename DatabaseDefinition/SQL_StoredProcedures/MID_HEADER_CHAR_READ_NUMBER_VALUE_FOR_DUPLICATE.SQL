--dv =============================================
--dv Create date: 3/24/2015
--dv Description:	Returns a count of rows that match the proposed number value
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_READ_NUMBER_VALUE_FOR_DUPLICATE]
	@PROPOSED_VALUE FLOAT,
	@HCG_RID INT,
	@HC_EDIT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT HC_RID
	FROM HEADER_CHAR
	WHERE NUMBER_VALUE=@PROPOSED_VALUE
		AND HCG_RID = @HCG_RID
		AND HC_RID <> @HC_EDIT_RID
	

END
GO