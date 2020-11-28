/* Add foreign key constraints to table "FILTER_BASIS_SIZE_ALLOCATION".                       */
--dv =============================================
--dv Modified date: 8/12/2014
--dv History:		TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
--dv =============================================
alter table "FILTER_BASIS_SIZE_ALLOCATION"
	add constraint "FILTER_BASIS_SIZE_AL_FK1" foreign key (
		"STORE_FILTER_RID")
	 references "FILTER" (
		"FILTER_RID")  
go
