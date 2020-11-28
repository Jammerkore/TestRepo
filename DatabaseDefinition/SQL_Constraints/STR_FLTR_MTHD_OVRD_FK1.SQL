--dv =============================================
--dv Modified date: 8/12/2014
--dv History:		TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
--dv =============================================
alter table "METHOD_OVERRIDE"
	add constraint "STR_FLTR_MTHD_OVRD_FK1" foreign key (
		"STORE_FILTER_RID")
	 references "FILTER" (
		"FILTER_RID")  
go
