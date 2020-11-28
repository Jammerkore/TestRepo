/* Add foreign key constraints to table "OVERRIDE_LL_MODEL_HEADER".                          */
alter table "OVERRIDE_LL_MODEL_HEADER"
	add constraint "OVRDE_LL_HDR_HN_RID_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  
go
