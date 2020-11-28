alter table "METHOD_GENERAL_ALLOCATION"
	add constraint "HN_METH_GEN_AL_FK1" foreign key (
		"MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
