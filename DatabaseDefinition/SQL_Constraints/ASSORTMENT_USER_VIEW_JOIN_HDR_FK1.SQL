alter table "ASSORTMENT_USER_VIEW_JOIN"
	add constraint "ASSORTMENT_USER_VIEW_JOIN_HDR_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")   

go