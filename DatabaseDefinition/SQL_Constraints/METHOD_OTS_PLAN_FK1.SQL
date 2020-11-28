/* Add foreign key constraints to table "OTS_PLAN".                                           */
alter table "OTS_PLAN"
	add constraint "METHOD_OTS_PLAN_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
