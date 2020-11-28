/* Add foreign key constraints to table "DEFAULT_OTS_PLAN_METHOD".                            */
alter table "DEFAULT_OTS_PLAN_METHOD"
	add constraint "OTS_PLAN_METHOD_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
