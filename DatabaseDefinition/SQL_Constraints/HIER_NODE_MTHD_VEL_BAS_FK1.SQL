alter table "METHOD_VELOCITY_BASIS"
	add constraint "HIER_NODE_MTHD_VEL_BAS_FK1" foreign key (
		"BASIS_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
