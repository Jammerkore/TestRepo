CREATE FUNCTION [dbo].[UDF_MID_FLAGTOBIT]
(	@flags int,
	@flag tinyint )
returns bit
as

begin
if 	@flag = 0 return @flags & 0x0001
else if @flag = 1 return @flags & 0x0002
else if @flag = 2 return @flags & 0x0004
else if @flag = 3 return @flags & 0x0008
else if @flag = 4 return @flags & 0x0010
else if @flag = 5 return @flags & 0x0020
else if @flag = 6 return @flags & 0x0040
else if @flag = 7 return @flags & 0x0080
else if @flag = 8 return @flags & 0x0100
else if @flag = 9 return @flags & 0x0200
else if @flag = 10 return @flags & 0x0400
else if @flag = 11 return @flags & 0x0800
else if @flag = 12 return @flags & 0x01000
else if @flag = 13 return @flags & 0x02000
else if @flag = 14 return @flags & 0x04000
else if @flag = 15 return @flags & 0x08000
else if @flag = 16 return @flags & 0x10000
else if @flag = 17 return @flags & 0x20000
else if @flag = 18 return @flags & 0x40000
else if @flag = 19 return @flags & 0x80000

return '0'
end

GO
