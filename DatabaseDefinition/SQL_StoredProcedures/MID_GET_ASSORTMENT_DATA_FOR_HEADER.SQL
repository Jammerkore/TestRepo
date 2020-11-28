--dv Create date:	9/30/2013
--dv Description:	Gets assortment data for a header in one database call
--dv				Placeholder data, if needed, will be returned in separate data tables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GET_ASSORTMENT_DATA_FOR_HEADER] 
	@headerRID int,
	@placeholderRID int,
	@getAssortmentProperties bit,
	@getPlaceholderData bit
AS
BEGIN
	SET NOCOUNT ON;

	IF @getAssortmentProperties=1
		BEGIN
			SELECT * FROM [dbo].[UDF_GET_ASSORTMENT_PROPERTIES_FOR_HEADER](@headerRID);
		END
	ELSE
		BEGIN
			SELECT 0 WHERE 1=2;  --return empty result set
		END



	IF @getPlaceholderData=1
		BEGIN
		   SELECT * FROM [dbo].[UDF_GET_ASSORTMENT_PROPERTIES_FOR_HEADER](@placeholderRID);
		   SELECT HDR_RID FROM HEADER WHERE PLACEHOLDER_RID = @headerRID;
		END
	ELSE
		BEGIN
			SELECT 0 WHERE 1=2; --return empty result set
			SELECT 0 WHERE 1=2; --return empty result set
		END

END
GO
