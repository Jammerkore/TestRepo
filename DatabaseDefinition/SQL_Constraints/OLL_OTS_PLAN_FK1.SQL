alter table "OTS_PLAN"
	add constraint "OLL_OTS_PLAN_FK1" foreign key ("OLL_RID")
	 references "OVERRIDE_LL_MODEL_HEADER" ("OLL_RID") on update no action on delete no action  	 
go
