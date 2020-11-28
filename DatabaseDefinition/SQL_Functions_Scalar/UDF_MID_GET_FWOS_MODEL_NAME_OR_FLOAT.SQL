--dv =============================================
--dv Create date:	9/24/2014
--dv Description:	Returns either the FWOS Max Model Name or the FWOS Max float value based on an indicator field
--dv History:		TT#4041 -Node Properties Override report doesn't show decimals or FWOS Max Models
--dv =============================================
CREATE FUNCTION [dbo].[UDF_MID_GET_FWOS_MODEL_NAME_OR_FLOAT]
 (
 @IMO_FWOS_MAX_TYPE int, --the indicator type field
 @IMO_FWOS_MAX_RID int, --the rid to the FWOS Max Model
 @IMO_FWOS_MAX float -- the float value for the FWOS Max
 )
 RETURNS varchar(50)
 AS
 BEGIN
 
	DECLARE @returnText varchar(50)
 

	IF @IMO_FWOS_MAX_TYPE = 1 --Use the float value
		BEGIN
			SET @returnText = CAST(COALESCE(@IMO_FWOS_MAX, 0) as varchar(50));
		END
	ELSE IF @IMO_FWOS_MAX_TYPE = 2 --Go get the name of the model
		BEGIN
			SELECT @returnText = FWOSMAX_ID FROM FWOS_MAX_MODEL fwos WHERE fwos.FWOSMAX_RID=@IMO_FWOS_MAX_RID;
		END
	ELSE
		BEGIN
			SET @returnText = '';
		END

	RETURN @returnText
 END
GO