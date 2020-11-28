--dv =============================================
--dv Create date: 4/5/2018
--dv Description:	Read Store Data
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Stores_Data_Read]
AS
BEGIN
SET NOCOUNT ON
DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX)

select @cols = STUFF((SELECT ',' + QUOTENAME(Characteristic) 
                    from Stores_Characteristics
                    group by [Characteristic]
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')


set @query = N'SELECT *
	           INTO #c
	           FROM 
	           (
	           SELECT [ID],' + @cols + N' from 
             (
                select [ID], [Value], [Characteristic]
                from Stores_Characteristics
            ) x
            pivot 
            (
                max([Value])
                for [Characteristic] in (' + @cols + N')
            ) p 
			) x
			
			SELECT sd.*, ' + @cols + ' 
			FROM Stores_Data sd
			JOIN #c c ON c.ID = sd.ID
			'

exec sp_executesql @query;

END
GO
