/* Add foreign key constraints to table "GROUP_LEVEL_FUNCTION".                               */
alter table "GROUP_LEVEL_FUNCTION"
	add constraint "METHOD_PCT_CONTRIB_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
