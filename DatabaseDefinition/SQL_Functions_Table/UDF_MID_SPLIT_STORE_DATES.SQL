CREATE FUNCTION [dbo].[UDF_MID_SPLIT_STORE_DATES]
(@list  text, @delim char(1) = ';')
   RETURNS @t TABLE (Store int, StartDate int, EndDate int)
 AS
 BEGIN
 DECLARE @slices TABLE (slice varchar(8000) NOT NULL)
 DECLARE @slice varchar(8000),
 @textpos int,
 @maxlen int,
 @stoppos int
 SELECT @textpos = 1, @maxlen = 7999
 WHILE datalength(@list) - (@textpos - 1) >= @maxlen
 BEGIN
 SELECT @slice = substring(@list, @textpos, @maxlen)
 SELECT @stoppos = @maxlen - charindex(@delim, reverse(@slice))
 INSERT @slices (slice) VALUES (@delim + left(@slice, @stoppos) + @delim)
 SELECT @textpos = @textpos - 1 + @stoppos + 1   -- On the other side of the delimeter.
 END
 INSERT @slices (slice)
 VALUES (@delim + substring(@list, @textpos, @maxlen) + @delim)
 declare @strings table (st varchar(30))
 INSERT @strings
 SELECT ltrim(rtrim(substring(s.slice, N.NUMBER + 1,
 charindex(@delim, s.slice, N.NUMBER + 1) - N.NUMBER - 1)))
 FROM  NUMBERS N
 JOIN  @slices s
 ON N.NUMBER <= len(s.slice) - 1
 AND substring(s.slice, N.NUMBER, 1) = @delim
 INSERT @t
 SELECT substring(st,1,charindex(',',st)-1), substring(st,len(st)-14,7), substring(st,len(st)-6,7)
 FROM @strings
 WHERE len(st) > 1
 RETURN
 END

GO
