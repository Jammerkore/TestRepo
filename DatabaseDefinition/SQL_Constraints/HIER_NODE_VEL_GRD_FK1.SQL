/* Add foreign key constraints to table "VELOCITY_GRADE".                                     */
alter table "VELOCITY_GRADE"
	add constraint "HIER_NODE_VEL_GRD_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
