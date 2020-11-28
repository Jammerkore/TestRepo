--dv =============================================
--dv Create date: 3/24/2015
--dv Description:	Returns a count of rows that match the proposed date value
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_READ_DATE_VALUE_FOR_DUPLICATE]
	@PROPOSED_VALUE DATETIME,
	@SCG_RID INT,
	@SC_EDIT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SC_RID
	FROM STORE_CHAR
	WHERE DATE_VALUE=@PROPOSED_VALUE
		AND SCG_RID = @SCG_RID
		AND SC_RID <> @SC_EDIT_RID
	

END
GO