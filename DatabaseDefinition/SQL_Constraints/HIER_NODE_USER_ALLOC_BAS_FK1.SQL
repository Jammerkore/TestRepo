alter table "USER_ALLOCATION_BASIS"
	add constraint "HIER_NODE_USER_ALLOC_BAS_FK1" foreign key (
		"BASIS_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  
go
