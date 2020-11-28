alter table "DETAIL_ALLOCATION"
	add constraint "RCT_SUBTOTAL_KNOWN_DIST_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
