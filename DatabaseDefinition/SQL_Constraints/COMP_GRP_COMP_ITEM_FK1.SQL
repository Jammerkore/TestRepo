/* Add foreign key constraints to table "COMPUTATION_ITEM".                                   */
alter table "COMPUTATION_ITEM"
	add constraint "COMP_GRP_COMP_ITEM_FK1" foreign key (
		"CG_RID")
	 references "COMPUTATION_GROUP" (
		"CG_RID")  

go
