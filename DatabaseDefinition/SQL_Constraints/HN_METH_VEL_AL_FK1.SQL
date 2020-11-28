/* BEGIN TT#1287 - AGallagher - Inventory Min/Max */
alter table "METHOD_VELOCITY"
	add constraint "HN_METH_VEL_AL_FK1" foreign key (
		"MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  
go
