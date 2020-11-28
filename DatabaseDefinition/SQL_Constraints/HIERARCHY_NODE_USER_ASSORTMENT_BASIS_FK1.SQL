alter table "USER_ASSORTMENT_BASIS"
	add constraint "HIERARCHY_NODE_USER_ASSORTMENT_BASIS_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
