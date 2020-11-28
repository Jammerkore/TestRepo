alter table "METHOD_RULE"
	add constraint "STR_GRP_LVL_MTHD_RULE_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
