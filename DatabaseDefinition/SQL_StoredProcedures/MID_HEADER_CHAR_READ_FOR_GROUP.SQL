--dv =============================================
--dv Create date: 6/2/2014
--dv Description: Reads all possible header characteristic values from a header characteristic group
--dv Example:  Header characteristic group=Fiber
--dv		   Header characteristic values=Cotton, Wool
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_READ_FOR_GROUP]
	@HCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HEADER_CHAR_GROUP_VALUE_TYPE INT;
	SELECT @HEADER_CHAR_GROUP_VALUE_TYPE=HCG_TYPE FROM HEADER_CHAR_GROUP WHERE HCG_RID=@HCG_RID

	SELECT 

		CASE 		
			WHEN @HEADER_CHAR_GROUP_VALUE_TYPE=1 THEN CONVERT(varchar(50), HC.DATE_VALUE, 101)
			WHEN @HEADER_CHAR_GROUP_VALUE_TYPE=2 THEN CONVERT(varchar(50), HC.NUMBER_VALUE)
			WHEN @HEADER_CHAR_GROUP_VALUE_TYPE=3 THEN CONVERT(varchar(50), HC.DOLLAR_VALUE)
			ELSE HC.TEXT_VALUE
		END AS CHAR_VALUE,
	    HC.HC_RID
	FROM HEADER_CHAR HC WITH (NOLOCK)
	WHERE HC.HCG_RID=@HCG_RID
	ORDER BY CHAR_VALUE

END

GO


