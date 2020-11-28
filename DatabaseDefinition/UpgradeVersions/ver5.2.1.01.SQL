/*RELEASE 5.2.1.1 10-10-2013 -TT#848-MD -jsobek -Fill to Size Plan Presentation */
IF .dbo.UDF_ColumnAlreadyExists('HEADER_BULK_COLOR_SIZE_NEED', 'FILL_SIZES_TO_TYPE')=0
BEGIN
	alter table HEADER_BULK_COLOR_SIZE_NEED add FILL_SIZES_TO_TYPE int  null
END
go
