/*RELEASE 5.4.0.54 2-23-2015 TT#4294 -stodd - avg units enhancement */
/* This had to be done because, although these numbers have not been used before, the */
/* numbers existed on the DB. */

delete from APPLICATION_TEXT where TEXT_CODE = 804638
delete from APPLICATION_TEXT where TEXT_CODE = 804659
go

exec SP_MID_SET_TEXT 804638,'# Stores Alloc',30,77,null,null,0
exec SP_MID_SET_TEXT 804659,'# Stores Alloc',30,80,null,null,0
GO
