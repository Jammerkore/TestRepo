alter table "ASSORTMENT_PROPERTIES_BASIS"
	add constraint "HIERARCHY_NODE_ASSORTMENT_PROP_BASIS_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
