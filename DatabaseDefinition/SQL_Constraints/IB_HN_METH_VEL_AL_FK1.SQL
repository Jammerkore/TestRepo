/* BEGIN TT#1287 - AGallagher - Inventory Min/Max */
alter table "METHOD_OVERRIDE"
	add constraint "IB_HN_METH_VEL_AL_FK1" foreign key (
		"IB_MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  
go
