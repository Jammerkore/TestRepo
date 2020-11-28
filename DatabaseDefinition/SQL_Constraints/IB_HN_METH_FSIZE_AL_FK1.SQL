/* BEGIN TT#41-MD - AGallagher - Size Inventory Min/Max */
alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "IB_HN_METH_FSIZE_AL_FK1" foreign key (
		"IB_MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  
go
