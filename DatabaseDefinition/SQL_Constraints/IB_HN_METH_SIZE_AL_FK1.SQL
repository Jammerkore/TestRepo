alter table "METHOD_SIZE_NEED"
	add constraint "IB_HN_METH_SIZE_AL_FK1" foreign key (
		"IB_MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  
go
