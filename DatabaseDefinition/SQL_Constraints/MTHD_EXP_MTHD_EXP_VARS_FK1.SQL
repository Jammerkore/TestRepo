/* Add foreign key constraints to table "METHOD_EXPORT_VARIABLES".                            */
alter table "METHOD_EXPORT_VARIABLES"
	add constraint "MTHD_EXP_MTHD_EXP_VARS_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_EXPORT" (
		"METHOD_RID")  

go
