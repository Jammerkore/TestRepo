/* begin TT#1287 - Inventory Minimum and Maximum */
alter table "HEADER"
	add constraint "HIERARCHY_NODE_HEADER_FK4" foreign key (
		"GRADE_INVENTORY_HNRID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
