--dv =============================================
--dv Create date: 3/24/2015
--dv Description:	Returns a count of rows that match the proposed text value
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_READ_TEXT_VALUE_FOR_DUPLICATE]
	@PROPOSED_VALUE VARCHAR(50),
	@SCG_RID INT,
	@SC_EDIT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SC_RID
	FROM STORE_CHAR
	WHERE TEXT_VALUE=@PROPOSED_VALUE
		AND SCG_RID = @SCG_RID
		AND SC_RID <> @SC_EDIT_RID
	

END
GO