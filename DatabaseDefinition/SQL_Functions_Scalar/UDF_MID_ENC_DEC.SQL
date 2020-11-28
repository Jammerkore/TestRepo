CREATE FUNCTION [dbo].[UDF_MID_ENC_DEC]
(
	@Offset INT,
	@Text VARCHAR(8000)
)
RETURNS	VARCHAR(8000)
AS

BEGIN
    DECLARE	@ASCIIList VARCHAR(256)

	DECLARE	@Index SMALLINT,
		@i SMALLINT,
      	@Cipher VARCHAR(8000),
      	@c CHAR
      	
    set @Index = 0
    set @ASCIIList = ''
    WHILE @Index <= 255
	BEGIN
		set @ASCIIList =  @ASCIIList + convert(varchar,CHAR(@Index))
		SELECT	@Index = @Index + 1
	END

	SELECT	@Index = 1,
		@Cipher = ''
		
    if SUBSTRING(@Text, 0, 8) = '##MID##'
    BEGIN
       set @Index = 8
       WHILE @Index <= DATALENGTH(@Text)
	   BEGIN
         SELECT @i = CHARINDEX(SUBSTRING(@Text, @Index, 1), @ASCIIList);
         set @c = SUBSTRING(@ASCIIList, @i - @Offset, 1)
		 set @Cipher =  @Cipher + @c
			
		 SELECT	@Index = @Index  +1
       END
    END
    ELSE
    BEGIN
       set @Cipher = '##MID##'
	   WHILE @Index <= DATALENGTH(@Text)
	   BEGIN
         SELECT @i = CHARINDEX(SUBSTRING(@Text, @Index, 1), @ASCIIList);
         set @c = SUBSTRING(@ASCIIList, @i + @Offset, 1)
		 set @Cipher =  @Cipher + @c
			
		 SELECT	@Index = @Index  +1
       END
    END

	RETURN	@Cipher
END
go
