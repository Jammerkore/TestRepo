--dv =============================================
--dv Create date: 2/9/2018
--dv Description:	Write Store Characteristics
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Stores_Characteristics_Write]
	@dt Stores_Characteristics_Type READONLY
AS
BEGIN
SET NOCOUNT ON
MERGE INTO Stores_Characteristics with (ROWLOCK) AS TARGET
USING @dt AS SOURCE
on (SOURCE.[ID] = TARGET.[ID] and SOURCE.[Characteristic] = TARGET.[Characteristic])
WHEN MATCHED THEN
UPDATE 
         SET TARGET.[Value] = COALESCE(SOURCE.[Value], TARGET.[Value])
WHEN NOT MATCHED BY TARGET THEN
INSERT ([ID], [Characteristic], [Value])
VALUES ( SOURCE.[ID], SOURCE.[Characteristic], SOURCE.[Value]);

END
GO





















