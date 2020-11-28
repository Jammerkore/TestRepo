/* Add foreign key constraints to table "METHOD_OVERRIDE_CAPACITY".                           */
alter table "METHOD_OVERRIDE_CAPACITY"
	add constraint "STR_GRP_LVL_MTHD_OVRD_CAP_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
