/* Add foreign key constraints to table "METHOD_CHANGE_HISTORY".                              */
alter table "METHOD_CHANGE_HISTORY"
	add constraint "MTHD_MTHD_CHGE_HIS_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
