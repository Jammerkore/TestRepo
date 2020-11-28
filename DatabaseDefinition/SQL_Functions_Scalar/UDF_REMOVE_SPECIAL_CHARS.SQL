--dv =============================================
--dv Create date:	9/28/2015
--dv Description:	Removes Special characters
--dv =============================================
CREATE FUNCTION [dbo].[UDF_REMOVE_SPECIAL_CHARS] 
(@S VARCHAR(256)
) 
RETURNS VARCHAR(256)
BEGIN
   if @S IS NULL
      RETURN NULL
   DECLARE @S2 varchar(256)
   SET @S2 = ''
   DECLARE @l int
   SET @l = len(@S)
   DECLARE @p int
   SET @p = 1
   while @p <= @l BEGIN
      DECLARE @c int
      SET @c = ascii(substring(@S, @p, 1))
      if @c BETWEEN 48 AND 57 OR @c BETWEEN 65 AND 90 OR @c BETWEEN 97 AND 122
         SET @S2 = @S2 + char(@c)
      SET @p = @p + 1
      END
   if len(@S2) = 0
      RETURN NULL
   RETURN @S2
   END
GO
